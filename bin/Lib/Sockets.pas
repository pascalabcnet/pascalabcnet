// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
unit Sockets;

{$reference 'System.dll'}

interface

function CreateServerSocket(port: integer) : integer;
function Accept(sock: integer) : integer;
procedure ReceiveString(sock : integer; var s : string);
procedure CloseSocket(sock : integer);
function CreateClientSocket : integer;
function Connect(sock : integer; ip_addr : string; port : integer) : boolean;
procedure SendString(sock : integer; s : string);

implementation

uses System, System.Net, System.Net.Sockets, System.IO;

type TByteArray = array of byte;

var hts : System.Collections.Hashtable;
	htc : System.Collections.Hashtable;
	htsc : System.Collections.Hashtable;
	num_sock := 0;
	//num_srv_clnt : integer=0;
	bytes : TByteArray;
	
function CreateServerSocket(port: integer) : integer;
var server : TCPListener;
begin
	server := TCPListener.Create(IPAddress.Parse('127.0.0.1'),port);
	server.Start;
	hts[num_sock] := server;
	Result := num_sock;
	Inc(num_sock);
end;

function CreateClientSocket : integer;
var client : TCPClient;
begin
	client := TCPClient.Create;
	htc[num_sock] := client;
	Result := num_sock;
	Inc(num_sock);
end;

function Accept(sock: integer) : integer;
var client : TCPClient;
begin
	client := (hts[sock] as TCPListener).AcceptTCPClient;
	htsc[num_sock] := client;
	Result := num_sock;
	Inc(num_sock);
end;

procedure SendString(sock : integer; s : string);
var buf : NetworkStream;
	client : TCPClient;
begin
	client := htc[sock] as TCPClient;
	buf := client.GetStream;
	buf.Write(System.Text.Encoding.ASCII.GetBytes(s),0,s.Length);
end;

procedure ReceiveString(sock : integer; var s : string);
var buf : NetworkStream;
	client : TCPClient;
	len : integer;
	
begin
	client := htsc[sock] as TCPClient;
	buf := client.GetStream;
	len := buf.Read(bytes,0,bytes.Length);
	s := System.Text.Encoding.ASCII.GetString(bytes, 0, len);
end;

procedure CloseSocket(sock : integer);
var o : System.Object;
begin
	o := hts[sock];
	if o <> nil then (TCPListener(o)).Stop
	else
	begin
		o := htc[sock];
		if o <> nil then (TCPClient(o)).Close
		else
		begin
			o := htsc[sock];
			if o <> nil then (TCPClient(o)).Close;
		end;
	end
end;

function Connect(sock : integer; ip_addr : string; port : integer) : boolean;
begin
	(htc[sock] as TCPClient).Connect(ip_addr,port);
	Result := true;
end;

initialization
htc := new System.Collections.Hashtable;
hts := new System.Collections.Hashtable;
htsc := new System.Collections.Hashtable;
bytes := TByteArray(System.Array.CreateInstance(typeof(byte),1024));
end.