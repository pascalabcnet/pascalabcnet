// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VisualPascalABCPlugins;

namespace VisualPascalABC
{
    public class WorkbenchServiceContainer: IWorkbenchServiceContainer
    {

        public IWorkbenchDocumentService DocumentService
        {
            get { return WorkbenchServiceFactory.DocumentService; }
        }

        public IWorkbenchOptionService OptionsService
        {
            get { return WorkbenchServiceFactory.OptionsService; }
        }

        public IWorkbenchUpdateService UpdateService
        {
            get { return WorkbenchServiceFactory.UpdateService; }
        }

        public IEditorService EditorService
        {
            get { return WorkbenchServiceFactory.EditorService; }
        }

        public IHelpService HelpService
        {
            get { return WorkbenchServiceFactory.HelpService; }
        }

        public IWorkbenchDebuggerOperationsService DebuggerOperationsService
        {
            get { return WorkbenchServiceFactory.DebuggerOperationsService; }
        }

        public IWorkbenchBuildService BuildService
        {
            get { return WorkbenchServiceFactory.BuildService; }
        }

        public IWorkbenchFileService FileService
        {
            get { return WorkbenchServiceFactory.FileService; }
        }

        public IWorkbenchProjectService ProjectService
        {
            get { return WorkbenchServiceFactory.ProjectService; }
        }

        public IWorkbenchRunService RunService
        {
            get { return WorkbenchServiceFactory.RunService; }
        }

        public IWorkbenchDesignerService DesignerService
        {
            get { return WorkbenchServiceFactory.DesignerService; }
        }

        public IWorkbenchOperationsService OperationsService
        {
            get { return WorkbenchServiceFactory.OperationsService; }
        }

        public ICodeCompletionService CodeCompletionService
        {
            get { return WorkbenchServiceFactory.CodeCompletionService; }
        }
    }

    public class WorkbenchServiceFactory
    {
        static IWorkbenchBuildService _buildService;
        static IWorkbenchRunService _runService;
        static IWorkbenchUpdateService _updateService;
        static CodeCompletionParserController _codeCompletionParserController;
        static DebugHelper _debuggerManager;

        public static IWorkbenchBuildService BuildService
        {
            get
            {
                if (_buildService == null)
                    _buildService = new WorkbenchBuildService();
                return _buildService;
            }
        }

        public static IWorkbenchRunService RunService
        {
            get
            {
                if (_runService == null)
                    _runService = new WorkbenchRunService();
                return _runService;
            }
        }

        public static IWorkbenchUpdateService UpdateService
        {
            get
            {
                if (_updateService == null)
                    _updateService = new WorkbenchUpdateService();
                return _updateService;
            }
        }

        public static IWorkbenchOptionService OptionsService
        {
            get
            {
                return VisualPABCSingleton.MainForm;
            }
        }

        public static IWorkbenchOperationsService OperationsService
        {
            get
            {
                return VisualPABCSingleton.MainForm;
            }
        }

        public static IEditorService EditorService
        {
            get
            {
                return VisualPABCSingleton.MainForm;
            }
        }

        public static IWorkbenchDocumentService DocumentService
        {
            get
            {
                return VisualPABCSingleton.MainForm;
            }
        }

        public static IWorkbenchDebuggerOperationsService DebuggerOperationsService
        {
            get
            {
                return VisualPABCSingleton.MainForm;
            }
        }

        public static IWorkbenchProjectService ProjectService
        {
            get
            {
                return VisualPABCSingleton.MainForm;
            }
        }

        public static IWorkbenchFileService FileService
        {
            get
            {
                return VisualPABCSingleton.MainForm;
            }
        }

        public static IHelpService HelpService
        {
            get
            {
                return VisualPABCSingleton.MainForm;
            }
        }

        public static IWorkbenchDesignerService DesignerService
        {
            get
            {
                return VisualPABCSingleton.MainForm;
            }
        }

        public static IWorkbench Workbench
        {
            get
            {
                return VisualPABCSingleton.MainForm;
            }
        }

        public static DebugHelper DebuggerManager
        {
            get
            {
                if (_debuggerManager == null)
                    _debuggerManager = new DebugHelper();
                return _debuggerManager;
            }
        }

        public static ICodeCompletionService CodeCompletionService
        {
            get
            {
                return CodeCompletionParserController;
            }
        }

        public static CodeCompletionParserController CodeCompletionParserController
        {
            get
            {
                if (_codeCompletionParserController == null)
                    _codeCompletionParserController = new CodeCompletionParserController();
                return _codeCompletionParserController;
            }
        }
    }

    public static class WorkbenchStorage
    {
        internal static string WorkingDirectory = null;
        internal static string WorkingDirectoryInOptionsFile = null;
        internal static string LibSourceDirectory = null;
        internal static Dictionary<string, string> StandartDirectories;
        internal static System.Threading.Thread MainProgramThread;
        internal static bool SetCurrentTabPageIfWriteToOutputWindow = false;
        internal static bool WorkingDirectoryExsist
        {
            get { return Directory.Exists(WorkingDirectory); }

        }
    }

}
