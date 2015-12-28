// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using PascalABCCompiler.Errors;
using System.IO;
using System.Diagnostics;

namespace VisualPascalABC
{
    public class RunManager 
    {
        Hashtable StartedProcesses = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
        Hashtable StartedFiles = new Hashtable();
        PascalABCCompiler.EventedStreamReaderList EventedStreamReaderList;
        //EventedEventWaitHandleList ReadSignalList;
        //Encoding InputEncoding = Encoding.GetEncoding(866);
        Encoding InputEncoding = Encoding.UTF8;
		
        string OutputStreamId = "OUT";
        string ErrorStreamId = "ERR";
        const int idStreamLength = 3;
        public enum StreamType { Output, Error };

        string RuntimeExceptionDelimer1 = "[EXCEPTION]";
        string RuntimeExceptionDelimer2 = "[MESSAGE]";
        string RuntimeExceptionDelimer3 = "[STACK]";
        string RuntimeExceptionDelimer4 = "[END]";
        string RuntimeExceptionAtIdent = " at ";
        string RuntimeExceptionLineIdent = ":line ";
        string RuntimeExceptionInIdent = " in ";
        string RuntimeCommandReadlnSignal = "[READLNSIGNAL]";
        string RuntimeCommandCodePage = "[CODEPAGE";
        
        public int Count
        {
            get { return StartedProcesses.Count; }
        }

        public delegate void TextRecivedDelegate(string fileName, StreamType streamType, string text);
        public event TextRecivedDelegate OutputStringReceived;
        
        public delegate void ReadStringRequestDelegate(string ForId);
        ReadStringRequestDelegate ReadStringRequest;

        void StringRecived(string id, string Data)
        {
            System.Threading.Thread.Sleep(50);
            string streamId = id.Substring(0, idStreamLength);
            id = id.Substring(idStreamLength);
            StreamType streamType = StreamType.Output;
            if (streamId == ErrorStreamId)
                streamType = StreamType.Error;
            if (streamType == StreamType.Error && IsSpecialMessage(id, Data))
                return;
            if (OutputStringReceived != null)
                OutputStringReceived(id, streamType, Data);
        }

        string waitSpecialMessageText = "";
        bool IsSpecialMessage(string id, string data)
        {
            bool res = false;
            try
            {
                if (data == RuntimeCommandReadlnSignal)
                {
                    ReadStringRequest(id);
                    return true;
                }
                if (data.IndexOf(RuntimeCommandCodePage)==0)
                {
                    int t = data.IndexOf(']');
                    int encodingNum = Convert.ToInt32(data.Substring(RuntimeCommandCodePage.Length, t - RuntimeCommandCodePage.Length));
                    Encoding NewEncoding = Encoding.GetEncoding(encodingNum);
                    Encoding OldEncoding = EventedStreamReaderList.GetEncoding(OutputStreamId + id); 
                    EventedStreamReaderList.SetEncoding(OutputStreamId + id, NewEncoding);
                    EventedStreamReaderList.SetEncoding(ErrorStreamId + id, NewEncoding);
                    if (data.Length > t + 1)
                    {
                        data = data.Substring(t + 1);
                        data = NewEncoding.GetString(OldEncoding.GetBytes(data));
                        if (data == RuntimeCommandReadlnSignal)
                		{
                    		ReadStringRequest(id);
                    		return true;
                		}
                        res = true;
                    }
                    else
                        return true;
                }
                if (data == '\n'.ToString())
                {
                    return true;
                }
                if (data.IndexOf(StackOverflowExceptionText) >= 0)
                {
                    RunnerManagerUnhanledRuntimeException(id, StackOverflowExceptionType, StackOverflowExceptionText, null, new List<StackTraceItem>());
                    return true;
                }
                if ((waitSpecialMessageText != "" || (data.Length >= RuntimeExceptionDelimer1.Length && data.Substring(0, RuntimeExceptionDelimer1.Length) == RuntimeExceptionDelimer1)) && RunnerManagerUnhanledRuntimeException != null)
                {
                    if (data.IndexOf(RuntimeExceptionDelimer4) < 0)
                    {
                        waitSpecialMessageText += data;
                        return true;
                    }
                    if (waitSpecialMessageText != "")
                        data = waitSpecialMessageText + data;
                    int Section1Begin = RuntimeExceptionDelimer1.Length;
                    int Section1Length = data.IndexOf(RuntimeExceptionDelimer2) - Section1Begin;
                    int Section2Begin = Section1Begin + Section1Length + RuntimeExceptionDelimer2.Length;
                    int Section2Length = data.IndexOf(RuntimeExceptionDelimer3) - Section2Begin;
                    int Section3Begin = Section2Begin + Section2Length + RuntimeExceptionDelimer3.Length;
                    int Section3Length = data.IndexOf(RuntimeExceptionDelimer4) - Section3Begin;
                    string ExceptionType = data.Substring(Section1Begin, Section1Length);
                    string ExceptionMessage = data.Substring(Section2Begin, Section2Length);
                    string StackTraceData = data.Substring(Section3Begin, Section3Length);
                    List<StackTraceItem> StackTrace = new List<StackTraceItem>();
                    string[] delimer = new string[1];
                    delimer[0] = Environment.NewLine;
                    string[] StackItemsData = StackTraceData.Split(delimer, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string StackItemData in StackItemsData)
                    {
                        StackTraceItem StackTraceItem = new StackTraceItem();
                        string str = StackItemData.TrimStart(' ');
                        int beg = str.IndexOf(' ');
                        int end = str.IndexOf(')');
                        StackTraceItem.FunctionName = str.Substring(beg + 1, end - beg);
                        if (end + 1 < str.Length)
                        {
                            str = str.Substring(end + 1);
                            str = str.TrimStart(' ');
                            beg = str.IndexOf(' ');
                            end = str.IndexOf(':');
                            end = str.IndexOf(':', end + 1);
                            StackTraceItem.SourceFileName = str.Substring(beg + 1, end - beg-1);
                            beg = str.IndexOf(' ',end);
                            if (beg > 0)
                            {
                                string slnum = str.Substring(beg + 1).Replace(".", "");                                
                                StackTraceItem.LineNumber = Convert.ToInt32(slnum);
                                if (StackTraceItem.LineNumber >= 16777214)
                                {
                                    StackTraceItem.LineNumber = 0;
                                    StackTraceItem.SourceFileName = null;
                                }
                            }
                        }
                        /*int RuntimeExceptionInIdentIndex = StackItemData.IndexOf(RuntimeExceptionInIdent);
                        int FunctionNameBegin = StackItemData.IndexOf(RuntimeExceptionAtIdent) + RuntimeExceptionAtIdent.Length;
                        if (RuntimeExceptionInIdentIndex > 0)
                        {
                            StackTraceItem.FunctionName = StackItemData.Substring(FunctionNameBegin, RuntimeExceptionInIdentIndex - FunctionNameBegin);
                            int RuntimeExceptionLineIdentIndex = StackItemData.IndexOf(RuntimeExceptionLineIdent);
                            int RuntimeExceptionInIdentEndPos = RuntimeExceptionInIdentIndex + RuntimeExceptionInIdent.Length;
                            StackTraceItem.SourceFileName = StackItemData.Substring(RuntimeExceptionInIdentEndPos, RuntimeExceptionLineIdentIndex - RuntimeExceptionInIdentEndPos);
                            int LineNumberStartPos = RuntimeExceptionLineIdentIndex + RuntimeExceptionLineIdent.Length;
                            int LineNumberLength = 0;
                            int i = LineNumberStartPos;
                            while (i < StackItemData.Length && !char.IsWhiteSpace(StackItemData[i]))
                            {
                                LineNumberLength++;
                                i++;
                            }
                            StackTraceItem.LineNumber = Convert.ToInt32(StackItemData.Substring(LineNumberStartPos, LineNumberLength));
                            if (StackTraceItem.LineNumber >= 16777214)
                            {
                                StackTraceItem.LineNumber = 0;
                                StackTraceItem.SourceFileName = null;
                            }
                        }
                        else
                        {
                            StackTraceItem.FunctionName = StackItemData.Substring(FunctionNameBegin, StackItemData.Length - FunctionNameBegin);
                        }*/
                        StackTrace.Add(StackTraceItem);
                    }
                    RunnerManagerUnhanledRuntimeException(id, ExceptionType, ExceptionMessage, StackTraceData, StackTrace);
                    waitSpecialMessageText = "";
                    return true;
                }
            }
            catch (Exception e)
            {
                //OutputStringReceived(id, StreamType.Output,e.ToString());
            }
            return res;
        }

        string StackOverflowExceptionText;
        string StackOverflowExceptionType;
        public RunManager(ReadStringRequestDelegate ReadStringRequest)
        {
            this.ReadStringRequest = ReadStringRequest;
            EventedStreamReaderList = new PascalABCCompiler.EventedStreamReaderList(StringRecived);
            StackOverflowExceptionText = PascalABCCompiler.StringResources.Get("!STACK_OVERFLOW_EXCEPTION_TEXT");
            StackOverflowExceptionType = "StackOverflowException";
            //ReadSignalList = new EventedEventWaitHandleList(ReadSignal);
            /*
                string name_format = ".\\" + Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName) + "{0}";
                string name;
                while (MessageQueue == null)
                {
                    name = string.Format(name_format, i);
                    if (!System.Messaging.MessageQueue.Exists(name))
                        MessageQueue = new System.Messaging.MessageQueue(name);
                    i++;
                }
            */
        }


        public delegate void RunnerManagerActionDelegate(string fileName);
        public event RunnerManagerActionDelegate Exited;
        public event RunnerManagerActionDelegate Starting;

        public class StackTraceItem
        {
            public string SourceFileName;
            public int LineNumber;
            public string FunctionName;
        }

        public delegate void RunnerManagerUnhanledRuntimeExceptionDelegate(string id, string ExceptionType, string ExceptionMessage, string StackTraceData, List<StackTraceItem> StackTrace);
        public event RunnerManagerUnhanledRuntimeExceptionDelegate RunnerManagerUnhanledRuntimeException;

        

        public bool IsRun(string fileName)
        {
            return StartedProcesses[fileName] != null;
        }

        public bool IsRun()
        {
            return StartedProcesses.Count != 0;
        }
        public void KillAll()
        {
            Utils.ProcessRunner[] process = new Utils.ProcessRunner[StartedProcesses.Values.Count];
            StartedProcesses.Values.CopyTo(process, 0);
            foreach (Utils.ProcessRunner pr in process)
            {
                pr.Kill();
                PRunner_ProcessExited(pr,null);
            }
        }
        
        public void SendText(string FileName, string Text)
        {
            //messageServer.SendText(FileName, Text);
        }

        /*private void MessageQueueReceive_Callback(IAsyncResult ar)
        {
            if (ar.IsCompleted)
            {
                System.Messaging.Message msg = MessageQueue.EndReceive(ar);
                string s = msg.Body as string;
                int delimerPos = s.IndexOf(':');
                string id = s.Substring(0, delimerPos - 1);
                string cmd = s.Substring(delimerPos);
                if (OutputLineReceived != null)
                    OutputLineReceived(id, cmd);
            }
            if (StartedFiles.Count > 0)
                MessageQueue.BeginReceive(TimeSpan.FromSeconds(10), null, MessageQueueReceive_Callback);
            else
                MessageQueueBeginReceive = false;
        }*/

        //bool MessageQueueBeginReceive = false;
        //Dictionary<string, string> ReadSignalNames = new Dictionary<string, string>();

        public Dictionary<string, string> TempBatFiles = new Dictionary<string, string>();


        public void Run(string fileName,string args,bool redirectIO,bool redirectErrors, bool RunWithPause, string WorkingDirectory, bool attachDebugger, bool fictive_attach)
        {
            Utils.ProcessRunner PRunner = new Utils.ProcessRunner();
            PRunner.ProcessExited += new EventHandler(PRunner_ProcessExited);
            //PRunner.OutputLineReceived += new Utils.LineReceivedEventHandler(PRunner_OutputLineReceived);
            PRunner.WorkingDirectory = WorkingDirectory;
            StartedProcesses.Add(fileName, PRunner);
            StartedFiles.Add(PRunner, fileName);
            string ReadSignalName=null;
            /*if (redirectIO)
            {
                ReadSignalName = Path.GetFileNameWithoutExtension(fileName) + (new Random()).Next(10000).ToString();
                args = "READSIGNAL" + ReadSignalName + " " + args;
                ReadSignalNames.Add(ReadSignalName, fileName);
                ReadSignalNames.Add(fileName, ReadSignalName);
                ReadSignalList.Add(ReadSignalName);
            }
            if (redirectErrors)
            {
                //args = "CATCHUNHANDLEDEXCEPTIONS" +" "+ args;
            }*/
            if (Starting != null)
                Starting(fileName);
            try
            {
                PRunner.Start(fileName, args, redirectIO, redirectErrors, RunWithPause, attachDebugger, fictive_attach);
                if ((PRunner.TempBatFile != null) && !TempBatFiles.ContainsKey(fileName))
                    TempBatFiles.Add(fileName, PRunner.TempBatFile);
                if (redirectIO)
                {
                    EventedStreamReaderList.Add(PRunner.process.StandardOutput, OutputStreamId + fileName, InputEncoding);
                    PRunner.process.StandardInput.AutoFlush = true;
                }
                if (redirectErrors)
                {
                    EventedStreamReaderList.Add(PRunner.process.StandardError, ErrorStreamId + fileName, InputEncoding);
                }
            }
            catch(Exception)
            {
                RemoveFromTables(fileName);
                throw;
            }
        }

        private void Write_Callback(IAsyncResult ar)
        {
            StreamObject so = (StreamObject)ar.AsyncState;
            so.stream.EndWrite(ar);
            //if (OutputStringReceived != null)
            //    OutputStringReceived(so.id, StreamType.Output, so.text);
            so.stream.Flush();
        }
        class StreamObject
        {
            public Stream stream;
            public string text;
            public string id;
        }
        public void WritelnStringToProcess(string id, string data)
        {
            Utils.ProcessRunner pr = StartedProcesses[id] as Utils.ProcessRunner;
            /*string s=StartedProcesses.Count.ToString()+" ";
            foreach (string ss in StartedProcesses.Keys)
                s += ss + "|";
            if (pr == null) throw new Exception("pr==null, " + id + " " + data+" started process"+s);*/
            byte[] buffer = InputEncoding.GetBytes(data + pr.process.StandardInput.NewLine);
            StreamObject so=new StreamObject();
            so.stream = pr.process.StandardInput.BaseStream;
            so.text = data + Environment.NewLine;
            so.id = id;
            pr.process.StandardInput.BaseStream.BeginWrite(buffer, 0, buffer.Length, Write_Callback, so);
        }

        /*void PRunner_OutputLineReceived(object sender, global::VisualPascalABC.Utils.LineReceivedEventArgs e)
        {
            if (OutputStringReceived != null)
                OutputStringReceived((string)StartedFiles[sender], e.Line);
        }*/

        public void Run(string fileName, bool redirectIO, string ModeName, bool RunWithPause, string WorkingDirectory, bool attachDebugger, bool fictive_attach)
        {
            if (ModeName == null) 
                ModeName = string.Empty;
            Run(fileName, ModeName, redirectIO, redirectIO, RunWithPause, WorkingDirectory, attachDebugger, fictive_attach);
        }

        public void Run(string fileName, bool redirectIO, string ModeName, bool RunWithPause, string WorkingDirectory, string CommandLineArguments, bool attachDebugger, bool fictive_attach)
        {
            if (ModeName == null)
                ModeName = string.Empty;
            Run(fileName, ModeName+(string.IsNullOrEmpty(CommandLineArguments)?"":" "+CommandLineArguments), redirectIO, redirectIO, RunWithPause, WorkingDirectory, attachDebugger, fictive_attach);
        }

        void PRunner_ProcessExited(object sender, EventArgs e)
        {
            string fileName = (string)StartedFiles[sender];
            if (fileName == null) return;//TODO: это ошибочная ситуация
            if (!WorkbenchServiceFactory.Workbench.UserOptions.AlwaysAttachDebuggerAtStart)
                WorkbenchServiceFactory.Workbench.DebuggerManager.NullProcessHandleIfNeed(fileName);
            EventedStreamReaderList.Remove(fileName);
            RemoveFromTables(fileName);
            if (Exited != null)
                Exited(fileName);
        }
        
        void RemoveFromTables(string FileName)
        {
            if (StartedProcesses[FileName] != null)//TODO: это ошибочная ситуация
            {
                EventedStreamReaderList.Remove(FileName);
                StartedFiles.Remove(StartedProcesses[FileName]);
                StartedProcesses.Remove(FileName);
                /*if (ReadSignalNames.ContainsKey(FileName))
                {
                    ReadSignalList.Remove(ReadSignalNames[FileName]);
                    ReadSignalNames.Remove(FileName);
                }*/
            }
            //messageServer.Disconnect(FileName);
        }
        public void Stop(string fileName)
        {
            EventedStreamReaderList.Remove(fileName);
            if (Path.GetExtension((StartedProcesses[fileName] as Utils.ProcessRunner).process.StartInfo.FileName) == ".bat")
            {
                Process[] ps = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(fileName));
                foreach (Process p in ps)
                    if (Tools.FileNameToLower(p.Modules[0].FileName) == Tools.FileNameToLower(fileName))
                    {
                        p.Kill();
                        p.Close();
                        p.Dispose();
                        break;
                    }
            }
            (StartedProcesses[fileName] as Utils.ProcessRunner).Kill();
            if (!WorkbenchServiceFactory.Workbench.UserOptions.AlwaysAttachDebuggerAtStart)
                WorkbenchServiceFactory.Workbench.DebuggerManager.NullProcessHandleIfNeed(fileName);
            RemoveFromTables(fileName);
            if (Exited != null)
                Exited(fileName);
            
        }
    }
}
