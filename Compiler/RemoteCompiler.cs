// Copyright (©) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using PascalABCCompiler.Errors;
using PascalABCCompiler.TreeRealization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;

namespace PascalABCCompiler
{

    public enum RemoteCompilerChannelEventType { Send, Receive };
    public delegate void RemoteCompilerChannelEventDelegate(RemoteCompilerChannelEventType eventType, string text);
    public interface IRemoteCompiler
    {
        int MaxProcessMemoryMB
        {
            get;
            set;            
        }
        event RemoteCompilerChannelEventDelegate RemoteCompilerChannelAction;
    }

    public class RemoteCompilerError : TreeConverter.CompilationErrorWithLocation
    {
        string msg;
        public RemoteCompilerError(string msg, location loc)
            : base(loc)
    	{
            this.msg = msg;
    	}
    	
		public override string ToString()
		{
			return msg;
		}
    }
    public class RemoteCompilerInternalError : CompilerInternalError
    {
        public RemoteCompilerInternalError(string msg)
            : base("[pabcnetc.exe]",new Exception(msg))
        {
        }
    }

    public class RemoteCompilerWarning : Errors.CompilerWarning
    {
        string msg;
        location loc;

        public override SourceLocation SourceLocation
        {
            get
            {
                if(loc!=null)
                    return new SourceLocation(loc.doc.file_name, loc.begin_line_num, loc.begin_column_num, loc.end_line_num, loc.end_column_num);
                else
                    return null;
            }
        }

        public RemoteCompilerWarning(string msg, location loc)
        {
            this.msg = msg;
            this.loc = loc;
        }

        public override string ToString()
        {
            return msg;
        }
    }

    public class RemoteCompiler: ICompiler
    {

        int maxProcessMemoryMB = ConsoleCompilerConstants.MaxProcessMemoryMB;
        Process pabcnetcProcess=null;
        EventedStreamReaderList pabcnetcStreamReader;
        string inputId = "pabcnetc_input";
        string pabcnetcFileName = "pabcnetc.exe";
        public const int sendCommandStartNumber = 100;
        public volatile CompilerState compilerState = CompilerState.Reloading; // PVS 01/2022 volatile
        Encoding inputEncoding = System.Text.Encoding.UTF8;
        bool compilationSatarted = false;

        public int MaxProcessMemoryMB
        {
            get
            {
                return maxProcessMemoryMB;
            }
            set
            {
                maxProcessMemoryMB=value;
                CheckProcessMemory();
            }
        }

        public RemoteCompiler(int maxProcessMemoryMB, ChangeCompilerStateEventDelegate ChangeCompilerState, SourceFilesProviderDelegate sourceFilesProvider)
        {
            this.OnChangeCompilerState += ChangeCompilerState;
            this.sourceFilesProvider = sourceFilesProvider;
            this.maxProcessMemoryMB = maxProcessMemoryMB;
            pabcnetcStreamReader = new EventedStreamReaderList(stringRecived);
            pabcnetcStreamReader.DataSeparator = ConsoleCompilerConstants.DataSeparator;
            Reload();
        }

        public void stringRecived(string id, string line)
        {
            string arg = null;
            if (line.Length < 3)
                return;
            if ((int)line[0] == 65279 && (int)line[1] == 65279)
            {
                line = line.Substring(2);
                if (line.Length < 3)
                    return;
            }
                
            if (line.Length > 3)
                arg = line.Substring(4);
            string[] args = Tools.SplitString(arg, ConsoleCompilerConstants.MessageSeparator);
            int command = Convert.ToInt32(line.Substring(0, 3));
            
            if (command < sendCommandStartNumber + 50)
            {
                ChangeCompilerState((CompilerState)(command - sendCommandStartNumber), arg);
                return;
            }
            switch (command)
            {
                case ConsoleCompilerConstants.PABCHealth:
                    pABCCodeHealth = Convert.ToInt32(arg);
                    break;
                case ConsoleCompilerConstants.LinesCompiled:
                    linesCompiled = Convert.ToUInt32(arg);
                    break;
                case ConsoleCompilerConstants.BeginOffest:
                    beginOffset = Convert.ToInt32(arg);
                    break;
                case ConsoleCompilerConstants.VarBeginOffest:
                    varBeginOffset = Convert.ToInt32(arg);
                    break;
                case ConsoleCompilerConstants.Warning:
                case ConsoleCompilerConstants.InternalError:
                case ConsoleCompilerConstants.Error:
                    location loc = null;
                    if (args.Length == 6)
                        loc = new location(
                            Convert.ToInt32(args[1]), Convert.ToInt32(args[2]),
                            Convert.ToInt32(args[3]), Convert.ToInt32(args[4]),
                            new document(args[5]));
                    if (args.Length == 2)
                        loc = new location(0,0,0,0,new document(args[1]));
                    switch(command)
                    {
                        case ConsoleCompilerConstants.Error:
                            errorsList.Add(new RemoteCompilerError(args[0], loc));
                            break;
                        case ConsoleCompilerConstants.Warning:
                            warnings.Add(new RemoteCompilerWarning(args[0], loc));
                            break;
                        case ConsoleCompilerConstants.InternalError:
                            errorsList.Add(new RemoteCompilerInternalError(args[0]));
                            break;
                    }
                    break;
                case ConsoleCompilerConstants.WorkingSet:
                    remoteCompilerWorkingSet = Convert.ToInt64(arg);
                    break;
                case ConsoleCompilerConstants.CompilerOptionsOutputType:
                    compilerOptions.OutputFileType = (CompilerOptions.OutputType)Convert.ToInt32(arg);
                    break;
                case ConsoleCompilerConstants.FileExsist:
                    bool b = (bool)sourceFilesProvider(arg, SourceFileOperation.Exists);
                    sendCommand(ConsoleCompilerConstants.FileExsist, b);
                    break;
                case ConsoleCompilerConstants.GetLastWriteTime:
                    DateTime d = (DateTime)sourceFilesProvider(arg, SourceFileOperation.GetLastWriteTime);
                    sendCommand(ConsoleCompilerConstants.GetLastWriteTime, d.Ticks);
                    break;
                case ConsoleCompilerConstants.SourceFileText:
                    string text = (string)sourceFilesProvider(arg, SourceFileOperation.GetText);
                    if (text == null)
                    {
                        sendCommand(ConsoleCompilerConstants.Error);
                        return;
                        //throw new CompilerInternalError("RemoteCompiler", new Exception(string.Format("Could not compile: {0}", arg)));
                    }
                    //Encoding enc = (Encoding)sourceFilesProvider(arg, SourceFileOperation.FileEncoding);
                    //text = Tools.ChangeEncoding(text, enc, Encoding.UTF8);
                    //text = Tools.ChangeEncoding(text, Encoding.GetEncoding(1251), Encoding.UTF8);
                    if (pabcnetcProcess == null)
                        Reload();
                    Process pr = pabcnetcProcess;
                    sendCommand(ConsoleCompilerConstants.SourceFileText, text.Length);
                    byte[] buf = Encoding.UTF8.GetBytes(text);
                    pabcnetcProcess.StandardInput.BaseStream.Write(buf, 0, buf.Length);
                    pabcnetcProcess.StandardInput.Flush();
                    break;
                default:
                    throw new CompilerInternalError("RemoteCompiler", new Exception(string.Format("Unkown command from pabcnetc.exe: {0}", command)));
            }
        }

        void ChangeCompilerState(CompilerState State, string FileName)
        {
            if (State == CompilerState.Ready && compilationSatarted)
            {
                compilationSatarted = false;
                if (CheckProcessMemory())
                    return;
            }
            compilerState = State;
            if (OnChangeCompilerState != null)
                OnChangeCompilerState(this, State, FileName);
        }

        void stopCompiler()
        {
            try
            {
                if (pabcnetcProcess != null && !pabcnetcProcess.HasExited)
                {
                    pabcnetcProcess.EnableRaisingEvents = false;
                    pabcnetcStreamReader.Remove(inputId);
                    //sendCommand(ConsoleCompilerConstants.CommandExit);
                    pabcnetcProcess.Kill();
                    pabcnetcProcess = null;
                }
                remoteCompilerWorkingSet = 0;
            }
            catch
            {
                pabcnetcProcess = null;
                remoteCompilerWorkingSet = 0;
            }
        }


        uint linesCompiled = 0;
        public uint LinesCompiled
        {
            get
            {
                return linesCompiled;
            }
        }

        // SSM 18/09/20
        int pABCCodeHealth = 0;
        public int PABCCodeHealth { get { return pABCCodeHealth; } }

        CompilerInternalDebug internalDebug=new CompilerInternalDebug();
        public CompilerInternalDebug InternalDebug
        {
            get
            {
                return internalDebug;
            }
            set
            {
                internalDebug = value;
            }
        }

        public CompilerState State
        {
            get 
            {
                return compilerState;
            }
        }

        CompilerOptions compilerOptions = new CompilerOptions();
        public CompilerOptions CompilerOptions
        {
            get
            {
                return compilerOptions;
            }
            set
            {
                compilerOptions = value;
            }
        }

        List<PascalABCCompiler.Errors.Error> errorsList = new List<PascalABCCompiler.Errors.Error>();
        public List<PascalABCCompiler.Errors.Error> ErrorsList
        {
            get 
            {
                return errorsList;
            }
        }

        public List<PascalABCCompiler.Errors.CompilerWarning> warnings = new List<PascalABCCompiler.Errors.CompilerWarning>();
        public List<PascalABCCompiler.Errors.CompilerWarning> Warnings
        {
            get 
            { 
                return warnings; 
            }
        }

        int beginOffset = 0;
        public int BeginOffset
        {
            get { return beginOffset; }
        }

        int varBeginOffset = 0;
        public int VarBeginOffset
        {
            get { return varBeginOffset; }
        }
        
        public event ChangeCompilerStateEventDelegate OnChangeCompilerState;

        void sendCommand(int command, object arg)
        {
            if (pabcnetcProcess == null)
                Reload();
            pabcnetcProcess.StandardInput.WriteLine(string.Format("{0} {1}", command, arg));
            pabcnetcProcess.StandardInput.Flush();
        }

        void sendObjectAsByteArray(int command, string obj)
        {
            if (pabcnetcProcess == null)
                Reload();
            pabcnetcProcess.StandardInput.WriteLine(string.Format("{0} {1}", command, obj.Length));
            pabcnetcProcess.StandardInput.Flush();
            byte[] buf = Encoding.UTF8.GetBytes(obj);
            pabcnetcProcess.StandardInput.BaseStream.Write(buf, 0, buf.Length);
            pabcnetcProcess.StandardInput.Flush();
        }

        void sendCommand(int command, params object[] args)
        {
            string s = args[0].ToString();
            for (int i = 1; i < args.Length; i++)
                s += ConsoleCompilerConstants.MessageSeparator + args[i].ToString();
            sendCommand(command, s);
        }
        
        void sendCommand(int command)
        {
            if (pabcnetcProcess == null)
                Reload();
            pabcnetcProcess.StandardInput.WriteLine(command);
        }
        
        void sendObject(int id, object o)
        {
            sendCommand(id);
            MemoryStream ms = new MemoryStream();
            (new BinaryFormatter()).Serialize(ms, o);
            ms.WriteTo(pabcnetcProcess.StandardInput.BaseStream);            
        }

        void sendCompilerOptions()
        {
            byte[] encoded = Encoding.Unicode.GetBytes(compilerOptions.SourceFileName);
            string file_name = Encoding.UTF8.GetString(encoded);
            sendObjectAsByteArray(ConsoleCompilerConstants.CompilerOptionsFileName, compilerOptions.SourceFileName);
            //sendCommand(ConsoleCompilerConstants.CompilerOptionsFileName, file_name);//compilerOptions.SourceFileName);
            sendCommand(ConsoleCompilerConstants.CompilerOptionsProjectCompiled, compilerOptions.ProjectCompiled);
            sendCommand(ConsoleCompilerConstants.UseDllForSystemUnits, compilerOptions.UseDllForSystemUnits);
            sendCommand(ConsoleCompilerConstants.CompilerOptionsDebug, compilerOptions.Debug);
            sendCommand(ConsoleCompilerConstants.CompilerOptionsRebuild, compilerOptions.Rebuild);
            sendCommand(ConsoleCompilerConstants.CompilerOptionsOutputType, (int)compilerOptions.OutputFileType);
            sendCommand(ConsoleCompilerConstants.InternalDebugSavePCU, InternalDebug.PCUGenerate);
            sendCommand(ConsoleCompilerConstants.CompilerOptionsForDebugging, CompilerOptions.ForDebugging);
            sendCommand(ConsoleCompilerConstants.CompilerOptionsRunWithEnvironment, CompilerOptions.RunWithEnvironment);
            sendCommand(ConsoleCompilerConstants.IDELocale, Encoding.UTF8.GetString(Encoding.Unicode.GetBytes(StringResourcesLanguage.CurrentLanguageName)));
            encoded = Encoding.Unicode.GetBytes(compilerOptions.OutputDirectory);
            string dir_name = Encoding.UTF8.GetString(encoded);
            //sendCommand(ConsoleCompilerConstants.CompilerOptionsOutputDirectory, dir_name);
            sendObjectAsByteArray(ConsoleCompilerConstants.CompilerOptionsOutputDirectory, compilerOptions.OutputDirectory);
            sendCommand(ConsoleCompilerConstants.CompilerOptionsClearStandartModules);
            if (compilerOptions.Locale != null)
                sendCommand(ConsoleCompilerConstants.CompilerLocale, compilerOptions.Locale);
            
            foreach (var kv in compilerOptions.StandardModulesByLanguages)
            {
                foreach (CompilerOptions.StandardModule module in kv.Value)
                {
					sendCommand(ConsoleCompilerConstants.CompilerOptionsStandartModule,
					    module.name, (int)module.addMethod, module.languageToAdd);
				}
            }

        }

        public delegate void EnvorimentIdleDelegate();
        public event EnvorimentIdleDelegate EnvorimentIdle;

        void waitCompilerReloading()
        {
            while (compilerReloading)
            {
                Thread.Sleep(5);
                if (EnvorimentIdle != null)
                    EnvorimentIdle();
            }
        }

        public string Compile()
        {
            errorsList.Clear();
            warnings.Clear();

            //sendObject(ConsoleCompilerConstants.InternalDebug, internalDebug);
            //sendObject(ConsoleCompilerConstants.CompilerOptions, compilerOptions);
            waitCompilerReloading();

            sendCompilerOptions();
            compilerState = CompilerState.CompilationStarting;
            compilationSatarted = true;
            sendCommand(ConsoleCompilerConstants.CommandCompile);
            while (compilerState != CompilerState.Ready)
                Thread.Sleep(5);
            if (errorsList.Count > 0)
                return null;
            return compilerOptions.OutputFileName;
        }

        bool CheckProcessMemory()
        {
            if (RemoteCompilerWorkingSet / 1024 / 1024 > maxProcessMemoryMB)
            {
                (new Thread(Reload)).Start();
                return true;
            }
            return false;

        }

        long remoteCompilerWorkingSet = 0;
        public long RemoteCompilerWorkingSet
        {
            get
            {
                return remoteCompilerWorkingSet;
                //return pabcnetcProcess.WorkingSet64;
            }
        }

        public void StartCompile()
        {
            errorsList.Clear();
            warnings.Clear();
            waitCompilerReloading();
            sendCompilerOptions();
            compilationSatarted = true;
            sendCommand(ConsoleCompilerConstants.CommandCompile);
        }

        public void AddWarnings(List<PascalABCCompiler.Errors.CompilerWarning> WarningList)
        {
            warnings.AddRange(WarningList);
        }


        SourceFilesProviderDelegate sourceFilesProvider = null;
        public SourceFilesProviderDelegate SourceFilesProvider
        {
            get
            {
                return sourceFilesProvider;
            }
            set
            {
                sourceFilesProvider = value;
            }
        }

        public SupportedSourceFile[] SupportedSourceFiles
        {
            get
            {
                throw new NotSupportedException();
            }
        }
		
        public SupportedSourceFile[] SupportedProjectFiles
        {
            get
            {
                throw new NotSupportedException();
            }
        }
        
        public PascalABCCompiler.SyntaxTree.compilation_unit ParseText(string FileName, string Text, List<PascalABCCompiler.Errors.Error> ErrorList, List<CompilerWarning> Warnings)
        {
            throw new NotSupportedException();
        }

        public string GetSourceFileText(string FileName)
        {
            throw new NotSupportedException();
        }

        public PascalABCCompiler.SyntaxTreeConverters.SyntaxTreeConvertersController SyntaxTreeConvertersController
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public PascalABCCompiler.SemanticTreeConverters.SemanticTreeConvertersController SemanticTreeConvertersController
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public PascalABCCompiler.Parsers.Controller ParsersController
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public PascalABCCompiler.SemanticTree.IProgramNode SemanticTree
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public CompilationUnitHashTable UnitTable
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        bool compilerReloading = true;
        public void Reload()
        {
            pABCCodeHealth = 0;
            compilerReloading = true;
            stopCompiler();
            pabcnetcProcess = new Process();
            if (!IsUnix())
            {
                pabcnetcProcess.StartInfo.FileName = Path.Combine(Tools.GetExecutablePath(), pabcnetcFileName);
                pabcnetcProcess.StartInfo.Arguments = "commandmode";
            }
            else
            {
                pabcnetcProcess.StartInfo.FileName = "mono";
                pabcnetcProcess.StartInfo.Arguments = Path.Combine(Tools.GetExecutablePath(), pabcnetcFileName)+" commandmode";
                
            }
                
            pabcnetcProcess.StartInfo.UseShellExecute = false;
            pabcnetcProcess.StartInfo.CreateNoWindow = true;
            pabcnetcProcess.StartInfo.RedirectStandardOutput = true;
            pabcnetcProcess.StartInfo.RedirectStandardInput = true;
            pabcnetcProcess.StartInfo.RedirectStandardError = true;
            pabcnetcProcess.EnableRaisingEvents = true;
            pabcnetcProcess.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
            pabcnetcProcess.Exited += pabcnetcProcess_Exited;
            pabcnetcStreamReader.Remove(inputId);
            pabcnetcProcess.Start();
            //pabcnetcProcess.StandardInput.Encoding;
            pabcnetcStreamReader.Add(pabcnetcProcess.StandardOutput, inputId, inputEncoding);
            compilerReloading = false;
        }

        public static bool IsUnix()
        {
            return System.Environment.OSVersion.Platform == System.PlatformID.Unix || System.Environment.OSVersion.Platform == System.PlatformID.MacOSX;
        }

        void pabcnetcProcess_Exited(object sender, EventArgs e)
        {
            string error = pabcnetcProcess.StandardError.ReadToEnd();

            if (!compilerReloading)
            {
                pabcnetcProcess = null;
                //Reload();
            }
        }

        public CompilerType CompilerType
        {
            get
            {
                return CompilerType.Remote;
            }
        }

        ~RemoteCompiler()
        {
            Free();
        }

        public void Free()
        {
            stopCompiler();
        }

    }
}
