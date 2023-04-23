using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
#if !NETCOREAPP
using System.Runtime.Remoting.Messaging;
#endif
using System.Text;
using System.Threading.Tasks;

namespace Mono.Debugger.Soft
{
	public class LaunchOptions {
		public string AgentArgs {
			get; set;
		}

		public bool Valgrind {
			get; set;
		}
		
		public ProcessLauncher CustomProcessLauncher {
			get; set;
		}

		public TargetProcessLauncher CustomTargetProcessLauncher {
			get; set;
		}

		public delegate Process ProcessLauncher (ProcessStartInfo info);
		public delegate ITargetProcess TargetProcessLauncher (ProcessStartInfo info);
	}

	public class VirtualMachineManager
	{
		private delegate VirtualMachine LaunchCallback (ITargetProcess p, ProcessStartInfo info, Socket socket);
		private delegate VirtualMachine ListenCallback (Socket dbg_sock, Socket con_sock); 
		private delegate VirtualMachine ConnectCallback (Socket dbg_sock, Socket con_sock, IPEndPoint dbg_ep, IPEndPoint con_ep); 

		internal VirtualMachineManager () {
		}

		public static VirtualMachine LaunchInternal (Process p, ProcessStartInfo info, Socket socket)
		{
			return LaunchInternal (new ProcessWrapper (p), info, socket);
		}
			
		public static VirtualMachine LaunchInternal (ITargetProcess p, ProcessStartInfo info, Socket socket) {
			return LaunchInternalAsync (p, info, socket).Result;
		}

		public static async Task<VirtualMachine> LaunchInternalAsync (ITargetProcess p, ProcessStartInfo info, Socket socket)
		{
			Socket accepted = null;
			try {
				accepted = await socket.AcceptAsync ().ConfigureAwait(false);
			} catch (Exception) {
				throw;
			}

			Connection conn = new TcpConnection (accepted);

			VirtualMachine vm = new VirtualMachine (p, conn);

			if (info.RedirectStandardOutput)
				vm.StandardOutput = p.StandardOutput;

			if (info.RedirectStandardError)
				vm.StandardError = p.StandardError;

			conn.EventHandler = new EventHandler (vm);

			vm.connect ();

			return vm;
		}

		public static IAsyncResult BeginLaunch (ProcessStartInfo info, AsyncCallback callback)
		{
			return BeginLaunch (info, callback, null);
		}

		public static IAsyncResult BeginLaunch (ProcessStartInfo info, AsyncCallback callback, LaunchOptions options)
		{
			if (info == null)
				throw new ArgumentNullException ("info");

			Socket socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			socket.Bind (new IPEndPoint (IPAddress.Loopback, 0));
			socket.Listen (1000);
			IPEndPoint ep = (IPEndPoint) socket.LocalEndPoint;

			// We need to inject our arguments into the psi
			info.Arguments = string.Format ("{0} --debug --debugger-agent=transport=dt_socket,address={1}:{2}{3} {4}", 
								options == null || !options.Valgrind ? "" : info.FileName,
								ep.Address,
								ep.Port,
								options == null || options.AgentArgs == null ? "" : "," + options.AgentArgs,
								info.Arguments);

			if (options != null && options.Valgrind)
				info.FileName = "valgrind";
			info.UseShellExecute = false;

			if (info.RedirectStandardError)
				info.StandardErrorEncoding = Encoding.UTF8;

			if (info.RedirectStandardOutput)
				info.StandardOutputEncoding = Encoding.UTF8;

			ITargetProcess p;
			if (options != null && options.CustomProcessLauncher != null)
				p = new ProcessWrapper (options.CustomProcessLauncher (info));
			else if (options != null && options.CustomTargetProcessLauncher != null)
				p = options.CustomTargetProcessLauncher (info);
			else
				p = new ProcessWrapper (Process.Start (info));
			
			p.Exited += delegate (object sender, EventArgs eargs) {
				socket.Close ();
			};

			var listenTask = LaunchInternalAsync (p, info, socket);
			listenTask.ContinueWith (t => callback (listenTask));
			return listenTask;
		}

		public static VirtualMachine EndLaunch (IAsyncResult asyncResult) {
			if (asyncResult == null)
				throw new ArgumentNullException ("asyncResult");

			var listenTask = (Task<VirtualMachine>)asyncResult;
			return listenTask.Result;
		}

		public static VirtualMachine Launch (ProcessStartInfo info)
		{
			return Launch (info, null);
		}

		public static VirtualMachine Launch (ProcessStartInfo info, LaunchOptions options)
		{
			return EndLaunch (BeginLaunch (info, null, options));
		}

		public static VirtualMachine Launch (string[] args)
		{
			return Launch (args, null);
		}

		public static VirtualMachine Launch (string[] args, LaunchOptions options)
		{
			ProcessStartInfo pi = new ProcessStartInfo ("mono");
			pi.Arguments = String.Join (" ", args);

			return Launch (pi, options);
		}
			
		public static VirtualMachine ListenInternal (Socket dbg_sock, Socket con_sock)
		{
			return ListenInternalAsync (dbg_sock, con_sock).Result;
		}

		static async Task<VirtualMachine> ListenInternalAsync (Socket dbg_sock, Socket con_sock)
		{
			Socket con_acc = null;
			Socket dbg_acc = null;

			if (con_sock != null) {
				try {
					con_acc = await con_sock.AcceptAsync ().ConfigureAwait (false);
				} catch (Exception) {
					try {
						dbg_sock.Close ();
					} catch { }
					throw;
				}
			}

			try {
				dbg_acc = await dbg_sock.AcceptAsync ().ConfigureAwait (false);
			} catch (Exception) {
				if (con_sock != null) {
					try {
						con_sock.Close ();
						con_acc.Close ();
					} catch { }
				}
				throw;
			}

			if (con_sock != null) {
				if (con_sock.Connected)
					con_sock.Disconnect (false);
				con_sock.Close ();
			}

			if (dbg_sock.Connected)
				dbg_sock.Disconnect (false);
			dbg_sock.Close ();

			Connection transport = new TcpConnection (dbg_acc);
			StreamReader console = con_acc != null ? new StreamReader (new NetworkStream (con_acc)) : null;

			return Connect (transport, console, null);
		}

		public static IAsyncResult BeginListen (IPEndPoint dbg_ep, AsyncCallback callback) {
			return BeginListen (dbg_ep, null, callback);
		}
		
		public static IAsyncResult BeginListen (IPEndPoint dbg_ep, IPEndPoint con_ep, AsyncCallback callback)
		{
			int dbg_port, con_port;
			return BeginListen (dbg_ep, con_ep, callback, out dbg_port, out con_port);
		}

		public static IAsyncResult BeginListen (IPEndPoint dbg_ep, IPEndPoint con_ep, AsyncCallback callback,
			out int dbg_port, out int con_port)
		{
			dbg_port = con_port = 0;
			
			Socket dbg_sock = null;
			Socket con_sock = null;

			dbg_sock = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			dbg_sock.Bind (dbg_ep);
			dbg_sock.Listen (1000);
			dbg_port = ((IPEndPoint) dbg_sock.LocalEndPoint).Port;

			if (con_ep != null) {
				con_sock = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				con_sock.Bind (con_ep);
				con_sock.Listen (1000);
				con_port = ((IPEndPoint) con_sock.LocalEndPoint).Port;
			}

			var listenTask = ListenInternalAsync (dbg_sock, con_sock);
			listenTask.ContinueWith (t => callback (listenTask));
			return listenTask;
		}

		public static VirtualMachine EndListen (IAsyncResult asyncResult) {
			if (asyncResult == null)
				throw new ArgumentNullException ("asyncResult");

			var listenTask = (Task<VirtualMachine>)asyncResult;
			return listenTask.Result;
		}

		public static VirtualMachine Listen (IPEndPoint dbg_ep)
		{
			return Listen (dbg_ep, null);
		}

		public static VirtualMachine Listen (IPEndPoint dbg_ep, IPEndPoint con_ep)
		{
			return EndListen (BeginListen (dbg_ep, con_ep, null));
		}

		/*
		 * Connect to a virtual machine listening at the specified address.
		 */
		public static VirtualMachine Connect (IPEndPoint endpoint) {
			return Connect (endpoint, null);
		}

		public static VirtualMachine Connect (IPEndPoint endpoint, IPEndPoint consoleEndpoint) { 
			if (endpoint == null)
				throw new ArgumentNullException ("endpoint");

			return EndConnect (BeginConnect (endpoint, consoleEndpoint, null));
		}

		public static VirtualMachine ConnectInternal (Socket dbg_sock, Socket con_sock, IPEndPoint dbg_ep, IPEndPoint con_ep)
		{
			return ConnectInternalAsync (dbg_sock, con_sock, dbg_ep, con_ep).Result;
		}

		public static async Task<VirtualMachine> ConnectInternalAsync (Socket dbg_sock, Socket con_sock, IPEndPoint dbg_ep, IPEndPoint con_ep)
		{
			if (con_sock != null) {
				try {
					await con_sock.ConnectAsync(con_ep).ConfigureAwait (false);
				} catch (Exception) {
					try {
						dbg_sock.Close ();
					} catch { }
					throw;
				}
			}

			try {
				await dbg_sock.ConnectAsync(dbg_ep).ConfigureAwait(false);
			} catch (Exception) {
				if (con_sock != null) {
					try {
						con_sock.Close ();
					} catch { }
				}
				throw;
			}

			Connection transport = new TcpConnection (dbg_sock);
			StreamReader console = con_sock != null ? new StreamReader (new NetworkStream (con_sock)) : null;

			return Connect (transport, console, null);
		}

		public static IAsyncResult BeginConnect (IPEndPoint dbg_ep, AsyncCallback callback) {
			return BeginConnect (dbg_ep, null, callback);
		}

		public static IAsyncResult BeginConnect (IPEndPoint dbg_ep, IPEndPoint con_ep, AsyncCallback callback) {
			Socket dbg_sock = null;
			Socket con_sock = null;

			dbg_sock = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			if (con_ep != null) {
				con_sock = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			}

			var connectTask = ConnectInternalAsync (dbg_sock, con_sock, dbg_ep, con_ep);
			connectTask.ContinueWith (t => callback (connectTask));
			return connectTask;
		}

		public static VirtualMachine EndConnect (IAsyncResult asyncResult) {
			if (asyncResult == null)
				throw new ArgumentNullException ("asyncResult");

			var connectTask = (Task<VirtualMachine>)asyncResult;
			return connectTask.Result;
		}

		public static void CancelConnection (IAsyncResult asyncResult)
		{
			//AsyncState could be null if the debugger incoming connection doesn't happen, so there's no socket between debugger and debuggee
			//This could occur if the debugger session started but the connection to the app failed at some point
			((Socket)asyncResult.AsyncState)?.Close ();
		}
		
		public static VirtualMachine Connect (Connection transport, StreamReader standardOutput, StreamReader standardError)
		{
			VirtualMachine vm = new VirtualMachine (null, transport);
			
			vm.StandardOutput = standardOutput;
			vm.StandardError = standardError;
			
			transport.EventHandler = new EventHandler (vm);

			vm.connect ();

			return vm;
		}
	}
}
