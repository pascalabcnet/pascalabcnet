// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
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

    public static class DebuggerIcons
    {
        static ImageList imageList;

        public static ImageList ImageList
        {
            get
            {
                return imageList;
            }
        }

        static DebuggerIcons()
        {
            imageList = new ImageList();
            //System.Resources.ResourceManager rm = new System.Resources.ResourceManager("VisualPascalABCNET.BitmapResources", System.Reflection.Assembly.GetExecutingAssembly());
            imageList.Images.Add(new Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("VisualPascalABC.Resources.Icons.16x16.Class.png")));
            imageList.Images.Add(new Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("VisualPascalABC.Resources.Icons.16x16.Field.png")));
            imageList.Images.Add(new Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("VisualPascalABC.Resources.Icons.16x16.Property.png")));

        }

        public static Image GetImage(Value val)
        {
            return imageList.Images[GetImageListIndex(val)];
        }
        [HandleProcessCorruptedStateExceptionsAttribute]
        public static int GetImageListIndex(Value val)
        {
            try
            {
                if (val.IsObject)
                {
                    return 0; // Class
                }
                else
                {
                    return 1; // Field
                }
            }
            catch (System.Exception e)
            {
                return 1;
            }
        }
    }

    public abstract class ListItem : VisualPascalABCPlugins.IListItem
    {
        private string specialName;
        
    	public event EventHandler<ListItemEventArgs> Changed;
		
        public abstract int ImageIndex { get; }
        public abstract string Name { get; }
        public string SpecialName
        {
        	get
        	{
        		return specialName;
        	}
        	set
        	{
        		specialName = value;
        	}
        }
        public abstract string Text { get; }
        public abstract bool CanEditText { get; }
        public abstract string Type { get; }
        public abstract bool HasSubItems { get; }
        public abstract IList<IListItem> SubItems { get; }
        /*IList<IListItem> VisualPascalABCPlugins.IListItem.SubItems
        {
            get
            {
                List<IListItem> lst = new List<IListItem>();
                IList<ListItem> item_lst = SubItems;
                foreach (ListItem li in item_lst)
                    lst.Add(li);
                return lst;
            }
        }*/
        public abstract bool IsLiteral { get;}
        public System.Drawing.Image Image
        {
            get
            {
                if (ImageIndex == -1)
                {
                    return null;
                }
                else
                {
                    //return (Image)DebuggerIcons.ImageList.Images[ImageIndex];
                    return CodeCompletionProvider.ImagesProvider.ImageList.Images[ImageIndex];
                }
            }
        }

        protected virtual void OnChanged(ListItemEventArgs e)
        {
            if (Changed != null)
            {
                Changed(this, e);
            }
        }

        public virtual bool SetText(string newValue)
        {
            throw new NotImplementedException();
        }

        public virtual ContextMenuStrip GetContextMenu()
        {
            return null;
        }
    }

    

    public class ModuleItem : ListItem
    {
        private Function func;
		private List<DebugType> types;
        private Mono.Debugging.Client.StackFrame frame;
        private List<Mono.Debugging.Evaluation.TypeValueReference> monoTypes;

        public ModuleItem(List<Mono.Debugging.Evaluation.TypeValueReference> monoTypes, Mono.Debugging.Client.StackFrame frame)
        {
            this.monoTypes = monoTypes;
            this.frame = frame;
        }

        public override int ImageIndex
        {
            get
            {
                return -1;
            }
        }

        public override bool IsLiteral
        {
            get { return false; }
        }

        public override string Name
        {
            get
            {
                //return ns_name;
                return PascalABCCompiler.StringResources.Get("DEBUG_VIEW_MODULE");;
            }
        }

        public override string Text
        {
            get
            {
                return String.Empty;
            }
        }

        public override bool CanEditText
        {
            get
            {
                return false;
            }
        }

        public override string Type
        {
            get
            {
                return String.Empty;
            }
        }

        public override bool HasSubItems
        {
            get
            {
                return true;
            }
        }

        public override IList<IListItem> SubItems
        {
            get
            {
                List<IListItem> ret = new List<IListItem>();
                foreach (var tr in monoTypes)
                {
                    var fields = tr.GetChildReferences(Mono.Debugging.Client.EvaluationOptions.DefaultOptions);
                    foreach (var fi in fields)
                  	    if (!fi.Name.Contains("$")) 
                            ret.Add(new ValueItem(fi.CreateObjectValue(false, Mono.Debugging.Client.EvaluationOptions.DefaultOptions)));
                }
                return ret;
            }
        }
    }

    public class FunctionItem : ListItem
    {
        Mono.Debugging.Client.StackFrame frame;


        public override bool IsLiteral
        {
            get
            {
                return false;
            }
        }
        public override int ImageIndex
        {
            get
            {
                return -1;
            }
        }

        public override string Name
        {
            get
            {
                return frame.SourceLocation.MethodName;
            }
        }

        public override string Text
        {
            get
            {
                return String.Empty;
            }
        }

        public override bool CanEditText
        {
            get
            {
                return false;
            }
        }

        public override string Type
        {
            get
            {
                return String.Empty;
            }
        }

        public override bool HasSubItems
        {
            get
            {
                return true;
            }
        }
       
        public override IList<IListItem> SubItems
        {
            [HandleProcessCorruptedStateExceptionsAttribute]
            get
            {
                List<IListItem> ret = new List<IListItem>();
                Hashtable ht = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
                foreach (var val in frame.GetAllLocals())
                {
                    if (val.Name.Contains("$class_var"))
                    {
                        List<Mono.Debugging.Evaluation.TypeValueReference> types = new List<Mono.Debugging.Evaluation.TypeValueReference>();
                        try
                        {
                            if (val.TypeName.Contains(PascalABCCompiler.StringConstants.ImplementationSectionNamespaceName))
                        	{
                                string interf_name = val.TypeName.Substring(0, val.TypeName.IndexOf(PascalABCCompiler.StringConstants.ImplementationSectionNamespaceName));
                        		Type t = AssemblyHelper.GetTypeForStatic(interf_name);
                                var tr = new Mono.Debugging.Evaluation.TypeValueReference(frame.SourceBacktrace.GetEvaluationContext(frame.Index, Mono.Debugging.Client.EvaluationOptions.DefaultOptions), 
                                            (frame.DebuggerSession as Mono.Debugging.Soft.SoftDebuggerSession).GetType(t.FullName));
                        		types.Add(tr);
                        	}
                            var tp = (frame.DebuggerSession as Mono.Debugging.Soft.SoftDebuggerSession).GetType(val.TypeName);
                            if (tp != null)
                            {
                                var valtr = new Mono.Debugging.Evaluation.TypeValueReference(frame.SourceBacktrace.GetEvaluationContext(frame.Index, Mono.Debugging.Client.EvaluationOptions.DefaultOptions),
                                            (frame.DebuggerSession as Mono.Debugging.Soft.SoftDebuggerSession).GetType(val.TypeName));
                                types.Add(valtr);
                            }
                            
                        }
                        catch (System.Exception e)
                        {
#if (DEBUG)
                            Console.WriteLine(e.Message + " " + e.StackTrace);
#endif
                        }
                        
                        ret.Add(new ModuleItem(types, frame));
                    }
                }
                foreach (var val in frame.GetAllLocals())
                {
                    if (!val.Name.Contains("$"))
                    	ret.Add(new ValueItem(val));
                    else if (val.Name.Contains("$rv"))
                    {
                        ret.Add(new ValueItem(val));
                    }
                    /*else if (val.Name == "$disp$")
                    {
                        IList<FieldInfo> fields = val.Type.GetFields(BindingFlags.All);
                        foreach (FieldInfo fi in fields)
                        if (!fi.Name.Contains("$")) 
                        {
                        	ret.Add(new ValueItem(fi.GetValue(val),fi.DeclaringType));
                        	ht[fi.Name] = fi.Name;
                        }
                    }*/
                }
                Mono.Debugging.Client.ObjectValue self_nv = null;
                try
                {
                    foreach (var val in frame.GetParameters())
                    {
                        if (!val.Name.Contains("$") && !ht.ContainsKey(val.Name))
                            ret.Add(new ValueItem(val));
                        else if (val.Name == "$obj$")
                            self_nv = val;
                    }
                }
                catch
                {
                }
                if (frame.GetThisReference() != null) 
                    ret.Add(new ValueItem(frame.GetThisReference()));
                else if (self_nv != null)
                	ret.Add(new ValueItem(self_nv));
                return ret.AsReadOnly();
            }
        }

        public FunctionItem(Mono.Debugging.Client.StackFrame frame)
        {
            this.frame = frame;
        }
    }
    
    class PairValueItem : ValueItem
    {
    	Value first;
    	Value second;
    	private string name;
    	
    	public PairValueItem(Value first, Value second, string name)
    	{
    		this.name = name;
    		this.first = first;
    		this.second = second;
    	}
    	
		public override string Text {
			get { 
    			ValueItem vi = new ValueItem(first,"",first.Type);
    			ValueItem vi2 = new ValueItem(second,"",second.Type);
    			return "["+vi.Text+","+vi2.Text+"]";
    		}
		}
    	
		public override string Type {
			get { return ""; }
		}
    	
		public override bool CanEditText {
			get { return false; }
		}
    	
		public override bool HasSubItems {
			get { return false; }
		}
    	
		public override int ImageIndex {
			get { return 1; }
		}
    	
		public override bool IsLiteral {
			get { return false; }
		}
    	
		public override IList<IListItem> SubItems {
			get { return new List<IListItem>(); }
		}
    	
		public override string Name {
			get { return name; }
		}
    }
    
    class ValueItem : ListItem
    {
        Value val;
        Mono.Debugging.Client.ObjectValue monoValue;

		public DebugType declaringType;
        public Mono.Debugger.Soft.TypeMirror monoDeclaringType;


        public ValueItem() {}
        
        public override bool IsLiteral
        {
            [HandleProcessCorruptedStateExceptionsAttribute]
            get
            {
                
                try
                {
                    return monoValue.IsPrimitive;
                }
                catch (System.Exception e)
                {
                    return true;
                }
            }
        }

        public Value Value
        {
            get
            {
                return val;
            }
        }

        public static bool ShowValuesInHexadecimal
        {
            get
            {
                return false;//return ((WindowsDebugger)DebuggerService.CurrentDebugger).Properties.Get("ShowValuesInHexadecimal", false);
            }
            set
            {
                //((WindowsDebugger)DebuggerService.CurrentDebugger).Properties.Set("ShowValuesInHexadecimal", value);
            }
        }

        private bool primitive = true;
		private int imageIndex = -1;
		private string name;
        
        public override int ImageIndex
        {
            [HandleProcessCorruptedStateExceptionsAttribute]
            get
            {
                if (monoValue != null)
                {
                    if (monoValue.HasFlag(Mono.Debugging.Client.ObjectValueFlags.Property))
                    {
                        if (monoValue.HasFlag(Mono.Debugging.Client.ObjectValueFlags.Private))
                            return CodeCompletionProvider.ImagesProvider.IconNumberPrivateProperty;
                        if (monoValue.HasFlag(Mono.Debugging.Client.ObjectValueFlags.Protected))
                            return CodeCompletionProvider.ImagesProvider.IconNumberProtectedProperty;
                        return CodeCompletionProvider.ImagesProvider.IconNumberProperty;
                    }
                    else if (monoValue.HasFlag(Mono.Debugging.Client.ObjectValueFlags.Field))
                    {
                        if (monoValue.HasFlag(Mono.Debugging.Client.ObjectValueFlags.Private))
                            return CodeCompletionProvider.ImagesProvider.IconNumberPrivateField;
                        if (monoValue.HasFlag(Mono.Debugging.Client.ObjectValueFlags.Protected))
                            return CodeCompletionProvider.ImagesProvider.IconNumberProtectedField;
                        return CodeCompletionProvider.ImagesProvider.IconNumberField;
                    }
                }

                return 1;
            }
        }
        
        public override string Name
        {
            [HandleProcessCorruptedStateExceptionsAttribute]
            get
            {
                
                try
                {
                    if (monoValue.Name.IndexOf(':') != -1)
                        return monoValue.Name.Substring(0, monoValue.Name.IndexOf(':'));
                    if (monoValue.Name.StartsWith("$rv_"))
                        return "Result";
                    return monoValue.Name;
                }
                catch
                {
                    return "";
                }
            }
        }

        public static string MakeEnumText(Value val)
        {
        	FieldInfo val_fld = (FieldInfo)val.Type.GetMember("value__",BindingFlags.All)[0];
            MemberValue mv = val_fld.GetValue(val);
            int v = (int)mv.PrimitiveValue;
            Type t = AssemblyHelper.GetType(val.Type.FullName);
            //System.Reflection.FieldInfo[] flds = t.GetFields();
            string s = Enum.GetName(t,v);
            if (s != null) return s;
            Array arr = Enum.GetValues(t);
            StringBuilder sb = new StringBuilder();
            List<int> lst = new List<int>();
            foreach (int i in arr)
            	lst.Add(i);
            bool is_first = false;
            for (int i=0; i<lst.Count; i++)
            {
            	if ((lst[i] & v) == lst[i])
            	{
            		s = Enum.GetName(t,lst[i]);
            		if (!string.IsNullOrEmpty(s))
            		{
            			if (is_first)
            			sb.Append(" or ");
            			sb.Append(s);
            			is_first = true;
            		}
            		else continue;
            		
            	}
            	else continue;
            }
            return sb.ToString();
            //return flds[v].Name;
        }
        
        private Value tmp_val = null;

        public override string Text
        {
            [HandleProcessCorruptedStateExceptionsAttribute]
            get
            {
                return monoValue.DisplayValue;
                /*if (ShowValuesInHexadecimal && val.IsInteger)
                {
                    return String.Format("0x{0:X}", val.PrimitiveValue);
                }
                else
                {
                    try
                    {
                        if ((val.Type.IsByRef() && !val.Type.IsPrimitive))
                        {
                            if (val is NamedValue)
                                name = (val as NamedValue).Name;
                            tmp_val = val;
                            val = val.Dereference;
                        }
                        if (IsEnum())
                        {
                            return MakeEnumText(val);
                        }
                        //else 
                        //if (IsArrayWrap())
                        NamedValue nv = GetNullBasedArray();
                        if (nv != null)
                        {
                            if (nv.ArrayLenght > 40)
                                return DebugUtils.WrapTypeName(val.Type);
                            NamedValueCollection nvc = GetNullBasedArray().GetArrayElements();
                            StringBuilder sb = new StringBuilder();
                            sb.Append('(');
                            for (int i = 0; i < nvc.Count; i++)
                            {
                                if (i > 10)
                                {
                                    sb.Append("...");
                                    break;
                                }
                                sb.Append(WorkbenchServiceFactory.DebuggerManager.MakeValueView(nvc[i]));
                                if (i < nvc.Count - 1) sb.Append(',');
                            }
                            sb.Append(')');
                            return sb.ToString();
                        }
                        else if (val.IsArray)
                        {
                            uint[] dims = val.Dereference.ArrayDimensions;
                            StringBuilder sb = new StringBuilder();
                            for (int i = 0; i < dims.Length; i++)
                            {
                                sb.Append(dims[i].ToString());
                                if (i < dims.Length - 1)
                                    sb.Append(',');
                            }
                            return DebugUtils.WrapTypeName(val.Type.GetElementType()) + "[" + sb.ToString() + "]";
                        }
                        else
                            if (val.IsNull)
                                if (WorkbenchServiceFactory.DebuggerManager.parser != null)
                                    return WorkbenchServiceFactory.DebuggerManager.parser.LanguageInformation.GetKeyword(PascalABCCompiler.Parsers.SymbolKind.Null);
                                else
                                    return "nil";
                            else
                                if (string.Compare(this.Value.Type.FullName, "PABCSystem.TypedSet", true) == 0)
                                {
                                    return WorkbenchServiceFactory.DebuggerManager.MakeValueView(this.Value);
                                }
                                else
                                    if (val.Type.ManagedType == typeof(string))
                                        return "'" + val.AsString + "'";
                                    else
                                        if (val.Type.ManagedType == typeof(char))
                                            return "'" + val.AsString + "'";
                                        else if (val.Type.ManagedType == typeof(double) || val.Type.ManagedType == typeof(float))
                                            return Convert.ToString(val.PrimitiveValue, System.Globalization.NumberFormatInfo.InvariantInfo);
                                        else
                                            if (!val.Type.IsPrimitive)
                                            {
                                                if (DebugUtils.CheckForCollection(val.Type))
                                                    return DebugUtils.MakeViewForCollection(val);
                                                bool failed = false;
                                                string s = ExpressionEvaluator.GetToString(val, out failed);
                                                this.failed = failed;
                                                if (s != null) return s;
                                                return val.AsString;
                                            }
                                            else
                                                return val.AsString;
                    }
                    catch (System.Exception e)
                    {
                        return "";
                    }
                }*/
            }
        }
		
        private bool failed = false;
        
        public override bool CanEditText
        {
            [HandleProcessCorruptedStateExceptionsAttribute]
            get
            {
                return false;
            }
        }

        public override string Type
        {
            [HandleProcessCorruptedStateExceptionsAttribute]
            get
            {
                try
                {
                    return monoValue.TypeName;
                }
                catch(System.Exception e)
                {
                	return "";
                }
            }
        }
        [HandleProcessCorruptedStateExceptionsAttribute]
        public bool IsObject()
        {
            try
            {
                return monoValue.IsObject;
            }
            catch (System.Exception e)
            {
                return false;
            }
        }
        
        public override bool HasSubItems
        {
            [HandleProcessCorruptedStateExceptionsAttribute]
            get
            {
                try
                {
                    if (monoValue.TypeName == "string")
                        return false;
                    return monoValue.IsObject || monoValue.IsArray;
                	//if (IsEnum() || val.Type.ManagedType == typeof(string)) return false;
                    //return val.IsObject || val.IsArray;
                }
                catch (System.Exception e)
                {
                    return false;
                }
            }
        }

        public override IList<IListItem> SubItems
        {
            get
            {

                List<IListItem> list = new List<IListItem>();
               
                if (monoValue != null && monoValue.IsObject)
                {
                    if (monoValue.TypeName.IndexOf("System.Collections.Generic.") != -1)
                    {
                        var tm = monoValue.Type as Mono.Debugger.Soft.TypeMirror;
                        if (tm == null)
                            return list;
                        var fields = tm.GetFields();
                        var tr = new Mono.Debugging.Evaluation.TypeValueReference(VisualPascalABC.WorkbenchServiceFactory.DebuggerManager.StackFrame.SourceBacktrace.GetEvaluationContext(VisualPascalABC.WorkbenchServiceFactory.DebuggerManager.StackFrame.Index, Mono.Debugging.Client.EvaluationOptions.DefaultOptions), tm);
                        foreach (var fi in fields)
                        {
                            try
                            {
                                var valref = tr.GetChild(fi.Name, Mono.Debugging.Client.EvaluationOptions.DefaultOptions);
                                
                                list.Add(new ValueItem(valref.CreateObjectValue(false)));
                            }
                            catch (System.Exception ex)
                            {
#if (DEBUG)
                                Console.WriteLine(ex.Message+" "+ex.StackTrace);
#endif
                            }
                            
                        }
                        return list;
                    }

                    foreach (var element in monoValue.GetAllChildren())
                    {
                        list.Add(new ValueItem(element));
                    }
                    return list;
                }
                /*if (monoValue.IsObject || !monoValue.IsPrimitive)
                {
                    //if (IsArrayWrap())
                    //{
                    var lv = GetNullBasedArray();
                    if (lv != null)
                    {
                        int i = 0;
                        foreach (var element in lv.GetAllChildren())
                        {
                            list.Add(new ArrayValueItem(element, monoValue, lv, i++));

                        }
                    }
                    //}
                    else
                    {
                        if (!val.Type.IsByRef())
                            return new BaseTypeItem(val, val.Type).SubItems;
                    }
                }*/
                return list;
            }
        }

        private Mono.Debugging.Client.ObjectValue GetNullBasedArray()
        {
            var fields = monoValue.GetAllChildren();
            if (fields.Length != 3) 
                return null;
            foreach (var fi in fields)
                if (fi.Name == "NullBasedArray")
                    return fi;
            return null;
        }

        private bool IsArrayWrap()
        {
            System.Reflection.FieldInfo fi = val.Type.ManagedType.GetField("NullBasedArray");
            if (fi != null && fi.FieldType.IsArray)
            {
                return true;
            }
            return false;
        }
        
        public bool IsEnum()
        {
            if (IsObject())
            {
                IList<FieldInfo> fields = val.Type.GetFields(BindingFlags.All);
                bool is_enum = true;
                foreach (FieldInfo fi in fields)
                {
                    if (!(fi.IsStatic && fi.IsLiteral))
                    {
                        if (fi.Name != "value__")
                        {
                            is_enum = false;
                            break;
                        }
                    }
                }
                if (fields.Count == 0) is_enum = false;
                return is_enum;
            }
            return false;
        }
		
        public ValueItem(Value val, string name, DebugType declaringType)
        {
        	this.val = val;
        	this.declaringType = declaringType;
        	this.name = name;
        	val.Changed += delegate { OnChanged(new ListItemEventArgs(this)); };
        }

        public ValueItem(Mono.Debugging.Client.ObjectValue val, string name, Mono.Debugger.Soft.TypeMirror declaringType)
        {
            this.monoValue = val;
            this.monoDeclaringType = declaringType;
            this.name = name;
            val.ValueChanged += delegate { OnChanged(new ListItemEventArgs(this)); };
        }

        public ValueItem(Value val, string name, DebugType declaringType, bool is_in_collect)
        {
        	this.val = val;
        	this.declaringType = declaringType;
        	this.name = name;
        	if (is_in_collect)
        		this.imageIndex = 1;
        	val.Changed += delegate { OnChanged(new ListItemEventArgs(this)); };
        }
        
        public ValueItem(NamedValue val, DebugType declaringType)
        {
            this.val = val;
			this.declaringType = declaringType;
			
			try
            {
				//this.primitive = val.IsPrimitive;
            }
			catch(System.Exception e)
            {
                if (val.Type.IsByRef()) this.primitive = false;
            }
            val.Changed += delegate { OnChanged(new ListItemEventArgs(this)); };
        }

        public ValueItem(Mono.Debugging.Client.ObjectValue val)
        {
            this.monoValue = val;
            val.ValueChanged += delegate { OnChanged(new ListItemEventArgs(this)); };
        }

        [HandleProcessCorruptedStateExceptionsAttribute]
        public override bool SetText(string newValue)
        {
            try
            {
                monoValue.SetValue(newValue);
                return true;
            }
            catch (NotSupportedException)
            {

            }
            catch (System.Runtime.InteropServices.COMException)
            {

            }
            return false;
        }

        public override ContextMenuStrip GetContextMenu()
        {

            return null;
        }
    }

    class ArrayValueItem : ValueItem
    {
        Value arr;
        object low_bound;
        int ind;
        System.Text.Encoding enc = System.Text.Encoding.GetEncoding(1251);

        public ArrayValueItem(NamedValue val, DebugType type, Value arr, Value sz_arr, int ind):base(val,type)
        {
            this.arr = arr;
            this.ind = ind;
            System.Reflection.FieldInfo tmp_fi = AssemblyHelper.GetType(arr.Type.FullName).GetField("LowerIndex");
            if (!tmp_fi.FieldType.IsEnum)
                low_bound = tmp_fi.GetRawConstantValue();
            else
                low_bound = tmp_fi.GetValue(sz_arr);
        }

        public ArrayValueItem(Mono.Debugging.Client.ObjectValue val, Mono.Debugging.Client.ObjectValue arr, Mono.Debugging.Client.ObjectValue sz_arr, int ind)
        {

        }

        public override string Name
        {
            get
            {
                if (low_bound is int)
                    return "[" + (ind + (int)low_bound).ToString() + "]";
                else if (low_bound is byte)
                    return "[" + (ind + (byte)low_bound).ToString() + "]";
                else if (low_bound is sbyte)
                    return "[" + (ind + (sbyte)low_bound).ToString() + "]";
                else if (low_bound is short)
                    return "[" + (ind + (short)low_bound).ToString() + "]";
                else if (low_bound is ushort)
                    return "[" + (ind + (ushort)low_bound).ToString() + "]";
                else if (low_bound is uint)
                    return "[" + (ind + (uint)low_bound).ToString() + "]";
                else if (low_bound is long)
                    return "[" + (ind + (long)low_bound).ToString() + "]";
                else if (low_bound is char)
                    return "[" + "'" + enc.GetChars(new byte[] { (byte)(ind + enc.GetBytes(new char[] { (char)low_bound })[0]) })[0] + "'" + "]";
                else if (low_bound.GetType().IsEnum)
                    return "[" + Enum.GetName(low_bound.GetType(),ind+Convert.ToInt32(low_bound)) + "]";
                else
                    return base.Name;
            }
        }
    }

    public class BaseTypeItem : ListItem
    {
        DebugType type;
        Mono.Debugging.Evaluation.TypeValueReference monoType;


        public override bool IsLiteral
        {
            get
            {
                return false;
            }
        }

        public override int ImageIndex
        {
            get
            {
                return imageIndex;
            }
        }

        public override string Name
        {
            get
            {
                return "";// StringParser.Parse("${res:MainWindow.Windows.Debug.LocalVariables.BaseClass}");
            }
        }

        public override string Text
        {
            get
            {
            	
            	return "";
            }
        }

        public override bool CanEditText
        {
            get
            {
                return false;
            }
        }

        public override string Type
        {
            get
            {
            	return Mono.Debugging.Client.ObjectValue.GetPascalTypeName(monoType.Name);
            }
        }
		
        private int has_sub_items=-1;
        
        public override bool HasSubItems
        {
            get
            {
            	if (managed_type != null)
            	{
            		if (has_sub_items >= 0) 
            			return has_sub_items == 1;
            		if (managed_type.GetFields(System.Reflection.BindingFlags.FlattenHierarchy|System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Static).Length>0
            		    || managed_type.GetProperties(System.Reflection.BindingFlags.FlattenHierarchy|System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Static).Length>0)
            			has_sub_items = 1;
            		else 
            			has_sub_items = 0;
            		return has_sub_items == 1;
            	}
            	return true;
            }
        }
	
        public override IList<IListItem> SubItems
        {
            get
            {
                return GetSubItems(false);
            }
        }
		
        private int imageIndex = -1;
        private Type managed_type;
        
        public BaseTypeItem(DebugType type, Type mt)
        {
        	this.type = type;
        	this.managed_type = mt;
//        	if (mt.IsValueType)
//        		this.imageIndex = CodeCompletionProvider.ImagesProvider.IconNumberStruct;
//        	else
        		this.imageIndex = CodeCompletionProvider.ImagesProvider.IconNumberClass;
        }

        public BaseTypeItem(Mono.Debugging.Evaluation.TypeValueReference type, Type mt)
        {
            this.monoType = type;
            this.managed_type = mt;
            //        	if (mt.IsValueType)
            //        		this.imageIndex = CodeCompletionProvider.ImagesProvider.IconNumberStruct;
            //        	else
            this.imageIndex = CodeCompletionProvider.ImagesProvider.IconNumberClass;
        }

        public bool IsExtern()
        {
            return !type.Module.SymbolsLoaded;
        }
        [HandleProcessCorruptedStateExceptionsAttribute]
        List<IListItem> GetMembers(bool privateMembers)
        {
            List<IListItem> list = new List<IListItem>();
            var mis = monoType.GetChildReferences(Mono.Debugging.Client.EvaluationOptions.DefaultOptions);
            foreach (var mi in mis)
            {
                try
                {
                    if (!mi.Name.Contains("$"))
                    {
                        /*if (!privateMembers && ((mi.Flags & Mono.Debugging.Client.ObjectValueFlags.Public) == Mono.Debugging.Client.ObjectValueFlags.Public
                            || (mi.Flags & Mono.Debugging.Client.ObjectValueFlags.Internal) == Mono.Debugging.Client.ObjectValueFlags.Internal
                            || (mi.Flags & Mono.Debugging.Client.ObjectValueFlags.Protected) == Mono.Debugging.Client.ObjectValueFlags.Protected
                            || (mi.Flags & Mono.Debugging.Client.ObjectValueFlags.InternalProtected) == Mono.Debugging.Client.ObjectValueFlags.InternalProtected))
                            list.Add(new ValueItem(mi.CreateObjectValue(false, Mono.Debugging.Client.EvaluationOptions.DefaultOptions)));
                        else if (privateMembers && (mi.Flags & Mono.Debugging.Client.ObjectValueFlags.Private) == Mono.Debugging.Client.ObjectValueFlags.Private)
                            list.Add(new ValueItem(mi.CreateObjectValue(false, Mono.Debugging.Client.EvaluationOptions.DefaultOptions)));*/
                        list.Add(new ValueItem(mi.CreateObjectValue(false, Mono.Debugging.Client.EvaluationOptions.DefaultOptions)));
                    }
                }
                catch (System.Exception e)
                {
#if (DEBUG)
                    Console.WriteLine(e.Message+" "+e.StackTrace);
#endif
                }
            }
            System.Reflection.BindingFlags bf = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static;
            if (!privateMembers) 
                bf |= System.Reflection.BindingFlags.Public;
            else 
                bf |= System.Reflection.BindingFlags.NonPublic;
            System.Reflection.FieldInfo[] fis = managed_type.GetFields(bf);
            /*for (int i = 0; i < fis.Length; i++)
            {
                if (fis[i].IsLiteral)
                {
                    Value v = DebugUtils.MakeValue(fis[i].GetRawConstantValue());
                    list.Add(new ValueItem(v, fis[i].Name, v.Type));
                }
            }*/
            //return new List<ListItem>();
            return list;
        }

        [HandleProcessCorruptedStateExceptionsAttribute]
        IList<IListItem> GetSubItemsForCollection()
        {
        	List<IListItem> list = new List<IListItem>();
        	return list;
			
        }
        [HandleProcessCorruptedStateExceptionsAttribute]
        IList<IListItem> GetSubItems(bool is_raw)
        {
            List<IListItem> list = new List<IListItem>();
            try
            {
                if (DebugUtils.CheckForCollection(monoType) && !is_raw)
                {
                    IList<IListItem> lst = GetSubItemsForCollection();
                    if (lst != null) return lst;
                }
                string privateRes = PascalABCCompiler.StringResources.Get("DEBUG_VIEW_PRIVATE");//StringParser.Parse("${res:MainWindow.Windows.Debug.LocalVariables.PrivateMembers}");
                string staticRes = PascalABCCompiler.StringResources.Get("DEBUG_VIEW_STATIC");//StringParser.Parse("${res:MainWindow.Windows.Debug.LocalVariables.StaticMembers}");
                string privateStaticRes = PascalABCCompiler.StringResources.Get("DEBUG_VIEW_PRIVATE_STATIC");// StringParser.Parse("${res:MainWindow.Windows.Debug.LocalVariables.PrivateStaticMembers}");

                List<IListItem> publicStatic = GetMembers(false);
                //List<IListItem> privateStatic = GetMembers(true);

                
                list.AddRange(publicStatic);
                //if (privateStatic.Count > 0)
                //    list.Add(new FixedItem(-1, privateRes, String.Empty, String.Empty, true, privateStatic));
            }
            catch (System.Exception e)
            {

            }
            return list;
        }
    }

    public class FixedItem : ListItem
    {
        public int imageIndex;
        public string name;
        public string text;
        public string type;
        bool hasSubItems;
        IList<IListItem> subItems;
		public Value val;
		
        public FixedItem(string name, string text, string type)
        {
        	imageIndex = CodeCompletionProvider.ImagesProvider.IconNumberField;
        	this.name = name;
        	this.text = text;
        	this.type = type;
        }
        
        public FixedItem(string text, Value v)
        {
        	this.text = text;
        	this.val = v;
        }
        
        public override bool IsLiteral
        {
            get
            {
                return false;
            }
        }

        public override int ImageIndex
        {
            get
            {
                return imageIndex;
            }
        }

        public override string Name
        {
            get
            {
                return name;
            }
        }

        public override string Text
        {
            get
            {
                return text;
            }
        }

        public override bool CanEditText
        {
            get
            {
                return false;
            }
        }

        public override string Type
        {
            get
            {
                return type;
            }
        }

        public override bool HasSubItems
        {
            get
            {
                return hasSubItems;
            }
        }

        public override IList<IListItem> SubItems
        {
            get
            {
                return subItems;
            }
        }

        public FixedItem(int imageIndex, string name, string text, string type, bool hasSubItems, IList<IListItem> subItems)
        {
            this.imageIndex = imageIndex;
            this.name = name;
            this.text = text;
            this.type = type;
            this.hasSubItems = hasSubItems;
            this.subItems = subItems;
        }
    }

}
