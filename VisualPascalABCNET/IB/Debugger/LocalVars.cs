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
        private NamedValue val;
        private string ns_name;
        private Function func;
		private List<DebugType> types;
		
        /*public ModuleItem(NamedValue val, Function func)
        {
            this.val = val;
            ns_name = val.Type.FullName.Substring(0, val.Type.FullName.LastIndexOf('.'));
            this.func = func;
            this.func.Process.DebuggeeStateChanged += delegate
            {
                this.OnChanged(new ListItemEventArgs(this));
            };
        }*/
        
		public ModuleItem(List<DebugType> types, Function func)
        {
            //this.val = val;
            //ns_name = val.Type.FullName.Substring(0, val.Type.FullName.LastIndexOf('.'));
            this.types = types;
            this.func = func;
            this.func.Process.DebuggeeStateChanged += delegate
            {
                this.OnChanged(new ListItemEventArgs(this));
            };
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
                foreach (DebugType dt in types)
                {
                	//IList<FieldInfo> fis = val.Type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);  
                	IList<FieldInfo> fis = dt.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                	foreach (FieldInfo fi in fis)
                  	if (!fi.Name.Contains("$")) ret.Add(new ValueItem(fi.GetValue(null),fi.DeclaringType));
                }
                return ret;
            }
        }
    }

    public class FunctionItem : ListItem
    {
        Function function;

        public Function Function
        {
            get
            {
                return function;
            }
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
                return -1;
            }
        }

        public override string Name
        {
            get
            {
                return function.Name;
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
                foreach (NamedValue val in function.LocalVariables)
                {
                    if (val.Name.Contains("$class_var") /*&& !val.Type.FullName.Contains("$")*/)
                    {
                        //IList<FieldInfo> fis = val.Type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                        //if (fis.Count != 0)
                        List<DebugType> types = new List<DebugType>();
                        try
                        {
                            if (val.Type.FullName.Contains(PascalABCCompiler.StringConstants.ImplementationSectionNamespaceName))
                        	{
                                string interf_name = val.Type.FullName.Substring(0, val.Type.FullName.IndexOf(PascalABCCompiler.StringConstants.ImplementationSectionNamespaceName));
                        		Type t = AssemblyHelper.GetTypeForStatic(interf_name);
                        		DebugType dt = DebugUtils.GetDebugType(t);
                        		types.Add(dt);
                        	}
                            types.Add(val.Type);
                        }
                        catch (System.Exception e)
                        {
                        	
                        }
                        
                        ret.Add(new ModuleItem(types, function));
                        //foreach (FieldInfo fi in fis)
                        //  if (!fi.Name.Contains("$")) ret.Add(new ValueItem(fi.GetValue(val)));
                    }
                }
                foreach (NamedValue val in function.LocalVariables)
                {
                    //if (val.IsObject) 
                    
                    if (!val.Name.Contains("$"))
                    	ret.Add(new ValueItem(val,null));
                    else if (val.Name.Contains("$rv"))
                    {
                        ret.Add(new ValueItem(val,null));
                    }
                    else if (val.Name == "$disp$")
                    {
                        IList<FieldInfo> fields = val.Type.GetFields(BindingFlags.All);
                        foreach (FieldInfo fi in fields)
                        if (!fi.Name.Contains("$")) 
                        {
                        	ret.Add(new ValueItem(fi.GetValue(val),fi.DeclaringType));
                        	ht[fi.Name] = fi.Name;
                        }
                    }
                }
                NamedValue self_nv = null;
                try
                {
                    foreach (NamedValue val in function.Arguments)
                    {
                        if (!val.Name.Contains("$") && !ht.ContainsKey(val.Name))
                            ret.Add(new ValueItem(val, null));
                        else if (val.Name == "$obj$")
                            self_nv = val;
                    }
                }
                catch
                {
                }
                if (!function.IsStatic) ret.Add(new ValueItem(function.ThisValue,null));
                else if (self_nv != null)
                	ret.Add(new ValueItem(self_nv,"self",null));
                
                /*foreach (NamedValue val in function.ContaingClassVariables)
                {
                    if (!val.Name.Contains("$"))
                    ret.Add(new ValueItem(val));
                }*/
                return ret.AsReadOnly();
            }
        }

        public FunctionItem(Function function)
        {
            this.function = function;
            this.function.Process.DebuggeeStateChanged += delegate
            {
                this.OnChanged(new ListItemEventArgs(this));
            };
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
		public DebugType declaringType;
		
		public ValueItem() {}
        
        public override bool IsLiteral
        {
            [HandleProcessCorruptedStateExceptionsAttribute]
            get
            {
                try
                {
                    bool b = val.IsObject;
                    return false;
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
            	if (!(val is NamedValue)) return 1;
            	if (imageIndex != -1) return imageIndex;
            	if (declaringType == null) return 1;
            	//Type declType = AssemblyHelper.GetType(declaringType.FullName);
            	//if (declType == null) return 1;
            	//System.Reflection.MemberInfo[] mis = declType.GetMembers(System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic);//declType.GetMember(val.Name,System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic);
            	IList<MemberInfo> mis = declaringType.GetMember((val is NamedValue)?(val as NamedValue).Name:name, BindingFlags.All);
            	if (mis == null || mis.Count == 0)
            	{
                    try
                    {
                        DebugType tmp = declaringType.BaseType;
                        while ((mis == null || mis.Count == 0) && tmp != null)
                        {
                            mis = tmp.GetMember((val is NamedValue) ? (val as NamedValue).Name : name, BindingFlags.All);
                            tmp = tmp.BaseType;
                        }
                    }
                    catch (System.Exception e)
                    {

                    }
            	}
            	if (mis == null || mis.Count == 0)
                {
                	
            		if (val.IsObject || val.Type.IsByRef() && !val.IsPrimitive)
                	{
                    	imageIndex = -1;
                		return 1; // Class
                	}
                	else
                	{
                    	imageIndex = 1;
                		return 1; // Field
                	}
                }
                else
                {
                	if (mis[0] is FieldInfo)
                	{
                		FieldInfo fi = mis[0] as FieldInfo;
                		if (fi.IsPrivate)
                		{
                			imageIndex = CodeCompletionProvider.ImagesProvider.IconNumberPrivateField;
                		}
                		else
                		{
                			imageIndex = CodeCompletionProvider.ImagesProvider.IconNumberField;
                		}
                		/*else if (fi.IsFamily || fi.IsFamilyAndAssembly)
                		{
                			imageIndex = CodeCompletionProvider.ImagesProvider.IconNumberProtectedField;
                		}
                		else if (fi.IsAssembly || fi.IsFamilyOrAssembly)
                		{
                			imageIndex = CodeCompletionProvider.ImagesProvider.IconNumberInternalField;
                		}
                		else if (fi.IsPrivate)
                		{
                			imageIndex = CodeCompletionProvider.ImagesProvider.IconNumberPrivateField;
                		}*/
                	}
                	else if (mis[0] is PropertyInfo)
                	{
                		MethodInfo fi = (mis[0] as PropertyInfo).GetGetMethod();
                		if (fi == null) return -1;
                		if (fi.IsPrivate)
                		{
                			imageIndex = CodeCompletionProvider.ImagesProvider.IconNumberPrivateProperty;
                		}
                		else
                		{
                			imageIndex = CodeCompletionProvider.ImagesProvider.IconNumberProperty;
                		}
                		/*else if (fi.IsFamily || fi.IsFamilyAndAssembly)
                		{
                			imageIndex = CodeCompletionProvider.ImagesProvider.IconNumberProtectedProperty;
                		}
                		else if (fi.IsAssembly || fi.IsFamilyOrAssembly)
                		{
                			imageIndex = CodeCompletionProvider.ImagesProvider.IconNumberInternalProperty;
                		}
                		else if (fi.IsPrivate)
                		{
                			imageIndex = CodeCompletionProvider.ImagesProvider.IconNumberPrivateProperty;
                		}*/
                	}
                	return imageIndex;
                }
                
                //return 1;
            }
        }
        
        public override string Name
        {
            [HandleProcessCorruptedStateExceptionsAttribute]
            get
            {
                try
                {
                    if (!string.IsNullOrEmpty(name))
                        return name;
                    if (!((val is NamedValue) ? (val as NamedValue).Name : name).Contains("$rv"))
                        return (val is NamedValue) ? ((val as NamedValue).Name.IndexOf(':') != -1 ? (val as NamedValue).Name.Substring(0, (val as NamedValue).Name.IndexOf(':')) : (val as NamedValue).Name) : name;
                    return "Result";
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
                if (ShowValuesInHexadecimal && val.IsInteger)
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
                                if (WorkbenchServiceFactory.DebuggerManager.language != null)
                                    return WorkbenchServiceFactory.DebuggerManager.language.LanguageIntellisenseSupport.GetKeyword(PascalABCCompiler.Parsers.SymbolKind.Null);
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
                }
            }
        }
		
        private bool failed = false;
        
        public override bool CanEditText
        {
            [HandleProcessCorruptedStateExceptionsAttribute]
            get
            {
                return false;
            	try
            	{
            		if (!failed)
            			return val.IsInteger && !ShowValuesInHexadecimal || val.IsPrimitive;
            		else 
            			return false;
            	}
            	catch
            	{
            		return false;
            	}
            }
        }

        public override string Type
        {
            [HandleProcessCorruptedStateExceptionsAttribute]
            get
            {
                try
                {
            		if (val.Type != null)
                	{
            			return DebugUtils.WrapTypeName(val.Type);
                	}
                	else
                	{
                    	return String.Empty;
                	}
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
                return val.IsObject;
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
                	if (IsEnum() || val.Type.ManagedType == typeof(string)) return false;
                    return val.IsObject || val.IsArray;
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
                if (val.IsArray)
                {
                    foreach (NamedValue element in val.GetArrayElements())
                    {
                        list.Add(new ValueItem(element,null));
                    }
                }
                if (val.IsObject || val.Type.IsByRef() && !val.IsPrimitive)
                {
                    //if (IsArrayWrap())
                    //{
                    NamedValue nv = GetNullBasedArray();
                    if (nv != null)
                    {
                        int i = 0;
                        foreach (NamedValue element in nv.GetArrayElements())
                        {
                            list.Add(new ArrayValueItem(element, null, val, nv, i++));

                        }
                    }
                    //}
                    else
                    {
                        if (!val.Type.IsByRef())
                            return new BaseTypeItem(val, val.Type).SubItems;
                    }
                }
                return list;
            }
        }

        private NamedValue GetNullBasedArray()
        {
            IList<FieldInfo> flds = val.Type.GetFields();
            if (flds.Count != 3) return null;
            foreach (FieldInfo fi in flds)
                if (fi.Name == "NullBasedArray") return fi.GetValue(val);
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
        [HandleProcessCorruptedStateExceptionsAttribute]
        public override bool SetText(string newValue)
        {
            try
            {
                val.PrimitiveValue = newValue;
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
        Value val;
        DebugType type;

        public Value Value
        {
            get
            {
                return val;
            }
        }

        public override bool IsLiteral
        {
            get
            {
                return false;
            }
        }

        public DebugType DebugType
        {
            get
            {
                return type;
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
            	return DebugUtils.WrapTypeName(type);
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
        
        public BaseTypeItem(Value val, DebugType type)
        {
            this.val = val;
            this.type = type;
        }

        public bool IsExtern()
        {
            return !type.Module.SymbolsLoaded;
        }
        [HandleProcessCorruptedStateExceptionsAttribute]
        List<IListItem> GetMembers(BindingFlags flags)
        {
            List<IListItem> list = new List<IListItem>();
            if (val != null)
            {
            	NamedValueCollection nvc = val.GetMembers(type, flags);
            	foreach (NamedValue v in nvc)
            	{
                	try
                	{
                        if (v is MemberValue && (v as MemberValue).MemberInfo is PropertyInfo && ((v as MemberValue).MemberInfo as PropertyInfo).GetGetMethod().ParameterCount > 0)
                            continue;
                        list.Add(new ValueItem(v,this.DebugType));
                	}
                	catch (System.Exception)
                	{
                	}
            	}
            }
            else
            {
            	IList<MemberInfo> mis = type.GetMembers(flags);
            	for (int i=0; i<mis.Count; i++)
            	{
            		try
            		{
                        if (mis[i].IsStatic && !mis[i].Name.Contains("$"))
                        {
                            if (mis[i] is FieldInfo)
                            {
                                FieldInfo fi = mis[i] as FieldInfo;
                                Value v = fi.GetValue(null);
                                list.Add(new ValueItem(v as NamedValue, this.DebugType));
                            }
                            else if (mis[i] is PropertyInfo)
                            {
                                PropertyInfo pi = mis[i] as PropertyInfo;
                                Value v = pi.GetValue(null);
                                list.Add(new ValueItem(v as NamedValue, this.DebugType));
                            }
                        }
            		}
            		catch(System.Exception e)
            		{
            			
            		}
            	}
            	System.Reflection.BindingFlags bf = System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.Static;
            	if ((flags & BindingFlags.Public) == BindingFlags.Public) bf |= System.Reflection.BindingFlags.Public;
            	else bf |= System.Reflection.BindingFlags.NonPublic;
            	System.Reflection.FieldInfo[] fis = managed_type.GetFields(bf);
            	for (int i=0; i<fis.Length; i++)
            	{
            		if (fis[i].IsLiteral)
            		{
            			Value v = DebugUtils.MakeValue(fis[i].GetRawConstantValue());
            			list.Add(new ValueItem(v,fis[i].Name,v.Type));
            		}
            	}
            }
            //return new List<ListItem>();
            return list;
        }
        [HandleProcessCorruptedStateExceptionsAttribute]
        IList<IListItem> GetSubItemsForCollection()
        {
        	List<IListItem> list = new List<IListItem>();
        	if (val == null) return list;
			try
			{
				int count = (int)val.GetMember("Count").PrimitiveValue;
				if (DebugUtils.IsStack(type) || DebugUtils.IsQueue(type))
				{
					Value nv = val.GetMember("_array");
					NamedValueCollection nvc = nv.GetArrayElements();
					count = (count < 10)?count:10;
					for (int i=0; i<count; i++)
					{
						list.Add(new ValueItem(nvc[i],nvc[i].Type));
					}
				}
				else if (DebugUtils.IsDictionary(type))
				{
					NamedValue nv = val.GetMember("entries");
					NamedValueCollection nvc = nv.GetArrayElements();
					count = (count < 10)?count:10;
					for (int i=0; i<count; i++)
					{
						nv = nvc[i];
						Value key = nv.GetMember("key");
						Value _val = nv.GetMember("value");
						
						list.Add(new PairValueItem(key as NamedValue,_val as NamedValue,"["+i.ToString()+"]"));
					}
				}
				else
				if (DebugUtils.IsHashtable(type))
				{
					NamedValue nv = val.GetMember("buckets");
					NamedValueCollection nvc = nv.GetArrayElements();
					count = 0;
					for (int i=0; i<nvc.Count; i++)
					{
						nv = nvc[i];
						Value key = nv.GetMember("key");
						Value _val = nv.GetMember("val");
						if (!key.IsNull)
						{
							list.Add(new PairValueItem(key as NamedValue,_val as NamedValue,"["+count.ToString()+"]"));
							if (count > 10) break;
							count++;
						}
					}
				}
				else if (DebugUtils.IsSortedList(type))
				{
					NamedValue nv_keys = val.GetMember("keys");
					NamedValue nv_values = val.GetMember("values");
					count = (int)val.GetMember("Count").PrimitiveValue;
					NamedValueCollection nvc_keys = nv_keys.GetArrayElements();
					NamedValueCollection nvc_values = nv_values.GetArrayElements();
					for (int i=0; i<count; i++)
					{
						list.Add(new PairValueItem(nvc_keys[i],nvc_values[i],"["+i.ToString()+"]"));
						if (i > 10) break;
					}
				}
				else if (DebugUtils.IsSortedDictionary(type))
				{
					NamedValue nv = val.GetMember("_set").GetMember("root");
					int i = 0;
					while (i<=10 && !nv.IsNull)
					{
						NamedValue tmp = nv.GetMember("item");
						list.Add(new PairValueItem(tmp.GetMember("key"),tmp.GetMember("value"),"["+i.ToString()+"]"));
						nv = nv.GetMember("right");
						i++;
					}
				}
				else if (DebugUtils.IsLinkedList(type))
				{
					NamedValue nv = val.GetMember("First");
					int i = 0;
					while (i<=10 && !nv.IsNull)
					{
						list.Add(new ValueItem(nv.GetMember("Value"),"["+i.ToString()+"]",nv.Type,true));
						nv = nv.GetMember("Next");
						i++;
					}
				}
				else
				{
					MethodInfo mi = type.GetMember("get_Item",BindingFlags.All)[0] as MethodInfo;
					count = (count < 100)?count:100;
					for (int i=0; i<count; i++)
					{
						Value v = mi.Invoke(val,new Value[1]{DebugUtils.MakeValue(i)});
						list.Add(new ValueItem(v,"["+i.ToString()+"]",v.Type));
					}
				}
				IList<IListItem> raw_items = GetSubItems(true);
				list.Add(new FixedItem(-1,PascalABCCompiler.StringResources.Get("DEBUG_VIEW_RAW_VIEW"),string.Empty,string.Empty,true,raw_items));
			}
			catch (System.Exception e)
			{
				return null;
			}
			return list;
        }
        [HandleProcessCorruptedStateExceptionsAttribute]
        IList<IListItem> GetSubItems(bool is_raw)
        {
            List<IListItem> list = new List<IListItem>();
            try
            {
                if (DebugUtils.CheckForCollection(type) && !is_raw)
                {
                    IList<IListItem> lst = GetSubItemsForCollection();
                    if (lst != null) return lst;
                }
                string privateRes = PascalABCCompiler.StringResources.Get("DEBUG_VIEW_PRIVATE");//StringParser.Parse("${res:MainWindow.Windows.Debug.LocalVariables.PrivateMembers}");
                string staticRes = PascalABCCompiler.StringResources.Get("DEBUG_VIEW_STATIC");//StringParser.Parse("${res:MainWindow.Windows.Debug.LocalVariables.StaticMembers}");
                string privateStaticRes = PascalABCCompiler.StringResources.Get("DEBUG_VIEW_PRIVATE_STATIC");// StringParser.Parse("${res:MainWindow.Windows.Debug.LocalVariables.PrivateStaticMembers}");

                List<IListItem> publicInstance = null;
                List<IListItem> privateInstance = null;

                if (val != null)
                {
                    publicInstance = GetMembers(BindingFlags.Public | BindingFlags.Instance);
                    privateInstance = GetMembers(BindingFlags.NonPublic | BindingFlags.Instance);
                }
                List<IListItem> publicStatic = GetMembers(BindingFlags.Public | BindingFlags.Static);
                List<IListItem> privateStatic = GetMembers(BindingFlags.NonPublic | BindingFlags.Static);

                if (type.BaseType != null && type.BaseType.FullName != "System.Object" && type.BaseType.FullName != "System.ValueType")
                {
                    list.Add(new BaseTypeItem(val, type.BaseType));
                }
                if (val != null)
                {
                    list.AddRange(publicInstance);
                    if (publicStatic.Count > 0)
                    {
                        list.Add(new FixedItem(-1, staticRes, String.Empty, String.Empty, true, publicStatic));
                    }

                    if (privateInstance.Count > 0 || privateStatic.Count > 0)
                    {
                        List<IListItem> nonPublicItems = new List<IListItem>();
                        if (privateStatic.Count > 0)
                        {
                            nonPublicItems.Add(new FixedItem(-1, privateStaticRes, String.Empty, String.Empty, true, privateStatic));
                        }
                        nonPublicItems.AddRange(privateInstance);
                        list.Add(new FixedItem(-1, privateRes, String.Empty, String.Empty, true, nonPublicItems));
                    }
                }
                else
                {
                    list.AddRange(publicStatic);
                    if (privateStatic.Count > 0)
                        list.Add(new FixedItem(-1, privateRes, String.Empty, String.Empty, true, privateStatic));
                }
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
