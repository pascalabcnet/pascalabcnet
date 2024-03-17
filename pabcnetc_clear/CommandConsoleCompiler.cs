// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PascalABCCompiler
{
    internal class SourceFile
    {
        internal string Text = null;
        internal DateTime LastWriteTime = DateTime.MinValue;
    }
    public class CommandConsoleCompiler
    {


        private Compiler compiler;

        private void ChangeCompilerState(ICompiler sender, PascalABCCompiler.CompilerState State, string FileName)
        {
            switch (State)
            {
                case CompilerState.CompilationFinished:
                    if (compiler.ErrorsList.Count > 0)
                        foreach (Errors.Error er in compiler.ErrorsList)
                            SendErrorOrWarning(er);
                    if (compiler.Warnings.Count > 0)
                        foreach (Errors.Error er in compiler.Warnings)
                            SendErrorOrWarning(er);
                    SendCommand(ConsoleCompilerConstants.LinesCompiled, compiler.LinesCompiled.ToString());
                    SendCommand(ConsoleCompilerConstants.BeginOffest, compiler.BeginOffset.ToString());
                    SendCommand(ConsoleCompilerConstants.VarBeginOffest, compiler.VarBeginOffset.ToString());
                    SendCommand(ConsoleCompilerConstants.CompilerOptionsOutputType, ((int)compiler.CompilerOptions.OutputFileType).ToString());
                    sendWorkingSet();
                    break;
                case CompilerState.Ready:
                    if(compilerReloading)
                        sendWorkingSet();
                    break;
            }
            if (FileName != null)
                SendCommand(ConsoleCompilerConstants.CommandStartNumber + (int)State, FileName);
            else
                SendCommand(ConsoleCompilerConstants.CommandStartNumber + (int)State);
        }

        internal Dictionary<string, SourceFile> SourceFiles = new Dictionary<string, SourceFile>();



        public object SourceFilesProvider(string FileName, SourceFileOperation FileOperation)
        {
            string fn = FileName.ToLower();
            int command=0;
            string arg=null;
            switch (FileOperation)
            {
                case SourceFileOperation.GetText:
                    if (SourceFiles.ContainsKey(fn))
                        return SourceFiles[fn].Text;
                    string text = getFileText(FileName);
                    if (text != null)
                        return text;
                    if (!File.Exists(FileName))
                        return null;
                    /*TextReader tr = new StreamReader(file_name, System.Text.Encoding.GetEncoding(1251));
                    text = tr.ReadToEnd();
                    tr.Close();*/
                    text = PascalABCCompiler.FileReader.ReadFileContent(FileName, null);
                    return text;
                case SourceFileOperation.Exists:
                    if (SourceFiles.ContainsKey(fn))
                        return true;
                    SendCommand(ConsoleCompilerConstants.FileExsist, FileName);
                    ReadCommand(out command, out arg);
                    bool b = Convert.ToBoolean(arg);
                    if (!b)
                        return File.Exists(fn);
                    else
                        return true;
                case SourceFileOperation.GetLastWriteTime:
                    if (SourceFiles.ContainsKey(fn))
                        return SourceFiles[fn].LastWriteTime;
                    SendCommand(ConsoleCompilerConstants.GetLastWriteTime, FileName);
                    ReadCommand(out command, out arg);
                    if (arg != null)
                        return new DateTime(Convert.ToInt64(arg));
                    return File.GetLastWriteTime(FileName);
            }
            return null;
        }

        private string getFileText(string FileName)
        {
            SendCommand(ConsoleCompilerConstants.SourceFileText, FileName);
            int command = 0;
            string arg = null;
            ReadCommand(out command, out arg);
            int length = Convert.ToInt32(arg);
            return readStringFromConsole(length);
        }

        string readStringFromConsole(int count)
        {
            char[] buf=new char[count];
            Console.In.ReadBlock(buf, 0, count);
            return new string(buf);
            //return Tools.ChangeEncoding(new string(buf), Encoding.UTF8, Encoding.GetEncoding(1251));
            /*
            Stream stream=Console.OpenStandardInput();
            int c = 0;
            byte[] buffer = new byte[Math.Min(count, 2048)];
            byte[] res = new byte[count];
            while (c != count)
            {
                int i = stream.Read(buffer, 0, Math.Min(count - c, buffer.Length));
                buffer.
                    (res, c);
                c += i;
            }
            return Encoding.GetEncoding(1251).GetString(res);*/
        }
        bool compilerReloading = false;
        public void LoadCompiler()
        {
            compilerReloading = true;
            compiler = new Compiler(SourceFilesProvider, ChangeCompilerState);
            compilerReloading = false;
        }

        void SendCommand(int command, string arg)
        {
            Console.Out.Write(string.Format("{0} {1}", command, arg) + ConsoleCompilerConstants.DataSeparator);
            Console.Out.Flush();
        }
        void SendCommand(int command)
        {
            Console.Out.Write(command + ConsoleCompilerConstants.DataSeparator);
            Console.Out.Flush();
        }
        void SendErrorOrWarning(Errors.Error er)
        {
            string str = er.Message;
            if (er is Errors.LocatedError)
            {
                Errors.LocatedError le = er as Errors.LocatedError;
                if (le.SourceLocation != null)
                {
                    str += ConsoleCompilerConstants.MessageSeparator + le.SourceLocation.BeginPosition.Line;
                    str += ConsoleCompilerConstants.MessageSeparator + le.SourceLocation.BeginPosition.Column;
                    str += ConsoleCompilerConstants.MessageSeparator + le.SourceLocation.BeginPosition.Line;
                    str += ConsoleCompilerConstants.MessageSeparator + le.SourceLocation.EndPosition.Column;
                    str += ConsoleCompilerConstants.MessageSeparator + le.SourceLocation.FileName;
                }
                else
                    if (le.fileName != null && le.fileName != "")
                        str += ConsoleCompilerConstants.MessageSeparator + le.fileName;
            }
            if (er is Errors.CompilerWarning)
                SendCommand(ConsoleCompilerConstants.Warning, str);
            else
                if (er is Errors.CompilerInternalError)
                    SendCommand(ConsoleCompilerConstants.InternalError, str);
                else
                    SendCommand(ConsoleCompilerConstants.Error, str);
        }

        void ReadCommand(out int command, out string arg)
        {
            string line = Console.ReadLine();
            command = 0;
            arg = null;
            if (line == null)
                return;
            if (line.Length > 3)
                arg = line.Substring(4);
            command = Convert.ToInt32(line.Substring(0, 3));
        }

        Stream standardInput;

        object ReadObject()
        {
            standardInput = Console.OpenStandardInput();
            object o=(new BinaryFormatter()).Deserialize(standardInput);
            return o;
        }

        bool executeCommand(string line)
        {
            string arg = null;
            if (line.Length > 3)
                arg = line.Substring(4);
            string[] args = Tools.SplitString(arg, ConsoleCompilerConstants.MessageSeparator);
            int command = Convert.ToInt32(line.Substring(0, 3));
            byte[] encoded;
            switch (command)
            {
                case ConsoleCompilerConstants.CommandExit:
                    SendCommand(ConsoleCompilerConstants.CommandExit, "Bye!");
                    return false;
                case ConsoleCompilerConstants.CommandCompile:
                    SourceFiles.Clear();
                    compiler.Compile();
                    break;
                case ConsoleCompilerConstants.CompilerOptionsOutputType:
                    compiler.CompilerOptions.OutputFileType = (CompilerOptions.OutputType)Convert.ToInt32(arg);
                    break;
                case ConsoleCompilerConstants.CompilerOptionsOutputDirectory:
                    encoded = Encoding.UTF8.GetBytes(arg);
                    compiler.CompilerOptions.OutputDirectory = Encoding.Unicode.GetString(encoded); //arg;
                    break;
                case ConsoleCompilerConstants.CompilerOptionsDebug:
                    compiler.CompilerOptions.Debug = Convert.ToBoolean(arg);
                    break;
                case ConsoleCompilerConstants.CompilerOptionsRebuild:
                    compiler.CompilerOptions.Rebuild = Convert.ToBoolean(arg);
                    break;
                case ConsoleCompilerConstants.CompilerOptionsProjectCompiled:
                    compiler.CompilerOptions.ProjectCompiled = Convert.ToBoolean(arg);
                    break;
                case ConsoleCompilerConstants.UseDllForSystemUnits:
                    compiler.CompilerOptions.UseDllForSystemUnits = Convert.ToBoolean(arg);
                    break;
                case ConsoleCompilerConstants.CompilerOptionsForDebugging:
                    compiler.CompilerOptions.ForDebugging = Convert.ToBoolean(arg);
                    break;
                case ConsoleCompilerConstants.CompilerOptionsRunWithEnvironment:
                    compiler.CompilerOptions.RunWithEnvironment = Convert.ToBoolean(arg);
                    break;
                case ConsoleCompilerConstants.CompilerOptionsFileName:
                    encoded = Encoding.UTF8.GetBytes(arg);
                    compiler.CompilerOptions.SourceFileName = Encoding.Unicode.GetString(encoded); //arg;
                    break;
                case ConsoleCompilerConstants.CommandGCCollect:
                    GC.Collect();
                    break;
                case ConsoleCompilerConstants.InternalDebugSavePCU:
                    compiler.InternalDebug.PCUGenerate = Convert.ToBoolean(arg);
                    break;
                case ConsoleCompilerConstants.WorkingSet:
                    sendWorkingSet();
                    break;
                case ConsoleCompilerConstants.CompilerOptionsClearStandartModules:
                    foreach (var modulesList in compiler.CompilerOptions.StandardModules.Values)
                        modulesList.Clear();
                    break;
                case ConsoleCompilerConstants.CompilerOptionsStandartModule:
                    CompilerOptions.StandardModule sm = new CompilerOptions.StandardModule(args[0],
                        (CompilerOptions.StandardModuleAddMethod)Convert.ToInt32(args[1]),
                        args[2]);
                    compiler.CompilerOptions.StandardModules[sm.languageToAdd].Add(sm);
                    break;
                case ConsoleCompilerConstants.InternalDebug:
                    compiler.InternalDebug = (CompilerInternalDebug)ReadObject();
                    break;
                case ConsoleCompilerConstants.CompilerOptions:
                    compiler.CompilerOptions = (CompilerOptions)ReadObject();
                    break;
            }
            return true;
        }

        public int Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            // загрузка всех парсеров и других составляющих языков  EVA
            LanguageIntegration.LanguageIntegrator.LoadAllLanguages();

            LoadCompiler();
            do
            {
                string line = Console.ReadLine();
                if (line == null)
                    return 0;
                if (!executeCommand(line))
                    return 0;
            }
            while (true);
        }

        private void sendWorkingSet()
        {
            SendCommand(ConsoleCompilerConstants.WorkingSet, Environment.WorkingSet.ToString());
        }
    }
}
