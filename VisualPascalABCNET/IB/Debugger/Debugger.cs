// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using VisualPascalABCPlugins;
using VisualPascalABC.Utils;
using Debugger;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using System.Runtime.ExceptionServices;

namespace VisualPascalABC
{
    
	//Klass predostavljaushij informaciju ob otlazhivaemoj programme
	public class AssemblyHelper
    {
        private static System.Reflection.Assembly a;
        private static List<System.Reflection.Assembly> ref_modules = new List<System.Reflection.Assembly>();
        private static Hashtable ns_ht = new Hashtable();
        private static Hashtable stand_types = new Hashtable(StringComparer.OrdinalIgnoreCase);
        private static List<Type> unit_types = new List<Type>();//spisok tipov-obertok nad moduljami
        private static List<DebugType> unit_debug_types;//to zhe samoe, no tipy Debugger.Core
        private static DebugType pabc_system_type = null;

        static AssemblyHelper()
        {
        	stand_types["integer"] = typeof(int);
        	stand_types["byte"] = typeof(byte);
        	stand_types["shortint"] = typeof(sbyte);
        	stand_types["word"] = typeof(ushort);
        	stand_types["smallint"] = typeof(short);
        	stand_types["longint"] = typeof(int);
        	stand_types["longword"] = typeof(uint);
        	stand_types["int64"] = typeof(long);
        	stand_types["uint64"] = typeof(ulong);
        	stand_types["real"] = typeof(double);
        	stand_types["single"] = typeof(float);
        	stand_types["char"] = typeof(char);
        	stand_types["string"] = typeof(string);
        	stand_types["boolean"] = typeof(bool);
        	stand_types["object"] = typeof(object);
        }
        
        public static bool Is32BitAssembly()
        {
            if (!Environment.Is64BitProcess)
                return false;
            System.Reflection.PortableExecutableKinds peKind;
            System.Reflection.ImageFileMachine machine;
            a.ManifestModule.GetPEKind(out peKind, out machine);
            return peKind == System.Reflection.PortableExecutableKinds.Required32Bit;
        }

        public static void LoadAssembly(string file_name)
        {
        	//ad = AppDomain.CreateDomain("DebugDomain",null,Path.GetDirectoryName(file_name),Path.GetDirectoryName(file_name),false);
            try
            {
                FileStream fs = File.OpenRead(file_name);
                byte[] buf = new byte[fs.Length];
                fs.Read(buf, 0, (int)fs.Length);
                fs.Close();
                a = System.Reflection.Assembly.Load(buf);
                
                Type[] tt = a.GetTypes();
                foreach (Type t in tt)
                {
                    if (t.Namespace != null)
                        ns_ht[t.Namespace] = t.Namespace;
                    if (t.Namespace == t.Name)
                    {
                        unit_types.Add(t);
                    }
                    try
                    {
                        object[] attrs = t.GetCustomAttributes(false);
                        foreach (Attribute attr in attrs)
                        {
                            Type attr_t = attr.GetType();
                            if (attr_t.Name == "$UsedNsAttr")
                            {
                                int count = (int)attr_t.GetField("count").GetValue(attr);
                                string ns = attr_t.GetField("ns").GetValue(attr) as string;
                                int j = 0;
                                for (int i = 0; i < count; i++)
                                {
                                    byte str_len = (byte)ns[j];
                                    string ns_s = ns.Substring(j + 1, str_len);
                                    ns_ht[ns_s] = ns_s;
                                    j += str_len + 1;
                                }
                                break;
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }
            catch (System.Exception e)
            {

            }
        }

        [HandleProcessCorruptedStateExceptionsAttribute]
        public static bool IsDebuggerStepThrough(Function f)
        {
            if (f == null)
                return false;
            try
            {
                System.Reflection.MethodBase mb = a.ManifestModule.ResolveMethod((int)f.Token);
                if (mb != null)
                    return mb.GetCustomAttributes(typeof(System.Diagnostics.DebuggerStepThroughAttribute), false).Length > 0;
            }
            catch
            {
            }
            return false;
        }


        public static DebugType GetPABCSystemType()
        {
            if (pabc_system_type == null)
            {
                string file_name = PascalABCCompiler.Compiler.GetReferenceFileName("PABCRtl.dll");
                System.Reflection.Assembly assm = PascalABCCompiler.NetHelper.NetHelper.LoadAssembly(file_name);
                PascalABCCompiler.NetHelper.NetHelper.init_namespaces(assm);
                pabc_system_type = DebugUtils.GetDebugType(PascalABCCompiler.NetHelper.NetHelper.PABCSystemType);
            }
            return pabc_system_type;
        }

        public static Type GetTypeForStatic(string name)
        {
            Type t = stand_types[name] as Type;
            if (t != null) return t;
            if (t == null)
                foreach (string s in ns_ht.Keys)
                {
                    t = a.GetType(s + "." + name, false, true);
                    if (t != null)
                        return t;
                }
            t = PascalABCCompiler.NetHelper.NetHelper.FindType(name);
            if (t == null) t = PascalABCCompiler.NetHelper.NetHelper.FindType("System." + name);
            if (t == null)
                foreach (string s in ns_ht.Keys)
                {
                    t = PascalABCCompiler.NetHelper.NetHelper.FindType(s + "." + name);
                    if (t != null)
                        break;
                }
            return t;
        }
        
        public static Type GetTypeForStatic(string name, List<Type> gen_types)
        {
        	Type t = null;
        	foreach (string s in ns_ht.Keys)
            {
            	t = a.GetType(s+"."+name+"`"+gen_types.Count,false,true);
            	if (t != null)
            	{
            		//t = t.MakeGenericType(gen_types.ToArray());
            		return t;
            	}
            }
        	t = PascalABCCompiler.NetHelper.NetHelper.FindType(name+"`"+gen_types.Count);
            if (t == null) t = PascalABCCompiler.NetHelper.NetHelper.FindType("System."+name+"`"+gen_types.Count);
            if (t == null)
            foreach (string s in ns_ht.Keys)
            {
            	t = PascalABCCompiler.NetHelper.NetHelper.FindType(s+"."+name+"`"+gen_types.Count);
            	if (t != null)
            		break;
            }
            
            //if (t != null)
            //	t = t.MakeGenericType(gen_types.ToArray());
            return t;
        }
        
        public static Type GetType(string name)
        {
        	Type t = stand_types[name] as Type;
        	if (t != null) return t;
        	t = a.GetType(name,false,true);
            if (t == null)
            foreach (string s in ns_ht.Keys)
            {
            	t = a.GetType(s+"."+name,false,true);
            	if (t != null) break;
            }
            if (t == null) t = PascalABCCompiler.NetHelper.NetHelper.FindType(name);
            if (t == null) t = PascalABCCompiler.NetHelper.NetHelper.FindType("System."+name);
            if (t == null)
            foreach (string s in ns_ht.Keys)
            {
            	t = PascalABCCompiler.NetHelper.NetHelper.FindType(s+"."+name);
            	if (t != null)
            		break;
            }
//            if (t == null) t = Type.GetType(name,false,true);
//            if (t == null) t = Type.GetType("System."+name,false,true);
//            if (t == null)
//            foreach(System.Reflection.Assembly ass in ref_modules)
//            {
//            	t = ass.GetType(name);
//            	if (t != null)
//            		break;
//            }
            return t;
        }
        
        public static List<DebugType> GetUsesTypes(Process p, DebugType dt)
        {
        	if (unit_debug_types == null)
        	{
        		unit_debug_types = new List<DebugType>();
        		foreach (Type t in unit_types)
        			unit_debug_types.Add(DebugType.Create(p.GetModule(t.Assembly.ManifestModule.ScopeName),(uint)t.MetadataToken));
        	}
        	return unit_debug_types;
        }
        
        public static void Unload()
        {
            //if (ad != null) AppDomain.Unload(ad);
            //GC.Collect();
            ns_ht.Clear();
            unit_types.Clear();
            if (unit_debug_types != null)
            unit_debug_types.Clear();
            unit_debug_types = null;
        }
    }
	
	public class ProcessDelegator : IProcess
	{
		private Process process;
		
		public ProcessDelegator(Process process)
		{
			this.process = process;
		}
		
		public bool HasExited
		{
			get
			{
				return process.HasExited;
			}
		}
	}
	
	public class ProcessEventArgsDelegator : EventArgs, IProcessEventArgs
	{
		private ProcessDelegator process;
		
		public ProcessEventArgsDelegator(ProcessDelegator process)
		{
			this.process = process;
		}
		
		public IProcess Process
		{
			get
			{
				return process;
			}
		}
	}
	
    /// <summary>
    /// Класс для отладки программ
    /// </summary>
    public class DebugHelper : IDebuggerManager
    {
        private NDebugger dbg;
        private Process debuggedProcess;
        private IWorkbench workbench;
        private string FileName;
        private string FullFileName;
        private string PrevFullFileName;
        private Breakpoint brPoint;
        private Breakpoint currentBreakpoint;
        private int CurrentLine;
        public DebugStatus Status;
        private bool MustDebug = false;
        public bool IsRunning = false;
        public string ExeFileName;
		public bool ShowDebugTabs=true;
		public PascalABCCompiler.Parsers.IParser parser = null;
		EventHandler<EventArgs> debuggerStateEvent;
		
        public DebugHelper()
        {
            //System.Threading.Thread.CurrentThread.SetApartmentState(System.Threading.ApartmentState.MTA);
            dbg = new NDebugger();
            //th = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(frm.RefreshPad));
            this.workbench = WorkbenchServiceFactory.Workbench;
        }

        public Process DebuggedProcess
        {
            get
            {
                return debuggedProcess;
            }
        }
        
        event EventHandler<EventArgs> IDebuggerManager.DebuggeeStateChanged
       	{
        	add
        	{
            	debuggerStateEvent += value;
        	}

        	remove
        	{
            	debuggerStateEvent -= value;
        	}
    	}
        
        public Breakpoint CurrentBreakpoint
        {
            get { return currentBreakpoint; }
            set { currentBreakpoint = value; }
        }

        /// <summary>
        /// Добавить Breakpoint в файл fileName строку line
        /// </summary>
        public Breakpoint AddBreakPoint(string fileName, int line, bool commonBreakpoint)
        {
            Breakpoint br = null;
            bool added = false;
            if (!workbench.UserOptions.AlwaysAttachDebuggerAtStart && handle != 0 && commonBreakpoint && this.debuggedProcess == null)
            {
            	Attach(handle, ExeFileName, true,true);
            }
            foreach (Breakpoint bp in dbg.Breakpoints)
            {
                if (bp.SourcecodeSegment.SourceFullFilename == fileName && bp.SourcecodeSegment.StartLine == line)
                {
                    added = true;
                    br = bp;
                }
            }
            if (!added)
                br = dbg.AddBreakpoint(fileName, line);
            return br;
        }
		
        /// <summary>
        /// Получить список Breakpointov в файле fileName
        /// </summary>
        public List<Breakpoint> GetBreakpointsInFile(string fileName)
        {
            List<Breakpoint> lst = new List<Breakpoint>();
            foreach (Breakpoint bp in dbg.Breakpoints)
            {
                if (bp.SourcecodeSegment.SourceFullFilename == fileName)
                {
                    lst.Add(bp);
                }
            }
            return lst;
        }
		
        /// <summary>
        /// Удалить Breakpoint
        /// </summary>
        public void RemoveBreakpoint(Breakpoint br)
        {
            dbg.RemoveBreakpoint(br);
        }

        public delegate void DebugHelperActionDelegate(string FileName);
        public event DebugHelperActionDelegate Exited;
        public event DebugHelperActionDelegate Starting;
		
		/// <summary>
		/// Запуск процесса на отладку
		/// </summary>
		/// <param name="fileName">Имя ехе-файла</param>
		/// <param name="sourceFileName">Имя исходного файла</param>
		/// <param name="workingDirectory">Каталог файла</param>
		/// <param name="arguments">Аргументы командной строки</param>
		/// <param name="need_first_brpt">Флаг, нужно ли устанавливать Breakpoint на первом операторе (F8, F7)</param>
		/// <param name="redirectOutput">Временно не используется</param>
        public void Start(string fileName, string sourceFileName, string workingDirectory, string arguments, bool need_first_brpt, bool redirectOutput)
        {
            if (Starting != null)
                Starting(fileName);
            //dbg = new NDebugger();
            dbg.ProcessStarted += debugProcessStarted;
            dbg.ProcessExited += debugProcessExit;
            dbg.BreakpointHit += debugBreakpointHit;
            //if (brPoint != null) dbg.RemoveBreakpoint(brPoint);
            this.FileName = sourceFileName;//Path.GetFileNameWithoutExtension(fileName) + ".pas";
            this.FullFileName = Path.Combine(Path.GetDirectoryName(fileName), this.FileName);
            this.ExeFileName = fileName;
            this.PrevFullFileName = FullFileName;
            if (need_first_brpt) brPoint = dbg.AddBreakpoint(FileName, workbench.VisualEnvironmentCompiler.Compiler.BeginOffset);
            AssemblyHelper.LoadAssembly(fileName);
            debuggedProcess = dbg.Start(fileName, workingDirectory, arguments);
            SelectProcess(debuggedProcess);
        }
		
        private uint handle=0;
        
        public void NullProcessHandleIfNeed(string fileName)
        {
        	if (string.Compare(fileName,this.ExeFileName,true)==0)
        	{
        		FileName = null;
        		handle = 0;
        		if (debuggedProcess == null)
        		{
        			IsRunning = false;
        			ExeFileName = null;
        			WorkbenchServiceFactory.OperationsService.ClearTabStack();
        			WorkbenchServiceFactory.DebuggerOperationsService.ClearDebugTabs();
                    WorkbenchServiceFactory.Workbench.DisassemblyWindow.ClearWindow();
        		}
        	}
        }
        
        public void Attach(uint handle, string fileName, bool real_attach, bool late_attach)
        {
        	if (!real_attach)
        	{
        		this.handle = handle;
        		this.ExeFileName = fileName;
        		IsRunning = true;
        		return;
        	}
        	if (handle == 0)
        		return;
        	if (Starting != null)
                Starting(fileName);
        	
        	string sourceFileName = null;
        	if (!ProjectFactory.Instance.ProjectLoaded)
        	    sourceFileName = workbench.VisualEnvironmentCompiler.Compiler.CompilerOptions.SourceFileName;
        	else
        	    sourceFileName = ProjectFactory.Instance.CurrentProject.MainFile;
        	this.FileName = sourceFileName;//Path.GetFileNameWithoutExtension(fileName) + ".pas";
            this.FullFileName = Path.Combine(Path.GetDirectoryName(fileName), this.FileName);
            this.ExeFileName = fileName;
            CurrentLine = 0;
            this.parser = CodeCompletion.CodeCompletionController.ParsersController.selectParser(Path.GetExtension(FullFileName).ToLower());
            this.PrevFullFileName = FullFileName;
            AssemblyHelper.LoadAssembly(fileName);
        	dbg.ProcessStarted += debugProcessStarted;
            dbg.ProcessExited += debugProcessExit;
            //brPoint = dbg.AddBreakpoint(FileName, frm.VisualEnvironmentCompiler.Compiler.BeginOffset);
            try
            {
                debuggedProcess = dbg.DebugActiveProcess(handle, fileName);
            }
            catch (System.Exception ex)
            {
                return;
            }
        	SelectProcess(debuggedProcess);
        	//debuggedProcess.Break();
        }

        Function currentFunction;
        Dictionary<int, int> sourceMap;

        void debuggedProcess_DebuggeeStateChanged(object sender, ProcessEventArgs e)
        {
            if (currentBreakpoint != null)
            {
            	dbg.RemoveBreakpoint(currentBreakpoint);
                RemoveGotoBreakpoints();
                currentBreakpoint = null; //RemoveBreakpoints();
            }
            JumpToCurrentLine();
            //ChangeLocalVars(e.Process);
            //if (e.Process.IsPaused)
            WorkbenchServiceFactory.DebuggerOperationsService.RefreshPad(new FunctionItem(e.Process.SelectedFunction).SubItems);
            workbench.WidgetController.SetStartDebugEnabled();
            if (currentFunction != e.Process.SelectedFunction)
            {
                if (WorkbenchServiceFactory.Workbench.DisassemblyWindow.IsVisible)
                {
                    var tp = GetNativeCodeOfSelectedFunction(e.Process.SelectedFunction);
                    WorkbenchServiceFactory.DebuggerOperationsService.DisplayDisassembledCode(tp.Item1);
                    sourceMap = tp.Item2;
                    currentFunction = e.Process.SelectedFunction;
                }
            }
            if (debuggerStateEvent != null)
            	debuggerStateEvent(this, new ProcessEventArgsDelegator(new ProcessDelegator(this.debuggedProcess)));
        }
		
        private void debugBreakpointHit(object sender, BreakpointEventArgs e)
        {
        	
        }
        
        public void SetFirstBreakpoint(string fileName, int line)
        {
        	brPoint = dbg.AddBreakpoint(fileName, line);
        }
        
        private void SelectProcess(Debugger.Process process)
        {
            if (debuggedProcess != null)
            {
                //debuggedProcess.DebuggingPaused -= debuggedProcess_DebuggingPaused;
                //debuggedProcess.ExceptionThrown -= debuggedProcess_ExceptionThrown;
                debuggedProcess.DebuggeeStateChanged -= debuggedProcess_DebuggeeStateChanged;
                debuggedProcess.DebuggingResumed -= debuggedProcess_DebuggingResumed;
                debuggedProcess.DebuggingPaused -= debuggedProcess_DebuggingPaused;
                debuggedProcess.ExceptionThrown -= debuggedProcess_ExceptionThrown;
                debuggedProcess.Expired -= debuggedProcess_Expired;
                //debuggedProcess.LogMessage -= debuggedProcess_logMessage;
            }
            debuggedProcess = process;
            if (debuggedProcess != null)
            {
                //debuggedProcess.DebuggingPaused += debuggedProcess_DebuggingPaused;
                //debuggedProcess.ExceptionThrown += debuggedProcess_ExceptionThrown;
                debuggedProcess.DebuggeeStateChanged += debuggedProcess_DebuggeeStateChanged;
                debuggedProcess.DebuggingResumed += debuggedProcess_DebuggingResumed;
                debuggedProcess.DebuggingPaused += debuggedProcess_DebuggingPaused;
                debuggedProcess.ExceptionThrown += debuggedProcess_ExceptionThrown;
                debuggedProcess.Expired += debuggedProcess_Expired;
                //debuggedProcess.LogMessage += debuggedProcess_logMessage;
            }
            JumpToCurrentLine();
            OnProcessSelected(new ProcessEventArgs(process));
        }
		
        private void debuggerTraceMessage(object sender, MessageEventArgs e)
        {
        	//frm.WriteToOutputBox(e.Message);
        }
        
        private void debuggedProcess_logMessage(object sender, MessageEventArgs e)
        {
            WorkbenchServiceFactory.OperationsService.WriteToOutputBox(e.Message, false);
        	//debuggedProcess.TraceMessage("kkkk");
        }
        
        private void debuggedProcess_Expired(object sender, EventArgs e)
        {
            workbench.WidgetController.SetStartDebugEnabled();
        }

        private void debuggedProcess_ExceptionThrown(object sender, ExceptionEventArgs e)
        {
        	if (e.Exception.Message == "System.StackOverflowException")
                WorkbenchServiceFactory.OperationsService.WriteToOutputBox(RuntimeExceptionsStringResources.Get("Process is terminated due to StackOverflowException."), true);
        	else
                WorkbenchServiceFactory.OperationsService.WriteToOutputBox(e.Exception.Message, true);
        }

        private void debuggedProcess_DebuggingPaused(object sender, ProcessEventArgs e)
        {
        	if (MustDebug && e.Process.PausedReason != PausedReason.Exception)
            {
                //System.Threading.Thread.Sleep(10);
                if (Status == DebugStatus.StepOver)
                    e.Process.StepOver();
                else
                    e.Process.StepInto();
                MustDebug = false;
            }
            workbench.WidgetController.SetDebugPausedDisabled();
        }

        protected virtual void OnProcessSelected(ProcessEventArgs e)
        {
        }

        void RemoveBreakpoints()
        {
            if (brPoint != null)
            {
                dbg.RemoveBreakpoint(brPoint);
                brPoint = null;
            }
        }

        void debugProcessExit(object sender, ProcessEventArgs e)
        {
            if (Exited != null && ExeFileName != null)
                Exited(ExeFileName);
            curPage = null;            
            ShowDebugTabs = true;
            workbench.WidgetController.SetPlayButtonsVisible(false);
            workbench.WidgetController.SetAddExprMenuVisible(false);
            workbench.WidgetController.SetDisassemblyMenuVisible(false);
            DebugWatchListWindowForm.WatchWindow.ClearAllSubTrees();
            IsRunning = false;
            workbench.WidgetController.SetDebugStopDisabled();
            workbench.WidgetController.ChangeContinueDebugNameOnStart();
            CurrentLineBookmark.Remove();
            WorkbenchServiceFactory.DebuggerOperationsService.ClearLocalVarTree();
            WorkbenchServiceFactory.DebuggerOperationsService.ClearDebugTabs();
            WorkbenchServiceFactory.DebuggerOperationsService.ClearWatch();
            WorkbenchServiceFactory.Workbench.DisassemblyWindow.ClearWindow();
            workbench.WidgetController.SetDebugTabsVisible(false);
            WorkbenchServiceFactory.OperationsService.ClearTabStack();
            workbench.WidgetController.EnableCodeCompletionToolTips(true);
            RemoveGotoBreakpoints();
            AssemblyHelper.Unload();
            CloseOldToolTip();
            //RemoveMarker(frm.CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.Document);
            evaluator = null;
            parser= null;
            FileName = null;
            handle = 0;
            ExeFileName = null;
            FullFileName = null;
            PrevFullFileName = null;
            Status = DebugStatus.None;
            dbg.ProcessStarted -= debugProcessStarted;
            dbg.ProcessExited -= debugProcessExit;
            dbg.BreakpointHit -= debugBreakpointHit;
            debuggedProcess = null;
            //GC.Collect();
            //dbg = null;
        }

        void debuggedProcess_DebuggingResumed(object sender, ProcessEventArgs e)
        {
            //CurrentLineBookmark.Remove();
            //frm.SetDebugStopDisabled();
        }
		
        public ExpressionEvaluator evaluator;
        
        void debugProcessStarted(object sender, ProcessEventArgs e)
        {
            workbench.WidgetController.SetDebugTabsVisible(true);
            workbench.WidgetController.SetPlayButtonsVisible(true);
            workbench.WidgetController.SetDebugStopEnabled();//e.Process.LogMessage += messageEventProc;
            workbench.WidgetController.SetStartDebugDisabled();
            workbench.WidgetController.ChangeStartDebugNameOnContinue();
            workbench.WidgetController.EnableCodeCompletionToolTips(false);
            workbench.WidgetController.SetAddExprMenuVisible(true);
            workbench.WidgetController.SetDisassemblyMenuVisible(true);
            TooltipServiceManager.hideToolTip();
            IsRunning = true;
            evaluator = new ExpressionEvaluator(e.Process,workbench.VisualEnvironmentCompiler, FileName);
        }
		
        /// <summary>
        /// Вычисляет значение выражения (WATCH)
        /// </summary>
        public RetValue Evaluate(string expr)
        {
        	if (evaluator != null)
        	return evaluator.Evaluate(expr, false);
        	else return new RetValue();
        }

        private NamedValue GetNullBasedArray(Value val)
        {
            IList<FieldInfo> flds = val.Type.GetFields();
            if (flds.Count != 3) return null;
            foreach (FieldInfo fi in flds)
                if (fi.Name == "NullBasedArray") return fi.GetValue(val);
            return null;
        }
        
        /// <summary>
        /// Возвращает текстовое представление для значения val
        /// </summary>
        public string MakeValueView(Value val)
        {
            bool by_ref = false;
            if ((val.Type.IsByRef() && !val.Type.IsPrimitive))
            {
                by_ref = true;
                val = val.Dereference;
            }
            int v = 0;
            NamedValue nv = GetNullBasedArray(val);//mozhet eto massiv
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (nv != null)//staticheskij massiv
            {
                sb.Append('(');
                NamedValueCollection nvc = nv.GetArrayElements();
                for (int i = 0; i < nvc.Count; i++)
                {
                    if (i > 10)
                    {
                        sb.Append("...");
                        break;
                    }
                    sb.Append(MakeValueView(nvc[i]));
                    if (i < nvc.Count - 1) sb.Append(',');
                }
                sb.Append(')');
                //sb.AppendLine();
                //sb.Remove(
            }
            else if (val.IsPrimitive)
            {
                if (val.Type.ManagedType == typeof(string) || val.Type.ManagedType == typeof(char))
                    sb.Append("'" + val.AsString + "'");
                else
                    sb.Append(val.AsString);
            }
            else if (val.IsNull)
            {
                sb.Append("nil");
            }
            else if (DebugUtils.IsEnum(val, out v))
            {
                sb.Append(ValueItem.MakeEnumText(val));
            }
            else if (val.Type.IsValueType && val.Type.GetMembers().Count <= 7)
            {
                sb.Append('{');
                NamedValueCollection nvc = val.GetMembers();
                for (int i = 0; i < nvc.Count; i++)
                {
                    sb.Append(nvc[i].Name + ": " + MakeValueView(nvc[i]));
                    if (i < nvc.Count - 1) sb.Append("; ");
                }
                sb.Append('}');
            }
            else if (val.IsArray)//dinamicheskij massiv
            {
                NamedValueCollection nvc = val.GetArrayElements();
                sb.Append('(');
                for (int i = 0; i < nvc.Count; i++)
                {
                    if (i > 10)
                    {
                        sb.Append("...");
                        break;
                    }
                    sb.Append(MakeValueView(nvc[i]));
                    if (i < nvc.Count - 1) sb.Append(',');
                }
                sb.Append(')');
                //sb.AppendLine();
            }
            else if (val.IsObject)
            {
                if (val.Type.FullName == "PABCSystem.TypedSet")
                    sb.Append((val.Type.GetMember("ToString", BindingFlags.All)[0] as MethodInfo).Invoke(val, new Value[0] { }).AsString);
                else
                    if (!val.Type.IsPrimitive)
                    {
                        if (DebugUtils.CheckForCollection(val.Type))
                            return DebugUtils.MakeViewForCollection(val);//esli eto kollekcija, to vyvodim ee po osobomu 
                        bool is_failed = false;
                        string s = ExpressionEvaluator.GetToString(val, out is_failed);
                        if (s != null) return s;
                        else return val.AsString;
                    }
                    else
                        return val.AsString;
            }
            else return val.AsString;
            return sb.ToString();
        }
        
        private void OnAddBreakpoint(object sender, EventArgs e)
        {

        }

        private TextArea curTextArea = null;
		private ICodeFileDocument curPage;
		
        private bool is_out = false;
        private SourcecodeSegment stepin_stmt = null;
		private bool tab_changed=false;
		
        /// <summary>
        /// Перейти к следующей строке при отладке
        /// </summary>
        private void JumpToCurrentLine()
        {
            workbench.MainForm.Activate();
            CurrentLineBookmark.Remove();//udalajem zheltyj kursor otladki
            //System.Threading.Thread.Sleep(70);
            if (debuggedProcess != null)
            {
                SourcecodeSegment nextStatement = debuggedProcess.NextStatement;
                //debuggedProcess.Modules[0].SymReader.GetMethod(debuggedProcess.SelectedFunction.Token);
                if (nextStatement != null)
                {
                    string save_PrevFullFileName = PrevFullFileName;
                    //CodeFileDocumentControl page = null;
                    //DebuggerService.JumpToCurrentLine(nextStatement.SourceFullFilename, nextStatement.StartLine, nextStatement.StartColumn, nextStatement.EndLine, nextStatement.EndColumn);
                    if (!ShowDebugTabs)//esli eshe ne pokazany watch i lokal, pokazyvaem
                    {
                        ShowDebugTabs = true;
                        workbench.WidgetController.SetDebugTabsVisible(true);
                        workbench.WidgetController.SetAddExprMenuVisible(true);
                    }
                    if (debuggedProcess.NextStatement.StartLine == 0xFFFFFF)
                    {
                        //sjuda popadem, esli vyzyvaem Test(arr), gde arr - massiv peredavaemyj po znacheniju
                        //metod $Copy ne imeet sequence pointov, poetomu chtoby vojti v proceduru vyzovy $Copy my pomechaem 0xFFFFFFF
                        //dolgo s etoj badjagoj muchalsja
                        is_out = true;
                        debuggedProcess.StepInto();//vhodim neposredstvenno v proceduru
                        return;
                    }
                    else if (AssemblyHelper.IsDebuggerStepThrough(debuggedProcess.SelectedFunction))
                    {
                        MustDebug = true;
                        debuggedProcess.StepOut();
                        
                        return;
                    }
                    else if (is_out)
                    {
                        is_out = false;
                        if (stepin_stmt.SourceFullFilename == debuggedProcess.NextStatement.SourceFullFilename && stepin_stmt.StartLine != debuggedProcess.NextStatement.StartLine)
                            stepin_stmt = debuggedProcess.NextStatement;
                        else
                        {
                            debuggedProcess.StepInto();
                            return;
                        }
                    }
                    if (debuggedProcess.NextStatement.SourceFullFilename != this.PrevFullFileName || !WorkbenchServiceFactory.DocumentService.ContainsTab(debuggedProcess.NextStatement.SourceFullFilename))
                    {
                        tab_changed = true;
                        curPage = WorkbenchServiceFactory.FileService.OpenFileForDebug(debuggedProcess.NextStatement.SourceFullFilename);
                        if (curPage == null)
                        {
                            MustDebug = true;
                            return;
                        }
                        PrevFullFileName = debuggedProcess.NextStatement.SourceFullFilename;
                    }
                    else if (curPage != WorkbenchServiceFactory.DocumentService.CurrentCodeFileDocument)
                        WorkbenchServiceFactory.DocumentService.CurrentCodeFileDocument = curPage;
                    bool remove_breakpoints = true;
                    IDocument doc = curPage.TextEditor.Document;
                    curTextArea = curPage.TextEditor.ActiveTextAreaControl.TextArea;
                    LineSegment lseg = curPage.TextEditor.ActiveTextAreaControl.Document.GetLineSegment(debuggedProcess.NextStatement.StartLine - 1);
                    int len = lseg.Length + 1;
                    /*if (lseg.Words.Count == 1 && string.Compare(lseg.Words[0].Word,"begin",true)==0)
                    {
                    	debuggedProcess.StepOver();
                        return;
                    }*/
                    //eto badjaga prepjatstvuet popadaniju na begin
                    //luchshego sposoba naverno net
                    bool in_comm = false;
                    bool beg = false;
                    bool in_str = false;
                    for (int i = 0; i < lseg.Words.Count; i++)
                    {
                        if (lseg.Words[i].Type == TextWordType.Word)
                        {
                            string word = lseg.Words[i].Word;
                            if (string.Compare(word, "begin", true) == 0 && !in_str && !in_comm)
                                beg = true;
                            else if (string.Compare(word, "{") == 0)
                            {
                                if (!in_str) in_comm = true;
                            }
                            else if (string.Compare(word, "}") == 0)
                            {
                                if (!in_str && in_comm) in_comm = false;
                            }
                            else if (string.Compare(word, "'") == 0)
                                in_str = !in_str;
                            else if (string.Compare(word, "/") == 0)
                            {
                                if (i < lseg.Words.Count - 1 && string.Compare(lseg.Words[i + 1].Word, "/") == 0)
                                    break;
                            }
                            else if (string.Compare(word, "(") == 0)
                            {
                                if (i < lseg.Words.Count - 1 && string.Compare(lseg.Words[i + 1].Word, "*") == 0)
                                {
                                    beg = false;
                                    break;
                                }
                            }
                            else if (!in_str && !in_comm)
                            {
                                beg = false;
                                break;
                            }
                        }
                    }
                    if (beg)
                    {
                        if (debuggedProcess.PausedReason != PausedReason.Exception)
                            debuggedProcess.StepOver();
                        return;
                    }
                    if (debuggedProcess.PausedReason == PausedReason.Breakpoint)
                    {
                        Breakpoint br = dbg.GetBreakpoint(debuggedProcess.NextStatement.SourceFullFilename, nextStatement.StartLine);
                        if (br != null && !BreakPointFactory.MustHit(br))
                        {
                            if (Status == DebugStatus.StepOver || Status == DebugStatus.StepIn)
                            {
                                remove_breakpoints = false;
                            }
                            else
                            {
                                debuggedProcess.Continue();
                                return;
                            }
                        }
                    }
                    //for (int i=0; i<lseg.Words.Count; i++)
                    CurrentLineBookmark.SetPosition(debuggedProcess.NextStatement.SourceFilename, curPage.TextEditor.Document, nextStatement.StartLine, 1, nextStatement.StartLine,
                       len);
                    curPage.TextEditor.ActiveTextAreaControl.JumpTo(nextStatement.StartLine - 1, 0);

                    if ((Status == DebugStatus.StepOver || Status == DebugStatus.StepIn) && (CurrentLine == nextStatement.StartLine && save_PrevFullFileName == debuggedProcess.NextStatement.SourceFilename))
                    {
                        if (curILOffset != nextStatement.ILOffset)
                        {
                            curILOffset = nextStatement.ILOffset;
                            //debuggedProcess.StepOver();
                            MustDebug = true;
                        }
                        CurrentLine = nextStatement.StartLine;
                        //return;
                    }
                    else
                    {
                        curILOffset = nextStatement.ILOffset;
                        CurrentLine = nextStatement.StartLine;
                        MustDebug = false;
                        //MustDebug = ((nextStatement.ILOffset + 1) == nextStatement.ILEnd);
                    }
                    //MustDebug = false;
                    //if (remove_breakpoints)
                    {
                        RemoveBreakpoints();
                        if (currentBreakpoint != null)
                        {
                            dbg.RemoveBreakpoint(currentBreakpoint);
                            currentBreakpoint = null; //RemoveBreakpoints();
                        }
                    }
                }

            }
        }

        private int curILOffset = 0;

        struct COR_DEBUG_IL_TO_NATIVE_MAP
        {
            public uint ilOffset;
            public uint nativeStartOffset;
            public uint nativeEndOffset;
        }

        public Tuple<string, Dictionary<int, int>> GetNativeCodeOfSelectedFunction(Function selectedFunction=null)
        {
            if (selectedFunction == null)
                selectedFunction = debuggedProcess.SelectedFunction;
            uint size = selectedFunction.CorFunction.NativeCode.Size;
            byte[] buffer = new byte[size];
            uint mapSize;
            COR_DEBUG_IL_TO_NATIVE_MAP[] mapBuffer = new COR_DEBUG_IL_TO_NATIVE_MAP[selectedFunction.CorFunction.ILCode.Size];
            unsafe
            {
                fixed (byte* ptr = &buffer[0])
                {
                    IntPtr iptr = new IntPtr((void*)ptr);
                    selectedFunction.CorFunction.NativeCode.GetCode(0, size, size, iptr);
                }
                fixed (void* ptr = &mapBuffer[0])
                {
                    IntPtr iptr = new IntPtr(ptr);
                    selectedFunction.CorFunction.NativeCode.GetILToNativeMapping((uint)mapBuffer.Length, out mapSize, iptr);
                } 
            }
            Dictionary<uint, uint> il2asm = new Dictionary<uint, uint>();
            for (int i = 0; i < mapSize; i++)
            {
                il2asm.Add(mapBuffer[i].nativeStartOffset, mapBuffer[i].ilOffset);
            }
            SharpDisasm.Disassembler disasm = new SharpDisasm.Disassembler(buffer, AssemblyHelper.Is32BitAssembly()?SharpDisasm.ArchitectureMode.x86_32: SharpDisasm.ArchitectureMode.x86_64);
            IEnumerable<SharpDisasm.Instruction> instructions = disasm.Disassemble();
            StringBuilder sb = new StringBuilder();
            int linesNum = 0;
            Dictionary<int, int> sourceMap = new Dictionary<int, int>();
            Dictionary<int, int> linesDict = new Dictionary<int, int>();
            bool firstLine = true;
            foreach (var instruction in instructions)
            {
                linesNum++;

                uint ilOffset;
                if (il2asm.TryGetValue((uint)instruction.Offset, out ilOffset))
                {
                    var segment = selectedFunction.GetSegmentForOffet(ilOffset);
                    
                    if (segment != null && !linesDict.ContainsKey(segment.StartLine))
                    {
                        if (ilOffset > 2000000000)
                        {
                            if (firstLine)
                                continue;
                        }
                        else
                            firstLine = false;
                        int line = segment.StartLine;
                        string text = curPage.TextEditor.Document.GetText(curPage.TextEditor.Document.GetLineSegment(line - 1));
                        sb.AppendLine((segment.StartLine+":").PadRight(10, ' ') + text.Trim());
                        linesDict.Add(segment.StartLine, segment.StartLine);
                    }
                }
                sb.Append("///"+selectedFunction.CorFunction.NativeCode.Address.ToString("X") + "+0x"+instruction.Offset.ToString("X") + ": " + instruction.ToString());
                sb.AppendLine();

            }
            return new Tuple<string, Dictionary<int, int>>(sb.ToString(),sourceMap);
        }

        public void DoInPausedState(MethodInvoker action)
        {
            Debugger.Process process = debuggedProcess;
            if (process.IsPaused)
            {
            	//System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ThreadStart(delegate{action();}));
            	//th.Start();
            	action();
            }
            else
            {
                EventHandler<ProcessEventArgs> onDebuggingPaused = null;
                onDebuggingPaused = delegate
                {
                    action();
                    process.DebuggingPaused -= onDebuggingPaused;
                };
                process.DebuggingPaused += onDebuggingPaused;
            }
        }

        private NamedValue GetVariableFromName(string variableName)
        {
            if (debuggedProcess == null || debuggedProcess.IsRunning)
            {
                return null;
            }
            else
            {
                return debuggedProcess.GetValue(variableName);
            }
        }
        
        DebuggerGridControl oldToolTipControl;

        void TextAreaMouseLeave(object source, EventArgs e)
        {
            if (CanCloseOldToolTip && !oldToolTipControl.IsMouseOver)
                CloseOldToolTip();
        }
		
         /// <summary>
        /// Получить выражение под курсором
        /// </summary>
        public string GetVariable(TextArea textArea, out int num_line)
        {
            IDocument doc = textArea.Document;
            string textContent = doc.TextContent;
            num_line = textArea.Caret.Line;
            int start_off = 0;
            int end_off = 0;
            string expressionResult = FindFullExpression(textContent, textArea.Caret.Offset,doc, out start_off, out end_off);
            return expressionResult;
        }

        private string GetVariable(ToolTipRequestEventArgs e, TextArea textArea, out int num_line, out int start_off, out int end_off)
        {
            //Point logicPos = e.LogicalPosition;
            ICSharpCode.TextEditor.TextLocation logicPos = e.LogicalPosition;
            IDocument doc = textArea.Document;
            LineSegment seg = doc.GetLineSegment(logicPos.Y);
            num_line = seg.LineNumber;
            start_off = 0;
            end_off = 0;
            if (logicPos.X > seg.Length - 1)
                return null;
            string textContent = doc.TextContent;
            string expressionResult = FindFullExpression(textContent, seg.Offset + logicPos.X,doc, out start_off, out end_off);
            return expressionResult;
        }

        private string FindFullExpression(string text, int offset, IDocument doc, out int start_off, out int end_off)
        {
            int i = offset;
            int beg_line = 1;
            int off = 0;
            start_off = 0;
            end_off = 0;
            if (debuggedProcess != null)
            {
                beg_line = (int)debuggedProcess.SelectedFunction.symMethod.SequencePoints[0].Line;
                off = doc.LineSegmentCollection[beg_line - 1].Offset;
            }
            //int lll = doc.GetLineNumberForOffset(offset) - 1;
            int cur_str_off = doc.LineSegmentCollection[doc.GetLineNumberForOffset(offset)].Offset;
            List<int> strs = new List<int>();
            i = cur_str_off;
            int cur_sk = 0;
            //while (i >= 0 && text[i] != '\n')
            //int len = text.Length;
            bool format_str = false;
            bool var_in_format_str = false;
            while (text[i] != '\n')
                if (text[i] == '\'')
                {
                    if (i == offset) return null;
                    if (i > 0 && text[i - 1] == '$')
                        format_str = true;
                    cur_sk = (cur_sk == 0) ? 1 : 0;
                    i++;
                }
                //else if (i == offset && cur_sk == 1) return null;
                else i++;
            i = offset;
            bool new_line = false;
            while (i >= off)
                if (text[i] == '\n') { new_line = true; i--; }
                else
                    if (text[i] == '/' && i > 0 && text[i - 1] == '/') if (!new_line) return null; else i--;
                else if (text[i] == '}') break;
                else if (text[i] == '{')
                {
                    if (!format_str)
                        return null;
                    else
                    {
                        var_in_format_str = true;
                        i--;
                    }
                        
                }
                    
                else i--;
            i = offset;
            if (format_str && !var_in_format_str)
                return null;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (i >= 0 && !(Char.IsLetterOrDigit(text[i]) || text[i] == '_')) i--;
            while (i >= 0 && (Char.IsLetterOrDigit(text[i]) || text[i] == '_'))
                i--;
            bool is_dot = false;
            if (i >= 0)
            {
                if (text[i] == '.') is_dot = true;
                int j = i + 1;
                while (j < text.Length && (Char.IsLetterOrDigit(text[j]) || text[j] == '_'))
                    sb.Append(text[j++]);
                if (j < text.Length && text[j] == '(')
                    return null;
                end_off = j - 1;
                start_off = i + 1;
            }
            else
            {
                int j = i + 1;
                while (j < text.Length && (Char.IsLetterOrDigit(text[j]) || text[j] == '_'))
                    sb.Append(text[j++]);
                if (j < text.Length && text[j] == '(')
                    return null;
                end_off = j - 1;
                start_off = i + 1;
            }

            if (is_dot)
            {
                sb.Insert(0, '.');
                PascalABCCompiler.Parsers.KeywordKind keyw = PascalABCCompiler.Parsers.KeywordKind.None;
                string s = CodeCompletion.CodeCompletionController.CurrentParser.LanguageInformation.
                    FindExpression(i, text, 0, 0, out keyw);
                if (s != null)
                {
                    sb.Insert(0, s);
                    string tmp = s.TrimStart(' ', '\n', '\t', '\r');
                    start_off = i - tmp.Length;
                }
            }
            else
            {
                string s = sb.ToString().Trim(' ', '\n', '\t', '\r');
                if (string.Compare(s, "array", true) == 0)
                    return "";
            }
            return sb.ToString();
        }
		
        /// <summary>
        /// Возвращает значение переменной при отладке
        /// </summary>
        [HandleProcessCorruptedStateExceptionsAttribute]
        public ListItem FindVarByName(string var, int num_line)
        {
            try
            {
                if (!var.Contains("."))
                {
                    List<NamedValue> unit_vars = new List<NamedValue>();
                    NamedValueCollection nvc = null;
                    NamedValue global_nv = null;
                    NamedValue disp_nv = null;
                    NamedValue ret_nv = null;
                    nvc = debuggedProcess.SelectedFunction.LocalVariables;
                    foreach (NamedValue nv in nvc)//smotrim sredi lokalnyh peremennyh
                    {
                        if (nv.Name.IndexOf(':') != -1)
                        {
                            int pos = nv.Name.IndexOf(':');
                            string name = nv.Name.Substring(0, pos);
                            if (string.Compare(name, var, true) == 0)
                            {
                                int start_line = Convert.ToInt32(nv.Name.Substring(pos + 1, nv.Name.LastIndexOf(':') - pos - 1));
                                int end_line = Convert.ToInt32(nv.Name.Substring(nv.Name.LastIndexOf(':') + 1));
                                if (num_line >= start_line - 1 && num_line <= end_line - 1)
                                    return new ValueItem(nv, null);
                            }
                        }
                        if (string.Compare(nv.Name, var, true) == 0)
                        {
                            return new ValueItem(nv, null);
                        }
                        else if (string.Compare(nv.Name, "self", true) == 0 && debuggedProcess.SelectedFunction.ThisValue != null)
                            return new ValueItem(debuggedProcess.SelectedFunction.ThisValue, null);
                        else if (nv.Name.Contains("$class_var")) global_nv = nv;
                        else if (nv.Name.Contains("$unit_var")) unit_vars.Add(nv);
                        else if (nv.Name == "$disp$") disp_nv = nv;//vo vlozhennyh procedurah ssylka na verh zapis aktivacii
                        else if (nv.Name.StartsWith("$rv")) ret_nv = nv;//vozvrashaemoe znachenie
                    }
                    nvc = debuggedProcess.SelectedFunction.Arguments;
                    NamedValue self_nv = null;
                    foreach (NamedValue nv in nvc)
                    {
                        if (string.Compare(nv.Name, var, true) == 0) return new ValueItem(nv, null);
                        if (nv.Name == "$obj$")
                            self_nv = nv;
                    }
                    if (var.ToLower() == "self")
                    {
                        try
                        {
                            return new ValueItem(debuggedProcess.SelectedFunction.ThisValue, null);
                        }
                        catch
                        {
                            if (self_nv != null)
                                return new ValueItem(self_nv, null);
                        }
                    }
                    if (disp_nv != null)//prohodimsja po vlozhennym podprogrammam
                    {
                        NamedValue parent_val = null;
                        IList<FieldInfo> fields = disp_nv.Type.GetFields(BindingFlags.All);
                        foreach (FieldInfo fi in fields)
                            if (string.Compare(fi.Name, var, true) == 0) return new ValueItem(fi.GetValue(disp_nv), fi.DeclaringType);
                            else if (fi.Name == "$parent$") parent_val = fi.GetValue(disp_nv);
                        NamedValue pv = parent_val;
                        while (!pv.IsNull)
                        {
                            fields = parent_val.Type.GetFields(BindingFlags.All);
                            foreach (FieldInfo fi in fields)
                                if (string.Compare(fi.Name, var, true) == 0) return new ValueItem(fi.GetValue(parent_val), fi.DeclaringType);
                                else if (fi.Name == "$parent$") pv = fi.GetValue(parent_val);
                            if (!pv.IsNull) parent_val = pv;
                        }
                    }
                    //                nvc = debuggedProcess.SelectedFunction.ContaingClassVariables;
                    //                foreach (NamedValue nv in nvc)
                    //                {
                    //                    if (string.Compare(nv.Name, var, true) == 0) return new ValueItem(nv,null);
                    //                }
                    //				try
                    //				{
                    //				if (debuggedProcess.SelectedFunction.ThisValue != null)
                    //				{
                    //					NamedValue nv = debuggedProcess.SelectedFunction.ThisValue.GetMember(var);
                    //					if (nv != null) return new ValueItem(nv,null);
                    //				}
                    //				}
                    //				catch(System.Exception e)
                    //				{
                    //					
                    //				}
                    nvc = debuggedProcess.SelectedFunction.ContaingClassVariables;
                    foreach (NamedValue nv in nvc)//chleny klassa
                    {
                        if (string.Compare(nv.Name, var, true) == 0) return new ValueItem(nv, nv.Type);
                    }
                    if (self_nv != null)
                    {
                        IList<FieldInfo> fields = self_nv.Dereference.Type.GetFields(BindingFlags.All);
                        foreach (FieldInfo fi in fields)
                            if (string.Compare(fi.Name, var, true) == 0)
                                return new ValueItem(fi.GetValue(self_nv.Dereference), fi.DeclaringType);
                    }
                    if (global_nv != null)
                    {
                        IList<FieldInfo> fields = global_nv.Type.GetFields(BindingFlags.All);
                        foreach (FieldInfo fi in fields)
                            if (string.Compare(fi.Name, var, true) == 0) return new ValueItem(fi.GetValue(global_nv), fi.DeclaringType);
                        Type global_type = AssemblyHelper.GetType(global_nv.Type.FullName);
                        if (global_type != null)
                        {
                            System.Reflection.FieldInfo fi = global_type.GetField(var, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.IgnoreCase);
                            if (fi != null && fi.IsLiteral)
                                return new ValueItem(DebugUtils.MakeValue(fi.GetRawConstantValue()), var, global_nv.Type);
                        }
                    }


                    List<DebugType> types = AssemblyHelper.GetUsesTypes(debuggedProcess, debuggedProcess.SelectedFunction.DeclaringType);
                    foreach (DebugType dt in types)
                    {
                        IList<FieldInfo> fields = dt.GetFields(BindingFlags.All);
                        foreach (FieldInfo fi in fields)
                            if (fi.IsStatic && string.Compare(fi.Name, var, true) == 0)
                                return new ValueItem(fi.GetValue(null), fi.DeclaringType);
                        Type unit_type = AssemblyHelper.GetType(dt.FullName);
                        if (unit_type != null)
                        {
                            System.Reflection.FieldInfo fi = unit_type.GetField(var, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.IgnoreCase);
                            if (fi != null && fi.IsLiteral)
                                return new ValueItem(DebugUtils.MakeValue(fi.GetRawConstantValue()), var, dt);
                        }
                    }

                    /*foreach (NamedValue nv in unit_vars)
                    {
                        IList<FieldInfo> fields = nv.Type.GetFields(BindingFlags.All);
                        foreach (FieldInfo fi in fields)
                            if (string.Compare(fi.Name, var, true) == 0) return new ValueItem(fi.GetValue(nv),fi.DeclaringType);
                    }*/

                    if (ret_nv != null && string.Compare(var, "Result", true) == 0)
                        return new ValueItem(ret_nv, null);
                    Type t = AssemblyHelper.GetTypeForStatic(var);
                    if (t != null)
                    {
                        DebugType dt = DebugUtils.GetDebugType(t);//DebugType.Create(this.debuggedProcess.GetModule(var),(uint)t.MetadataToken);
                        return new BaseTypeItem(dt, t);
                    }
                }
                else
                {
                    if (evaluator == null) return null;
                    Debugger.Wrappers.CorSym.ISymUnmanagedMethod sym_meth = debuggedProcess.SelectedFunction.symMethod;
                    //            	if (sym_meth == null) return null;
                    //               		if (sym_meth.SequencePoints.Length > 0)
                    //                 	if (num_line < sym_meth.SequencePoints[0].Line || num_line > sym_meth.SequencePoints[sym_meth.SequencePoints.Length - 1].EndLine)
                    //                     return null;
                    string preformat;
                    RetValue rv = evaluator.GetValueForExpression(var, out preformat);
                    if (rv.obj_val != null && rv.obj_val is NamedValue)
                    {
                        Value v = rv.obj_val;
                        ValueItem vi = new ValueItem(v, var, evaluator.declaringType);
                        vi.SpecialName = preformat;
                        return vi;
                    }
                    else if (rv.prim_val != null)
                    {
                        ValueItem vi = new ValueItem(DebugUtils.MakeValue(rv.prim_val), var, evaluator.declaringType);
                        vi.SpecialName = preformat;
                        return vi;
                    }
                    else if (rv.type != null)
                    {
                        return new BaseTypeItem(rv.type, rv.managed_type);
                    }
                }
            }
            catch (System.Exception e)
            {

            }
            return null;
        }
		
        private TextMarker oldMarker;
        
        public void RemoveMarker(IDocument doc)
        {
        	try
        	{
        		if (oldMarker != null && doc != null && doc.TextLength >= oldMarker.Offset)
        		{
        			doc.MarkerStrategy.RemoveMarker(oldMarker);
        			doc.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.SingleLine,doc.GetLineNumberForOffset(oldMarker.Offset)));
                	doc.CommitUpdate();
        		}
        	}
        	catch(System.Exception e)
        	{
        		
        	}
        }
        
        public bool HasBreakpoints()
        {
        	return BreakPointFactory.HasBreakpoints();
        }
        
        /// <summary>
        /// Обработчик события запроса на подсказку значения переменной
        /// </summary>
        [HandleProcessCorruptedStateExceptionsAttribute]
        public void TextAreaToolTipRequest(object sender, ToolTipRequestEventArgs e)
        {
            DebuggerGridControl toolTipControl = null;
            TextMarker marker = null;
            //RemoveMarker((sender as TextArea).Document);
            if (debuggedProcess == null) return;
            if (debuggedProcess.SelectedFunction == null || !debuggedProcess.IsPaused) return;
            try
            {
                TextArea textArea = (TextArea)sender;
                
                if (textArea != curTextArea) return;
                if (e.ToolTipShown) return;
                if (oldToolTipControl != null && !oldToolTipControl.AllowClose) return;
                if (!IsRunning) return;

                if (e.InDocument)
                {
                    ToolTipInfo ti = null;
                    int num_line = 0;
                    int start_off = 0;
                    int end_off = 0;
                    string var = GetVariable(e,textArea,out num_line, out start_off, out end_off);
                    if (var == null || var == "") return;
                    //if (debuggedProcess.SelectedFunction.LocalVariables.Count > 0)
                    {
                        //NamedValue nv = FindVarByName(var, num_line);
                        ListItem nv = FindVarByName(var, num_line);
                        if (nv != null)
                        {
                        	System.Threading.Thread.Sleep(50);
                        	DebuggerGridControl dgc = new DebuggerGridControl(new DynamicTreeDebuggerRow(nv));
                        	dgc.doc = textArea.Document;
                        	ti = new ToolTipInfo(dgc);
                        	ICSharpCode.TextEditor.TextLocation logicPos = e.LogicalPosition;
            				LineSegment seg = textArea.Document.GetLineSegment(logicPos.Y);
            				marker = new TextMarker(start_off, end_off-start_off+1, TextMarkerType.Cant, Color.Black, Color.White);
                        	textArea.Document.MarkerStrategy.AddMarker(marker);
                        	textArea.Document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.SingleLine,seg.LineNumber));
                        	textArea.Document.CommitUpdate();
                        }
                    }
                    if (ti != null)
                    {
                        toolTipControl = ti.ToolTipControl as DebuggerGridControl;
                        if (ti.ToolTipText != null)
                        {
                            e.ShowToolTip(ti.ToolTipText);
                        }
                    }
                    CloseOldToolTip();
                    if (toolTipControl != null)
                    {
                        toolTipControl.ShowForm(textArea, e.LogicalPosition);
                    }
                    oldToolTipControl = toolTipControl;
                    RemoveMarker(textArea.Document);
                    oldMarker = marker;

                }
            }
            catch (System.Exception ex)
            {
                //frm.WriteToOutputBox(ex.Message);// ICSharpCode.Core.MessageService.ShowError(ex);
            }
            finally
            {
                if (toolTipControl == null && CanCloseOldToolTip)
                {
                    CloseOldToolTip();
                    RemoveMarker((sender as TextArea).Document);
                }
            }
        }

        bool CanCloseOldToolTip
        {
            get
            {
                return oldToolTipControl != null && oldToolTipControl.AllowClose;
            }
        }

        void CloseOldToolTip()
        {
            if (oldToolTipControl != null)
            {
                Form frm = oldToolTipControl.FindForm();
                if (frm != null) frm.Close();
                oldToolTipControl = null;
            }
            	
        }

        /// <summary>
        /// Завершить процесс
        /// </summary>
        [HandleProcessCorruptedStateExceptionsAttribute]
        public void Stop(string ExeFileName)
        {
            try
            {
                workbench.WidgetController.SetStartDebugEnabled();
                dbg.Processes[0].Terminate();
            }
            catch (System.Exception e)
            {
            }
        }
		
        /// <summary>
        /// Прервать процесс
        /// </summary>
        [HandleProcessCorruptedStateExceptionsAttribute]
        public void DebugBreak()
        {
            try
            {
                workbench.WidgetController.SetStartDebugEnabled();
                dbg.Processes[0].Break();
            }
            catch (System.Exception e)
            {
            }
        }

        /// <summary>
        /// Проверяет, запущен ли процесс с именем ExeFileName на отладку
        /// </summary>
        public bool IsRun(string ExeFileName)
        {
        	return IsRunning && string.Compare(this.ExeFileName,ExeFileName,true)==0;
        }

        public bool IsRun(string ExeFileName, string SourceFileName)
        {
            return IsRunning && string.Compare(this.ExeFileName, ExeFileName, true) == 0 && string.Compare(this.FullFileName, SourceFileName, true) == 0;
        }

        /// <summary>
        /// Проверяет, запущен ли процесс с именем ExeFileName на отладку
        /// </summary>
        public bool IsRunAndInThisTabPage(string ExeFileName, ICodeFileDocument cfdc)
        {
        	if (WorkbenchServiceFactory.DocumentService.ContainsTab(cfdc))
        		return true;
        	return IsRunning && (/*string.Compare(this.ExeFileName,ExeFileName,true)==0 || */WorkbenchServiceFactory.DocumentService.CurrentCodeFileDocument == this.curPage);
        }
        
        public void SetAsPossibleDebugPage(CodeFileDocumentControl ctrl)
        {
        	if (IsRunning && tab_changed && ctrl != this.curPage)
        	{
        		curPage = ctrl;
        	}
        	tab_changed = false;
        }
        
        public CodeFileDocumentControl CurPage
        {
        	get
        	{
        		return curPage as CodeFileDocumentControl;
        	}
        }
        
        public void SetAsDebugPage(ICodeFileDocument ctrl)
        {
        	curPage = ctrl;
        }
        
        public bool IsRunAndInThisTabPage()
        {
            return IsRunning && (WorkbenchServiceFactory.DocumentService.CurrentCodeFileDocument == this.curPage || WorkbenchServiceFactory.DocumentService.ContainsTab(WorkbenchServiceFactory.DocumentService.CurrentCodeFileDocument));
        }

        /// <summary>
        /// Шаг без входа в подпрограмму
        /// </summary>
        [HandleProcessCorruptedStateExceptionsAttribute]
        public void StepOver()
        {
            try
            {
                workbench.WidgetController.SetStartDebugDisabled();
                if (debuggedProcess.SelectedFunction.Name == ".cctor")
                {
                    var sequencePoints = debuggedProcess.SelectedFunction.symMethod.SequencePoints;
                    if (sequencePoints != null && debuggedProcess.NextStatement.StartLine == sequencePoints[sequencePoints.Length-1].Line)
                    {
                        debuggedProcess.StepOut();
                    }
                    else
                        debuggedProcess.StepOver();
                }
                else
                    debuggedProcess.StepOver();
                CurrentLineBookmark.Remove();
            }
            catch (System.Exception e)
            {
            }
        }
		
        /// <summary>
        /// Продолжить выполнение процесса
        /// </summary>
        [HandleProcessCorruptedStateExceptionsAttribute]
        public void Continue()
        {
            try
            {
                workbench.WidgetController.SetStartDebugDisabled();
                dbg.Processes[0].Continue();
                CurrentLineBookmark.Remove();
            }
            catch (System.Exception e)
            {
            }
        }
		
        /// <summary>
        /// Шаг с входом в подпрограмму
        /// </summary>
        [HandleProcessCorruptedStateExceptionsAttribute]
        public void StepInto()
        {
            try
            {
                workbench.WidgetController.SetStartDebugDisabled();
                stepin_stmt = dbg.Processes[0].NextStatement;
                dbg.Processes[0].StepInto();
                CurrentLineBookmark.Remove();
            }
            catch (System.Exception e)
            {
            }
        }
		
        /// <summary>
        /// Шаг с входом из подпрограммы
        /// </summary>
        [HandleProcessCorruptedStateExceptionsAttribute]
        public void StepOut()
        {
            try
            {
                if (IsRunning)
                {
                    workbench.WidgetController.SetStartDebugDisabled();
                    dbg.Processes[0].StepOut();
                    CurrentLineBookmark.Remove();
                }
            }
            catch (System.Exception e)
            {
            }
        }
		
        /// <summary>
        /// Идти к курсору
        /// </summary>
        [HandleProcessCorruptedStateExceptionsAttribute]
        public void RunToCursor()
        {
            try
            {
                //cur_brpt = dbg.AddBreakpoint(new SourcecodeSegment((frm.CurrentTabPage.ag as CodeFileDocumentControl).FileName,(frm.CurrentTabPage.ag as CodeFileDocumentControl).TextEditor.ActiveTextAreaControl.Caret.Line + 1,(frm.CurrentTabPage.Tag as CodeFileDocumentControl).TextEditor.ActiveTextAreaControl.Caret.Column + 1,
                  //  (frm.CurrentTabPage.ag as CodeFileDocumentControl).TextEditor.ActiveTextAreaControl.Caret.Column+100), true);
                workbench.WidgetController.SetStartDebugDisabled();
                currentBreakpoint = dbg.AddBreakpoint(WorkbenchServiceFactory.DocumentService.CurrentCodeFileDocument.FileName, WorkbenchServiceFactory.DocumentService.CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.Caret.Line + 1);
                AddGoToBreakPoint(currentBreakpoint);
                Status = DebugStatus.None;
                dbg.Processes[0].Continue();
                //dbg.RemoveBreakpoint(cur_brpt);
                CurrentLineBookmark.Remove();
            }
            catch (System.Exception e)
            {
            }
        }

        private Stack<Breakpoint> goto_brs = new Stack<Breakpoint>();

        /// <summary>
        /// Добавляет Breakpoint по F4
        /// </summary>
        public void AddGoToBreakPoint(Breakpoint br)
        {
            goto_brs.Push(br);
        }

        public string ExecuteImmediate(string stmt)
        {
            RetValue rv = evaluator.Evaluate(stmt, true);
            if (rv.prim_val != null)
                return rv.prim_val.ToString();
            if (rv.obj_val != null)
                return rv.obj_val.AsString;
            if (rv.err_mes != null)
                return rv.err_mes;
            if (rv.syn_err)
                return PascalABCCompiler.StringResources.Get("EXPR_VALUE_SYNTAX_ERROR_IN_EXPR");
            else
                return PascalABCCompiler.StringResources.Get("EXPR_VALUE_ERROR_IN_EXPR");
        }
        
        public void AddGoToBreakPoint(string fileName, int line)
        {
        	goto_brs.Push(dbg.AddBreakpoint(fileName, line));
        }
        
        /// <summary>
        /// Удаляет все F4-Breakpoint
        /// </summary>
        [HandleProcessCorruptedStateExceptionsAttribute]
        private void RemoveGotoBreakpoints()
        {
            while (goto_brs.Count > 0)
            {
                Breakpoint br = goto_brs.Pop();
                try
                {

                    dbg.RemoveBreakpoint(br);
                }
                catch (System.Exception e)
                {
                }
            }
        }

        IRetValue IDebuggerManager.Evaluate(string expr)
        {
            return Evaluate(expr);
        }

        string IDebuggerManager.MakeValueView(IValue value)
        {
            return MakeValueView(value as Value);
        }

        bool IDebuggerManager.IsRunning
        {
            get
            {
                return IsRunning;
            }
        }
    }

}
