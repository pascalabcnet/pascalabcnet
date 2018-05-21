// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using PascalABCCompiler.SemanticTree;
using System.Threading;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics.SymbolStore;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Security;
using System.Runtime.Versioning;

namespace PascalABCCompiler.NETGenerator
{

    public enum TargetType
    {
        Exe,
        Dll,
        WinExe
    }

    public enum DebugAttributes
    {
        Debug,
        ForDebugging,
        Release
    }


    //compiler options class
    public class CompilerOptions
    {
        public enum PlatformTarget { x64, x86, AnyCPU };

        public TargetType target = TargetType.Exe;
        public DebugAttributes dbg_attrs = DebugAttributes.Release;
        public bool optimize = false;
        public bool ForRunningWithEnvironment = false;
        public bool GenerateDebugInfoForInitalValue = true;

        public bool NeedDefineVersionInfo = false;
        private string _Product = "";
        private PlatformTarget _platformtarget = PlatformTarget.AnyCPU;

        public PlatformTarget platformtarget
        {
            get { return _platformtarget; }
            set { _platformtarget = value; }
        }

        public string Product
        {
            get { return _Product; }
            set { _Product = value; NeedDefineVersionInfo = true; }
        }
        private string _ProductVersion = "";

        public string ProductVersion
        {
            get { return _ProductVersion; }
            set { _ProductVersion = value; NeedDefineVersionInfo = true; }
        }
        private string _Company = "";

        public string Company
        {
            get { return _Company; }
            set { _Company = value; NeedDefineVersionInfo = true; }
        }
        private string _Copyright = "";

        public string Copyright
        {
            get { return _Copyright; }
            set { _Copyright = value; NeedDefineVersionInfo = true; }
        }
        private string _TradeMark = "";

        public string TradeMark
        {
            get { return _TradeMark; }
            set { _TradeMark = value; NeedDefineVersionInfo = true; }
        }

        public string MainResourceFileName = null;

        public byte[] MainResourceData = null;
        public CompilerOptions() { }
    }

    /// <summary>
    /// Класс, переводящий сем. дерево в сборку .NET
    /// </summary>
    public class ILConverter : AbstractVisitor
    {
        protected AppDomain ad;//домен приложения (в нем будет генерироваться сборка)
        protected AssemblyName an;//имя сборки
        protected AssemblyBuilder ab;//билдер для сборки
        protected ModuleBuilder mb;//билдер для модуля
        protected TypeBuilder entry_type;//тип-обертка над осн. программой
        protected TypeBuilder cur_type;//текущий компилируемый тип
        protected MethodBuilder entry_meth;//входная точка в приложение
        protected MethodBuilder cur_meth;//текущий билдер для метода
        protected MethodBuilder init_variables_mb;
        protected ILGenerator il;//стандартный класс для генерации IL-кода
        protected ISymbolDocumentWriter doc;//класс для генерации отладочной информации
        protected ISymbolDocumentWriter first_doc;//класс для генерации отладочной информации
        protected Stack<Label> labels = new Stack<Label>();//стек меток для break
        protected Stack<Label> clabels = new Stack<Label>();//стек меток для continue
        protected Stack<MethInfo> smi = new Stack<MethInfo>();//стек вложенных функций
        protected Helper helper = new Helper();//привязывает классы сем. дерева к нетовским билдерам
        protected int num_scope = 0;//уровень вложенности
        protected List<TypeBuilder> types = new List<TypeBuilder>();//список закрытия типов
        protected List<TypeBuilder> value_types = new List<TypeBuilder>();//список закрытия размерных типов (треб. особый порядок)
        protected int uid = 1;//счетчик для задания уникальных имен (исп. при именовании классов-оболочек над влож. ф-ми)
        protected List<ICommonFunctionNode> funcs = new List<ICommonFunctionNode>();//
        protected bool is_addr = false;//флаг, передается ли значение как факт. var-параметр
        protected string cur_unit;//имя текущего модуля
        protected ConstructorBuilder cur_cnstr;//текущий конструктор - тоже нужен (ssyy)
        protected bool is_dot_expr = false;//флаг, стоит ли после выражения точка (нужно для упаковки размерных типов)
        protected TypeInfo cur_ti;//текущий клас
        protected CompilerOptions comp_opt = new CompilerOptions();//опции компилятора
        protected Dictionary<string, ISymbolDocumentWriter> sym_docs = new Dictionary<string, ISymbolDocumentWriter>();//таблица отладочных документов
        protected bool is_constructor = false;//флаг, переводим ли мы конструктор
        protected bool init_call_awaited = false;
        protected bool save_debug_info = false;
        protected bool add_special_debug_variables = false;
        protected bool make_next_spoint = true;
        protected SemanticTree.ILocation EntryPointLocation;
        protected Label ExitLabel;//метка для выхода из процедуры
        protected bool ExitProcedureCall = false; //призна того что всертиласть exit и надо пометиь коней процедуры
        protected Dictionary<IConstantNode, FieldBuilder> ConvertedConstants = new Dictionary<IConstantNode, FieldBuilder>();
        //ivan
        protected List<EnumBuilder> enums = new List<EnumBuilder>();
        protected List<TypeBuilder> NamespaceTypesList = new List<TypeBuilder>();
        protected TypeBuilder cur_unit_type;
        private Dictionary<IFunctionNode, IFunctionNode> prop_accessors = new Dictionary<IFunctionNode, IFunctionNode>();
        //ssyy
        private const int num_try_save = 10; //Кол-во попыток сохранения
        private ICommonTypeNode converting_generic_param = null;
        private Dictionary<ICommonFunctionNode, List<IGenericTypeInstance>> instances_in_functions =
            new Dictionary<ICommonFunctionNode, List<IGenericTypeInstance>>();

        private static MethodInfo ActivatorCreateInstance = typeof(Activator).GetMethod("CreateInstance", Type.EmptyTypes);
        //\ssyy

        private Dictionary<TypeBuilder, TypeBuilder> marked_with_extension_attribute = new Dictionary<TypeBuilder, TypeBuilder>();

        private LocalBuilder current_index_lb;
        private bool has_dereferences = false;
        private bool safe_block = false;
        private int cur_line = 0;
        private ISymbolDocumentWriter new_doc;
        private List<LocalBuilder> pinned_variables = new List<LocalBuilder>();
        private bool pabc_rtl_converted = false;

        private void CheckLocation(SemanticTree.ILocation Location)
        {
            if (Location != null)
            {
                ISymbolDocumentWriter temp_doc = null;
                if (sym_docs.ContainsKey(Location.document.file_name))
                {
                    temp_doc = sym_docs[Location.document.file_name];
                }
                else
                if (save_debug_info) // иногда вызывается MarkSequencePoint при save_debug_info = false
                {
                    temp_doc = mb.DefineDocument(Location.document.file_name, SymDocumentType.Text, SymLanguageType.Pascal, SymLanguageVendor.Microsoft);
                    sym_docs.Add(Location.document.file_name, temp_doc);
                }
                if (temp_doc != doc)
                {
                    doc = temp_doc;
                    cur_line = -1;
                }
            }
        }

        private bool OnNextLine(ILocation loc)
        {
            if (doc != new_doc)
            {
                new_doc = doc;
                cur_line = loc.begin_line_num;
                return true;
            }
            if (loc.begin_line_num == cur_line) return false;
            cur_line = loc.begin_line_num;
            return true;
        }

        public bool EnterSafeBlock()
        {
            bool tmp = safe_block;
            safe_block = true;
            return tmp;
        }

        public void LeaveSafeBlock(bool value)
        {
            safe_block = value;
        }

        protected void MarkSequencePoint(SemanticTree.ILocation Location)
        {
            CheckLocation(Location);
            if (Location != null && OnNextLine(Location))
                MarkSequencePoint(il, Location);
        }

        protected void MarkSequencePointToEntryPoint(ILGenerator ilg)
        {
            MarkSequencePoint(ilg, EntryPointLocation);
        }

        protected void MarkSequencePoint(ILGenerator ilg, SemanticTree.ILocation Location)
        {
            if (Location != null)
            {
                CheckLocation(Location);
                MarkSequencePoint(ilg, Location.begin_line_num, Location.begin_column_num, Location.end_line_num, Location.end_column_num);
            }
        }

        protected void MarkSequencePoint(ILGenerator ilg, int bl, int bc, int el, int ec)
        {
            //if (make_next_spoint)
            ilg.MarkSequencePoint(doc, bl, bc, el, ec + 1);
            make_next_spoint = true;
        }

        private Hashtable StandartDirectories;

        public ILConverter(Hashtable StandartDirectories)
        {
            this.StandartDirectories = StandartDirectories;
        }

        int TempNamesCount = 0;
        private string GetTempName()
        {
            return String.Format("$PABCNET_TN{0}$", TempNamesCount++);
        }

        protected FieldBuilder GetConvertedConstants(IConstantNode c)
        {
            if (ConvertedConstants.ContainsKey(c))
                return ConvertedConstants[c];
            ILGenerator ilb = il;
            il = ModulesInitILGenerators[cur_unit_type];
            ConvertConstantDefinitionNode(null, GetTempName(), c.type, c);
            il = ilb;
            return ConvertedConstants[c];
        }

        private Dictionary<TypeBuilder, ILGenerator> ModulesInitILGenerators = new Dictionary<TypeBuilder, ILGenerator>();

        private ConstructorBuilder fileOfAttributeConstructor;

        private ConstructorBuilder FileOfAttributeConstructor
        {
            get
            {
                if (fileOfAttributeConstructor != null) return fileOfAttributeConstructor;
                TypeBuilder tb = mb.DefineType(PascalABCCompiler.TreeConverter.compiler_string_consts.file_of_attr_name, TypeAttributes.Public | TypeAttributes.BeforeFieldInit, typeof(Attribute));
                types.Add(tb);
                fileOfAttributeConstructor = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, new Type[1] { TypeFactory.ObjectType });
                FieldBuilder fld = tb.DefineField("Type", TypeFactory.ObjectType, FieldAttributes.Public);
                ILGenerator cnstr_il = fileOfAttributeConstructor.GetILGenerator();
                cnstr_il.Emit(OpCodes.Ldarg_0);
                cnstr_il.Emit(OpCodes.Ldarg_1);
                cnstr_il.Emit(OpCodes.Stfld, fld);
                cnstr_il.Emit(OpCodes.Ret);
                return fileOfAttributeConstructor;
            }
        }

        private ConstructorBuilder setOfAttributeConstructor;

        private ConstructorBuilder SetOfAttributeConstructor
        {
            get
            {
                if (setOfAttributeConstructor != null) return setOfAttributeConstructor;
                TypeBuilder tb = mb.DefineType(PascalABCCompiler.TreeConverter.compiler_string_consts.set_of_attr_name, TypeAttributes.Public | TypeAttributes.BeforeFieldInit, typeof(Attribute));
                types.Add(tb);
                setOfAttributeConstructor = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, new Type[1] { TypeFactory.ObjectType });
                FieldBuilder fld = tb.DefineField("Type", TypeFactory.ObjectType, FieldAttributes.Public);
                ILGenerator cnstr_il = setOfAttributeConstructor.GetILGenerator();
                cnstr_il.Emit(OpCodes.Ldarg_0);
                cnstr_il.Emit(OpCodes.Ldarg_1);
                cnstr_il.Emit(OpCodes.Stfld, fld);
                cnstr_il.Emit(OpCodes.Ret);

                return setOfAttributeConstructor;
            }
        }

        private ConstructorBuilder templateClassAttributeConstructor;

        private ConstructorBuilder TemplateClassAttributeConstructor
        {
            get
            {
                if (templateClassAttributeConstructor != null) return templateClassAttributeConstructor;
                TypeBuilder tb = mb.DefineType(PascalABCCompiler.TreeConverter.compiler_string_consts.template_class_attr_name, TypeAttributes.Public | TypeAttributes.BeforeFieldInit, typeof(Attribute));
                types.Add(tb);
                templateClassAttributeConstructor = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, new Type[1] { TypeFactory.ByteType.MakeArrayType() });
                FieldBuilder fld = tb.DefineField("Tree", TypeFactory.ByteType.MakeArrayType(), FieldAttributes.Public);
                ILGenerator cnstr_il = templateClassAttributeConstructor.GetILGenerator();
                cnstr_il.Emit(OpCodes.Ldarg_0);
                cnstr_il.Emit(OpCodes.Ldarg_1);
                cnstr_il.Emit(OpCodes.Stfld, fld);
                cnstr_il.Emit(OpCodes.Ret);

                return templateClassAttributeConstructor;
            }
        }

        private ConstructorBuilder typeSynonimAttributeConstructor;

        private ConstructorBuilder TypeSynonimAttributeConstructor
        {
            get
            {
                if (typeSynonimAttributeConstructor != null) return typeSynonimAttributeConstructor;
                TypeBuilder tb = mb.DefineType(PascalABCCompiler.TreeConverter.compiler_string_consts.type_synonim_attr_name, TypeAttributes.Public | TypeAttributes.BeforeFieldInit, typeof(Attribute));
                types.Add(tb);
                typeSynonimAttributeConstructor = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, new Type[1] { TypeFactory.ObjectType });
                FieldBuilder fld = tb.DefineField("Type", TypeFactory.ObjectType, FieldAttributes.Public);
                ILGenerator cnstr_il = typeSynonimAttributeConstructor.GetILGenerator();
                cnstr_il.Emit(OpCodes.Ldarg_0);
                cnstr_il.Emit(OpCodes.Ldarg_1);
                cnstr_il.Emit(OpCodes.Stfld, fld);
                cnstr_il.Emit(OpCodes.Ret);

                return typeSynonimAttributeConstructor;
            }
        }

        private ConstructorBuilder shortStringAttributeConstructor;

        private ConstructorBuilder ShortStringAttributeConstructor
        {
            get
            {
                if (shortStringAttributeConstructor != null) return shortStringAttributeConstructor;
                TypeBuilder tb = mb.DefineType(PascalABCCompiler.TreeConverter.compiler_string_consts.short_string_attr_name, TypeAttributes.Public | TypeAttributes.BeforeFieldInit, typeof(Attribute));
                types.Add(tb);
                shortStringAttributeConstructor = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, new Type[1] { TypeFactory.Int32Type });
                FieldBuilder fld = tb.DefineField("Length", TypeFactory.Int32Type, FieldAttributes.Public);
                ILGenerator cnstr_il = shortStringAttributeConstructor.GetILGenerator();
                cnstr_il.Emit(OpCodes.Ldarg_0);
                cnstr_il.Emit(OpCodes.Ldarg_1);
                cnstr_il.Emit(OpCodes.Stfld, fld);
                cnstr_il.Emit(OpCodes.Ret);

                return shortStringAttributeConstructor;
            }
        }

        private ICommonFunctionNode GetGenericFunctionContainer(ITypeNode tn)
        {
            if (tn.common_generic_function_container != null)
            {
                return tn.common_generic_function_container;
            }
            if (tn.type_special_kind == type_special_kind.typed_file)
            {
                return GetGenericFunctionContainer(tn.element_type);
            }
            if (tn.type_special_kind == type_special_kind.set_type)
            {
                return GetGenericFunctionContainer(tn.element_type);
            }
            if (tn.type_special_kind == type_special_kind.array_kind)
            {
                return GetGenericFunctionContainer(tn.element_type);
            }
            IRefTypeNode ir = tn as IRefTypeNode;
            if (ir != null)
            {
                return GetGenericFunctionContainer(ir.pointed_type);
            }
            IGenericTypeInstance igti = tn as IGenericTypeInstance;
            if (igti != null)
            {
                foreach (ITypeNode par in igti.generic_parameters)
                {
                    ICommonFunctionNode rez = GetGenericFunctionContainer(par);
                    if (rez != null)
                    {
                        return rez;
                    }
                }
            }
            return null;
        }

        private void AddTypeInstanceToFunction(ICommonFunctionNode func, IGenericTypeInstance gti)
        {
            if (func == null)
                return;
            List<IGenericTypeInstance> instances;
            //if (func == null) // SSM 3.07.16 Это решает проблему с оставшимся после перевода в сем. дерево узлом IEnumerable<UnknownType>, но очень грубо - пробую найти ошибку раньше
            //    return;
            bool found = instances_in_functions.TryGetValue(func, out instances);
            if (!found)
            {
                instances = new List<IGenericTypeInstance>();
                instances_in_functions.Add(func, instances);
            }
            if (!instances.Contains(gti))
            {
                instances.Add(gti);
            }
        }

        //Метод, переводящий семантическое дерево в сборку .NET
        public void ConvertFromTree(SemanticTree.IProgramNode p, string TargetFileName, string SourceFileName, CompilerOptions options, string[] ResourceFiles)
        {
            //SystemLibrary.SystemLibInitializer.RestoreStandardFunctions();
            bool RunOnly = false;
            string fname = TargetFileName;
            comp_opt = options;
            ad = Thread.GetDomain(); //получаем домен приложения
            an = new AssemblyName(); //создаем имя сборки
            an.Version = new Version("1.0.0.0");
            string dir = Directory.GetCurrentDirectory();
            string source_name = fname;//p.Location.document.file_name;
            int pos = source_name.LastIndexOf(Path.DirectorySeparatorChar);
            if (pos != -1) //если имя файла указано с путем, то выделяем
            {
                dir = source_name.Substring(0, pos + 1);
                an.CodeBase = String.Concat("file:///", source_name.Substring(0, pos));
                source_name = source_name.Substring(pos + 1);
            }
            string name = source_name.Substring(0, source_name.LastIndexOf('.'));
            if (comp_opt.target == TargetType.Exe || comp_opt.target == TargetType.WinExe)
                an.Name = name;// + ".exe";
            else an.Name = name; //+ ".dll";

            if (name == "PABCRtl" || name == "PABCRtl32")
            {
                pabc_rtl_converted = true;
                an.Flags = AssemblyNameFlags.PublicKey;
                an.VersionCompatibility = System.Configuration.Assemblies.AssemblyVersionCompatibility.SameProcess;
                an.HashAlgorithm = System.Configuration.Assemblies.AssemblyHashAlgorithm.None;
                FileStream publicKeyStream = File.Open(Path.Combine(Path.GetDirectoryName(TargetFileName), name == "PABCRtl" ? "PublicKey.snk" : "PublicKey32.snk"), FileMode.Open);
                byte[] publicKey = new byte[publicKeyStream.Length];
                publicKeyStream.Read(publicKey, 0, (int)publicKeyStream.Length);
                // Provide the assembly with a public key.
                an.SetPublicKey(publicKey);
                publicKeyStream.Close();
            }
            if (RunOnly)
                ab = ad.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run, dir);//определяем сборку
            else
                ab = ad.DefineDynamicAssembly(an, AssemblyBuilderAccess.Save, dir);//определяем сборку

            //int nn = ad.GetAssemblies().Length;
            if (options.NeedDefineVersionInfo)
            {
                ab.DefineVersionInfoResource(options.Product, options.ProductVersion, options.Company,
                    options.Copyright, options.TradeMark);
            }
            if (options.MainResourceFileName != null)
            {
                try
                {
                    ab.DefineUnmanagedResource(options.MainResourceFileName);
                }
                catch
                {
                    throw new TreeConverter.SourceFileError(options.MainResourceFileName);
                }
            }
            else if (options.MainResourceData != null)
            {
                try
                {
                    ab.DefineUnmanagedResource(options.MainResourceData);
                }
                catch
                {
                    throw new TreeConverter.SourceFileError("");
                }
            }
            save_debug_info = comp_opt.dbg_attrs == DebugAttributes.Debug || comp_opt.dbg_attrs == DebugAttributes.ForDebugging;
            add_special_debug_variables = comp_opt.dbg_attrs == DebugAttributes.ForDebugging;

            //bool emit_sym = true;
            if (save_debug_info) //если модуль отладочный, то устанавливаем атрибут, запрещающий inline методов
                ab.SetCustomAttribute(typeof(System.Diagnostics.DebuggableAttribute).GetConstructor(new Type[] { typeof(bool), typeof(bool) }), new byte[] { 0x01, 0x00, 0x01, 0x01, 0x00, 0x00 });
            if (RunOnly)
                mb = ab.DefineDynamicModule(name, save_debug_info);
            else
            {
                if (comp_opt.target == TargetType.Exe || comp_opt.target == TargetType.WinExe)
                    mb = ab.DefineDynamicModule(name + ".exe", an.Name + ".exe", save_debug_info); //определяем модуль (save_debug_info - флаг включать отладочную информацию)
                else
                    mb = ab.DefineDynamicModule(name + ".dll", an.Name + ".dll", save_debug_info);
            }

            cur_unit = Path.GetFileNameWithoutExtension(SourceFileName);
            string entry_cur_unit = cur_unit;
            entry_type = mb.DefineType(cur_unit + ".Program", TypeAttributes.Public);//определяем синтетический статический класс основной программы
            cur_type = entry_type;
            //точка входа в приложение
            if (p.main_function != null)
            {
                ConvertFunctionHeader(p.main_function);
                entry_meth = helper.GetMethod(p.main_function).mi as MethodBuilder;
                cur_meth = entry_meth;
                il = cur_meth.GetILGenerator();
                if (options.target != TargetType.Dll && options.dbg_attrs == DebugAttributes.ForDebugging)
                    AddSpecialInitDebugCode();
            }
            ILGenerator tmp_il = il;
            MethodBuilder tmp_meth = cur_meth;

            //при отладке компилятора здесь иногда ничего нет!
            ICommonNamespaceNode[] cnns = p.namespaces;


            //создаем отладочные документы
            if (save_debug_info)
            {
                first_doc = mb.DefineDocument(SourceFileName, SymDocumentType.Text, SymLanguageType.Pascal, SymLanguageVendor.Microsoft);
                sym_docs.Add(SourceFileName, first_doc);
                for (int iii = 0; iii < cnns.Length; iii++)
                {
                    string cnns_document_file_name = null;
                    if (cnns[iii].Location != null)
                    {
                        cnns_document_file_name = cnns[iii].Location.document.file_name;
                        doc = mb.DefineDocument(cnns_document_file_name, SymDocumentType.Text, SymLanguageType.Pascal, SymLanguageVendor.Microsoft);
                    }
                    else
                        doc = first_doc;
                    if (cnns_document_file_name != null && !sym_docs.ContainsKey(cnns_document_file_name))
                        sym_docs.Add(cnns_document_file_name, doc);//сохраняем его в таблице документов
                }
                first_doc = sym_docs[cnns[0].Location == null ? SourceFileName : cnns[0].Location.document.file_name];

                if (p.main_function != null)
                {
                    if (p.main_function.function_code is IStatementsListNode)
                        EntryPointLocation = ((IStatementsListNode)p.main_function.function_code).LeftLogicalBracketLocation;
                    else
                        EntryPointLocation = p.main_function.function_code.Location;
                }
                else
                    EntryPointLocation = null;
            }
            ICommonNamespaceNode entry_ns = null;

            //Переводим заголовки типов
            for (int iii = 0; iii < cnns.Length; iii++)
            {
                if (save_debug_info) doc = sym_docs[cnns[iii].Location == null ? SourceFileName : cnns[iii].Location.document.file_name];
                bool is_main_namespace = cnns[iii].namespace_name == "" && comp_opt.target != TargetType.Dll || comp_opt.target == TargetType.Dll && cnns[iii].namespace_name == "";
                ICommonNamespaceNode cnn = cnns[iii];
                cur_type = entry_type;
                if (!is_main_namespace)
                    cur_unit = cnn.namespace_name;
                else
                    cur_unit = entry_cur_unit;
                if (iii == cnns.Length - 1 && comp_opt.target != TargetType.Dll || comp_opt.target == TargetType.Dll && iii == cnns.Length - 1)
                    entry_ns = cnn;
                ConvertTypeHeaders(cnn.types);
            }

            //Переводим псевдоинстанции generic-типов
            foreach (ICommonTypeNode ictn in p.generic_type_instances)
            {
                ConvertTypeHeaderInSpecialOrder(ictn);
            }

            Dictionary<ICommonNamespaceNode, TypeBuilder> NamespacesTypes = new Dictionary<ICommonNamespaceNode, TypeBuilder>();

            for (int iii = 0; iii < cnns.Length; iii++)
            {
                bool is_main_namespace = cnns[iii].namespace_name == "" && comp_opt.target != TargetType.Dll || comp_opt.target == TargetType.Dll && cnns[iii].namespace_name == "";
                if (!is_main_namespace)
                {
                    //определяем синтетический класс для модуля
                    cur_type = mb.DefineType(cnns[iii].namespace_name + "." + cnns[iii].namespace_name, TypeAttributes.Public);
                    types.Add(cur_type);
                    NamespaceTypesList.Add(cur_type);
                    NamespacesTypes.Add(cnns[iii], cur_type);
                    if (cnns[iii].IsMain)
                    {
                        TypeBuilder attr_class = mb.DefineType(cnns[iii].namespace_name + "." + "$GlobAttr", TypeAttributes.Public | TypeAttributes.BeforeFieldInit, typeof(Attribute));
                        ConstructorInfo attr_ci = attr_class.DefineDefaultConstructor(MethodAttributes.Public);
                        cur_type.SetCustomAttribute(attr_ci, new byte[4] { 0x01, 0x00, 0x00, 0x00 });
                        attr_class.CreateType();
                    }
                    else
                    {
                        TypeBuilder attr_class = mb.DefineType(cnns[iii].namespace_name + "." + "$ClassUnitAttr", TypeAttributes.Public | TypeAttributes.BeforeFieldInit, typeof(Attribute));
                        ConstructorInfo attr_ci = attr_class.DefineDefaultConstructor(MethodAttributes.Public);
                        cur_type.SetCustomAttribute(attr_ci, new byte[4] { 0x01, 0x00, 0x00, 0x00 });
                        attr_class.CreateType();
                    }
                }
                else
                {
                    NamespacesTypes.Add(cnns[iii], entry_type);
                }

            }

            if (comp_opt.target == TargetType.Dll)
            {
                for (int iii = 0; iii < cnns.Length; iii++)
                {
                    string tmp = cur_unit;
                    if (cnns[iii].namespace_name != "")
                        cur_unit = cnns[iii].namespace_name;
                    else
                        cur_unit = entry_cur_unit;
                    foreach (ITemplateClass tc in cnns[iii].templates)
                    {
                        CreateTemplateClass(tc);
                    }
                    cur_unit = tmp;
                }
                for (int iii = 0; iii < cnns.Length; iii++)
                {
                    string tmp = cur_unit;
                    if (cnns[iii].namespace_name != "")
                        cur_unit = cnns[iii].namespace_name;
                    else
                        cur_unit = entry_cur_unit;
                    foreach (ITypeSynonym ts in cnns[iii].type_synonims)
                    {
                        CreateTypeSynonim(ts);
                    }
                    cur_unit = tmp;
                }
            }
            for (int iii = 0; iii < cnns.Length; iii++)
            {
                if (save_debug_info) doc = sym_docs[cnns[iii].Location == null ? SourceFileName : cnns[iii].Location.document.file_name];
                cur_type = NamespacesTypes[cnns[iii]];
                cur_unit_type = NamespacesTypes[cnns[iii]];
                ConvertTypeMemberHeaders(cnns[iii].types);
            }

            for (int iii = 0; iii < cnns.Length; iii++)
            {
                if (save_debug_info) doc = sym_docs[cnns[iii].Location == null ? SourceFileName : cnns[iii].Location.document.file_name];
                cur_type = NamespacesTypes[cnns[iii]];
                cur_unit_type = NamespacesTypes[cnns[iii]];
                ConvertFunctionHeaders(cnns[iii].functions);
            }
            if (p.InitializationCode != null)
            {
                tmp_il = il;
                if (entry_meth != null)
                {
                    il = entry_meth.GetILGenerator();
                    ConvertStatement(p.InitializationCode);
                }
                il = tmp_il;
            }

            //Переводим псевдоинстанции generic-типов
            foreach (IGenericTypeInstance ictn in p.generic_type_instances)
            {
                ConvertGenericInstanceTypeMembers(ictn);
            }

            //Переводим псевдоинстанции функций
            foreach (IGenericFunctionInstance igfi in p.generic_function_instances)
            {
                ConvertGenericFunctionInstance(igfi);
            }

            for (int iii = 0; iii < cnns.Length; iii++)
            {
                if (save_debug_info) doc = sym_docs[cnns[iii].Location == null ? SourceFileName : cnns[iii].Location.document.file_name];
                cur_type = NamespacesTypes[cnns[iii]];
                cur_unit_type = NamespacesTypes[cnns[iii]];
                //генерим инциализацию для полей
                foreach (SemanticTree.ICommonTypeNode ctn in cnns[iii].types)
                    GenerateInitCodeForFields(ctn);
            }

            ConstructorBuilder unit_cci = null;

            //Переводим заголовки всего остального (процедур, переменных)
            for (int iii = 0; iii < cnns.Length; iii++)
            {
                if (save_debug_info) doc = sym_docs[cnns[iii].Location == null ? SourceFileName : cnns[iii].Location.document.file_name];
                bool is_main_namespace = iii == cnns.Length - 1 && comp_opt.target != TargetType.Dll;
                ICommonNamespaceNode cnn = cnns[iii];
                string tmp_unit_name = cur_unit;
                if (!is_main_namespace)
                    cur_unit = cnn.namespace_name;
                else
                    cur_unit = entry_cur_unit;
                cur_type = NamespacesTypes[cnn];

                //ConvertFunctionHeaders(cnn.functions);
                
                if (!is_main_namespace)
                {
                    //определяем статический конструктор класса для модуля
                    ConstructorBuilder cb = cur_type.DefineConstructor(MethodAttributes.Static, CallingConventions.Standard, Type.EmptyTypes);
                    il = cb.GetILGenerator();
                    if (cnn.IsMain) unit_cci = cb;
                    ModulesInitILGenerators.Add(cur_type, il);
                    
                    //перводим константы
                    ConvertNamespaceConstants(cnn.constants);
                    //переводим глобальные переменные модуля
                    ConvertGlobalVariables(cnn.variables);
                    ConvertNamespaceEvents(cnn.events);
                    //il.Emit(OpCodes.Ret);
                }
                else
                {
                    //Не нарвится мне порядок вызова. надо с этим разобраться
                    init_variables_mb = helper.GetMethodBuilder(cnn.functions[cnn.functions.Length-1]);// cur_type.DefineMethod("$InitVariables", MethodAttributes.Public | MethodAttributes.Static);
                    il = entry_meth.GetILGenerator();
                    ModulesInitILGenerators.Add(cur_type, il);
                    il = init_variables_mb.GetILGenerator();
                    //перводим константы
                    ConvertNamespaceConstants(cnn.constants);
                    ConvertGlobalVariables(cnn.variables);
                    ConvertNamespaceEvents(cnn.events);
                    il = entry_meth.GetILGenerator();
                    //il.Emit(OpCodes.Ret);
                }

                cur_unit = tmp_unit_name;
            }

            if (p.InitializationCode != null)
            {
                tmp_il = il;
                if (entry_meth == null)
                {
                    il = unit_cci.GetILGenerator();
                    ConvertStatement(p.InitializationCode);
                }
                il = tmp_il;
            }
            cur_type = entry_type;
            //is_in_unit = false;
            //переводим реализации
            for (int iii = 0; iii < cnns.Length; iii++)
            {
                if (save_debug_info) doc = sym_docs[cnns[iii].Location == null ? SourceFileName : cnns[iii].Location.document.file_name];
                bool is_main_namespace = iii == 0 && comp_opt.target != TargetType.Dll;
                ICommonNamespaceNode cnn = cnns[iii];
                string tmp_unit_name = cur_unit;
                if (!is_main_namespace) cur_unit = cnn.namespace_name;
                //if (iii > 0) is_in_unit = true;
                cur_unit_type = NamespacesTypes[cnns[iii]];
                cur_type = cur_unit_type;
                ConvertTypeImplementations(cnn.types);
                ConvertFunctionsBodies(cnn.functions);
                cur_unit = tmp_unit_name;
            }
            if (comp_opt.target != TargetType.Dll && p.main_function != null)
            {
                cur_unit_type = NamespacesTypes[cnns[0]];
                cur_type = cur_unit_type;
                ConvertBody(p.main_function.function_code);
            }
            for (int iii = 0; iii < cnns.Length; iii++)
            {
                if (save_debug_info) doc = sym_docs[cnns[iii].Location == null ? SourceFileName : cnns[iii].Location.document.file_name];
                cur_type = NamespacesTypes[cnns[iii]];
                cur_unit_type = NamespacesTypes[cnns[iii]];
                //вставляем ret в int_meth
                foreach (SemanticTree.ICommonTypeNode ctn in cnns[iii].types)
                    GenerateRetForInitMeth(ctn);
                ModulesInitILGenerators[cur_type].Emit(OpCodes.Ret);
            }
            for (int iii = 0; iii < cnns.Length; iii++)
            {
                MakeAttribute(cnns[iii]);
            }
            doc = first_doc;
            cur_type = entry_type;

            CloseTypes();//закрываем типы
            entry_type.CreateType();
            switch (comp_opt.target)
            {
                case TargetType.Exe: ab.SetEntryPoint(entry_meth, PEFileKinds.ConsoleApplication); break;
                case TargetType.WinExe:
                    if (!comp_opt.ForRunningWithEnvironment)
                        ab.SetEntryPoint(entry_meth, PEFileKinds.WindowApplication);
                    else
                        ab.SetEntryPoint(entry_meth, PEFileKinds.ConsoleApplication); break;
            }

            /**/
            try
            { //ne osobo vazhnaja vesh, sohranjaet v exe-shnik spisok ispolzuemyh prostranstv imen, dlja strahovki obernuli try catch

                if (comp_opt.dbg_attrs == DebugAttributes.ForDebugging)
                {
                    string[] namespaces = p.UsedNamespaces;

                    TypeBuilder attr_class = mb.DefineType("$UsedNsAttr", TypeAttributes.Public | TypeAttributes.BeforeFieldInit, typeof(Attribute));
                    FieldBuilder fld_ns = attr_class.DefineField("ns", TypeFactory.StringType, FieldAttributes.Public);
                    FieldBuilder fld_count = attr_class.DefineField("count", TypeFactory.Int32Type, FieldAttributes.Public);
                    ConstructorBuilder attr_ci = attr_class.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, new Type[2] { TypeFactory.Int32Type, TypeFactory.StringType });
                    ILGenerator attr_il = attr_ci.GetILGenerator();
                    attr_il.Emit(OpCodes.Ldarg_0);
                    attr_il.Emit(OpCodes.Ldarg_1);
                    attr_il.Emit(OpCodes.Stfld, fld_count);
                    attr_il.Emit(OpCodes.Ldarg_0);
                    attr_il.Emit(OpCodes.Ldarg_2);
                    attr_il.Emit(OpCodes.Stfld, fld_ns);
                    attr_il.Emit(OpCodes.Ret);
                    int len = 2 + 2 + 4 + 1;
                    foreach (string ns in namespaces)
                    {
                        len += ns.Length + 1;
                    }
                    byte[] bytes = new byte[len];
                    bytes[0] = 1;
                    bytes[1] = 0;
                    using (BinaryWriter bw = new BinaryWriter(new MemoryStream()))
                    {
                        bw.Write(namespaces.Length);
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        foreach (string ns in namespaces)
                        {
                            sb.Append(Convert.ToChar(ns.Length));
                            sb.Append(ns);
                            //bw.Write(ns);
                        }
                        if (sb.Length > 127)
                        {
                            len += 1;
                            bytes = new byte[len];
                            bytes[0] = 1;
                            bytes[1] = 0;
                        }
                        bw.Write(sb.ToString());
                        bw.Seek(0, SeekOrigin.Begin);
                        bw.BaseStream.Read(bytes, 2, len - 4);
                        if (sb.Length > 127)
                        {
                            bytes[7] = (byte)(sb.Length & 0xFF);
                            bytes[6] = (byte)(0x80 | ((sb.Length & 0xFF00) >> 8));
                        }
                    }
                    entry_type.SetCustomAttribute(attr_ci, bytes);
                    attr_class.CreateType();
                }
            }
            catch (Exception e)
            {

            }
            if (an.Name == "PABCRtl" || an.Name == "PABCRtl32")
            {
                CustomAttributeBuilder cab = new CustomAttributeBuilder(typeof(AssemblyKeyFileAttribute).GetConstructor(new Type[] { typeof(string) }), new object[] { an.Name == "PABCRtl" ? "PublicKey.snk" : "PublicKey32.snk" });
                ab.SetCustomAttribute(cab);
                cab = new CustomAttributeBuilder(typeof(AssemblyDelaySignAttribute).GetConstructor(new Type[] { typeof(bool) }), new object[] { true });
                ab.SetCustomAttribute(cab);
                cab = new CustomAttributeBuilder(typeof(TargetFrameworkAttribute).GetConstructor(new Type[] { typeof(string) }), new object[] { ".NETFramework,Version=v4.0" });
                ab.SetCustomAttribute(cab);
            }

            ab.SetCustomAttribute(new CustomAttributeBuilder(typeof(SecurityRulesAttribute).GetConstructor(new Type[] { typeof(SecurityRuleSet) }), new object[] { SecurityRuleSet.Level2 },
                new PropertyInfo[] { typeof(SecurityRulesAttribute).GetProperty("SkipVerificationInFullTrust") },
                new object[] { true }));
            if (entry_meth != null && comp_opt.target == TargetType.WinExe)
            {
                entry_meth.SetCustomAttribute(typeof(STAThreadAttribute).GetConstructor(Type.EmptyTypes), new byte[] { 0x01, 0x00, 0x00, 0x00 });
            }
            List<FileStream> ResStreams = new List<FileStream>();
            if (ResourceFiles != null)
                foreach (string resname in ResourceFiles)
                {
                    FileStream stream = File.OpenRead(resname);
                    ResStreams.Add(stream);
                    mb.DefineManifestResource(Path.GetFileName(resname), stream, ResourceAttributes.Public);
                }
            ab.SetCustomAttribute(typeof(System.Runtime.CompilerServices.CompilationRelaxationsAttribute).GetConstructor(new Type[1] { TypeFactory.Int32Type }),
                                  new byte[] { 0x01, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00 });

            if (RunOnly)
            {
                object main_class = ab.CreateInstance(cur_unit + ".Program");
                MethodInfo methodInfo = main_class.GetType().GetMethod("Main");
                methodInfo.Invoke(main_class, null);
            }
            else
            {
                int tries = 0;
                bool not_done = true;
                do
                {
                    try
                    {
                        if (comp_opt.target == TargetType.Exe || comp_opt.target == TargetType.WinExe)
                        {
                            if (comp_opt.platformtarget == NETGenerator.CompilerOptions.PlatformTarget.x86)
                                ab.Save(an.Name + ".exe", PortableExecutableKinds.Required32Bit, ImageFileMachine.I386);
                            //else if (comp_opt.platformtarget == NETGenerator.CompilerOptions.PlatformTarget.x64)
                            //    ab.Save(an.Name + ".exe", PortableExecutableKinds.PE32Plus, ImageFileMachine.IA64);
                            else ab.Save(an.Name + ".exe");
                            //сохраняем сборку
                        }
                        else
                        {
                            if (comp_opt.platformtarget == NETGenerator.CompilerOptions.PlatformTarget.x86)
                                ab.Save(an.Name + ".dll", PortableExecutableKinds.Required32Bit, ImageFileMachine.I386);
                            //else if (comp_opt.platformtarget == NETGenerator.CompilerOptions.PlatformTarget.x64)
                            //    ab.Save(an.Name + ".dll", PortableExecutableKinds.PE32Plus, ImageFileMachine.IA64);
                            else ab.Save(an.Name + ".dll");
                        }
                        not_done = false;
                    }
                    catch (System.Runtime.InteropServices.COMException e)
                    {
                        throw new TreeConverter.SaveAssemblyError(e.Message);
                    }
                    catch (System.IO.IOException e)
                    {
                        if (tries < num_try_save)
                            tries++;
                        else
                            throw new TreeConverter.SaveAssemblyError(e.Message);
                    }
                }
                while (not_done);
            }
            foreach (FileStream fs in ResStreams)
                fs.Close();

        }

        private void AddSpecialInitDebugCode()
        {
            //il.Emit(OpCodes.Call,typeof(Console).GetMethod("ReadLine"));
            //il.Emit(OpCodes.Pop);
        }

        private void ConvertNamespaceConstants(INamespaceConstantDefinitionNode[] Constants)
        {
            foreach (INamespaceConstantDefinitionNode Constant in Constants)
                ConvertConstantDefinitionNode(Constant, Constant.name, Constant.type, Constant.constant_value);
        }

        private void ConvertNamespaceEvents(ICommonNamespaceEventNode[] Events)
        {
            foreach (ICommonNamespaceEventNode Event in Events)
                Event.visit(this);
        }

        private void ConvertCommonFunctionConstantDefinitions(ICommonFunctionConstantDefinitionNode[] Constants)
        {
            foreach (ICommonFunctionConstantDefinitionNode Constant in Constants)
                //ConvertFunctionConstantDefinitionNode(Constant);
                ConvertConstantDefinitionNode(Constant, Constant.name, Constant.type, Constant.constant_value);
        }

        private void ConvertConstantDefinitionNode(IConstantDefinitionNode cnst, string name, ITypeNode type, IConstantNode constant_value)
        {
            if (constant_value is IArrayConstantNode)
                ConvertArrayConstantDef(cnst, name, type, constant_value);
            else
                if (constant_value is IRecordConstantNode || constant_value is ICompiledStaticMethodCallNodeAsConstant)
                    ConvertConstantDefWithInitCall(cnst, name, type, constant_value);
                else if (constant_value is ICommonNamespaceFunctionCallNodeAsConstant || constant_value is IBasicFunctionCallNodeAsConstant || constant_value is ICommonConstructorCallAsConstant || constant_value is ICompiledStaticFieldReferenceNodeAsConstant)
                    ConvertSetConstantDef(cnst, name, type, constant_value);
                else ConvertSimpleConstant(cnst, name, type, constant_value);
        }

        private void ConvertSetConstantDef(IConstantDefinitionNode cnst, string name, ITypeNode type, IConstantNode constant_value)
        {
            TypeInfo ti = helper.GetTypeReference(type);
            FieldAttributes attrs = FieldAttributes.Public | FieldAttributes.Static;
            if (comp_opt.target == TargetType.Dll)
                attrs |= FieldAttributes.InitOnly;
            FieldBuilder fb = cur_type.DefineField(name, ti.tp, attrs);
            //il.Emit(OpCodes.Newobj, ti.tp.GetConstructor(Type.EmptyTypes));
            //il.Emit(OpCodes.Stsfld, fb);
            if (cnst != null)
                helper.AddConstant(cnst, fb);
            bool tmp = save_debug_info;
            save_debug_info = false;
            constant_value.visit(this);
            save_debug_info = tmp;
            il.Emit(OpCodes.Stsfld, fb);

            if (!ConvertedConstants.ContainsKey(constant_value))
                ConvertedConstants.Add(constant_value, fb);
        }

        private void ConvertSimpleConstant(IConstantDefinitionNode cnst, string name, ITypeNode type, IConstantNode constant_value)
        {
            FieldBuilder fb = cur_type.DefineField(name, helper.GetTypeReference(type).tp, FieldAttributes.Static | FieldAttributes.Public | FieldAttributes.Literal);
            Type t = helper.GetTypeReference(type).tp;
            if (t.IsEnum)
            {
                if (!(t is EnumBuilder))
                    fb.SetConstant(Enum.ToObject(t, (constant_value as IEnumConstNode).constant_value));
                else
                    fb.SetConstant(constant_value.value);
            }
            else if (!(constant_value is INullConstantNode))
            {
                if (constant_value.value.GetType() != t)
                {

                }
                else
                fb.SetConstant(constant_value.value);
            }
        }

        private void PushConstantValue(IConstantNode cnst)
        {
            if (cnst is IIntConstantNode)
                PushIntConst((cnst as IIntConstantNode).constant_value);
            else if (cnst is IDoubleConstantNode)
                PushDoubleConst((cnst as IDoubleConstantNode).constant_value);
            else if (cnst is IFloatConstantNode)
                PushFloatConst((cnst as IFloatConstantNode).constant_value);
            else if (cnst is ICharConstantNode)
                PushCharConst((cnst as ICharConstantNode).constant_value);
            else if (cnst is IStringConstantNode)
                PushStringConst((cnst as IStringConstantNode).constant_value);
            else if (cnst is IByteConstantNode)
                PushByteConst((cnst as IByteConstantNode).constant_value);
            else if (cnst is ILongConstantNode)
                PushLongConst((cnst as ILongConstantNode).constant_value);
            else if (cnst is IBoolConstantNode)
                PushBoolConst((cnst as IBoolConstantNode).constant_value);
            else if (cnst is ISByteConstantNode)
                PushSByteConst((cnst as ISByteConstantNode).constant_value);
            else if (cnst is IUShortConstantNode)
                PushUShortConst((cnst as IUShortConstantNode).constant_value);
            else if (cnst is IUIntConstantNode)
                PushUIntConst((cnst as IUIntConstantNode).constant_value);
            else if (cnst is IULongConstantNode)
                PushULongConst((cnst as IULongConstantNode).constant_value);
            else if (cnst is IShortConstantNode)
                PushShortConst((cnst as IShortConstantNode).constant_value);
            else if (cnst is IEnumConstNode)
                PushIntConst((cnst as IEnumConstNode).constant_value);
            else if (cnst is INullConstantNode)
                il.Emit(OpCodes.Ldnull);
        }

        private void ConvertConstantDefWithInitCall(IConstantDefinitionNode cnst, string name, ITypeNode type, IConstantNode constant_value)
        {
            TypeInfo ti = helper.GetTypeReference(type);
            FieldAttributes attrs = FieldAttributes.Public | FieldAttributes.Static;
            if (comp_opt.target == TargetType.Dll)
                attrs |= FieldAttributes.InitOnly;
            FieldBuilder fb = cur_type.DefineField(name, ti.tp, attrs);
            if (cnst != null)
                helper.AddConstant(cnst, fb);
            bool tmp = save_debug_info;
            save_debug_info = false;
            AddInitCall(il, fb, ti.init_meth, constant_value);
            save_debug_info = tmp;
            if (!ConvertedConstants.ContainsKey(constant_value))
                ConvertedConstants.Add(constant_value, fb);
        }

        private void ConvertArrayConstantDef(IConstantDefinitionNode cnst, string name, ITypeNode type, IConstantNode constant_value)
        {
            //ConvertedConstants.ContainsKey(ArrayConstant)
            TypeInfo ti = helper.GetTypeReference(type);
            FieldAttributes attrs = FieldAttributes.Public | FieldAttributes.Static;
            if (comp_opt.target == TargetType.Dll)
                attrs |= FieldAttributes.InitOnly;
            FieldBuilder fb = cur_type.DefineField(name, ti.tp, attrs);
            if (cnst != null)
                helper.AddConstant(cnst, fb);
            CreateArrayGlobalVariable(il, fb, ti, constant_value as IArrayConstantNode, type);

            if (!ConvertedConstants.ContainsKey(constant_value))
                ConvertedConstants.Add(constant_value, fb);
        }

        //это требование Reflection.Emit - все типы должны быть закрыты
        private void CloseTypes()
        {
            //(ssyy) TODO: подумать, в каком порядке создавать типы
            for (int i = 0; i < types.Count; i++)
                if (types[i].IsInterface)
                    types[i].CreateType();
            for (int i = 0; i < enums.Count; i++)
                enums[i].CreateType();
            for (int i = 0; i < value_types.Count; i++)
                value_types[i].CreateType();
            for (int i = 0; i < types.Count; i++)
                if (!types[i].IsInterface)
                    types[i].CreateType();
        }

        //перевод тела
        private void ConvertBody(IStatementNode body)
        {
            if (!(body is IStatementsListNode) && save_debug_info && body.Location != null)
                if (body.Location.begin_line_num == 0xFFFFFF) MarkSequencePoint(il, body.Location);
            body.visit(this);
            OptMakeExitLabel();
        }

        private void OptMakeExitLabel()
        {
            if (ExitProcedureCall)
            {
                il.MarkLabel(ExitLabel);
                ExitProcedureCall = false;
            }
        }

        //перевод заголовков типов
        private void ConvertTypeHeaders(ICommonTypeNode[] types)
        {
            foreach (ICommonTypeNode t in types)
            {
                ConvertTypeHeaderInSpecialOrder(t);
            }
        }

        private void CreateTemplateClass(ITemplateClass t)
        {
            if (t.serialized_tree != null)
            {
                TypeBuilder tb = mb.DefineType(cur_unit + ".%" + t.name, TypeAttributes.Public, TypeFactory.ObjectType);
                types.Add(tb);
                CustomAttributeBuilder cust_bldr = new CustomAttributeBuilder(this.TemplateClassAttributeConstructor, new object[1] { t.serialized_tree });
                tb.SetCustomAttribute(cust_bldr);
            }
        }

        private void CreateTypeSynonim(ITypeSynonym t)
        {
            TypeBuilder tb = mb.DefineType(cur_unit + ".%" + t.name, TypeAttributes.Public, TypeFactory.ObjectType);
            types.Add(tb);
            add_possible_type_attribute(tb, t);
        }

        private Type CreateTypedFileType(ICommonTypeNode t)
        {
            Type tt = helper.GetPascalTypeReference(t);
            if (tt != null) return tt;
            TypeBuilder tb = mb.DefineType(cur_unit + ".%" + t.name, TypeAttributes.Public, TypeFactory.ObjectType);
            types.Add(tb);
            helper.AddPascalTypeReference(t, tb);
            add_possible_type_attribute(tb, t);
            return tb;
        }

        private Type CreateTypedSetType(ICommonTypeNode t)
        {
            Type tt = helper.GetPascalTypeReference(t);
            if (tt != null) return tt;
            TypeBuilder tb = mb.DefineType(cur_unit + ".%" + t.name, TypeAttributes.Public, TypeFactory.ObjectType);
            types.Add(tb);
            helper.AddPascalTypeReference(t, tb);
            add_possible_type_attribute(tb, t);
            return tb;
        }

        private Type CreateShortStringType(ITypeNode t)
        {
            TypeBuilder tb = mb.DefineType(cur_unit + ".$string" + (uid++).ToString(), TypeAttributes.Public, TypeFactory.ObjectType);
            types.Add(tb);
            add_possible_type_attribute(tb, t);
            return tb;
        }

        //переводим заголовки типов в порядке начиная с базовых классов (т. е. у которых наследники - откомпилированные типы)
        private void ConvertTypeHeaderInSpecialOrder(ICommonTypeNode t)
        {
            if (t.type_special_kind == type_special_kind.diap_type) return;
            if (t.type_special_kind == type_special_kind.array_kind) return;
            if (t.depended_from_indefinite) return;
            if (t.type_special_kind == type_special_kind.typed_file && comp_opt.target == TargetType.Dll)
            {
                if (!t.name.Contains(" "))
                {
                    CreateTypedFileType(t);
                    return;
                }
            }
            else
                if (t.type_special_kind == type_special_kind.set_type && comp_opt.target == TargetType.Dll)
                {
                    if (!t.name.Contains(" "))
                    {
                        CreateTypedSetType(t);
                        return;
                    }
                }

            if (helper.GetTypeReference(t) != null && !t.is_generic_parameter)
                return;
            helper.SetAsProcessing(t);
            if (t.is_generic_parameter)
            {
                //ConvertTypeHeaderInSpecialOrder(t.generic_container);
                AddTypeWithoutConvert(t);
                if (converting_generic_param != t)
                {
                    return;
                }
                converting_generic_param = null;
            }
            IGenericTypeInstance gti = t as IGenericTypeInstance;
            if (gti != null)
            {
                if (gti.original_generic is ICommonTypeNode)
                {
                    
                    ConvertTypeHeaderInSpecialOrder((ICommonTypeNode)gti.original_generic);
                }
                
                foreach (ITypeNode itn in gti.generic_parameters)
                {
                    if (itn is ICommonTypeNode && !itn.is_generic_parameter && !helper.IsProcessing(itn as ICommonTypeNode))
                    {
                        
                        ConvertTypeHeaderInSpecialOrder((ICommonTypeNode)itn);
                    }
                }
            }
            if (t.is_generic_type_definition)
            {
                AddTypeWithoutConvert(t);
                foreach (ICommonTypeNode par in t.generic_params)
                {
                    converting_generic_param = par;
                    ConvertTypeHeaderInSpecialOrder(par);
                }
            }
            else if ((t.type_special_kind == type_special_kind.none_kind ||
                t.type_special_kind == type_special_kind.record) && !t.IsEnum &&
                !t.is_generic_type_instance && !t.is_generic_parameter)
            {
                AddTypeWithoutConvert(t);
            }
            foreach (ITypeNode interf in t.ImplementingInterfaces)
                if (!(interf is ICompiledTypeNode))
                    ConvertTypeHeaderInSpecialOrder((ICommonTypeNode)interf);
            if (t.base_type != null && !(t.base_type is ICompiledTypeNode))
            {
                ConvertTypeHeaderInSpecialOrder((ICommonTypeNode)t.base_type);
            }
            ConvertTypeHeader(t);
        }

        private void AddTypeWithoutConvert(ICommonTypeNode t)
        {
            if (helper.GetTypeReference(t) != null) return;
            TypeBuilder tb = mb.DefineType(BuildTypeName(t.name), ConvertAttributes(t), null, new Type[0]);
            helper.AddType(t, tb);
            //(ssyy) обрабатываем generics
            if (t.is_generic_type_definition)
            {
                int count = t.generic_params.Count;
                string[] par_names = new string[count];
                //Создаём массив имён параметров
                for (int i = 0; i < count; i++)
                {
                    par_names[i] = t.generic_params[i].name;
                }
                //Определяем параметры в строящемся типе
                GenericTypeParameterBuilder[] net_pars = tb.DefineGenericParameters(par_names);
                for (int i = 0; i < count; i++)
                {
                    //добавляем параметр во внутр. структуры
                    helper.AddExistingType(t.generic_params[i], net_pars[i]);
                }
            }
        }

        //перевод релизаций типов
        private void ConvertTypeImplementations(ICommonTypeNode[] types)
        {
            foreach (ICommonTypeNode t in types)
            //если это не особый тип переводим реализацию наверно здесь много лишнего нужно оставить ISimpleArrayNode
            {
                if ( t.type_special_kind != type_special_kind.diap_type &&
                    !t.depended_from_indefinite)
                    t.visit(this);
            }
        }

        private void ConvertTypeMemberHeaderAndRemoveFromList(ICommonTypeNode type, List<ICommonTypeNode> types)
        {
            if (!type.depended_from_indefinite)
            {
                if (type.type_special_kind == type_special_kind.array_wrapper &&
                    type.element_type.type_special_kind == type_special_kind.array_wrapper &&
                    type.element_type is ICommonTypeNode &&
                    types.IndexOf((ICommonTypeNode)(type.element_type)) > -1)
                {
                    ConvertTypeMemberHeaderAndRemoveFromList((ICommonTypeNode)(type.element_type), types);
                }
                ConvertTypeMemberHeader(type);
            }
            types.Remove(type);
        }

        //перевод заголовков членов класса
        private void ConvertTypeMemberHeaders(ICommonTypeNode[] types)
        {
            //(ssyy) Переупорядочиваем, чтобы массивы создавались в правильном порядке
            List<ICommonTypeNode> ts = new List<ICommonTypeNode>(types);
            while (ts.Count > 0)
            {
                ConvertTypeMemberHeaderAndRemoveFromList(ts[0], ts);
            }
            foreach (ICommonTypeNode t in types)
            {
                foreach (ICommonMethodNode meth in t.methods)
                {
                    if (meth.is_generic_function)
                    {
                        ConvertTypeInstancesMembersInFunction(meth);
                    }
                }
            }
        }

        private Dictionary<TypeBuilder, TypeBuilder> added_types = new Dictionary<TypeBuilder, TypeBuilder>();
        private void BuildCloseTypeOrder(ICommonTypeNode value, TypeBuilder tb)
        {
            foreach (ICommonClassFieldNode fld in value.fields)
            {
                ITypeNode ctn = fld.type;
                TypeInfo ti = helper.GetTypeReference(ctn);
                if (ctn is ICommonTypeNode && ti.tp.IsValueType && ti.tp is TypeBuilder && tb != ti.tp)
                {
                    BuildCloseTypeOrder((ICommonTypeNode)ctn, (TypeBuilder)ti.tp);
                }
            }
            if (!added_types.ContainsKey(tb))
            {
                value_types.Add(tb);
                added_types[tb] = tb;
            }
        }

        private Type GetTypeOfGenericInstanceField(Type t, FieldInfo finfo)
        {
            if (finfo.FieldType.IsGenericParameter)
            {
                return t.GetGenericArguments()[finfo.FieldType.GenericParameterPosition];
            }
            else
            {
                return finfo.FieldType;
            }
        }

        private void ConvertGenericInstanceTypeMembers(IGenericTypeInstance value)
        {
            if (helper.GetTypeReference(value) == null)
            {
                return;
            }
            ICompiledGenericTypeInstance compiled_inst = value as ICompiledGenericTypeInstance;
            if (compiled_inst != null)
            {
                ConvertCompiledGenericInstanceTypeMembers(compiled_inst);
                return;
            }
            ICommonGenericTypeInstance common_inst = value as ICommonGenericTypeInstance;
            if (common_inst != null)
            {
                ConvertCommonGenericInstanceTypeMembers(common_inst);
                return;
            }
        }

        //ssyy 04.02.2010. Вернул следующие 2 функции в исходное состояние.
        private void ConvertCompiledGenericInstanceTypeMembers(ICompiledGenericTypeInstance value)
        {
            Type t = helper.GetTypeReference(value).tp;
            bool is_delegated_type = t.BaseType == TypeFactory.MulticastDelegateType;
            foreach (IDefinitionNode dn in value.used_members.Keys)
            {
                ICompiledConstructorNode iccn = dn as ICompiledConstructorNode;
                if (iccn != null)
                {
                    ConstructorInfo ci = TypeBuilder.GetConstructor(t, iccn.constructor_info);
                    helper.AddConstructor(value.used_members[dn] as IFunctionNode, ci);
                    continue;
                }
                ICompiledMethodNode icmn = dn as ICompiledMethodNode;
                if (icmn != null)
                {
                    if (is_delegated_type && icmn.method_info.IsSpecialName) continue;
                    MethodInfo mi = null;
                    try
                    {
                        mi = TypeBuilder.GetMethod(t, icmn.method_info);
                    }
                    catch
                    {
                        if (icmn.method_info.DeclaringType.IsGenericType && !icmn.method_info.DeclaringType.IsGenericTypeDefinition)
                        {
                            Type gen_def_type = icmn.method_info.DeclaringType.GetGenericTypeDefinition();

                            foreach (MethodInfo mi2 in gen_def_type.GetMethods())
                            {
                                if (mi2.MetadataToken == icmn.method_info.MetadataToken)
                                {
                                    mi = mi2;
                                    break;
                                }
                            }

                            mi = TypeBuilder.GetMethod(t, mi);
                        }
                        else
                            mi = icmn.method_info;
                    }
                    helper.AddMethod(value.used_members[dn] as IFunctionNode, mi);
                    continue;
                }
                ICompiledClassFieldNode icfn = dn as ICompiledClassFieldNode;
                if (icfn != null)
                {
                    Type ftype = GetTypeOfGenericInstanceField(t, icfn.compiled_field);
                    FieldInfo fi = TypeBuilder.GetField(t, icfn.compiled_field);

                    helper.AddGenericField(value.used_members[dn] as ICommonClassFieldNode, fi, ftype);
                    continue;
                }
            }
        }

        private void ConvertCommonGenericInstanceTypeMembers(ICommonGenericTypeInstance value)
        {
            Type t = helper.GetTypeReference(value).tp;
            var genericInstances = new List<ICommonMethodNode>();
            Func<ICommonMethodNode, bool> processInstances = (icmn) =>
            {
                if (icmn.is_constructor)
                {
                    MethInfo mi = helper.GetConstructor(icmn);
                    if (mi != null)
                    {
                        ConstructorInfo cnstr = mi.cnstr;
                        ConstructorInfo ci = TypeBuilder.GetConstructor(t, cnstr);
                        helper.AddConstructor(value.used_members[icmn] as IFunctionNode, ci);
                    }
                    return true;
                }
                else
                {
                    var methtmp = helper.GetMethod(icmn);
                    if (methtmp == null)
                        return true;
                    MethodInfo meth = methtmp.mi;
                    if (meth.GetType().FullName == "System.Reflection.Emit.MethodOnTypeBuilderInstantiation")
                        meth = meth.GetGenericMethodDefinition();
                    MethodInfo mi = TypeBuilder.GetMethod(t, meth);
                    helper.AddMethod(value.used_members[icmn] as IFunctionNode, mi);
                    return true;
                }
            };
            foreach (IDefinitionNode dn in value.used_members.Keys)
            {
                ICommonMethodNode icmn = dn as ICommonMethodNode;
                if (icmn != null)
                {
                    if (icmn.comperehensive_type.is_generic_type_instance)
                    {
                        genericInstances.Add(icmn);
                        continue;
                    }

                    if (processInstances(icmn))
                        continue;
                }
                ICommonClassFieldNode icfn = dn as ICommonClassFieldNode;
                if (icfn != null)
                {
                    FldInfo fldinfo = helper.GetField(icfn);
                    if (!(fldinfo is GenericFldInfo))
                    {
                        FieldInfo finfo = fldinfo.fi;
                        Type ftype = GetTypeOfGenericInstanceField(t, finfo);
                        FieldInfo fi = TypeBuilder.GetField(t, finfo);
                        helper.AddGenericField(value.used_members[dn] as ICommonClassFieldNode, fi, ftype);
                    }
                    else
                    {
                        FieldInfo finfo = fldinfo.fi;
                        FieldInfo fi = finfo;
                        helper.AddGenericField(value.used_members[dn] as ICommonClassFieldNode, fi, (fldinfo as GenericFldInfo).field_type);
                    }
                    continue;
                }
            }

            foreach (ICommonMethodNode icmn in genericInstances)
            {
                processInstances(icmn);
            }
        }

        private object[] get_constants(IConstantNode[] cnsts)
        {
            object[] objs = new object[cnsts.Length];
            for (int i = 0; i < objs.Length; i++)
            {
                objs[i] = cnsts[i].value;
            }
            return objs;
        }

        private PropertyInfo[] get_named_properties(IPropertyNode[] props)
        {
            PropertyInfo[] arr = new PropertyInfo[props.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                if (props[i] is ICompiledPropertyNode)
                    arr[i] = (props[i] as ICompiledPropertyNode).property_info;
                else
                    arr[i] = helper.GetProperty(props[i]).prop;
            }
            return arr;
        }

        private FieldInfo[] get_named_fields(IVAriableDefinitionNode[] fields)
        {
            FieldInfo[] arr = new FieldInfo[fields.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                if (fields[i] is ICompiledClassFieldNode)
                    arr[i] = (fields[i] as ICompiledClassFieldNode).compiled_field;
                else
                    arr[i] = helper.GetField(fields[i] as ICommonClassFieldNode).fi;
            }
            return arr;
        }

        private void MakeAttribute(ICommonNamespaceNode cnn)
        {
            IAttributeNode[] attrs = cnn.Attributes;
            for (int i = 0; i < attrs.Length; i++)
            {
                CustomAttributeBuilder cab = new CustomAttributeBuilder
                    ((attrs[i].AttributeConstructor is ICompiledConstructorNode) ? (attrs[i].AttributeConstructor as ICompiledConstructorNode).constructor_info : helper.GetConstructor(attrs[i].AttributeConstructor).cnstr, get_constants(attrs[i].Arguments),
                    get_named_properties(attrs[i].PropertyNames), get_constants(attrs[i].PropertyInitializers),
                    get_named_fields(attrs[i].FieldNames), get_constants(attrs[i].FieldInitializers));
                ab.SetCustomAttribute(cab);
            }
        }

        private void MakeAttribute(ICommonTypeNode ctn)
        {
            Type t = helper.GetTypeReference(ctn).tp;
            IAttributeNode[] attrs = ctn.Attributes;
            for (int i = 0; i < attrs.Length; i++)
            {
                //if (attrs[i].AttributeType == SystemLibrary.SystemLibrary.comimport_type)
                //	continue;

                CustomAttributeBuilder cab = new CustomAttributeBuilder
                    ((attrs[i].AttributeConstructor is ICompiledConstructorNode) ? (attrs[i].AttributeConstructor as ICompiledConstructorNode).constructor_info : helper.GetConstructor(attrs[i].AttributeConstructor).cnstr, get_constants(attrs[i].Arguments),
                    get_named_properties(attrs[i].PropertyNames), get_constants(attrs[i].PropertyInitializers),
                    get_named_fields(attrs[i].FieldNames), get_constants(attrs[i].FieldInitializers));
                if (t is TypeBuilder)
                    (t as TypeBuilder).SetCustomAttribute(cab);
                else if (t is EnumBuilder)
                    (t as EnumBuilder).SetCustomAttribute(cab);
            }
        }

        private void MakeAttribute(ICommonPropertyNode prop)
        {
            PropertyBuilder pb = (PropertyBuilder)helper.GetProperty(prop).prop;
            IAttributeNode[] attrs = prop.Attributes;
            for (int i = 0; i < attrs.Length; i++)
            {
                CustomAttributeBuilder cab = new CustomAttributeBuilder
                    ((attrs[i].AttributeConstructor is ICompiledConstructorNode) ? (attrs[i].AttributeConstructor as ICompiledConstructorNode).constructor_info : helper.GetConstructor(attrs[i].AttributeConstructor).cnstr, get_constants(attrs[i].Arguments),
                    get_named_properties(attrs[i].PropertyNames), get_constants(attrs[i].PropertyInitializers),
                    get_named_fields(attrs[i].FieldNames), get_constants(attrs[i].FieldInitializers));
                pb.SetCustomAttribute(cab);
            }
        }

        private void MakeAttribute(ICommonClassFieldNode fld)
        {
            FieldBuilder fb = (FieldBuilder)helper.GetField(fld).fi;
            IAttributeNode[] attrs = fld.Attributes;
            for (int i = 0; i < attrs.Length; i++)
            {
                CustomAttributeBuilder cab = new CustomAttributeBuilder
                    ((attrs[i].AttributeConstructor is ICompiledConstructorNode) ? (attrs[i].AttributeConstructor as ICompiledConstructorNode).constructor_info : helper.GetConstructor(attrs[i].AttributeConstructor).cnstr, get_constants(attrs[i].Arguments),
                    get_named_properties(attrs[i].PropertyNames), get_constants(attrs[i].PropertyInitializers),
                    get_named_fields(attrs[i].FieldNames), get_constants(attrs[i].FieldInitializers));
                fb.SetCustomAttribute(cab);
            }
        }

        private void MakeAttribute(ICommonFunctionNode func)
        {
            MethodBuilder mb = helper.GetMethod(func).mi as MethodBuilder;
            IAttributeNode[] attrs = func.Attributes;
            for (int i = 0; i < attrs.Length; i++)
            {

                CustomAttributeBuilder cab = new CustomAttributeBuilder
                    ((attrs[i].AttributeConstructor is ICompiledConstructorNode) ? (attrs[i].AttributeConstructor as ICompiledConstructorNode).constructor_info : helper.GetConstructor(attrs[i].AttributeConstructor).cnstr, get_constants(attrs[i].Arguments),
                    get_named_properties(attrs[i].PropertyNames), get_constants(attrs[i].PropertyInitializers),
                    get_named_fields(attrs[i].FieldNames), get_constants(attrs[i].FieldInitializers));
                mb.SetCustomAttribute(cab);
            }
            foreach (IParameterNode pn in func.parameters)
            {
                ParamInfo pi = helper.GetParameter(pn);
                if (pi == null) continue;
                ParameterBuilder pb = pi.pb;
                attrs = pn.Attributes;
                for (int i = 0; i < attrs.Length; i++)
                {
                    CustomAttributeBuilder cab = new CustomAttributeBuilder
                        ((attrs[i].AttributeConstructor is ICompiledConstructorNode) ? (attrs[i].AttributeConstructor as ICompiledConstructorNode).constructor_info : helper.GetConstructor(attrs[i].AttributeConstructor).cnstr, get_constants(attrs[i].Arguments),
                        get_named_properties(attrs[i].PropertyNames), get_constants(attrs[i].PropertyInitializers),
                        get_named_fields(attrs[i].FieldNames), get_constants(attrs[i].FieldInitializers));
                    pb.SetCustomAttribute(cab);
                }
            }
        }

        //определяем заголовки членов класса
        private void ConvertTypeMemberHeader(ICommonTypeNode value)
        {
            //если это оболочка над массивом переводим ее особым образом
            if (value.type_special_kind == type_special_kind.diap_type || value.type_special_kind == type_special_kind.array_kind) return;
            if (value.fields.Length == 1 && value.fields[0].type is ISimpleArrayNode)
            {
                ConvertArrayWrapperType(value);
                return;
            }
            if (value is ISimpleArrayNode) return;
            //этот тип уже был переведен, поэтому находим его
            TypeInfo ti = helper.GetTypeReference(value);

            //ivan
            if (ti.tp.IsEnum || !(ti.tp is TypeBuilder)) return;
            TypeBuilder tb = (TypeBuilder)ti.tp;
            if (tb.IsValueType) BuildCloseTypeOrder(value, tb);
            //сохраняем контекст
            TypeInfo tmp_ti = cur_ti;
            cur_ti = ti;
            TypeBuilder tmp = cur_type;
            cur_type = tb;

            //(ssyy) Если это интерфейс, то пропускаем следующую хрень
            if (!value.IsInterface)
            {
                //определяем метод $Init$ для выделения памяти, если метод еще не определен (в структурах он опред-ся раньше)
                MethodBuilder clone_mb = null;
                MethodBuilder ass_mb = null;
                if (ti.init_meth != null && tb.IsValueType)
                {
                    clone_mb = ti.clone_meth as MethodBuilder;
                    ass_mb = ti.assign_meth as MethodBuilder;
                }
                foreach (ICommonClassFieldNode fld in value.fields)
                    fld.visit(this);

                foreach (ICommonMethodNode meth in value.methods)
                    ConvertMethodHeader(meth);
                foreach (ICommonPropertyNode prop in value.properties)
                    prop.visit(this);

                foreach (IClassConstantDefinitionNode constant in value.constants)
                    constant.visit(this);

                foreach (ICommonEventNode evnt in value.events)
                    evnt.visit(this);

                if (clone_mb != null)
                {
                    clone_mb.GetILGenerator().Emit(OpCodes.Ldloc_0);
                    clone_mb.GetILGenerator().Emit(OpCodes.Ret);
                }
                if (ass_mb != null)
                {
                    ass_mb.GetILGenerator().Emit(OpCodes.Ret);
                }
                if (ti.fix_meth != null)
                {
                    ti.fix_meth.GetILGenerator().Emit(OpCodes.Ret);
                }
            }
            else
            {
                //(ssyy) сейчас переводим интерфейс

                foreach (ICommonMethodNode meth in value.methods)
                    ConvertMethodHeader(meth);
                foreach (ICommonPropertyNode prop in value.properties)
                    prop.visit(this);
                foreach (ICommonEventNode evnt in value.events)
                    evnt.visit(this);
            }

            if (value.default_property != null)
            {
                using (BinaryWriter bw = new BinaryWriter(new MemoryStream()))
                {
                    bw.Write(value.default_property.name);
                    bw.Seek(0, SeekOrigin.Begin);
                    byte[] bytes = new byte[2 + value.default_property.name.Length + 1 + 2];
                    bytes[0] = 1;
                    bytes[1] = 0;
                    bw.BaseStream.Read(bytes, 2, value.default_property.name.Length + 1);
                    tb.SetCustomAttribute(TypeFactory.DefaultMemberAttributeType.GetConstructor(new Type[1] { TypeFactory.StringType }), bytes);
                }
            }
            //восстанавливаем контекст
            cur_type = tmp;
            cur_ti = tmp_ti;
        }

        private bool NeedAddCloneMethods(ICommonTypeNode ctn)
        {
            foreach (ICommonClassFieldNode cfn in ctn.fields)
            {
                if (cfn.polymorphic_state != polymorphic_state.ps_static &&
                    (cfn.type.type_special_kind == type_special_kind.array_wrapper ||
                    cfn.type.type_special_kind == type_special_kind.base_set_type ||
                    cfn.type.type_special_kind == type_special_kind.short_string ||
                    cfn.type.type_special_kind == type_special_kind.text_file ||
                    cfn.type.type_special_kind == type_special_kind.typed_file ||
                    cfn.type.type_special_kind == type_special_kind.binary_file ||
                    cfn.type.type_special_kind == type_special_kind.set_type))
                    return true;
                if (cfn.polymorphic_state != polymorphic_state.ps_static && cfn.type.type_special_kind == type_special_kind.record && cfn.type is ICommonTypeNode)
                    if (NeedAddCloneMethods(cfn.type as ICommonTypeNode))
                        return true;
            }
            return false;
        }

        private void AddInitMembers(TypeInfo ti, TypeBuilder tb, ICommonTypeNode ctn)
        {
            MethodBuilder init_mb = tb.DefineMethod("$Init$", MethodAttributes.Public, TypeFactory.VoidType, Type.EmptyTypes);
            ti.init_meth = init_mb;
            //определяем метод $Init$ для выделения памяти, если метод еще не определен (в структурах он опред-ся раньше)
            //MethodBuilder init_mb = ti.init_meth;
            //if (init_mb == null) init_mb = tb.DefineMethod("$Init$", MethodAttributes.Public, typeof(void), Type.EmptyTypes);
            ti.init_meth = init_mb;
            //определяем метод Clone и Assign
            if (tb.IsValueType)
            {
                MethodBuilder clone_mb = null;
                MethodBuilder ass_mb = null;
                if (NeedAddCloneMethods(ctn))
                {
                    clone_mb = tb.DefineMethod("$Clone$", MethodAttributes.Public, tb, Type.EmptyTypes);
                    LocalBuilder lb = clone_mb.GetILGenerator().DeclareLocal(tb);
                    MarkSequencePoint(clone_mb.GetILGenerator(), 0xFFFFFF, 0, 0xFFFFFF, 0);
                    clone_mb.GetILGenerator().Emit(OpCodes.Ldloca, lb);
                    clone_mb.GetILGenerator().Emit(OpCodes.Call, init_mb);
                    ti.clone_meth = clone_mb;

                    ass_mb = tb.DefineMethod("$Assign$", MethodAttributes.Public, TypeFactory.VoidType, new Type[1] { tb });
                    ass_mb.DefineParameter(1, ParameterAttributes.None, "$obj$");
                    ti.assign_meth = ass_mb;
                }
                MethodBuilder fix_mb = tb.DefineMethod("$Fix$", MethodAttributes.Public, TypeFactory.VoidType, Type.EmptyTypes);
                ti.fix_meth = fix_mb;
            }
        }

        private void AddTypeToCloseList(TypeBuilder tb)
        {
            if (!tb.IsValueType) types.Add(tb);
        }

        private void AddEnumToCloseList(EnumBuilder emb)
        {
            enums.Add(emb);
        }

        private Hashtable accessors_names = new Hashtable();

        private string GetPossibleAccessorName(IFunctionNode fn, out bool get_set)
        {
            get_set = false;
            return fn.name;
        }

        private bool has_com_import_attr(ICommonTypeNode value)
        {
            IAttributeNode[] attrs = value.Attributes;
            foreach (IAttributeNode attr in attrs)
                if (attr.AttributeType == SystemLibrary.SystemLibrary.comimport_type)
                    return true;
            return false;
        }

        //Переводит аттрибуты типа в аттрибуты .NET
        private TypeAttributes ConvertAttributes(ICommonTypeNode value)
        {
            TypeAttributes ta = TypeAttributes.Public;
            if (value.type_access_level == type_access_level.tal_internal)
                ta = TypeAttributes.NotPublic;
            //(ssyy) 27.10.2007 Прекратить бардак!  Я в третий раз устанавливаю здесь аттрибут Sealed!
            //Это надо, чтобы нельзя было наследовать от записей.
            //В следующий раз разработчику, снявшему аттрибут Sealed, указать причину, по которой это было сделано!
            if (value.is_value_type)
                ta |= TypeAttributes.SequentialLayout | TypeAttributes.BeforeFieldInit | TypeAttributes.Sealed;
            //TypeAttributes.Sealed нужно!

            if (value.IsSealed)
                ta |= TypeAttributes.Sealed;

            ICompiledTypeNode ictn = value.base_type as ICompiledTypeNode;
            if (ictn != null)
            {
                if (ictn.compiled_type == TypeFactory.MulticastDelegateType)
                {
                    ta |= TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.Sealed;
                }
            }
            if (value.IsInterface)
            {
                ta |= TypeAttributes.Interface | TypeAttributes.Abstract | TypeAttributes.AnsiClass;
            }
            if (value.IsAbstract)
            {
                ta |= TypeAttributes.Abstract;
            }
            return ta;
        }

        public void ConvertTypeInstancesInFunction(ICommonFunctionNode func)
        {
            List<IGenericTypeInstance> insts;
            bool flag = instances_in_functions.TryGetValue(func, out insts);
            if (!flag) return;
            foreach (IGenericTypeInstance igi in insts)
            {
                ConvertTypeHeaderInSpecialOrder(igi);
            }
        }

        public void ConvertTypeInstancesMembersInFunction(ICommonFunctionNode func)
        {
            List<IGenericTypeInstance> insts;
            bool flag = instances_in_functions.TryGetValue(func, out insts);
            if (!flag) return;
            foreach (IGenericTypeInstance igi in insts)
            {
                ConvertGenericInstanceTypeMembers(igi);
            }
        }

        private void AddPropertyAccessors(ICommonTypeNode ctn)
        {
            ICommonPropertyNode[] props = ctn.properties;
            foreach (ICommonPropertyNode prop in props)
            {
                if (prop.get_function != null && !prop_accessors.ContainsKey(prop.get_function))
                    prop_accessors.Add(prop.get_function, prop.get_function);
                if (prop.set_function != null && !prop_accessors.ContainsKey(prop.set_function))
                    prop_accessors.Add(prop.set_function, prop.set_function);
            }
            ICommonMethodNode[] meths = ctn.methods;
            foreach (ICommonMethodNode meth in meths)
            {
                if (meth.overrided_method != null && !prop_accessors.ContainsKey(meth) && prop_accessors.ContainsKey(meth.overrided_method))
                    prop_accessors.Add(meth, meth);
            }
        }

        private string BuildTypeName(string type_name)
        {
            if (type_name.IndexOf(".") == -1)
                return cur_unit + "." + type_name;
            return type_name;
        }

        //Перевод заголовка типа
        private void ConvertTypeHeader(ICommonTypeNode value)
        {
            //(ssyy) Обрабатываем инстанции generic-типов
            //FillGetterMethodsTable(value);
            IGenericTypeInstance igtn = value as IGenericTypeInstance;
            if (igtn != null)
            {
                //Формируем список типов-параметров 
                List<Type> iparams = new List<Type>();
                foreach (ITypeNode itn in igtn.generic_parameters)
                {
                    TypeInfo tinfo = helper.GetTypeReference(itn);
                    if (tinfo == null)
                    {
                        AddTypeInstanceToFunction(GetGenericFunctionContainer(value), igtn);
                        return;
                    }
                    iparams.Add(tinfo.tp);
                }
                //Запрашиваем инстанцию
                //ICompiledTypeNode icompiled_type = igtn.original_generic as ICompiledTypeNode;
                Type orig_type = helper.GetTypeReference(igtn.original_generic).tp;
                Type rez = orig_type.MakeGenericType(iparams.ToArray());
                //Добавляем в хэш
                TypeInfo inst_ti = helper.AddExistingType(igtn, rez);
                TypeInfo generic_def_ti = helper.GetTypeReference(igtn.original_generic);
                if (generic_def_ti.init_meth != null)
                    inst_ti.init_meth = TypeBuilder.GetMethod(rez, generic_def_ti.init_meth);
                if (generic_def_ti.clone_meth != null)
                    inst_ti.clone_meth = TypeBuilder.GetMethod(rez, generic_def_ti.clone_meth);
                if (generic_def_ti.assign_meth != null)
                    inst_ti.assign_meth = TypeBuilder.GetMethod(rez, generic_def_ti.assign_meth);
                return;
            }
            if (comp_opt.target == TargetType.Dll)
                AddPropertyAccessors(value);
            Type[] interfaces = new Type[value.ImplementingInterfaces.Count];
            for (int i = 0; i < interfaces.Length; i++)
            {
                TypeInfo ii_ti = helper.GetTypeReference(value.ImplementingInterfaces[i]);
                interfaces[i] = ii_ti.tp;
            }

            //определяем тип
            TypeInfo ti = helper.GetTypeReference(value);
            bool not_exist = ti == null;
            TypeBuilder tb = null;
            GenericTypeParameterBuilder gtpb = null;
            if (!not_exist)
            {
                tb = ti.tp as TypeBuilder;
                gtpb = ti.tp as GenericTypeParameterBuilder;
            }

            TypeAttributes ta = (not_exist) ? ConvertAttributes(value) : TypeAttributes.NotPublic;

            if (value.base_type is ICompiledTypeNode && (value.base_type as ICompiledTypeNode).compiled_type == TypeFactory.EnumType)
            {
                ta = TypeAttributes.Public;
                if (value.type_access_level == type_access_level.tal_internal)
                    ta = TypeAttributes.NotPublic;
                EnumBuilder emb = mb.DefineEnum(BuildTypeName(value.name), ta, TypeFactory.Int32Type);
                //int num = 0;
                foreach (IClassConstantDefinitionNode ccfn in value.constants)
                {
                    emb.DefineLiteral(ccfn.name, (ccfn.constant_value as IEnumConstNode).constant_value);
                }
                AddEnumToCloseList(emb);
                helper.AddEnum(value, emb);
                return;
            }
            else
            {
                if (not_exist)
                {
                    tb = mb.DefineType(BuildTypeName(value.name), ta, null, interfaces);
                }
                else
                {
                    if (gtpb == null)
                    {
                        foreach (Type interf in interfaces)
                        {
                            tb.AddInterfaceImplementation(interf);
                        }
                    }
                    else
                    {
                        gtpb.SetInterfaceConstraints(interfaces);
                        GenericParameterAttributes gpa = GenericParameterAttributes.None;
                        if (value.is_value_type)
                            gpa |= GenericParameterAttributes.NotNullableValueTypeConstraint;
                        if (value.is_class)
                            gpa |= GenericParameterAttributes.ReferenceTypeConstraint;
                        if (value.methods.Length > 0)
                            gpa |= GenericParameterAttributes.DefaultConstructorConstraint;
                        gtpb.SetGenericParameterAttributes(gpa);
                    }
                }
            }
            //добавлям его во внутр. структуры
            if (not_exist)
            {
                ti = helper.AddType(value, tb);
            }
            //if (value.fields.Length == 1 && value.fields[0].type is ISimpleArrayNode)
            if (value.type_special_kind == type_special_kind.array_wrapper)
            {
                ti.is_arr = true;
            }
            if (value.base_type != null && !value.IsInterface)
            {
                Type base_type = helper.GetTypeReference(value.base_type).tp;
                if (gtpb == null)
                {
                    tb.SetParent(base_type);
                }
                else
                {
                    if (base_type != TypeFactory.ObjectType)
                        gtpb.SetBaseTypeConstraint(base_type);
                }
            }
            if (!value.is_generic_parameter)
            {
                AddTypeToCloseList(tb);//добавляем его в список закрытия
                if (!value.IsInterface && value.type_special_kind != type_special_kind.array_wrapper)
                    AddInitMembers(ti, tb, value);
            }
            //если это обертка над массивом, сразу переводим реализацию
            //if (value.fields.Length == 1 && value.fields[0].type is ISimpleArrayNode) ConvertArrayWrapperType(value);
        }

        //перевод заголовков функций
        private void ConvertFunctionHeaders(ICommonNamespaceFunctionNode[] funcs)
        {
            for (int i = 0; i < funcs.Length; i++)
            {
                IStatementsListNode sl = (IStatementsListNode)funcs[i].function_code;
                IStatementNode[] statements = sl.statements;
                if (statements.Length > 0 && statements[0] is IExternalStatementNode)
                {
                    //функция импортируется из dll
                    ICommonNamespaceFunctionNode func = funcs[i];
                    Type ret_type = null;
                    //получаем тип возвр. значения
                    if (func.return_value_type == null)
                        ret_type = null;//typeof(void);
                    else
                        ret_type = helper.GetTypeReference(func.return_value_type).tp;
                    Type[] param_types = GetParamTypes(func);//получаем параметры процедуры

                    IExternalStatementNode esn = (IExternalStatementNode)statements[0];
                    string module_name = Tools.ReplaceAllKeys(esn.module_name, StandartDirectories);
                    MethodBuilder methb = cur_type.DefinePInvokeMethod(func.name, module_name, esn.name,
                                                                       MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.PinvokeImpl | MethodAttributes.HideBySig,
                                                                       CallingConventions.Standard, ret_type, param_types, CallingConvention.Winapi,
                                                                       CharSet.Ansi);//определяем PInvoke-метод
                    methb.SetImplementationFlags(MethodImplAttributes.PreserveSig);
                    helper.AddMethod(func, methb);
                    IParameterNode[] parameters = func.parameters;
                    //определяем параметры с указанием имени
                    for (int j = 0; j < parameters.Length; j++)
                    {
                        ParameterAttributes pars = ParameterAttributes.None;
                        //if (func.parameters[j].parameter_type == parameter_type.var)
                        //  pars = ParameterAttributes.Out;
                        methb.DefineParameter(j + 1, pars, parameters[j].name);
                    }
                }
                else
                    if (statements.Length > 0 && statements[0] is IPInvokeStatementNode)
                    {
                        //функция импортируется из dll
                        ICommonNamespaceFunctionNode func = funcs[i];
                        Type ret_type = null;
                        //получаем тип возвр. значения
                        if (func.return_value_type == null)
                            ret_type = null;//typeof(void);
                        else
                            ret_type = helper.GetTypeReference(funcs[i].return_value_type).tp;
                        Type[] param_types = GetParamTypes(funcs[i]);//получаем параметры процедуры

                        MethodBuilder methb = cur_type.DefineMethod(func.name, MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.PinvokeImpl | MethodAttributes.HideBySig, ret_type, param_types);//определяем PInvoke-метод
                        methb.SetImplementationFlags(MethodImplAttributes.PreserveSig);
                        helper.AddMethod(funcs[i], methb);
                        IParameterNode[] parameters = funcs[i].parameters;
                        //определяем параметры с указанием имени
                        for (int j = 0; j < parameters.Length; j++)
                        {
                            ParameterAttributes pars = ParameterAttributes.None;
                            //if (func.parameters[j].parameter_type == parameter_type.var)
                            //  pars = ParameterAttributes.Out;
                            methb.DefineParameter(j + 1, pars, parameters[j].name);
                        }
                    }
                    else
                        ConvertFunctionHeader(funcs[i]);
            }
            //(ssyy) 21.05.2008
            foreach (ICommonNamespaceFunctionNode ifn in funcs)
            {
                if (ifn.is_generic_function)
                {
                    ConvertTypeInstancesMembersInFunction(ifn);
                }
            }
        }

        //перевод тел функций
        private void ConvertFunctionsBodies(ICommonFunctionNode[] funcs)
        {
            for (int i = 0; i < funcs.Length; i++)
            {
                IStatementsListNode sl = (IStatementsListNode)funcs[i].function_code;
                /*if (sl.statements.Length > 0 && (sl.statements[0] is IExternalStatementNode))
                {
                    continue;
                }*/
                ConvertFunctionBody(funcs[i]);
            }
        }

        //создание записи активации для влож. процедур
        private Frame MakeAuxType(ICommonFunctionNode func)
        {

            TypeBuilder tb = cur_type.DefineNestedType("$" + func.name + "$" + uid++, TypeAttributes.NestedPublic);
            //определяем поле - ссылку на верхнюю запись активации
            FieldBuilder fb = tb.DefineField("$parent$", tb.DeclaringType.IsValueType ? tb.DeclaringType.MakePointerType() : tb.DeclaringType, FieldAttributes.Public);
            //конструктор в кач-ве параметра, которого передается ссылка на верх. з/а
            ConstructorBuilder cb = null;
            //определяем метод для инициализации
            MethodBuilder mb = tb.DefineMethod("$Init$", MethodAttributes.Private, TypeFactory.VoidType, Type.EmptyTypes);
            if (funcs.Count > 0)
            {
                cb = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, new Type[1] { tb.DeclaringType.IsValueType ? tb.DeclaringType.MakeByRefType() : tb.DeclaringType });
                cb.DefineParameter(1, ParameterAttributes.None, "$parent$");
            }
            else
                cb = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, Type.EmptyTypes);
            ILGenerator il = cb.GetILGenerator();
            //сохраняем ссылку на верхнюю запись активации
            if (func is ICommonNestedInFunctionFunctionNode)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Stfld, fb);
            }
            //вызываем метод $Init$
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, mb);
            il.Emit(OpCodes.Ret);
            types.Add(tb);
            //создаем кадр записи активации
            Frame frm = new Frame();
            frm.cb = cb;
            frm.mb = mb;
            frm.tb = tb;
            frm.parent = fb;
            return frm;
        }

        //перевод псевдоинстанции функции
        private void ConvertGenericFunctionInstance(IGenericFunctionInstance igfi)
        {
            if (helper.GetMethod(igfi) != null)
                return;
            ICompiledMethodNode icm = igfi.original_function as ICompiledMethodNode;
            MethodInfo mi;
            if (icm != null)
            {
                mi = icm.method_info;
            }
            else
            {
                MethInfo methi = helper.GetMethod(igfi.original_function);
                if (methi == null)//not used functions from pcu
                    return;
                mi = methi.mi;
            }
            int tcount = igfi.generic_parameters.Count;
            Type[] tpars = new Type[tcount];
            for (int i = 0; i < tcount; i++)
            {
                TypeInfo ti = helper.GetTypeReference(igfi.generic_parameters[i]);
                if (ti == null)
                    return;
                tpars[i] = ti.tp;
            }
            MethodInfo rez = mi.MakeGenericMethod(tpars);
            helper.AddMethod(igfi, rez);
        }

        //перевод заголовка функции
        private void ConvertFunctionHeader(ICommonFunctionNode func)
        {
            //if (is_in_unit && helper.IsUsed(func)==false) return;
            num_scope++; //увеличиваем глубину обл. видимости
            TypeBuilder tb = null, tmp_type = cur_type;
            Frame frm = null;

            //func.functions_nodes.Length > 0 - имеет вложенные
            //funcs.Count > 0 - сама вложенная
            if (func.functions_nodes.Length > 0 || funcs.Count > 0)
            {
                frm = MakeAuxType(func);//создаем запись активации
                tb = frm.tb;
                cur_type = tb;
            }
            else tb = cur_type;
            MethodAttributes attrs = MethodAttributes.Public | MethodAttributes.Static;
            //определяем саму процедуру/функцию
            MethodBuilder methb = null;
            methb = tb.DefineMethod(func.name, attrs);

            if (func.is_generic_function)
            {
                int count = func.generic_params.Count;
                string[] names = new string[count];
                for (int i = 0; i < count; i++)
                {
                    names[i] = func.generic_params[i].name;
                }
                methb.DefineGenericParameters(names);
                Type[] genargs = methb.GetGenericArguments();
                for (int i = 0; i < count; i++)
                {
                    helper.AddExistingType(func.generic_params[i], genargs[i]);
                }
                foreach (ICommonTypeNode par in func.generic_params)
                {
                    converting_generic_param = par;
                    ConvertTypeHeaderInSpecialOrder(par);
                }
                ConvertTypeInstancesInFunction(func);
            }

            Type ret_type = null;
            //получаем тип возвр. значения
            if (func.return_value_type == null)
                ret_type = TypeFactory.VoidType;
            else
                ret_type = helper.GetTypeReference(func.return_value_type).tp;
            //получаем типы параметров
            Type[] param_types = GetParamTypes(func);

            methb.SetParameters(param_types);
            methb.SetReturnType(ret_type);

            MethInfo mi = null;
            if (smi.Count != 0)
                //добавляем вложенную процедуру, привязывая ее к верхней процедуре
                mi = helper.AddMethod(func, methb, smi.Peek());
            else
                mi = helper.AddMethod(func, methb);
            mi.num_scope = num_scope;
            mi.disp = frm;//тип - запись активации
            smi.Push(mi);
            ParameterBuilder pb = null;
            int num = 0;
            ILGenerator tmp_il = il;
            il = methb.GetILGenerator();

            if (save_debug_info)
            {
                if (func.function_code is IStatementsListNode)
                    MarkSequencePoint(((IStatementsListNode)func.function_code).LeftLogicalBracketLocation);
                else
                    MarkSequencePoint(func.function_code.Location);
            }

            //if (ret_type != typeof(void)) mi.ret_val = il.DeclareLocal(ret_type);
            //если функция вложенная, то добавляем фиктивный параметр
            //ссылку на верхнюю запись активации
            if (funcs.Count > 0)
            {
                mi.nested = true;//это вложенная процедура
                methb.DefineParameter(1, ParameterAttributes.None, "$up$");
                num = 1;
            }
            //все нелокальные параметры будем хранить в нестатических полях
            //записи активации. В начале функции инициализируем эти поля
            //параметрами
            IParameterNode[] parameters = func.parameters;
            FieldBuilder[] fba = new FieldBuilder[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                object default_value = null;
                if (parameters[i].default_value != null)
                {
                    default_value = helper.GetConstantForExpression(parameters[i].default_value);
                }
                ParameterAttributes pa = ParameterAttributes.None;
                //if (func.parameters[i].parameter_type == parameter_type.var)
                //    pa = ParameterAttributes.Retval;
                if (default_value != null)
                    pa |= ParameterAttributes.Optional;
                pb = methb.DefineParameter(i + num + 1, pa, parameters[i].name);

                if (parameters[i].is_params)
                    pb.SetCustomAttribute(TypeFactory.ParamArrayAttributeConstructor, new byte[] { 0x1, 0x0, 0x0, 0x0 });
                if (default_value != null)
                {
                    if (default_value.GetType() != param_types[i + num] && param_types[i + num].IsEnum && (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX))
                        default_value = Enum.ToObject(param_types[i + num], default_value);
                    pb.SetConstant(default_value);
                }
                if (func.functions_nodes.Length > 0)
                {
                    FieldBuilder fb = null;
                    //если параметр передается по значению, то все нормально
                    if (parameters[i].parameter_type == parameter_type.value)
                        fb = frm.tb.DefineField(parameters[i].name, param_types[i + num], FieldAttributes.Public);
                    else
                    {
                        //иначе параметр передается по ссылке
                        //тогда вместо типа параметра тип& используем тип*
                        //например System.Int32& - System.Int32* (unmanaged pointer)
                        Type pt = param_types[i + num].GetElementType().MakePointerType();

                        //определяем поле для параметра
                        fb = frm.tb.DefineField(parameters[i].name, pt, FieldAttributes.Public);
                    }

                    //добавляем как глобальный параметр
                    helper.AddGlobalParameter(parameters[i], fb).meth = smi.Peek();
                    fba[i] = fb;
                }
                else
                {
                    //если проца не содержит вложенных, то все хорошо
                    helper.AddParameter(parameters[i], pb).meth = smi.Peek();
                }
            }

            if (func is ICommonNamespaceFunctionNode && (func as ICommonNamespaceFunctionNode).ConnectedToType != null)
            {
                if (!marked_with_extension_attribute.ContainsKey(cur_unit_type))
                {
                    cur_unit_type.SetCustomAttribute(TypeFactory.ExtensionAttributeType.GetConstructor(new Type[0]), new byte[0]);
                    marked_with_extension_attribute[cur_unit_type] = cur_unit_type;
                }
                methb.SetCustomAttribute(TypeFactory.ExtensionAttributeType.GetConstructor(new Type[0]), new byte[0]);
            }
            if (func.functions_nodes.Length > 0 || funcs.Count > 0)
            {
                //определяем переменную, хранящую ссылку на запись активации данной процедуры
                LocalBuilder frame = il.DeclareLocal(cur_type);
                mi.frame = frame;
                if (doc != null) frame.SetLocalSymInfo("$disp$");
                if (funcs.Count > 0)
                {
                    //если она вложенная, то конструктору зап. акт. передаем ссылку на верх. з. а.
                    il.Emit(OpCodes.Ldarg_0);
                    //создаем запись активации
                    il.Emit(OpCodes.Newobj, frm.cb);
                    il.Emit(OpCodes.Stloc, frame);
                }
                else
                {
                    //в противном случае просто создаем з. а.
                    il.Emit(OpCodes.Newobj, frm.cb);
                    il.Emit(OpCodes.Stloc_0, frame);
                }
                if (func.functions_nodes.Length > 0)
                    for (int j = 0; j < fba.Length; j++)
                    {
                        //сохраняем нелокальные параметры в полях
                        il.Emit(OpCodes.Ldloc_0);
                        parameters = func.parameters;
                        if (parameters[j].parameter_type == parameter_type.value)
                        {
                            if (funcs.Count > 0) il.Emit(OpCodes.Ldarg_S, (byte)(j + 1));
                            else il.Emit(OpCodes.Ldarg_S, (byte)j);
                        }
                        else
                        {
                            if (funcs.Count > 0) il.Emit(OpCodes.Ldarg_S, (byte)(j + 1));
                            else il.Emit(OpCodes.Ldarg_S, (byte)j);
                        }
                        il.Emit(OpCodes.Stfld, fba[j]);
                    }
            }
            funcs.Add(func); //здесь наверное дублирование
            MethodBuilder tmp = cur_meth;
            cur_meth = methb;
            
            //если функция не содержит вложенных процедур, то
            //переводим переменные как локальные
            if (func.functions_nodes.Length > 0)
                ConvertNonLocalVariables(func.var_definition_nodes, frm.mb);
            //переводим заголовки вложенных функций
            ConvertNestedFunctionHeaders(func.functions_nodes);
            //переводим тела вложенных функций
            //foreach (ICommonNestedInFunctionFunctionNode f in func.functions_nodes)
            //	ConvertFunctionBody(f);
            if (frm != null)
                frm.mb.GetILGenerator().Emit(OpCodes.Ret);
            //восстанавливаем текущие значения
            cur_type = tmp_type;
            num_scope--;
            smi.Pop();
            funcs.RemoveAt(funcs.Count - 1);
        }

        private bool IsVoidOrNull(ITypeNode tn)
        {
            if (tn == null)
                return true;
            if ((tn is ICompiledTypeNode) && (tn as ICompiledTypeNode).compiled_type == TypeFactory.VoidType)
                return true;
            return false;
        }


        //Ета штуковина все жутко тормозит. особенно генерацию EXE
        private void AddSpecialDebugVariables()
        {
            if (this.add_special_debug_variables)
            {
                LocalBuilder spec_var = il.DeclareLocal(cur_unit_type);
                spec_var.SetLocalSymInfo("$class_var$0");
            }
        }

        //перевод тела процедуры
        //(ssyy) По-моему, это вызывается только для вложенных процедур.
        private void ConvertFunctionBody(ICommonFunctionNode func)
        {
            //if (is_in_unit && helper.IsUsed(func)==false) return;
            num_scope++;
            MakeAttribute(func);
            IStatementsListNode sl = (IStatementsListNode)func.function_code;
            IStatementNode[] statements = sl.statements;
            if (sl.statements.Length > 0 && (statements[0] is IPInvokeStatementNode || (statements[0] is IExternalStatementNode)))
            {
                num_scope--;
                return;
            }
            MethInfo mi = helper.GetMethod(func);
            TypeBuilder tmp_type = cur_type;
            if (mi.disp != null) cur_type = mi.disp.tb;
            MethodBuilder tmp = cur_meth;
            cur_meth = (MethodBuilder)mi.mi;
            ILGenerator tmp_il = il;
            il = cur_meth.GetILGenerator();
            smi.Push(mi);
            funcs.Add(func);
            ConvertCommonFunctionConstantDefinitions(func.constants);
            if (func.functions_nodes.Length == 0)
                ConvertLocalVariables(func.var_definition_nodes);

            foreach (ICommonNestedInFunctionFunctionNode f in func.functions_nodes)
                ConvertFunctionBody(f);
            //перевод тела
            ConvertBody(func.function_code);
            //ivan for debug
            if (save_debug_info)
            {
                AddSpecialDebugVariables();
            }
            //\ivan for debug
            if (func.return_value_type == null || func.return_value_type == SystemLibrary.SystemLibrary.void_type)
                il.Emit(OpCodes.Ret);
            cur_meth = tmp;
            cur_type = tmp_type;
            il = tmp_il;
            smi.Pop();
            funcs.RemoveAt(funcs.Count - 1);
            num_scope--;
        }

        //перевод тела функции
        private void ConvertFunctionBody(ICommonFunctionNode func, MethInfo mi, bool conv_first_stmt)
        {
            //if (is_in_unit && helper.IsUsed(func)==false) return;
            num_scope++;
            TypeBuilder tmp_type = cur_type;
            if (mi.disp != null) cur_type = mi.disp.tb;
            MethodBuilder tmp = cur_meth;
            cur_meth = (MethodBuilder)mi.mi;
            ILGenerator tmp_il = il;
            il = cur_meth.GetILGenerator();
            smi.Push(mi);
            funcs.Add(func);
            if (conv_first_stmt)
                ConvertBody(func.function_code);//переводим тело
            else
            {
                ConvertStatementsListWithoutFirstStatement(func.function_code as IStatementsListNode);
                OptMakeExitLabel();
            }
            //ivan for debug
            if (save_debug_info)
            {
                AddSpecialDebugVariables();
            }
            //\ivan for debug
            if (cur_meth.ReturnType == TypeFactory.VoidType)
                il.Emit(OpCodes.Ret);
            //восстановление значений
            cur_meth = tmp;
            cur_type = tmp_type;
            il = tmp_il;
            smi.Pop();
            funcs.RemoveAt(funcs.Count - 1);
            num_scope--;
        }

        private void ConvertNestedFunctionHeaders(ICommonNestedInFunctionFunctionNode[] funcs)
        {
            for (int i = 0; i < funcs.Length; i++)
                ConvertFunctionHeader(funcs[i]);
        }

        //процедура получения типов параметров процедуры
        private Type[] GetParamTypes(ICommonFunctionNode func)
        {
            Type[] tt = null;
            int num = 0;
            IParameterNode[] parameters = func.parameters;
            if (funcs.Count > 0)
            {
                tt = new Type[parameters.Length + 1];
                tt[num++] = cur_type.DeclaringType;
            }
            else
                tt = new Type[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                //этот тип уже был определен, поэтому получаем его с помощью хелпера
                Type tp = helper.GetTypeReference(parameters[i].type).tp;
                if (parameters[i].parameter_type == parameter_type.value)
                    tt[i + num] = tp;
                else
                    tt[i + num] = tp.MakeByRefType();
            }
            return tt;
        }

        private void ConvertNonLocalVariables(ILocalVariableNode[] vars, MethodBuilder cb)
        {
            for (int i = 0; i < vars.Length; i++)
            {
                //если лок. переменная используется как нелокальная
                if (vars[i].is_used_as_unlocal == true)
                    ConvertNonLocalVariable(vars[i], cb);
                else
                    ConvertLocalVariable(vars[i], false, 0, 0);
            }
        }

        //создание нелокальной переменной
        //нелок. перем. представляется в виде нестат. поля класса-обертки над процедурой
        private void ConvertNonLocalVariable(ILocalVariableNode var, MethodBuilder cb)
        {
            TypeInfo ti = helper.GetTypeReference(var.type);
            //cur_type сейчас хранит ссылку на созданный тип - обертку
            FieldBuilder fb = cur_type.DefineField(var.name, ti.tp, FieldAttributes.Public);
            VarInfo vi = helper.AddNonLocalVariable(var, fb);
            vi.meth = smi.Peek();
            //если перем. имеет тип массив, то выделяем под него память
            //che-to nelogichno massivy v konstruktore zapisi aktivacii, a konstanty v kode procedury, nado pomenjat
            if (ti.is_arr)
            {
                if (var.inital_value == null || var.inital_value is IArrayConstantNode)
                    CreateArrayForClassField(cb.GetILGenerator(), fb, ti, var.inital_value as IArrayConstantNode, var.type);
                else if (var.inital_value is IArrayInitializer)
                    CreateArrayForClassField(cb.GetILGenerator(), fb, ti, var.inital_value as IArrayInitializer, var.type);
            }
            else if (var.inital_value is IArrayConstantNode)
                CreateArrayForClassField(cb.GetILGenerator(), fb, ti, var.inital_value as IArrayConstantNode, var.type);
            else if (var.inital_value is IArrayInitializer)
                CreateArrayForClassField(cb.GetILGenerator(), fb, ti, var.inital_value as IArrayInitializer, var.type);
            else
                if (var.type.is_value_type || var.inital_value is IConstantNode && !(var.inital_value is INullConstantNode))
                    AddInitCall(vi, fb, ti.init_meth, var.inital_value as IConstantNode);
            in_var_init = true;
            GenerateInitCode(var, il);
            in_var_init = false;
        }

        private void ConvertLocalVariables(ILocalVariableNode[] vars)
        {
            for (int i = 0; i < vars.Length; i++)
                ConvertLocalVariable(vars[i], false, 0, 0);
        }

        //создание локальной переменной
        private void ConvertLocalVariable(IVAriableDefinitionNode var, bool add_line_info, int beg_line, int end_line)
        {
            TypeInfo ti = helper.GetTypeReference(var.type);
            bool pinned = false;
            if (ti.tp.IsPointer) pinned = true;
            LocalBuilder lb = il.DeclareLocal(ti.tp, pinned);
            //если модуль отладочный, задаем имя переменной
            if (save_debug_info)
                if (add_line_info)
                    lb.SetLocalSymInfo(var.name + ":" + beg_line + ":" + end_line);
                else
                    lb.SetLocalSymInfo(var.name);
            helper.AddVariable(var, lb);//добавляем переменную
            if (var.type.is_generic_parameter && var.inital_value == null)
            {
                CreateRuntimeInitCodeWithCheck(il, lb, var.type as ICommonTypeNode);
            }
            if (ti.is_arr)
            {
                if (var.inital_value == null || var.inital_value is IArrayConstantNode)
                    CreateArrayLocalVariable(il, lb, ti, var.inital_value as IArrayConstantNode, var.type);
                else if (var.inital_value is IArrayInitializer)
                    CreateArrayLocalVariable(il, lb, ti, var.inital_value as IArrayInitializer, var.type);
            }
            else if (var.inital_value is IArrayConstantNode)
                CreateArrayLocalVariable(il, lb, ti, var.inital_value as IArrayConstantNode, var.type);
            else if (var.inital_value is IArrayInitializer)
                CreateArrayLocalVariable(il, lb, ti, var.inital_value as IArrayInitializer, var.type);
            else
                if (var.type.is_value_type || var.inital_value is IConstantNode && !(var.inital_value is INullConstantNode))
                    AddInitCall(lb, ti.init_meth, var.inital_value as IConstantNode, var.type);
            if (ti.is_set && var.type.type_special_kind == type_special_kind.set_type && var.inital_value == null)
            {
                il.Emit(OpCodes.Ldnull);
                il.Emit(OpCodes.Ldnull);
                il.Emit(OpCodes.Newobj, ti.def_cnstr);
                il.Emit(OpCodes.Stloc, lb);
            }
            in_var_init = true;
            GenerateInitCode(var, il);
            in_var_init = false;
        }

        private void ConvertGlobalVariables(ICommonNamespaceVariableNode[] vars)
        {
            for (int i = 0; i < vars.Length; i++)
                ConvertGlobalVariable(vars[i]);
        }

        private void PushLdfld(FieldBuilder fb)
        {
            if (fb.IsStatic)
            {
                if (fb.FieldType.IsValueType)
                    il.Emit(OpCodes.Ldsflda, fb);
                else
                    il.Emit(OpCodes.Ldsfld, fb);
            }
            else
            {
                il.Emit(OpCodes.Ldarg_0);
                if (fb.FieldType.IsValueType)
                    il.Emit(OpCodes.Ldflda, fb);
                else
                    il.Emit(OpCodes.Ldfld, fb);
            }
        }

        private void PushLdfldWithOutLdarg(FieldBuilder fb)
        {
            if (fb.IsStatic)
            {
                if (fb.FieldType.IsValueType)
                    il.Emit(OpCodes.Ldsflda, fb);
                else
                    il.Emit(OpCodes.Ldsfld, fb);
            }
            else
            {
                if (fb.FieldType.IsValueType)
                    il.Emit(OpCodes.Ldflda, fb);
                else
                    il.Emit(OpCodes.Ldfld, fb);
            }
        }

        //eto dlja inicializacii nelokalnyh peremennyh, tut nado ispolzovat disp!!!!
        private void AddInitCall(VarInfo vi, FieldBuilder fb, MethodInfo init_meth, IConstantNode init_value)
        {
            if (init_meth != null)
            {
                if (init_value == null || init_value != null && init_value.type.type_special_kind != type_special_kind.set_type && init_value.type.type_special_kind != type_special_kind.base_set_type)
                {
                    //kladem displej tekushej procedury
                    il.Emit(OpCodes.Ldloc, vi.meth.frame);
                    PushLdfldWithOutLdarg(fb);
                    il.Emit(OpCodes.Call, init_meth);
                }
            }
            if (init_value != null)
            {
                if (init_value is IRecordConstantNode)
                {
                    LocalBuilder lb = il.DeclareLocal(fb.FieldType.MakePointerType());
                    il.Emit(OpCodes.Ldloc, vi.meth.frame);
                    PushLdfldWithOutLdarg(fb);
                    il.Emit(OpCodes.Stloc, lb);
                    GenerateRecordInitCode(il, lb, init_value as IRecordConstantNode);
                }
                else
                {
                    if (!fb.IsStatic)
                    {
                        //il.Emit(OpCodes.Ldloc, vi.meth.frame);
                        //il.Emit(OpCodes.Ldfld, fb);

                        il.Emit(OpCodes.Ldloc, vi.meth.frame);
                        init_value.visit(this);
                        EmitBox(init_value, fb.FieldType);
                        il.Emit(OpCodes.Stfld, fb);
                    }
                    else
                    {
                        init_value.visit(this);
                        EmitBox(init_value, fb.FieldType);
                        il.Emit(OpCodes.Stsfld, fb);
                    }
                }
            }
        }

        private void AddInitCall(ILGenerator il, FieldBuilder fb, MethodInfo init_meth, IConstantNode init_value)
        {
            if (init_meth != null)
            {
                if (init_value == null || init_value != null && init_value.type.type_special_kind != type_special_kind.set_type && init_value.type.type_special_kind != type_special_kind.base_set_type)
                {
                    PushLdfld(fb);
                    il.Emit(OpCodes.Call, init_meth);
                }
            }
            if (init_value != null)
            {
                if (init_value is IRecordConstantNode)
                {
                    LocalBuilder lb = il.DeclareLocal(fb.FieldType.MakePointerType());
                    PushLdfld(fb);
                    il.Emit(OpCodes.Stloc, lb);
                    GenerateRecordInitCode(il, lb, init_value as IRecordConstantNode);
                }
                else
                {
                    if (!fb.IsStatic)
                    {
                        //PushLdfld(fb);
                        il.Emit(OpCodes.Ldarg_0);
                        init_value.visit(this);
                        EmitBox(init_value, fb.FieldType);
                        il.Emit(OpCodes.Stfld, fb);
                    }
                    else
                    {
                        init_value.visit(this);
                        EmitBox(init_value, fb.FieldType);
                        il.Emit(OpCodes.Stsfld, fb);
                    }
                }
            }
        }

        private void AddInitCall(LocalBuilder lb, MethodInfo init_meth, IConstantNode init_value, ITypeNode type)
        {
            if (init_meth != null)
            {
                if (init_value == null || init_value != null && init_value.type.type_special_kind != type_special_kind.set_type && init_value.type.type_special_kind != type_special_kind.base_set_type)
                {
                    if (lb.LocalType.IsValueType)
                        il.Emit(OpCodes.Ldloca, lb);
                    else
                        il.Emit(OpCodes.Ldloc, lb);
                    il.Emit(OpCodes.Call, init_meth);
                }
            }
            if (init_value != null)
            {
                if (init_value is IRecordConstantNode)
                {
                    LocalBuilder llb = il.DeclareLocal(lb.LocalType.MakePointerType());
                    il.Emit(OpCodes.Ldloca, lb);
                    il.Emit(OpCodes.Stloc, llb);
                    GenerateRecordInitCode(il, llb, init_value as IRecordConstantNode);
                }
                else
                {
                    init_value.visit(this);
                    EmitBox(init_value, lb.LocalType);
                    il.Emit(OpCodes.Stloc, lb);
                }
            }
        }

        private void AddInitCall(FieldBuilder fb, ILGenerator il, MethodInfo called_mb, ConstructorInfo cnstr, IConstantNode init_value)
        {
            ILGenerator ilc = this.il;
            this.il = il;
            //il = mb.GetILGenerator();
            if (called_mb != null && (init_value == null || init_value.type.type_special_kind != type_special_kind.set_type && init_value.type.type_special_kind != type_special_kind.base_set_type))
            {
                if (fb.IsStatic == false)
                {
                    il.Emit(OpCodes.Ldarg_0);
                    if (fb.FieldType.IsValueType)
                        il.Emit(OpCodes.Ldflda, fb);
                    else
                        il.Emit(OpCodes.Ldfld, fb);
                }
                else
                {
                    if (fb.FieldType.IsValueType)
                        il.Emit(OpCodes.Ldsflda, fb);
                    else
                        il.Emit(OpCodes.Ldsfld, fb);
                }
                il.Emit(OpCodes.Call, called_mb);
            }
            if (init_value != null)
            {
                if (init_value is IRecordConstantNode)
                {
                    LocalBuilder lb = il.DeclareLocal(fb.FieldType.MakePointerType());
                    PushLdfld(fb);
                    il.Emit(OpCodes.Stloc, lb);
                    GenerateRecordInitCode(il, lb, init_value as IRecordConstantNode);
                }
                else
                {
                    if (!(init_value is IStringConstantNode))
                    {
                        if (!fb.IsStatic)
                        {
                            il.Emit(OpCodes.Ldarg_0);
                            init_value.visit(this);
                            EmitBox(init_value, fb.FieldType);
                            il.Emit(OpCodes.Stfld, fb);
                        }
                        else
                        {
                            init_value.visit(this);
                            EmitBox(init_value, fb.FieldType);
                            il.Emit(OpCodes.Stsfld, fb);
                        }
                    }
                    else
                    {
                        if (!fb.IsStatic)
                        {
                            Label lbl = il.DefineLabel();
                            il.Emit(OpCodes.Ldarg_0);
                            il.Emit(OpCodes.Ldfld, fb);
                            il.Emit(OpCodes.Call, TypeFactory.StringNullOrEmptyMethod);
                            il.Emit(OpCodes.Brfalse, lbl);
                            il.Emit(OpCodes.Ldarg_0);
                            init_value.visit(this);
                            il.Emit(OpCodes.Stfld, fb);
                            il.MarkLabel(lbl);
                        }
                        else
                        {
                            Label lbl = il.DefineLabel();
                            il.Emit(OpCodes.Ldsfld, fb);
                            il.Emit(OpCodes.Call, TypeFactory.StringNullOrEmptyMethod);
                            il.Emit(OpCodes.Brfalse, lbl);
                            init_value.visit(this);
                            il.Emit(OpCodes.Stsfld, fb);
                            il.MarkLabel(lbl);
                        }
                    }
                }
            }
            this.il = ilc;
        }

        //(ssyy) Инициализации переменных типа параметр дженерика
        private void CreateRuntimeInitCodeWithCheck(ILGenerator il, LocalBuilder lb, ICommonTypeNode type)
        {
            if (type.runtime_initialization_marker == null) return;
            Type tinfo = helper.GetTypeReference(type).tp;
            FieldInfo finfo = helper.GetField(type.runtime_initialization_marker).fi;
            Label lab = il.DefineLabel();
            il.Emit(OpCodes.Ldsfld, finfo);
            il.Emit(OpCodes.Brfalse, lab);
            il.Emit(OpCodes.Ldsfld, finfo);
            il.Emit(OpCodes.Ldloc, lb);
            il.Emit(OpCodes.Box, tinfo);
            MethodInfo rif = null;
            if (SystemLibrary.SystemLibInitializer.RuntimeInitializeFunction.sym_info is ICompiledMethodNode)
                rif = (SystemLibrary.SystemLibInitializer.RuntimeInitializeFunction.sym_info as ICompiledMethodNode).method_info;
            else
                rif = helper.GetMethod(SystemLibrary.SystemLibInitializer.RuntimeInitializeFunction.sym_info as IFunctionNode).mi;
            il.EmitCall(OpCodes.Call, rif, null);
            il.Emit(OpCodes.Unbox_Any, tinfo);
            il.Emit(OpCodes.Stloc, lb);
            il.MarkLabel(lab);
        }

        //(ssyy) Инициализации полей типа параметр дженерика
        private void CreateRuntimeInitCodeWithCheck(ILGenerator il, FieldBuilder fb, ICommonTypeNode type)
        {
            if (type.runtime_initialization_marker == null) return;
            Type tinfo = helper.GetTypeReference(type).tp;
            FieldInfo finfo = helper.GetField(type.runtime_initialization_marker).fi;
            Label lab = il.DefineLabel();
            il.Emit(OpCodes.Ldsfld, finfo);
            il.Emit(OpCodes.Brfalse, lab);
            if (!fb.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            il.Emit(OpCodes.Ldsfld, finfo);
            if (fb.IsStatic)
            {
                il.Emit(OpCodes.Ldsfld, fb);
            }
            else
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, fb);
            }
            il.Emit(OpCodes.Box, tinfo);
            MethodInfo rif = null;
            if (SystemLibrary.SystemLibInitializer.RuntimeInitializeFunction.sym_info is ICompiledMethodNode)
                rif = (SystemLibrary.SystemLibInitializer.RuntimeInitializeFunction.sym_info as ICompiledMethodNode).method_info;
            else
                rif = helper.GetMethod(SystemLibrary.SystemLibInitializer.RuntimeInitializeFunction.sym_info as IFunctionNode).mi;
            il.EmitCall(OpCodes.Call, rif, null);
            il.Emit(OpCodes.Unbox_Any, tinfo);
            if (fb.IsStatic)
            {
                il.Emit(OpCodes.Stsfld, fb);
            }
            else
            {
                il.Emit(OpCodes.Stfld, fb);
            }
            il.MarkLabel(lab);
        }

        private void CreateArrayForClassField(ILGenerator il, FieldBuilder fb, TypeInfo ti, IArrayInitializer InitalValue, ITypeNode ArrayType)
        {
            int rank = 1;
            il.Emit(OpCodes.Ldarg_0);
            if (NETGeneratorTools.IsBoundedArray(ti))
                NETGeneratorTools.CreateBoundedArray(il, fb, ti);
            else
            {
                rank = get_rank(ArrayType);
                if (rank == 1)
                    CreateUnsizedArray(il, fb, helper.GetTypeReference(InitalValue.ElementType), InitalValue.ElementValues.Length);
                else
                    CreateNDimUnsizedArray(il, fb, ArrayType, helper.GetTypeReference(InitalValue.ElementType), rank, get_sizes(InitalValue, rank));

            }
            if (InitalValue != null)
            {
                LocalBuilder lb = null;
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, fb);
                if (NETGeneratorTools.IsBoundedArray(ti))
                {
                    lb = il.DeclareLocal(ti.arr_fld.FieldType);
                    il.Emit(OpCodes.Ldfld, ti.arr_fld);
                }
                else
                {
                    lb = il.DeclareLocal(ti.tp);
                }
                il.Emit(OpCodes.Stloc, lb);
                if (rank == 1)
                    GenerateArrayInitCode(il, lb, InitalValue, ArrayType);
                else
                    GenerateNDimArrayInitCode(il, lb, InitalValue, ArrayType, rank);
            }
        }

        //поля класса
        private void CreateArrayForClassField(ILGenerator il, FieldBuilder fb, TypeInfo ti, IArrayConstantNode InitalValue, ITypeNode arr_type)
        {
            int rank = 1;
            if (!fb.IsStatic)
                il.Emit(OpCodes.Ldarg_0);
            if (NETGeneratorTools.IsBoundedArray(ti))
                NETGeneratorTools.CreateBoundedArray(il, fb, ti);
            else
            {
                rank = get_rank(arr_type);
                if (rank == 1)
                    CreateUnsizedArray(il, fb, helper.GetTypeReference(InitalValue.ElementType), InitalValue.ElementValues.Length);
                else
                    CreateNDimUnsizedArray(il, fb, arr_type, helper.GetTypeReference(arr_type.element_type), rank, get_sizes(InitalValue, rank));
            }
            if (InitalValue != null)
            {
                LocalBuilder lb = null;
                if (!fb.IsStatic)
                {
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldfld, fb);
                }
                else
                    il.Emit(OpCodes.Ldsfld, fb);
                if (NETGeneratorTools.IsBoundedArray(ti))
                {
                    lb = il.DeclareLocal(ti.arr_fld.FieldType);
                    il.Emit(OpCodes.Ldfld, ti.arr_fld);
                }
                else
                {
                    lb = il.DeclareLocal(ti.tp);
                }
                il.Emit(OpCodes.Stloc, lb);
                if (rank == 1)
                    GenerateArrayInitCode(il, lb, InitalValue);
                else
                    GenerateNDimArrayInitCode(il, lb, InitalValue, arr_type, rank);
            }
        }

        private void CreateRecordLocalVariable(ILGenerator il, LocalBuilder lb, TypeInfo ti, IRecordInitializer InitalValue)
        {
            if (ti.init_meth != null)
            {
                il.Emit(OpCodes.Ldloca, lb);
                il.Emit(OpCodes.Call, ti.init_meth);
            }
            LocalBuilder llb = il.DeclareLocal(lb.LocalType.MakePointerType());
            il.Emit(OpCodes.Ldloca, lb);
            il.Emit(OpCodes.Stloc, llb);
            GenerateRecordInitCode(il, llb, InitalValue, false);
        }

        private void CreateArrayLocalVariable(ILGenerator il, LocalBuilder fb, TypeInfo ti, IArrayInitializer InitalValue, ITypeNode ArrayType)
        {
            int rank = 1;
            if (NETGeneratorTools.IsBoundedArray(ti))
                NETGeneratorTools.CreateBoudedArray(il, fb, ti);
            else
            {
                rank = get_rank(ArrayType);
                if (rank == 1)
                    CreateUnsizedArray(il, fb, helper.GetTypeReference(ArrayType.element_type), InitalValue.ElementValues.Length);
                else
                    CreateNDimUnsizedArray(il, fb, ArrayType, helper.GetTypeReference(ArrayType.element_type), rank, get_sizes(InitalValue, rank));
            }
            if (InitalValue != null)
            {
                LocalBuilder lb = null;
                if (NETGeneratorTools.IsBoundedArray(ti))
                {
                    lb = il.DeclareLocal(ti.arr_fld.FieldType);
                    il.Emit(OpCodes.Ldloc, fb);
                    il.Emit(OpCodes.Ldfld, ti.arr_fld);
                }
                else
                {
                    lb = il.DeclareLocal(ti.tp);
                    il.Emit(OpCodes.Ldloc, fb);
                }
                il.Emit(OpCodes.Stloc, lb);
                if (rank == 1)
                    GenerateArrayInitCode(il, lb, InitalValue, ArrayType);
                else
                    GenerateNDimArrayInitCode(il, lb, InitalValue, ArrayType, rank);
            }
        }

        //создание массива (точнее класса-обертки над массивом) (лок. переменная)
        private void CreateArrayLocalVariable(ILGenerator il, LocalBuilder fb, TypeInfo ti, IArrayConstantNode InitalValue, ITypeNode arr_type)
        {
            int rank = 1;
            if (NETGeneratorTools.IsBoundedArray(ti))
                NETGeneratorTools.CreateBoudedArray(il, fb, ti);
            else
            {
                rank = get_rank(arr_type);
                if (rank == 1)
                    CreateUnsizedArray(il, fb, helper.GetTypeReference(InitalValue.ElementType), InitalValue.ElementValues.Length);
                else
                    CreateNDimUnsizedArray(il, fb, arr_type, helper.GetTypeReference(arr_type.element_type), rank, get_sizes(InitalValue, rank));
            }
            if (InitalValue != null)
            {
                LocalBuilder lb = null;
                if (NETGeneratorTools.IsBoundedArray(ti))
                {
                    lb = il.DeclareLocal(ti.arr_fld.FieldType);
                    il.Emit(OpCodes.Ldloc, fb);
                    il.Emit(OpCodes.Ldfld, ti.arr_fld);
                }
                else
                {
                    lb = il.DeclareLocal(ti.tp);
                    il.Emit(OpCodes.Ldloc, fb);
                }
                il.Emit(OpCodes.Stloc, lb);
                if (rank == 1)
                    GenerateArrayInitCode(il, lb, InitalValue);
                else
                    GenerateNDimArrayInitCode(il, lb, InitalValue, arr_type, rank);
            }
        }

        private int get_rank(ITypeNode t)
        {
            if (t is ICommonTypeNode)
                return (t as ICommonTypeNode).rank;
            else if (t is ICompiledTypeNode)
                return (t as ICompiledTypeNode).rank;
            return 1;
        }

        private int[] get_sizes(IArrayConstantNode InitalValue, int rank)
        {
            List<int> sizes = new List<int>();
            sizes.Add(InitalValue.ElementValues.Length);
            if (rank > 1)
                sizes.AddRange(get_sizes(InitalValue.ElementValues[0] as IArrayConstantNode, rank - 1));
            return sizes.ToArray();
        }

        private int[] get_sizes(IArrayInitializer InitalValue, int rank)
        {
            List<int> sizes = new List<int>();
            sizes.Add(InitalValue.ElementValues.Length);
            if (rank > 1)
                sizes.AddRange(get_sizes(InitalValue.ElementValues[0] as IArrayInitializer, rank - 1));
            return sizes.ToArray();
        }

        private void CreateArrayGlobalVariable(ILGenerator il, FieldBuilder fb, TypeInfo ti, IArrayInitializer InitalValue, ITypeNode arr_type)
        {
            int rank = 1;
            if (NETGeneratorTools.IsBoundedArray(ti))
                NETGeneratorTools.CreateBoundedArray(il, fb, ti);
            else
            {
                rank = get_rank(arr_type);
                if (rank == 1)
                    CreateUnsizedArray(il, fb, helper.GetTypeReference(arr_type.element_type), InitalValue.ElementValues.Length);
                else
                    CreateNDimUnsizedArray(il, fb, arr_type, helper.GetTypeReference(arr_type.element_type), rank, get_sizes(InitalValue, rank));
            }
            if (InitalValue != null)
            {
                LocalBuilder lb = null;
                if (NETGeneratorTools.IsBoundedArray(ti))
                {
                    lb = il.DeclareLocal(ti.arr_fld.FieldType);
                    il.Emit(OpCodes.Ldsfld, fb);
                    il.Emit(OpCodes.Ldfld, ti.arr_fld);
                }
                else
                {
                    lb = il.DeclareLocal(ti.tp);
                    il.Emit(OpCodes.Ldsfld, fb);
                }
                il.Emit(OpCodes.Stloc, lb);
                if (rank == 1)
                    GenerateArrayInitCode(il, lb, InitalValue, arr_type);
                else
                    GenerateNDimArrayInitCode(il, lb, InitalValue, arr_type, rank);
            }
        }

        //глобальные переменные
        private void CreateArrayGlobalVariable(ILGenerator il, FieldBuilder fb, TypeInfo ti, IArrayConstantNode InitalValue, ITypeNode arr_type)
        {
            int rank = 1;
            if (NETGeneratorTools.IsBoundedArray(ti))
                NETGeneratorTools.CreateBoundedArray(il, fb, ti);
            else
            {
                rank = get_rank(arr_type);
                if (rank == 1)
                    CreateUnsizedArray(il, fb, helper.GetTypeReference(InitalValue.ElementType), InitalValue.ElementValues.Length);
                else
                    CreateNDimUnsizedArray(il, fb, arr_type, helper.GetTypeReference(arr_type.element_type), rank, get_sizes(InitalValue, rank));
            }
            if (InitalValue != null)
            {
                LocalBuilder lb = null;
                if (NETGeneratorTools.IsBoundedArray(ti))
                {
                    lb = il.DeclareLocal(ti.arr_fld.FieldType);
                    il.Emit(OpCodes.Ldsfld, fb);
                    il.Emit(OpCodes.Ldfld, ti.arr_fld);
                }
                else
                {
                    lb = il.DeclareLocal(ti.tp);
                    il.Emit(OpCodes.Ldsfld, fb);
                }
                il.Emit(OpCodes.Stloc, lb);
                if (rank == 1)
                    GenerateArrayInitCode(il, lb, InitalValue);
                else
                    GenerateNDimArrayInitCode(il, lb, InitalValue, arr_type, rank);
            }
        }

        private void CreateNDimUnsizedArray(ILGenerator il, FieldBuilder fb, ITypeNode arr_type, TypeInfo ti, int rank, int[] sizes)
        {
            CreateNDimUnsizedArray(il, arr_type, ti, rank, sizes, fb.FieldType.GetElementType());
            if (fb.IsStatic)
                il.Emit(OpCodes.Stsfld, fb);
            else
                il.Emit(OpCodes.Stfld, fb);
        }

        private void CreateNDimUnsizedArray(ILGenerator il, LocalBuilder fb, ITypeNode arr_type, TypeInfo ti, int rank, int[] sizes)
        {
            CreateNDimUnsizedArray(il, arr_type, ti, rank, sizes, fb.LocalType.GetElementType());
            il.Emit(OpCodes.Stloc, fb);
        }

        private void CreateUnsizedArray(ILGenerator il, FieldBuilder fb, TypeInfo ti, int size)
        {
            CreateUnsizedArray(il, ti, size, fb.FieldType.GetElementType());
            if (fb.IsStatic)
                il.Emit(OpCodes.Stsfld, fb);
            else
                il.Emit(OpCodes.Stfld, fb);
            //CreateInitCodeForUnsizedArray(il, ti, fb, size);
        }

        private void CreateUnsizedArray(ILGenerator il, LocalBuilder lb, TypeInfo ti, int size)
        {
            //il.Emit(OpCodes.Ldloca, lb);
            CreateUnsizedArray(il, ti, size, lb.LocalType.GetElementType());
            il.Emit(OpCodes.Stloc, lb);
            //CreateInitCodeForUnsizedArray(il, ti, lb, size);
        }

        private void InitializeNDimUnsizedArray(ILGenerator il, TypeInfo ti, ITypeNode _arr_type, IExpressionNode[] exprs, int rank)
        {
            Type arr_type = helper.GetTypeReference(_arr_type).tp.MakeArrayType(rank);
            LocalBuilder tmp = il.DeclareLocal(arr_type);
            CreateArrayLocalVariable(il, tmp, helper.GetTypeReference((exprs[2 + rank] as IArrayInitializer).type), exprs[2 + rank] as IArrayInitializer, (exprs[2 + rank] as IArrayInitializer).type);
            il.Emit(OpCodes.Ldloc, tmp);
        }

        private void InitializeUnsizedArray(ILGenerator il, TypeInfo ti, ITypeNode _arr_type, IExpressionNode[] exprs, int rank)
        {
            Type arr_type = helper.GetTypeReference(_arr_type).tp.MakeArrayType();
            LocalBuilder tmp = il.DeclareLocal(arr_type);
            il.Emit(OpCodes.Stloc, tmp);
            for (int i = 2 + rank; i < exprs.Length; i++)
            {
                il.Emit(OpCodes.Ldloc, tmp);
                PushIntConst(il, i - 2 - rank);
                ILGenerator ilb = this.il;

                if (ti != null && ti.tp.IsValueType && !TypeFactory.IsStandType(ti.tp) && !ti.tp.IsEnum)
                    il.Emit(OpCodes.Ldelema, ti.tp);

                this.il = il;
                exprs[i].visit(this);
                bool box = EmitBox(exprs[i], arr_type.GetElementType());
                this.il = ilb;

                TypeInfo ti2 = helper.GetTypeReference(exprs[i].type);
                if (ti2 != null && !box)
                    NETGeneratorTools.PushStelem(il, ti2.tp);
                else
                    il.Emit(OpCodes.Stelem_Ref);

            }
            il.Emit(OpCodes.Ldloc, tmp);
        }

        private void CreateInitCodeForUnsizedArray(ILGenerator il, TypeInfo ti, ITypeNode _arr_type, LocalBuilder size)
        {
            Type arr_type = helper.GetTypeReference(_arr_type).tp.MakeArrayType();
            ILGenerator tmp_il = this.il;
            this.il = il;
            if (ti.tp.IsValueType && ti.init_meth != null || ti.is_arr || ti.is_set || ti.is_typed_file || ti.is_text_file || ti.tp == TypeFactory.StringType)
            {
                LocalBuilder tmp = il.DeclareLocal(arr_type);
                il.Emit(OpCodes.Stloc, tmp);
                LocalBuilder clb = il.DeclareLocal(TypeFactory.Int32Type);
                il.Emit(OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Stloc, clb);
                Label tlabel = il.DefineLabel();
                Label flabel = il.DefineLabel();
                il.MarkLabel(tlabel);
                il.Emit(OpCodes.Ldloc, clb);
                il.Emit(OpCodes.Ldloc, size);
                il.Emit(OpCodes.Bge, flabel);
                il.Emit(OpCodes.Ldloc, tmp);
                il.Emit(OpCodes.Ldloc, clb);
                if (!ti.is_arr && !ti.is_set && !ti.is_typed_file && !ti.is_text_file)
                {
                    if (ti.tp != TypeFactory.StringType)
                    {
                        il.Emit(OpCodes.Ldelema, ti.tp);
                        il.Emit(OpCodes.Call, ti.init_meth);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldstr, "");
                        il.Emit(OpCodes.Stelem_Ref);
                    }
                }
                else
                {
                    Label label1 = il.DefineLabel();
                    il.Emit(OpCodes.Ldelem_Ref);
                    il.Emit(OpCodes.Ldnull);
                    il.Emit(OpCodes.Ceq);
                    il.Emit(OpCodes.Brfalse, label1);
                    il.Emit(OpCodes.Ldloc, tmp);
                    il.Emit(OpCodes.Ldloc, clb);
                    if (ti.is_set)
                    {
                        IConstantNode cn1 = (_arr_type as ICommonTypeNode).lower_value;
                        IConstantNode cn2 = (_arr_type as ICommonTypeNode).upper_value;
                        if (cn1 != null && cn2 != null)
                        {
                            cn1.visit(this);
                            il.Emit(OpCodes.Box, helper.GetTypeReference(cn1.type).tp);
                            cn2.visit(this);
                            il.Emit(OpCodes.Box, helper.GetTypeReference(cn2.type).tp);
                        }
                        else
                        {
                            il.Emit(OpCodes.Ldnull);
                            il.Emit(OpCodes.Ldnull);
                        }
                    }
                    else if (ti.is_typed_file)
                    {
                        NETGeneratorTools.PushTypeOf(il, helper.GetTypeReference((_arr_type as ICommonTypeNode).element_type).tp);
                    }
                    il.Emit(OpCodes.Newobj, ti.def_cnstr);
                    il.Emit(OpCodes.Stelem_Ref);
                    il.MarkLabel(label1);
                }
                il.Emit(OpCodes.Ldloc, clb);
                il.Emit(OpCodes.Ldc_I4_1);
                il.Emit(OpCodes.Add);
                il.Emit(OpCodes.Stloc, clb);
                il.Emit(OpCodes.Br, tlabel);
                il.MarkLabel(flabel);
                il.Emit(OpCodes.Ldloc, tmp);
            }
            this.il = tmp_il;
        }

        struct TmpForNDimArr
        {
            public LocalBuilder clb;
            public Label tlabel;
            public Label flabel;

            public TmpForNDimArr(LocalBuilder clb, Label tlabel, Label flabel)
            {
                this.clb = clb;
                this.tlabel = tlabel;
                this.flabel = flabel;
            }
        }

        private void CreateInitCodeForNDimUnsizedArray(ILGenerator il, TypeInfo ti, ITypeNode _arr_type, int rank, IExpressionNode[] exprs)
        {
            Type arr_type = helper.GetTypeReference(_arr_type).tp.MakeArrayType(rank);
            ILGenerator tmp_il = this.il;
            this.il = il;
            MethodInfo set_meth = null;
            MethodInfo addr_meth = null;
            MethodInfo get_meth = null;
            List<Type> lst2 = new List<Type>();
            for (int i = 0; i < exprs.Length; i++)
                lst2.Add(TypeFactory.Int32Type);
            get_meth = mb.GetArrayMethod(arr_type, "Get", CallingConventions.HasThis, ti.tp, lst2.ToArray());
            addr_meth = mb.GetArrayMethod(arr_type, "Address", CallingConventions.HasThis, ti.tp.MakeByRefType(), lst2.ToArray());
            lst2.Add(ti.tp);
            set_meth = mb.GetArrayMethod(arr_type, "Set", CallingConventions.HasThis, TypeFactory.VoidType, lst2.ToArray());
            if (ti.tp.IsValueType && ti.init_meth != null || ti.is_arr || ti.is_set || ti.is_typed_file || ti.is_text_file || ti.tp == TypeFactory.StringType)
            {
                LocalBuilder tmp = il.DeclareLocal(arr_type);
                il.Emit(OpCodes.Stloc, tmp);
                List<TmpForNDimArr> lst = new List<TmpForNDimArr>();
                for (int i = 0; i < exprs.Length; i++)
                {
                    LocalBuilder clb = il.DeclareLocal(TypeFactory.Int32Type);
                    il.Emit(OpCodes.Ldc_I4_0);
                    il.Emit(OpCodes.Stloc, clb);
                    Label tlabel = il.DefineLabel();
                    Label flabel = il.DefineLabel();
                    il.MarkLabel(tlabel);
                    il.Emit(OpCodes.Ldloc, clb);
                    TmpForNDimArr tmp_arr_str = new TmpForNDimArr(clb, tlabel, flabel);
                    lst.Add(tmp_arr_str);
                    exprs[i].visit(this);
                    il.Emit(OpCodes.Bge, flabel);
                }
                il.Emit(OpCodes.Ldloc, tmp);
                for (int i = 0; i < exprs.Length; i++)
                {
                    il.Emit(OpCodes.Ldloc, lst[i].clb);
                }
                if (!ti.is_arr && !ti.is_set && !ti.is_typed_file && !ti.is_text_file)
                {
                    if (ti.tp != TypeFactory.StringType)
                    {
                        il.Emit(OpCodes.Call, addr_meth);
                        il.Emit(OpCodes.Call, ti.init_meth);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldstr, "");
                        il.Emit(OpCodes.Call, set_meth);
                    }
                }
                else
                {
                    Label label1 = il.DefineLabel();
                    il.Emit(OpCodes.Call, get_meth);
                    il.Emit(OpCodes.Ldnull);
                    il.Emit(OpCodes.Ceq);
                    il.Emit(OpCodes.Brfalse, label1);
                    il.Emit(OpCodes.Ldloc, tmp);
                    for (int i = 0; i < exprs.Length; i++)
                    {
                        il.Emit(OpCodes.Ldloc, lst[i].clb);
                    }
                    if (ti.is_set)
                    {
                        IConstantNode cn1 = (_arr_type as ICommonTypeNode).lower_value;
                        IConstantNode cn2 = (_arr_type as ICommonTypeNode).upper_value;
                        if (cn1 != null && cn2 != null)
                        {
                            cn1.visit(this);
                            il.Emit(OpCodes.Box, helper.GetTypeReference(cn1.type).tp);
                            cn2.visit(this);
                            il.Emit(OpCodes.Box, helper.GetTypeReference(cn2.type).tp);
                        }
                        else
                        {
                            il.Emit(OpCodes.Ldnull);
                            il.Emit(OpCodes.Ldnull);
                        }
                    }
                    else if (ti.is_typed_file)
                    {
                        NETGeneratorTools.PushTypeOf(il, helper.GetTypeReference((_arr_type as ICommonTypeNode).element_type).tp);
                    }
                    il.Emit(OpCodes.Newobj, ti.def_cnstr);
                    il.Emit(OpCodes.Call, set_meth);
                    il.MarkLabel(label1);
                }
                for (int i = exprs.Length - 1; i >= 0; i--)
                {
                    il.Emit(OpCodes.Ldloc, lst[i].clb);
                    il.Emit(OpCodes.Ldc_I4_1);
                    il.Emit(OpCodes.Add);
                    il.Emit(OpCodes.Stloc, lst[i].clb);
                    il.Emit(OpCodes.Br, lst[i].tlabel);
                    il.MarkLabel(lst[i].flabel);
                }
                il.Emit(OpCodes.Ldloc, tmp);
            }
            this.il = tmp_il;
        }

        private void CreateInitCodeForUnsizedArray(ILGenerator il, ITypeNode itn, IExpressionNode arr, LocalBuilder len, LocalBuilder start_index=null)
        {
            ILGenerator tmp_il = this.il;
            TypeInfo ti = helper.GetTypeReference(itn);
            ICommonTypeNode ictn = itn as ICommonTypeNode;
            bool generic_param = (ictn != null && ictn.runtime_initialization_marker != null);
            FieldInfo finfo = null;
            MethodInfo rif = null;
            Label lab = default(Label);
            this.il = il;
            if (generic_param)
            {
                finfo = helper.GetField(ictn.runtime_initialization_marker).fi;
                lab = il.DefineLabel();
                il.Emit(OpCodes.Ldsfld, finfo);
                il.Emit(OpCodes.Brfalse, lab);
                if (SystemLibrary.SystemLibInitializer.RuntimeInitializeFunction.sym_info is ICompiledMethodNode)
                    rif = (SystemLibrary.SystemLibInitializer.RuntimeInitializeFunction.sym_info as ICompiledMethodNode).method_info;
                else
                    rif = helper.GetMethod(SystemLibrary.SystemLibInitializer.RuntimeInitializeFunction.sym_info as IFunctionNode).mi;
            }
            if (ti.tp.IsValueType && ti.init_meth != null || ti.is_arr || ti.is_set || ti.is_typed_file || ti.is_text_file || ti.tp == TypeFactory.StringType ||
                (generic_param))
            {
                LocalBuilder clb = null;
                if (start_index == null)
                {
                    clb = il.DeclareLocal(TypeFactory.Int32Type);
                    il.Emit(OpCodes.Ldc_I4_0);
                    il.Emit(OpCodes.Stloc, clb);
                }
                else
                    clb = start_index;
                
                Label tlabel = il.DefineLabel();
                Label flabel = il.DefineLabel();
                il.MarkLabel(tlabel);
                il.Emit(OpCodes.Ldloc, clb);
                il.Emit(OpCodes.Ldloc, len);
                il.Emit(OpCodes.Bge, flabel);
                if (generic_param)
                {
                    arr.visit(this);
                    il.Emit(OpCodes.Ldloc, clb);
                    il.Emit(OpCodes.Ldsfld, finfo);
                }
                arr.visit(this);
                il.Emit(OpCodes.Ldloc, clb);
                if (!ti.is_arr && !ti.is_set && !ti.is_typed_file && !ti.is_text_file)
                {
                    if (generic_param)
                    {
                        il.Emit(OpCodes.Ldelem, ti.tp);
                        il.Emit(OpCodes.Box, ti.tp);
                        il.EmitCall(OpCodes.Call, rif, null);
                        il.Emit(OpCodes.Unbox_Any, ti.tp);
                        il.Emit(OpCodes.Stelem, ti.tp);
                    }
                    else if (ti.tp != TypeFactory.StringType)
                    {
                        il.Emit(OpCodes.Ldelema, ti.tp);
                        il.Emit(OpCodes.Call, ti.init_meth);
                    }
                    else
                    {
                        Label lb1 = il.DefineLabel();
                        Label lb2 = il.DefineLabel();
                        il.Emit(OpCodes.Ldelem_Ref);
                        il.Emit(OpCodes.Ldnull);
                        il.Emit(OpCodes.Beq, lb2);
                        arr.visit(this);
                        il.Emit(OpCodes.Ldloc, clb);
                        il.Emit(OpCodes.Ldelem_Ref);
                        il.Emit(OpCodes.Ldstr, "");
                        il.Emit(OpCodes.Ceq);
                        il.Emit(OpCodes.Brfalse, lb1);
                        il.MarkLabel(lb2);
                        arr.visit(this);
                        il.Emit(OpCodes.Ldloc, clb);
                        il.Emit(OpCodes.Ldstr, "");
                        il.Emit(OpCodes.Stelem_Ref);
                        il.MarkLabel(lb1);
                    }
                }
                else
                {
                    Label label1 = il.DefineLabel();
                    il.Emit(OpCodes.Ldelem_Ref);
                    il.Emit(OpCodes.Ldnull);
                    il.Emit(OpCodes.Ceq);
                    il.Emit(OpCodes.Brfalse, label1);
                    arr.visit(this);
                    il.Emit(OpCodes.Ldloc, clb);
                    if (ti.is_set)
                    {
                        IConstantNode cn1 = (arr.type.element_type as ICommonTypeNode).lower_value;
                        IConstantNode cn2 = (arr.type.element_type as ICommonTypeNode).upper_value;
                        if (cn1 != null && cn2 != null)
                        {
                            cn1.visit(this);
                            il.Emit(OpCodes.Box, helper.GetTypeReference(cn1.type).tp);
                            cn2.visit(this);
                            il.Emit(OpCodes.Box, helper.GetTypeReference(cn2.type).tp);
                        }
                        else
                        {
                            il.Emit(OpCodes.Ldnull);
                            il.Emit(OpCodes.Ldnull);
                        }
                    }
                    else if (ti.is_typed_file)
                    {
                        NETGeneratorTools.PushTypeOf(il, helper.GetTypeReference((arr.type.element_type as ICommonTypeNode).element_type).tp);
                    }

                    il.Emit(OpCodes.Newobj, ti.def_cnstr);

                    il.Emit(OpCodes.Stelem_Ref);
                    il.MarkLabel(label1);
                }
                il.Emit(OpCodes.Ldloc, clb);
                il.Emit(OpCodes.Ldc_I4_1);
                il.Emit(OpCodes.Add);
                il.Emit(OpCodes.Stloc, clb);
                il.Emit(OpCodes.Br, tlabel);
                il.MarkLabel(flabel);
            }
            if (generic_param)
            {
                il.MarkLabel(lab);
            }
            this.il = tmp_il;
        }

        private void CreateInitCodeForUnsizedArray(ILGenerator il, TypeInfo ti, IExpressionNode arr, IExpressionNode size)
        {
            ILGenerator tmp_il = this.il;
            this.il = il;
            if (ti.tp.IsValueType && ti.init_meth != null || ti.is_arr || ti.is_set || ti.is_typed_file || ti.is_text_file || ti.tp == TypeFactory.StringType)
            {
                LocalBuilder clb = il.DeclareLocal(TypeFactory.Int32Type);
                il.Emit(OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Stloc, clb);
                Label tlabel = il.DefineLabel();
                Label flabel = il.DefineLabel();
                il.MarkLabel(tlabel);
                il.Emit(OpCodes.Ldloc, clb);
                size.visit(this);
                il.Emit(OpCodes.Bge, flabel);
                arr.visit(this);
                il.Emit(OpCodes.Ldloc, clb);
                if (!ti.is_arr && !ti.is_set && !ti.is_typed_file && !ti.is_text_file)
                {
                    if (ti.tp != TypeFactory.StringType)
                    {
                        il.Emit(OpCodes.Ldelema, ti.tp);
                        il.Emit(OpCodes.Call, ti.init_meth);
                    }
                    else
                    {
                        Label lb1 = il.DefineLabel();
                        Label lb2 = il.DefineLabel();
                        il.Emit(OpCodes.Ldelem_Ref);
                        il.Emit(OpCodes.Ldnull);
                        il.Emit(OpCodes.Beq, lb2);
                        arr.visit(this);
                        il.Emit(OpCodes.Ldloc, clb);
                        il.Emit(OpCodes.Ldelem_Ref);
                        il.Emit(OpCodes.Ldstr, "");
                        il.Emit(OpCodes.Ceq);
                        il.Emit(OpCodes.Brfalse, lb1);
                        il.MarkLabel(lb2);
                        arr.visit(this);
                        il.Emit(OpCodes.Ldloc, clb);
                        il.Emit(OpCodes.Ldstr, "");
                        il.Emit(OpCodes.Stelem_Ref);
                        il.MarkLabel(lb1);
                    }
                }
                else
                {
                    Label label1 = il.DefineLabel();
                    il.Emit(OpCodes.Ldelem_Ref);
                    il.Emit(OpCodes.Ldnull);
                    il.Emit(OpCodes.Ceq);
                    il.Emit(OpCodes.Brfalse, label1);
                    arr.visit(this);
                    il.Emit(OpCodes.Ldloc, clb);
                    if (ti.is_set)
                    {
                        IConstantNode cn1 = (arr.type.element_type as ICommonTypeNode).lower_value;
                        IConstantNode cn2 = (arr.type.element_type as ICommonTypeNode).upper_value;
                        if (cn1 != null && cn2 != null)
                        {
                            cn1.visit(this);
                            il.Emit(OpCodes.Box, helper.GetTypeReference(cn1.type).tp);
                            cn2.visit(this);
                            il.Emit(OpCodes.Box, helper.GetTypeReference(cn2.type).tp);
                        }
                        else
                        {
                            il.Emit(OpCodes.Ldnull);
                            il.Emit(OpCodes.Ldnull);
                        }
                    }
                    else if (ti.is_typed_file)
                    {
                        NETGeneratorTools.PushTypeOf(il, helper.GetTypeReference((arr.type.element_type as ICommonTypeNode).element_type).tp);
                    }

                    il.Emit(OpCodes.Newobj, ti.def_cnstr);

                    il.Emit(OpCodes.Stelem_Ref);
                    il.MarkLabel(label1);
                }
                il.Emit(OpCodes.Ldloc, clb);
                il.Emit(OpCodes.Ldc_I4_1);
                il.Emit(OpCodes.Add);
                il.Emit(OpCodes.Stloc, clb);
                il.Emit(OpCodes.Br, tlabel);
                il.MarkLabel(flabel);
            }
            this.il = tmp_il;
        }

        private void CreateNDimUnsizedArray(ILGenerator il, ITypeNode ArrType, TypeInfo ti, int rank, int[] sizes, Type elem_type)
        {
            Type arr_type = helper.GetTypeReference(ArrType).tp;
            List<Type> types = new List<Type>();
            for (int i = 2; i < rank + 2; i++)
                types.Add(TypeFactory.Int32Type);
            ConstructorInfo ci = null;
            MethodInfo mi = null;
            if (ArrType is ICompiledTypeNode)
                ci = arr_type.GetConstructor(types.ToArray());
            else
                mi = mb.GetArrayMethod(arr_type, ".ctor", CallingConventions.HasThis, null, types.ToArray());
            for (int i = 0; i < sizes.Length; i++)
                il.Emit(OpCodes.Ldc_I4, sizes[i]);
            if (ci != null)
                il.Emit(OpCodes.Newobj, ci);
            else
                il.Emit(OpCodes.Newobj, mi);
        }

        private void CreateUnsizedArray(ILGenerator il, TypeInfo ti, int size, Type elem_type)
        {
            PushIntConst(il, size);
            if (ti != null)
                il.Emit(OpCodes.Newarr, ti.tp);
            else
                il.Emit(OpCodes.Newarr, elem_type);
        }

        private void CreateUnsizedArray(ILGenerator il, TypeInfo ti, LocalBuilder size)
        {
            il.Emit(OpCodes.Ldloc, size);
            il.Emit(OpCodes.Newarr, ti.tp);
        }

        private void CreateNDimUnsizedArray(ILGenerator il, TypeInfo ti, ITypeNode tn, int rank, IExpressionNode[] sizes)
        {
            Type arr_type = ti.tp.MakeArrayType(rank);
            List<Type> types = new List<Type>();
            for (int i = 2; i < rank + 2; i++)
                types.Add(TypeFactory.Int32Type);
            ConstructorInfo ci = null;
            MethodInfo mi = null;
            if (tn is ICompiledTypeNode)
                ci = arr_type.GetConstructor(types.ToArray());
            else
                mi = mb.GetArrayMethod(arr_type, ".ctor", CallingConventions.HasThis, null, types.ToArray());
            ILGenerator tmp_il = this.il;
            this.il = il;
            for (int i = 2; i < rank + 2; i++)
                sizes[i].visit(this);
            this.il = tmp_il;
            if (ci != null)
                il.Emit(OpCodes.Newobj, ci);
            else
                il.Emit(OpCodes.Newobj, mi);
        }

        private void CreateUnsizedArray(ILGenerator il, TypeInfo ti, IExpressionNode size)
        {
            size.visit(this);
            il.Emit(OpCodes.Newarr, ti.tp);
        }

        private void EmitArrayIndex(ILGenerator il, MethodInfo set_meth, LocalBuilder lb, IArrayConstantNode InitalValue, int rank, int act_rank, List<int> indices)
        {
            IConstantNode[] ElementValues = InitalValue.ElementValues;
            for (int i = 0; i < ElementValues.Length; i++)
            {
                if (indices.Count < act_rank)
                    indices.Add(i);
                else
                    indices[indices.Count - rank] = i;
                if (rank > 1)
                    EmitArrayIndex(il, set_meth, lb, ElementValues[i] as IArrayConstantNode, rank - 1, act_rank, indices);
                else
                {
                    if (indices.Count < act_rank)
                        indices.Add(i);
                    else
                        indices[indices.Count - rank] = i;
                    il.Emit(OpCodes.Ldloc, lb);
                    for (int j = 0; j < indices.Count; j++)
                        il.Emit(OpCodes.Ldc_I4, indices[j]);
                    ILGenerator tmp_il = this.il;
                    this.il = il;
                    ElementValues[i].visit(this);
                    EmitBox(InitalValue.ElementValues[i], lb.LocalType.GetElementType());
                    this.il = tmp_il;
                    il.Emit(OpCodes.Call, set_meth);
                }
            }
        }

        private void EmitArrayIndex(ILGenerator il, MethodInfo set_meth, LocalBuilder lb, IArrayInitializer InitalValue, int rank, int act_rank, List<int> indices)
        {
            IExpressionNode[] ElementValues = InitalValue.ElementValues;
            for (int i = 0; i < ElementValues.Length; i++)
            {
                if (indices.Count < act_rank)
                    indices.Add(i);
                else
                    indices[indices.Count - rank] = i;
                if (rank > 1)
                    EmitArrayIndex(il, set_meth, lb, ElementValues[i] as IArrayInitializer, rank - 1, act_rank, indices);
                else
                {
                    if (indices.Count < act_rank)
                        indices.Add(i);
                    else
                        indices[indices.Count - rank] = i;
                    il.Emit(OpCodes.Ldloc, lb);
                    for (int j = 0; j < indices.Count; j++)
                        il.Emit(OpCodes.Ldc_I4, indices[j]);
                    ILGenerator tmp_il = this.il;
                    this.il = il;
                    ElementValues[i].visit(this);
                    EmitBox(ElementValues[i], lb.LocalType.GetElementType());
                    this.il = tmp_il;
                    il.Emit(OpCodes.Call, set_meth);
                }
            }
        }

        private void GenerateNDimArrayInitCode(ILGenerator il, LocalBuilder lb, IArrayConstantNode InitalValue, ITypeNode ArrayType, int rank)
        {
            IConstantNode[] ElementValues = InitalValue.ElementValues;
            Type elem_type = helper.GetTypeReference(ArrayType.element_type).tp;
            MethodInfo set_meth = null;
            if (ArrayType is ICompiledTypeNode)
                set_meth = lb.LocalType.GetMethod("Set");
            else
            {
                List<Type> lst = new List<Type>();
                for (int i = 0; i < rank; i++)
                    lst.Add(TypeFactory.Int32Type);
                lst.Add(elem_type);
                set_meth = mb.GetArrayMethod(lb.LocalType, "Set", CallingConventions.HasThis, TypeFactory.VoidType, lst.ToArray());
            }
            List<int> indices = new List<int>();
            for (int i = 0; i < ElementValues.Length; i++)
            {
                if (i == 0)
                    indices.Add(i);
                else
                    indices[indices.Count - rank] = i;
                EmitArrayIndex(il, set_meth, lb, ElementValues[i] as IArrayConstantNode, rank - 1, rank, indices);
            }
        }

        private void GenerateNDimArrayInitCode(ILGenerator il, LocalBuilder lb, IArrayInitializer InitalValue, ITypeNode ArrayType, int rank)
        {
            IExpressionNode[] ElementValues = InitalValue.ElementValues;
            Type elem_type = helper.GetTypeReference(ArrayType.element_type).tp;
            MethodInfo set_meth = null;
            if (ArrayType is ICompiledTypeNode)
                set_meth = lb.LocalType.GetMethod("Set");
            else
            {
                List<Type> lst = new List<Type>();
                for (int i = 0; i < rank; i++)
                    lst.Add(TypeFactory.Int32Type);
                lst.Add(elem_type);
                set_meth = mb.GetArrayMethod(lb.LocalType, "Set", CallingConventions.HasThis, TypeFactory.VoidType, lst.ToArray());
            }
            List<int> indices = new List<int>();
            for (int i = 0; i < ElementValues.Length; i++)
            {
                if (i == 0)
                    indices.Add(i);
                else
                    indices[indices.Count - rank] = i;
                EmitArrayIndex(il, set_meth, lb, ElementValues[i] as IArrayInitializer, rank - 1, rank, indices);
            }
        }

        private void GenerateArrayInitCode(ILGenerator il, LocalBuilder lb, IArrayInitializer InitalValue, ITypeNode ArrayType)
        {
            IExpressionNode[] ElementValues = InitalValue.ElementValues;
            if (ElementValues[0] is IArrayInitializer)
            {
                bool is_unsized_array;
                Type FieldType, ArrType;
                int rank = get_rank(ElementValues[0].type);
                TypeInfo ti = helper.GetTypeReference(ElementValues[0].type);
                if (NETGeneratorTools.IsBoundedArray(ti))
                {
                    is_unsized_array = false;
                    ArrType = ti.tp;
                    FieldType = ti.arr_fld.FieldType;
                }
                else
                {
                    is_unsized_array = true;
                    ArrType = helper.GetTypeReference(ElementValues[0].type).tp;
                    FieldType = ArrType;
                }
                LocalBuilder llb = il.DeclareLocal(FieldType);
                for (int i = 0; i < ElementValues.Length; i++)
                {
                    il.Emit(OpCodes.Ldloc, lb);
                    PushIntConst(il, i);
                    if (!is_unsized_array)
                    {
                        il.Emit(OpCodes.Ldelem, ArrType);
                        il.Emit(OpCodes.Ldfld, ti.arr_fld);
                    }
                    else
                    {
                        if (rank > 1)
                            CreateNDimUnsizedArray(il, (ElementValues[i] as IArrayInitializer).type, helper.GetTypeReference((ElementValues[i] as IArrayInitializer).type.element_type), rank, get_sizes(ElementValues[0] as IArrayInitializer, rank), lb.LocalType.GetElementType());
                        else
                            CreateUnsizedArray(il, helper.GetTypeReference((ElementValues[i] as IArrayInitializer).type.element_type), (ElementValues[0] as IArrayInitializer).ElementValues.Length, lb.LocalType.GetElementType());
                        il.Emit(OpCodes.Stelem, ArrType);
                        il.Emit(OpCodes.Ldloc, lb);
                        PushIntConst(il, i);
                        il.Emit(OpCodes.Ldelem, ArrType);
                    }
                    il.Emit(OpCodes.Stloc, llb);
                    if (rank > 1)
                        GenerateNDimArrayInitCode(il, llb, ElementValues[i] as IArrayInitializer, ElementValues[i].type, rank);
                    else
                        GenerateArrayInitCode(il, llb, ElementValues[i] as IArrayInitializer, ArrayType);
                }
            }
            else
                if (ElementValues[0] is IRecordConstantNode || ElementValues[0] is IRecordInitializer)
                {
                    TypeInfo ti = helper.GetTypeReference(ElementValues[0].type);
                    LocalBuilder llb = il.DeclareLocal(ti.tp.MakePointerType());
                    for (int i = 0; i < ElementValues.Length; i++)
                    {
                        il.Emit(OpCodes.Ldloc, lb);
                        PushIntConst(il, i);
                        il.Emit(OpCodes.Ldelema, ti.tp);
                        il.Emit(OpCodes.Stloc, llb);
                        if (ElementValues[0] is IRecordConstantNode)
                            GenerateRecordInitCode(il, llb, ElementValues[i] as IRecordConstantNode);
                        else GenerateRecordInitCode(il, llb, ElementValues[i] as IRecordInitializer, true);
                    }
                }
                else
                    for (int i = 0; i < ElementValues.Length; i++)
                    {
                        il.Emit(OpCodes.Ldloc, lb);
                        PushIntConst(il, i);
                        ILGenerator ilb = this.il;
                        TypeInfo ti = helper.GetTypeReference(ElementValues[i].type);

                        if (ti != null && ti.is_set)
                        {
                            this.il = il;
                            IConstantNode cn1 = null;
                            IConstantNode cn2 = null;
                            if (ArrayType != null && ArrayType.element_type.element_type is ICommonTypeNode)
                            {
                                cn1 = (ArrayType.element_type.element_type as ICommonTypeNode).lower_value;
                                cn2 = (ArrayType.element_type.element_type as ICommonTypeNode).upper_value;
                            }
                            if (cn1 != null && cn2 != null)
                            {
                                cn1.visit(this);
                                il.Emit(OpCodes.Box, helper.GetTypeReference(cn1.type).tp);
                                cn2.visit(this);
                                il.Emit(OpCodes.Box, helper.GetTypeReference(cn2.type).tp);
                            }
                            else
                            {
                                il.Emit(OpCodes.Ldnull);
                                il.Emit(OpCodes.Ldnull);
                            }
                            il.Emit(OpCodes.Newobj, ti.def_cnstr);
                            il.Emit(OpCodes.Stelem_Ref);
                            il.Emit(OpCodes.Ldloc, lb);
                            PushIntConst(il, i);
                            this.il = ilb;
                        }
                        if (ti != null && ti.tp.IsValueType && !TypeFactory.IsStandType(ti.tp) && !ti.tp.IsEnum)
                            il.Emit(OpCodes.Ldelema, ti.tp);
                        else
                            if (ti != null && ti.assign_meth != null)
                                il.Emit(OpCodes.Ldelem_Ref);
                        this.il = il;
                        ElementValues[i].visit(this);
                        if (ti != null && ti.assign_meth != null)
                        {
                            il.Emit(OpCodes.Call, ti.assign_meth);
                            this.il = ilb;
                            continue;
                        }
                        bool box = EmitBox(ElementValues[i], lb.LocalType.GetElementType());
                        this.il = ilb;
                        if (ti != null && !box)
                            NETGeneratorTools.PushStelem(il, ti.tp);
                        else
                            il.Emit(OpCodes.Stelem_Ref);
                    }
        }

        private void GenerateArrayInitCode(ILGenerator il, LocalBuilder lb, IArrayConstantNode InitalValue)
        {
            IExpressionNode[] ElementValues = InitalValue.ElementValues;
            if (ElementValues[0] is IArrayConstantNode)
            {
                bool is_unsized_array;
                Type FieldType, ArrType;
                TypeInfo ti = null;
                if (NETGeneratorTools.IsBoundedArray(helper.GetTypeReference(ElementValues[0].type)))
                {
                    is_unsized_array = false;
                    ti = helper.GetTypeReference(ElementValues[0].type);
                    ArrType = ti.tp;
                    FieldType = ti.arr_fld.FieldType;
                }
                else
                {
                    is_unsized_array = true;
                    ArrType = helper.GetTypeReference(ElementValues[0].type).tp;
                    FieldType = ArrType;
                }
                LocalBuilder llb = il.DeclareLocal(FieldType);
                for (int i = 0; i < ElementValues.Length; i++)
                {
                    il.Emit(OpCodes.Ldloc, lb);
                    PushIntConst(il, i);
                    if (!is_unsized_array)
                    {
                        il.Emit(OpCodes.Ldelem, ArrType);
                        il.Emit(OpCodes.Ldfld, ti.arr_fld);
                    }
                    else
                    {
                        CreateUnsizedArray(il, helper.GetTypeReference((ElementValues[i] as IArrayConstantNode).type.element_type), (ElementValues[0] as IArrayConstantNode).ElementValues.Length, lb.LocalType.GetElementType());
                        il.Emit(OpCodes.Stelem, ArrType);
                        il.Emit(OpCodes.Ldloc, lb);
                        PushIntConst(il, i);
                        il.Emit(OpCodes.Ldelem, ArrType);
                    }
                    il.Emit(OpCodes.Stloc, llb);
                    GenerateArrayInitCode(il, llb, ElementValues[i] as IArrayConstantNode);
                }
            }
            else
                if (ElementValues[0] is IRecordConstantNode)
                {
                    TypeInfo ti = helper.GetTypeReference(ElementValues[0].type);
                    LocalBuilder llb = il.DeclareLocal(ti.tp.MakePointerType());
                    for (int i = 0; i < ElementValues.Length; i++)
                    {
                        il.Emit(OpCodes.Ldloc, lb);
                        PushIntConst(il, i);
                        il.Emit(OpCodes.Ldelema, ti.tp);
                        il.Emit(OpCodes.Stloc, llb);
                        GenerateRecordInitCode(il, llb, ElementValues[i] as IRecordConstantNode);
                    }
                }
                else
                    for (int i = 0; i < ElementValues.Length; i++)
                    {
                        il.Emit(OpCodes.Ldloc, lb);
                        TypeInfo ti = helper.GetTypeReference(ElementValues[i].type);
                        PushIntConst(il, i);
                        if (ti != null && ti.tp.IsValueType && !TypeFactory.IsStandType(ti.tp) && !ti.tp.IsEnum)
                            il.Emit(OpCodes.Ldelema, ti.tp);
                        ILGenerator ilb = this.il;
                        this.il = il;
                        ElementValues[i].visit(this);
                        this.il = ilb;
                        bool box = EmitBox(ElementValues[i], lb.LocalType.GetElementType());
                        this.il = ilb;
                        if (ti != null && !box)
                            NETGeneratorTools.PushStelem(il, ti.tp);
                        else
                            il.Emit(OpCodes.Stelem_Ref);
                    }
        }

        private void GenerateRecordInitCode(ILGenerator il, LocalBuilder lb, IRecordInitializer init_value, bool is_in_arr)
        {
            ICommonTypeNode ctn = init_value.type as ICommonTypeNode;
            IExpressionNode[] FieldValues = init_value.FieldValues;
            ICommonClassFieldNode[] Fields = ctn.fields;

            for (int i = 0; i < Fields.Length; i++)
            {
                FldInfo field = helper.GetField(Fields[i]);
                if (FieldValues[i] is IArrayInitializer)
                {
                    TypeInfo ti = helper.GetTypeReference(FieldValues[i].type);
                    LocalBuilder alb = il.DeclareLocal(ti.tp);
                    CreateArrayLocalVariable(il, alb, ti, FieldValues[i] as IArrayInitializer, FieldValues[i].type);
                    il.Emit(OpCodes.Ldloc, lb);
                    il.Emit(OpCodes.Ldloc, alb);
                    il.Emit(OpCodes.Stfld, field.fi);
                }
                else
                    if (FieldValues[i] is IRecordInitializer)
                    {
                        LocalBuilder llb = il.DeclareLocal(field.fi.FieldType.MakePointerType());
                        il.Emit(OpCodes.Ldloc, lb);
                        il.Emit(OpCodes.Ldflda, field.fi);
                        il.Emit(OpCodes.Stloc, llb);
                        GenerateRecordInitCode(il, llb, FieldValues[i] as IRecordInitializer, false);
                    }
                    else
                    {
                        is_dot_expr = false;
                        if (is_in_arr)
                            il.Emit(OpCodes.Ldloc, lb);
                        else
                            il.Emit(OpCodes.Ldloc, lb);
                        ILGenerator tmp_il = this.il;
                        this.il = il;
                        FieldValues[i].visit(this);
                        this.il = tmp_il;
                        il.Emit(OpCodes.Stfld, field.fi);
                    }
            }
        }

        private void GenerateRecordInitCode(ILGenerator il, LocalBuilder lb, IRecordConstantNode init_value)
        {
            ICommonTypeNode ctn = init_value.type as ICommonTypeNode;
            IConstantNode[] FieldValues = init_value.FieldValues;
            ICommonClassFieldNode[] Fields = ctn.fields;

            for (int i = 0; i < Fields.Length; i++)
            {
                FldInfo field = helper.GetField(Fields[i]);
                if (FieldValues[i] is IArrayConstantNode)
                {
                    TypeInfo ti = helper.GetTypeReference(FieldValues[i].type);
                    LocalBuilder alb = il.DeclareLocal(ti.tp);
                    CreateArrayLocalVariable(il, alb, ti, FieldValues[i] as IArrayConstantNode, FieldValues[i].type);
                    il.Emit(OpCodes.Ldloc, lb);
                    il.Emit(OpCodes.Ldloc, alb);
                    il.Emit(OpCodes.Stfld, field.fi);
                }
                else
                    if (FieldValues[i] is IRecordConstantNode)
                    {
                        LocalBuilder llb = il.DeclareLocal(field.fi.FieldType.MakePointerType());
                        il.Emit(OpCodes.Ldloc, lb);
                        il.Emit(OpCodes.Ldflda, field.fi);
                        il.Emit(OpCodes.Stloc, llb);
                        GenerateRecordInitCode(il, llb, FieldValues[i] as IRecordConstantNode);
                    }
                    else
                    {
                        //bool tmp = is_dot_expr;

                        is_dot_expr = false;
                        il.Emit(OpCodes.Ldloc, lb);
                        ILGenerator tmp_il = this.il;
                        this.il = il;
                        FieldValues[i].visit(this);
                        this.il = tmp_il;
                        il.Emit(OpCodes.Stfld, field.fi);
                        //is_dot_expr = tmp;
                    }
            }
        }

        private bool in_var_init = false;

        private Type get_type_reference_for_pascal_attributes(ITypeNode tn)
        {
            if (tn.type_special_kind == type_special_kind.short_string)
            {
                return CreateShortStringType(tn);
            }
            else if (tn.type_special_kind == type_special_kind.typed_file)
            {
                return CreateTypedFileType(tn as ICommonTypeNode);
            }
            else if (tn.type_special_kind == type_special_kind.set_type)
            {
                return CreateTypedSetType(tn as ICommonTypeNode);
            }
            else
                return helper.GetTypeReference(tn).tp;
        }

        private void add_possible_type_attribute(TypeBuilder tb, ITypeSynonym type)
        {
            Type orig_type = get_type_reference_for_pascal_attributes(type.original_type);
            CustomAttributeBuilder cust_bldr = null;
            if (type.original_type is ICompiledTypeNode || type.original_type is IRefTypeNode && (type.original_type as IRefTypeNode).pointed_type is ICompiledTypeNode)
                cust_bldr = new CustomAttributeBuilder(this.TypeSynonimAttributeConstructor, new object[1] { orig_type });
            else
                cust_bldr = new CustomAttributeBuilder(this.TypeSynonimAttributeConstructor, new object[1] { orig_type.FullName });
            tb.SetCustomAttribute(cust_bldr);
        }

        private void add_possible_type_attribute(TypeBuilder tb, ITypeNode type)
        {
            if (comp_opt.target != TargetType.Dll)
                return;
            if (type.type_special_kind == type_special_kind.typed_file)
            {
                Type elem_type = helper.GetTypeReference(type.element_type).tp;
                CustomAttributeBuilder cust_bldr = null;
                if (type.element_type is ICompiledTypeNode || type.element_type is IRefTypeNode && (type.element_type as IRefTypeNode).pointed_type is ICompiledTypeNode)
                    cust_bldr = new CustomAttributeBuilder(this.FileOfAttributeConstructor, new object[1] { elem_type });
                else
                    cust_bldr = new CustomAttributeBuilder(this.FileOfAttributeConstructor, new object[1] { elem_type.FullName });
                tb.SetCustomAttribute(cust_bldr);
            }
            else if (type.type_special_kind == type_special_kind.set_type)
            {
                Type elem_type = helper.GetTypeReference(type.element_type).tp;
                CustomAttributeBuilder cust_bldr = null;
                if (type.element_type is ICompiledTypeNode || type.element_type is IRefTypeNode && (type.element_type as IRefTypeNode).pointed_type is ICompiledTypeNode)
                    cust_bldr = new CustomAttributeBuilder(this.SetOfAttributeConstructor, new object[1] { elem_type });
                else
                    cust_bldr = new CustomAttributeBuilder(this.SetOfAttributeConstructor, new object[1] { elem_type.FullName });
                tb.SetCustomAttribute(cust_bldr);
            }
            else if (type.type_special_kind == type_special_kind.short_string)
            {
                int len = (type as IShortStringTypeNode).Length;
                CustomAttributeBuilder cust_bldr = new CustomAttributeBuilder(this.ShortStringAttributeConstructor, new object[1] { len });
                tb.SetCustomAttribute(cust_bldr);
            }
        }

        private void add_possible_type_attribute(FieldBuilder fb, ITypeNode type)
        {
            if (comp_opt.target != TargetType.Dll)
                return;
            if (type.type_special_kind == type_special_kind.typed_file)
            {
                Type elem_type = helper.GetTypeReference(type.element_type).tp;
                CustomAttributeBuilder cust_bldr = null;
                if (type.element_type is ICompiledTypeNode || type.element_type is IRefTypeNode && (type.element_type as IRefTypeNode).pointed_type is ICompiledTypeNode)
                    cust_bldr = new CustomAttributeBuilder(this.FileOfAttributeConstructor, new object[1] { elem_type });
                else
                    cust_bldr = new CustomAttributeBuilder(this.FileOfAttributeConstructor, new object[1] { elem_type.FullName });
                fb.SetCustomAttribute(cust_bldr);
            }
            else if (type.type_special_kind == type_special_kind.set_type)
            {
                Type elem_type = helper.GetTypeReference(type.element_type).tp;
                CustomAttributeBuilder cust_bldr = null;
                if (type.element_type is ICompiledTypeNode || type.element_type is IRefTypeNode && (type.element_type as IRefTypeNode).pointed_type is ICompiledTypeNode)
                    cust_bldr = new CustomAttributeBuilder(this.SetOfAttributeConstructor, new object[1] { elem_type });
                else
                    cust_bldr = new CustomAttributeBuilder(this.SetOfAttributeConstructor, new object[1] { elem_type.FullName });
                fb.SetCustomAttribute(cust_bldr);
            }
            else if (type.type_special_kind == type_special_kind.short_string)
            {
                int len = (type as IShortStringTypeNode).Length;
                CustomAttributeBuilder cust_bldr = new CustomAttributeBuilder(this.ShortStringAttributeConstructor, new object[1] { len });
                fb.SetCustomAttribute(cust_bldr);
            }
        }

        //перевод глобальной переменной (переменной модуля и основной программы)
        private void ConvertGlobalVariable(ICommonNamespaceVariableNode var)
        {
            //Console.WriteLine(is_in_unit);
            //if (is_in_unit && helper.IsUsed(var)==false) return;
            TypeInfo ti = helper.GetTypeReference(var.type);
            FieldBuilder fb = cur_type.DefineField(var.name, ti.tp, FieldAttributes.Public | FieldAttributes.Static);
            helper.AddGlobalVariable(var, fb);
            add_possible_type_attribute(fb, var.type);
            
            //если переменная имеет тип - массив, то создаем его
            if (ti.is_arr)
            {
                if (var.inital_value == null || var.inital_value is IArrayConstantNode)
                    CreateArrayGlobalVariable(il, fb, ti, var.inital_value as IArrayConstantNode, var.type);
                else if (var.inital_value is IArrayInitializer)
                    CreateArrayGlobalVariable(il, fb, ti, var.inital_value as IArrayInitializer, var.type);
            }
            else if (var.inital_value is IArrayConstantNode)
                CreateArrayGlobalVariable(il, fb, ti, var.inital_value as IArrayConstantNode, var.type);
            else if (var.inital_value is IArrayInitializer)
                CreateArrayGlobalVariable(il, fb, ti, var.inital_value as IArrayInitializer, var.type);
            else
                if (var.type.is_value_type || var.inital_value is IConstantNode && !(var.inital_value is INullConstantNode))
                    AddInitCall(il, fb, ti.init_meth, var.inital_value as IConstantNode);
            if (ti.is_set && var.type.type_special_kind == type_special_kind.set_type && var.inital_value == null)
            {
                il.Emit(OpCodes.Ldnull);
                il.Emit(OpCodes.Ldnull);
                il.Emit(OpCodes.Newobj, ti.def_cnstr);
                il.Emit(OpCodes.Stsfld, fb);
            }
            in_var_init = true;
            GenerateInitCode(var, il);
            in_var_init = false;
        }

        //private void GenerateInit

        private void GenerateInitCode(IVAriableDefinitionNode var, ILGenerator ilg)
        {
            ILGenerator ilgn = il;
            IExpressionNode expr = var.inital_value;
            il = ilg;
            if (expr != null && save_debug_info && comp_opt.GenerateDebugInfoForInitalValue)
            {
                if (expr.Location != null && !(expr is IConstantNode) && !(expr is IArrayInitializer) && !(expr is IRecordInitializer))
                    MarkSequencePoint(expr.Location);
            }
            if (expr != null && !(expr is IConstantNode) && !(expr is IArrayInitializer))
            {
                expr.visit(this);
            }
            il = ilgn;
        }

        public override void visit(SemanticTree.ICompiledPropertyNode value)
        {

        }

        public override void visit(SemanticTree.IBasicPropertyNode value)
        {

        }

        private bool is_get_set = false;
        private string cur_prop_name;

        //перевод свойства класса
        public override void visit(SemanticTree.ICommonPropertyNode value)
        {
            //получаем тип свойства
            Type ret_type = helper.GetTypeReference(value.property_type).tp;
            //получаем параметры свойства
            Type[] tt = GetParamTypes(value);
            PropertyAttributes pa = PropertyAttributes.None;
            if (value.common_comprehensive_type.default_property == value)
                pa = PropertyAttributes.HasDefault;

            PropertyBuilder pb = cur_type.DefineProperty(value.name, pa, ret_type, tt);
            helper.AddProperty(value, pb);
            //переводим заголовки методов доступа
            if (value.get_function != null)
            {
                is_get_set = true; cur_prop_name = "get_" + value.name;
                ConvertMethodHeader((ICommonMethodNode)value.get_function);
                is_get_set = false;
                MethodBuilder mb = helper.GetMethodBuilder(value.get_function);
                TypeInfo ti = helper.GetTypeReference(value.comperehensive_type);
                if (ti.is_arr) AddToCompilerGenerated(mb);
            }
            if (value.set_function != null)
            {
                is_get_set = true; cur_prop_name = "set_" + value.name;
                ConvertMethodHeader((ICommonMethodNode)value.set_function);
                is_get_set = false;
            }
            //привязываем эти методы к свойству
            if (value.get_function != null)
                pb.SetGetMethod(helper.GetMethodBuilder(value.get_function));
            if (value.set_function != null)
                pb.SetSetMethod(helper.GetMethodBuilder(value.set_function));
            MakeAttribute(value);
        }

        public override void visit(SemanticTree.IPropertyNode value)
        {

        }

        public override void visit(SemanticTree.IConstantDefinitionNode value)
        {

        }

        public override void visit(SemanticTree.ICompiledParameterNode value)
        {

        }

        public override void visit(SemanticTree.IBasicParameterNode value)
        {

        }

        public override void visit(SemanticTree.ICommonParameterNode value)
        {

        }

        public override void visit(SemanticTree.IParameterNode value)
        {

        }

        public override void visit(SemanticTree.ICompiledClassFieldNode value)
        {

        }

        //добавление методов копирования и проч. в массив
        private void AddSpecialMembersToArray(SemanticTree.ICommonClassFieldNode value, FieldAttributes fattr)
        {
            TypeInfo aux_ti = helper.GetTypeReference(value.comperehensive_type);
            if (aux_ti.clone_meth != null) return;
            aux_ti.is_arr = true;
            ISimpleArrayNode arr_type = value.type as ISimpleArrayNode;
            TypeInfo elem_ti = helper.GetTypeReference(arr_type.element_type);
            //переводим ISimpleArrayNode в .NET тип
            Type type = elem_ti.tp.MakeArrayType();
            //определяем поле для хранения ссылки на массив .NET
            FieldBuilder fb = cur_type.DefineField(value.name, type, fattr);
            helper.AddField(value, fb);
            TypeBuilder tb = (TypeBuilder)aux_ti.tp;
            //определяем конструктор, в котором создаем массив
            ConstructorBuilder cb = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, Type.EmptyTypes);
            TypeInfo ti = helper.AddType(value.type, tb);
            ti.tp = type;
            ti.arr_fld = fb;
            aux_ti.def_cnstr = cb;
            aux_ti.arr_fld = fb;
            aux_ti.arr_len = arr_type.length;
            ILGenerator cb_il = cb.GetILGenerator();
            //вызов констуктора по умолчанию
            cb_il.Emit(OpCodes.Ldarg_0);
            cb_il.Emit(OpCodes.Call, TypeFactory.ObjectType.GetConstructor(Type.EmptyTypes));

            //создание массива
            if (!elem_ti.is_arr)
            {
                //если массив одномерный
                cb_il.Emit(OpCodes.Ldarg_0);
                cb_il.Emit(OpCodes.Ldc_I4, arr_type.length);
                cb_il.Emit(OpCodes.Newarr, elem_ti.tp);
                cb_il.Emit(OpCodes.Stfld, fb);
                //вызовы методов $Init$ для каждого элемента массива
                if (elem_ti.is_set || elem_ti.is_typed_file || elem_ti.is_text_file || elem_ti.tp == TypeFactory.StringType || elem_ti.tp.IsValueType && elem_ti.init_meth != null)
                {
                    //cb_il.Emit(OpCodes.Ldarg_0);
                    LocalBuilder clb = cb_il.DeclareLocal(TypeFactory.Int32Type);
                    cb_il.Emit(OpCodes.Ldc_I4_0);
                    cb_il.Emit(OpCodes.Stloc, clb);
                    Label tlabel = cb_il.DefineLabel();
                    Label flabel = cb_il.DefineLabel();
                    cb_il.MarkLabel(tlabel);
                    cb_il.Emit(OpCodes.Ldloc, clb);
                    cb_il.Emit(OpCodes.Ldc_I4, arr_type.length);
                    cb_il.Emit(OpCodes.Bge, flabel);
                    cb_il.Emit(OpCodes.Ldarg_0);
                    cb_il.Emit(OpCodes.Ldfld, fb);
                    cb_il.Emit(OpCodes.Ldloc, clb);
                    if (!elem_ti.is_set && !elem_ti.is_typed_file && !elem_ti.is_text_file)
                    {
                        if (elem_ti.tp != TypeFactory.StringType)
                        {
                            cb_il.Emit(OpCodes.Ldelema, elem_ti.tp);
                            cb_il.Emit(OpCodes.Call, elem_ti.init_meth);
                        }
                        else
                        {
                            cb_il.Emit(OpCodes.Ldstr, "");
                            cb_il.Emit(OpCodes.Stelem_Ref);
                        }
                    }
                    else if (elem_ti.is_set)
                    {
                        IConstantNode cn1 = (arr_type.element_type as ICommonTypeNode).lower_value;
                        IConstantNode cn2 = (arr_type.element_type as ICommonTypeNode).upper_value;
                        if (cn1 != null && cn2 != null)
                        {
                            ILGenerator tmp_il = il;
                            il = cb_il;
                            cn1.visit(this);
                            il.Emit(OpCodes.Box, helper.GetTypeReference(cn1.type).tp);
                            cn2.visit(this);
                            il.Emit(OpCodes.Box, helper.GetTypeReference(cn2.type).tp);
                            il = tmp_il;
                        }
                        else
                        {
                            cb_il.Emit(OpCodes.Ldnull);
                            cb_il.Emit(OpCodes.Ldnull);
                        }
                        cb_il.Emit(OpCodes.Newobj, elem_ti.def_cnstr);
                        cb_il.Emit(OpCodes.Stelem_Ref);
                    }
                    else if (elem_ti.is_typed_file)
                    {
                        NETGeneratorTools.PushTypeOf(cb_il, helper.GetTypeReference(arr_type.element_type.element_type).tp);
                        cb_il.Emit(OpCodes.Newobj, elem_ti.def_cnstr);
                        cb_il.Emit(OpCodes.Stelem_Ref);
                    }
                    else if (elem_ti.is_text_file)
                    {
                        cb_il.Emit(OpCodes.Newobj, elem_ti.def_cnstr);
                        cb_il.Emit(OpCodes.Stelem_Ref);
                    }
                    cb_il.Emit(OpCodes.Ldloc, clb);
                    cb_il.Emit(OpCodes.Ldc_I4_1);
                    cb_il.Emit(OpCodes.Add);
                    cb_il.Emit(OpCodes.Stloc, clb);
                    cb_il.Emit(OpCodes.Br, tlabel);
                    cb_il.MarkLabel(flabel);
                }
                cb_il.Emit(OpCodes.Ret);
            }
            else
            {
                //если массив многомерный, то в цикле по создаем
                //элементы-массивы
                LocalBuilder clb = cb_il.DeclareLocal(TypeFactory.Int32Type);
                cb_il.Emit(OpCodes.Ldc_I4_0);
                cb_il.Emit(OpCodes.Stloc, clb);
                cb_il.Emit(OpCodes.Ldarg_0);
                cb_il.Emit(OpCodes.Ldc_I4, arr_type.length);
                cb_il.Emit(OpCodes.Newarr, elem_ti.tp);
                cb_il.Emit(OpCodes.Stfld, fb);
                Label tlabel = cb_il.DefineLabel();
                Label flabel = cb_il.DefineLabel();
                cb_il.MarkLabel(tlabel);
                cb_il.Emit(OpCodes.Ldloc, clb);
                cb_il.Emit(OpCodes.Ldc_I4, arr_type.length);
                cb_il.Emit(OpCodes.Bge, flabel);
                cb_il.Emit(OpCodes.Ldarg_0);
                cb_il.Emit(OpCodes.Ldfld, fb);
                cb_il.Emit(OpCodes.Ldloc, clb);
                cb_il.Emit(OpCodes.Newobj, elem_ti.def_cnstr);
                cb_il.Emit(OpCodes.Stelem_Ref);
                cb_il.Emit(OpCodes.Ldloc, clb);
                cb_il.Emit(OpCodes.Ldc_I4_1);
                cb_il.Emit(OpCodes.Add);
                cb_il.Emit(OpCodes.Stloc, clb);
                cb_il.Emit(OpCodes.Br, tlabel);
                cb_il.MarkLabel(flabel);
                cb_il.Emit(OpCodes.Ret);
            }
            //определяем метод копирование массива
            MethodBuilder clone_mb = tb.DefineMethod("Clone", MethodAttributes.Public, tb, Type.EmptyTypes);
            ILGenerator clone_il = clone_mb.GetILGenerator();
            //если массив одномерный
            if (elem_ti.clone_meth == null)
            {
                MarkSequencePoint(clone_il, 0xFFFFFF, 0, 0xFFFFFF, 0);
                LocalBuilder lb = clone_il.DeclareLocal(tb);
                clone_il.Emit(OpCodes.Newobj, cb);
                clone_il.Emit(OpCodes.Stloc, lb);

                clone_il.Emit(OpCodes.Ldarg_0);
                clone_il.Emit(OpCodes.Ldfld, fb);
                clone_il.Emit(OpCodes.Ldloc, lb);
                clone_il.Emit(OpCodes.Ldfld, fb);
                clone_il.Emit(OpCodes.Ldc_I4, arr_type.length);
                clone_il.Emit(OpCodes.Call, TypeFactory.ArrayCopyMethod);

                clone_il.Emit(OpCodes.Ldloc, lb);
                clone_il.Emit(OpCodes.Ret);
            }
            else
            {
                MarkSequencePoint(clone_il, 0xFFFFFF, 0, 0xFFFFFF, 0);
                //если массив многомерный, то в цикле копируем
                LocalBuilder lb = clone_il.DeclareLocal(tb);
                clone_il.Emit(OpCodes.Newobj, cb);
                clone_il.Emit(OpCodes.Stloc, lb);

                LocalBuilder clb = clone_il.DeclareLocal(TypeFactory.Int32Type);
                clone_il.Emit(OpCodes.Ldc_I4_0);
                clone_il.Emit(OpCodes.Stloc, clb);
                Label tlabel = clone_il.DefineLabel();
                Label flabel = clone_il.DefineLabel();
                clone_il.MarkLabel(tlabel);
                clone_il.Emit(OpCodes.Ldloc, clb);
                clone_il.Emit(OpCodes.Ldc_I4, arr_type.length);
                clone_il.Emit(OpCodes.Bge, flabel);
                clone_il.Emit(OpCodes.Ldloc, lb);
                clone_il.Emit(OpCodes.Ldfld, fb);
                clone_il.Emit(OpCodes.Ldloc, clb);
                if (elem_ti.tp.IsValueType)
                    clone_il.Emit(OpCodes.Ldelema, elem_ti.tp);
                clone_il.Emit(OpCodes.Ldarg_0);
                clone_il.Emit(OpCodes.Ldfld, fb);
                clone_il.Emit(OpCodes.Ldloc, clb);
                if (!elem_ti.tp.IsValueType)
                    clone_il.Emit(OpCodes.Ldelem_Ref);
                else
                    clone_il.Emit(OpCodes.Ldelema, elem_ti.tp);
                clone_il.Emit(OpCodes.Call, elem_ti.clone_meth);
                if (!elem_ti.tp.IsValueType)
                    clone_il.Emit(OpCodes.Stelem_Ref);
                else
                    clone_il.Emit(OpCodes.Stobj, elem_ti.tp);
                clone_il.Emit(OpCodes.Ldloc, clb);
                clone_il.Emit(OpCodes.Ldc_I4_1);
                clone_il.Emit(OpCodes.Add);
                clone_il.Emit(OpCodes.Stloc, clb);
                clone_il.Emit(OpCodes.Br, tlabel);
                clone_il.MarkLabel(flabel);
                clone_il.Emit(OpCodes.Ldloc, lb);
                clone_il.Emit(OpCodes.Ret);
            }
            //привязываем метод копирования
            //он нужен будет при передаче массива в кач-ве параметра по значению
            aux_ti.clone_meth = clone_mb;
        }

        private FieldAttributes ConvertFALToFieldAttributes(field_access_level fal)
        {
            switch (fal)
            {
                case field_access_level.fal_public: return FieldAttributes.Public;
                case field_access_level.fal_internal: return FieldAttributes.Assembly;
                case field_access_level.fal_protected: return FieldAttributes.FamORAssem;
                case field_access_level.fal_private: return FieldAttributes.Assembly;
            }
            return FieldAttributes.Assembly;
        }

        private FieldAttributes AddPSToFieldAttributes(polymorphic_state ps, FieldAttributes fa)
        {
            switch (ps)
            {
                case polymorphic_state.ps_static: fa |= FieldAttributes.Static; break;
            }
            return fa;
        }

        private MethodAttributes ConvertFALToMethodAttributes(field_access_level fal)
        {
            switch (fal)
            {
                case field_access_level.fal_public: return MethodAttributes.Public;
                case field_access_level.fal_internal: return (comp_opt.target == TargetType.Dll && pabc_rtl_converted) ? MethodAttributes.Public : MethodAttributes.Assembly;
                case field_access_level.fal_protected: return MethodAttributes.FamORAssem;
                case field_access_level.fal_private: return MethodAttributes.Assembly;
            }
            return MethodAttributes.Assembly;
        }


        //перевод поля класса
        public override void visit(SemanticTree.ICommonClassFieldNode value)
        {
            //if (is_in_unit && helper.IsUsed(value)==false) return;
            FieldAttributes fattr = ConvertFALToFieldAttributes(value.field_access_level);
            fattr = AddPSToFieldAttributes(value.polymorphic_state, fattr);
            Type type = null;
            TypeInfo ti = helper.GetTypeReference(value.type);
            //далее идет байда, связанная с массивами, мы здесь добавляем методы копирования и проч.
            if (ti == null)
            {
                AddSpecialMembersToArray(value, fattr);
            }
            else
            {
                //иначе все хорошо
                type = ti.tp;
                FieldBuilder fb = cur_type.DefineField(value.name, type, fattr);
                helper.AddField(value, fb);
                MakeAttribute(value);
                if (cur_type.IsValueType && cur_ti.clone_meth != null)
                {
                    NETGeneratorTools.CloneField(cur_ti.clone_meth as MethodBuilder, fb, ti);
                    NETGeneratorTools.AssignField(cur_ti.assign_meth as MethodBuilder, fb, ti);
                    switch (value.type.type_special_kind)
                    {
                        case type_special_kind.array_wrapper:
                        case type_special_kind.set_type:
                        case type_special_kind.short_string:
                        case type_special_kind.typed_file:
                            NETGeneratorTools.FixField(cur_ti.fix_meth, fb, ti);
                            break;
                    }
                }
            }
        }

        internal void GenerateInitCodeForFields(SemanticTree.ICommonTypeNode ctn)
        {
            TypeInfo ti = helper.GetTypeReference(ctn);
            //(ssyy) 16.05.08 добавил проверку, это надо для ф-ций, зависящих от generic-параметров.
            if (ti == null) return;
            if (!ctn.IsInterface && ti.init_meth != null)
            {
                foreach (SemanticTree.ICommonClassFieldNode ccf in ctn.fields)
                    if (ccf.polymorphic_state != polymorphic_state.ps_static)
                        GenerateInitCodeForField(ccf);
                    else
                        GenerateInitCodeForStaticField(ccf);
            }
        }

        internal void GenerateRetForInitMeth(SemanticTree.ICommonTypeNode ctn)
        {
            TypeInfo ti = helper.GetTypeReference(ctn);
            if (ti == null)
            {
                return;
            }
            if (!ctn.IsInterface && ti.init_meth != null)
                (ti.init_meth as MethodBuilder).GetILGenerator().Emit(OpCodes.Ret);
        }

        internal void GenerateInitCodeForStaticField(SemanticTree.ICommonClassFieldNode value)
        {
            TypeInfo ti = helper.GetTypeReference(value.type), cur_ti = helper.GetTypeReference(value.comperehensive_type);
            FieldBuilder fb = helper.GetField(value).fi as FieldBuilder;
            if (value.type.is_generic_parameter && value.inital_value == null)
            {
                CreateRuntimeInitCodeWithCheck((cur_ti.init_meth as MethodBuilder).GetILGenerator(), fb, value.type as ICommonTypeNode);
            }
            if (ti.is_arr)
            {
                if (value.inital_value == null || value.inital_value is IArrayConstantNode)
                    CreateArrayForClassField(cur_ti.static_cnstr.GetILGenerator(), fb, ti, value.inital_value as IArrayConstantNode, value.type);
                else if (value.inital_value is IArrayInitializer)
                    CreateArrayForClassField(cur_ti.static_cnstr.GetILGenerator(), fb, ti, value.inital_value as IArrayInitializer, value.type);
            }
            else if (value.inital_value is IArrayConstantNode)
                CreateArrayForClassField(cur_ti.static_cnstr.GetILGenerator(), fb, ti, value.inital_value as IArrayConstantNode, value.type);
            else if (value.inital_value is IArrayInitializer)
                CreateArrayForClassField(cur_ti.static_cnstr.GetILGenerator(), fb, ti, value.inital_value as IArrayInitializer, value.type);
            else
                if (value.type.is_value_type || value.inital_value is IConstantNode && !(value.inital_value is INullConstantNode))
                    AddInitCall(fb, cur_ti.static_cnstr.GetILGenerator(), ti.init_meth, ti.def_cnstr, value.inital_value as IConstantNode);
            in_var_init = true;
            GenerateInitCode(value, cur_ti.static_cnstr.GetILGenerator());
            in_var_init = false;
        }

        internal void GenerateInitCodeForField(SemanticTree.ICommonClassFieldNode value)
        {
            TypeInfo ti = helper.GetTypeReference(value.type), cur_ti = helper.GetTypeReference(value.comperehensive_type);
            FieldBuilder fb = helper.GetField(value).fi as FieldBuilder;
            if (value.type.is_generic_parameter && value.inital_value == null)
            {
                CreateRuntimeInitCodeWithCheck((cur_ti.init_meth as MethodBuilder).GetILGenerator(), fb, value.type as ICommonTypeNode);
            }
            if (ti.is_arr)
            {
                if (value.inital_value == null || value.inital_value is IArrayConstantNode)
                    CreateArrayForClassField((cur_ti.init_meth as MethodBuilder).GetILGenerator(), fb, ti, value.inital_value as IArrayConstantNode, value.type);
                else if (value.inital_value is IArrayInitializer)
                    CreateArrayForClassField((cur_ti.init_meth as MethodBuilder).GetILGenerator(), fb, ti, value.inital_value as IArrayInitializer, value.type);
            }
            else if (value.inital_value is IArrayConstantNode)
                CreateArrayForClassField((cur_ti.init_meth as MethodBuilder).GetILGenerator(), fb, ti, value.inital_value as IArrayConstantNode, value.type);
            else if (value.inital_value is IArrayInitializer)
                CreateArrayForClassField((cur_ti.init_meth as MethodBuilder).GetILGenerator(), fb, ti, value.inital_value as IArrayInitializer, value.type);
            else
                if (value.type.is_value_type || value.inital_value is IConstantNode && !(value.inital_value is INullConstantNode))
                    AddInitCall(fb, (cur_ti.init_meth as MethodBuilder).GetILGenerator(), ti.init_meth, ti.def_cnstr, value.inital_value as IConstantNode);
            in_var_init = true;
            GenerateInitCode(value, (cur_ti.init_meth as MethodBuilder).GetILGenerator());
            in_var_init = false;
        }

        public override void visit(SemanticTree.ICommonNamespaceVariableNode value)
        {

        }

        public override void visit(SemanticTree.ILocalVariableNode value)
        {

        }

        public override void visit(SemanticTree.IVAriableDefinitionNode value)
        {

        }

        //перевод символьной константы
        //команда ldc_i4_s
        //is_dot_expr - признак того, что после этого выражения
        //идет точка (например 'a'.ToString)
        public override void visit(SemanticTree.ICharConstantNode value)
        {
            NETGeneratorTools.LdcIntConst(il, value.constant_value);

            if (is_dot_expr == true)
            {
                //определяем временную переменную
                LocalBuilder lb = il.DeclareLocal(TypeFactory.CharType);
                //сохраняем в переменной симв. константу
                il.Emit(OpCodes.Stloc, lb);
                //кладем адрес этой переменной
                il.Emit(OpCodes.Ldloca, lb);
            }
        }

        public override void visit(SemanticTree.IFloatConstantNode value)
        {
            il.Emit(OpCodes.Ldc_R4, value.constant_value);
            if (is_dot_expr)
                NETGeneratorTools.CreateLocalAndLdloca(il, TypeFactory.SingleType);
        }

        public override void visit(SemanticTree.IDoubleConstantNode value)
        {
            il.Emit(OpCodes.Ldc_R8, value.constant_value);
            if (is_dot_expr)
                NETGeneratorTools.CreateLocalAndLdloca(il, TypeFactory.DoubleType);
        }

        //перевод целой константы
        //команда ldc_i4
        private void PushIntConst(int e)
        {
            PushIntConst(il, e);
        }

        private void PushIntConst(ILGenerator il, int e)
        {
            NETGeneratorTools.LdcIntConst(il, e);
            if (is_dot_expr == true)
            {
                LocalBuilder lb = null;
                lb = il.DeclareLocal(TypeFactory.Int32Type);
                il.Emit(OpCodes.Stloc, lb);
                il.Emit(OpCodes.Ldloca, lb);
            }
        }

        //ivan
        private void PushFloatConst(float value)
        {
            il.Emit(OpCodes.Ldc_R4, value);
        }

        private void PushDoubleConst(double value)
        {
            il.Emit(OpCodes.Ldc_R8, value);
        }

        private void PushCharConst(char value)
        {
            NETGeneratorTools.LdcIntConst(il, value);
        }

        private void PushStringConst(string value)
        {
            il.Emit(OpCodes.Ldstr, value);
        }

        private void PushByteConst(byte value)
        {
            NETGeneratorTools.LdcIntConst(il, value);
        }

        private void PushLongConst(long value)
        {
            il.Emit(OpCodes.Ldc_I8, (long)value);
        }

        private void PushShortConst(short value)
        {
            NETGeneratorTools.LdcIntConst(il, value);
        }

        private void PushUShortConst(ushort value)
        {
            NETGeneratorTools.LdcIntConst(il, value);
        }

        private void PushUIntConst(uint value)
        {
            NETGeneratorTools.LdcIntConst(il, (int)value);
        }

        private void PushULongConst(ulong value)
        {
            long l = (long)(value & 0x7FFFFFFFFFFFFFFF);
            if ((value & 0x8000000000000000) != 0)
            {
                long l2 = 0x4000000000000000 << 1;
                l |= l2;
            }
            il.Emit(OpCodes.Ldc_I8, l);
        }

        private void PushSByteConst(sbyte value)
        {
            NETGeneratorTools.LdcIntConst(il, value);
        }

        private void PushBoolConst(bool value)
        {
            if (value)
                il.Emit(OpCodes.Ldc_I4_1);
            else
                il.Emit(OpCodes.Ldc_I4_0);
        }
        //\ivan
        public override void visit(SemanticTree.IIntConstantNode value)
        {
            PushIntConst(value.constant_value);
        }

        //перевод long константы
        //команда ldc_i8
        public override void visit(SemanticTree.ILongConstantNode value)
        {
            il.Emit(OpCodes.Ldc_I8, (long)value.constant_value);
            if (is_dot_expr == true)
            {
                LocalBuilder lb = null;
                lb = il.DeclareLocal(typeof(long));
                il.Emit(OpCodes.Stloc, lb);
                il.Emit(OpCodes.Ldloca, lb);
            }
        }
        //перевод byte константы
        //команда ldc_i4_S
        public override void visit(SemanticTree.IByteConstantNode value)
        {
            NETGeneratorTools.LdcIntConst(il, value.constant_value);
            if (is_dot_expr == true)
            {
                LocalBuilder lb = null;
                lb = il.DeclareLocal(TypeFactory.ByteType);
                il.Emit(OpCodes.Stloc, lb);
                il.Emit(OpCodes.Ldloca, lb);
            }
        }

        public override void visit(SemanticTree.IShortConstantNode value)
        {
            NETGeneratorTools.LdcIntConst(il, value.constant_value);
            if (is_dot_expr == true)
            {
                LocalBuilder lb = null;
                lb = il.DeclareLocal(typeof(short));
                il.Emit(OpCodes.Stloc, lb);
                il.Emit(OpCodes.Ldloca, lb);
            }
        }

        public override void visit(SemanticTree.IUShortConstantNode value)
        {
            NETGeneratorTools.LdcIntConst(il, value.constant_value);
            if (is_dot_expr == true)
            {
                LocalBuilder lb = null;
                lb = il.DeclareLocal(typeof(ushort));
                il.Emit(OpCodes.Stloc, lb);
                il.Emit(OpCodes.Ldloca, lb);
            }
        }

        public override void visit(SemanticTree.IUIntConstantNode value)
        {
            NETGeneratorTools.LdcIntConst(il, (int)value.constant_value);
            if (is_dot_expr == true)
            {
                LocalBuilder lb = null;
                lb = il.DeclareLocal(typeof(uint));
                il.Emit(OpCodes.Stloc, lb);
                il.Emit(OpCodes.Ldloca, lb);
            }
        }

        public override void visit(SemanticTree.IULongConstantNode value)
        {
            long l = (long)(value.constant_value & 0x7FFFFFFFFFFFFFFF);
            if ((value.constant_value & 0x8000000000000000) != 0)
            {
                long l2 = 0x4000000000000000 << 1;
                l |= l2;
            }
            il.Emit(OpCodes.Ldc_I8, l);
            if (is_dot_expr == true)
            {
                LocalBuilder lb = null;
                lb = il.DeclareLocal(typeof(ulong));
                il.Emit(OpCodes.Stloc, lb);
                il.Emit(OpCodes.Ldloca, lb);
            }
        }

        public override void visit(SemanticTree.ISByteConstantNode value)
        {
            //il.Emit(OpCodes.Ldc_I4, (int)value.constant_value);
            NETGeneratorTools.LdcIntConst(il, value.constant_value);
            if (is_dot_expr == true)
            {
                LocalBuilder lb = null;
                lb = il.DeclareLocal(typeof(sbyte));
                il.Emit(OpCodes.Stloc, lb);
                il.Emit(OpCodes.Ldloca, lb);
            }
        }

        //перевод булевской константы
        //команда ldc_i4_0/1
        public override void visit(SemanticTree.IBoolConstantNode value)
        {
            if (value.constant_value == true)
                il.Emit(OpCodes.Ldc_I4_1);
            else
                il.Emit(OpCodes.Ldc_I4_0);
            if (is_dot_expr == true)
            {
                LocalBuilder lb = il.DeclareLocal(typeof(bool));
                il.Emit(OpCodes.Stloc, lb);
                il.Emit(OpCodes.Ldloca, lb);
            }
        }

        public override void visit(SemanticTree.IConstantNode value)
        {

        }

        private void PushParameter(int pos)
        {
            switch (pos)
            {
                case 0: il.Emit(OpCodes.Ldarg_0); break;
                case 1: il.Emit(OpCodes.Ldarg_1); break;
                case 2: il.Emit(OpCodes.Ldarg_2); break;
                case 3: il.Emit(OpCodes.Ldarg_3); break;
                default:
                    if (pos <= 255)
                        il.Emit(OpCodes.Ldarg_S, (byte)pos);
                    else
                        //здесь надо быть внимательнее
                        il.Emit(OpCodes.Ldarg, (short)pos);
                    break;
            }
        }

        private void PushParameterAddress(int pos)
        {
            if (pos <= byte.MaxValue)
                il.Emit(OpCodes.Ldarga_S, (byte)pos);
            else
                il.Emit(OpCodes.Ldarga, pos);
        }

        //перевод ссылки на параметр
        public override void visit(SemanticTree.ICommonParameterReferenceNode value)
        {
            bool must_push_addr = false;//должен ли упаковываться, но это если после идет точка
            if (is_dot_expr == true)//если после идет точка
            {
                if (value.type.is_value_type || value.type.is_generic_parameter)
                    must_push_addr = true;
            }
            ParamInfo pi = helper.GetParameter(value.parameter);
            if (pi.kind == ParamKind.pkNone)
            {
                //этот параметр яв-ся локальным
                ParameterBuilder pb = pi.pb;
                //это хрень с позициями меня достает
                byte pos = (byte)(pb.Position - 1);
                if (is_constructor || !cur_meth.IsStatic)
                    pos = (byte)pb.Position;
                else
                    pos = (byte)(pb.Position - 1);
                if (value.parameter.parameter_type == parameter_type.value)
                {
                    //напомним, что is_addr - передается ли он в качестве факт. параметра по ссылке
                    if (is_addr == false)
                    {
                        if (must_push_addr)
                        {
                            //здесь кладем адрес параметра
                            PushParameterAddress(pos);
                        }
                        else
                            PushParameter(pos);
                    }
                    else
                        PushParameterAddress(pos);
                }
                else
                {
                    //это var-параметр
                    PushParameter(pos);
                    if (is_addr == false && !must_push_addr)
                    {
                        TypeInfo ti = helper.GetTypeReference(value.parameter.type);
                        NETGeneratorTools.PushParameterDereference(il, ti.tp);
                    }
                }
            }
            else
            {
                //это параметр нелокальный
                FieldBuilder fb = pi.fb;
                MethInfo cur_mi = smi.Peek();
                int dist = smi.Peek().num_scope - pi.meth.num_scope;
                //проходимся по цепочке записей активации
                il.Emit(OpCodes.Ldloc, cur_mi.frame);
                for (int i = 0; i < dist; i++)
                {
                    il.Emit(OpCodes.Ldfld, cur_mi.disp.parent);
                    cur_mi = cur_mi.up_meth;
                }
                if (value.parameter.parameter_type == parameter_type.value)
                {
                    if (!is_addr)
                    {
                        if (!must_push_addr) il.Emit(OpCodes.Ldfld, fb);
                        else il.Emit(OpCodes.Ldflda, fb);
                    }
                    else il.Emit(OpCodes.Ldflda, fb);
                }
                else
                {
                    il.Emit(OpCodes.Ldfld, fb);
                    if (is_addr == false && must_push_addr == false)
                    {
                        TypeInfo ti = helper.GetTypeReference(value.parameter.type);
                        NETGeneratorTools.PushParameterDereference(il, ti.tp);
                    }
                }
            }
        }

        //доступ к статическому откомпилированному типу
        public override void visit(SemanticTree.IStaticCompiledFieldReferenceNode value)
        {
            //если у поля нет постоянное значение
            if (!value.static_field.compiled_field.IsLiteral)
            {
                if (is_addr == false)
                {
                    if (is_dot_expr == true && value.static_field.compiled_field.FieldType.IsValueType)
                    {
                        il.Emit(OpCodes.Ldsflda, value.static_field.compiled_field);
                    }
                    else il.Emit(OpCodes.Ldsfld, value.static_field.compiled_field);
                }
                else il.Emit(OpCodes.Ldsflda, value.static_field.compiled_field);

            }
            else
            {
                //иначе кладем константу
                NETGeneratorTools.PushLdc(il, value.static_field.compiled_field.FieldType, value.static_field.compiled_field.GetRawConstantValue());
                if (is_dot_expr)
                {
                    //нужно упаковать
                    il.Emit(OpCodes.Box, value.static_field.compiled_field.FieldType);
                }
            }
        }

        //доступ к откомпилированному нестатическому полю
        public override void visit(SemanticTree.ICompiledFieldReferenceNode value)
        {
            bool tmp_dot = is_dot_expr;
            if (!tmp_dot)
                is_dot_expr = true;
            if (value.field.compiled_field.IsLiteral == false)
            {
                value.obj.visit(this);
                if (!is_addr)
                {
                    if (tmp_dot && value.field.compiled_field.FieldType.IsValueType)
                    {
                        il.Emit(OpCodes.Ldflda, value.field.compiled_field);
                    }
                    else il.Emit(OpCodes.Ldfld, value.field.compiled_field);
                }
                else il.Emit(OpCodes.Ldflda, value.field.compiled_field);
            }
            else
            {
                NETGeneratorTools.PushLdc(il, value.field.compiled_field.FieldType, value.field.compiled_field.GetRawConstantValue());
                if (tmp_dot)
                {
                    il.Emit(OpCodes.Box, value.field.compiled_field.FieldType);
                }
            }
            if (tmp_dot == false)
            {
                is_dot_expr = false;
            }
        }

        public override void visit(SemanticTree.IStaticCommonClassFieldReferenceNode value)
        {
            bool tmp_dot = is_dot_expr;
            FldInfo fi_info = helper.GetField(value.static_field);
            FieldInfo fi = fi_info.fi;
            if (!is_addr)
            {
                if (tmp_dot)
                {
                    if (fi_info.field_type.IsValueType || fi_info.field_type.IsGenericParameter)
                    {
                        il.Emit(OpCodes.Ldsflda, fi);
                    }
                    else
                        il.Emit(OpCodes.Ldsfld, fi);
                }
                else
                    il.Emit(OpCodes.Ldsfld, fi);
            }
            else
                il.Emit(OpCodes.Ldsflda, fi);
        }

        public override void visit(SemanticTree.ICommonClassFieldReferenceNode value)
        {
            bool tmp_dot = is_dot_expr;
            if (!tmp_dot)
                is_dot_expr = true;
            bool temp_is_addr = is_addr;
            is_addr = false;
            //is_dot_expr = false;
            value.obj.visit(this);
            is_addr = temp_is_addr;
            FldInfo fi_info = helper.GetField(value.field);
            FieldInfo fi = fi_info.fi;
            if (!is_addr)
            {
                if (tmp_dot)
                {
                    if (fi_info.field_type.IsValueType || fi_info.field_type.IsGenericParameter)
                    {
                        il.Emit(OpCodes.Ldflda, fi);
                    }
                    else
                        il.Emit(OpCodes.Ldfld, fi);
                }
                else
                    il.Emit(OpCodes.Ldfld, fi);
            }
            else
                il.Emit(OpCodes.Ldflda, fi);

            if (!tmp_dot)
            {
                is_dot_expr = false;
            }
        }

        public override void visit(SemanticTree.INamespaceVariableReferenceNode value)
        {
            VarInfo vi = helper.GetVariable(value.variable);
            if (vi == null)
            {
                ConvertGlobalVariable(value.variable);
                vi = helper.GetVariable(value.variable);
            }
            FieldBuilder fb = vi.fb;
            if (is_addr == false)
            {
                if (is_dot_expr == true)
                {
                    if (fb.FieldType.IsValueType == true)
                    {
                        il.Emit(OpCodes.Ldsflda, fb);
                    }
                    else
                        il.Emit(OpCodes.Ldsfld, fb);
                }
                else
                    il.Emit(OpCodes.Ldsfld, fb);
            }
            else il.Emit(OpCodes.Ldsflda, fb);

        }

        //чтобы перевести переменную, нужно до фига проверок
        public override void visit(SemanticTree.ILocalVariableReferenceNode value)
        {
            VarInfo vi = helper.GetVariable(value.variable);
            if (vi == null)
            {
                ConvertLocalVariable(value.variable, false, 0, 0);
                vi = helper.GetVariable(value.variable);
            }
            if (vi.kind == VarKind.vkLocal)//если локальная
            {
                LocalBuilder lb = vi.lb;
                if (!is_addr)//если это факт. var-параметр
                {
                    if (is_dot_expr) //если после перем. в выражении стоит точка
                    {
                        if (lb.LocalType.IsGenericParameter)
                        {
                            il.Emit(OpCodes.Ldloc, lb);
                            il.Emit(OpCodes.Box, lb.LocalType);
                        }
                        else
                            if (lb.LocalType.IsValueType)
                        {
                            il.Emit(OpCodes.Ldloca, lb);//если перем. размерного типа кладем ее адрес
                        }
                        else
                        {

                            il.Emit(OpCodes.Ldloc, lb);
                        }
                    }
                    else il.Emit(OpCodes.Ldloc, lb);
                }
                else il.Emit(OpCodes.Ldloca, lb);//в этом случае перем. - фактический var-параметр процедуры
            }
            else if (vi.kind == VarKind.vkNonLocal) //переменная нелокальная
            {
                FieldBuilder fb = vi.fb; //значит, это поля класса-обертки
                MethInfo cur_mi = smi.Peek();
                int dist = smi.Peek().num_scope - vi.meth.num_scope;//получаем разность глубин вложенности
                il.Emit(OpCodes.Ldloc, cur_mi.frame); //кладем объект класса-обертки
                for (int i = 0; i < dist; i++)
                {
                    il.Emit(OpCodes.Ldfld, cur_mi.disp.parent); //проходимся по цепочке
                    cur_mi = cur_mi.up_meth;
                }
                if (is_addr == false) //здесь уже ясно
                {
                    if (is_dot_expr == true) //в выражении после стоит точка
                    {
                        if (fb.FieldType.IsValueType == true)
                        {
                            il.Emit(OpCodes.Ldflda, fb);//для размерного значения кладем адрес
                        }
                        else il.Emit(OpCodes.Ldfld, fb);
                    }
                    else
                        il.Emit(OpCodes.Ldfld, fb);
                }
                else il.Emit(OpCodes.Ldflda, fb);
            }
        }

        public override void visit(SemanticTree.IAddressedExpressionNode value)
        {

        }

        public override void visit(SemanticTree.IProgramNode value)
        {

        }

        public override void visit(SemanticTree.IDllNode value)
        {

        }

        public override void visit(SemanticTree.ICompiledNamespaceNode value)
        {

        }

        public override void visit(SemanticTree.ICommonNamespaceNode value)
        {

        }

        public override void visit(SemanticTree.INamespaceNode value)
        {

        }

        private void ConvertStatementsListWithoutFirstStatement(SemanticTree.IStatementsListNode value)
        {
            if (save_debug_info)
            {
                if (gen_left_brackets)
                    MarkSequencePoint(value.LeftLogicalBracketLocation);
                else
                    il.MarkSequencePoint(doc, 0xFeeFee, 0xFeeFee, 0xFeeFee, 0xFeeFee);
                //il.MarkSequencePoint(doc,0xFFFFFF,0xFFFFFF,0xFFFFFF,0xFFFFFF);
                il.Emit(OpCodes.Nop);
            }
            ILocalBlockVariableNode[] localVariables = value.LocalVariables;
            for (int i = 0; i < localVariables.Length; i++)
            {
                ConvertLocalVariable(localVariables[i], true, value.Location.begin_line_num, value.Location.end_line_num);
            }
            IStatementNode[] statements = value.statements;
            if (statements.Length == 0)
            {
                if (save_debug_info)
                    il.Emit(OpCodes.Nop);
                return;
            }

            for (int i = 1; i < statements.Length - 1; i++)
            {
                ConvertStatement(statements[i]);
            }

            if (save_debug_info)
                if (statements[statements.Length - 1] is SemanticTree.IReturnNode)
                    //если return не имеет location то метим точку на месте закрывающей логической скобки
                    if (statements[statements.Length - 1].Location == null)
                        MarkSequencePoint(value.RightLogicalBracketLocation);

            ConvertStatement(statements[statements.Length - 1]);

            //TODO: переделать. сдель функцию которая ложет ret и MarkSequencePoint
            if (save_debug_info && !(statements[statements.Length - 1] is SemanticTree.IReturnNode))
            {
                //если почледний оператор не Return то пометить закрывающуюю логическую скобку
                if (gen_right_brackets)
                    MarkSequencePoint(value.RightLogicalBracketLocation);
                //il.Emit(OpCodes.Nop);
            }
        }

        public override void visit(SemanticTree.IStatementsListNode value)
        {
            IStatementNode[] statements = value.statements;
            if (save_debug_info)
            {
                if (gen_left_brackets || value.LeftLogicalBracketLocation == null)
                    MarkSequencePoint(value.LeftLogicalBracketLocation);
                else
                    il.MarkSequencePoint(doc, 0xFeeFee, 0xFeeFee, 0xFeeFee, 0xFeeFee);
                il.Emit(OpCodes.Nop);
            }
            
            ILocalBlockVariableNode[] localVariables = value.LocalVariables;
            for (int i = 0; i < localVariables.Length; i++)
            {
                if (value.Location != null && value.LeftLogicalBracketLocation == null && statements.Length > 0 && statements[statements.Length - 1].Location != null &&
                    statements[statements.Length - 1].Location.begin_line_num == value.Location.begin_line_num)
                    ConvertLocalVariable(localVariables[i], true, statements[statements.Length - 1].Location.begin_line_num, statements[statements.Length - 1].Location.end_line_num);
                else if (value.Location != null)
                    ConvertLocalVariable(localVariables[i], true, value.Location.begin_line_num, value.Location.end_line_num);
                else
                    ConvertLocalVariable(localVariables[i], false, 0, 0);
            }

            if (statements.Length == 0)
            {
                if (save_debug_info)
                    il.Emit(OpCodes.Nop);
                return;
            }

            for (int i = 0; i < statements.Length - 1; i++)
            {
                ConvertStatement(statements[i]);
            }

            if (save_debug_info)
                if (statements[statements.Length - 1] is SemanticTree.IReturnNode)
                    //если return не имеет location то метим точку на месте закрывающей логической скобки
                    if (statements[statements.Length - 1].Location == null)
                        MarkSequencePoint(value.RightLogicalBracketLocation);

            ConvertStatement(statements[statements.Length - 1]);

            //TODO: переделать. сдель функцию которая ложет ret и MarkSequencePoint
            if (save_debug_info && !(statements[statements.Length - 1] is SemanticTree.IReturnNode))
            {
                //если почледний оператор не Return то пометить закрывающуюю логическую скобку
                if (gen_right_brackets)
                    MarkSequencePoint(value.RightLogicalBracketLocation);
                //il.Emit(OpCodes.Nop);
            }
        }

        private bool is_assign(basic_function_type bft)
        {
            switch (bft)
            {
                case basic_function_type.iassign:
                case basic_function_type.bassign:
                case basic_function_type.lassign:
                case basic_function_type.sassign:
                case basic_function_type.dassign:
                case basic_function_type.fassign:
                case basic_function_type.boolassign:
                case basic_function_type.objassign:
                case basic_function_type.charassign: return true;
            }
            return false;
        }

        private void ConvertExpression(IExpressionNode value)
        {
            make_next_spoint = false;
            value.visit(this);
        }

        private bool BeginOnForNode(IStatementNode value)
        {
            //if (value is IForNode) return true;
            IStatementsListNode stats = value as IStatementsListNode;
            if (stats == null) return false;
            if (stats.statements.Length == 0) return false;
            //if (stats.statements[0] is IForNode) return true;
            return false;
        }

        //перевод statement-а
        private void ConvertStatement(IStatementNode value)
        {
            make_next_spoint = true;
            if (save_debug_info /*&& !(value is IForNode)*/)
                MarkSequencePoint(value.Location);
            make_next_spoint = false;
            value.visit(this);
            make_next_spoint = true;
            //нужно для очистки стека после вызова функции в качестве процедуры
            //ssyy добавил
            //если вызов конструктора предка, то стек не очищаем
            if (!(
                (value is IFunctionCallNode) && ((IFunctionCallNode)value).last_result_function_call ||
                (value is ICompiledConstructorCall) && !((ICompiledConstructorCall)value).new_obj_awaited() ||
                (value is ICommonConstructorCall) && !((ICommonConstructorCall)value).new_obj_awaited()
            ))
                //\ssyy
                if ((value is IFunctionCallNode) && !(value is IBasicFunctionCallNode && (value as IBasicFunctionCallNode).basic_function.basic_function_type != basic_function_type.none))
                {
                    IFunctionCallNode fc = value as IFunctionCallNode;
                    if (fc.function.return_value_type != null)
                    {
                        ICompiledTypeNode ct = fc.function.return_value_type as ICompiledTypeNode;
                        if ((ct == null) || (ct != null && (ct.compiled_type != TypeFactory.VoidType)))
                            il.Emit(OpCodes.Pop);
                    }
                }
        }

        private bool gen_right_brackets = true;
        private bool gen_left_brackets = true;
        public override void visit(SemanticTree.IForNode value)
        {
            Label l1 = il.DefineLabel();
            Label l2 = il.DefineLabel();
            Label lcont = il.DefineLabel();
            Label lbreak = il.DefineLabel();
            bool tmp = save_debug_info;
            save_debug_info = false;
            if (value.initialization_statement != null)
                ConvertStatement(value.initialization_statement);
            save_debug_info = tmp;
            if (value.init_while_expr != null)
            {
                value.init_while_expr.visit(this);
                il.Emit(OpCodes.Brfalse, lbreak);
            }
            else
                il.Emit(OpCodes.Br, l1);
            il.MarkLabel(l2);
            labels.Push(lbreak);
            clabels.Push(lcont);
            bool tmp_rb = gen_right_brackets;
            bool tmp_lb = gen_left_brackets;
            gen_right_brackets = false;
            gen_left_brackets = false;
            ConvertStatement(value.body);
            gen_right_brackets = tmp_rb;
            gen_left_brackets = tmp_lb;
            il.MarkLabel(lcont);
            tmp = save_debug_info;

            if (value.init_while_expr == null)
            {
                save_debug_info = false;
                //				MarkSequencePoint(il,0xFeeFee,0xFeeFee,0xFeeFee,0xFeeFee);
                ConvertStatement(value.increment_statement);
                save_debug_info = tmp;
            }
            il.MarkLabel(l1);
            MarkSequencePoint(il, value.increment_statement.Location);
            value.while_expr.visit(this);
            //if (!value.IsBoolCycle)
            if (value.init_while_expr == null)
                il.Emit(OpCodes.Brtrue, l2);
            else
            {
                Label l3 = il.DefineLabel();
                il.Emit(OpCodes.Brfalse, l3);
                save_debug_info = false;
                //				MarkSequencePoint(il,0xFeeFee,0xFeeFee,0xFeeFee,0xFeeFee);
                ConvertStatement(value.increment_statement);
                save_debug_info = tmp;
                il.Emit(OpCodes.Br, l2);
                il.MarkLabel(l3);
            }
            il.MarkLabel(lbreak);
            labels.Pop();
            clabels.Pop();
        }

        public override void visit(SemanticTree.IRepeatNode value)
        {
            Label TrueLabel, FalseLabel;
            TrueLabel = il.DefineLabel();
            FalseLabel = il.DefineLabel();
            il.MarkLabel(TrueLabel);
            labels.Push(FalseLabel);//break
            clabels.Push(TrueLabel);//continue
            if (save_debug_info) MarkForCicles(value.Location, value.body.Location);
            ConvertStatement(value.body);
            value.condition.visit(this);
            il.Emit(OpCodes.Brfalse, TrueLabel);
            il.MarkLabel(FalseLabel);
            clabels.Pop();
            labels.Pop();
        }

        private void MarkForCicles(ILocation loc, ILocation body_loc)
        {
            if (loc != null)
                if (loc.begin_line_num == body_loc.end_line_num) MarkSequencePoint(il, body_loc);
        }

        public override void visit(SemanticTree.IWhileNode value)
        {
            Label TrueLabel, FalseLabel;
            TrueLabel = il.DefineLabel();
            FalseLabel = il.DefineLabel();
            il.MarkLabel(TrueLabel);
            value.condition.visit(this);
            il.Emit(OpCodes.Brfalse, FalseLabel);
            labels.Push(FalseLabel);//break
            clabels.Push(TrueLabel);//continue
            bool tmp_lb = gen_left_brackets;
            gen_left_brackets = false;
            ConvertStatement(value.body);
            gen_left_brackets = tmp_lb;
            il.Emit(OpCodes.Br, TrueLabel);
            il.MarkLabel(FalseLabel);
            clabels.Pop();
            labels.Pop();
        }

        public override void visit(SemanticTree.ITryBlockNode value)
        {
            Label exBl = il.BeginExceptionBlock();
            var safe_block = EnterSafeBlock();
            ConvertStatement(value.TryStatements);
            LeaveSafeBlock(safe_block);
            if (value.ExceptionFilters.Length != 0)
            {
                foreach (SemanticTree.IExceptionFilterBlockNode iefbn in value.ExceptionFilters)
                {
                    Type typ;
                    if (iefbn.ExceptionType != null)
                        typ = helper.GetTypeReference(iefbn.ExceptionType).tp;
                    else
                        typ = TypeFactory.ExceptionType;
                    il.BeginCatchBlock(typ);
                    
                    if (iefbn.ExceptionInstance != null)
                    {
                        LocalBuilder lb = il.DeclareLocal(typ);
                        helper.AddVariable(iefbn.ExceptionInstance.Variable, lb);
                        if (save_debug_info && iefbn.ExceptionInstance.Location != null)
                            lb.SetLocalSymInfo(iefbn.ExceptionInstance.Variable.name + ":" + iefbn.ExceptionInstance.Location.begin_line_num + ":" + iefbn.ExceptionInstance.Location.end_line_num);
                        il.Emit(OpCodes.Stloc, lb);
                    }
                    else
                    {
                        il.Emit(OpCodes.Pop);
                    }
                    safe_block = EnterSafeBlock();
                    ConvertStatement(iefbn.ExceptionHandler);
                    LeaveSafeBlock(safe_block);
                }
            }
            if (value.FinallyStatements != null)
            {
                il.BeginFinallyBlock();
                safe_block = EnterSafeBlock();
                ConvertStatement(value.FinallyStatements);
                LeaveSafeBlock(safe_block);
            }
            il.EndExceptionBlock();
        }

        public override void visit(ILabeledStatementNode value)
        {
            Label lab = helper.GetLabel(value.label, il);
            il.MarkLabel(lab);
            value.statement.visit(this);
        }

        public override void visit(IGotoStatementNode value)
        {
            Label lab = helper.GetLabel(value.label, il);
            if (safe_block)
                il.Emit(OpCodes.Leave, lab);
            else
                il.Emit(OpCodes.Br, lab);
        }

        private Stack<Label> if_stack = new Stack<Label>();

        private bool contains_only_if(IStatementNode stmt)
        {
            return stmt is IIfNode || stmt is ISwitchNode || stmt is IStatementsListNode && (stmt as IStatementsListNode).statements.Length == 1 &&
                ((stmt as IStatementsListNode).statements[0] is IIfNode || (stmt as IStatementsListNode).statements[0] is ISwitchNode);
        }

        public override void visit(SemanticTree.IIfNode value)
        {
            Label FalseLabel, EndLabel;
            FalseLabel = il.DefineLabel();
            bool end_label_def = false;
            bool is_first_if = false;
            if (contains_only_if(value.then_body))
            {
                if (if_stack.Count == 0)
                {
                    EndLabel = il.DefineLabel();
                    end_label_def = true;
                    is_first_if = true;
                    if_stack.Push(EndLabel);
                }
                else
                    EndLabel = if_stack.Peek();
            }
            else if (if_stack.Count > 0 && !contains_only_if(value.then_body))
                EndLabel = if_stack.Pop();
            else
            {
                end_label_def = true;
                EndLabel = il.DefineLabel();
            }
            value.condition.visit(this);
            il.Emit(OpCodes.Brfalse, FalseLabel);

            ConvertStatement(value.then_body);
            il.Emit(OpCodes.Br, EndLabel);
            il.MarkLabel(FalseLabel);
            if (value.else_body != null)
                ConvertStatement(value.else_body);
            if (end_label_def)
            {
                if (is_first_if)
                    if_stack.Clear();
                il.MarkLabel(EndLabel);
            }
        }

        public override void visit(IWhileBreakNode value)
        {
            Label l = labels.Peek();
            if (safe_block)
                il.Emit(OpCodes.Leave, l);
            else
                il.Emit(OpCodes.Br, l);
        }

        public override void visit(IRepeatBreakNode value)
        {
            Label l = labels.Peek();
            if (safe_block)
                il.Emit(OpCodes.Leave, l);
            else
                il.Emit(OpCodes.Br, l);
        }

        public override void visit(IForBreakNode value)
        {
            Label l = labels.Peek();
            if (safe_block)
                il.Emit(OpCodes.Leave, l);
            else
                il.Emit(OpCodes.Br, l);
        }

        public override void visit(IWhileContinueNode value)
        {
            Label l = clabels.Peek();
            if (safe_block)
                il.Emit(OpCodes.Leave, l);
            else
                il.Emit(OpCodes.Br, l);
        }

        public override void visit(IRepeatContinueNode value)
        {
            Label l = clabels.Peek();
            if (safe_block)
                il.Emit(OpCodes.Leave, l);
            else
                il.Emit(OpCodes.Br, l);
        }

        public override void visit(IForContinueNode value)
        {
            Label l = clabels.Peek();
            if (safe_block)
                il.Emit(OpCodes.Leave, l);
            else
                il.Emit(OpCodes.Br, l);
        }

        public override void visit(IForeachBreakNode value)
        {
            Label l = labels.Peek();
            if (safe_block)
                il.Emit(OpCodes.Leave, l);
            else
                il.Emit(OpCodes.Br, l);
        }

        public override void visit(IForeachContinueNode value)
        {
            Label l = clabels.Peek();
            if (safe_block)
                il.Emit(OpCodes.Leave, l);
            else
                il.Emit(OpCodes.Br, l);
        }

        public override void visit(SemanticTree.ICompiledMethodNode value)
        {

        }

        //перевод тела конструктора
        private void ConvertConstructorBody(SemanticTree.ICommonMethodNode value)
        {
            num_scope++;
            //получаем билдер конструктора
            ConstructorBuilder cnstr = helper.GetConstructorBuilder(value);
            ConstructorBuilder tmp = cur_cnstr;
            cur_cnstr = cnstr;
            ILGenerator tmp_il = il;
            MethInfo copy_mi = null;
            if (value.functions_nodes.Length > 0)
            {
                copy_mi = ConvertMethodWithNested(value, cnstr);
            }
            if (value.functions_nodes.Length == 0)
            {
                if (!(value.function_code is SemanticTree.IRuntimeManagedMethodBody))
                {
                    il = cnstr.GetILGenerator();
                    //переводим локальные переменные
                    ConvertCommonFunctionConstantDefinitions(value.constants);
                    ConvertLocalVariables(value.var_definition_nodes);
                    //вызываем метод $Init$ для инициализации массивов и проч.
                    /*if (value.polymorphic_state != polymorphic_state.ps_static && value.common_comprehensive_type.base_type is ICompiledTypeNode && (value.common_comprehensive_type.base_type as ICompiledTypeNode).compiled_type == TypeFactory.ObjectType)
                    {
                        il.Emit(OpCodes.Ldarg_0);
                        il.Emit(OpCodes.Call, TypeFactory.ObjectType.GetConstructor(Type.EmptyTypes));
                    }*/
                    if (value.polymorphic_state != polymorphic_state.ps_static)
                    {
                        init_call_awaited = true;

                        //il.Emit(OpCodes.Ldarg_0);
                        //il.Emit(OpCodes.Call, cur_ti.init_meth);
                    }
                    //переводим тело
                    is_constructor = true;
                    ConvertBody(value.function_code);
                    if (save_debug_info)
                    {
                        AddSpecialDebugVariables();
                    }
                    is_constructor = false;
                }
            }
            else
            {
                ConvertFunctionBody(value, copy_mi, false);
                //вызов статического метода-клона
                //при этом явно передается this
                il = cnstr.GetILGenerator();
                if (save_debug_info)
                    MarkSequencePoint(il, 0xFFFFFF, 0, 0xFFFFFF, 0);
                ConvertStatement((value.function_code as IStatementsListNode).statements[0]);
                il.Emit(OpCodes.Ldarg_0);
                IParameterNode[] parameters = value.parameters;
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (i + 1 < 255)
                        il.Emit(OpCodes.Ldarg_S, (byte)(i + 1));
                    else
                        //здесь надо быть внимательнее
                        il.Emit(OpCodes.Ldarg, (short)(i + 1));
                }
                il.Emit(OpCodes.Call, copy_mi.mi);
                il.Emit(OpCodes.Pop);
                il.Emit(OpCodes.Ret);
            }
            cur_cnstr = tmp;
            il = tmp_il;
            num_scope--;
        }

        public override void visit(SemanticTree.ICommonMethodNode value)
        {
            if (value.is_constructor == true)
            {
                ConvertConstructorBody(value);
                return;
            }
            if (value.function_code is IStatementsListNode)
            {
                IStatementNode[] statements = (value.function_code as IStatementsListNode).statements;
                if (statements.Length > 0 && (statements[0] is IExternalStatementNode || statements[0] is IPInvokeStatementNode))
                {
                    MakeAttribute(value);
                    return;
                }
                    
            }
            
            num_scope++;
            MakeAttribute(value);
            MethodBuilder methb = helper.GetMethodBuilder(value);
            //helper.GetMethod(value)
            MethodBuilder tmp = cur_meth;
            MethInfo copy_mi = null;
            cur_meth = methb;
            ILGenerator tmp_il = il;
            //если метод содержит вложенные процедуры
            if (value.functions_nodes.Length > 0)
            {
                copy_mi = ConvertMethodWithNested(value, methb);
            }
            //если нет вложенных процедур
            if (value.functions_nodes.Length == 0)
            {
                if (!(value.function_code is SemanticTree.IRuntimeManagedMethodBody))
                {
                    //ssyy!!! добавил условие для интерфейсов
                    if (value.function_code != null)
                    {
                        il = methb.GetILGenerator();
                        ConvertLocalVariables(value.var_definition_nodes);
                        ConvertCommonFunctionConstantDefinitions(value.constants);
                        ConvertBody(value.function_code);
                        if (save_debug_info)
                        {
                            AddSpecialDebugVariables();
                        }
                        if (methb.ReturnType == TypeFactory.VoidType)
                            il.Emit(OpCodes.Ret);
                    }
                }
            }
            else
            {
                ConvertFunctionBody(value, copy_mi, true);
                //вызов статического метода-клона
                //при этом явно передается this
                il = methb.GetILGenerator();
                if (save_debug_info)
                    MarkSequencePoint(il, 0xFFFFFF, 0, 0xFFFFFF, 0);
                il.Emit(OpCodes.Ldarg_0);
                IParameterNode[] parameters = value.parameters;
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (i + 1 < 255)
                        il.Emit(OpCodes.Ldarg_S, (byte)(i + 1));
                    else
                        //здесь надо быть внимательнее
                        il.Emit(OpCodes.Ldarg, (short)(i + 1));
                }
                il.Emit(OpCodes.Call, copy_mi.mi);
                il.Emit(OpCodes.Ret);
            }
            cur_meth = tmp;
            il = tmp_il;
            num_scope--;
        }

        private MethInfo ConvertMethodWithNested(SemanticTree.ICommonMethodNode meth, ConstructorBuilder methodb)
        {
            num_scope++; //увеличиваем глубину обл. видимости
            TypeBuilder tb = null, tmp_type = cur_type;
            Frame frm = null;
            //func.functions_nodes.Length > 0 - имеет вложенные
            //funcs.Count > 0 - сама вложенная
            frm = MakeAuxType(meth);//создаем запись активации
            tb = frm.tb;
            cur_type = tb;
            //получаем тип возвр. значения
            Type[] tmp_param_types = GetParamTypes(meth);
            Type[] param_types = new Type[tmp_param_types.Length + 1];
            //прибавляем тип this
            param_types[0] = methodb.DeclaringType;
            tmp_param_types.CopyTo(param_types, 1);

            //определяем метод
            MethodBuilder methb = tb.DefineMethod("cnstr$" + uid++, MethodAttributes.Public | MethodAttributes.Static, methodb.DeclaringType, param_types);
            MethInfo mi = null;
            //добавляем его фиктивно (т.е. не заносим в таблицы Helper-а) дабы остальные вызывали метод-заглушку
            mi = helper.AddFictiveMethod(meth, methb);
            mi.num_scope = num_scope;
            mi.disp = frm;//задаем запись активации
            mi.is_in_class = true;//указываем что метод в классе
            smi.Push(mi);//кладем его в стек
            ParameterBuilder pb = null;
            int num = 1;
            ILGenerator tmp_il = il;

            il = methb.GetILGenerator();
            IParameterNode[] parameters = meth.parameters;
            //if (ret_type != typeof(void)) mi.ret_val = il.DeclareLocal(ret_type);
            FieldBuilder[] fba = new FieldBuilder[parameters.Length];
            //явно определяем this
            pb = methb.DefineParameter(1, ParameterAttributes.None, "$obj$");

            //та же самая чертовщина с глобальными параметрами, только здесь учитываем
            //наличие дополнительного параметра this
            for (int i = 0; i < parameters.Length; i++)
            {
                pb = methb.DefineParameter(i + num + 1, ParameterAttributes.None, parameters[i].name);
                if (parameters[i].is_params)
                    pb.SetCustomAttribute(TypeFactory.ParamArrayAttributeConstructor, new byte[] { 0x1, 0x0, 0x0, 0x0 });
                FieldBuilder fb = null;
                if (parameters[i].parameter_type == parameter_type.value)
                    fb = frm.tb.DefineField(parameters[i].name, param_types[i + num], FieldAttributes.Public);
                else
                {
                    Type pt = param_types[i + num].Module.GetType(param_types[i + num].FullName.Substring(0, param_types[i + num].FullName.IndexOf('&')) + "*");
                    if (pt == null) mb.GetType(param_types[i + num].FullName.Substring(0, param_types[i + num].FullName.IndexOf('&')) + "*");
                    fb = frm.tb.DefineField(parameters[i].name, pt, FieldAttributes.Public);
                }
                helper.AddGlobalParameter(parameters[i], fb).meth = smi.Peek();
                fba[i] = fb;
            }
            //переменная, хранящая запись активации
            LocalBuilder frame = il.DeclareLocal(cur_type);
            mi.frame = frame;
            if (doc != null) frame.SetLocalSymInfo("$disp$");
            //создание записи активации
            il.Emit(OpCodes.Newobj, frm.cb);
            il.Emit(OpCodes.Stloc_0, frame);
            //заполнение полей параметрами
            for (int j = 0; j < fba.Length; j++)
            {
                il.Emit(OpCodes.Ldloc_0);
                if (parameters[j].parameter_type == parameter_type.value)
                {
                    il.Emit(OpCodes.Ldarg_S, (byte)(j + 1));
                }
                else
                {
                    il.Emit(OpCodes.Ldarg_S, (byte)(j + 1));
                }
                il.Emit(OpCodes.Stfld, fba[j]);
            }
            funcs.Add(meth);
            MethodBuilder tmp = cur_meth;
            cur_meth = methb;
            //перевод нелокальных переменных
            ConvertNonLocalVariables(meth.var_definition_nodes, frm.mb);
            //перевод процедур, вложенных в метод
            ConvertNestedInMethodFunctionHeaders(meth.functions_nodes, methodb.DeclaringType);
            il = tmp_il;
            foreach (ICommonNestedInFunctionFunctionNode f in meth.functions_nodes)
                ConvertFunctionBody(f);
            if (frm != null)
                frm.mb.GetILGenerator().Emit(OpCodes.Ret);
            cur_type = tmp_type;
            num_scope--;
            smi.Pop();
            funcs.RemoveAt(funcs.Count - 1);
            return mi;
        }

        //перевод метода с вложенными процедурами
        private MethInfo ConvertMethodWithNested(SemanticTree.ICommonMethodNode meth, MethodBuilder methodb)
        {
            num_scope++; //увеличиваем глубину обл. видимости
            TypeBuilder tb = null, tmp_type = cur_type;
            Frame frm = null;
            //func.functions_nodes.Length > 0 - имеет вложенные
            //funcs.Count > 0 - сама вложенная
            frm = MakeAuxType(meth);//создаем запись активации
            tb = frm.tb;
            cur_type = tb;
            //получаем тип возвр. значения
            Type[] tmp_param_types = GetParamTypes(meth);
            Type[] param_types = new Type[tmp_param_types.Length + 1];
            //прибавляем тип this
            if (methodb.DeclaringType.IsValueType)
                param_types[0] = methodb.DeclaringType.MakeByRefType();
            else
                param_types[0] = methodb.DeclaringType;
            tmp_param_types.CopyTo(param_types, 1);

            //определяем метод
            MethodBuilder methb = tb.DefineMethod(methodb.Name, MethodAttributes.Public | MethodAttributes.Static, methodb.ReturnType, param_types);
            MethInfo mi = null;
            //добавляем его фиктивно (т.е. не заносим в таблицы Helper-а) дабы остальные вызывали метод-заглушку
            mi = helper.AddFictiveMethod(meth, methb);
            mi.num_scope = num_scope;
            mi.disp = frm;//задаем запись активации
            mi.is_in_class = true;//указываем что метод в классе
            smi.Push(mi);//кладем его в стек
            ParameterBuilder pb = null;
            int num = 1;
            ILGenerator tmp_il = il;

            IParameterNode[] parameters = meth.parameters;
            il = methb.GetILGenerator();
            //if (ret_type != typeof(void)) mi.ret_val = il.DeclareLocal(ret_type);
            FieldBuilder[] fba = new FieldBuilder[parameters.Length];
            //явно определяем this
            pb = methb.DefineParameter(1, ParameterAttributes.None, "$obj$");

            //та же самая чертовщина с глобальными параметрами, только здесь учитываем
            //наличие дополнительного параметра this
            for (int i = 0; i < parameters.Length; i++)
            {
                pb = methb.DefineParameter(i + num + 1, ParameterAttributes.None, parameters[i].name);
                FieldBuilder fb = null;
                if (parameters[i].parameter_type == parameter_type.value)
                    fb = frm.tb.DefineField(parameters[i].name, param_types[i + num], FieldAttributes.Public);
                else
                {
                    Type pt = param_types[i + num].Module.GetType(param_types[i + num].FullName.Substring(0, param_types[i + num].FullName.IndexOf('&')) + "*");
                    if (pt == null) mb.GetType(param_types[i + num].FullName.Substring(0, param_types[i + num].FullName.IndexOf('&')) + "*");
                    fb = frm.tb.DefineField(parameters[i].name, pt, FieldAttributes.Public);
                }
                helper.AddGlobalParameter(parameters[i], fb).meth = smi.Peek();
                fba[i] = fb;
            }
            //переменная, хранящая запись активации
            LocalBuilder frame = il.DeclareLocal(cur_type);
            mi.frame = frame;
            if (doc != null) frame.SetLocalSymInfo("$disp$");
            //создание записи активации
            il.Emit(OpCodes.Newobj, frm.cb);
            il.Emit(OpCodes.Stloc_0, frame);
            //заполнение полей параметрами
            for (int j = 0; j < fba.Length; j++)
            {
                il.Emit(OpCodes.Ldloc_0);
                if (parameters[j].parameter_type == parameter_type.value)
                {
                    il.Emit(OpCodes.Ldarg_S, (byte)(j + 1));
                }
                else
                {
                    il.Emit(OpCodes.Ldarg_S, (byte)(j + 1));
                }
                il.Emit(OpCodes.Stfld, fba[j]);
            }
            funcs.Add(meth);
            MethodBuilder tmp = cur_meth;
            cur_meth = methb;
            ConvertCommonFunctionConstantDefinitions(meth.constants);
            //перевод нелокальных переменных
            ConvertNonLocalVariables(meth.var_definition_nodes, frm.mb);
            //перевод процедур, вложенных в метод
            ConvertNestedInMethodFunctionHeaders(meth.functions_nodes, methodb.DeclaringType);
            il = tmp_il;
            foreach (ICommonNestedInFunctionFunctionNode f in meth.functions_nodes)
                ConvertFunctionBody(f);
            if (frm != null)
                frm.mb.GetILGenerator().Emit(OpCodes.Ret);
            cur_type = tmp_type;
            num_scope--;
            smi.Pop();
            funcs.RemoveAt(funcs.Count - 1);
            return mi;
        }

        private void ConvertNestedInMethodFunctionHeaders(ICommonNestedInFunctionFunctionNode[] funcs, Type decl_type)
        {
            foreach (ICommonNestedInFunctionFunctionNode func in funcs)
            {
                ConvertNestedInMethodFunctionHeader(func, decl_type);
            }
        }

        private void ConvertNestedInMethodFunctionHeader(ICommonNestedInFunctionFunctionNode func, Type decl_type)
        {
            num_scope++; //увеличиваем глубину обл. видимости
            TypeBuilder tb = null, tmp_type = cur_type;
            Frame frm = null;
            //func.functions_nodes.Length > 0 - имеет вложенные
            //funcs.Count > 0 - сама вложенная
            frm = MakeAuxType(func);//создаем запись активации
            tb = frm.tb;
            cur_type = tb;
            Type ret_type = null;
            //получаем тип возвр. значения
            if (func.return_value_type == null)
                ret_type = TypeFactory.VoidType;
            else
                ret_type = helper.GetTypeReference(func.return_value_type).tp;
            //получаем типы параметров
            Type[] tmp_param_types = GetParamTypes(func);
            Type[] param_types = new Type[tmp_param_types.Length + 1];
            if (decl_type.IsValueType)
                param_types[0] = decl_type.MakeByRefType();
            else
                param_types[0] = decl_type;
            tmp_param_types.CopyTo(param_types, 1);
            MethodAttributes attrs = MethodAttributes.Public | MethodAttributes.Static;
            //определяем саму процедуру/функцию
            MethodBuilder methb = null;
            methb = tb.DefineMethod(func.name, attrs, ret_type, param_types);
            MethInfo mi = null;
            if (smi.Count != 0)
                mi = helper.AddMethod(func, methb, smi.Peek());
            else
                mi = helper.AddMethod(func, methb);
            mi.num_scope = num_scope;
            mi.disp = frm;
            mi.is_in_class = true;//процедура вложена в метод
            smi.Push(mi);
            ParameterBuilder pb = null;
            int num = 0;
            ILGenerator tmp_il = il;
            il = methb.GetILGenerator();
            //if (ret_type != typeof(void)) mi.ret_val = il.DeclareLocal(ret_type);
            mi.nested = true;
            methb.DefineParameter(1, ParameterAttributes.None, "$obj$");
            methb.DefineParameter(2, ParameterAttributes.None, "$up$");
            num = 2;
            IParameterNode[] parameters = func.parameters;
            //
            FieldBuilder[] fba = new FieldBuilder[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                pb = methb.DefineParameter(i + num + 1, ParameterAttributes.None, parameters[i].name);
                if (parameters[i].is_params)
                    pb.SetCustomAttribute(TypeFactory.ParamArrayAttributeConstructor, new byte[] { 0x1, 0x0, 0x0, 0x0 });
                if (func.functions_nodes.Length > 0)
                {
                    FieldBuilder fb = null;
                    if (parameters[i].parameter_type == parameter_type.value)
                        fb = frm.tb.DefineField(parameters[i].name, param_types[i + num], FieldAttributes.Public);
                    else
                    {
                        Type pt = param_types[i + num].Module.GetType(param_types[i + num].FullName.Substring(0, param_types[i + num].FullName.IndexOf('&')) + "*");
                        if (pt == null) mb.GetType(param_types[i + num].FullName.Substring(0, param_types[i + num].FullName.IndexOf('&')) + "*");
                        fb = frm.tb.DefineField(parameters[i].name, pt, FieldAttributes.Public);
                    }
                    helper.AddGlobalParameter(parameters[i], fb).meth = smi.Peek();
                    fba[i] = fb;
                }
                else helper.AddParameter(parameters[i], pb).meth = smi.Peek();
            }

            LocalBuilder frame = il.DeclareLocal(cur_type);
            mi.frame = frame;
            if (doc != null) frame.SetLocalSymInfo("$disp$");
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Newobj, frm.cb);
            il.Emit(OpCodes.Stloc, frame);

            //инициализация полей записи активации нелокальными параметрами
            if (func.functions_nodes.Length > 0)
                for (int j = 0; j < fba.Length; j++)
                {
                    il.Emit(OpCodes.Ldloc_0);
                    if (parameters[j].parameter_type == parameter_type.value)
                    {
                        il.Emit(OpCodes.Ldarg_S, (byte)(j + 2));
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldarga_S, (byte)(j + 2));
                    }
                    il.Emit(OpCodes.Stfld, fba[j]);
                }
            funcs.Add(func);
            MethodBuilder tmp = cur_meth;
            cur_meth = methb;
            //переводим переменные как нелокальные
            ConvertNonLocalVariables(func.var_definition_nodes, frm.mb);
            //переводим описания вложенных процедур
            ConvertNestedInMethodFunctionHeaders(func.functions_nodes, decl_type);
            foreach (ICommonNestedInFunctionFunctionNode f in func.functions_nodes)
                ConvertFunctionBody(f);
            if (frm != null)
                frm.mb.GetILGenerator().Emit(OpCodes.Ret);

            cur_type = tmp_type;
            num_scope--;
            smi.Pop();
            funcs.RemoveAt(funcs.Count - 1);
        }

        private Type[] GetParamTypes(ICommonMethodNode func)
        {
            Type[] tt = null;
            int num = 0;
            IParameterNode[] parameters = func.parameters;
            tt = new Type[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                Type tp = helper.GetTypeReference(parameters[i].type).tp;
                if (parameters[i].parameter_type == parameter_type.value)
                    tt[i + num] = tp;
                else
                {
                    tt[i + num] = tp.MakeByRefType();
                }
            }
            return tt;
        }

        private Type[] GetParamTypes(ICommonPropertyNode func)
        {
            Type[] tt = null;
            int num = 0;
            IParameterNode[] parameters = func.parameters;
            tt = new Type[parameters.Length];
            for (int i = 0; i < func.parameters.Length; i++)
            {
                Type tp = helper.GetTypeReference(parameters[i].type).tp;
                if (func.parameters[i].parameter_type == parameter_type.value)
                    tt[i + num] = tp;
                else
                {
                    tt[i + num] = tp.MakeByRefType();
                }
            }
            return tt;
        }

        //получение атрибутов метода
        private MethodAttributes GetMethodAttributes(SemanticTree.ICommonMethodNode value, bool is_accessor)
        {
            MethodAttributes attrs = ConvertFALToMethodAttributes(value.field_access_level);
            if (is_accessor)
                attrs = MethodAttributes.Public;
            switch (value.polymorphic_state)
            {
                case polymorphic_state.ps_static: attrs |= MethodAttributes.Static; break;
                case polymorphic_state.ps_virtual:
                    attrs |= MethodAttributes.Virtual;
                    break;
                //ssyy
                case polymorphic_state.ps_virtual_abstract: attrs |= MethodAttributes.Virtual | MethodAttributes.Abstract; break;
                //\ssyy
            }
            return attrs;
        }

        private MethodAttributes GetConstructorAttributes(SemanticTree.ICommonMethodNode value)
        {
            MethodAttributes attrs = ConvertFALToMethodAttributes(value.field_access_level);
            switch (value.polymorphic_state)
            {
                case polymorphic_state.ps_virtual: attrs |= MethodAttributes.Virtual; break;
            }
            return attrs;
        }

        //перевод заголовка конструктора
        private void ConvertConstructorHeader(SemanticTree.ICommonMethodNode value)
        {
            if (helper.GetConstructor(value) != null) return;

            //определяем конструктор
            ConstructorBuilder cnstr;
            IRuntimeManagedMethodBody irmmb = null;
            if (value.polymorphic_state == polymorphic_state.ps_static)
            {
                cnstr = cur_type.DefineTypeInitializer();
                cur_ti.static_cnstr = cnstr;
            }
            else
            {
                Type[] param_types = GetParamTypes(value);
                MethodAttributes attrs = GetConstructorAttributes(value);

                irmmb = value.function_code as IRuntimeManagedMethodBody;
                if (irmmb != null)
                {
                    if (irmmb.runtime_statement_type == SemanticTree.runtime_statement_type.ctor_delegate)
                    {
                        attrs = MethodAttributes.Public | MethodAttributes.HideBySig;
                        param_types = new Type[2];
                        param_types[0] = TypeFactory.ObjectType;
                        param_types[1] = TypeFactory.IntPtr;
                    }
                }
                cnstr = cur_type.DefineConstructor(attrs, CallingConventions.HasThis, param_types);
            }

            if (irmmb != null)
            {
                cnstr.SetImplementationFlags(MethodImplAttributes.Runtime);
            }

            MethInfo mi = null;
            mi = helper.AddConstructor(value, cnstr);
            mi.num_scope = num_scope + 1;
            if (save_debug_info)
            {
                if (value.function_code is IStatementsListNode)
                    MarkSequencePoint(cnstr.GetILGenerator(), ((IStatementsListNode)value.function_code).LeftLogicalBracketLocation);
            }
            ConvertConstructorParameters(value, cnstr);
        }

        //процедура проверки нужно ли заменять тип возвр. знач. метода get_val массива на указатель
        private bool IsNeedCorrectGetType(TypeInfo cur_ti, Type ret_type)
        {
            return (cur_ti.is_arr && ret_type != TypeFactory.VoidType && ret_type.IsValueType && !TypeFactory.IsStandType(ret_type) && !TypeIsEnum(ret_type));
        }

        private bool IsPropertyAccessor(ICommonMethodNode value)
        {
            return comp_opt.target == TargetType.Dll && prop_accessors.ContainsKey(value);
        }

        private void ConvertExternalMethod(SemanticTree.ICommonMethodNode meth)
        {
            IStatementsListNode sl = (IStatementsListNode)meth.function_code;
            IStatementNode[] statements = sl.statements;
            //функция импортируется из dll
            Type ret_type = null;
            //получаем тип возвр. значения
            if (meth.return_value_type == null)
                ret_type = null;//typeof(void);
            else
                ret_type = helper.GetTypeReference(meth.return_value_type).tp;
            Type[] param_types = GetParamTypes(meth);//получаем параметры процедуры
            MethodBuilder methb = null;
            if (statements[0] is IExternalStatementNode)
            {
                IExternalStatementNode esn = (IExternalStatementNode)statements[0];
                string module_name = Tools.ReplaceAllKeys(esn.module_name, StandartDirectories);
                methb = cur_type.DefinePInvokeMethod(meth.name, module_name, esn.name,
                                                                   MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.PinvokeImpl | MethodAttributes.HideBySig,
                                                                   CallingConventions.Standard, ret_type, param_types, CallingConvention.Winapi,
                                                                   CharSet.Ansi);//определяем PInvoke-метод
            }
            else
            {
                methb = cur_type.DefineMethod(meth.name, MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.PinvokeImpl | MethodAttributes.HideBySig, ret_type, param_types);//определяем PInvoke-метод
                methb.SetImplementationFlags(MethodImplAttributes.PreserveSig);
            }
            methb.SetImplementationFlags(MethodImplAttributes.PreserveSig);
            helper.AddMethod(meth, methb);
            IParameterNode[] parameters = meth.parameters;
            //определяем параметры с указанием имени
            for (int j = 0; j < parameters.Length; j++)
            {
                ParameterAttributes pars = ParameterAttributes.None;
                //if (func.parameters[j].parameter_type == parameter_type.var)
                //  pars = ParameterAttributes.Out;
                methb.DefineParameter(j + 1, pars, parameters[j].name);
            }
        }

        //перевод заголовка метода
        private void ConvertMethodHeader(SemanticTree.ICommonMethodNode value)
        {
            if (value.is_constructor == true)
            {
                ConvertConstructorHeader(value);
                return;
            }

            if (helper.GetMethod(value) != null)
                return;
            if (value.function_code is IStatementsListNode)
            {
                IStatementsListNode sl = (IStatementsListNode)value.function_code;
                IStatementNode[] statements = sl.statements;
                if (statements.Length > 0 && (statements[0] is IExternalStatementNode || statements[0] is IPInvokeStatementNode))
                {
                    ConvertExternalMethod(value);
                    return;
                }
            }
            MethodBuilder methb = null;
            bool is_prop_acc = IsPropertyAccessor(value);
            MethodAttributes attrs = GetMethodAttributes(value, is_prop_acc);
            IRuntimeManagedMethodBody irmmb = value.function_code as IRuntimeManagedMethodBody;
            if (irmmb != null)
            {
                if ((irmmb.runtime_statement_type == SemanticTree.runtime_statement_type.invoke_delegate) ||
                    (irmmb.runtime_statement_type == SemanticTree.runtime_statement_type.begin_invoke_delegate) ||
                    (irmmb.runtime_statement_type == SemanticTree.runtime_statement_type.end_invoke_delegate))
                {
                    attrs = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot |
                        MethodAttributes.Virtual;
                }
            }

            //определяем метод
            string method_name = OperatorsNameConvertor.convert_name(value.name);

            if (method_name != null)
            {
                attrs |= MethodAttributes.SpecialName;
            }
            else
            {
                bool get_set = false;
                method_name = GetPossibleAccessorName(value, out get_set);
                if (get_set)
                {
                    attrs |= MethodAttributes.SpecialName;
                }
            }

            //ssyy
            if (value.comperehensive_type.IsInterface)
            {
                attrs |= MethodAttributes.Virtual | MethodAttributes.Abstract | MethodAttributes.NewSlot;
            }

            if (value.is_final)
            {
                attrs |= MethodAttributes.Virtual | MethodAttributes.Final;
            }

            if (value.newslot_awaited)
            {
                attrs |= MethodAttributes.NewSlot;
            }
            //\ssyy

            methb = cur_type.DefineMethod(method_name, attrs);

            if (value.is_generic_function)
            {
                int count = value.generic_params.Count;
                string[] names = new string[count];
                for (int i = 0; i < count; i++)
                {
                    names[i] = value.generic_params[i].name;
                }
                methb.DefineGenericParameters(names);
                Type[] genargs = methb.GetGenericArguments();
                for (int i = 0; i < count; i++)
                {
                    helper.AddExistingType(value.generic_params[i], genargs[i]);
                }
                foreach (ICommonTypeNode par in value.generic_params)
                {
                    converting_generic_param = par;
                    ConvertTypeHeaderInSpecialOrder(par);
                }
                ConvertTypeInstancesInFunction(value);
            }

            Type ret_type = null;
            bool is_ptr_ret_type = false;
            if (value.return_value_type == null)
                ret_type = TypeFactory.VoidType;
            else
            {
                TypeInfo ti = helper.GetTypeReference(value.return_value_type);
                ret_type = ti.tp;
                if (IsNeedCorrectGetType(cur_ti, ret_type))
                {
                    ret_type = ret_type.MakePointerType();
                    is_ptr_ret_type = true;
                }
            }
            Type[] param_types = GetParamTypes(value);

            methb.SetParameters(param_types);
            methb.SetReturnType(ret_type);

            if (irmmb != null)
            {
                methb.SetImplementationFlags(MethodImplAttributes.Runtime);
            }

            if (save_debug_info)
            {
                if (value.function_code is IStatementsListNode)
                    MarkSequencePoint(methb.GetILGenerator(), ((IStatementsListNode)value.function_code).LeftLogicalBracketLocation);
            }
            MethInfo mi = null;
            mi = helper.AddMethod(value, methb);
            //binding CloneSet to set type
            if (value.comperehensive_type.type_special_kind == type_special_kind.base_set_type && value.name == "GetEnumerator")
            {
                helper.GetTypeReference(value.comperehensive_type).enumerator_meth = methb;
            }
            mi.is_ptr_ret_type = is_ptr_ret_type;
            mi.num_scope = num_scope + 1;
            ConvertMethodParameters(value, methb);
        }

        private void ConvertMethodParameters(ICommonMethodNode value, MethodBuilder methb)
        {
            ParameterBuilder pb = null;
            IParameterNode[] parameters = value.parameters;
            for (int i = 0; i < parameters.Length; i++)
            {
                object default_value = null;
                if (parameters[i].default_value != null)
                    default_value = helper.GetConstantForExpression(parameters[i].default_value);

                ParameterAttributes pa = ParameterAttributes.None;
                if (default_value != null)
                    pa |= ParameterAttributes.Optional;
                pb = methb.DefineParameter(i + 1, pa, parameters[i].name);
                if (default_value != null)
                    pb.SetConstant(default_value);
                helper.AddParameter(parameters[i], pb);
                if (parameters[i].is_params)
                    pb.SetCustomAttribute(TypeFactory.ParamArrayAttributeConstructor, new byte[] { 0x1, 0x0, 0x0, 0x0 });
            }
        }

        private void ConvertConstructorParameters(ICommonMethodNode value, ConstructorBuilder methb)
        {
            ParameterBuilder pb = null;
            IParameterNode[] parameters = value.parameters;
            for (int i = 0; i < parameters.Length; i++)
            {
                object default_value = null;
                if (parameters[i].default_value != null)
                    default_value = helper.GetConstantForExpression(parameters[i].default_value);
                ParameterAttributes pa = ParameterAttributes.None;
                if (parameters[i].parameter_type == parameter_type.var)
                    pa = ParameterAttributes.Retval;

                if (default_value != null)
                    pa |= ParameterAttributes.Optional;
                pb = methb.DefineParameter(i + 1, pa, parameters[i].name);
                if (default_value != null)
                    pb.SetConstant(default_value);
                helper.AddParameter(parameters[i], pb);
                if (parameters[i].is_params)
                    pb.SetCustomAttribute(TypeFactory.ParamArrayAttributeConstructor, new byte[] { 0x1, 0x0, 0x0, 0x0 });
            }
        }

        //вызов откомпилированного статического метода
        public override void visit(SemanticTree.ICompiledStaticMethodCallNode value)
        {
            if (comp_opt.dbg_attrs == DebugAttributes.Release && has_debug_conditional_attr(value.static_method.method_info))
                return;
            bool tmp_dot = is_dot_expr;//идет ли после этого точка
            is_dot_expr = false;
            ParameterInfo[] pinfs = value.static_method.method_info.GetParameters();
            //кладем параметры
            MethodInfo mi = value.static_method.method_info;
            IExpressionNode[] real_parameters = value.real_parameters;
            IParameterNode[] parameters = value.static_method.parameters;
            if (mi.DeclaringType == TypeFactory.ArrayType && mi.Name == "Resize" && helper.GetTypeReference(value.template_parametres[0]).tp.IsPointer)
            {
                is_addr = true;
                real_parameters[0].visit(this);
                is_addr = false;
                Label l1 = il.DefineLabel();
                Label l2 = il.DefineLabel();
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Ldind_Ref);
                il.Emit(OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Beq, l1);
                il.Emit(OpCodes.Dup);
                //il.Emit(OpCodes.Ldloc, lb);
                il.Emit(OpCodes.Ldind_Ref);
                real_parameters[1].visit(this);
                Type el_tp = helper.GetTypeReference(value.template_parametres[0]).tp;
                il.Emit(OpCodes.Newarr, el_tp);
                LocalBuilder tmp_lb = il.DeclareLocal(el_tp.MakeArrayType());
                il.Emit(OpCodes.Stloc, tmp_lb);
                il.Emit(OpCodes.Ldloc, tmp_lb);
                real_parameters[0].visit(this);
                il.Emit(OpCodes.Callvirt, TypeFactory.ArrayType.GetMethod("get_Length"));
                il.Emit(OpCodes.Call, TypeFactory.ArrayCopyMethod);
                il.Emit(OpCodes.Ldloc, tmp_lb);
                il.Emit(OpCodes.Br, l2);
                il.MarkLabel(l1);
                real_parameters[1].visit(this);
                il.Emit(OpCodes.Newarr, el_tp);
                il.MarkLabel(l2);
                il.Emit(OpCodes.Stind_Ref);
                TypeInfo ti = helper.GetTypeReference(real_parameters[0].type.element_type);
                this.CreateInitCodeForUnsizedArray(il, ti, real_parameters[0], real_parameters[1]);
                return;
            }
            LocalBuilder len_lb = null;
            LocalBuilder start_index_lb = null;
            if (mi.DeclaringType == TypeFactory.ArrayType && mi.Name == "Resize" && real_parameters.Length == 2)
            {
                start_index_lb = il.DeclareLocal(TypeFactory.Int32Type);
                il.Emit(OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Stloc, start_index_lb);
                Label lbl = il.DefineLabel();
                real_parameters[0].visit(this);
                il.Emit(OpCodes.Ldnull);
                il.Emit(OpCodes.Beq, lbl);
                real_parameters[0].visit(this);
                il.Emit(OpCodes.Ldlen);
                //real_parameters[1].visit(this);
                //il.Emit(OpCodes.Sub);
                il.Emit(OpCodes.Stloc, start_index_lb);
                il.MarkLabel(lbl);
            }
            //len_lb = EmitArguments(parameters, real_parameters, mi);
            
            for (int i = 0; i < real_parameters.Length; i++)
            {
            	if (real_parameters[i] is INullConstantNode && parameters[i].type.is_nullable_type)
                {
        			Type tp = helper.GetTypeReference(parameters[i].type).tp;
        			LocalBuilder lb = il.DeclareLocal(tp);
        			il.Emit(OpCodes.Ldloca, lb);
        			il.Emit(OpCodes.Initobj, tp);
        			il.Emit(OpCodes.Ldloc, lb);
        			continue;
        		}
                if (parameters[i].parameter_type == parameter_type.var)
                    is_addr = true;
                //TODO:Переделать.
                if (!is_addr)
                {
                    if (pinfs[i].ParameterType.IsByRef)
                    {
                        is_addr = true;
                    }
                }
                real_parameters[i].visit(this);
                
                if (mi.DeclaringType == TypeFactory.ArrayType && mi.Name == "Resize" && i == 1)
                {
                    if (real_parameters.Length == 2)
                    {
                        len_lb = il.DeclareLocal(helper.GetTypeReference(real_parameters[1].type).tp);
                        il.Emit(OpCodes.Stloc, len_lb);
                        il.Emit(OpCodes.Ldloc, len_lb);
                    }
                }
                //ICompiledTypeNode ctn = value.real_parameters[i].type as ICompiledTypeNode;
                ICompiledTypeNode ctn2 = parameters[i].type as ICompiledTypeNode;
                ITypeNode ctn3 = real_parameters[i].type;
                ITypeNode ctn4 = real_parameters[i].conversion_type;
                if (ctn2 != null && !(real_parameters[i] is SemanticTree.INullConstantNode) && (ctn3.is_value_type || ctn3.is_generic_parameter) && ctn2.compiled_type == TypeFactory.ObjectType)
                    il.Emit(OpCodes.Box, helper.GetTypeReference(ctn3).tp);
                else if (ctn2 != null && !(real_parameters[i] is SemanticTree.INullConstantNode) && ctn4 != null && (ctn4.is_value_type || ctn4.is_generic_parameter) && ctn2.compiled_type == TypeFactory.ObjectType)
                    il.Emit(OpCodes.Box, helper.GetTypeReference(ctn4).tp);
                is_addr = false;
            }
            //вызов метода

            if (value.template_parametres.Length > 0)
            {
                Type[] type_arr = new Type[value.template_parametres.Length];
                for (int int_i = 0; int_i < value.template_parametres.Length; int_i++)
                {
                    type_arr[int_i] = helper.GetTypeReference(value.template_parametres[int_i]).tp;
                }
                mi = mi.MakeGenericMethod(type_arr);
            }
            il.EmitCall(OpCodes.Call, mi, null);
            if (tmp_dot)
            {
                //MethodInfo mi = value.static_method.method_info;
                if ((mi.ReturnType.IsValueType || mi.ReturnType.IsGenericParameter) && !NETGeneratorTools.IsPointer(mi.ReturnType))
                {
                    LocalBuilder lb = il.DeclareLocal(mi.ReturnType);
                    il.Emit(OpCodes.Stloc, lb);
                    il.Emit(OpCodes.Ldloca, lb);
                }
                is_dot_expr = tmp_dot;
            }
            if (mi.DeclaringType == TypeFactory.ArrayType && mi.Name == "Resize")
            {
                if (real_parameters.Length == 2)
                {
                    this.CreateInitCodeForUnsizedArray(il, real_parameters[0].type.element_type,
                        real_parameters[0], len_lb, start_index_lb);
                }
            }
            EmitFreePinnedVariables();
            if (mi.ReturnType == TypeFactory.VoidType)
                il.Emit(OpCodes.Nop);
        }

        private bool has_debug_conditional_attr(MethodInfo mi)
        {
            var attrs = mi.GetCustomAttributes(typeof(System.Diagnostics.ConditionalAttribute), true);
            if (attrs != null && attrs.Length > 0 && (attrs[0] as System.Diagnostics.ConditionalAttribute).ConditionString == "DEBUG")
                return true;
            return false;
        }

        //вызов откомпилированного метода
        public override void visit(SemanticTree.ICompiledMethodCallNode value)
        {
            IExpressionNode[] real_parameters = value.real_parameters;
            IParameterNode[] parameters = value.compiled_method.parameters;
            bool tmp_dot = is_dot_expr;
            is_dot_expr = true;
            //DarkStar Fixed: type t:=i.gettype();
            bool _box = value.obj.type.is_value_type && !value.compiled_method.method_info.DeclaringType.IsValueType;
            if (!_box && value.obj.conversion_type != null)
            	_box = value.obj.conversion_type.is_value_type;
            if (_box)
                is_dot_expr = false;
            value.obj.visit(this);
            if (value.obj.type.is_value_type && !value.compiled_method.method_info.DeclaringType.IsValueType)
            {
                il.Emit(OpCodes.Box, helper.GetTypeReference(value.obj.type).tp);
            }
            else if (value.obj.type.is_generic_parameter && !(value.obj is IAddressedExpressionNode))
            {
                LocalBuilder lb = il.DeclareLocal(helper.GetTypeReference(value.obj.type).tp);
                il.Emit(OpCodes.Stloc, lb);
                il.Emit(OpCodes.Ldloca, lb);
            }
            else if (value.obj.conversion_type != null && value.obj.conversion_type.is_value_type && !value.compiled_method.method_info.DeclaringType.IsValueType)
            {
            	il.Emit(OpCodes.Box, helper.GetTypeReference(value.obj.conversion_type).tp);
            }
            else if (_box && value.obj.type.is_value_type)
            {
                LocalBuilder lb = il.DeclareLocal(helper.GetTypeReference(value.obj.type).tp);
                il.Emit(OpCodes.Stloc, lb);
                il.Emit(OpCodes.Ldloca, lb);
            }
            is_dot_expr = false;
            EmitArguments(parameters, real_parameters);
            MethodInfo mi = value.compiled_method.method_info;
            if (value.compiled_method.comperehensive_type.is_value_type || !value.virtual_call && value.compiled_method.polymorphic_state == polymorphic_state.ps_virtual || value.compiled_method.polymorphic_state == polymorphic_state.ps_static)
            {
                il.EmitCall(OpCodes.Call, mi, null);
            }
            else
            {
                if (value.obj.type.is_generic_parameter)
                    il.Emit(OpCodes.Constrained, helper.GetTypeReference(value.obj.type).tp);
                il.EmitCall(OpCodes.Callvirt, mi, null);
            }

            EmitFreePinnedVariables();
            if (tmp_dot == true)
            {
                //MethodInfo mi = value.compiled_method.method_info;
                if ((mi.ReturnType.IsValueType || mi.ReturnType.IsGenericParameter) && !NETGeneratorTools.IsPointer(mi.ReturnType))
                {
                    LocalBuilder lb = il.DeclareLocal(mi.ReturnType);
                    il.Emit(OpCodes.Stloc, lb);
                    il.Emit(OpCodes.Ldloca, lb);
                }
            }
            else
            {
                is_dot_expr = false;
            }
            if (mi.ReturnType == TypeFactory.VoidType)
                il.Emit(OpCodes.Nop);
        }

        //вызов статического метода
        public override void visit(SemanticTree.ICommonStaticMethodCallNode value)
        {
            //if (save_debug_info)
            //MarkSequencePoint(value.Location);
            IExpressionNode[] real_parameters = value.real_parameters;
            MethInfo meth = helper.GetMethod(value.static_method);
            MethodInfo mi = meth.mi;
            bool tmp_dot = is_dot_expr;
            is_dot_expr = false;
            bool is_comp_gen = false;
            IParameterNode[] parameters = value.static_method.parameters;
            EmitArguments(parameters, real_parameters);
            il.EmitCall(OpCodes.Call, mi, null);
            if (tmp_dot)
            {
                if (mi.ReturnType.IsValueType && !NETGeneratorTools.IsPointer(mi.ReturnType))
                {
                    LocalBuilder lb = il.DeclareLocal(mi.ReturnType);
                    il.Emit(OpCodes.Stloc, lb);
                    il.Emit(OpCodes.Ldloca, lb);
                }
                is_dot_expr = tmp_dot;
            }
            else if (meth.is_ptr_ret_type && is_addr == false) il.Emit(OpCodes.Ldobj, helper.GetTypeReference(value.static_method.return_value_type).tp);
            if (mi.ReturnType == TypeFactory.VoidType)
                il.Emit(OpCodes.Nop);
        }

        private Hashtable mis = new Hashtable();

        private void AddToCompilerGenerated(MethodInfo mi)
        {
            mis[mi] = mi;
        }

        private bool IsArrayGetter(MethodInfo mi)
        {
            if (mis[mi] != null) return true;
            return false;
        }

        private bool CheckForCompilerGenerated(IExpressionNode expr)
        {
            if (save_debug_info)
                if (expr is ICommonMethodCallNode)
                {
                    ICommonMethodCallNode cmcn = expr as ICommonMethodCallNode;
                    if (IsArrayGetter(helper.GetMethod(cmcn.method).mi))
                    {
                        // MarkSequencePoint(il, 0xFeeFee, 1, 0xFeeFee, 1);
                        return true;
                    }
                    return false;
                }
            return false;
        }

        //вызов нестатического метода
        public override void visit(SemanticTree.ICommonMethodCallNode value)
        {
            MethInfo meth = helper.GetMethod(value.method);
            MethodInfo mi = meth.mi;
            IExpressionNode[] real_parameters = value.real_parameters;
            bool tmp_dot = is_dot_expr;
            if (!tmp_dot)
                is_dot_expr = true;
            value.obj.visit(this);
            if ((value.obj.type.is_value_type) && !value.method.common_comprehensive_type.is_value_type)
            {
                il.Emit(OpCodes.Box, helper.GetTypeReference(value.obj.type).tp);
            }
            else if (value.obj.type.is_generic_parameter && !(value.obj is IAddressedExpressionNode))
            {
                LocalBuilder lb = il.DeclareLocal(helper.GetTypeReference(value.obj.type).tp);
                il.Emit(OpCodes.Stloc, lb);
                il.Emit(OpCodes.Ldloca, lb);
            }
            else if (value.obj.conversion_type != null && value.obj.conversion_type.is_value_type && !value.method.common_comprehensive_type.is_value_type)
            {
            	il.Emit(OpCodes.Box, helper.GetTypeReference(value.obj.conversion_type).tp);
            }
            else if (value.obj.type.is_value_type && !(value.obj is IAddressedExpressionNode) && !(value.obj is IThisNode))
            {
                LocalBuilder lb = il.DeclareLocal(helper.GetTypeReference(value.obj.type).tp);
                il.Emit(OpCodes.Stloc, lb);
                il.Emit(OpCodes.Ldloca, lb);
            }
            is_dot_expr = false;
            //bool is_comp_gen = false;
            //bool need_fee = false;
            IParameterNode[] parameters = value.method.parameters;
            EmitArguments(parameters, real_parameters);
            //вызов метода
            //(ssyy) Функции размерных типов всегда вызываются через call
            if (value.method.comperehensive_type.is_value_type || !value.virtual_call && value.method.polymorphic_state == polymorphic_state.ps_virtual || value.method.polymorphic_state == polymorphic_state.ps_static /*|| !value.virtual_call || (value.method.polymorphic_state != polymorphic_state.ps_virtual && value.method.polymorphic_state != polymorphic_state.ps_virtual_abstract && !value.method.common_comprehensive_type.IsInterface)*/)
            {
                il.EmitCall(OpCodes.Call, mi, null);
            }
            else
            {
                if (value.obj.type.is_generic_parameter)
                    il.Emit(OpCodes.Constrained, helper.GetTypeReference(value.obj.type).tp);
                il.EmitCall(OpCodes.Callvirt, mi, null);
            }
            EmitFreePinnedVariables();
            if (tmp_dot == true)
            {
                //if (mi.ReturnType.IsValueType && !NETGeneratorTools.IsPointer(mi.ReturnType))
                //Для правильной работы шаблонов поменял условие (ssyy, 15.05.2009)
                if ((value.method.return_value_type != null && value.method.return_value_type.is_value_type /*|| value.method.return_value_type != null && value.method.return_value_type.is_generic_parameter*/) && !NETGeneratorTools.IsPointer(mi.ReturnType))
                {
                    LocalBuilder lb = mi.ReturnType.IsGenericParameter ?
                        il.DeclareLocal(helper.GetTypeReference(value.method.return_value_type).tp) :
                        il.DeclareLocal(mi.ReturnType);
                    il.Emit(OpCodes.Stloc, lb);
                    il.Emit(OpCodes.Ldloca, lb);
                }
            }
            else
            {
                is_dot_expr = false;
                if (meth.is_ptr_ret_type && is_addr == false) il.Emit(OpCodes.Ldobj, helper.GetTypeReference(value.method.return_value_type).tp);
            }
            if (value.last_result_function_call)
            {
                il.Emit(OpCodes.Ret);
            }
            if (mi.ReturnType == TypeFactory.VoidType)
                il.Emit(OpCodes.Nop);
        }

        bool CallCloneIfNeed(ILGenerator il, IParameterNode parameter, IExpressionNode expr)
        {
            TypeInfo ti = helper.GetTypeReference(parameter.type);
            if (ti != null && ti.clone_meth != null && parameter.parameter_type == parameter_type.value && !parameter.is_const &&
                parameter.type.type_special_kind != type_special_kind.base_set_type)
            {
                il.Emit(OpCodes.Call, ti.clone_meth);
                return true;
            }
            return false;
        }

        //вызов вложенной процедуры
        public override void visit(SemanticTree.ICommonNestedInFunctionFunctionCallNode value)
        {
            IExpressionNode[] real_parameters = value.real_parameters;
            //if (save_debug_info)
            //MarkSequencePoint(value.Location);

            MethInfo meth = helper.GetMethod(value.common_function);
            MethodInfo mi = meth.mi;
            bool tmp_dot = is_dot_expr;
            is_dot_expr = false;
            MethInfo cur_mi = null;
            int scope_off = 0;
            //если процедура вложена в метод, то кладем дополнительный параметр this
            if (meth.is_in_class == true)
                il.Emit(OpCodes.Ldarg_0);
            if (smi.Count > 0)
            {
                cur_mi = smi.Peek();
                scope_off = meth.num_scope - cur_mi.num_scope;
            }
            //вызываемой процедуре нужно передать верхнюю по отношению
            // к ней запись активации. Можно вызвать процедуру уровня -1, 0, 1, ...
            //относительно вызывающей функции. Проходимся по цепочке записей активации
            //чтобы достучаться до нужной.
            if (meth.nested == true)
            {
                il.Emit(OpCodes.Ldloc, cur_mi.frame);
                if (scope_off <= 0)
                {
                    scope_off = Math.Abs(scope_off) + 1;
                    for (int j = 0; j < scope_off; j++)
                    {
                        il.Emit(OpCodes.Ldfld, cur_mi.disp.parent);
                        cur_mi = cur_mi.up_meth;
                    }
                }
            }
            bool is_comp_gen = false;
            IParameterNode[] parameters = value.common_function.parameters;
            for (int i = 0; i < real_parameters.Length; i++)
            {
                if (parameters[i].parameter_type == parameter_type.var)
                    is_addr = true;
                ITypeNode ctn = real_parameters[i].type;
                TypeInfo ti = helper.GetTypeReference(ctn);
                
                //(ssyy) moved up
                ITypeNode tn2 = parameters[i].type;
                ICompiledTypeNode ctn2 = tn2 as ICompiledTypeNode;
                ITypeNode ctn3 = real_parameters[i].type;
                //(ssyy) 07.12.2007 При боксировке нужно вызывать Ldsfld вместо Ldsflda.
                //Дополнительная проверка введена именно для этого.
                bool box_awaited =
                    (ctn2 != null && ctn2.compiled_type == TypeFactory.ObjectType || tn2.IsInterface) && !(real_parameters[i] is SemanticTree.INullConstantNode) && (ctn3.is_value_type || ctn3.is_generic_parameter);

                if (ti != null && ti.clone_meth != null && ti.tp != null && ti.tp.IsValueType && !box_awaited && !parameters[i].is_const)
                    is_dot_expr = true;
                is_comp_gen = CheckForCompilerGenerated(real_parameters[i]);
                real_parameters[i].visit(this);
                is_dot_expr = false;
                CallCloneIfNeed(il, parameters[i], real_parameters[i]);
                if (box_awaited)
                    il.Emit(OpCodes.Box, helper.GetTypeReference(ctn3).tp);
                is_addr = false;
            }
            //if (save_debug_info && need_fee)
            // MarkSequencePoint(il, value.Location);
            //вызов процедуры
            il.EmitCall(OpCodes.Call, mi, null);
            EmitFreePinnedVariables();
            if (tmp_dot == true)
            {
                if (mi.ReturnType.IsValueType && !NETGeneratorTools.IsPointer(mi.ReturnType))
                {
                    LocalBuilder lb = il.DeclareLocal(mi.ReturnType);
                    il.Emit(OpCodes.Stloc, lb);
                    il.Emit(OpCodes.Ldloca, lb);
                }
                is_dot_expr = tmp_dot;
            }
            else
                if (meth.is_ptr_ret_type && is_addr == false) il.Emit(OpCodes.Ldobj, helper.GetTypeReference(value.common_function.return_value_type).tp);
            //if (is_stmt == true) il.Emit(OpCodes.Pop);
            if (mi.ReturnType == TypeFactory.VoidType)
                il.Emit(OpCodes.Nop);
        }

        //это не очень нравится - у некоторых
        private bool GenerateStandardFuncCall(ICommonNamespaceFunctionCallNode value, ILGenerator il)
        {
            IExpressionNode[] real_parameters = value.real_parameters;
            switch (value.namespace_function.SpecialFunctionKind)
            {
                case SpecialFunctionKind.NewArray:
                    //первый параметр - ITypeOfOperator
                    TypeInfo ti = helper.GetTypeReference(((ITypeOfOperator)real_parameters[0]).oftype);
                    int rank = (real_parameters[1] as IIntConstantNode).constant_value;

                    if (ti.tp.IsValueType && ti.init_meth != null || ti.is_arr || ti.is_set || ti.is_typed_file || ti.is_text_file || ti.tp == TypeFactory.StringType)
                    {
                        //value.real_parameters[1].visit(this);
                        if (rank == 0)
                        {
                            CreateUnsizedArray(il, ti, real_parameters[1]);
                        }
                        else if (rank == 1)
                        {
                            real_parameters[2].visit(this);
                            LocalBuilder size = NETGeneratorTools.CreateLocal(il, helper.GetTypeReference(real_parameters[2].type).tp);
                            CreateUnsizedArray(il, ti, size);
                            CreateInitCodeForUnsizedArray(il, ti, ((ITypeOfOperator)real_parameters[0]).oftype, size);
                        }
                        else
                        {
                            if (real_parameters.Length <= 2 + rank)
                            {
                                CreateNDimUnsizedArray(il, ti, ((ITypeOfOperator)real_parameters[0]).oftype, rank, real_parameters);
                                List<IExpressionNode> prms = new List<IExpressionNode>();
                                prms.AddRange(real_parameters);
                                prms.RemoveRange(0, 2);
                                CreateInitCodeForNDimUnsizedArray(il, ti, ((ITypeOfOperator)real_parameters[0]).oftype, rank, prms.ToArray());
                            }
                        }
                    }
                    else
                    {
                        if (rank == 0)
                        {
                            CreateUnsizedArray(il, ti, real_parameters[1]);
                        }
                        else if (rank == 1)
                        {
                            CreateUnsizedArray(il, ti, real_parameters[2]);
                        }
                        else
                        {
                            if (real_parameters.Length <= 2 + rank)
                                CreateNDimUnsizedArray(il, ti, ((ITypeOfOperator)real_parameters[0]).oftype, rank, real_parameters);
                        }
                    }
                    if (real_parameters.Length > 2 + rank)
                        if (rank == 1)
                            InitializeUnsizedArray(il, ti, ((ITypeOfOperator)real_parameters[0]).oftype, real_parameters, rank);
                        else if (rank != 0)
                            InitializeNDimUnsizedArray(il, ti, ((ITypeOfOperator)real_parameters[0]).oftype, real_parameters, rank);
                    return true;
            }
            return false;
        }

        private MethInfo MakeStandardFunc(ICommonNamespaceFunctionCallNode value)
        {
            ICommonNamespaceFunctionNode func = value.namespace_function;
            MethodBuilder methodb;
            ParameterBuilder pb;
            ILGenerator il;
            MethInfo mi;
            switch (func.SpecialFunctionKind)
            {
                case SpecialFunctionKind.New:
                    methodb = cur_type.DefineMethod(func.name, MethodAttributes.Public | MethodAttributes.Static, null, new Type[2] { Type.GetType("System.Void*&"), TypeFactory.Int32Type });
                    pb = methodb.DefineParameter(1, ParameterAttributes.None, "ptr");
                    pb = methodb.DefineParameter(2, ParameterAttributes.None, "size");
                    il = methodb.GetILGenerator();
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Call, typeof(Marshal).GetMethod("AllocHGlobal", new Type[1] { TypeFactory.Int32Type }));
                    il.Emit(OpCodes.Stind_I);
                    LocalBuilder lb = il.DeclareLocal(typeof(byte[]));
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Newarr, TypeFactory.ByteType);
                    il.Emit(OpCodes.Stloc, lb);
                    il.Emit(OpCodes.Ldloc, lb);
                    il.Emit(OpCodes.Ldc_I4_0);
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldind_I);
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Call, typeof(Marshal).GetMethod("Copy", new Type[4] { typeof(byte[]), TypeFactory.Int32Type, TypeFactory.IntPtr, TypeFactory.Int32Type }));
                    il.Emit(OpCodes.Ret);
                    mi = helper.AddMethod(func, methodb);
                    mi.stand = true;
                    return mi;
                case SpecialFunctionKind.Dispose:
                    methodb = cur_type.DefineMethod(func.name, MethodAttributes.Public | MethodAttributes.Static, null, new Type[2] { Type.GetType("System.Void*&"), TypeFactory.Int32Type });
                    pb = methodb.DefineParameter(1, ParameterAttributes.None, "ptr");
                    pb = methodb.DefineParameter(2, ParameterAttributes.None, "size");
                    il = methodb.GetILGenerator();
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldind_I);
                    il.Emit(OpCodes.Call, typeof(Marshal).GetMethod("FreeHGlobal", new Type[1] { typeof(IntPtr) }));
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldnull);
                    il.Emit(OpCodes.Stind_I);
                    il.Emit(OpCodes.Ret);
                    mi = helper.AddMethod(func, methodb);
                    mi.stand = true;
                    return mi;
            }
            return null;
        }

        private void FixPointer()
        {
            il.Emit(OpCodes.Call, TypeFactory.GCHandleAlloc);
            il.Emit(OpCodes.Pop);
        }

        //вызов глобальной процедуры
        public override void visit(SemanticTree.ICommonNamespaceFunctionCallNode value)
        {
            MethInfo meth = helper.GetMethod(value.namespace_function);
            IExpressionNode[] real_parameters = value.real_parameters;
            //если это стандартная (New или Dispose)
            if (meth == null || meth.stand == true)
            {
                if (GenerateStandardFuncCall(value, il))
                    return;
                if (meth == null)
                    meth = MakeStandardFunc(value);
                IRefTypeNode rtn = (IRefTypeNode)real_parameters[0].type;
                TypeInfo ti = helper.GetTypeReference(rtn.pointed_type);
                //int size = 0;
                //if (ti.tp.IsPointer == true) size = Marshal.SizeOf(TypeFactory.Int32Type);
                //else size = GetTypeSize(ti.tp, rtn.pointed_type);
                is_addr = true;
                real_parameters[0].visit(this);
                is_addr = false;
                //il.Emit(OpCodes.Ldc_I4, size);
                //NETGeneratorTools.LdcIntConst(il, size);
                PushSize(ti.tp);
                il.Emit(OpCodes.Call, meth.mi);
                if (value.namespace_function.SpecialFunctionKind == SpecialFunctionKind.New)
                {
                    ITypeNode tn = (real_parameters[0].type as IRefTypeNode).pointed_type;
                    if (tn.type_special_kind == type_special_kind.array_wrapper)
                    {
                        ICommonTypeNode ctn = tn as ICommonTypeNode;
                        real_parameters[0].visit(this);
                        il.Emit(OpCodes.Newobj, helper.GetTypeReference(ctn).def_cnstr);
                        il.Emit(OpCodes.Stind_Ref);
                        real_parameters[0].visit(this);
                        il.Emit(OpCodes.Ldind_Ref);
                        FixPointer();
                    }
                    else if (tn.type_special_kind == type_special_kind.set_type)
                    {
                        ICommonNamespaceFunctionNode cnfn = PascalABCCompiler.SystemLibrary.SystemLibInitializer.TypedSetInitProcedureWithBounds.sym_info as ICommonNamespaceFunctionNode;
                        real_parameters[0].visit(this);
                        IConstantNode cn1 = (tn as ICommonTypeNode).lower_value;
                        IConstantNode cn2 = (tn as ICommonTypeNode).upper_value;
                        if (cn1 != null && cn2 != null)
                        {
                            cn1.visit(this);
                            il.Emit(OpCodes.Box, helper.GetTypeReference(cn1.type).tp);
                            cn2.visit(this);
                            il.Emit(OpCodes.Box, helper.GetTypeReference(cn2.type).tp);
                        }
                        else
                        {
                            il.Emit(OpCodes.Ldnull);
                            il.Emit(OpCodes.Ldnull);
                        }
                        il.Emit(OpCodes.Newobj, ti.def_cnstr);
                        il.Emit(OpCodes.Stind_Ref);
                        real_parameters[0].visit(this);
                        il.Emit(OpCodes.Ldind_Ref);
                        FixPointer();
                    }
                    else if (tn.type_special_kind == type_special_kind.typed_file)
                    {
                        NETGeneratorTools.PushTypeOf(il, helper.GetTypeReference((tn as ICommonTypeNode).element_type).tp);
                        il.Emit(OpCodes.Newobj, ti.def_cnstr);
                        il.Emit(OpCodes.Stind_Ref);
                        real_parameters[0].visit(this);
                        il.Emit(OpCodes.Ldind_Ref);
                        FixPointer();
                    }
                    else if (tn.type_special_kind == type_special_kind.short_string)
                    {
                        real_parameters[0].visit(this);
                        il.Emit(OpCodes.Ldstr, "");
                        il.Emit(OpCodes.Stind_Ref);
                        real_parameters[0].visit(this);
                        il.Emit(OpCodes.Ldind_Ref);
                        FixPointer();
                    }
                    else if (tn.is_value_type && tn is ICommonTypeNode)
                    {
                        TypeInfo ti2 = helper.GetTypeReference(tn);
                        if (ti2.init_meth != null)
                        {
                            real_parameters[0].visit(this);
                            il.Emit(OpCodes.Call, ti2.init_meth);
                        }
                        if (ti2.fix_meth != null)
                        {
                            real_parameters[0].visit(this);
                            il.Emit(OpCodes.Call, ti2.fix_meth);
                        }
                    }
                }
                return;
            }
            bool tmp_dot = is_dot_expr;
            is_dot_expr = false;
            
            MethodInfo mi = meth.mi;
            IParameterNode[] parameters = value.namespace_function.parameters;
            EmitArguments(parameters, real_parameters);
            il.EmitCall(OpCodes.Call, mi, null);
            EmitFreePinnedVariables();
            if (tmp_dot == true)
            {
                if (mi.ReturnType.IsValueType && !NETGeneratorTools.IsPointer(mi.ReturnType))
                {
                    LocalBuilder lb = il.DeclareLocal(mi.ReturnType);
                    il.Emit(OpCodes.Stloc, lb);
                    il.Emit(OpCodes.Ldloca, lb);
                }
                is_dot_expr = tmp_dot;
            }
            else
                if (meth.is_ptr_ret_type && is_addr == false)
                    il.Emit(OpCodes.Ldobj, helper.GetTypeReference(value.namespace_function.return_value_type).tp);
            if (mi.ReturnType == TypeFactory.VoidType)
                il.Emit(OpCodes.Nop);
            //if (is_stmt == true) il.Emit(OpCodes.Pop);
        }
        
        private void EmitArguments(IParameterNode[] parameters, IExpressionNode[] real_parameters)
        {
        	bool is_comp_gen = false;
        	for (int i = 0; i < real_parameters.Length; i++)
            {
        		if (real_parameters[i] is INullConstantNode && parameters[i].type.is_nullable_type)
                {
        			Type tp = helper.GetTypeReference(parameters[i].type).tp;
        			LocalBuilder lb = il.DeclareLocal(tp);
        			il.Emit(OpCodes.Ldloca, lb);
        			il.Emit(OpCodes.Initobj, tp);
        			il.Emit(OpCodes.Ldloc, lb);
        			continue;
        		}
                if (parameters[i].parameter_type == parameter_type.var)
                    is_addr = true;
                ITypeNode ctn = real_parameters[i].type;
                TypeInfo ti = null;

                //(ssyy) moved up
                ITypeNode tn2 = parameters[i].type;
                ICompiledTypeNode ctn2 = tn2 as ICompiledTypeNode;
                ITypeNode ctn3 = real_parameters[i].type;
                ITypeNode ctn4 = real_parameters[i].conversion_type;
                bool use_stn4 = false;
                //(ssyy) 07.12.2007 При боксировке нужно вызывать Ldsfld вместо Ldsflda.
                //Дополнительная проверка введена именно для этого.
                bool box_awaited =
                    (ctn2 != null && ctn2.compiled_type == TypeFactory.ObjectType || tn2.IsInterface) && !(real_parameters[i] is SemanticTree.INullConstantNode) 
                	&& (ctn3.is_value_type || ctn3.is_generic_parameter);
                if (!box_awaited && (ctn2 != null && ctn2.compiled_type == TypeFactory.ObjectType || tn2.IsInterface) && !(real_parameters[i] is SemanticTree.INullConstantNode) 
                	&& ctn4 != null && ctn4.is_value_type)
                {
                	box_awaited = true;
                	use_stn4 = true;
                }
                	
                if (!(real_parameters[i] is INullConstantNode))
                {
                    ti = helper.GetTypeReference(ctn);
                    if (ti.clone_meth != null && ti.tp != null && ti.tp.IsValueType && !box_awaited && !parameters[i].is_const)
                        is_dot_expr = true;
                }
                is_comp_gen = CheckForCompilerGenerated(real_parameters[i]);
                real_parameters[i].visit(this);
                is_dot_expr = false;
                CallCloneIfNeed(il, parameters[i], real_parameters[i]);
                if (box_awaited)
                {
                	if (use_stn4)
                		il.Emit(OpCodes.Box, helper.GetTypeReference(ctn4).tp);
                	else
                    	il.Emit(OpCodes.Box, helper.GetTypeReference(ctn3).tp);
                }
                is_addr = false;
            }
        }
        
        private void EmitFreePinnedVariables()
        {
            /*foreach (LocalBuilder lb in pinned_variables)
            {
                il.Emit(OpCodes.Ldloca, lb);
                il.Emit(OpCodes.Call, TypeFactory.GCHandleFreeMethod);
            }
            pinned_variables.Clear();*/
        }

        //присваивание глобальной переменной
        private void AssignToNamespaceVariableNode(IExpressionNode to, IExpressionNode from)
        {
            INamespaceVariableReferenceNode var = (INamespaceVariableReferenceNode)to;
            //получаем переменную
            VarInfo vi = helper.GetVariable(var.variable);
            FieldBuilder fb = vi.fb;
            TypeInfo ti = helper.GetTypeReference(to.type);
            if (to.type.is_value_type)
            {
                //ti = helper.GetTypeReference(to.type);
                if (ti.assign_meth != null || from is INullConstantNode && to.type.is_nullable_type)
                    il.Emit(OpCodes.Ldsflda, fb);
            }
            else if (to.type.type_special_kind == type_special_kind.set_type && !in_var_init)
            {
                il.Emit(OpCodes.Ldsfld, fb);
                from.visit(this);
                il.Emit(OpCodes.Call, ti.assign_meth);
                return;
            }
            else ti = null;
            if (from is INullConstantNode && to.type.is_nullable_type)
            {
            	il.Emit(OpCodes.Initobj, ti.tp);
            	return;
            }
            //что присвоить
            from.visit(this);
            if (ti != null && ti.assign_meth != null)
            {
                il.Emit(OpCodes.Call, ti.assign_meth);
                return;
            }
            
            //это если например переменной типа object присваивается число
            EmitBox(from, fb.FieldType);

            CheckArrayAssign(to, from, il);

            //присваиваем
            il.Emit(OpCodes.Stsfld, fb);
        }

        private bool TypeIsEnum(Type T)
        {
            if (T.IsGenericType || T.IsGenericTypeDefinition || T.IsGenericParameter)
                return false;
            return T.IsEnum;
        }


        private bool TypeIsInterface(Type T)
        {
            return !T.IsPointer && !T.IsGenericParameter && T.IsInterface;
        }

        private bool TypeIsClass(Type T)
        {
            if (T.IsGenericType)
            {
                return T.GetGenericTypeDefinition().IsClass;
            }
            else
            {
                return T.IsClass;
            }
        }

        private bool EmitBox(IExpressionNode from, Type LocalType)
        {
            if ((from.type.is_value_type || from.type.is_generic_parameter) && !(from is SemanticTree.INullConstantNode) && (LocalType == TypeFactory.ObjectType || TypeIsInterface(LocalType)))
            {
                il.Emit(OpCodes.Box, helper.GetTypeReference(from.type).tp);//упаковка
                return true;
            }
            if (from.conversion_type != null && from.conversion_type.is_value_type && !(from is SemanticTree.INullConstantNode) && (LocalType == TypeFactory.ObjectType || TypeIsInterface(LocalType)))
            {
            	il.Emit(OpCodes.Box, helper.GetTypeReference(from.conversion_type).tp);
            }
            return false;
        }

        internal void CheckArrayAssign(IExpressionNode to, IExpressionNode from, ILGenerator il)
        {
            //DarkStar Add 07.11.06 02:32
            //Массив присваиваем массиву=>надо вызвать копирование
            TypeInfo ti_l = helper.GetTypeReference(to.type);
            TypeInfo ti_r = helper.GetTypeReference(from.type);
            if (ti_l.is_arr && ti_r.is_arr)
            {
                il.Emit(OpCodes.Call, ti_r.clone_meth);
            }
            else if (ti_l.is_set && ti_r.is_set)
            {
                //if (!(from is ICommonConstructorCall))
                //il.Emit(OpCodes.Callvirt, ti_r.clone_meth);
            }
        }

        //присваивание локальной переменной
        private void AssignToLocalVariableNode(IExpressionNode to, IExpressionNode from)
        {
            IReferenceNode var = (IReferenceNode)to;
            VarInfo vi = helper.GetVariable(var.Variable);
            if (vi.kind == VarKind.vkLocal)
            {
                LocalBuilder lb = vi.lb;
                TypeInfo ti = helper.GetTypeReference(to.type);
                if (to.type.is_value_type)
                {
                    //ti = helper.GetTypeReference(to.type);
                    if (ti.assign_meth != null || from is INullConstantNode && to.type.is_nullable_type)
                        il.Emit(OpCodes.Ldloca, lb);
                    
                }
                else if (to.type.type_special_kind == type_special_kind.set_type && !in_var_init)
                {
                    il.Emit(OpCodes.Ldloc, lb);
                    from.visit(this);
                    il.Emit(OpCodes.Call, ti.assign_meth);
                    return;
                }
                else ti = null;
                if (from is INullConstantNode && to.type.is_nullable_type)
                {
                	il.Emit(OpCodes.Initobj, ti.tp);
                	return;
                }
                //что присвоить
                from.visit(this);
                if (ti != null && ti.assign_meth != null)
                {
                    il.Emit(OpCodes.Call, ti.assign_meth);
                    return;
                }
                EmitBox(from, lb.LocalType);
                CheckArrayAssign(to, from, il);
                il.Emit(OpCodes.Stloc, lb);
            }
            else if (vi.kind == VarKind.vkNonLocal)
            {
                FieldBuilder fb = vi.fb;
                MethInfo cur_mi = smi.Peek();
                int dist = smi.Peek().num_scope - vi.meth.num_scope;
                il.Emit(OpCodes.Ldloc, cur_mi.frame);
                for (int i = 0; i < dist; i++)
                {
                    il.Emit(OpCodes.Ldfld, cur_mi.disp.parent);
                    cur_mi = cur_mi.up_meth;
                }
                TypeInfo ti = helper.GetTypeReference(to.type);
                if (to.type.is_value_type)
                {
                    //ti = helper.GetTypeReference(to.type);
                    if (ti.assign_meth != null || from is INullConstantNode && to.type.is_nullable_type) 
                    	il.Emit(OpCodes.Ldflda, fb);
                }
                else if (to.type.type_special_kind == type_special_kind.set_type && !in_var_init)
                {
                    il.Emit(OpCodes.Ldfld, fb);
                    from.visit(this);
                    il.Emit(OpCodes.Call, ti.assign_meth);
                    return;
                }
                else ti = null;
                if (from is INullConstantNode && to.type.is_nullable_type)
                {
                	il.Emit(OpCodes.Initobj, ti.tp);
                	return;
                }
                //что присвоить
                from.visit(this);
                if (ti != null && ti.assign_meth != null)
                {
                    il.Emit(OpCodes.Call, ti.assign_meth);
                    return;
                }
                
                EmitBox(from, fb.FieldType);
                CheckArrayAssign(to, from, il);
                il.Emit(OpCodes.Stfld, fb);
            }
        }

        private void BoxAssignToParameter(IExpressionNode to, IExpressionNode from)
        {
            ICompiledTypeNode ctn2 = to.type as ICompiledTypeNode;
            if ((from.type.is_value_type || from.type.is_generic_parameter) && ctn2 != null && (ctn2.compiled_type == TypeFactory.ObjectType || ctn2.IsInterface))
            {
                il.Emit(OpCodes.Box, helper.GetTypeReference(from.type).tp);
            }
            else if (from.conversion_type != null && (from.type.is_value_type || from.type.is_generic_parameter) && ctn2 != null && (ctn2.compiled_type == TypeFactory.ObjectType || ctn2.IsInterface))
            {
            	il.Emit(OpCodes.Box, helper.GetTypeReference(from.conversion_type).tp);
            }
            CheckArrayAssign(to, from, il);
        }

        private void StoreParameterByReference(Type t)
        {
            NETGeneratorTools.PushStind(il, t);
        }

        //присвоение параметру
        //полная бяка, например нужно присвоить нелокальному var-параметру какое-то значение
        private void AssignToParameterNode(IExpressionNode to, IExpressionNode from)
        {
            ICommonParameterReferenceNode var = (ICommonParameterReferenceNode)to;
            ParamInfo pi = helper.GetParameter(var.parameter);
            if (pi.kind == ParamKind.pkNone)//если параметр локальный
            {
                ParameterBuilder pb = pi.pb;
                //byte pos = (byte)(pb.Position-1);
                //***********************Kolay modified**********************
                ushort pos = (ushort)(pb.Position - 1);
                if (is_constructor || cur_meth.IsStatic == false)
                    pos = (ushort)pb.Position;
                else
                    pos = (ushort)(pb.Position - 1);
                //***********************End of Kolay modified**********************
                if (var.parameter.parameter_type == parameter_type.value)
                {
                    TypeInfo ti = helper.GetTypeReference(to.type);
                    if (to.type.is_value_type)
                    {
                        if (ti.assign_meth != null || from is INullConstantNode && to.type.is_nullable_type) 
                        	il.Emit(OpCodes.Ldarga, pos);
                    }
                    else if (to.type.type_special_kind == type_special_kind.set_type)
                    {
                        il.Emit(OpCodes.Ldarg, pos);
                        from.visit(this);
                        il.Emit(OpCodes.Call, ti.assign_meth);
                        return;
                    }
                    else ti = null;
                    if (from is INullConstantNode && to.type.is_nullable_type)
                    {
                    	il.Emit(OpCodes.Initobj, ti.tp);
                    	return;
                    }
                    //что присвоить
                    from.visit(this);
                    if (ti != null && ti.assign_meth != null)
                    {
                        il.Emit(OpCodes.Call, ti.assign_meth);
                        return;
                    }
                    
                    BoxAssignToParameter(to, from);
                    //il.Emit(OpCodes.Dup);
                    if (pos <= 255) 
                    	il.Emit(OpCodes.Starg_S, pos);
                    else 
                    	il.Emit(OpCodes.Starg, pos);
                }
                else
                {
                    TypeInfo ti = helper.GetTypeReference(to.type);
                    if (to.type.is_value_type)
                    {
                        //ti = helper.GetTypeReference(to.type);
                        if (ti.assign_meth != null)
                        {
                            //здесь надо быть внимательнее
                            il.Emit(OpCodes.Ldarg, pos);
                            from.visit(this);
                            il.Emit(OpCodes.Call, ti.assign_meth);
                            return;
                        }
                        if (from is INullConstantNode && to.type.is_nullable_type)
                    	{
                        	il.Emit(OpCodes.Ldarg, pos);
                    		il.Emit(OpCodes.Initobj, ti.tp);
                    		return;
                    	}
                    }
                    else if (to.type.type_special_kind == type_special_kind.set_type)
                    {
                        il.Emit(OpCodes.Ldarg, pos);
                        il.Emit(OpCodes.Ldind_Ref);
                        from.visit(this);
                        il.Emit(OpCodes.Call, ti.assign_meth);
                        return;
                    }
                    else ti = null;
                    PushParameter(pos);
                    from.visit(this);
                    BoxAssignToParameter(to, from);
                    //il.Emit(OpCodes.Dup);
                    ti = helper.GetTypeReference(var.type);
                    StoreParameterByReference(ti.tp);
                }
            }
            else//иначе нелокальный
            {
                FieldBuilder fb = pi.fb;
                MethInfo cur_mi = (MethInfo)smi.Peek();
                int dist = ((MethInfo)smi.Peek()).num_scope - pi.meth.num_scope;
                il.Emit(OpCodes.Ldloc, cur_mi.frame);
                for (int i = 0; i < dist; i++)
                {
                    il.Emit(OpCodes.Ldfld, cur_mi.disp.parent);
                    cur_mi = cur_mi.up_meth;
                }

                if (var.parameter.parameter_type == parameter_type.value)
                {
                    TypeInfo ti = helper.GetTypeReference(to.type);
                    if (to.type.is_value_type)
                    {
                        //ti = helper.GetTypeReference(to.type);
                        if (ti.assign_meth != null || from is INullConstantNode && to.type.is_nullable_type) 
                        	il.Emit(OpCodes.Ldflda, fb);
                    }
                    else if (to.type.type_special_kind == type_special_kind.set_type)
                    {
                        il.Emit(OpCodes.Ldfld, fb);
                        from.visit(this);
                        il.Emit(OpCodes.Call, ti.assign_meth);
                        return;
                    }
                    else ti = null;
                    if (from is INullConstantNode && to.type.is_nullable_type)
                    {
                    	il.Emit(OpCodes.Initobj, ti.tp);
                    	return;
                    }
                    //что присвоить
                    from.visit(this);
                    if (ti != null && ti.assign_meth != null)
                    {
                        il.Emit(OpCodes.Call, ti.assign_meth);
                        return;
                    }
                    
                    BoxAssignToParameter(to, from);
                    //il.Emit(OpCodes.Dup);
                    il.Emit(OpCodes.Stfld, fb);
                }
                else
                {
                    TypeInfo ti = helper.GetTypeReference(to.type);
                    il.Emit(OpCodes.Ldfld, fb);

                    if (to.type.is_value_type)
                    {
                        //ti = helper.GetTypeReference(to.type);
                        if (ti.assign_meth != null)
                        {
                            il.Emit(OpCodes.Call, ti.assign_meth);
                            return;
                        }
                        if (from is INullConstantNode && to.type.is_nullable_type)
                    	{
                    		il.Emit(OpCodes.Initobj, ti.tp);
                    		return;
                    	}
                    }
                    else if (to.type.type_special_kind == type_special_kind.set_type)
                    {
                        //il.Emit(OpCodes.Ldarg, pos);
                        //il.Emit(OpCodes.Ldind_Ref);
                        //from.visit(this);
                        il.Emit(OpCodes.Ldind_Ref);
                        from.visit(this);
                        il.Emit(OpCodes.Call, ti.assign_meth);
                        return;
                    }
                    else ti = null;
                    from.visit(this);
                    BoxAssignToParameter(to, from);
                    //il.Emit(OpCodes.Dup);
                    ti = helper.GetTypeReference(var.type);
                    StoreParameterByReference(ti.tp);
                }
            }
        }

        //присвоение полю
        private void AssignToField(IExpressionNode to, IExpressionNode from)
        {
            ICommonClassFieldReferenceNode value = (ICommonClassFieldReferenceNode)to;
            FldInfo fi_info = helper.GetField(value.field);
            FieldInfo fi = fi_info.fi;
            is_dot_expr = true;
            has_dereferences = false;
            value.obj.visit(this);
            bool has_dereferences_tmp = has_dereferences;
            has_dereferences = false;
            is_dot_expr = false;
            TypeInfo ti = helper.GetTypeReference(to.type);
            if (to.type.is_value_type)
            {
                if (ti.assign_meth != null || from is INullConstantNode && to.type.is_nullable_type)
                    il.Emit(OpCodes.Ldflda, fi);
            }
            else if (to.type.type_special_kind == type_special_kind.set_type && !in_var_init)
            {
                il.Emit(OpCodes.Ldfld, fi);
                from.visit(this);
                il.Emit(OpCodes.Call, ti.assign_meth);
                return;
            }
            else ti = null;
            if (from is INullConstantNode && to.type.is_nullable_type)
            {
            	il.Emit(OpCodes.Initobj, ti.tp);
                return;
           	}
            //что присвоить
            from.visit(this);
            if (ti != null && ti.assign_meth != null)
            {
                il.Emit(OpCodes.Call, ti.assign_meth);
                if (has_dereferences_tmp)
                {
                    if (ti.fix_meth != null)
                    {
                        is_dot_expr = true;
                        value.obj.visit(this);
                        is_dot_expr = false;
                        il.Emit(OpCodes.Ldflda, fi);
                        il.Emit(OpCodes.Call, ti.fix_meth);
                    }
                }
                return;
            }
            
            EmitBox(from, fi_info.field_type);
            CheckArrayAssign(to, from, il);
            il.Emit(OpCodes.Stfld, fi);
            if (has_dereferences_tmp && TypeNeedToFix(to.type))
            {
                is_dot_expr = true;
                value.obj.visit(this);
                is_dot_expr = false;
                il.Emit(OpCodes.Ldfld, fi);
                FixPointer();
            }
        }

        //присвоение статическому полю
        private void AssignToStaticField(IExpressionNode to, IExpressionNode from)
        {
            IStaticCommonClassFieldReferenceNode value = (IStaticCommonClassFieldReferenceNode)to;
            FieldInfo fi = helper.GetField(value.static_field).fi;
            TypeInfo ti = helper.GetTypeReference(to.type);
            if (to.type.is_value_type)
            {
                //ti = helper.GetTypeReference(to.type);
                if (ti.assign_meth != null || from is INullConstantNode && to.type.is_nullable_type) 
                	il.Emit(OpCodes.Ldsflda, fi);
            }
            else if (to.type.type_special_kind == type_special_kind.set_type)
            {
                il.Emit(OpCodes.Ldsfld, fi);
                from.visit(this);
                il.Emit(OpCodes.Call, ti.assign_meth);
                return;
            }
            else ti = null;
            if (from is INullConstantNode && to.type.is_nullable_type)
            {
            	il.Emit(OpCodes.Initobj, ti.tp);
                return;
           	}
            //что присвоить
            from.visit(this);
            if (ti != null && ti.assign_meth != null)
            {
                il.Emit(OpCodes.Call, ti.assign_meth);
                return;
            }
            if (ti != null)
                EmitBox(from, ti.tp);
            else
                EmitBox(from, fi.FieldType);
            CheckArrayAssign(to, from, il);
            il.Emit(OpCodes.Stsfld, fi);
        }

        private void AssignToCompiledField(IExpressionNode to, IExpressionNode from)
        {
            ICompiledFieldReferenceNode value = (ICompiledFieldReferenceNode)to;
            FieldInfo fi = value.field.compiled_field;
            is_dot_expr = true;
            value.obj.visit(this);
            is_dot_expr = false;
            from.visit(this);
            EmitBox(from, fi.FieldType);
            il.Emit(OpCodes.Stfld, fi);
        }

        private void AssignToStaticCompiledField(IExpressionNode to, IExpressionNode from)
        {
            IStaticCompiledFieldReferenceNode value = (IStaticCompiledFieldReferenceNode)to;
            FieldInfo fi = value.static_field.compiled_field;
            from.visit(this);
            EmitBox(from, fi.FieldType);
            il.Emit(OpCodes.Stsfld, fi);
        }

        //присвоение элементу массива
        //
        private void AssignToSimpleArrayNode(IExpressionNode to, IExpressionNode from)
        {
            ISimpleArrayIndexingNode value = (ISimpleArrayIndexingNode)to;
            TypeInfo ti = helper.GetTypeReference(value.array.type);
            ISimpleArrayNode arr_type = value.array.type as ISimpleArrayNode;
            TypeInfo elem_ti = null;
            if (arr_type != null)
                elem_ti = helper.GetTypeReference(arr_type.element_type);
            else if (value.array.type.type_special_kind == type_special_kind.array_kind && value.array.type is ICommonTypeNode)
                elem_ti = helper.GetTypeReference(value.array.type.element_type);
            Type elem_type = null;
            if (elem_ti != null)
                elem_type = elem_ti.tp;
            else
                elem_type = ti.tp.GetElementType();
            value.array.visit(this);
            MethodInfo get_meth = null;
            MethodInfo addr_meth = null;
            MethodInfo set_meth = null;
            LocalBuilder index_lb = null;
            if (value.indices == null)
            {
                value.index.visit(this);
                if (from is IBasicFunctionCallNode && (from as IBasicFunctionCallNode).real_parameters[0] == to && current_index_lb == null)
                {
                    index_lb = il.DeclareLocal(helper.GetTypeReference(value.index.type).tp);
                    il.Emit(OpCodes.Stloc, index_lb);
                    il.Emit(OpCodes.Ldloc, index_lb);
                    current_index_lb = index_lb;

                }
            }
            else
            {
                if (value.array.type is ICompiledTypeNode)
                {
                    get_meth = ti.tp.GetMethod("Get");
                    addr_meth = ti.tp.GetMethod("Address");
                    set_meth = ti.tp.GetMethod("Set");
                }
                else
                {
                    List<Type> lst = new List<Type>();
                    for (int i = 0; i < value.indices.Length; i++)
                        lst.Add(TypeFactory.Int32Type);
                    get_meth = mb.GetArrayMethod(ti.tp, "Get", CallingConventions.HasThis, elem_type, lst.ToArray());
                    addr_meth = mb.GetArrayMethod(ti.tp, "Address", CallingConventions.HasThis, elem_type.MakeByRefType(), lst.ToArray());
                    lst.Add(elem_type);
                    set_meth = mb.GetArrayMethod(ti.tp, "Set", CallingConventions.HasThis, TypeFactory.VoidType, lst.ToArray());
                }
                for (int i = 0; i < value.indices.Length; i++)
                    value.indices[i].visit(this);
            }
            if (elem_type.IsValueType && !TypeFactory.IsStandType(elem_type) && !TypeIsEnum(elem_type) || to.type.is_nullable_type)
            {
                if (value.indices == null)
                    il.Emit(OpCodes.Ldelema, elem_type);
            }
            else if (elem_ti != null && elem_ti.assign_meth != null)
            {
                if (value.indices == null)
                    il.Emit(OpCodes.Ldelem_Ref);
                else
                    il.Emit(OpCodes.Call, get_meth);
            }
            if (from is INullConstantNode && to.type.is_nullable_type)
            {
                il.Emit(OpCodes.Initobj, elem_type);
                return;
            }
            from.visit(this);
            if (elem_ti != null && elem_ti.assign_meth != null)
            {
                il.Emit(OpCodes.Call, elem_ti.assign_meth);
                return;
            }
            ICompiledTypeNode ctn2 = to.type as ICompiledTypeNode;
            if ((from.type.is_value_type || from.type.is_generic_parameter) && ctn2 != null && (ctn2.compiled_type == TypeFactory.ObjectType || ctn2.IsInterface))
            {
                il.Emit(OpCodes.Box, helper.GetTypeReference(from.type).tp);
            }
            else if ((from.type.is_value_type || from.type.is_generic_parameter) && to.type.IsInterface)
            {
                il.Emit(OpCodes.Box, helper.GetTypeReference(from.type).tp);
            }
            else if (from.conversion_type != null && (from.conversion_type.is_value_type || from.conversion_type.is_generic_parameter) && ctn2 != null && (ctn2.compiled_type == TypeFactory.ObjectType || ctn2.IsInterface))
            {
                il.Emit(OpCodes.Box, helper.GetTypeReference(from.conversion_type).tp);
            }

            CheckArrayAssign(to, from, il);
            if (value.indices == null)
                NETGeneratorTools.PushStelem(il, elem_type);
            else
                il.Emit(OpCodes.Call, set_meth);
        }

        //присвоение например a^ := 1
        private void AssignToDereferenceNode(IExpressionNode to, IExpressionNode from)
        {
            IDereferenceNode value = (IDereferenceNode)to;
            TypeInfo ti = helper.GetTypeReference(to.type);
            value.derefered_expr.visit(this);
            if (ti != null && ti.assign_meth != null && !ti.tp.IsValueType)
                il.Emit(OpCodes.Ldind_Ref);
            from.visit(this);
            if (ti != null && ti.assign_meth != null)
            {
                il.Emit(OpCodes.Call, ti.assign_meth);
                if (ti.tp.IsValueType && ti.fix_meth != null)
                {
                    value.derefered_expr.visit(this);
                    il.Emit(OpCodes.Call, ti.fix_meth);
                }
                return;
            }
            //ICompiledTypeNode ctn = from.type as ICompiledTypeNode;
            ICompiledTypeNode ctn2 = to.type as ICompiledTypeNode;
            if ((from.type.is_value_type || from.type.is_generic_parameter) && ctn2 != null && (ctn2.compiled_type == TypeFactory.ObjectType || (ctn2.compiled_type == TypeFactory.ObjectType || ctn2.compiled_type.IsInterface)))
            {
                il.Emit(OpCodes.Box, helper.GetTypeReference(from.type).tp);
            }
            else if (from.conversion_type != null && (from.conversion_type.is_value_type || from.conversion_type.is_generic_parameter) && ctn2 != null && (ctn2.compiled_type == TypeFactory.ObjectType || ctn2.compiled_type.IsInterface))
            {
                il.Emit(OpCodes.Box, helper.GetTypeReference(from.conversion_type).tp);
            }
            CheckArrayAssign(to, from, il);
            NETGeneratorTools.PushStind(il, ti.tp);
            if (TypeNeedToFix(value.type))
            {
                value.derefered_expr.visit(this);
                il.Emit(OpCodes.Ldind_Ref);
                FixPointer();
            }
            else if (ti.tp.IsValueType && ti.fix_meth != null)
            {
                value.derefered_expr.visit(this);
                il.Emit(OpCodes.Call, ti.fix_meth);
            }
        }

        //присваивание
        private void ConvertAssignExpr(IExpressionNode to, IExpressionNode from)
        {
            if (to is INamespaceVariableReferenceNode)
            {
                AssignToNamespaceVariableNode(to, from);
            }
            else if (to is ILocalVariableReferenceNode || to is ILocalBlockVariableReferenceNode)
            {
                AssignToLocalVariableNode(to, from);
            }
            else if (to is ICommonParameterReferenceNode)
            {
                AssignToParameterNode(to, from);
            }
            else if (to is ICommonClassFieldReferenceNode)
            {
                AssignToField(to, from);
            }
            else if (to is IStaticCommonClassFieldReferenceNode)
            {
                AssignToStaticField(to, from);
            }
            else if (to is ICompiledFieldReferenceNode)
            {
                AssignToCompiledField(to, from);
            }
            else if (to is IStaticCompiledFieldReferenceNode)
            {
                AssignToStaticCompiledField(to, from);
            }
            else if (to is ISimpleArrayIndexingNode)
            {
                AssignToSimpleArrayNode(to, from);
            }
            else if (to is IDereferenceNode)
            {
                AssignToDereferenceNode(to, from);
            }
        }

        //перевод инкремента
        private void ConvertInc(IExpressionNode e)
        {
            Type tp = helper.GetTypeReference(e.type).tp;
            if (e is INamespaceVariableReferenceNode)
            {
                e.visit(this);

                //DS0030 fixed
                NETGeneratorTools.PushLdc(il, tp, 1);
                //il.Emit(OpCodes.Ldc_I4_1);

                il.Emit(OpCodes.Add);
                INamespaceVariableReferenceNode var = (INamespaceVariableReferenceNode)e;
                VarInfo vi = helper.GetVariable(var.variable);
                FieldBuilder fb = vi.fb;
                il.Emit(OpCodes.Stsfld, fb);
            }
            else if (e is ILocalVariableReferenceNode || e is ILocalBlockVariableReferenceNode)
            {
                IReferenceNode var = (IReferenceNode)e;
                VarInfo vi = helper.GetVariable(var.Variable);
                if (vi.kind == VarKind.vkLocal)
                {
                    LocalBuilder lb = vi.lb;
                    e.visit(this);
                    if (vi.lb.LocalType != TypeFactory.BoolType)
                    {
                        //DS0030 fixed
                        NETGeneratorTools.PushLdc(il, tp, 1);
                        //il.Emit(OpCodes.Ldc_I4_1);

                        il.Emit(OpCodes.Add);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldc_I4_0);
                        il.Emit(OpCodes.Ceq);
                    }
                    il.Emit(OpCodes.Stloc, lb);
                }
                else if (vi.kind == VarKind.vkNonLocal)
                {
                    FieldBuilder fb = vi.fb;
                    MethInfo cur_mi = (MethInfo)smi.Peek();
                    int dist = ((MethInfo)smi.Peek()).num_scope - vi.meth.num_scope;
                    il.Emit(OpCodes.Ldloc, cur_mi.frame);
                    for (int i = 0; i < dist; i++)
                    {
                        il.Emit(OpCodes.Ldfld, cur_mi.disp.parent);
                        cur_mi = cur_mi.up_meth;
                    }
                    e.visit(this);
                    if (vi.fb.FieldType != TypeFactory.BoolType)
                    {
                        //DS0030 fixed
                        NETGeneratorTools.PushLdc(il, tp, 1);
                        //il.Emit(OpCodes.Ldc_I4_1);

                        il.Emit(OpCodes.Add);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldc_I4_0);
                        il.Emit(OpCodes.Ceq);
                    }
                    il.Emit(OpCodes.Stfld, fb);
                }
            }
            else if (e is ICommonParameterReferenceNode)
            {
                ICommonParameterReferenceNode var = (ICommonParameterReferenceNode)e;
                ParamInfo pi = helper.GetParameter(var.parameter);
                if (pi.kind == ParamKind.pkNone)
                {
                    ParameterBuilder pb = pi.pb;
                    //byte pos = (byte)(pb.Position-1);
                    //***********************Kolay modified**********************
                    byte pos = (byte)(pb.Position - 1);
                    if (is_constructor || cur_meth.IsStatic == false) pos = (byte)pb.Position;
                    else pos = (byte)(pb.Position - 1);
                    //***********************End of Kolay modified**********************
                    if (var.parameter.parameter_type == parameter_type.value)
                    {
                        e.visit(this);
                        if (helper.GetTypeReference(var.parameter.type).tp != TypeFactory.BoolType)
                        {
                            //DS0030 fixed
                            NETGeneratorTools.PushLdc(il, tp, 1);
                            //il.Emit(OpCodes.Ldc_I4_1);

                            il.Emit(OpCodes.Add);
                        }
                        else
                        {
                            il.Emit(OpCodes.Ldc_I4_0);
                            il.Emit(OpCodes.Ceq);
                        }

                        if (pos <= 255) il.Emit(OpCodes.Starg_S, pos);
                        else il.Emit(OpCodes.Starg, pos);
                    }
                    else
                    {
                        PushParameter(pos);
                        e.visit(this);
                        if (helper.GetTypeReference(var.parameter.type).tp != TypeFactory.BoolType)
                        {
                            //DS0030 fixed
                            NETGeneratorTools.PushLdc(il, tp, 1);
                            //il.Emit(OpCodes.Ldc_I4_1);

                            il.Emit(OpCodes.Add);
                        }
                        else
                        {
                            il.Emit(OpCodes.Ldc_I4_0);
                            il.Emit(OpCodes.Ceq);
                        }
                        TypeInfo ti = helper.GetTypeReference(var.type);
                        StoreParameterByReference(ti.tp);
                    }
                }
                else
                {
                    FieldBuilder fb = pi.fb;
                    MethInfo cur_mi = (MethInfo)smi.Peek();
                    int dist = ((MethInfo)smi.Peek()).num_scope - pi.meth.num_scope;
                    il.Emit(OpCodes.Ldloc, cur_mi.frame);
                    for (int i = 0; i < dist; i++)
                    {
                        il.Emit(OpCodes.Ldfld, cur_mi.disp.parent);
                        cur_mi = cur_mi.up_meth;
                    }

                    if (var.parameter.parameter_type == parameter_type.value)
                    {
                        e.visit(this);
                        if (fb.FieldType != TypeFactory.BoolType)
                        {
                            //DS0030 fixed
                            NETGeneratorTools.PushLdc(il, tp, 1);
                            //il.Emit(OpCodes.Ldc_I4_1);

                            il.Emit(OpCodes.Add);
                        }
                        else
                        {
                            il.Emit(OpCodes.Ldc_I4_0);
                            il.Emit(OpCodes.Ceq);
                        }
                        il.Emit(OpCodes.Stfld, fb);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldfld, fb);
                        e.visit(this);
                        if (fb.FieldType != TypeFactory.BoolType)
                        {
                            //DS0030 fixed
                            NETGeneratorTools.PushLdc(il, tp, 1);
                            //il.Emit(OpCodes.Ldc_I4_1);

                            il.Emit(OpCodes.Add);
                        }
                        else
                        {
                            il.Emit(OpCodes.Ldc_I4_0);
                            il.Emit(OpCodes.Ceq);
                        }
                        TypeInfo ti = helper.GetTypeReference(var.type);
                        StoreParameterByReference(ti.tp);
                    }
                }
            }
        }

        //перевод декремента
        private void ConvertDec(IExpressionNode e)
        {
            Type tp = helper.GetTypeReference(e.type).tp;
            if (e is INamespaceVariableReferenceNode)
            {
                e.visit(this);
                //DS0030 fixed
                NETGeneratorTools.PushLdc(il, tp, 1);
                //il.Emit(OpCodes.Ldc_I4_1);
                il.Emit(OpCodes.Sub);
                INamespaceVariableReferenceNode var = (INamespaceVariableReferenceNode)e;
                VarInfo vi = helper.GetVariable(var.variable);
                FieldBuilder fb = vi.fb;
                il.Emit(OpCodes.Stsfld, fb);
            }
            else if (e is ILocalVariableReferenceNode || e is ILocalBlockVariableReferenceNode)
            {
                IReferenceNode var = (IReferenceNode)e;
                VarInfo vi = helper.GetVariable(var.Variable);
                if (vi.kind == VarKind.vkLocal)
                {
                    LocalBuilder lb = vi.lb;
                    e.visit(this);
                    //DS0030 fixed
                    NETGeneratorTools.PushLdc(il, tp, 1);
                    //il.Emit(OpCodes.Ldc_I4_1);
                    il.Emit(OpCodes.Sub);
                    il.Emit(OpCodes.Stloc, lb);
                }
                else if (vi.kind == VarKind.vkNonLocal)
                {
                    FieldBuilder fb = vi.fb;
                    MethInfo cur_mi = smi.Peek();
                    int dist = (smi.Peek()).num_scope - vi.meth.num_scope;
                    il.Emit(OpCodes.Ldloc, cur_mi.frame);
                    for (int i = 0; i < dist; i++)
                    {
                        il.Emit(OpCodes.Ldfld, cur_mi.disp.parent);
                        cur_mi = cur_mi.up_meth;
                    }
                    e.visit(this);
                    //DS0030 fixed
                    NETGeneratorTools.PushLdc(il, tp, 1);
                    //il.Emit(OpCodes.Ldc_I4_1);
                    il.Emit(OpCodes.Sub);
                    il.Emit(OpCodes.Stfld, fb);
                }
            }
            else if (e is ICommonParameterReferenceNode)
            {
                ICommonParameterReferenceNode var = (ICommonParameterReferenceNode)e;
                ParamInfo pi = helper.GetParameter(var.parameter);
                if (pi.kind == ParamKind.pkNone)
                {
                    ParameterBuilder pb = pi.pb;
                    //byte pos = (byte)(pb.Position-1);
                    //***********************Kolay modified**********************
                    byte pos = (byte)(pb.Position - 1);
                    if (is_constructor || cur_meth.IsStatic == false) pos = (byte)pb.Position;
                    else pos = (byte)(pb.Position - 1);
                    //***********************End of Kolay modified**********************
                    if (var.parameter.parameter_type == parameter_type.value)
                    {
                        e.visit(this);
                        //DS0030 fixed
                        NETGeneratorTools.PushLdc(il, tp, 1);
                        //il.Emit(OpCodes.Ldc_I4_1);
                        il.Emit(OpCodes.Sub);

                        if (pos <= 255) il.Emit(OpCodes.Starg_S, pos);
                        else il.Emit(OpCodes.Starg, pos);
                    }
                    else
                    {
                        PushParameter(pos);
                        e.visit(this);
                        //DS0030 fixed
                        NETGeneratorTools.PushLdc(il, tp, 1);
                        //il.Emit(OpCodes.Ldc_I4_1);
                        il.Emit(OpCodes.Sub);
                        TypeInfo ti = helper.GetTypeReference(var.type);
                        StoreParameterByReference(ti.tp);
                    }
                }
                else
                {
                    FieldBuilder fb = pi.fb;
                    MethInfo cur_mi = smi.Peek();
                    int dist = (smi.Peek()).num_scope - pi.meth.num_scope;
                    il.Emit(OpCodes.Ldloc, cur_mi.frame);
                    for (int i = 0; i < dist; i++)
                    {
                        il.Emit(OpCodes.Ldfld, cur_mi.disp.parent);
                        cur_mi = cur_mi.up_meth;
                    }

                    if (var.parameter.parameter_type == parameter_type.value)
                    {
                        e.visit(this);
                        //DS0030 fixed
                        NETGeneratorTools.PushLdc(il, tp, 1);
                        //il.Emit(OpCodes.Ldc_I4_1);
                        il.Emit(OpCodes.Sub);
                        il.Emit(OpCodes.Stfld, fb);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldfld, fb);
                        e.visit(this);
                        //DS0030 fixed
                        NETGeneratorTools.PushLdc(il, tp, 1);
                        //il.Emit(OpCodes.Ldc_I4_1);
                        il.Emit(OpCodes.Sub);
                        TypeInfo ti = helper.GetTypeReference(var.type);
                        StoreParameterByReference(ti.tp);
                    }
                }
            }
        }

        //перевод бинарных, унарных и проч. выражений
        public override void visit(SemanticTree.IBasicFunctionCallNode value)
        {
            make_next_spoint = true;
            bool tmp_dot = is_dot_expr;
            IExpressionNode[] real_parameters = value.real_parameters;
            is_dot_expr = false;
            {
                //(ssyy) 29.01.2008 Внёс band, bor под switch
                basic_function_type ft = value.basic_function.basic_function_type;
                switch (ft)
                {
                    case basic_function_type.booland:
                        ConvertSokrAndExpression(value);//сокращенное вычисление and
                        return;
                    case basic_function_type.boolor:
                        ConvertSokrOrExpression(value);//сокращенное вычисление or
                        return;
                    case basic_function_type.iassign: ConvertAssignExpr(real_parameters[0], real_parameters[1]); return;
                    case basic_function_type.bassign: ConvertAssignExpr(real_parameters[0], real_parameters[1]); return;
                    case basic_function_type.lassign: ConvertAssignExpr(real_parameters[0], real_parameters[1]); return;
                    case basic_function_type.fassign: ConvertAssignExpr(real_parameters[0], real_parameters[1]); return;
                    case basic_function_type.dassign: ConvertAssignExpr(real_parameters[0], real_parameters[1]); return;
                    case basic_function_type.uiassign: ConvertAssignExpr(real_parameters[0], real_parameters[1]); return;
                    case basic_function_type.usassign: ConvertAssignExpr(real_parameters[0], real_parameters[1]); return;
                    case basic_function_type.ulassign: ConvertAssignExpr(real_parameters[0], real_parameters[1]); return;
                    case basic_function_type.sassign: ConvertAssignExpr(real_parameters[0], real_parameters[1]); return;
                    case basic_function_type.sbassign: ConvertAssignExpr(real_parameters[0], real_parameters[1]); return;
                    case basic_function_type.boolassign: ConvertAssignExpr(real_parameters[0], real_parameters[1]); return;
                    case basic_function_type.charassign: ConvertAssignExpr(real_parameters[0], real_parameters[1]); return;
                    case basic_function_type.objassign: ConvertAssignExpr(real_parameters[0], real_parameters[1]); return;
                    case basic_function_type.iinc:
                    case basic_function_type.binc:
                    case basic_function_type.sinc:
                    case basic_function_type.linc:
                    case basic_function_type.uiinc:
                    case basic_function_type.sbinc:
                    case basic_function_type.usinc:
                    case basic_function_type.ulinc:
                    case basic_function_type.boolinc:
                    case basic_function_type.cinc: ConvertInc(real_parameters[0]); return;
                    case basic_function_type.idec:
                    case basic_function_type.bdec:
                    case basic_function_type.sdec:
                    case basic_function_type.ldec:
                    case basic_function_type.uidec:
                    case basic_function_type.sbdec:
                    case basic_function_type.usdec:
                    case basic_function_type.uldec:
                    case basic_function_type.booldec:
                    case basic_function_type.cdec: ConvertDec(real_parameters[0]); return;
                }
                if (real_parameters.Length > 1)
                {
                	if (real_parameters[0].type.is_nullable_type && real_parameters[1] is INullConstantNode)
                	{
                		bool tmp = is_dot_expr;
                		is_dot_expr = true;
                		TypeInfo ti = helper.GetTypeReference(real_parameters[0].type);
                		real_parameters[0].visit(this);
                		is_dot_expr = tmp;
                		MethodInfo mi = null;
                		if (real_parameters[0].type is IGenericTypeInstance)
                			mi = TypeBuilder.GetMethod(ti.tp, typeof(Nullable<>).GetMethod("get_HasValue"));
                		else
                			mi = ti.tp.GetMethod("get_HasValue");
                		il.Emit(OpCodes.Call, mi);
                		if (ft == basic_function_type.objeq)
                			il.Emit(OpCodes.Not);
                		return;
                	}
                	else if (real_parameters[1].type.is_nullable_type && real_parameters[0] is INullConstantNode)
                	{
                		bool tmp = is_dot_expr;
                		is_dot_expr = true;
                		TypeInfo ti = helper.GetTypeReference(real_parameters[1].type);
                		real_parameters[1].visit(this);
                		is_dot_expr = tmp;
                		MethodInfo mi = null;
                		if (real_parameters[1].type is IGenericTypeInstance)
                			mi = TypeBuilder.GetMethod(ti.tp, typeof(Nullable<>).GetMethod("get_HasValue"));
                		else
                			mi = ti.tp.GetMethod("get_HasValue");
                		il.Emit(OpCodes.Call, mi);
                		if (ft == basic_function_type.objeq)
                			il.Emit(OpCodes.Not);
                		return;
                	}
                }
                real_parameters[0].visit(this);
                if (real_parameters.Length > 1)
                    real_parameters[1].visit(this);
                //is_dot_expr = tmp_dot;
                EmitOperator(value);//кладем соотв. команду
                if (tmp_dot == true)
                {
                    //LocalBuilder lb = il.DeclareLocal(helper.GetTypeReference(value.type).tp);
                    //il.Emit(OpCodes.Stloc, lb);
                    //il.Emit(OpCodes.Ldloca, lb);
                    is_dot_expr = tmp_dot;
                    NETGeneratorTools.CreateLocalAndLoad(il, helper.GetTypeReference(value.type).tp);
                }
            }
        }

        private void ConvertSokrAndExpression(IBasicFunctionCallNode expr)
        {
            Label lb1 = il.DefineLabel();
            Label lb2 = il.DefineLabel();
            IExpressionNode[] real_parameters = expr.real_parameters;
            real_parameters[0].visit(this);
            il.Emit(OpCodes.Brfalse, lb1);
            il.Emit(OpCodes.Ldc_I4_1);
            real_parameters[1].visit(this);
            il.Emit(OpCodes.And);
            il.Emit(OpCodes.Br, lb2);
            il.MarkLabel(lb1);
            il.Emit(OpCodes.Ldc_I4_0);
            il.MarkLabel(lb2);
        }

        private void ConvertSokrOrExpression(IBasicFunctionCallNode expr)
        {
            Label lb1 = il.DefineLabel();
            Label lb2 = il.DefineLabel();
            IExpressionNode[] real_parameters = expr.real_parameters;
            real_parameters[0].visit(this);
            il.Emit(OpCodes.Brtrue, lb1);
            il.Emit(OpCodes.Ldc_I4_0);
            real_parameters[1].visit(this);
            il.Emit(OpCodes.Or);
            il.Emit(OpCodes.Br, lb2);
            il.MarkLabel(lb1);
            il.Emit(OpCodes.Ldc_I4_1);
            il.MarkLabel(lb2);
        }

        protected virtual void EmitOperator(IBasicFunctionCallNode fn)
        {
            basic_function_type ft = fn.basic_function.basic_function_type;
            switch (ft)
            {
                case basic_function_type.none: return;

                case basic_function_type.iadd:
                case basic_function_type.badd:
                case basic_function_type.sadd:
                case basic_function_type.ladd:
                case basic_function_type.fadd:
                case basic_function_type.sbadd:
                case basic_function_type.usadd:
                case basic_function_type.uladd:
                case basic_function_type.dadd: il.Emit(OpCodes.Add); break;
                case basic_function_type.uiadd:
                    //il.Emit(OpCodes.Conv_U8);
                    il.Emit(OpCodes.Add);
                    il.Emit(OpCodes.Conv_I8);
                    break;

                case basic_function_type.isub:
                case basic_function_type.bsub:
                case basic_function_type.ssub:
                case basic_function_type.lsub:
                case basic_function_type.fsub:
                //case basic_function_type.uisub:
                case basic_function_type.sbsub:
                case basic_function_type.ussub:
                case basic_function_type.ulsub:
                case basic_function_type.dsub: il.Emit(OpCodes.Sub); break;
                case basic_function_type.uisub: il.Emit(OpCodes.Sub); il.Emit(OpCodes.Conv_I8); break;

                case basic_function_type.imul:
                case basic_function_type.bmul:
                case basic_function_type.smul:
                case basic_function_type.lmul:
                case basic_function_type.fmul:
                //case basic_function_type.uimul:
                case basic_function_type.sbmul:
                case basic_function_type.usmul:
                case basic_function_type.ulmul:
                case basic_function_type.dmul: il.Emit(OpCodes.Mul); break;
                case basic_function_type.uimul: il.Emit(OpCodes.Mul); il.Emit(OpCodes.Conv_I8); break;

                case basic_function_type.idiv:
                case basic_function_type.bdiv:
                case basic_function_type.sdiv:
                case basic_function_type.ldiv:
                case basic_function_type.fdiv:
                //case basic_function_type.uidiv:
                case basic_function_type.sbdiv:
                case basic_function_type.usdiv:

                case basic_function_type.ddiv: il.Emit(OpCodes.Div); break;
                case basic_function_type.uidiv: il.Emit(OpCodes.Div_Un); il.Emit(OpCodes.Conv_I8); break;
                case basic_function_type.uldiv: il.Emit(OpCodes.Div_Un); break;

                case basic_function_type.imod:
                case basic_function_type.bmod:
                case basic_function_type.smod:
                //case basic_function_type.uimod:
                case basic_function_type.sbmod:
                case basic_function_type.usmod:

                case basic_function_type.lmod: il.Emit(OpCodes.Rem); break;
                case basic_function_type.uimod: il.Emit(OpCodes.Rem_Un); il.Emit(OpCodes.Conv_I8); break;
                case basic_function_type.ulmod: il.Emit(OpCodes.Rem_Un); break;

                case basic_function_type.isinc:
                case basic_function_type.bsinc:
                case basic_function_type.sbsinc:
                case basic_function_type.ssinc:
                case basic_function_type.ussinc:
                case basic_function_type.uisinc:
                case basic_function_type.ulsinc: il.Emit(OpCodes.Ldc_I4_1); il.Emit(OpCodes.Add); break;
                case basic_function_type.lsinc: il.Emit(OpCodes.Ldc_I8, 1); il.Emit(OpCodes.Add); break;

                case basic_function_type.isdec:
                case basic_function_type.bsdec:
                case basic_function_type.sbsdec:
                case basic_function_type.ssdec:
                case basic_function_type.ussdec:
                case basic_function_type.uisdec:
                case basic_function_type.ulsdec: il.Emit(OpCodes.Ldc_I4_1); il.Emit(OpCodes.Sub); break;
                case basic_function_type.lsdec: il.Emit(OpCodes.Ldc_I8, 1); il.Emit(OpCodes.Sub); break;

                case basic_function_type.inot:
                case basic_function_type.bnot:
                case basic_function_type.snot:
                case basic_function_type.uinot:
                case basic_function_type.sbnot:
                case basic_function_type.usnot:
                case basic_function_type.ulnot:
                case basic_function_type.lnot: il.Emit(OpCodes.Not); break;
                case basic_function_type.boolsdec:
                case basic_function_type.boolsinc:
                case basic_function_type.boolnot: il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;

                case basic_function_type.ishl: il.Emit(OpCodes.Shl); break;
                case basic_function_type.ishr: il.Emit(OpCodes.Shr); break;
                case basic_function_type.bshl: il.Emit(OpCodes.Shl); break;
                case basic_function_type.bshr: il.Emit(OpCodes.Shr_Un); break;
                case basic_function_type.sshl: il.Emit(OpCodes.Shl); break;
                case basic_function_type.sshr: il.Emit(OpCodes.Shr); break;
                case basic_function_type.lshl: il.Emit(OpCodes.Shl); break;
                case basic_function_type.lshr: il.Emit(OpCodes.Shr); break;
                case basic_function_type.uishl: il.Emit(OpCodes.Shl); break;
                case basic_function_type.sbshl: il.Emit(OpCodes.Shl); break;
                case basic_function_type.usshl: il.Emit(OpCodes.Shl); break;
                case basic_function_type.ulshl: il.Emit(OpCodes.Shl); break;
                case basic_function_type.uishr: il.Emit(OpCodes.Shr_Un); break;
                case basic_function_type.sbshr: il.Emit(OpCodes.Shr); break;
                case basic_function_type.usshr: il.Emit(OpCodes.Shr_Un); break;
                case basic_function_type.ulshr: il.Emit(OpCodes.Shr_Un); break;

                case basic_function_type.ieq: il.Emit(OpCodes.Ceq); break;
                case basic_function_type.inoteq: il.Emit(OpCodes.Ceq); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.igr: il.Emit(OpCodes.Cgt); break;
                case basic_function_type.igreq: il.Emit(OpCodes.Clt); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.ism: il.Emit(OpCodes.Clt); break;
                case basic_function_type.ismeq: il.Emit(OpCodes.Cgt); il.Emit(OpCodes.Ldc_I4_0);
                    il.Emit(OpCodes.Ceq); break;

                case basic_function_type.seq: il.Emit(OpCodes.Ceq); break;
                case basic_function_type.snoteq: il.Emit(OpCodes.Ceq); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.sgr: il.Emit(OpCodes.Cgt); break;
                case basic_function_type.sgreq: il.Emit(OpCodes.Clt); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.ssm: il.Emit(OpCodes.Clt); break;
                case basic_function_type.ssmeq: il.Emit(OpCodes.Cgt); il.Emit(OpCodes.Ldc_I4_0);
                    il.Emit(OpCodes.Ceq); break;

                case basic_function_type.beq: il.Emit(OpCodes.Ceq); break;
                case basic_function_type.bnoteq: il.Emit(OpCodes.Ceq); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.bgr: il.Emit(OpCodes.Cgt); break;
                case basic_function_type.bgreq: il.Emit(OpCodes.Clt); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.bsm: il.Emit(OpCodes.Clt); break;
                case basic_function_type.bsmeq: il.Emit(OpCodes.Cgt); il.Emit(OpCodes.Ldc_I4_0);
                    il.Emit(OpCodes.Ceq); break;


                case basic_function_type.leq: il.Emit(OpCodes.Ceq); break;
                case basic_function_type.lnoteq: il.Emit(OpCodes.Ceq); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.lgr: il.Emit(OpCodes.Cgt); break;
                case basic_function_type.lgreq: il.Emit(OpCodes.Clt); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.lsm: il.Emit(OpCodes.Clt); break;
                case basic_function_type.lsmeq: il.Emit(OpCodes.Cgt); il.Emit(OpCodes.Ldc_I4_0);
                    il.Emit(OpCodes.Ceq); break;

                case basic_function_type.uieq: il.Emit(OpCodes.Ceq); break;
                case basic_function_type.uinoteq: il.Emit(OpCodes.Ceq); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.uigr: il.Emit(OpCodes.Cgt_Un); break;
                case basic_function_type.uigreq: il.Emit(OpCodes.Clt_Un); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.uism: il.Emit(OpCodes.Clt_Un); break;
                case basic_function_type.uismeq: il.Emit(OpCodes.Cgt_Un); il.Emit(OpCodes.Ldc_I4_0);
                    il.Emit(OpCodes.Ceq); break;

                case basic_function_type.sbeq: il.Emit(OpCodes.Ceq); break;
                case basic_function_type.sbnoteq: il.Emit(OpCodes.Ceq); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.sbgr: il.Emit(OpCodes.Cgt); break;
                case basic_function_type.sbgreq: il.Emit(OpCodes.Clt); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.sbsm: il.Emit(OpCodes.Clt); break;
                case basic_function_type.sbsmeq: il.Emit(OpCodes.Cgt); il.Emit(OpCodes.Ldc_I4_0);
                    il.Emit(OpCodes.Ceq); break;

                case basic_function_type.useq: il.Emit(OpCodes.Ceq); break;
                case basic_function_type.usnoteq: il.Emit(OpCodes.Ceq); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.usgr: il.Emit(OpCodes.Cgt_Un); break;
                case basic_function_type.usgreq: il.Emit(OpCodes.Clt_Un); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.ussm: il.Emit(OpCodes.Clt_Un); break;
                case basic_function_type.ussmeq: il.Emit(OpCodes.Cgt_Un); il.Emit(OpCodes.Ldc_I4_0);
                    il.Emit(OpCodes.Ceq); break;

                case basic_function_type.uleq: il.Emit(OpCodes.Ceq); break;
                case basic_function_type.ulnoteq: il.Emit(OpCodes.Ceq); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.ulgr: il.Emit(OpCodes.Cgt_Un); break;
                case basic_function_type.ulgreq: il.Emit(OpCodes.Clt_Un); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.ulsm: il.Emit(OpCodes.Clt_Un); break;
                case basic_function_type.ulsmeq: il.Emit(OpCodes.Cgt_Un); il.Emit(OpCodes.Ldc_I4_0);
                    il.Emit(OpCodes.Ceq); break;

                case basic_function_type.booleq: il.Emit(OpCodes.Ceq); break;
                case basic_function_type.boolnoteq: il.Emit(OpCodes.Ceq); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.boolgr: il.Emit(OpCodes.Cgt); break;
                case basic_function_type.boolgreq: il.Emit(OpCodes.Clt); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.boolsm: il.Emit(OpCodes.Clt); break;
                case basic_function_type.boolsmeq: il.Emit(OpCodes.Cgt); il.Emit(OpCodes.Ldc_I4_0);
                    il.Emit(OpCodes.Ceq); break;


                case basic_function_type.feq: il.Emit(OpCodes.Ceq); break;
                case basic_function_type.fnoteq: il.Emit(OpCodes.Ceq); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.fgr: il.Emit(OpCodes.Cgt); break;
                case basic_function_type.fgreq: il.Emit(OpCodes.Clt); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.fsm: il.Emit(OpCodes.Clt); break;
                case basic_function_type.fsmeq: il.Emit(OpCodes.Cgt); il.Emit(OpCodes.Ldc_I4_0);
                    il.Emit(OpCodes.Ceq); break;


                case basic_function_type.deq: il.Emit(OpCodes.Ceq); break;
                case basic_function_type.dnoteq: il.Emit(OpCodes.Ceq); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.dgr: il.Emit(OpCodes.Cgt); break;
                case basic_function_type.dgreq: il.Emit(OpCodes.Clt); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.dsm: il.Emit(OpCodes.Clt); break;
                case basic_function_type.dsmeq: il.Emit(OpCodes.Cgt); il.Emit(OpCodes.Ldc_I4_0);
                    il.Emit(OpCodes.Ceq); break;


                case basic_function_type.chareq: il.Emit(OpCodes.Ceq); break;
                case basic_function_type.charnoteq: il.Emit(OpCodes.Ceq); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.chargr: il.Emit(OpCodes.Cgt); break;
                case basic_function_type.chargreq: il.Emit(OpCodes.Clt); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.charsm: il.Emit(OpCodes.Clt); break;
                case basic_function_type.charsmeq: il.Emit(OpCodes.Cgt); il.Emit(OpCodes.Ldc_I4_0);
                    il.Emit(OpCodes.Ceq); break;

                case basic_function_type.objeq: il.Emit(OpCodes.Ceq); break;
                case basic_function_type.objnoteq: il.Emit(OpCodes.Ceq); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;

                case basic_function_type.band: il.Emit(OpCodes.And); break;
                case basic_function_type.bor: il.Emit(OpCodes.Or); break;
                case basic_function_type.bxor: il.Emit(OpCodes.Xor); break;

                case basic_function_type.iand: il.Emit(OpCodes.And); break;
                case basic_function_type.ior: il.Emit(OpCodes.Or); break;
                case basic_function_type.ixor: il.Emit(OpCodes.Xor); break;

                case basic_function_type.land: il.Emit(OpCodes.And); break;
                case basic_function_type.lor: il.Emit(OpCodes.Or); break;
                case basic_function_type.lxor: il.Emit(OpCodes.Xor); break;

                case basic_function_type.sand: il.Emit(OpCodes.And); break;
                case basic_function_type.sor: il.Emit(OpCodes.Or); break;
                case basic_function_type.sxor: il.Emit(OpCodes.Xor); break;

                case basic_function_type.uiand: il.Emit(OpCodes.And); break;
                case basic_function_type.uior: il.Emit(OpCodes.Or); break;
                case basic_function_type.uixor: il.Emit(OpCodes.Xor); break;

                case basic_function_type.sband: il.Emit(OpCodes.And); break;
                case basic_function_type.sbor: il.Emit(OpCodes.Or); break;
                case basic_function_type.sbxor: il.Emit(OpCodes.Xor); break;

                case basic_function_type.usand: il.Emit(OpCodes.And); break;
                case basic_function_type.usor: il.Emit(OpCodes.Or); break;
                case basic_function_type.usxor: il.Emit(OpCodes.Xor); break;

                case basic_function_type.uland: il.Emit(OpCodes.And); break;
                case basic_function_type.ulor: il.Emit(OpCodes.Or); break;
                case basic_function_type.ulxor: il.Emit(OpCodes.Xor); break;

                case basic_function_type.booland: il.Emit(OpCodes.And); break;
                case basic_function_type.boolor: il.Emit(OpCodes.Or); break;
                case basic_function_type.boolxor: il.Emit(OpCodes.Xor); break;
                //case basic_function_type.chareq: il.Emit(OpCodes.Ceq); break;
                //case basic_function_type.charnoteq: il.Emit(OpCodes.Ceq); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.enumgr: il.Emit(OpCodes.Cgt); break;
                case basic_function_type.enumgreq: il.Emit(OpCodes.Clt); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                case basic_function_type.enumsm: il.Emit(OpCodes.Clt); break;
                case basic_function_type.enumsmeq: il.Emit(OpCodes.Cgt); il.Emit(OpCodes.Ldc_I4_0);
                    il.Emit(OpCodes.Ceq); break;

                case basic_function_type.iunmin:
                case basic_function_type.bunmin:
                case basic_function_type.sunmin:
                case basic_function_type.funmin:
                case basic_function_type.dunmin:
                //case basic_function_type.uiunmin:
                case basic_function_type.sbunmin:
                case basic_function_type.usunmin:
                //case basic_function_type.ulunmin:
                case basic_function_type.lunmin: il.Emit(OpCodes.Neg); break;
                case basic_function_type.uiunmin: il.Emit(OpCodes.Neg); il.Emit(OpCodes.Conv_I8); break;
                case basic_function_type.chartoi: il.Emit(OpCodes.Conv_I4); break;
                case basic_function_type.chartous: il.Emit(OpCodes.Conv_U2); break;
                case basic_function_type.chartoui: il.Emit(OpCodes.Conv_U4); break;
                case basic_function_type.chartol: il.Emit(OpCodes.Conv_I8); break;
                case basic_function_type.chartoul: il.Emit(OpCodes.Conv_U8); break;
                case basic_function_type.chartof: il.Emit(OpCodes.Conv_R4); break;
                case basic_function_type.chartod: il.Emit(OpCodes.Conv_R8); break;
                case basic_function_type.chartob: il.Emit(OpCodes.Conv_U1); break;
                case basic_function_type.chartosb: il.Emit(OpCodes.Conv_I1); break;
                case basic_function_type.chartos: il.Emit(OpCodes.Conv_I2); break;

                case basic_function_type.itod: il.Emit(OpCodes.Conv_R8); break;
                case basic_function_type.itol: il.Emit(OpCodes.Conv_I8); break;
                case basic_function_type.itof: il.Emit(OpCodes.Conv_R4); break;
                case basic_function_type.itob: il.Emit(OpCodes.Conv_U1); break;
                case basic_function_type.itosb: il.Emit(OpCodes.Conv_I1); break;
                case basic_function_type.itos: il.Emit(OpCodes.Conv_I2); break;
                case basic_function_type.itous: il.Emit(OpCodes.Conv_U2); break;
                case basic_function_type.itoui: il.Emit(OpCodes.Conv_U4); break;
                case basic_function_type.itoul: il.Emit(OpCodes.Conv_U8); break;
                case basic_function_type.itochar: il.Emit(OpCodes.Conv_U2); break;

                case basic_function_type.btos: il.Emit(OpCodes.Conv_I2); break;
                case basic_function_type.btous: il.Emit(OpCodes.Conv_U2); break;
                case basic_function_type.btoi: il.Emit(OpCodes.Conv_I4); break;
                case basic_function_type.btoui: il.Emit(OpCodes.Conv_U4); break;
                case basic_function_type.btol: il.Emit(OpCodes.Conv_I8); break;
                case basic_function_type.btoul: il.Emit(OpCodes.Conv_U8); break;
                case basic_function_type.btof: il.Emit(OpCodes.Conv_R_Un); il.Emit(OpCodes.Conv_R4); break;
                case basic_function_type.btod: il.Emit(OpCodes.Conv_R_Un); il.Emit(OpCodes.Conv_R8); break;
                case basic_function_type.btosb: il.Emit(OpCodes.Conv_I1); break;
                case basic_function_type.btochar: il.Emit(OpCodes.Conv_U2); break;

                case basic_function_type.sbtos: il.Emit(OpCodes.Conv_I2); break;
                case basic_function_type.sbtoi: il.Emit(OpCodes.Conv_I4); break;
                case basic_function_type.sbtol: il.Emit(OpCodes.Conv_I8); break;
                case basic_function_type.sbtof: il.Emit(OpCodes.Conv_R4); break;
                case basic_function_type.sbtod: il.Emit(OpCodes.Conv_R8); break;
                case basic_function_type.sbtob: il.Emit(OpCodes.Conv_U1); break;
                case basic_function_type.sbtous: il.Emit(OpCodes.Conv_U2); break;
                case basic_function_type.sbtoui: il.Emit(OpCodes.Conv_U4); break;
                case basic_function_type.sbtoul: il.Emit(OpCodes.Conv_U8); break;
                case basic_function_type.sbtochar: il.Emit(OpCodes.Conv_U2); break;

                case basic_function_type.stoi: il.Emit(OpCodes.Conv_I4); break;
                case basic_function_type.stol: il.Emit(OpCodes.Conv_I8); break;
                case basic_function_type.stof: il.Emit(OpCodes.Conv_R4); break;
                case basic_function_type.stod: il.Emit(OpCodes.Conv_R8); break;
                case basic_function_type.stob: il.Emit(OpCodes.Conv_U1); break;
                case basic_function_type.stosb: il.Emit(OpCodes.Conv_I1); break;
                case basic_function_type.stous: il.Emit(OpCodes.Conv_U2); break;
                case basic_function_type.stoui: il.Emit(OpCodes.Conv_U4); break;
                case basic_function_type.stoul: il.Emit(OpCodes.Conv_U8); break;
                case basic_function_type.stochar: il.Emit(OpCodes.Conv_U2); break;

                case basic_function_type.ustoi: il.Emit(OpCodes.Conv_I4); break;
                case basic_function_type.ustoui: il.Emit(OpCodes.Conv_U4); break;
                case basic_function_type.ustol: il.Emit(OpCodes.Conv_I8); break;
                case basic_function_type.ustoul: il.Emit(OpCodes.Conv_U8); break;
                case basic_function_type.ustof: il.Emit(OpCodes.Conv_R_Un); il.Emit(OpCodes.Conv_R4); break;
                case basic_function_type.ustod: il.Emit(OpCodes.Conv_R_Un); il.Emit(OpCodes.Conv_R8); break;
                case basic_function_type.ustob: il.Emit(OpCodes.Conv_U1); break;
                case basic_function_type.ustosb: il.Emit(OpCodes.Conv_I1); break;
                case basic_function_type.ustos: il.Emit(OpCodes.Conv_I2); break;
                case basic_function_type.ustochar: il.Emit(OpCodes.Conv_U2); break;

                case basic_function_type.uitoi: il.Emit(OpCodes.Conv_I4); break;
                //case basic_function_type.ustoui: il.Emit(OpCodes.Conv_U4); break;
                case basic_function_type.uitol: il.Emit(OpCodes.Conv_U8); break;
                case basic_function_type.uitoul: il.Emit(OpCodes.Conv_U8); break;
                case basic_function_type.uitof: il.Emit(OpCodes.Conv_R_Un); il.Emit(OpCodes.Conv_R4); break;
                case basic_function_type.uitod: il.Emit(OpCodes.Conv_R_Un); il.Emit(OpCodes.Conv_R8); break;
                case basic_function_type.uitob: il.Emit(OpCodes.Conv_U1); break;
                case basic_function_type.uitosb: il.Emit(OpCodes.Conv_I1); break;
                case basic_function_type.uitos: il.Emit(OpCodes.Conv_I2); break;
                case basic_function_type.uitous: il.Emit(OpCodes.Conv_U2); break;
                case basic_function_type.uitochar: il.Emit(OpCodes.Conv_U2); break;

                case basic_function_type.ultof: il.Emit(OpCodes.Conv_R_Un); il.Emit(OpCodes.Conv_R4); break;
                case basic_function_type.ultod: il.Emit(OpCodes.Conv_R_Un); il.Emit(OpCodes.Conv_R8); break;
                case basic_function_type.ultob: il.Emit(OpCodes.Conv_U1); break;
                case basic_function_type.ultosb: il.Emit(OpCodes.Conv_I1); break;
                case basic_function_type.ultos: il.Emit(OpCodes.Conv_I2); break;
                case basic_function_type.ultous: il.Emit(OpCodes.Conv_U2); break;
                case basic_function_type.ultoi: il.Emit(OpCodes.Conv_I4); break;
                case basic_function_type.ultoui: il.Emit(OpCodes.Conv_U4); break;
                case basic_function_type.ultol: il.Emit(OpCodes.Conv_I8); break;
                case basic_function_type.ultochar: il.Emit(OpCodes.Conv_U2); break;

                case basic_function_type.ltof: il.Emit(OpCodes.Conv_R4); break;
                case basic_function_type.ltod: il.Emit(OpCodes.Conv_R8); break;
                case basic_function_type.ltob: il.Emit(OpCodes.Conv_U1); break;
                case basic_function_type.ltosb: il.Emit(OpCodes.Conv_I1); break;
                case basic_function_type.ltos: il.Emit(OpCodes.Conv_I2); break;
                case basic_function_type.ltous: il.Emit(OpCodes.Conv_U2); break;
                case basic_function_type.ltoi: il.Emit(OpCodes.Conv_I4); break;
                case basic_function_type.ltoui: il.Emit(OpCodes.Conv_U4); break;
                case basic_function_type.ltoul: il.Emit(OpCodes.Conv_U8); break;
                case basic_function_type.ltochar: il.Emit(OpCodes.Conv_U2); break;

                case basic_function_type.ftod: il.Emit(OpCodes.Conv_R8); break;
                case basic_function_type.ftob: il.Emit(OpCodes.Conv_U1); break;
                case basic_function_type.ftosb: il.Emit(OpCodes.Conv_I1); break;
                case basic_function_type.ftos: il.Emit(OpCodes.Conv_I2); break;
                case basic_function_type.ftous: il.Emit(OpCodes.Conv_U2); break;
                case basic_function_type.ftoi: il.Emit(OpCodes.Conv_I4); break;
                case basic_function_type.ftoui: il.Emit(OpCodes.Conv_U4); break;
                case basic_function_type.ftol: il.Emit(OpCodes.Conv_I8); break;
                case basic_function_type.ftoul: il.Emit(OpCodes.Conv_U8); break;
                case basic_function_type.ftochar: il.Emit(OpCodes.Conv_U2); break;

                case basic_function_type.dtob: il.Emit(OpCodes.Conv_U1); break;
                case basic_function_type.dtosb: il.Emit(OpCodes.Conv_I1); break;
                case basic_function_type.dtos: il.Emit(OpCodes.Conv_I2); break;
                case basic_function_type.dtous: il.Emit(OpCodes.Conv_U2); break;
                case basic_function_type.dtoi: il.Emit(OpCodes.Conv_I4); break;
                case basic_function_type.dtoui: il.Emit(OpCodes.Conv_U4); break;
                case basic_function_type.dtol: il.Emit(OpCodes.Conv_I8); break;
                case basic_function_type.dtoul: il.Emit(OpCodes.Conv_U8); break;
                case basic_function_type.dtof: il.Emit(OpCodes.Conv_R4); break;
                case basic_function_type.dtochar: il.Emit(OpCodes.Conv_U2); break;
                case basic_function_type.booltoi: il.Emit(OpCodes.Conv_I4); break;
                case basic_function_type.booltob: il.Emit(OpCodes.Conv_U1); break;
                case basic_function_type.booltosb: il.Emit(OpCodes.Conv_I1); break;
                case basic_function_type.booltos: il.Emit(OpCodes.Conv_I2); break;
                case basic_function_type.booltous: il.Emit(OpCodes.Conv_U2); break;
                case basic_function_type.booltoui: il.Emit(OpCodes.Conv_U4); break;
                case basic_function_type.booltol: il.Emit(OpCodes.Conv_I8); break;
                case basic_function_type.booltoul: il.Emit(OpCodes.Conv_U8); break;

                case basic_function_type.objtoobj:
                    {
                        //(ssyy) Вставил 15.05.08
                        Type from_val_type = null;
                        IExpressionNode par0 = fn.real_parameters[0];
                        if (!(par0 is SemanticTree.INullConstantNode) && (par0.type.is_value_type || par0.type.is_generic_parameter))
                        {
                            from_val_type = helper.GetTypeReference(par0.type).tp;
                        }
                        Type t = helper.GetTypeReference(fn.type).tp;
                        if (!fn.type.IsDelegate)
                            NETGeneratorTools.PushCast(il, t, from_val_type);
                        break;
                    }

            }
        }



        public override void visit(SemanticTree.IFunctionCallNode value)
        {

        }

        public override void visit(SemanticTree.IExpressionNode value)
        {

        }

        public override void visit(SemanticTree.IStatementNode value)
        {

        }

        public override void visit(SemanticTree.ICompiledTypeNode value)
        {

        }

        //перевод оболочки для массива
        public void ConvertArrayWrapperType(SemanticTree.ICommonTypeNode value)
        {
            ISimpleArrayNode arrt = value.fields[0].type as ISimpleArrayNode;
            TypeInfo elem_ti = helper.GetTypeReference(arrt.element_type);
            if (elem_ti == null)
            {
                ConvertTypeHeader((ICommonTypeNode)arrt.element_type);
            }
            else
                if (elem_ti.is_arr && elem_ti.def_cnstr == null)
                    ConvertArrayWrapperType((ICommonTypeNode)arrt.element_type);
            TypeInfo ti = helper.GetTypeReference(value);
            if (ti.def_cnstr != null) return;
            ti.is_arr = true;
            TypeBuilder tb = (TypeBuilder)ti.tp;
            TypeInfo tmp_ti = cur_ti;
            cur_ti = ti;
            //TypeBuilder tb = (TypeBuilder)helper.GetTypeBuilder(value);
            //это метод для выделения памяти под массивы
            MethodBuilder mb = tb.DefineMethod("$Init$", MethodAttributes.Private, TypeFactory.VoidType, Type.EmptyTypes);
            ti.init_meth = mb;
            MethodBuilder hndl_mb = null;
            TypeBuilder tmp = cur_type;
            cur_type = tb;

            foreach (ICommonClassFieldNode fld in value.fields)
                fld.visit(this);
            foreach (ICommonPropertyNode prop in value.properties)
                prop.visit(this);
            foreach (ICommonMethodNode meth in value.methods)
                ConvertMethodHeader(meth);
            //foreach (ICommonMethodNode meth in value.methods)
            //	meth.visit(this);
            foreach (IClassConstantDefinitionNode constant in value.constants)
                constant.visit(this);


            cur_type = tmp;
            mb.GetILGenerator().Emit(OpCodes.Ret);
            if (hndl_mb != null)
                hndl_mb.GetILGenerator().Emit(OpCodes.Ret);
            cur_ti = tmp_ti;
        }

        //перевод реализации типа
        public override void visit(SemanticTree.ICommonTypeNode value)
        {
            if (value is ISimpleArrayNode || value.type_special_kind == type_special_kind.array_kind) return;
            MakeAttribute(value);
            TypeInfo ti = helper.GetTypeReference(value);
            if (ti.tp.IsEnum || !(ti.tp is TypeBuilder)) return;
            TypeBuilder tb = (TypeBuilder)ti.tp;
            TypeInfo tmp_ti = cur_ti;
            cur_ti = ti;
            TypeBuilder tmp = cur_type;
            cur_type = tb;

            foreach (ICommonMethodNode meth in value.methods)
                meth.visit(this);
            cur_type = tmp;
            cur_ti = tmp_ti;
        }

        public override void visit(SemanticTree.IBasicTypeNode value)
        {

        }

        public override void visit(SemanticTree.ISimpleArrayNode value)
        {

        }

        //доступ к элементам массива
        public override void visit(SemanticTree.ISimpleArrayIndexingNode value)
        {
            //Console.WriteLine(value.array.type);
            bool temp_is_addr = is_addr;
            bool temp_is_dot_expr = is_dot_expr;
            is_addr = false;
            is_dot_expr = false;
            TypeInfo ti = helper.GetTypeReference(value.array.type);
            LocalBuilder tmp_current_index_lb = current_index_lb;
            current_index_lb = null;
            value.array.visit(this);
            current_index_lb = tmp_current_index_lb;
            bool string_getter = temp_is_addr && ti.tp == TypeFactory.StringType;
            LocalBuilder pin_lb = null;
            var indices = value.indices;
            if (string_getter)
            {
                pin_lb = il.DeclareLocal(TypeFactory.StringType, true);
                LocalBuilder chr_ptr_lb = il.DeclareLocal(TypeFactory.CharType.MakePointerType());
                //pinned_handle = il.DeclareLocal(TypeFactory.GCHandleType);
                Label false_lbl = il.DefineLabel();
                il.Emit(OpCodes.Stloc, pin_lb);
                /*il.Emit(OpCodes.Ldloc, pin_lb);
                il.Emit(OpCodes.Ldc_I4, (int)GCHandleType.Pinned);
                il.Emit(OpCodes.Call, TypeFactory.GCHandleAllocPinned);
                il.Emit(OpCodes.Stloc, pinned_handle);*/
                il.Emit(OpCodes.Ldloc, pin_lb);
                il.Emit(OpCodes.Conv_I);
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Brfalse_S, false_lbl);
                il.Emit(OpCodes.Call, TypeFactory.OffsetToStringDataProperty);
                il.Emit(OpCodes.Add);
                il.MarkLabel(false_lbl);
                il.Emit(OpCodes.Stloc, chr_ptr_lb);
                il.Emit(OpCodes.Ldloc, chr_ptr_lb);
            }
            //посещаем индекс
            MethodInfo get_meth = null;
            MethodInfo addr_meth = null;
            ISimpleArrayNode arr_type = value.array.type as ISimpleArrayNode;
            TypeInfo elem_ti = null;
            Type elem_type = null;
            if (arr_type != null)
            {
                elem_ti = helper.GetTypeReference(arr_type.element_type);
                elem_type = elem_ti.tp;
            }
            else
                elem_type = ti.tp.GetElementType();
            if (indices == null)
            {
                if (current_index_lb == null)
                    value.index.visit(this);
                else
                {
                    il.Emit(OpCodes.Ldloc, current_index_lb);
                    current_index_lb = null;
                }
                if (string_getter)
                {
                    Label except_lbl = il.DefineLabel();
                    Label ok_lbl = il.DefineLabel();
                    LocalBuilder ind_lb = il.DeclareLocal(helper.GetTypeReference(value.index.type).tp);
                    if (value.array.type is IShortStringTypeNode)
                    {
                        il.Emit(OpCodes.Ldc_I4_1);
                        il.Emit(OpCodes.Sub);
                    }
                    il.Emit(OpCodes.Stloc, ind_lb);
                    il.Emit(OpCodes.Ldloc, ind_lb);
                    il.Emit(OpCodes.Ldc_I4_0);
                    il.Emit(OpCodes.Blt, except_lbl);
                    il.Emit(OpCodes.Ldloc, ind_lb);
                    il.Emit(OpCodes.Ldloc, pin_lb);
                    il.Emit(OpCodes.Call, TypeFactory.StringLengthMethod);
                    il.Emit(OpCodes.Bge_S, except_lbl);
                    il.Emit(OpCodes.Ldloc, ind_lb);
                    il.Emit(OpCodes.Ldc_I4_2);
                    il.Emit(OpCodes.Mul);
                    il.Emit(OpCodes.Conv_I);
                    il.Emit(OpCodes.Add);
                    il.Emit(OpCodes.Br, ok_lbl);
                    il.MarkLabel(except_lbl);
                    il.Emit(OpCodes.Newobj, TypeFactory.IndexOutOfRangeConstructor);
                    il.Emit(OpCodes.Throw);
                    il.MarkLabel(ok_lbl);
                }
            }
            else
            {
                if (value.array.type is ICompiledTypeNode)
                {
                    get_meth = ti.tp.GetMethod("Get");
                    addr_meth = ti.tp.GetMethod("Address");
                }
                else
                {
                    List<Type> lst = new List<Type>();
                    for (int i = 0; i < value.indices.Length; i++)
                        lst.Add(TypeFactory.Int32Type);
                    get_meth = mb.GetArrayMethod(ti.tp, "Get", CallingConventions.HasThis, elem_type, lst.ToArray());
                    addr_meth = mb.GetArrayMethod(ti.tp, "Address", CallingConventions.HasThis, elem_type.MakeByRefType(), lst.ToArray());
                }
                
                for (int i = 0; i < indices.Length; i++)
                    indices[i].visit(this);
            }

            if (temp_is_addr)
            {
                if (value.indices == null)
                {
                    if (!string_getter)
                        il.Emit(OpCodes.Ldelema, elem_type);
                }
                else
                    il.Emit(OpCodes.Call, addr_meth);
            }
            else
                if (temp_is_dot_expr)
                {
                    if (elem_type.IsGenericParameter)
                    {
                        if (indices == null)
                            il.Emit(OpCodes.Ldelema, elem_type);
                        else
                            il.Emit(OpCodes.Call, addr_meth);
                }
                    else if (elem_type.IsValueType == true)
                    {
                        if (indices == null)
                            il.Emit(OpCodes.Ldelema, elem_type);
                        else
                            il.Emit(OpCodes.Call, addr_meth);
                    }
                    else if (elem_type.IsPointer)
                    {
                        if (indices == null)
                            il.Emit(OpCodes.Ldelem_I);
                        else
                            il.Emit(OpCodes.Call, addr_meth);
                    }
                    else
                        if (indices == null)
                            il.Emit(OpCodes.Ldelem_Ref);
                        else
                            il.Emit(OpCodes.Call, get_meth);

                }
                else
                {
                    if (indices == null)
                        NETGeneratorTools.PushLdelem(il, elem_type, true);
                    else
                        il.Emit(OpCodes.Call, get_meth);
                }
            is_addr = temp_is_addr;
            is_dot_expr = temp_is_dot_expr;
            //if (pinned_handle != null)
            //    pinned_variables.Add(pinned_handle);

        }

        public override void visit(SemanticTree.ITypeNode value)
        {

        }

        public override void visit(SemanticTree.IDefinitionNode value)
        {

        }

        public override void visit(SemanticTree.ISemanticNode value)
        {

        }

        public override void visit(SemanticTree.IReturnNode value)
        {
            if (save_debug_info)
                MarkSequencePoint(value.Location);

            OptMakeExitLabel();

            //(ssyy) Проверка на конструктор
            if (value.return_value != null)
            {
                if (!is_constructor)
                {
                    value.return_value.visit(this);
                }
            }
            il.Emit(OpCodes.Ret);
        }

        //строковая константа
        public override void visit(SemanticTree.IStringConstantNode value)
        {
            il.Emit(OpCodes.Ldstr, value.constant_value);
        }

        //реализация this
        public override void visit(SemanticTree.IThisNode value)
        {
            il.Emit(OpCodes.Ldarg_0);
            if (value.type.is_value_type && !is_dot_expr && !is_addr)
            {
                il.Emit(OpCodes.Ldobj, helper.GetTypeReference(value.type).tp);
            }
        }

        private void PushObjectCommand(SemanticTree.IFunctionCallNode ifc)
        {
            SemanticTree.ICommonNamespaceFunctionCallNode cncall = ifc as SemanticTree.ICommonNamespaceFunctionCallNode;
            if (cncall != null)
            {
                il.Emit(OpCodes.Ldnull);
                return;
            }
            SemanticTree.ICommonMethodCallNode cmcall = ifc as SemanticTree.ICommonMethodCallNode;
            if (cmcall != null)
            {
                cmcall.obj.visit(this);
                if (cmcall.obj.type.is_value_type)
                    il.Emit(OpCodes.Box, helper.GetTypeReference(cmcall.obj.type).tp);
                else if (cmcall.obj.conversion_type != null && cmcall.obj.conversion_type.is_value_type)
                	il.Emit(OpCodes.Box, helper.GetTypeReference(cmcall.obj.conversion_type).tp);
                return;
            }
            SemanticTree.ICommonStaticMethodCallNode csmcall = ifc as SemanticTree.ICommonStaticMethodCallNode;
            if (csmcall != null)
            {
                il.Emit(OpCodes.Ldnull);
                return;
            }
            SemanticTree.ICompiledMethodCallNode cmccall = ifc as SemanticTree.ICompiledMethodCallNode;
            if (cmccall != null)
            {
                cmccall.obj.visit(this);
                if (cmccall.obj.type.is_value_type)
                    il.Emit(OpCodes.Box, helper.GetTypeReference(cmccall.obj.type).tp);
                else if (cmccall.obj.conversion_type != null && cmccall.obj.conversion_type.is_value_type)
                	il.Emit(OpCodes.Box, helper.GetTypeReference(cmccall.obj.conversion_type).tp);
                return;
            }
            SemanticTree.ICompiledStaticMethodCallNode csmcall2 = ifc as SemanticTree.ICompiledStaticMethodCallNode;
            if (csmcall2 != null)
            {
                il.Emit(OpCodes.Ldnull);
                return;
            }
            SemanticTree.ICommonNestedInFunctionFunctionCallNode cnffcall = ifc as SemanticTree.ICommonNestedInFunctionFunctionCallNode;
            if (cnffcall != null)
            {
                //cnffcall.
                //TODO: Дописать код для этого случая.
                return;
            }
            return;
        }

        //Вызов конструктора параметра generic-типа
        public void ConvertGenericParamCtorCall(ICommonConstructorCall value)
        {
            Type gpar = helper.GetTypeReference(value.common_type).tp;
            MethodInfo create_inst = ActivatorCreateInstance.MakeGenericMethod(gpar);
            il.EmitCall(OpCodes.Call, create_inst, Type.EmptyTypes);
        }

        //вызов конструктора
        public override void visit(SemanticTree.ICommonConstructorCall value)
        {
            bool tmp_dot = is_dot_expr;
            //if (save_debug_info)
            //    MarkSequencePoint(value.Location);
            if (value.common_type.is_generic_parameter)
            {
                ConvertGenericParamCtorCall(value);
                return;
            }
            IExpressionNode[] real_parameters = value.real_parameters;
            IParameterNode[] parameters = value.static_method.parameters;
            MethInfo ci = helper.GetConstructor(value.static_method);
            ConstructorInfo cnstr = ci.cnstr;

            SemanticTree.IRuntimeManagedMethodBody irmmb = value.static_method.function_code as SemanticTree.IRuntimeManagedMethodBody;
            if (irmmb != null)
            {
                if (irmmb.runtime_statement_type == SemanticTree.runtime_statement_type.ctor_delegate)
                {
                    SemanticTree.IFunctionCallNode ifc = real_parameters[0] as SemanticTree.IFunctionCallNode;
                    MethodInfo mi = null;
                    ICompiledMethodCallNode icmcn = ifc as ICompiledMethodCallNode;
                    if (icmcn != null)
                    {
                        mi = icmcn.compiled_method.method_info;
                    }
                    else
                    {
                        ICompiledStaticMethodCallNode icsmcn = ifc as ICompiledStaticMethodCallNode;
                        if (icsmcn != null)
                        {
                            mi = icsmcn.static_method.method_info;
                        }
                        else
                        {
                            mi = helper.GetMethod(ifc.function).mi;
                        }
                    }
                    PushObjectCommand(ifc);
                    il.Emit(OpCodes.Ldftn, mi);
                    il.Emit(OpCodes.Newobj, cnstr);
                    return;
                }
                return;
            }

            if (!value.new_obj_awaited())
            {
                il.Emit(OpCodes.Ldarg_0);
            }

            is_dot_expr = false;
            bool need_fee = false;
            bool is_comp_gen = false;
            EmitArguments(parameters, real_parameters);
            if (value.new_obj_awaited())
            {
                il.Emit(OpCodes.Newobj, cnstr);
                var ti = helper.GetTypeReference(value.common_type);
                if (ti != null && ti.init_meth != null && value.common_type.is_value_type)
                {
                    LocalBuilder lb = il.DeclareLocal(ti.tp);
                    il.Emit(OpCodes.Stloc, lb);
                    il.Emit(OpCodes.Ldloca, lb);
                    il.Emit(OpCodes.Call, ti.init_meth);
                    il.Emit(OpCodes.Ldloc, lb);
                }
            }
            else
            {
                il.Emit(OpCodes.Call, cnstr);
            }
            EmitFreePinnedVariables();
            if (tmp_dot == true)
            {
                //MethodInfo mi = value.compiled_method.method_info;
                if (cnstr.DeclaringType.IsValueType && !NETGeneratorTools.IsPointer(cnstr.DeclaringType))
                {
                    LocalBuilder lb = il.DeclareLocal(cnstr.DeclaringType);
                    il.Emit(OpCodes.Stloc, lb);
                    il.Emit(OpCodes.Ldloca, lb);
                }
            }
            else
            {
                is_dot_expr = false;
            }
            if (init_call_awaited && !value.new_obj_awaited())
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Call, cur_ti.init_meth);
                init_call_awaited = false;
            }
        }

        //вызов откомпилированного конструктора
        public override void visit(SemanticTree.ICompiledConstructorCall value)
        {
            //if (save_debug_info) MarkSequencePoint(value.Location);
            bool tmp_dot = is_dot_expr;
            MethodInfo mi = null;
            IParameterNode[] parameters = value.constructor.parameters;
            IExpressionNode[] real_parameters = value.real_parameters;
            Type cons_type11 = value.constructor.comprehensive_type.compiled_type;
            if (cons_type11.BaseType == TypeFactory.MulticastDelegateType)
            {
                SemanticTree.IFunctionCallNode ifc = real_parameters[0] as SemanticTree.IFunctionCallNode;
                ICompiledMethodCallNode icmcn = ifc as ICompiledMethodCallNode;
                if (icmcn != null)
                {
                    mi = icmcn.compiled_method.method_info;
                }
                else
                {
                    ICompiledStaticMethodCallNode icsmcn = ifc as ICompiledStaticMethodCallNode;
                    if (icsmcn != null)
                    {
                        mi = icsmcn.static_method.method_info;
                    }
                    else
                    {
                        var meth = helper.GetMethod(ifc.function);
                        mi = meth.mi;
                    }
                }
                PushObjectCommand(ifc);
                il.Emit(OpCodes.Ldftn, mi);
                il.Emit(OpCodes.Newobj, value.constructor.constructor_info);
                return;
            }

            //ssyy
            if (!value.new_obj_awaited())
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            //\ssyy
            is_dot_expr = false;
            
            EmitArguments(parameters, real_parameters);
            //ssyy изменил
            if (value.new_obj_awaited())
            {
                il.Emit(OpCodes.Newobj, value.constructor.constructor_info);
            }
            else
            {
                il.Emit(OpCodes.Call, value.constructor.constructor_info);
            }
            EmitFreePinnedVariables();
            if (tmp_dot == true)
            {
                //MethodInfo mi = value.compiled_method.method_info;
                if (value.constructor.constructor_info.DeclaringType.IsValueType && !NETGeneratorTools.IsPointer(value.constructor.constructor_info.DeclaringType))
                {
                    LocalBuilder lb = il.DeclareLocal(value.constructor.constructor_info.DeclaringType);
                    il.Emit(OpCodes.Stloc, lb);
                    il.Emit(OpCodes.Ldloca, lb);
                }
            }
            else
            {
                is_dot_expr = false;
            }
            //\ssyy  
            if (init_call_awaited && !value.new_obj_awaited())
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Call, cur_ti.init_meth);
                init_call_awaited = false;
            }
        }

        private bool TypeNeedToFix(ITypeNode type)
        {
            switch (type.type_special_kind)
            {
                case type_special_kind.array_wrapper:
                case type_special_kind.array_kind:
                case type_special_kind.set_type:
                case type_special_kind.short_string:
                case type_special_kind.typed_file:
                    return true;
            }
            return false;
        }

        //перевод @
        public override void visit(SemanticTree.IGetAddrNode value)
        {
            IExpressionNode to = value.addr_of_expr;
            if (to is INamespaceVariableReferenceNode)
            {
                AddrOfNamespaceVariableNode(to as INamespaceVariableReferenceNode);
            }
            else if (to is ILocalVariableReferenceNode || to is ILocalBlockVariableReferenceNode)
            {
                AddrOfLocalVariableNode(to as IReferenceNode);
            }
            else if (to is ICommonParameterReferenceNode)
            {
                AddrOfParameterNode((ICommonParameterReferenceNode)to);
            }
            else if (to is ICommonClassFieldReferenceNode)
            {
                AddrOfField((ICommonClassFieldReferenceNode)to);
            }
            else if (to is IStaticCommonClassFieldReferenceNode)
            {
                AddrOfStaticField((IStaticCommonClassFieldReferenceNode)to);
            }
            else if (to is ICompiledFieldReferenceNode)
            {
                AddrOfCompiledField((ICompiledFieldReferenceNode)to);
            }
            else if (to is IStaticCompiledFieldReferenceNode)
            {
                AddrOfStaticCompiledField((IStaticCompiledFieldReferenceNode)to);
            }
            else if (to is ISimpleArrayIndexingNode)
            {
                AddrOfArrayIndexing((ISimpleArrayIndexingNode)to);
            }
            else if (to is IDereferenceNode)
            {
                (to as IDereferenceNode).derefered_expr.visit(this);
                return;
            }
            else if (to is IThisNode)
            {
                bool tmp = is_dot_expr;
                is_dot_expr = true;
                (to as IThisNode).visit(this);
                is_dot_expr = tmp;
                return;
            }
            if (TypeNeedToFix(to.type))
            {
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Ldind_Ref);
                FixPointer();
            }
            else
            {
                TypeInfo ti = helper.GetTypeReference(to.type);
                if (ti.fix_meth != null)
                {
                    il.Emit(OpCodes.Dup);
                    il.Emit(OpCodes.Call, ti.fix_meth);
                }
            }
        }

        private void AddrOfNamespaceVariableNode(INamespaceVariableReferenceNode value)
        {
            VarInfo vi = helper.GetVariable(value.variable);
            FieldBuilder fb = vi.fb;
            il.Emit(OpCodes.Ldsflda, fb);
        }

        private void AddrOfLocalVariableNode(IReferenceNode value)
        {
            VarInfo vi = helper.GetVariable(value.Variable);
            if (vi.kind == VarKind.vkLocal)
            {
                LocalBuilder lb = vi.lb;
                il.Emit(OpCodes.Ldloca, lb);
            }
            else if (vi.kind == VarKind.vkNonLocal)
            {
                FieldBuilder fb = vi.fb;
                MethInfo cur_mi = smi.Peek();
                int dist = (smi.Peek()).num_scope - vi.meth.num_scope;
                il.Emit(OpCodes.Ldloc, cur_mi.frame);
                for (int i = 0; i < dist; i++)
                {
                    il.Emit(OpCodes.Ldfld, cur_mi.disp.parent);
                    cur_mi = cur_mi.up_meth;
                }
                il.Emit(OpCodes.Ldflda, fb);
            }
        }

        private void AddrOfParameterNode(ICommonParameterReferenceNode value)
        {
            ParamInfo pi = helper.GetParameter(value.parameter);
            if (pi.kind == ParamKind.pkNone)
            {
                ParameterBuilder pb = pi.pb;
                //byte pos = (byte)(pb.Position-1);
                //***********************Kolay modified**********************
                byte pos = (byte)(pb.Position - 1);
                if (is_constructor || cur_meth.IsStatic == false) pos = (byte)pb.Position;
                else pos = (byte)(pb.Position - 1);
                //***********************End of Kolay modified**********************
                if (pos <= 255) il.Emit(OpCodes.Ldarga_S, pos);
                else il.Emit(OpCodes.Ldarga, pos);
            }
            else
            {
                FieldBuilder fb = pi.fb;
                MethInfo cur_mi = smi.Peek();
                int dist = (smi.Peek()).num_scope - pi.meth.num_scope;
                il.Emit(OpCodes.Ldloc, cur_mi.frame);
                for (int i = 0; i < dist; i++)
                {
                    il.Emit(OpCodes.Ldfld, cur_mi.disp.parent);
                    cur_mi = cur_mi.up_meth;
                }
                il.Emit(OpCodes.Ldflda, fb);
            }
        }

        private void AddrOfField(ICommonClassFieldReferenceNode value)
        {
            bool tmp_dot = is_dot_expr;
            if (tmp_dot == false)
                is_dot_expr = true;
            value.obj.visit(this);
            FieldInfo fi = helper.GetField(value.field).fi;
            il.Emit(OpCodes.Ldflda, fi);
            if (tmp_dot == false)
            {
                is_dot_expr = false;
            }
        }

        private void AddrOfStaticField(IStaticCommonClassFieldReferenceNode value)
        {
            bool tmp_dot = is_dot_expr;
            FieldInfo fi = helper.GetField(value.static_field).fi;
            il.Emit(OpCodes.Ldsflda, fi);
            if (tmp_dot == false)
            {
                is_dot_expr = false;
            }
        }

        private void AddrOfCompiledField(ICompiledFieldReferenceNode value)
        {
            if (value.field.compiled_field.IsLiteral == false)
            {
                value.obj.visit(this);
                il.Emit(OpCodes.Ldflda, value.field.compiled_field);
            }
        }

        private void AddrOfStaticCompiledField(IStaticCompiledFieldReferenceNode value)
        {
            if (value.static_field.compiled_field.IsLiteral == false)
            {
                il.Emit(OpCodes.Ldsflda, value.static_field.compiled_field);
            }
        }

        private void AddrOfArrayIndexing(ISimpleArrayIndexingNode value)
        {
            bool temp_is_addr = is_addr;
            bool temp_is_dot_expr = is_dot_expr;
            is_addr = false;
            is_dot_expr = false;
            var indices = value.indices;
            TypeInfo ti = helper.GetTypeReference(value.array.type);
            value.array.visit(this);
            il.Emit(OpCodes.Dup);
            FixPointer();
            //посещаем индекс
            //value.index.visit(this);
            ISimpleArrayNode arr_type = value.array.type as ISimpleArrayNode;
            TypeInfo elem_ti = null;
            Type elem_type = null;
            if (arr_type != null)
            {
                elem_ti = helper.GetTypeReference(arr_type.element_type);
                elem_type = elem_ti.tp;
            }
            else
                elem_type = ti.tp.GetElementType();
            MethodInfo addr_meth = null;
            if (indices == null)
            {
                value.index.visit(this);
            }
            else
            {
                if (value.array.type is ICompiledTypeNode)
                {
                    addr_meth = ti.tp.GetMethod("Address");
                }
                else
                {
                    List<Type> lst = new List<Type>();
                    for (int i = 0; i < indices.Length; i++)
                        lst.Add(TypeFactory.Int32Type);
                    addr_meth = mb.GetArrayMethod(ti.tp, "Address", CallingConventions.HasThis, elem_type.MakeByRefType(), lst.ToArray());
                }
                for (int i = 0; i < indices.Length; i++)
                    indices[i].visit(this);
            }

            if (value.indices == null)
                il.Emit(OpCodes.Ldelema, elem_type);
            else
                il.Emit(OpCodes.Call, addr_meth);
            is_addr = temp_is_addr;
            is_dot_expr = temp_is_dot_expr;
        }

        public override void visit(SemanticTree.IDereferenceNode value)
        {
            bool tmp = false;
            if (is_addr == true)
            {
                is_addr = false;
                tmp = true;
            }
            bool tmp_is_dot_expr = is_dot_expr;
            is_dot_expr = false;
            value.derefered_expr.visit(this);
            is_dot_expr = tmp_is_dot_expr;
            if (tmp == true) is_addr = true;
            TypeInfo ti = helper.GetTypeReference(((IRefTypeNode)value.derefered_expr.type).pointed_type);

            if (is_addr == false)
            {
                if (is_dot_expr == true)
                {
                    if (TypeIsClass(ti.tp)) il.Emit(OpCodes.Ldind_Ref);
                    else
                        if (ti.tp.IsPointer)
                            il.Emit(OpCodes.Ldind_I);
                }
                else
                {
                    NETGeneratorTools.PushParameterDereference(il, ti.tp);
                }
            }
            has_dereferences = true;
        }

        //перевод конструкции raise
        public override void visit(IThrowNode value)
        {
            value.exception_expresion.visit(this);
            il.Emit(OpCodes.Throw);
        }

        //перевод конструкции null
        public override void visit(INullConstantNode value)
        {
            il.Emit(OpCodes.Ldnull);
        }

        struct TmpForCase
        {
            public IStatementNode stmt;
            public IConstantNode cnst;
        }

        struct TmpForLabel
        {
            public Label[] labels;
            public int low_bound;
            private void Test() { }
        }

        //перевод конструкции case
        public override void visit(ISwitchNode value)
        {
            //if (save_debug_info)
            //    MarkSequencePoint(value.Location);

            Label default_case = il.DefineLabel();
            Label jump_def_label = il.DefineLabel();
            Label end_label;
            bool in_if = false;
            if (if_stack.Count == 0)
                end_label = il.DefineLabel();
            else
            {
                end_label = if_stack.Pop();
                in_if = true;
            }
            System.Collections.Generic.Dictionary<IConstantNode, Label> dict;
            TmpForLabel[] case_labels = GetCaseSelectors(value, jump_def_label, out dict);
            value.case_expression.visit(this);
            LocalBuilder lb = null;
            //if (case_labels.Length > 1)
            {
                lb = il.DeclareLocal(TypeFactory.Int32Type);
                il.Emit(OpCodes.Stloc, lb);
            }
            for (int i = 0; i < case_labels.Length; i++)
            {
                if (lb != null)
                {
                    il.Emit(OpCodes.Ldloc, lb);
                    //il.Emit(OpCodes.Ldc_I4, case_labels[i].low_bound);
                    NETGeneratorTools.LdcIntConst(il, case_labels[i].low_bound);
                    il.Emit(OpCodes.Sub);
                }
                else
                {
                    //il.Emit(OpCodes.Ldc_I4, case_labels[i].low_bound);
                    NETGeneratorTools.LdcIntConst(il, case_labels[i].low_bound);
                    il.Emit(OpCodes.Sub);
                }
                il.Emit(OpCodes.Switch, case_labels[i].labels);
            }
            il.MarkLabel(jump_def_label);
            ConvertRangedSelectors(value, end_label, lb);
            il.Emit(OpCodes.Br, default_case);

            foreach (ICaseVariantNode cvn in value.case_variants)
                ConvertCaseVariantNode(cvn, end_label, dict);
            CompleteRangedSelectors(value, end_label);
            il.MarkLabel(default_case);

            if (value.default_statement != null)
            {
                if (value.default_statement.Location != null)
                    MarkSequencePoint(il, value.default_statement.Location.begin_line_num, value.default_statement.Location.begin_column_num, value.default_statement.Location.begin_line_num, value.default_statement.Location.begin_column_num);
                value.default_statement.visit(this);
            }
            //MarkSequencePoint(il,0xFeeFee,0xFeeFee,0xFeeFee,0xFeeFee);
            if (!in_if)
                il.MarkLabel(end_label);
        }

        public override void visit(IStatementsExpressionNode value)
        {
            foreach (IStatementNode statement in value.statements)
            {
                ConvertStatement(statement);
            }
            value.expresion.visit(this);
        }

        public override void visit(IQuestionColonExpressionNode value)
        {
            Label EndLabel = il.DefineLabel();
            Label FalseLabel = il.DefineLabel();
            bool tmp_is_dot_expr = is_dot_expr;
            bool tmp_is_addr = is_addr;
            is_dot_expr = false;//don't box the condition expression
            is_addr = false;
            value.condition.visit(this);
            is_dot_expr = tmp_is_dot_expr;
            is_addr = tmp_is_addr;
            il.Emit(OpCodes.Brfalse, FalseLabel);
            value.ret_if_true.visit(this);
            var ti = helper.GetTypeReference(value.ret_if_true.type);
            if (ti != null)
                EmitBox(value.ret_if_true, ti.tp);
            il.Emit(OpCodes.Br, EndLabel);
            il.MarkLabel(FalseLabel);
            value.ret_if_false.visit(this);
            ti = helper.GetTypeReference(value.ret_if_false.type);
            if (ti != null)
                EmitBox(value.ret_if_false, ti.tp);
            il.MarkLabel(EndLabel);
            
        }

        private Hashtable range_stmts_labels = new Hashtable();

        //перевод селекторов-диапазонов case
        private void ConvertRangedSelectors(ISwitchNode value, Label end_label, LocalBuilder lb)
        {
            foreach (ICaseVariantNode cvn in value.case_variants)
            {
                var ranges = cvn.ranges;
                if (ranges.Length > 0)
                {
                    Label range_stmts_label = il.DefineLabel();
                    for (int i = 0; i < ranges.Length; i++)
                    {
                        Label false_label = il.DefineLabel();
                        il.Emit(OpCodes.Ldloc, lb);
                        ranges[i].lower_bound.visit(this);
                        il.Emit(OpCodes.Blt, false_label);
                        il.Emit(OpCodes.Ldloc, lb);
                        ranges[i].high_bound.visit(this);
                        il.Emit(OpCodes.Bgt, false_label);
                        il.Emit(OpCodes.Br, range_stmts_label);
                        range_stmts_labels[cvn.statement_to_execute] = range_stmts_label;
                        il.MarkLabel(false_label);
                    }
                }
            }
        }

        private void CompleteRangedSelectors(ISwitchNode value, Label end_label)
        {
            foreach (ICaseVariantNode cvn in value.case_variants)
            {
                if (cvn.ranges.Length > 0)
                {
                    il.MarkLabel((Label)range_stmts_labels[cvn.statement_to_execute]);
                    if (save_debug_info)
                        MarkSequencePoint(cvn.statement_to_execute.Location);
                    ConvertStatement(cvn.statement_to_execute);
                    il.Emit(OpCodes.Br, end_label);
                }
            }
        }

        //перевод селекторов case
        public void ConvertCaseVariantNode(ICaseVariantNode value, Label end_label, System.Collections.Generic.Dictionary<IConstantNode, Label> dict)
        {
            if (save_debug_info)
                MarkSequencePoint(value.statement_to_execute.Location);
            for (int i = 0; i < value.elements.Length; i++)
                il.MarkLabel(dict[value.elements[i]]);
            ConvertStatement(value.statement_to_execute);
            il.Emit(OpCodes.Br, end_label);
        }

        //сбор информации о селекторах (сортировка, группировка и т. д.)
        private TmpForLabel[] GetCaseSelectors(ISwitchNode value, Label default_label, out System.Collections.Generic.Dictionary<IConstantNode, Label> dict)
        {
            System.Collections.Generic.SortedDictionary<int, TmpForCase> sel_list = new System.Collections.Generic.SortedDictionary<int, TmpForCase>();
            System.Collections.Generic.Dictionary<ICaseRangeNode, IStatementNode> sel_range = new System.Collections.Generic.Dictionary<ICaseRangeNode, IStatementNode>();
            dict = new System.Collections.Generic.Dictionary<IConstantNode, Label>();
            //sobiraem informaciju o konstantah v case
            for (int i = 0; i < value.case_variants.Length; i++)
            {
                ICaseVariantNode cvn = value.case_variants[i];
                for (int j = 0; j < cvn.elements.Length; j++)
                {
                    IConstantNode cnst = cvn.elements[j];
                    if (cnst is IIntConstantNode)
                    {
                        TmpForCase tfc = new TmpForCase();
                        tfc.cnst = cnst;
                        tfc.stmt = cvn.statement_to_execute;
                        sel_list[((IIntConstantNode)cnst).constant_value] = tfc;
                    }
                    else if (cnst is ICharConstantNode)
                    {
                        TmpForCase tfc = new TmpForCase();
                        tfc.cnst = cnst;
                        tfc.stmt = cvn.statement_to_execute;
                        sel_list[(int)((ICharConstantNode)cnst).constant_value] = tfc;
                    }
                    else if (cnst is IBoolConstantNode)
                    {
                        TmpForCase tfc = new TmpForCase();
                        tfc.cnst = cnst;
                        tfc.stmt = cvn.statement_to_execute;
                        sel_list[Convert.ToInt32(((IBoolConstantNode)cnst).constant_value)] = tfc;
                    }
                }
            }
            System.Collections.Generic.List<int> lst = new System.Collections.Generic.List<int>();
            foreach (int val in sel_list.Keys)
            {
                lst.Add(val);
            }
            //sortiruem spisok perehodov v case
            lst.Sort();
            //int size = lst[lst.Count - 1] - lst[0] + 1;
            System.Collections.Generic.List<Label> label_list = new System.Collections.Generic.List<Label>();
            int last = 0;
            int size = 0;
            TmpForLabel tfl = new TmpForLabel();
            List<TmpForLabel> ltfl = new List<TmpForLabel>();
            //sozdaem metki dlja perehodov
            if (lst.Count > 0)
            {
                last = lst[0];
                size = 1;
                tfl.low_bound = last;//niznjaa granica
                Label l = il.DefineLabel();
                dict[sel_list[last].cnst] = l;//konstante sopostavim metku
                label_list.Add(l);
            }
            for (int i = 1; i < lst.Count; i++)
            {
                int dist = lst[i] - last;
                if (dist < 10)//esli rasstojanie mezhdu sosednimi konstantami nebolshoe
                {
                    last = lst[i];
                    size += dist;//pribavljaem rasstojanie
                    if (dist > 1)
                    {
                        for (int j = 1; j < dist; j++) //inache nado perehodit k proverke diapazonov
                            label_list.Add(default_label);
                    }
                    Label l = il.DefineLabel();
                    dict[sel_list[last].cnst] = l;
                    label_list.Add(l);
                }
                else
                {
                    tfl.labels = label_list.ToArray();//inache sozdaem otdelnuju tablicu perehodov
                    ltfl.Add(tfl);
                    tfl = new TmpForLabel();
                    label_list = new List<Label>();
                    tfl.low_bound = lst[i];
                    last = lst[i];
                    Label l = il.DefineLabel();
                    dict[sel_list[last].cnst] = l;
                    label_list.Add(l);
                }
            }
            tfl.labels = label_list.ToArray();
            ltfl.Add(tfl);
            return ltfl.ToArray();
        }

        public override void visit(SemanticTree.ICommonNestedInFunctionFunctionNode value)
        {

        }

        public override void visit(SemanticTree.ICommonNamespaceFunctionNode value)
        {

        }

        public override void visit(SemanticTree.ICommonFunctionNode value)
        {

        }

        public override void visit(SemanticTree.IBasicFunctionNode value)
        {

        }

        public override void visit(SemanticTree.INamespaceMemberNode value)
        {

        }

        public override void visit(SemanticTree.IFunctionMemberNode value)
        {

        }

        public override void visit(SemanticTree.ICommonClassMemberNode value)
        {

        }

        public override void visit(SemanticTree.ICompiledClassMemberNode value)
        {

        }

        public override void visit(SemanticTree.IClassMemberNode value)
        {

        }

        public override void visit(SemanticTree.IFunctionNode value)
        {

        }


        public override void visit(SemanticTree.IIsNode value)
        {
            bool idexpr = is_dot_expr;
            is_dot_expr = false;
            value.left.visit(this);
            if (!(value.left is INullConstantNode) && (value.left.type.is_value_type || value.left.type.is_generic_parameter))
                il.Emit(OpCodes.Box, helper.GetTypeReference(value.left.type).tp);
            is_dot_expr = idexpr;
            Type right = helper.GetTypeReference(value.right).tp;
            il.Emit(OpCodes.Isinst, right);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Cgt_Un);
            if (is_dot_expr)
                NETGeneratorTools.CreateLocalAndLoad(il, TypeFactory.BoolType);

        }

        public override void visit(SemanticTree.IAsNode value)
        {
            bool idexpr = is_dot_expr;
            is_dot_expr = false;
            value.left.visit(this);
            is_dot_expr = idexpr;
            Type right = helper.GetTypeReference(value.right).tp;
            if (!(value.left is SemanticTree.INullConstantNode) && (value.left.type.is_value_type || value.left.type.is_generic_parameter))
            {
                il.Emit(OpCodes.Box, helper.GetTypeReference(value.left.type).tp);
            }
            il.Emit(OpCodes.Isinst, right);
        }

        private void PushSize(Type t)
        {
            if (t.IsGenericParameter)
            {
                PushRuntimeSize(t);
                return;
            }
            if (t.IsGenericType)
            {
                PushRuntimeSize(t);
                return;
            }
            if (t.IsEnum)
            {
                PushIntConst(Marshal.SizeOf(TypeFactory.Int32Type));
                return;
            }
            PushRuntimeSize(t);
            return;
        }


        private void PushRuntimeSize(Type t)
        {
            if (!t.IsValueType)
                t = TypeFactory.IntPtr;
            if (t == TypeFactory.IntPtr)
            {
                il.Emit(OpCodes.Call, typeof(Environment).GetProperty("Is64BitProcess", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).GetGetMethod());
                Label brf = il.DefineLabel();
                Label bre = il.DefineLabel();
                il.Emit(OpCodes.Brfalse_S, brf);
                il.Emit(OpCodes.Ldc_I4_8);
                il.Emit(OpCodes.Br_S, bre);
                il.MarkLabel(brf);
                il.Emit(OpCodes.Ldc_I4_4);
                il.MarkLabel(bre);
            }
            else
            {
                //il.Emit(OpCodes.Ldtoken, t);
                NETGeneratorTools.PushTypeOf(il, t);
                Type typ = TypeFactory.MarshalType;
                Type[] prms = new Type[1];
                prms[0] = typeof(Type);
                il.EmitCall(OpCodes.Call, typ.GetMethod("SizeOf", prms), null);
            }
            return;
        }

        private void PushSizeForSizeof(Type t, ITypeNode tn)
        {
            PushSize(t);
        }

        public override void visit(ISizeOfOperator value)
        {
            //void.System.Runtime.InteropServices.Marshal.SizeOf()
            Type tp = helper.GetTypeReference(value.oftype).tp;
            if (tp.IsPrimitive)
            {
                PushIntConst(TypeFactory.GetPrimitiveTypeSize(tp));
                return;
            }
            PushSizeForSizeof(tp, value.oftype);
        }

        public override void visit(ITypeOfOperator value)
        {
            NETGeneratorTools.PushTypeOf(il, helper.GetTypeReference(value.oftype).tp);
        }

        public override void visit(IExitProcedure value)
        {
            if (!ExitProcedureCall)
                ExitLabel = il.DefineLabel();
            if (!safe_block)
                il.Emit(OpCodes.Br, ExitLabel);
            else
                il.Emit(OpCodes.Leave, ExitLabel);
            ExitProcedureCall = true;
        }

        public override void visit(IArrayConstantNode value)
        {
            il.Emit(OpCodes.Ldsfld, GetConvertedConstants(value));
        }

        public override void visit(IRecordConstantNode value)
        {
            if (is_dot_expr)
                il.Emit(OpCodes.Ldsflda, GetConvertedConstants(value));
            else
                il.Emit(OpCodes.Ldsfld, GetConvertedConstants(value));
        }

        public override void visit(IEnumConstNode value)
        {
            PushIntConst(value.constant_value);
        }

        public override void visit(IClassConstantDefinitionNode value)
        {
            FieldBuilder fb = null;
            if (value.type is ICompiledTypeNode && (value.type as ICompiledTypeNode).compiled_type.IsEnum)
                fb = cur_type.DefineField(value.name, TypeFactory.Int32Type, FieldAttributes.Literal | ConvertFALToFieldAttributes(value.field_access_level));
            else if (value.constant_value.value != null)
                fb = cur_type.DefineField(value.name, helper.GetTypeReference(value.type).tp, FieldAttributes.Literal | ConvertFALToFieldAttributes(value.field_access_level));
            else
                fb = cur_type.DefineField(value.name, helper.GetTypeReference(value.type).tp, FieldAttributes.Static | ConvertFALToFieldAttributes(value.field_access_level));
            if (value.constant_value.value != null)
                fb.SetConstant(value.constant_value.value);
            //else
            //    throw new Errors.CompilerInternalError("NetGenerator", new Exception("Invalid constant value in IClassConstantDefinitionNode"));
        }

        public override void visit(ICompiledStaticMethodCallNodeAsConstant value)
        {
            value.MethodCall.visit(this);
        }
        public override void visit(ICompiledConstructorCallAsConstant value)
        {
            value.MethodCall.visit(this);
        }

        public override void visit(ICommonNamespaceFunctionCallNodeAsConstant value)
        {
            value.MethodCall.visit(this);
        }

        public override void visit(IBasicFunctionCallNodeAsConstant value)
        {
            value.MethodCall.visit(this);
        }

        public override void visit(ICompiledStaticFieldReferenceNodeAsConstant value)
        {
            value.FieldReference.visit(this);
        }

        private void emit_numeric_conversion(Type to, Type from)
        {
            if (to != from)
            {
                if (helper.IsNumericType(to) && helper.IsNumericType(from))
                {
                    switch(Type.GetTypeCode(to))
                    {
                        case TypeCode.Byte: il.Emit(OpCodes.Conv_U1); break;
                        case TypeCode.SByte: il.Emit(OpCodes.Conv_I1); break;
                        case TypeCode.Int16: il.Emit(OpCodes.Conv_I2); break;
                        case TypeCode.UInt16: il.Emit(OpCodes.Conv_U2); break;
                        case TypeCode.Int32: il.Emit(OpCodes.Conv_I4); break;
                        case TypeCode.UInt32: il.Emit(OpCodes.Conv_U4); break;
                        case TypeCode.Int64: il.Emit(OpCodes.Conv_I8); break;
                        case TypeCode.UInt64: il.Emit(OpCodes.Conv_U8); break;
                        case TypeCode.Double: il.Emit(OpCodes.Conv_R8); break;
                        case TypeCode.Single: il.Emit(OpCodes.Conv_R4); break;
                    }
                }
            }
        }
        private void emit_conversion(Type t)
        {
            switch (Type.GetTypeCode(t))
            {
                case TypeCode.Byte: il.Emit(OpCodes.Call, TypeFactory.ConvertType.GetMethod("ToByte", new Type[1] { TypeFactory.ObjectType })); break;
                case TypeCode.SByte: il.Emit(OpCodes.Call, TypeFactory.ConvertType.GetMethod("ToSByte", new Type[1] { TypeFactory.ObjectType })); break;
                case TypeCode.Int16: il.Emit(OpCodes.Call, TypeFactory.ConvertType.GetMethod("ToInt16", new Type[1] { TypeFactory.ObjectType })); break;
                case TypeCode.UInt16: il.Emit(OpCodes.Call, TypeFactory.ConvertType.GetMethod("ToUInt16", new Type[1] { TypeFactory.ObjectType })); break;
                case TypeCode.Int32: il.Emit(OpCodes.Call, TypeFactory.ConvertType.GetMethod("ToInt32", new Type[1] { TypeFactory.ObjectType })); break;
                case TypeCode.UInt32: il.Emit(OpCodes.Call, TypeFactory.ConvertType.GetMethod("ToUInt32", new Type[1] { TypeFactory.ObjectType })); break;
                case TypeCode.Int64: il.Emit(OpCodes.Call, TypeFactory.ConvertType.GetMethod("ToInt64", new Type[1] { TypeFactory.ObjectType })); break;
                case TypeCode.UInt64: il.Emit(OpCodes.Call, TypeFactory.ConvertType.GetMethod("ToUInt64", new Type[1] { TypeFactory.ObjectType })); break;
                case TypeCode.Char: il.Emit(OpCodes.Call, TypeFactory.ConvertType.GetMethod("ToChar", new Type[1] { TypeFactory.ObjectType })); break;
                case TypeCode.Boolean: il.Emit(OpCodes.Call, TypeFactory.ConvertType.GetMethod("ToBoolean", new Type[1] { TypeFactory.ObjectType })); break;
                case TypeCode.Double: il.Emit(OpCodes.Call, TypeFactory.ConvertType.GetMethod("ToDouble", new Type[1] { TypeFactory.ObjectType })); break;
                case TypeCode.Single: il.Emit(OpCodes.Call, TypeFactory.ConvertType.GetMethod("ToSingle", new Type[1] { TypeFactory.ObjectType })); break;
                default: il.Emit(OpCodes.Unbox_Any, t); break;
            }
        }

        public override void visit(IForeachNode value)
        {
            VarInfo vi = helper.GetVariable(value.VarIdent);
            //Type interf = helper.GetTypeReference(value.InWhatExpr.type).tp;
            Type var_tp = helper.GetTypeReference(value.VarIdent.type).tp;
            //(ssyy) 12.04.2008 Поиск IEnumerable не нужен! Это дело семантики!
            Type in_what_type = helper.GetTypeReference(value.InWhatExpr.type).tp;
            Type return_type = null;
            bool is_generic = false;
            MethodInfo enumer_mi = null; //typeof(System.Collections.IEnumerable).GetMethod("GetEnumerator", Type.EmptyTypes);
            if (var_tp.IsValueType && !(in_what_type.IsArray && in_what_type.GetArrayRank() > 1))
            {
                enumer_mi = helper.GetEnumeratorMethod(in_what_type);
                if (enumer_mi == null)
                {
                    enumer_mi = typeof(System.Collections.IEnumerable).GetMethod("GetEnumerator", Type.EmptyTypes);
                    return_type = enumer_mi.ReturnType;
                }
                else
                {
                    is_generic = enumer_mi.ReturnType.IsGenericType;
                    return_type = enumer_mi.ReturnType;
                    if (in_what_type.IsGenericType && return_type.IsGenericType && !return_type.IsGenericTypeDefinition)
                        return_type = return_type.GetGenericTypeDefinition().MakeGenericType(in_what_type.GetGenericArguments());
                    else if (in_what_type.IsArray && return_type.IsGenericType && !return_type.IsGenericTypeDefinition)
                        return_type = return_type.GetGenericTypeDefinition().MakeGenericType(in_what_type.GetElementType());
                }
                
            }
            else
            {
                enumer_mi = typeof(System.Collections.IEnumerable).GetMethod("GetEnumerator", Type.EmptyTypes);
                return_type = enumer_mi.ReturnType;
            }
            LocalBuilder lb = il.DeclareLocal(return_type);
            if (save_debug_info) lb.SetLocalSymInfo("$enumer$" + uid++);
            value.InWhatExpr.visit(this);
            il.Emit(OpCodes.Callvirt, enumer_mi);
            il.Emit(OpCodes.Stloc, lb);
            Label exl = il.BeginExceptionBlock();
            Label l1 = il.DefineLabel();
            Label l2 = il.DefineLabel();
            Label leave_label = il.DefineLabel();
            il.Emit(OpCodes.Br, l2);
            il.MarkLabel(l1);
            if (vi.kind == VarKind.vkNonLocal)
                il.Emit(OpCodes.Ldloc, (smi.Peek() as MethInfo).frame);
            if (lb.LocalType.IsValueType)
                il.Emit(OpCodes.Ldloca, lb);
            else 
                il.Emit(OpCodes.Ldloc, lb);
            MethodInfo get_current_meth = enumer_mi.ReturnType.GetMethod("get_Current");
            if (enumer_mi.ReturnType.IsGenericType && !enumer_mi.ReturnType.IsGenericTypeDefinition)
            {
                if (helper.IsConstructedGenericType(return_type))
                    get_current_meth = TypeBuilder.GetMethod(return_type, enumer_mi.ReturnType.GetGenericTypeDefinition().GetMethod("get_Current"));
                else
                    get_current_meth = return_type.GetMethod("get_Current");
            }
            il.Emit(OpCodes.Callvirt, get_current_meth);
            if (vi.kind == VarKind.vkLocal)
            {
                if (!lb.LocalType.IsValueType && (vi.lb.LocalType.IsValueType || vi.lb.LocalType.IsGenericParameter))
                {
                    if (!is_generic)
                        emit_conversion(vi.lb.LocalType);
                    else
                        emit_numeric_conversion(vi.lb.LocalType, get_current_meth.ReturnType);
                }
                if (vi.lb.LocalType == TypeFactory.ObjectType && get_current_meth.ReturnType.IsValueType)
                    il.Emit(OpCodes.Box, get_current_meth.ReturnType);
                il.Emit(OpCodes.Stloc, vi.lb);
            }
            else if (vi.kind == VarKind.vkGlobal)
            {
                if (!lb.LocalType.IsValueType && (vi.fb.FieldType.IsValueType || vi.fb.FieldType.IsGenericParameter))
                {
                    if (!is_generic)
                        emit_conversion(vi.fb.FieldType);
                    else
                        emit_numeric_conversion(vi.fb.FieldType, get_current_meth.ReturnType);
                }
                if (vi.fb.FieldType == TypeFactory.ObjectType && get_current_meth.ReturnType.IsValueType)
                    il.Emit(OpCodes.Box, get_current_meth.ReturnType);
                il.Emit(OpCodes.Stsfld, vi.fb);
            }
            else
            {
                if (!lb.LocalType.IsValueType && (vi.fb.FieldType.IsValueType || vi.fb.FieldType.IsGenericParameter))
                {
                    if (!is_generic)
                        emit_conversion(vi.fb.FieldType);
                    else
                        emit_numeric_conversion(vi.fb.FieldType, get_current_meth.ReturnType);
                }
                if (vi.fb.FieldType == TypeFactory.ObjectType && get_current_meth.ReturnType.IsValueType)
                    il.Emit(OpCodes.Box, get_current_meth.ReturnType);
                il.Emit(OpCodes.Stfld, vi.fb);
            }
            labels.Push(leave_label);
            clabels.Push(l2);
            var safe_block = EnterSafeBlock();
            ConvertStatement(value.Body);
            LeaveSafeBlock(safe_block);
            //MarkSequencePoint(value.Location);
            il.MarkSequencePoint(doc, 0xFeeFee, 0xFeeFee, 0xFeeFee, 0xFeeFee);
            il.MarkLabel(l2);
            if (lb.LocalType.IsValueType)
                il.Emit(OpCodes.Ldloca, lb);
            else 
                il.Emit(OpCodes.Ldloc, lb);
            il.Emit(OpCodes.Callvirt, TypeFactory.IEnumeratorType.GetMethod("MoveNext", BindingFlags.Instance | BindingFlags.Public));
            il.Emit(OpCodes.Brtrue, l1);
            //il.Emit(OpCodes.Leave, leave_label);
            il.BeginFinallyBlock();
            //il.MarkLabel(br_lbl);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Stloc, lb);

            il.EndExceptionBlock();
            il.MarkLabel(leave_label);
            labels.Pop();
            clabels.Pop();
        }


        MethodInfo _monitor_enter = null;
        MethodInfo _monitor_exit = null;

        public override void visit(ILockStatement value)
        {
            if (_monitor_enter == null)
            {
                _monitor_enter = TypeFactory.MonitorType.GetMethod("Enter", new Type[1] { TypeFactory.ObjectType });
                _monitor_exit = TypeFactory.MonitorType.GetMethod("Exit", new Type[1] { TypeFactory.ObjectType });
            }
            LocalBuilder lb = il.DeclareLocal(TypeFactory.ObjectType);
            value.LockObject.visit(this);
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Stloc, lb);
            il.Emit(OpCodes.Call, _monitor_enter);
            il.BeginExceptionBlock();
            bool safe_block = EnterSafeBlock();
            ConvertStatement(value.Body);
            LeaveSafeBlock(safe_block);
            il.BeginFinallyBlock();
            il.Emit(OpCodes.Ldloc, lb);
            il.Emit(OpCodes.Call, _monitor_exit);
            il.EndExceptionBlock();
        }

        public override void visit(IRethrowStatement value)
        {
            il.Emit(OpCodes.Rethrow);
        }

        public override void visit(ILocalBlockVariableReferenceNode value)
        {
            VarInfo vi = helper.GetVariable(value.Variable);
            if (vi == null)
            {
                ConvertLocalVariable(value.Variable, false, 0, 0);
                vi = helper.GetVariable(value.Variable);
            }
            LocalBuilder lb = vi.lb;
            if (is_addr == false)//если это факт. var-параметр
            {
                if (is_dot_expr == true) //если после перем. в выражении стоит точка
                {
                    if (lb.LocalType.IsValueType == true || value.type.is_generic_parameter)
                    {
                        il.Emit(OpCodes.Ldloca, lb);//если перем. размерного типа кладем ее адрес
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldloc, lb);
                    }
                }
                else il.Emit(OpCodes.Ldloc, lb);
            }
            else il.Emit(OpCodes.Ldloca, lb);//в этом случае перем. - фактический var-параметр процедуры
        }

        public override void visit(INamespaceConstantReference value)
        {
            ConstInfo ci = helper.GetConstant(value.Constant);
            FieldBuilder fb = ci.fb;
            if (is_addr == false)//если это факт. var-параметр
            {
                if (is_dot_expr == true) //если после перем. в выражении стоит точка
                {
                    if (fb.FieldType.IsValueType == true)
                    {
                        il.Emit(OpCodes.Ldsflda, fb);//если перем. размерного типа кладем ее адрес
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldsfld, fb);
                    }
                }
                else il.Emit(OpCodes.Ldsfld, fb);
            }
            else il.Emit(OpCodes.Ldsflda, fb);
        }

        public override void visit(IFunctionConstantReference value)
        {
            ConstInfo ci = helper.GetConstant(value.Constant);
            FieldBuilder fb = ci.fb;
            if (is_addr == false)//если это факт. var-параметр
            {
                if (is_dot_expr == true) //если после перем. в выражении стоит точка
                {
                    if (fb.FieldType.IsValueType == true)
                    {
                        il.Emit(OpCodes.Ldsflda, fb);//если перем. размерного типа кладем ее адрес
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldsfld, fb);
                    }
                }
                else il.Emit(OpCodes.Ldsfld, fb);
            }
            else il.Emit(OpCodes.Ldsflda, fb);
        }

        public override void visit(ICommonConstructorCallAsConstant value)
        {
            value.ConstructorCall.visit(this);
        }

        public override void visit(IArrayInitializer value)
        {
            TypeInfo ti = helper.GetTypeReference(value.type);
            LocalBuilder lb = il.DeclareLocal(ti.tp);
            CreateArrayLocalVariable(il, lb, ti, value, value.type);
            il.Emit(OpCodes.Ldloc, lb);
        }

        public override void visit(IRecordInitializer value)
        {
            TypeInfo ti = helper.GetTypeReference(value.type);
            LocalBuilder lb = il.DeclareLocal(ti.tp);
            CreateRecordLocalVariable(il, lb, ti, value);
            il.Emit(OpCodes.Ldloc, lb);
        }

        public override void visit(ICommonEventNode value)
        {
            EventBuilder evb = cur_type.DefineEvent(value.Name, EventAttributes.None, helper.GetTypeReference(value.DelegateType).tp);
            if (value.AddMethod != null)
                evb.SetAddOnMethod(helper.GetMethodBuilder(value.AddMethod));
            if (value.RemoveMethod != null)
                evb.SetRemoveOnMethod(helper.GetMethodBuilder(value.RemoveMethod));
            if (value.RaiseMethod != null)
                evb.SetRaiseMethod(helper.GetMethodBuilder(value.RaiseMethod));
            helper.AddEvent(value, evb);
        }

        public override void visit(ICommonNamespaceEventNode value)
        {
            EventBuilder evb = cur_type.DefineEvent(value.Name, EventAttributes.None, helper.GetTypeReference(value.DelegateType).tp);
            if (value.AddFunction != null)
                evb.SetAddOnMethod(helper.GetMethodBuilder(value.AddFunction));
            if (value.RaiseFunction != null)
                evb.SetRemoveOnMethod(helper.GetMethodBuilder(value.RaiseFunction));
            if (value.RaiseFunction != null)
                evb.SetRaiseMethod(helper.GetMethodBuilder(value.RaiseFunction));
            helper.AddEvent(value, evb);
        }

        public override void visit(IDefaultOperatorNode value)
        {
            Type t = helper.GetTypeReference(value.type).tp;
            LocalBuilder def_var = il.DeclareLocal(t);
            il.Emit(OpCodes.Ldloca, def_var);
            il.Emit(OpCodes.Initobj, t);
            il.Emit(OpCodes.Ldloc, def_var);
        }

        public override void visit(INonStaticEventReference value)
        {
            value.obj.visit(this);
            if (value.Event is ICommonEventNode)
                il.Emit(OpCodes.Ldfld, helper.GetField((value.Event as ICommonEventNode).Field).fi);
        }

        public override void visit(IStaticEventReference value)
        {
            if (value.Event is ICommonEventNode)
                il.Emit(OpCodes.Ldsfld, helper.GetField((value.Event as ICommonEventNode).Field).fi);
            else
                il.Emit(OpCodes.Ldsfld, helper.GetVariable((value.Event as ICommonNamespaceEventNode).Field).fb);
        }

        public override void visit(SemanticTree.ILambdaFunctionNode value)
        {
        }

        public override void visit(SemanticTree.ILambdaFunctionCallNode value)
        {
        }
    }
}

