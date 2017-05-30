﻿// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace VisualPascalABC
{
    public partial class WorkbenchRunService : VisualPascalABCPlugins.IWorkbenchRunService
    {
        void RunnerManager_Exited(string fileName)
        {
            if (System.Threading.Thread.CurrentThread != WorkbenchStorage.MainProgramThread)
                Workbench.BeginInvoke(new SetTextDelegate(RunnerManager_Exited_Sync), fileName);
            else
                RunnerManager_Exited_Sync(fileName);
        }

        void RunnerManager_Started(string fileName)
        {
            Workbench.BeginInvoke(new SetTextDelegate(RunnerManager_Started_Sync), fileName);
        }

        void RunnerManager_Started_Sync(string fileName)
        {
            if (!ProjectFactory.Instance.ProjectLoaded)
            {
                fileName = Tools.FileNameToLower(fileName);
                if (Tools.FileNameToLower(Workbench.CurrentEXEFileName) == fileName)
                {
                    Workbench.WidgetController.SetStopEnabled(true);
                    Workbench.WidgetController.SetCompilingButtonsEnabled(false);
                    Workbench.WidgetController.SetDebugButtonsEnabled(false);
                    Workbench.WidgetController.SetOptionsEnabled(false);
                }
                RunTabs[fileName].Run = true;
                WorkbenchServiceFactory.DocumentService.SetTabPageText(RunTabs[fileName]);
            }
            else
            {
                fileName = Tools.FileNameToLower(fileName);
                Workbench.WidgetController.SetStopEnabled(true);
                Workbench.WidgetController.SetCompilingButtonsEnabled(false);
                Workbench.WidgetController.SetDebugButtonsEnabled(false);
                Workbench.WidgetController.SetOptionsEnabled(false);
                RunTabs[fileName].Run = true;
            }
        }

        void RunnerManager_Started_Sync_Thread(object fileName)
        {
            RunnerManager_Started_Sync(fileName as string);
        }

        void RunnerManager_OutputStringReceived(string fileName, RunManager.StreamType streamType, string text)
        {
            WorkbenchServiceFactory.OperationsService.AddTextToOutputWindowSync(fileName, text);
        }

        void RunnerManager_Exited_Sync(string fileName)
        {
            fileName = Tools.FileNameToLower(fileName);
            if (!RunTabs.ContainsKey(fileName))
            {
                string s = fileName + " ";
                foreach (string ss in RunTabs.Keys)
                    s += ss + " ";
                throw new Exception(s);
            }
            if (!ProjectFactory.Instance.ProjectLoaded)
            {
                if (Tools.FileNameToLower(WorkbenchServiceFactory.Workbench.CurrentEXEFileName) == fileName)
                {
                    Workbench.WidgetController.SetStopEnabled(false);
                    Workbench.WidgetController.SetCompilingButtonsEnabled(true);
                    Workbench.WidgetController.SetDebugButtonsEnabled(true);
                    //if (IsOneProgramStarted())
                        Workbench.WidgetController.SetOptionsEnabled(true);
                }
            }
            else
            {
                Workbench.WidgetController.SetStopEnabled(false);
                Workbench.WidgetController.SetCompilingButtonsEnabled(true);
                Workbench.WidgetController.SetDebugButtonsEnabled(true);
                //if (IsOneProgramStarted())
                    Workbench.WidgetController.SetOptionsEnabled(true);
            }
            RunTabs[fileName].Run = false;
            WorkbenchServiceFactory.DocumentService.SetTabPageText(RunTabs[fileName]);
            if (ReadRequests.ContainsKey(RunTabs[fileName]))
                ReadRequests.Remove(RunTabs[fileName]);
            UpdateReadRequest(false);
            WorkbenchServiceFactory.EditorService.SetFocusToEditor();
            if (TerminateAllPrograms)
                WaitCallback_DeleteEXEAndPDB(fileName);
            else
                System.Threading.ThreadPool.QueueUserWorkItem(WaitCallback_DeleteEXEAndPDB, fileName);
        }

        void RunnerManager_RunnerManagerUnhanledRuntimeException(string id, string ExceptionType, string ExceptionMessage, string StackTraceData, List<RunManager.StackTraceItem> StackTrace)
        {
            string localiseMsg = RuntimeExceptionsStringResources.Get(ExceptionMessage);
            WorkbenchServiceFactory.OperationsService.AddTextToOutputWindowSync(id, string.Format(Form1StringResources.Get("OW_RUNTIME_EXCEPTION{0}_MESSAGE{1}"), ExceptionType, localiseMsg) + Environment.NewLine);
            if (StackTraceData != null)
                WorkbenchServiceFactory.OperationsService.AddTextToOutputWindowSync(id, string.Format(Form1StringResources.Get("OW_RUNTIME_EXCEPTION_STACKTRACE{0}"), StackTraceData) + Environment.NewLine);
            RunManager.StackTraceItem ToSend = null;
            foreach (RunManager.StackTraceItem StackTraceItem in StackTrace)
            {
                if (StackTraceItem.SourceFileName != null)
                {
                    if (ToSend == null && File.Exists(StackTraceItem.SourceFileName))
                        ToSend = StackTraceItem;
                    if (Workbench.UserOptions.SkipStakTraceItemIfSourceFileInSystemDirectory && 
                        (Path.GetDirectoryName(StackTraceItem.SourceFileName) == WorkbenchServiceFactory.BuildService.CompilerOptions.SearchDirectory ||
                        Path.GetDirectoryName(StackTraceItem.SourceFileName) == WorkbenchServiceFactory.BuildService.CompilerOptions.SearchDirectory+"Source"))
                        continue;
                    if (!(bool)Workbench.VisualEnvironmentCompiler.SourceFilesProvider(StackTraceItem.SourceFileName, PascalABCCompiler.SourceFileOperation.Exists))
                    {
                        string fn = Path.Combine(WorkbenchStorage.LibSourceDirectory, Path.GetFileName(StackTraceItem.SourceFileName));
                        if (File.Exists(fn))
                            StackTraceItem.SourceFileName = fn;
                        else
                            continue;
                    }
                    ToSend = StackTraceItem;
                    break;
                }
            }
            if (ToSend != null)
            {
                List<PascalABCCompiler.Errors.Error> list = new List<PascalABCCompiler.Errors.Error>();
                list.Add(new RuntimeException(
                    string.Format(
                    Form1StringResources.Get("ERRORLIST_RUNTIME_EXCEPTION_MESSAGE{0}"),
                    localiseMsg.Replace(Environment.NewLine, " ")),
                    ToSend.SourceFileName,
                    0,
                    ToSend.LineNumber)
                    );
                Workbench.ErrorsListWindow.ShowErrorsSync(list, true);
                if (RunnerManager.Count > 0)
                {
                    RunnerManager.KillAll();
                }
            }
        }

        void WaitCallback_DeleteEXEAndPDB(object state)
        {
            int i = 0;
            string fileName = (string)state;
            while (i < 20)
            {
                try
                {
                    if (Workbench.UserOptions.DeleteEXEAfterExecute && File.Exists(fileName))
                        File.Delete(fileName);
                    if (Workbench.UserOptions.DeletePDBAfterExecute)
                    {
                        string pdbFileName = Path.ChangeExtension(fileName, ".pdb");
                        if (File.Exists(pdbFileName) && File.Exists(pdbFileName))
                            File.Delete(pdbFileName);
                    }
                    if (RunnerManager.TempBatFiles.ContainsKey(fileName))
                    {
                        string batname = RunnerManager.TempBatFiles[fileName];
                        if (File.Exists(batname))
                        {
                            File.Delete(batname);
                            RunnerManager.TempBatFiles.Remove(fileName);
                        }
                    }
                    return;
                }
                catch
                {
                }
                System.Threading.Thread.Sleep(20);
                i++;
            }
        }

        void ReadStringRequest(string ForId)
        {
            ForId = Tools.FileNameToLower(ForId);
            if (!ReadRequests.ContainsKey(RunTabs[ForId]))
            {
                if (WorkbenchServiceFactory.DebuggerManager.IsRun(ForId) && WorkbenchServiceFactory.DebuggerManager.CurPage != null)
                    ReadRequests.Add(WorkbenchServiceFactory.DebuggerManager.CurPage, "");
                else
                    ReadRequests.Add(RunTabs[ForId], "");
            }
            Workbench.BeginInvoke(new ReadStringRequestSyncDel(ReadStringRequestSync));
        }

        delegate void ReadStringRequestSyncDel();

        void ReadStringRequestSync()
        {
            UpdateReadRequest(false);
        }

        public void UpdateReadRequest(bool changeSelected)
        {
            if (changeSelected && Workbench.OutputWindow.InputPanelVisible)
            {
                ReadRequests[WorkbenchServiceFactory.DocumentService.LastSelectedTab] = Workbench.OutputWindow.InputTextBoxText;
            }
            if (ReadRequests.ContainsKey(WorkbenchServiceFactory.DocumentService.CurrentCodeFileDocument))
            {
                Workbench.OutputWindow.InputTextBoxText = ReadRequests[WorkbenchServiceFactory.DocumentService.CurrentCodeFileDocument];
                Workbench.OutputWindow.InputPanelVisible = true;
                Workbench.OutputWindow.InputTextBoxCursorToEnd();
                return;
            }
            Workbench.OutputWindow.InputPanelVisible = false;
        }

        bool IsOneProgramStarted()
        {
            int num = 0;
            foreach (VisualPascalABCPlugins.ICodeFileDocument doc in RunTabs.Values)
                if (doc.Run)
                    num++;
            return num == 1;
        }
    }
}
