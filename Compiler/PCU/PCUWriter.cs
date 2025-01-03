// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.IO;
using System.Collections;
using System.Reflection;
using TreeConverter;
using PascalABCCompiler.TreeConverter;
using PascalABCCompiler.TreeRealization;
using System.Collections.Generic;
using System.Linq;

namespace PascalABCCompiler.PCU
{
    public class PCUConsts
    {
        public const byte template_meth = 250;
        public const byte template_field = 249;
        public const byte template_prop = 248;

        public const byte generic_meth = 240;
        public const byte generic_ctor = 239;
        public const byte generic_field = 238;
        public const byte generic_prop = 237;
        public const byte generic_param_ctor = 236;

        public const byte common_method_generic = 230;
        public const byte compiled_method_generic = 229;
        public const byte common_namespace_generic = 228;

        public const int method_instance_as_compiled_function_node = -1;
    }
   
    //класс, хранящий информацию об имени интерфейсной сущности и ее смещении в модуле
    public class NameRef {
		public string name;//имя сущности
        public TreeConverter.access_level access_level;//модификатор видимости
        public symbol_kind symbol_kind = symbol_kind.sk_none;
		public int offset;//смещение в модуле
        public byte special_scope=0;//говорит о том что этот символ добавляется не в пространсво имен модуля
		public int index;//не сериализуется
        public semantic_node_type semantic_node_type;
        public bool always_restore;
        public bool is_static;

        public NameRef(string name, int index, TreeConverter.access_level access_level, semantic_node_type semantic_node_type)
		{
			this.name = name;
			this.index = index;//4
            this.access_level = access_level;//1
            this.semantic_node_type = semantic_node_type;
		}
        public NameRef(string name, int index)
        {
            this.name = name;
            this.index = index;
            this.access_level = TreeConverter.access_level.al_none;
        }
        public int Size
        {
            get { return (System.Text.UTF8Encoding.UTF8.GetByteCount(name) + 1) + 1 + 4 + 1 + 1 + 1 + 1 + 1; }
        }
    }


    public class ClassInfo
    {
        public int names_pos;
        public int def_prop_off;
        public int base_class_off;
        public int interf_impl_off;
        public NameRef[] names;

        public ClassInfo(int names_pos, int def_prop_off, int base_class_off, int interf_impl_off, NameRef[] names)
        {
            this.names_pos = names_pos;
            this.def_prop_off = def_prop_off;
            this.base_class_off = base_class_off;
            this.interf_impl_off = interf_impl_off;
            this.names = names;
        }
    }

    //вид импортируемой сущности
	public enum ImportKind {
		Common,
		DotNet
	}

    //
    public enum TypeKind
    {
        Array=2,
        Pointer=3,
        UnsizedArray=4,
        BoundedArray=5,
        TemplateInstance = 6,
        GenericInstance = 7,
        ShortString = 8,
        GenericParameterOfType = 9,
        GenericParameterOfFunction = 10,
        GenericParameterOfMethod = 11,
        LambdaAnyType = 12
    }

    public enum GenericParamKind
    {
        None=0,
        Class=1,
        Value=2
    }

    //класс, описывающий импортируемую сущность
	public class ImportedEntity {
		public ImportKind flag; //откуда импортируется сущность (сборка или PCU)
		public int num_unit;//
		public int offset;//для PCU - смещение в модуле, содержащем сущность, для .NET - MetadataToken сущности
		
        public int index;//индекс записи в списке импортируемых сущностей (не сериализуется)
		
		public int GetSize()
		{
			return 9;
		}
		
		public static int GetClassSize()
		{
			return 9;
		}
	}
	
    public enum DotNetKind
    {
        Field,
        Method,
        Constructor,
        Type,
        Property
    }

    public class DotAdditInfo
    {
        public int offset;
    }

    public class DotNetNameRef
    {
        public DotNetKind kind;
        public string name;
        public DotAdditInfo[] addit;
    }

   
    /// <summary>
    /// Класс, описывающий заголовок PCU-файла
    /// </summary>
	public class PCUFile {

        public static char[] Header = new char[3] { 'P', 'C', 'U'};

        public static Int16 SupportedVersion = PCUFileFormatVersion.Version;
        public static int SupportedRevision = Convert.ToInt32(RevisionClass.Revision);
        public Int16 Version;
        public int Revision;
        public static int CRCOffset = 3 + 2;

        public Int64 CRC;

        public bool UseRtlDll;
        public bool IncludeDebugInfo;

        public string SourceFileName;

        public bool interfaceScopeCaseSensitive;
        
        public NameRef[] names; //список имен интерфейсной части модуля
		public string[] incl_modules; //список подключаемых модулей
		public string[] used_namespaces;
		public string[] ref_assemblies; //список подключаемых сборок
        public DotNetNameRef[] dotnet_names;
		//public AssemblyRef[] ref_assemblies;
		public ImportedEntity[] imp_entitles; //список импортируемых сущностей
        public List<TreeRealization.compiler_directive> compiler_directives; //список директив

        public bool implementationScopeCaseSensitive;
        //ssyy
        public NameRef[] implementation_names;
        public int interface_uses_count; //Количество модулей из incl_modules, относящихся к секции interface
        //public List<type_synonym> interface_type_synonyms;
        //public List<type_synonym> implementation_type_synonyms;
        public int interface_synonyms_offset;
        public int implementation_synonyms_offset;
        //public string[] implementation_incl_modules;
        //\ssyy
		
		public PCUFile() {}
	}
	
    /// <summary>
    /// Класс, создающий PCU-модуль
    /// </summary>
	public class PCUWriter {
		private BinaryWriter bw;
        private MemoryStream ms;
        private string name;//имя модуля
        public string FileName;//имя файла модуля
		private PCUFile pcu_file = new PCUFile(); //заголовок PCU
		private CompilationUnit unit;
        private common_unit_node cun; //текущий сериализуемый unit
		private Dictionary<definition_node,int> members = new Dictionary<definition_node,int>(); //таблица для хранения смещений сущностей модуля
        private Dictionary<definition_node,NameRef> name_pool = new Dictionary<definition_node,NameRef>();//таблица для связывания имени сущности с ее описанием
        //ssyy
        private Dictionary<definition_node, NameRef> impl_name_pool = new Dictionary<definition_node, NameRef>();//то же для implementation - части
        //\ssyy
        private List<ImportedEntity> imp_entitles = new List<ImportedEntity>();//список импортируемых сущностей, заполняется в процессе
		private List<string> ref_assemblies = new List<string>();//список подключаемых сборок, заполняется в процессе
		private Dictionary<Assembly,int> assm_refs = new Dictionary<Assembly,int>();//таблица для привязывания сборки
        private Dictionary<definition_node, int> func_codes = new Dictionary<definition_node, int>();//таблица, хранящая смещения тел методов в PCU
        private Dictionary<common_unit_node, int> used_units = new Dictionary<common_unit_node, int>();
		private Dictionary<definition_node,ImportedEntity> ext_members = new Dictionary<definition_node,ImportedEntity>();//таблица, привязывающая импорт. сущность с записью в списке имп. сущ-тей 
		private common_namespace_node cur_cnn;//текущее пространство имен
		private bool is_interface = true;//переводим ли интерфейсную часть
        //глобальная таблица для хранения смещений сущностей
        private static Dictionary<definition_node,int> gl_members = new Dictionary<definition_node,int>();
        private Dictionary<int, common_namespace_function_node> function_references = new Dictionary<int, common_namespace_function_node>();
        private Dictionary<int, common_type_node> type_references = new Dictionary<int, common_type_node>();
        //глобальная таблица, привязывающая импортируемую сущность, смещение которой пока неизвестно с PCUWriter
        private static Dictionary<definition_node, List<PCUWriter>> not_comp_members = new Dictionary<definition_node, List<PCUWriter>>();
       
        private static void add_not_comp_members(definition_node dn, PCUWriter pw)
        {
            if (not_comp_members.ContainsKey(dn))
            {
                not_comp_members[dn].Add(pw);
            }
            else
            {
                List<PCUWriter> l=new List<PCUWriter>();
                l.Add(pw);
                not_comp_members.Add(dn, l);
            }
        }
        
        //локальная таблица, привязывающая импортируемую сущность с записью в таблице импортируемых сущностей
        private Dictionary<definition_node, int> ext_offsets = new Dictionary<definition_node, int>();
        private PCUReader pcu_reader = null; //требуется, если например требуется перекомпилировать модули PCU с циклическими связями
        private Dictionary<definition_node, ClassInfo> class_info = new Dictionary<definition_node, ClassInfo>();
        private int InitializationMethodOffset = 0;
        private int FinalizationMethodOffset = 0;
        internal static List<PCUWriter> AllWriters=new List<PCUWriter>();
        private Compiler compiler;
        
        public delegate void ChangeStateDelegate(object Sender, PCUReaderWriterState State, object obj);
        
        public event ChangeStateDelegate ChangeState;

        //ivan
        private List<DotNetNameRef> dot_net_name_list = new List<DotNetNameRef>();
        private Hashtable tokens = new Hashtable();
        //ivan


        public PCUWriter(Compiler compiler, ChangeStateDelegate changeState) 
		{
            ChangeState += changeState;
            this.compiler = compiler;
            AllWriters.Add(this);
        }

        public static void Clear()
        {
            gl_members.Clear();
            not_comp_members.Clear();
            foreach (PCUWriter pw in AllWriters)
                if (pw.ext_offsets.Count > 0)
                    ;//ошибка?
            AllWriters.Clear();
        }

        //процедура сохранения модуля на диск
		public void SaveSemanticTree(CompilationUnit Unit, string TargetFileName, bool IncludeDebugInfo)
		{
            pcu_file.IncludeDebugInfo = IncludeDebugInfo;
            pcu_file.UseRtlDll = compiler.CompilerOptions.UseDllForSystemUnits;
            unit = Unit; cun = (common_unit_node)unit.SemanticTree;
            this.FileName = TargetFileName;
            string program_folder=System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(TargetFileName));
			name = System.IO.Path.GetFileName(TargetFileName);
			//name = name.Substring(0,name.LastIndexOf('.'))+".pcu";
			//FileStream fs = new FileStream(name,FileMode.Create,FileAccess.ReadWrite);
            
            ChangeState(this, PCUReaderWriterState.BeginSaveTree, TargetFileName);

            pcu_file.compiler_directives = cun.compiler_directives;
            if (Path.GetDirectoryName(TargetFileName).ToLower() == Path.GetDirectoryName(Unit.SyntaxTree.file_name).ToLower())
                pcu_file.SourceFileName = Path.GetFileName(Unit.SyntaxTree.file_name);
            else
                pcu_file.SourceFileName = Unit.SyntaxTree.file_name;
			ms = new MemoryStream();
			bw = new BinaryWriter(ms);
			//if (Unit.InterfaceUsedUnits.Count > 0) Console.WriteLine("{0} {1}",name,Unit.InterfaceUsedUnits[0].namespaces[0].namespace_name);
			cur_cnn = cun.namespaces[0];
            
            //(ssyy) формируем список нешаблонных классов
            cur_cnn.MakeNonTemplateTypesList();
            
            GetUsedUnits();//заполняем список полключаемых модулей

            pcu_file.interfaceScopeCaseSensitive = cun.scope.CaseSensitive;

            GetCountOfMembers();//заполняем список имен интерфейсных сущностей
			WriteUnit();//пишем имя interface_namespace
			cur_cnn = cun.namespaces[1];

            //(ssyy) формируем список нешаблонных классов
            cur_cnn.MakeNonTemplateTypesList();

            pcu_file.implementationScopeCaseSensitive = cun.implementation_scope.CaseSensitive;

            GetCountOfImplementationMembers();//(ssyy) заполняем список имен сущностей реализации
			WriteUnit();//пишем имя implementation_namespace
            SaveOffsetForAttribute(cun.namespaces[0]);
            bw.Write(0);//attributes;
			cur_cnn = cun.namespaces[0];
            //bw.Write((byte)0);
            //bw.Write((byte)0);

            //ssyy
            VisitTemplateClasses();//сериализуем шаблонные классы
            //\ssyy

            VisitTypeDefinitions();//сериализуем описания типов интерфейсной части

            //ssyy
            pcu_file.interface_synonyms_offset = (int)bw.BaseStream.Position;
            VisitTypeSynonyms();
            //\ssyy

            cur_cnn = cun.namespaces[1];
            is_interface = false;

            //ssyy
            VisitLabelDeclarations(cur_cnn.labels); //сериализуем метки
            type_entity_index = 0;
            VisitTemplateClasses();//сериализуем шаблонные классы
            //\ssyy
            
            VisitTypeDefinitions();//сериализуем описания типов имплемент. части
            //ssyy
            pcu_file.implementation_synonyms_offset = (int)bw.BaseStream.Position;
            VisitTypeSynonyms();
            //\ssyy
            cur_cnn = cun.namespaces[0];
            is_interface = true;
            foreach (common_type_node ctn in cur_cnn.non_template_types)
            {
                VisitTypeMemberDefinition(ctn);
            }
            cur_cnn = cun.namespaces[1];
            is_interface = false;
            foreach (common_type_node ctn in cur_cnn.non_template_types)
            {
                VisitTypeMemberDefinition(ctn);
            }
            cur_cnn = cun.namespaces[0];
            is_interface = true;
            VisitConstantDefinitions();//сериализуем константы
			VisitVariableDefinitions();//сериализуем переменные
            
			VisitFunctionDefinitions();//сериализуем функции
            VisitRefTypeDefinitions();
            VisitEventDefinitions();
            cur_cnn = cun.namespaces[1];
			is_interface = false;
			entity_index = 0;
			//имплементационная часть
			VisitConstantDefinitions();
			VisitVariableDefinitions();
			VisitFunctionDefinitions();
            VisitRefTypeDefinitions();
            VisitEventDefinitions();
            cur_cnn = cun.namespaces[0];
            //сериализуем тела функций
            foreach (common_namespace_function_node cnfn in cur_cnn.functions)
            {
                VisitFunctionImplementation(cnfn);
            }
            //сериализуем тела методов и конструкторов типа
            foreach (common_type_node ctn in cur_cnn.non_template_types)
            {
                VisitTypeImplementation(ctn);
            }
            cur_cnn = cun.namespaces[1];
            //имплементационная часть
			foreach (common_namespace_function_node cnfn in cur_cnn.functions)
			 VisitFunctionImplementation(cnfn);
            foreach (common_type_node ctn in cur_cnn.non_template_types)
            {
                VisitTypeImplementation(ctn);
            }
            
            WriteVariablePositions();
            WriteConstantPositions();
            AddAttributes();
            WriteInitExpressions();
            WriteFunctionReferences();
            WriteTypeReferences();
            //сохранение интерфейсной и имплементац. частей модуля
            if (ext_offsets.Count != 0)
            {
                List<definition_node> dnl = new List<definition_node>(ext_offsets.Keys);
                foreach (definition_node wdn in dnl)
                {
                    if (wdn is wrapped_definition_node)
                        AddOffsetForMembers(wdn, (wdn as wrapped_definition_node).offset);
                    else
                    if (wdn is wrapped_common_type_node)
                        AddOffsetForMembers(wdn, (wdn as wrapped_common_type_node).offset);
                    else
                    if (PCUReader.AllReadOrWritedDefinitionNodesOffsets.ContainsKey(wdn))
                        AddOffsetForMembers(wdn, PCUReader.AllReadOrWritedDefinitionNodesOffsets[wdn]);
                }
            }
            
		}

        //запись шапки PCU на диск
		private void WritePCUHeader(BinaryWriter fbw)
		{
			pcu_file.ref_assemblies = new string[ref_assemblies.Count];
			for (int i=0; i<ref_assemblies.Count; i++)
				pcu_file.ref_assemblies[i] = (string)ref_assemblies[i];
			pcu_file.imp_entitles = new ImportedEntity[imp_entitles.Count];
			for (int i=0; i<imp_entitles.Count; i++)
				pcu_file.imp_entitles[i] = imp_entitles[i];
            fbw.Write(PCUFile.Header);
            fbw.Write(PCUFile.SupportedVersion);
            fbw.Write(Convert.ToInt32(RevisionClass.Revision));
            fbw.Write((Int64)0);//CRC32
            fbw.Write(pcu_file.UseRtlDll);
            fbw.Write(pcu_file.IncludeDebugInfo);
            if (pcu_file.IncludeDebugInfo)
                fbw.Write(pcu_file.SourceFileName);

            fbw.Write(pcu_file.interfaceScopeCaseSensitive);

            fbw.Write(pcu_file.names.Length);
			for (int i=0; i<pcu_file.names.Length; i++)
			{
				fbw.Write(pcu_file.names[i].name);
				fbw.Write(pcu_file.names[i].offset);
                fbw.Write((byte)pcu_file.names[i].symbol_kind);
                fbw.Write(pcu_file.names[i].special_scope);
                fbw.Write(pcu_file.names[i].always_restore);
			}

            fbw.Write(pcu_file.implementationScopeCaseSensitive);

            //ssyy
            fbw.Write(pcu_file.implementation_names.Length);
            for (int i = 0; i < pcu_file.implementation_names.Length; i++)
            {
                fbw.Write(pcu_file.implementation_names[i].name);
                fbw.Write(pcu_file.implementation_names[i].offset);
                fbw.Write((byte)pcu_file.implementation_names[i].symbol_kind);
                fbw.Write(pcu_file.implementation_names[i].special_scope);
                fbw.Write(pcu_file.implementation_names[i].always_restore);
            }
            //\ssyy

			fbw.Write(pcu_file.incl_modules.Length);
			for (int i=0; i<pcu_file.incl_modules.Length; i++)
			{
				fbw.Write(pcu_file.incl_modules[i]);
			}
			
			fbw.Write(pcu_file.used_namespaces.Length);
			for (int i=0; i<pcu_file.used_namespaces.Length; i++)
			{
				fbw.Write(pcu_file.used_namespaces[i]);
			}
            //ssyy
            fbw.Write(pcu_file.interface_uses_count);
            //\ssyy

			fbw.Write(pcu_file.ref_assemblies.Length);
			for (int i=0; i<pcu_file.ref_assemblies.Length; i++)
			{
				fbw.Write(pcu_file.ref_assemblies[i]);
			}

            fbw.Write(pcu_file.compiler_directives.Count);
            for (int i = 0; i < pcu_file.compiler_directives.Count; i++)
            {
                fbw.Write(pcu_file.compiler_directives[i].name);
                if (pcu_file.compiler_directives[i].directive != null)
                fbw.Write(pcu_file.compiler_directives[i].directive);
                else
                fbw.Write("");
                WriteDebugInfo(fbw,pcu_file.compiler_directives[i].location);
            }

            fbw.Write(pcu_file.imp_entitles.Length);
			for (int i=0; i<pcu_file.imp_entitles.Length; i++)
			{
				fbw.Write((byte)pcu_file.imp_entitles[i].flag);
				fbw.Write(pcu_file.imp_entitles[i].num_unit);
				fbw.Write(pcu_file.imp_entitles[i].offset);
			}

            //ssyy
            fbw.Write(pcu_file.interface_synonyms_offset);
            fbw.Write(pcu_file.implementation_synonyms_offset);
            //\ssyy

            fbw.Write(InitializationMethodOffset);
            fbw.Write(FinalizationMethodOffset);

            //ivan net entities
            fbw.Write(dot_net_name_list.Count);
            for (int i = 0; i < dot_net_name_list.Count; i++)
            {
                fbw.Write((byte)dot_net_name_list[i].kind);
                fbw.Write(dot_net_name_list[i].name);
                fbw.Write(dot_net_name_list[i].addit.Length);
                for (int j = 0; j < dot_net_name_list[i].addit.Length; j++)
                    fbw.Write(dot_net_name_list[i].addit[j].offset);
            }
		}

        //отложенное добавление смещения импортирумой сущности в список импорт. сущностей
        //используется при циклических связях модулей
        private void AddOffsetForMembers(definition_node dn, int offset)
        {
            if (!ext_offsets.ContainsKey(dn))
            {
            	return;
            }
        	int pos = ext_offsets[dn]; //берем индекс в списке имп. сущ.
           
            imp_entitles[pos].offset = offset; // сохраняем смещение в другом модуле
            ext_offsets.Remove(dn); //удаляем сущность из таблицы
            //if (ext_offsets.Count == 0) 
                //CloseWriter(); // если не разрешенных зависимостей больше нет, записываем модуль на диск
        }
		
        private Dictionary<definition_node, List<int>> attr_dict = new Dictionary<definition_node, List<int>>();
        
        private void SaveOffsetForAttribute(definition_node dn)
        {
        	if (!attr_dict.ContainsKey(dn))
        	{
        		attr_dict[dn] = new List<int>();
        	}
        	attr_dict[dn].Add((int)bw.BaseStream.Position);
        }
        
        private void AddAttributes()
        {
        	foreach (definition_node dn in attr_dict.Keys)
        	{
        		SaveAttributes(dn.attributes,attr_dict[dn]);
        	}
        }
        
        private void WriteCRC32InHeader(byte[] buf, BinaryWriter bw)
        {
            /*Crc32 CRC32 = new Crc32();
            CRC32.Update(buf);
            bw.Seek(PCUFile.CRCOffset, SeekOrigin.Begin);
            bw.Write(CRC32.Value);*/
            bw.Write((Int64)0);

        }
        // запись PCU на диск
        internal void CloseWriter()
        {
            FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite);
            BinaryWriter fbw = new BinaryWriter(fs);
            WritePCUHeader(fbw);
            byte[] buf = new byte[bw.BaseStream.Length];
            bw.Seek(0, SeekOrigin.Begin);
            bw.BaseStream.Read(buf, 0, buf.Length);
            fbw.Write(buf);
            WriteCRC32InHeader(buf, fbw);
            bw.Close();
            ms.Close();
            fbw.Close();
            fs.Close();
            if (pcu_reader != null) 
                pcu_reader.OpenUnit();
            ChangeState(this, PCUReaderWriterState.EndSaveTree, FileName);
        }

        internal void RemoveSelf()
        {
            AllWriters.Remove(this);
        }

      
        public void AddPCUToOpen(PCUReader pr)
        {
            pcu_reader = pr;
        }

        //сохранен ли PCU на диск
        public bool IsSaved()
        {
            return ext_offsets.Count == 0;
        }
		
        private Dictionary<expression_node, List<int>> exprs_cache = new Dictionary<expression_node, List<int>>();
        
        private void SaveExpressionAndOffset(expression_node expr)
        {
        	List<int> lst = null;
        	if (!exprs_cache.ContainsKey(expr))
        	{
        		lst = new List<int>();
        		exprs_cache[expr] = lst;
        	}
        	else lst = exprs_cache[expr];
        	lst.Add((int)bw.BaseStream.Position);
        	//System.Diagnostics.Debug.WriteLine((int)bw.BaseStream.Position);
        }
        
        private void FixupExpressionPosition(expression_node expr)
        {
        	List<int> poses = exprs_cache[expr];
        	//System.Diagnostics.Debug.WriteLine(pos);
        	foreach (int pos in poses)
        	{
        		int tmp = (int)bw.BaseStream.Position;
				bw.Seek(pos,SeekOrigin.Begin);
				bw.Write(tmp);
				bw.Seek(tmp,SeekOrigin.Begin);
        	}
        }
        
        private void WriteInitExpressions()
        {
        	foreach (expression_node expr in exprs_cache.Keys)
        	{
        		FixupExpressionPosition(expr);
        		VisitExpression(expr);
        	}
        }

        private void WriteFunctionReferences()
        {
            foreach (int off in function_references.Keys)
            {
                common_namespace_function_node cnfn = function_references[off];
                int pos = (int)bw.BaseStream.Position;
                bw.Seek(off, SeekOrigin.Begin);
                WriteFunctionReference(cnfn);
                bw.Seek(pos, SeekOrigin.Begin);
            }
        }

        private void WriteTypeReferences()
        {
            foreach (int off in type_references.Keys)
            {
                common_type_node ctn = type_references[off];
                int pos = (int)bw.BaseStream.Position;
                bw.Seek(off, SeekOrigin.Begin);
                WriteTypeReference(ctn);
                bw.Seek(pos, SeekOrigin.Begin);
            }
        }

        //получения индекса сборки в списке подключаемых сборок
        private int GetAssemblyToken(Assembly a)
		{
            int pos = 0;
            if (assm_refs.TryGetValue(a, out pos))
            {
                return pos;
            }
			int num = ref_assemblies.Count;
			ref_assemblies.Add(a.FullName);
			assm_refs[a]=num;
			return num;
		}
		
        //получение смещения сущности в модуле
		private int GetMemberOffset(definition_node dn)
		{
            int off = members[dn];

            return members[dn];
		}
		
        //получения индекса в таблице интерфейсных имен
		private int GetNameIndex(definition_node dn)
		{
			return name_pool[dn].index;
		}
		
		private int GetUnitReference(namespace_node nn)
		{
			return members[nn];
		}
		
        //получение смещения в другом модуле
		private int GetExternalOffset(definition_node dn)
		{
            int off = -1;
            if (!gl_members.TryGetValue(dn, out off))
            {
                add_not_comp_members(dn, this);
                ext_offsets[dn] = imp_entitles.Count;
            }
            return off;
		}

        //сериализована ли сущность
		private bool IsDefined(definition_node dn)
		{
            return members.ContainsKey(dn);
		}
		
        //добавить внешнюю сущность
        public static void AddExternalMember(definition_node dn, int offset)
        {
            gl_members[dn] = offset;
        }
		
        private int entity_index=0;
        private int type_entity_index = 0;

        //сохранить смещение сущности в таблицах и в списке интерфейсных имен
		private int SavePositionAndConstPool(definition_node dn)
		{
            int pos = (int)bw.BaseStream.Position;
            members[dn] = pos;
			if (name_pool.ContainsKey(dn))
            {
                name_pool[dn].offset = pos;
                if (dn is common_namespace_function_node)
                    if ((dn as common_namespace_function_node).is_overload)
                        name_pool[dn].symbol_kind = symbol_kind.sk_overload_function;
            }
            
			gl_members[dn] = pos;
            List<PCUWriter> pwl = null;
            //если какой-то модуль ждет смещения от этой сущности, сообщаем ему
            if (not_comp_members.TryGetValue(dn, out pwl))
                foreach(PCUWriter pw in pwl) 
            	pw.AddOffsetForMembers(dn, pos);
            PCUReader.AddReadOrWritedDefinitionNode(dn, pos);
            return pos;
		}
        //то же самое, но только для членов класса (это лишнее можно объединить)
/*		private int SavePositionAndConstPoolInType(definition_node dn)
		{
            int pos = (int)bw.BaseStream.Position;
            members[dn] = pos;
			name_pool[dn].offset = pos;
			gl_members[dn] = pos;
            PCUWriter pw = null;
            if (not_comp_members.TryGetValue(dn,out pw))
             pw.AddOffsetForMembers(dn,pos);
            return pos;
		}*/
		
        //просто сохраняем смещение сущности
		private int SavePosition(definition_node dn)
		{
            int pos = (int)bw.BaseStream.Position;
            members[dn] = pos;
			gl_members[dn] = pos;
            return pos;
		}

        //(ssyy) сохраняем смещение сущности (для implementation)
        private int SavePositionAndImplementationPool(definition_node dn)
        {
            int pos = (int)bw.BaseStream.Position;
            members[dn] = pos;
            impl_name_pool[dn].offset = pos;
            gl_members[dn] = pos;
            return pos;
        }

        //связываем сущность со смещением, по которому находится тело методов
		private void SaveCodeReference(function_node fn)
		{
			func_codes[fn] = (int)bw.BaseStream.Position;
		}
		
        //сообщаем функции смещение, по которому расположен ее код
		private void FixupCode(function_node fn)
		{
			int tmp = (int)bw.BaseStream.Position;
			int pos = func_codes[fn];
			bw.Seek(pos,SeekOrigin.Begin);
			bw.Write(tmp);
			bw.Seek(tmp,SeekOrigin.Begin);
		}
		
        //заполнение списка интерфейсных имен модуля
		private void GetCountOfMembers()
		{
            int num = cur_cnn.constants.Count + cur_cnn.functions.Count + cur_cnn.non_template_types.Count + cur_cnn.runtime_types.Count + cur_cnn.variables.Count + cur_cnn.templates.Count + cur_cnn.ref_types.Count + cur_cnn.events.Count;
            pcu_file.names = new NameRef[num];
            FillNames(pcu_file.names, name_pool);
        }

        //ssyy
        private void GetCountOfImplementationMembers()
        {
            int num = cur_cnn.constants.Count + cur_cnn.functions.Count + cur_cnn.non_template_types.Count + cur_cnn.runtime_types.Count + cur_cnn.variables.Count + cur_cnn.templates.Count + cur_cnn.ref_types.Count + cur_cnn.events.Count;
            pcu_file.implementation_names = new NameRef[num];
            FillNames(pcu_file.implementation_names, impl_name_pool);
        }
        //\ssyy

        private string GetSynonimName(common_namespace_node cnn, compiled_type_node ctn)
        {
            for (int i = 0; i < cnn.type_synonyms.Count; i++)
            {
                if (cnn.type_synonyms[i].original_type == ctn) return cnn.type_synonyms[i].name;
            }
            return ctn.name;
        }
        //ssyy

        private bool HasPCUAlwaysRestoreAttribute(definition_node dn)
        {
            foreach (var attr in dn.attributes)
            {
                if (attr.attribute_type.full_name == "PABCSystem.PCUAlwaysRestoreAttribute")
                    return true;
            }
            return false;
        }

        private void FillNames(NameRef[] name_array, Dictionary<definition_node, NameRef> pools)
        {
            int j = 0, i = 0;
            for (i = j; i < cur_cnn.constants.Count + j; i++)
            {
                name_array[i] = new NameRef(cur_cnn.constants[i - j].name, i);
                pools[cur_cnn.constants[i - j]] = name_array[i];
            }
            j = i;
            for (i = j; i < cur_cnn.templates.Count + j; i++)
            {
                name_array[i] = new NameRef(cur_cnn.templates[i - j].name, i);
                pools[cur_cnn.templates[i - j]] = name_array[i];
            }
            j = i;
            for (i = j; i < cur_cnn.functions.Count + j; i++)
            {
                name_array[i] = new NameRef(cur_cnn.functions[i - j].name, i);
                if (cur_cnn.functions[i - j].ConnectedToType != null)
                {
                    name_array[i].special_scope = 1;
                    if (cur_cnn.functions[i - j].return_value_type != null)
                        name_array[i].symbol_kind = symbol_kind.sk_overload_function;
                    else
                        name_array[i].symbol_kind = symbol_kind.sk_overload_procedure;
                }
                if (HasPCUAlwaysRestoreAttribute(cur_cnn.functions[i - j]))
                    name_array[i].always_restore = true;
                pools[cur_cnn.functions[i - j]] = name_array[i];
            }
            j = i;
            for (i = j; i < cur_cnn.non_template_types.Count + j; i++)
            {
                name_array[i] = new NameRef(cur_cnn.non_template_types[i - j].name, i);
                if (HasPCUAlwaysRestoreAttribute(cur_cnn.non_template_types[i - j]))
                    name_array[i].always_restore = true;
                pools[cur_cnn.non_template_types[i - j]] = name_array[i];
            }
            j = i;
            for (i = j; i < cur_cnn.runtime_types.Count + j; i++)
            {
                name_array[i] = new NameRef(GetSynonimName(cur_cnn,cur_cnn.runtime_types[i - j]), i);
                //name_array[i] = new NameRef(cur_cnn.runtime_types[i - j].name, i);
                pools[cur_cnn.runtime_types[i - j]] = name_array[i];
            }
           
            j = i;
            for (i = j; i < cur_cnn.ref_types.Count + j; i++)
            {
                name_array[i] = new NameRef(cur_cnn.ref_types[i - j].name, i);
                pools[cur_cnn.ref_types[i - j]] = name_array[i];
            }
            j = i;
            for (i = j; i < cur_cnn.variables.Count + j; i++)
            {
                name_array[i] = new NameRef(cur_cnn.variables[i - j].name, i);
                if (HasPCUAlwaysRestoreAttribute(cur_cnn.variables[i - j]))
                    name_array[i].always_restore = true;
                pools[cur_cnn.variables[i - j]] = name_array[i];
            }
            j = i;
            for (i = j; i < cur_cnn.events.Count + j; i++)
            {
                name_array[i] = new NameRef(cur_cnn.events[i - j].name, i);
                pools[cur_cnn.events[i - j]] = name_array[i];
            }
        }
        //\ssyy

        private HashSet<type_node> added_indirect_types = new HashSet<type_node>();

        private void AddIndirectUsedUnitsForType(type_node tn, Dictionary<common_namespace_node, bool> ns_dict, bool interf)
        {
            if (!added_indirect_types.Add(tn)) return;
            if (!(tn is common_type_node ctn)) return;

            common_namespace_node comp_cnn = ctn.comprehensive_namespace;
            if (tn is common_generic_instance_type_node cgitn)
            {
                comp_cnn = cgitn.common_original_generic.comprehensive_namespace;
                foreach (type_node param_tn in cgitn.instance_params)
                    AddIndirectUsedUnitsForType(param_tn, ns_dict, interf);
            }

            if (comp_cnn != null && !ns_dict.ContainsKey(comp_cnn) && unit.SemanticTree != comp_cnn.cont_unit)
            {
                var path = Compiler.GetUnitPath(unit, compiler.UnitsTopologicallySortedList.Find(u => u.SemanticTree == comp_cnn.cont_unit));

                if (interf)
                    unit.InterfaceUsedUnits.AddElement(comp_cnn.cont_unit, path);
                else
                    unit.ImplementationUsedUnits.AddElement(comp_cnn.cont_unit, path);

                ns_dict[comp_cnn] = true;
            }

            AddIndirectUsedUnitsForType(tn.base_type, ns_dict, interf);
        }

        private void AddIndirectUsedUnitsForFunction(common_namespace_function_node cnfn, Dictionary<common_namespace_node, bool> ns_dict, bool interf)
        {
            common_namespace_node comp_cnn = cnfn.namespace_node;
            if (comp_cnn != null && !ns_dict.ContainsKey(comp_cnn) && unit.SemanticTree != comp_cnn.cont_unit)
            {
                var path = Compiler.GetUnitPath(unit, compiler.UnitsTopologicallySortedList.Find(u => u.SemanticTree == comp_cnn.cont_unit));

                if (interf)
                    unit.InterfaceUsedUnits.AddElement(comp_cnn.cont_unit, path);
                else
                    unit.ImplementationUsedUnits.AddElement(comp_cnn.cont_unit, path);

                ns_dict[comp_cnn] = true;
            }
        }

        private void AddIndirectUsedUnitsInStatement(statement_node stmt, Dictionary<common_namespace_node, bool> ns_dict, bool interf)
        {
            if (stmt == null)
                return;
            if (stmt is statements_list)
            {
                statements_list stmt_list = stmt as statements_list;
                foreach (local_block_variable lv in stmt_list.local_variables)
                {
                    AddIndirectUsedUnitsForType(lv.type, ns_dict, interf);
                }
                foreach (statement_node st in stmt_list.statements)
                    AddIndirectUsedUnitsInStatement(st, ns_dict, interf);
            }
            else if (stmt is common_static_method_call)
            {
                common_static_method_call csmc = stmt as common_static_method_call;
                AddIndirectUsedUnitsForType(csmc.common_type, ns_dict, interf);
            }
            else if (stmt is common_method_call)
            {
                common_method_call cmc = stmt as common_method_call;
                AddIndirectUsedUnitsForType(cmc.function_node.cont_type, ns_dict, interf);
            }
            else if (stmt is common_namespace_function_call)
            {
                common_namespace_function_call cnfc = stmt as common_namespace_function_call;
                if (cnfc.function_node.ConnectedToType != null)
                    AddIndirectUsedUnitsForFunction(cnfc.function_node, ns_dict, interf);
            }
            else if (stmt is if_node)
            {
                if_node node = stmt as if_node;
                AddIndirectUsedUnitsInStatement(node.condition, ns_dict, interf);
                AddIndirectUsedUnitsInStatement(node.then_body, ns_dict, interf);
                AddIndirectUsedUnitsInStatement(node.else_body, ns_dict, interf);
            }
            else if (stmt is while_node)
            {
                while_node node = stmt as while_node;
                AddIndirectUsedUnitsInStatement(node.condition, ns_dict, interf);
                AddIndirectUsedUnitsInStatement(node.body, ns_dict, interf);
            }
            else if (stmt is repeat_node)
            {
                repeat_node node = stmt as repeat_node;
                AddIndirectUsedUnitsInStatement(node.condition, ns_dict, interf);
                AddIndirectUsedUnitsInStatement(node.body, ns_dict, interf);
            }
            else if (stmt is lock_statement)
            {
                lock_statement node = stmt as lock_statement;
                AddIndirectUsedUnitsInStatement(node.lock_object, ns_dict, interf);
                AddIndirectUsedUnitsInStatement(node.body, ns_dict, interf);
            }
            else if (stmt is foreach_node)
            {
                foreach_node node = stmt as foreach_node;
                AddIndirectUsedUnitsInStatement(node.in_what, ns_dict, interf);
                AddIndirectUsedUnitsInStatement(node.what_do, ns_dict, interf);
            }
            else if (stmt is for_node)
            {
                for_node node = stmt as for_node;
                AddIndirectUsedUnitsInStatement(node.init_while_expr, ns_dict, interf);
                AddIndirectUsedUnitsInStatement(node.while_expr, ns_dict, interf);
            }
            else if (stmt is try_block)
            {
                try_block node = stmt as try_block;
                AddIndirectUsedUnitsInStatement(node.try_statements, ns_dict, interf);
                foreach (var eh in node.filters)
                    AddIndirectUsedUnitsInStatement(eh.exception_handler, ns_dict, interf);
                AddIndirectUsedUnitsInStatement(node.finally_statements, ns_dict, interf);
            }
            else if (stmt is throw_statement_node)
            {
                throw_statement_node node = stmt as throw_statement_node;
                AddIndirectUsedUnitsInStatement(node.excpetion, ns_dict, interf);
            }
            if (stmt is base_function_call)
            {
                base_function_call bfc = stmt as base_function_call;
                foreach (var expr in bfc.parameters)
                {
                    AddIndirectUsedUnitsInStatement(expr, ns_dict, interf);
                }
            }
            
        }

        private void AddIndirectUsedUnitsInVariables<T>(IEnumerable<T> variables, Dictionary<common_namespace_node, bool> ns_dict, bool interf) where T: var_definition_node
        {
            if (variables == null)
                return;
            foreach (var lv in variables)
            {
                AddIndirectUsedUnitsForType(lv.type, ns_dict, interf);
            }
        }

        private void AddIndirectInteraceUsedUnits()
        {
            if (cun.namespaces.Count == 0)
                return;
            Dictionary<common_namespace_node, bool> interf_ns_dict = new Dictionary<common_namespace_node, bool>();
            common_unit_node unt = null;
            for (int j = 0; j < unit.InterfaceUsedUnits.Count; j++)
            {
                unt = unit.InterfaceUsedUnits[j] as common_unit_node;
                if (unt == null) continue;
                foreach (common_namespace_node ns in unt.namespaces)
                {
                    interf_ns_dict[ns] = true;
                }
            }
            common_namespace_node cnn = cun.namespaces[0];
            foreach (type_synonym ts in cnn.type_synonyms)
                AddIndirectUsedUnitsForType(ts.original_type, interf_ns_dict, true);
            foreach (common_type_node ctn in cnn.types)
            {
                if (ctn.base_type != null)
                    AddIndirectUsedUnitsForType(ctn.base_type, interf_ns_dict, true);
                foreach (common_method_node cmn in ctn.methods)
                {
                    if (cmn.is_constructor && cmn.function_code != null && cmn.function_code.location == null)
                    {
                        foreach (common_parameter cp in cmn.parameters)
                        {
                            AddIndirectUsedUnitsForType(cp.type, interf_ns_dict, true);
                        }
                    }
                    AddIndirectUsedUnitsInVariables(cmn.var_definition_nodes_list, interf_ns_dict, true);
                    AddIndirectUsedUnitsInStatement(cmn.function_code, interf_ns_dict, true);
                    if (cmn.return_value_type != null)
                        AddIndirectUsedUnitsForType(cmn.return_value_type, interf_ns_dict, true);
                }
                AddIndirectUsedUnitsInVariables(ctn.fields, interf_ns_dict, true);
            }
            foreach (common_namespace_function_node cnfn in cnn.functions)
            {
                AddIndirectUsedUnitsInVariables(cnfn.var_definition_nodes_list, interf_ns_dict, true);
                AddIndirectUsedUnitsInStatement(cnfn.function_code, interf_ns_dict, true);
                if (cnfn.return_value_type != null)
                    AddIndirectUsedUnitsForType(cnfn.return_value_type, interf_ns_dict, true);
            }
            AddIndirectUsedUnitsInVariables(cnn.variables, interf_ns_dict, true);
            if (cun.main_function != null)
                AddIndirectUsedUnitsInStatement(cun.main_function.function_code, interf_ns_dict, true);
            if (cun.finalization_method != null)
                AddIndirectUsedUnitsInStatement(cun.finalization_method.function_code, interf_ns_dict, true);
        }
        
        private void AddIndirectImplementationUsedUnits()
        {
            if (cun.namespaces.Count < 2)
                return;
            common_unit_node unt = null;
            Dictionary<common_namespace_node, bool> impl_ns_dict = new Dictionary<common_namespace_node, bool>();
            for (int j = 0; j < unit.ImplementationUsedUnits.Count; j++)
            {
                unt = unit.ImplementationUsedUnits[j] as common_unit_node;
                if (unt == null) continue;
                foreach (common_namespace_node ns in unt.namespaces)
                {
                    impl_ns_dict[ns] = true;
                }
            }
            common_namespace_node cnn = cun.namespaces[1];
            foreach (type_synonym ts in cnn.type_synonyms)
                AddIndirectUsedUnitsForType(ts.original_type, impl_ns_dict, true);
            foreach (common_type_node ctn in cnn.types)
            {
                if (ctn.base_type != null)
                    AddIndirectUsedUnitsForType(ctn.base_type, impl_ns_dict, true);
                foreach (common_method_node cmn in ctn.methods)
                {
                    if (cmn.is_constructor && cmn.function_code != null && cmn.function_code.location == null)
                    {
                        foreach (common_parameter cp in cmn.parameters)
                        {
                            AddIndirectUsedUnitsForType(cp.type, impl_ns_dict, false);
                        }
                    }
                    AddIndirectUsedUnitsInVariables(cmn.var_definition_nodes_list, impl_ns_dict, false);
                    AddIndirectUsedUnitsInStatement(cmn.function_code, impl_ns_dict, false);
                    if (cmn.return_value_type != null)
                        AddIndirectUsedUnitsForType(cmn.return_value_type, impl_ns_dict, false);
                }
                AddIndirectUsedUnitsInVariables(ctn.fields, impl_ns_dict, false);
            }
            foreach (common_namespace_function_node cnfn in cnn.functions)
            {
                AddIndirectUsedUnitsInVariables(cnfn.var_definition_nodes_list, impl_ns_dict, false);
                AddIndirectUsedUnitsInStatement(cnfn.function_code, impl_ns_dict, true);
                if (cnfn.return_value_type != null)
                    AddIndirectUsedUnitsForType(cnfn.return_value_type, impl_ns_dict, true);
            }
            AddIndirectUsedUnitsInVariables(cnn.variables, impl_ns_dict, true);
        }
        
        //заполнение списка подключаемых модулей
		private void GetUsedUnits()
		{
            AddIndirectInteraceUsedUnits();
            AddIndirectImplementationUsedUnits();

            var c1 = unit.InterfaceUsedUnits.unit_uses_paths.Count;
            var c2 = unit.ImplementationUsedUnits.unit_uses_paths.Count;
            pcu_file.interface_uses_count = c1;

            var incl_modules = new List<string>(c1 + c2);
            foreach (var used_unit in unit.InterfaceUsedUnits.OfType<common_unit_node>())
            {
                // Каждый модуль, зачем то, подключён сам к себе
                // Конечно, записи в unit_uses_paths для такого подключения - нет
                if (unit.SemanticTree == used_unit) continue;

                // AddIndirectInteraceUsedUnits может добавлять один и тот же модуль несколько раз
                if (used_units.ContainsKey(used_unit)) continue;

                // раньше вместо пути модуля - брало имя его первого пространства имён
                // и сразу стояла эта проверка. Если будет тут вылетать - наверное надо заменить throw на continue
                if (used_unit.namespaces.Count == 0) throw new InvalidOperationException();

                this.used_units.Add(used_unit, used_units.Count);
                incl_modules.Add(unit.InterfaceUsedUnits.unit_uses_paths[used_unit]);

            }
            foreach (var used_unit in unit.ImplementationUsedUnits.OfType<common_unit_node>())
            {
                if (unit.SemanticTree == used_unit) continue;
                if (used_units.ContainsKey(used_unit)) continue;
                if (used_unit.namespaces.Count == 0) throw new InvalidOperationException();

                this.used_units.Add(used_unit, used_units.Count);
                incl_modules.Add(unit.ImplementationUsedUnits.unit_uses_paths[used_unit]);

            }

            used_units.Add(unit.SemanticTree as common_unit_node, -1);

            pcu_file.incl_modules = incl_modules.ToArray();
            pcu_file.used_namespaces = cun.used_namespaces.ToArray();
		}
		
        //получения индекса модуля в списке подключ. модулей
		private int GetUnitToken(common_namespace_node ns)
		{
            return used_units[ns.cont_unit as common_unit_node];
		}
		
        private int GetTokenForNetEntity(FieldInfo val)
        {
            int off=0;
            object o = tokens[val];
            if (o != null) return (int)o;
            DotNetNameRef dnnr = new DotNetNameRef();
            dnnr.kind = DotNetKind.Field;
            dnnr.name = val.Name;
            dnnr.addit = new DotAdditInfo[0];
            off = dot_net_name_list.Count;
            tokens.Add(val, dot_net_name_list.Count);
            dot_net_name_list.Add(dnnr);
            return off;
        }

        private int GetTokenForNetEntity(MethodInfo val)
        {
            int off = 0;
            //if (tokens.TryGetValue(val, out off)) return off;
            object o = tokens[val];
            if (o != null) return (int)o;
            DotNetNameRef dnnr = new DotNetNameRef();
            dnnr.kind = DotNetKind.Method;
            dnnr.name = val.Name;
            //if (val.GetParameters() != null)
            //{
                ParameterInfo[] prms = val.GetParameters();
                
                if (prms != null)
                {
                    if (val.Name != "op_Explicit")
                        dnnr.addit = new DotAdditInfo[prms.Length];
                    else dnnr.addit = new DotAdditInfo[prms.Length+1];
                    for (int i = 0; i < prms.Length; i++)
                    {
                        dnnr.addit[i] = new DotAdditInfo();
                        dnnr.addit[i].offset = GetTokenForNetEntity(prms[i].ParameterType);
                    }
                    if (val.Name == "op_Explicit")
                    {
                        dnnr.addit[prms.Length] = new DotAdditInfo();
                        dnnr.addit[prms.Length].offset = GetTokenForNetEntity(val.ReturnType);
                    }
                }
                else dnnr.addit = new DotAdditInfo[0];
            //}
            off = dot_net_name_list.Count;
            tokens.Add(val, dot_net_name_list.Count);
            dot_net_name_list.Add(dnnr);
            return off;
        }

        private int GetTokenForNetEntity(ConstructorInfo val)
        {
            int off = 0;
            //if (tokens.TryGetValue(val, out off)) return off;
            object o = tokens[val];
            if (o != null) return (int)o;
            DotNetNameRef dnnr = new DotNetNameRef();
            dnnr.kind = DotNetKind.Constructor;
            dnnr.name = ".ctor";
            //if (val.GetParameters() != null)
            //{
                ParameterInfo[] prms = val.GetParameters();
                if (prms != null)
                {
                    dnnr.addit = new DotAdditInfo[prms.Length];
                    for (int i = 0; i < prms.Length; i++)
                    {
                        dnnr.addit[i] = new DotAdditInfo();
                        dnnr.addit[i].offset = GetTokenForNetEntity(prms[i].ParameterType);
                    }
                }
                else dnnr.addit = new DotAdditInfo[0];
            //}
            off = dot_net_name_list.Count;
            tokens.Add(val, dot_net_name_list.Count);
            dot_net_name_list.Add(dnnr);
            return off;
        }

        private int GetTokenForNetEntity(PropertyInfo val)
        {
            int off = 0;
            //if (tokens.TryGetValue(val, out off)) return off;
            object o = tokens[val];
            if (o != null) return (int)o;
            DotNetNameRef dnnr = new DotNetNameRef();
            dnnr.kind = DotNetKind.Property;
            dnnr.name = val.Name;
            //if (val.GetParameters() != null)
            //{
            /*ParameterInfo[] prms = val.GetIndexParameters();
            if (prms != null)
            {
                dnnr.addit = new DotAdditInfo[prms.Length];
                for (int i = 0; i < prms.Length; i++)
                {
                    dnnr.addit[i] = new DotAdditInfo();
                    dnnr.addit[i].offset = GetTokenForNetEntity(prms[i].ParameterType);
                }
            }
            else*/
            dnnr.addit = new DotAdditInfo[0];
            //}
            off = dot_net_name_list.Count;
            tokens.Add(val, dot_net_name_list.Count);
            dot_net_name_list.Add(dnnr);
            return off;
        }

        private int GetTokenForNetEntity(Type val)
        {
            int off = 0;
            //if (tokens.TryGetValue(val, out off)) return off;
            object o = tokens[val];
            if (o != null) return (int)o;
            DotNetNameRef dnnr = new DotNetNameRef();
            dnnr.kind = DotNetKind.Type;
            if (val.FullName != null)
            {
                //(ssyy) для инстанций generic-ов пишем имя оригинала
                if (val.IsGenericType && !val.IsGenericTypeDefinition)
                    dnnr.name = val.GetGenericTypeDefinition().FullName;
                else
                    dnnr.name = val.FullName;
            }
            else if (val.IsGenericType && !val.IsGenericTypeDefinition)
                dnnr.name = val.GetGenericTypeDefinition().FullName;
            else
                dnnr.name = val.Name;
           // if (val.IsGenericType)
            //{
                Type[] tt = (val.IsGenericTypeDefinition)?
                    new Type[0]
                    :
                    val.GetGenericArguments();
                dnnr.addit = new DotAdditInfo[tt.Length];
                for (int i = 0; i < tt.Length; i++)
                {
                    dnnr.addit[i] = new DotAdditInfo();
                    dnnr.addit[i].offset = GetTokenForNetEntity(tt[i]);
                }
            //}
            off = dot_net_name_list.Count;
            tokens.Add(val, dot_net_name_list.Count);
            dot_net_name_list.Add(dnnr);
            return off;
        }

        //получения ссылки на тип
		private int GetTypeReference(type_node tn, ref byte is_def)
		{
			//если это откомпилир. тип
            if (tn.semantic_node_type == semantic_node_type.compiled_type_node)
			{
				is_def = 0;
				compiled_type_node ctn = (compiled_type_node)tn;
                ImportedEntity ie = null; 
                if (ext_members.TryGetValue(ctn,out ie))
                {
                    return ie.index*ie.GetSize();
                }
				//заполняем структуру в списке импортируемых сущностей
                ie = new ImportedEntity();
				ie.index = imp_entitles.Count;
				ie.flag = ImportKind.DotNet;
				ie.num_unit = GetAssemblyToken(ctn.compiled_type.Assembly);
				//ie.offset = (int)ctn.compiled_type.MetadataToken;//токен для типа (уникален в сборке)
                ie.offset = GetTokenForNetEntity(ctn.compiled_type);
				int offset = imp_entitles.Count*ie.GetSize();
				imp_entitles.Add(ie);//добавляем структуру
				ext_members[ctn] = ie;
				return offset;//возвращаем смещение относительно начала списка импорт. сущ-тей
			}
			else
			{
                int off = 0;
                if (members.TryGetValue(tn, out off)) //если этот тип описан в этом модуле
				{
					is_def = 1;
					return off;//возвращаем его смещение
				}
                //иначе он описан в другом модуле
				is_def = 0;
                ImportedEntity ie = null;
                if (ext_members.TryGetValue(tn, out ie))
                {
                    return ie.index * ie.GetSize();
                }
				common_type_node ctn = (common_type_node)tn;
				ie = new ImportedEntity();
				ie.flag = ImportKind.Common;
				ie.num_unit = GetUnitToken(ctn.comprehensive_namespace);//получаем модуль
				ie.offset = GetExternalOffset(ctn);//получаем смещение в другом модуле
				int offset = imp_entitles.Count*ie.GetSize();
                ie.index = imp_entitles.Count;
                imp_entitles.Add(ie);
				ext_members[ctn] = ie;
				return offset;
			}
		}

        //ssyy
        private int GetTemplateTypeReference(template_class tc, ref byte is_def)
        {
            int off = 0;
            if (members.TryGetValue(tc, out off)) //если этот тип описан в этом модуле
            {
                is_def = 1;
                return off;//возвращаем его смещение
            }
            //иначе он описан в другом модуле
            is_def = 0;
            ImportedEntity ie = null;
            if (ext_members.TryGetValue(tc, out ie))
            {
                return ie.index * ie.GetSize();
            }
            ie = new ImportedEntity();
            ie.flag = ImportKind.Common;
            ie.num_unit = GetUnitToken(tc.cnn);//получаем модуль
            ie.offset = GetExternalOffset(tc);//получаем смещение в другом модуле
            int offset = imp_entitles.Count * ie.GetSize();
            ie.index = imp_entitles.Count;
            imp_entitles.Add(ie);
            ext_members[tc] = ie;
            return offset;
        }
        //\ssyy
		
        //получения ссылки на откомпилированный тип
		private int GetCompiledTypeReference(compiled_type_node ctn)
		{
            ImportedEntity ie = null;
            if (ext_members.TryGetValue(ctn, out ie))
            {
                return ie.index * ie.GetSize();
            }
			ie = new ImportedEntity();
			ie.index = imp_entitles.Count;
			ie.flag = ImportKind.DotNet;
			ie.num_unit = GetAssemblyToken(ctn.compiled_type.Assembly);
			//ie.offset = (int)ctn.compiled_type.MetadataToken;
            ie.offset = GetTokenForNetEntity(ctn.compiled_type);
			int offset = imp_entitles.Count*ie.GetSize();
			imp_entitles.Add(ie);
			ext_members[ctn] = ie;
			return offset;
		}
		
        //получение ссылки на тип, описанный в другом модуле
		private int GetExtTypeReference(common_type_node ctn)
		{
            ImportedEntity ie = null;
            if (ext_members.TryGetValue(ctn, out ie))
            {
                return ie.index * ie.GetSize();
            }
			ie = new ImportedEntity();
			ie.flag = ImportKind.Common;
			ie.num_unit = GetUnitToken(ctn.comprehensive_namespace);
			ie.offset = GetExternalOffset(ctn);
			int offset = imp_entitles.Count*ie.GetSize();
            ie.index = imp_entitles.Count;
            imp_entitles.Add(ie);
			ext_members[ctn] = ie;
			return offset;
		}
		
        //запись ссылки на тип
        //флаг|смещение
		private void WriteTypeReference(type_node type)
		{
            if (type == null)
            {
                bw.Write((System.Byte)255);
                return;
            }
			//если это массив
            if (type.semantic_node_type == semantic_node_type.simple_array)
            {
                WriteArrayType((simple_array)type);
                return;
            }
            //если это указатель
            //Наверно также надо сохранять Pointer,
            //  пока он востанавливается из таблицы nethelper.special_types
            if (type.semantic_node_type == semantic_node_type.ref_type_node)
            {
                WritePointerType((ref_type_node)type);
                return;
            }
            if (type.semantic_node_type == semantic_node_type.short_string)
            {
            	WriteShortStringType((short_string_type_node)type);
            	return;
            }
            internal_interface ii=type.get_internal_interface(internal_interface_kind.unsized_array_interface);
            //(ssyy) 18.05.2008 Убрал проверку на compiled_type_node
            if (ii != null) //&& type.element_type is compiled_type_node)
            {
                array_internal_interface aii=(array_internal_interface)ii;
                WriteUnsizedArrayType(type,aii);
                return;
            }
            
            //Пишем параметр generic-типа
            if (type.is_generic_parameter)
            {
                WriteGenericParameter(type as common_type_node);
                return;
            }

            //Пишем инстанцию generic-типа
            generic_instance_type_node gitn = type as generic_instance_type_node;
            if (gitn != null)
            {
                WriteGenericTypeInstance(gitn);
                return;
            }

            //Пишем инстанцию шаблонного класса
            common_type_node c_t_n = type as common_type_node;
            if (c_t_n != null && c_t_n.original_template != null)
            {
                WriteTemplateInstance(c_t_n);
                return;
            }
            
            if (type is lambda_any_type_node)
            {
                bw.Write((byte)TypeKind.LambdaAnyType); 
                return;
            }

            byte is_def = 0;
			int offset = GetTypeReference(type, ref is_def);
			bw.Write(is_def); //пишем флаг импортируемый ли это тип или нет
			bw.Write(offset); // сохраняем его смещение (это либо смещение в самом модуле, либо в списке импорт. сущностей)
		}

        private void WriteTypeReferenceWithDelay(common_type_node type)
        {
            if (!type_references.ContainsKey((int)bw.BaseStream.Position))
                type_references.Add((int)bw.BaseStream.Position, type);
            
            bw.Write((byte)0);
            bw.Write(0);
        }

        private void WriteTypeList(List<type_node> types)
        {
            bw.Write(types.Count);
            foreach (type_node t_n in types)
            {
                WriteTypeReference(t_n);
            }
        }

        //(ssyy) Сохранение инстанции шаблонного класса
        private void WriteTemplateInstance(common_type_node cnode)
        {
            bw.Write((byte)TypeKind.TemplateInstance);

            //Записать ссылку на шаблонный класс
            WriteTemplateClassReference(cnode.original_template);
            WriteTypeList(cnode.original_template.GetParamsList(cnode));
        }

        //(ssyy) Сохранение инстанции generic-класса
        private void WriteGenericTypeInstance(generic_instance_type_node gitn)
        {
            bw.Write((byte)TypeKind.GenericInstance);

            //Пишем ссылку на описание generic-типа
            WriteTypeReference(gitn.original_generic);
            WriteTypeList(gitn.instance_params);
        }

        private void WriteGenericNamespaceFunctionReference(generic_namespace_function_instance_node func)
        {
            bw.Write((byte)PCUConsts.common_namespace_generic);
            WriteFunctionReference(func.original_function as common_namespace_function_node);
            WriteTypeList(func.instance_params);
        }

        private void WriteGenericMethodReference(function_node meth)
        {
            common_method_node cnode = meth.original_function as common_method_node;
            if (cnode != null)
            {
                bw.Write((byte)PCUConsts.common_method_generic);
                WriteMethodReference(cnode);
            }
            else
            {
                compiled_function_node comp = meth.original_function as compiled_function_node;
                bw.Write((byte)PCUConsts.compiled_method_generic);
                WriteCompiledMethod(comp);
            }
            WriteTypeList(meth.get_generic_params_list());
        }

        //(ssyy) Сохранение параметров generic-типов
        private void WriteGenericParameter(common_type_node type)
        {
            if (type.generic_type_container != null)
            {
                bw.Write((byte)TypeKind.GenericParameterOfType);
                //Пишем ссылку на generic-тип, содержащий данный параметр
                if (type.generic_type_container is common_type_node)
                    WriteTypeReferenceWithDelay(type.generic_type_container as common_type_node);
                else
                    WriteTypeReference(type.generic_type_container as type_node);
            }
            else
            {
                common_method_node cnode = type.generic_function_container as common_method_node;
                if (cnode != null)
                {
                    bw.Write((byte)TypeKind.GenericParameterOfMethod);
                    WriteMethodReference(cnode);
                }
                else
                {
                    common_namespace_function_node cnfn = type.generic_function_container as common_namespace_function_node;
                    bw.Write((byte)TypeKind.GenericParameterOfFunction);
                    WriteFunctionReferenceWithDelay(cnfn);
                }
            }
            bw.Write(type.generic_param_index);
        }

        //сохранение типа-массив
        private void WriteArrayType(simple_array type)
        {
            int offset = (int)bw.BaseStream.Position;
            bw.Write((byte)TypeKind.Array);
            WriteTypeReference(type.element_type);
            bw.Write(type.length);
            members[type] = offset;//это для нашего модуля
            //ext_members[type] = offset;//это для других модулей
            //VisitPropertyDefinition(type.default_property_node, offset);
        }

        //сохранение типа-указатель
        private void WritePointerType(ref_type_node type)
        {
            bw.Write((byte)TypeKind.Pointer); 
            WriteTypeReference(type.pointed_type);

        }
		
        private void WriteShortStringType(short_string_type_node type)
        {
        	bw.Write((byte)TypeKind.ShortString);
        	bw.Write(type.Length);
        }
        
        private void WriteBoundedArray(bounded_array_interface bai)
        {
            bw.Write((byte)TypeKind.BoundedArray);
            VisitExpression(bai.ordinal_type_interface.lower_value);
            VisitExpression(bai.ordinal_type_interface.upper_value);
            WriteTypeReference(bai.element_type);
        }

        private void WriteUnsizedArrayType(type_node type,array_internal_interface aii)
        {
            bw.Write((byte)TypeKind.UnsizedArray);
            WriteDebugInfo((type as SemanticTree.ILocated).Location);
            WriteTypeReference(aii.element_type);
            bw.Write(aii.rank);
        }

        //получение ссылки на функцию
		private int GetFunctionReference(common_namespace_function_node fn, ref byte is_def)
		{
            int off = 0;
            if (members.TryGetValue(fn, out off)) //если этот тип описан в этом модуле
            {
                is_def = 1;
                return off;//возвращаем его смещение
            }
			is_def = 0;
            ImportedEntity ie = null;
            if (ext_members.TryGetValue(fn, out ie))
            {
                return ie.index * ie.GetSize();
            }
			ie = new ImportedEntity();
			ie.flag = ImportKind.Common;
			ie.num_unit = GetUnitToken(fn.namespace_node);
			ie.offset = GetExternalOffset(fn);
			int offset = imp_entitles.Count*ie.GetSize();
            ie.index = imp_entitles.Count;
            imp_entitles.Add(ie);
			ext_members[fn] = ie;
			return offset;
		}
		
        //получение ссылки на откомпилированный метод
		private int GetCompiledMethod(compiled_function_node cfn)
		{
            ImportedEntity ie = null;
            if (ext_members.TryGetValue(cfn, out ie))
            {
                return ie.index * ie.GetSize();
            }
			ie = new ImportedEntity();
			ie.flag = ImportKind.DotNet;
			ie.num_unit = GetCompiledTypeReference(cfn.cont_type);
			//ie.offset = (int)cfn.method_info.MetadataToken;
            ie.offset = GetTokenForNetEntity(cfn.method_info);
			int offset = imp_entitles.Count*ie.GetSize();
            ie.index = imp_entitles.Count;
			imp_entitles.Add(ie);
			ext_members[cfn] = ie;
			return offset;
		}

        //получение ссылки на откомпилированный конструктор
		private int GetCompiledConstructor(compiled_constructor_node ccn)
		{
            ImportedEntity ie = null;
            if (ext_members.TryGetValue(ccn, out ie))
            {
                return ie.index * ie.GetSize();
            }
			ie = new ImportedEntity();
			ie.flag = ImportKind.DotNet;
			ie.num_unit = GetCompiledTypeReference(ccn.compiled_type);
			//ie.offset = (int)ccn.constructor_info.MetadataToken;
            ie.offset = GetTokenForNetEntity(ccn.constructor_info);
			int offset = imp_entitles.Count*ie.GetSize();
            ie.index = imp_entitles.Count;
            imp_entitles.Add(ie);
			ext_members[ccn] = ie;
			return offset;
		}
		
        //получение ссылки на откомпилированное свойство
        private int GetCompiledProperty(compiled_property_node cpn)
        {
            ImportedEntity ie = null;
            if (ext_members.TryGetValue(cpn, out ie))
            {
                return ie.index * ie.GetSize();
            }
            ie = new ImportedEntity();
            ie.flag = ImportKind.DotNet;
            ie.num_unit = GetCompiledTypeReference(cpn.compiled_comprehensive_type);
            //ie.offset = (int)ccn.constructor_info.MetadataToken;
            ie.offset = GetTokenForNetEntity(cpn.property_info);
            int offset = imp_entitles.Count * ie.GetSize();
            ie.index = imp_entitles.Count;
            imp_entitles.Add(ie);
            ext_members[cpn] = ie;
            return offset;
        }

        private void WriteFunctionReferenceWithDelay(common_namespace_function_node fn)
        {
            function_references.Add((int)bw.BaseStream.Position, fn);
            bw.Write((byte)0);
            bw.Write(0);
        }

        //сохранение ссылки на функцию
        private void WriteFunctionReference(common_namespace_function_node fn)
		{
			generic_namespace_function_instance_node gi = fn as generic_namespace_function_instance_node;
            if (gi != null)
            {
                WriteGenericNamespaceFunctionReference(gi);
                return;
            }
			byte is_def = 0;
            int offset=0;
            if (fn.SpecialFunctionKind == SemanticTree.SpecialFunctionKind.None)
                offset = GetFunctionReference(fn, ref is_def);
            else
                is_def = 2;
			bw.Write(is_def);
            if (fn.SpecialFunctionKind == SemanticTree.SpecialFunctionKind.None)            
			    bw.Write(offset);
            else
                bw.Write((byte)fn.SpecialFunctionKind);
		}

        //сохранение ссылки на откомпилированный метод
		private void WriteCompiledMethod(compiled_function_node cfn)
		{
            if (cfn.is_generic_function_instance)
            {
                bw.Write(PCUConsts.method_instance_as_compiled_function_node);
                WriteGenericMethodReference(cfn);
                return;
            }
			bw.Write(GetCompiledMethod(cfn));
		}
		
		private void WriteCompiledConstructor(compiled_constructor_node ccn)
		{
			bw.Write(GetCompiledConstructor(ccn));
		}
		
        private void WriteCompiledProperty(compiled_property_node cpn)
        {
            bw.Write(GetCompiledProperty(cpn));
        }

		private int GetVariableReference(namespace_variable nv, ref byte is_def)
		{
            int off = 0;
            if (members.TryGetValue(nv, out off)) //если этот тип описан в этом модуле
            {
                is_def = 1;
                return off;//возвращаем его смещение
            }
            if (!used_units.ContainsKey(nv.namespace_node.cont_unit as common_unit_node))
            {
                is_def = 1;
                return -1;
            }
			is_def = 0;
            ImportedEntity ie = null;
            if (ext_members.TryGetValue(nv, out ie))
            {
                return ie.index * ie.GetSize();
            }
			ie = new ImportedEntity();
			ie.flag = ImportKind.Common;
			ie.num_unit = GetUnitToken(nv.namespace_node);
			ie.offset = GetExternalOffset(nv);
			int offset = imp_entitles.Count*ie.GetSize();
            ie.index = imp_entitles.Count;
            imp_entitles.Add(ie);
			ext_members[nv] = ie;
			return offset;
		}
		
		private int GetConstantReference(namespace_constant_definition nv, ref byte is_def)
		{
            int off = 0;
            if (members.TryGetValue(nv, out off)) //если этот тип описан в этом модуле
            {
                is_def = 1;
                return off;//возвращаем его смещение
            }
            if (!used_units.ContainsKey(nv.comprehensive_namespace.cont_unit as common_unit_node))
            {
                is_def = 1;
                return -1;
            }
			is_def = 0;
            ImportedEntity ie = null;
            if (ext_members.TryGetValue(nv, out ie))
            {
                return ie.index * ie.GetSize();
            }
			ie = new ImportedEntity();
			ie.flag = ImportKind.Common;
			ie.num_unit = GetUnitToken(nv.comprehensive_namespace);
			ie.offset = GetExternalOffset(nv);
			int offset = imp_entitles.Count*ie.GetSize();
            ie.index = imp_entitles.Count;
            imp_entitles.Add(ie);
			ext_members[nv] = ie;
			return offset;
		}
		
		private int GetCompiledVariable(compiled_variable_definition cvd)
		{
            ImportedEntity ie = null;
            if (ext_members.TryGetValue(cvd, out ie))
            {
                return ie.index * ie.GetSize();
            }
			ie = new ImportedEntity();
			ie.flag = ImportKind.DotNet;
			ie.num_unit = GetCompiledTypeReference(cvd.cont_type);
			//ie.offset = (int)cvd.compiled_field.MetadataToken;
            ie.offset = GetTokenForNetEntity(cvd.compiled_field);
            ie.index = imp_entitles.Count;
			int offset = imp_entitles.Count*ie.GetSize();
			imp_entitles.Add(ie);
			ext_members[cvd] = ie;
			return offset;
		}

        private Hashtable var_positions = new Hashtable();
        private Hashtable const_positions = new Hashtable();
        
        private void SaveVariableReferencePosition(namespace_variable nv)
        {
            List<int> offs = (List<int>)var_positions[nv];
            if (offs == null)
            {
                offs = new List<int>();
                var_positions[nv] = offs;
            }
            offs.Add((int)bw.BaseStream.Position);
        }
		
        private void SaveConstantReferencePosition(namespace_constant_definition nv)
        {
            List<int> offs = (List<int>)const_positions[nv];
            if (offs == null)
            {
                offs = new List<int>();
                const_positions[nv] = offs;
            }
            offs.Add((int)bw.BaseStream.Position);
        }
        
        private void WriteVariablePositions()
        {
            int tmp = (int)bw.BaseStream.Position;
            foreach (namespace_variable nv in var_positions.Keys)
            {
                List<int> offs = (List<int>)var_positions[nv];
                byte is_def = 0;
                for (int i = 0; i < offs.Count; i++)
                {
                    bw.Seek(offs[i], SeekOrigin.Begin);
                    bw.Write(GetVariableReference(nv,ref is_def));
                }
            }
            bw.BaseStream.Position = tmp;
        }
		
        private void WriteConstantPositions()
        {
            int tmp = (int)bw.BaseStream.Position;
            foreach (namespace_constant_definition nv in const_positions.Keys)
            {
                List<int> offs = (List<int>)const_positions[nv];
                byte is_def = 0;
                for (int i = 0; i < offs.Count; i++)
                {
                    bw.Seek(offs[i], SeekOrigin.Begin);
                    bw.Write(GetConstantReference(nv,ref is_def));
                }
            }
            bw.BaseStream.Position = tmp;
        }
        
		private void WriteVariableReference(namespace_variable nv)
		{
			byte is_def = 0;
			int offset = GetVariableReference(nv, ref is_def);
			bw.Write(is_def);
            if (offset == -1) SaveVariableReferencePosition(nv);
			bw.Write(offset);
		}
		
		private void WriteConstantReference(namespace_constant_definition nv)
		{
			byte is_def = 0;
			int offset = GetConstantReference(nv, ref is_def);
			bw.Write(is_def);
            if (offset == -1) SaveConstantReferencePosition(nv);
			bw.Write(offset);
		}
		
		private void WriteCompiledVariable(compiled_variable_definition cvd)
		{
			bw.Write(GetCompiledVariable(cvd));
		}

        private int GetEventReference(event_node ev, ref byte is_def)
        {
            if (ev.semantic_node_type == semantic_node_type.compiled_event)
            {
                is_def = 2;
                compiled_event cev = (compiled_event)ev;
                ImportedEntity ie = null;
                if (ext_members.TryGetValue(cev, out ie))
                {
                    return ie.index * ie.GetSize();
                }
                //заполняем структуру в списке импортируемых сущностей
                ie = new ImportedEntity();
                ie.index = imp_entitles.Count;
                ie.flag = ImportKind.DotNet;
                ie.num_unit = GetAssemblyToken(cev.event_info.DeclaringType.Assembly);
                //ie.offset = (int)ctn.compiled_type.MetadataToken;//токен для типа (уникален в сборке)
                ie.offset = GetTokenForNetEntity(cev.event_info.DeclaringType);
                int offset = imp_entitles.Count * ie.GetSize();
                imp_entitles.Add(ie);//добавляем структуру
                ext_members[cev] = ie;
                return offset;//возвращаем смещение относительно начала списка импорт. сущ-тей
            }
            else if (ev.semantic_node_type == semantic_node_type.common_namespace_event)
            {
                int off = 0;
                if (members.TryGetValue(ev, out off)) //если этот тип описан в этом модуле
                {
                    is_def = 4;
                    return off;//возвращаем его смещение
                }
                is_def = 3;
                ImportedEntity ie = null;
                if (ext_members.TryGetValue(ev, out ie))
                {
                    return ie.index * ie.GetSize();
                }
                ie = new ImportedEntity();
                ie.flag = ImportKind.Common;
                ie.num_unit = GetUnitReference((ev as common_namespace_event).namespace_node);
                ie.offset = GetExternalOffset(ev);
                int offset = imp_entitles.Count * ie.GetSize();
                ie.index = imp_entitles.Count;
                imp_entitles.Add(ie);
                ext_members[ev] = ie;
                return offset;
            }
            else
            {
                int off = 0;
                if (members.TryGetValue(ev, out off)) //если этот тип описан в этом модуле
                {
                    is_def = 1;
                    return off;//возвращаем его смещение
                }
                is_def = 0;
                ImportedEntity ie = null;
                if (ext_members.TryGetValue(ev, out ie))
                {
                    return ie.index * ie.GetSize();
                }
                ie = new ImportedEntity();
                ie.flag = ImportKind.Common;
                ie.num_unit = GetExtTypeReference((ev as common_event).cont_type);
                ie.offset = GetExternalOffset(ev);
                int offset = imp_entitles.Count * ie.GetSize();
                ie.index = imp_entitles.Count;
                imp_entitles.Add(ie);
                ext_members[ev] = ie;
                return offset;
            }
        }

		private int GetFieldReference(class_field field, ref byte is_def)
		{
            int off = 0;
            if (members.TryGetValue(field, out off)) //если этот тип описан в этом модуле
            {
                is_def = 1;
                return off;//возвращаем его смещение
            }
			is_def = 0;
            ImportedEntity ie = null;
            if (ext_members.TryGetValue(field, out ie))
            {
                return ie.index * ie.GetSize();
            }
			ie = new ImportedEntity();
			ie.flag = ImportKind.Common;
			ie.num_unit = GetExtTypeReference(field.cont_type);
			ie.offset = GetExternalOffset(field);
			int offset = imp_entitles.Count*ie.GetSize();
            ie.index = imp_entitles.Count;
            imp_entitles.Add(ie);
			ext_members[field] = ie;
			return offset;
		}
		
		private void WriteFieldReference(class_field field)
		{
            generic_instance_type_node gitn = field.cont_type as generic_instance_type_node;
            if (gitn != null)
            {
                bw.Write(PCUConsts.generic_field);
                WriteGenericTypeInstance(gitn);
                if (gitn is compiled_generic_instance_type_node)
                {
                    WriteCompiledVariable(
                        gitn.get_member_definition(field) as compiled_variable_definition
                        );
                }
                else
                {
                    WriteFieldReference(gitn.get_member_definition(field) as class_field);
                }
                return;
            }
            common_type_node ctnode = field.comperehensive_type as common_type_node;
            if (ctnode != null && ctnode.original_template != null)
            {
                bw.Write(PCUConsts.template_field); //т.е. пишем на место is_def
                WriteTemplateInstance(ctnode);
                bw.Write(ctnode.fields.IndexOf(field));
                return;
            }

            byte is_def = 0;
			int offset = GetFieldReference(field, ref is_def);
			bw.Write(is_def);
			bw.Write(offset);
		}
		
		private int GetMethodReference(common_method_node meth, ref byte is_def)
		{
            int off = 0;
            if (members.TryGetValue(meth, out off)) //если этот тип описан в этом модуле
            {
                is_def = 1;
                return off;//возвращаем его смещение
            }
			is_def = 0;
            ImportedEntity ie = null;
            if (ext_members.TryGetValue(meth, out ie))
            {
                return ie.index * ie.GetSize();
            }
			ie = new ImportedEntity();
			ie.flag = ImportKind.Common;
			ie.num_unit = GetExtTypeReference(meth.cont_type);
			ie.offset = GetExternalOffset(meth);
			int offset = imp_entitles.Count*ie.GetSize();
            ie.index = imp_entitles.Count;
            imp_entitles.Add(ie);
			ext_members[meth] = ie;
			return offset;
		}

        private void WriteEventReference(event_node ev)
        {
            byte is_def = 0;
            int offset = GetEventReference(ev, ref is_def);
            bw.Write(is_def);
            bw.Write(offset);
        }

		private void WriteMethodReference(common_method_node meth)
		{
            if (meth.comperehensive_type.is_generic_parameter)
            {
                bw.Write(PCUConsts.generic_param_ctor);
                WriteGenericParameter(meth.comperehensive_type as common_type_node);
                return;
            }

            generic_method_instance_node gmin = meth as generic_method_instance_node;
            if (gmin != null)
            {
                WriteGenericMethodReference(gmin);
                return;
            }

            generic_instance_type_node gitn = meth.comperehensive_type as generic_instance_type_node;
            if (gitn != null)
            {
                if (gitn is common_generic_instance_type_node)
                {
                    bw.Write(PCUConsts.generic_meth);
                    WriteGenericTypeInstance(gitn);
                    WriteMethodReference(gitn.get_member_definition(meth) as common_method_node);
                    return;
                }
                if (meth.is_constructor)
                {
                    bw.Write(PCUConsts.generic_ctor);
                    WriteGenericTypeInstance(gitn);
                    WriteCompiledConstructor(
                        gitn.get_member_definition(meth) as compiled_constructor_node
                        );
                    return;
                }
                bw.Write(PCUConsts.generic_meth);
                WriteGenericTypeInstance(gitn);
                WriteCompiledMethod(
                    gitn.get_member_definition(meth) as compiled_function_node
                    );
                return;
            }

            common_type_node ctnode = meth.comperehensive_type as common_type_node;
            if (ctnode != null && ctnode.original_template != null)
            {
                bw.Write(PCUConsts.template_meth); //т.е. пишем на место is_def
                WriteTemplateInstance(ctnode);
                bw.Write(ctnode.methods.IndexOf(meth));
                return;
            }

            //generic_namespace_function_instance_node gnfin = meth as generic_namespace_function_instance_node;
            //if (gnfin != null)
            //{
            //    WriteGenericNamespaceFunctionReference(gnfin);
            //    return;
            //}

            byte is_def = 0;
            int offset = GetMethodReference(meth, ref is_def);
			bw.Write(is_def);
			bw.Write(offset);
		}

        private void WriteTemplateClassReference(template_class tc)
        {
            byte is_def = 0;
            int t_offset = GetTemplateTypeReference(tc, ref is_def);
            bw.Write(is_def); //пишем флаг импортируемый ли это тип или нет
            bw.Write(t_offset); // сохраняем его смещение (это либо смещение в самом модуле, либо в списке импорт. сущностей)
        }

        private int GetPropertyReference(common_property_node prop, ref byte is_def)
        {
            int off = 0;
            if (members.TryGetValue(prop, out off)) //если этот тип описан в этом модуле
            {
                is_def = 1;
                return off;//возвращаем его смещение
            }
            is_def = 0;
            ImportedEntity ie = null;
            if (ext_members.TryGetValue(prop, out ie))
            {
                return ie.index * ie.GetSize();
            }
            ie = new ImportedEntity();
            ie.flag = ImportKind.Common;
            ie.num_unit = GetExtTypeReference(prop.common_comprehensive_type);
            ie.offset = GetExternalOffset(prop);
            int offset = imp_entitles.Count * ie.GetSize();
            ie.index = imp_entitles.Count;
            imp_entitles.Add(ie);
            ext_members[prop] = ie;
            return offset;
        }
		
        private void WriteMethodReference(function_node fn)
        {
        	if (fn is common_method_node)
        	{
        		bw.Write((byte)0);
        		WriteMethodReference(fn as common_method_node);
        	}
        	else if (fn is compiled_constructor_node)
        	{
        		bw.Write((byte)1);
        		bw.Write(GetCompiledConstructor(fn as compiled_constructor_node));
        	}
        	else if (fn is compiled_function_node)
        	{
        		bw.Write((byte)2);
        		bw.Write(GetCompiledMethod(fn as compiled_function_node));
        	}
        }
        
        private void WritePropertyReference(property_node pn)
        {
        	if (pn is common_property_node)
        	{
        		bw.Write((byte)0);
        		WritePropertyReference(pn as common_property_node);
        	}
        	else if (pn is compiled_property_node)
        	{
        		bw.Write((byte)1);
        		bw.Write(GetCompiledProperty(pn as compiled_property_node));
        	}
        }
        
        private void WriteFieldReference(var_definition_node vdn)
        {
        	if (vdn is class_field)
        	{
        		bw.Write((byte)0);
        		WriteFieldReference(vdn as class_field);
        	}
        	else if (vdn is compiled_variable_definition)
        	{
        		bw.Write((byte)1);
        		bw.Write(GetCompiledVariable(vdn as compiled_variable_definition));
        	}
        }
        
        private void WritePropertyReference(common_property_node prop)
        {
            //ssyy
            generic_instance_type_node gitn = prop.common_comprehensive_type as generic_instance_type_node;
            if (gitn != null)
            {
                bw.Write(PCUConsts.generic_prop);
                WriteGenericTypeInstance(gitn);
                if (gitn is compiled_generic_instance_type_node)
                {
                    WriteCompiledProperty(
                        gitn.get_member_definition(prop) as compiled_property_node
                        );
                }
                else
                {
                    WritePropertyReference(gitn.get_member_definition(prop) as common_property_node);
                }
                return;
            }
            common_type_node ctnode = prop.common_comprehensive_type as common_type_node;
            if (ctnode != null && ctnode.original_template != null)
            {
                bw.Write(PCUConsts.template_prop); //т.е. пишем на место is_def
                WriteTemplateInstance(ctnode);
                bw.Write(ctnode.properties.IndexOf(prop));
                return;
            }
            //\ssyy
            byte is_def = 0;
            int offset = GetPropertyReference(prop, ref is_def);
            bw.Write(is_def);
            bw.Write(offset);
        }

        //сохранение пространства имен
		private void WriteUnit()
		{
			SavePosition(cur_cnn);
			bw.Write(cur_cnn.namespace_name);
            WriteDebugInfo(cur_cnn.Location);
		}

        //сохранение констант
        private void VisitConstantDefinitions()
        {
            bw.Write(cur_cnn.constants.Count);
            for (int i = 0; i < cur_cnn.constants.Count; i++)
                VisitConstantDefinition(cur_cnn.constants[i]);
        }

        //сохранение указателей на тип
        private void VisitRefTypeDefinitions()
        {
            bw.Write(cur_cnn.ref_types.Count);
            for (int i = 0; i < cur_cnn.ref_types.Count; i++)
                VisitRefTypeDefinition(cur_cnn.ref_types[i]);
        }

        private void VisitRefTypeDefinition(ref_type_node node)
        {
            if (is_interface == true)
                SavePositionAndConstPool(node);
            else
                SavePositionAndImplementationPool(node);
            bw.Write((byte)node.semantic_node_type);
            bw.Write(is_interface);
            if (is_interface == true)
                bw.Write(GetNameIndex(node));
            else
                bw.Write(node.name);
            //bw.Write(GetUnitReference(node.comprehensive_namespace));
            WriteTypeReference(node.pointed_type);
            WriteDebugInfo(node.loc);
        }


        //сохранение константы
        private void VisitConstantDefinition(namespace_constant_definition cnst)
        {
            if (is_interface == true) 
                SavePositionAndConstPool(cnst);
            else 
                SavePositionAndImplementationPool(cnst);
            bw.Write((byte)cnst.semantic_node_type);
            bw.Write(is_interface);
            if (is_interface == true)
                bw.Write(GetNameIndex(cnst));
            else
                bw.Write(cnst.name);
            bw.Write(GetUnitReference(cnst.comprehensive_namespace));
            WriteTypeReference(cnst.type);
            SaveExpressionAndOffset(cnst.const_value);
            bw.Write(0);
            //VisitExpression(cnst.const_value);
            WriteDebugInfo(cnst.loc);
        }

        //сохранение переменных модуля
		private void VisitVariableDefinitions()
		{
			bw.Write(cur_cnn.variables.Count);
			for (int i=0; i<cur_cnn.variables.Count; i++)
				VisitVariableDefinition(cur_cnn.variables[i]);
		}

        private void VisitEventDefinitions()
        {
            bw.Write(cur_cnn.events.Count);
            for (int i = 0; i < cur_cnn.events.Count; i++)
                VisitNamespaceEventDefinition(cur_cnn.events[i]);
        }

        private void VisitNamespaceEventDefinition(common_namespace_event _event)
        {
            if (is_interface)
                SavePositionAndConstPool(_event);
            else
                SavePositionAndImplementationPool(_event);
            bw.Write((byte)_event.semantic_node_type);
            bw.Write(is_interface);
            if (is_interface == true)
                bw.Write(GetNameIndex(_event));
            else
                bw.Write(_event.name);
            WriteTypeReference(_event.delegate_type);
            if (CanWriteObject(_event.add_method))
                bw.Write(GetMemberOffset(_event.add_method));
            if (CanWriteObject(_event.remove_method))
                bw.Write(GetMemberOffset(_event.remove_method));
            if (CanWriteObject(_event.raise_method))
                bw.Write(GetMemberOffset(_event.raise_method));
            bw.Write(GetMemberOffset(_event.field));
            bw.Write(GetUnitReference(_event.namespace_node));
            WriteDebugInfo(_event.loc);
        }

        private void VisitVariableDefinition(namespace_variable var)
		{
			if (is_interface) 
                SavePositionAndConstPool(var);
			else 
                SavePositionAndImplementationPool(var);
			bw.Write((byte)var.semantic_node_type);
			bw.Write(entity_index++);
			bw.Write(is_interface);
			if (is_interface == true)
		 	    bw.Write(GetNameIndex(var));
			else
			    bw.Write(var.name);
			WriteTypeReference(var.type);
			bw.Write(GetUnitReference(var.namespace_node));
			WriteDebugInfo(var.loc);
            if (CanWriteObject(var.inital_value))
            {
            	SaveExpressionAndOffset(var.inital_value);
            	bw.Write(0);
            	//VisitExpression((expression_node)var.inital_value);
            }
        }
		
        //сохранение типов
		private void VisitTypeDefinitions()
		{
			bw.Write(cur_cnn.non_template_types.Count);
			for (int i=0; i<cur_cnn.non_template_types.Count; i++)
                if (!(cur_cnn.non_template_types[i].type_special_kind == SemanticTree.type_special_kind.array_wrapper && 
                    cur_cnn.non_template_types[i].name.StartsWith("$") && cur_cnn.non_template_types[i].element_type is common_type_node && cur_cnn.non_template_types[i].element_type.is_class))
                    VisitTypeDefinition(cur_cnn.non_template_types[i]);
            for (int i = 0; i < cur_cnn.non_template_types.Count; i++)
                if (cur_cnn.non_template_types[i].type_special_kind == SemanticTree.type_special_kind.array_wrapper && 
                    cur_cnn.non_template_types[i].name.StartsWith("$") && cur_cnn.non_template_types[i].element_type is common_type_node && cur_cnn.non_template_types[i].element_type.is_class)
                    VisitTypeDefinition(cur_cnn.non_template_types[i]);
            bw.Write(cur_cnn.runtime_types.Count);
			for (int i=0; i<cur_cnn.runtime_types.Count; i++)
				VisitCompiledTypeDefinition(cur_cnn.runtime_types[i]);
		}

        //сохранение меток
        private void VisitLabelDeclarations(List<label_node> labels)
        {
            bw.Write(labels.Count);
            foreach (label_node ln in labels)
            {
                VisitLabelDeclaration(ln);
            }
        }

        //сохранение шаблонных классов
        private void VisitTemplateClasses()
        {
            bw.Write(cur_cnn.templates.Count);
            for (int i = 0; i < cur_cnn.templates.Count; i++)
                VisitTemplateClassDefinition(cur_cnn.templates[i]);
        }

        //сохранение синонимов типов
        private void VisitTypeSynonyms()
        {
            bw.Write(cur_cnn.type_synonyms.Count);
            for (int i = 0; i < cur_cnn.type_synonyms.Count; i++)
                VisitTypeSynonym(cur_cnn.type_synonyms[i]);
        }
		
        private void SaveAttributes(attributes_list attrs, List<int> offs)
        {
        	int tmp = (int)bw.BaseStream.Position;
        	for (int i=0; i<offs.Count; i++)
        	{
        		bw.Seek(offs[i], SeekOrigin.Begin);
        		bw.Write(tmp);
        		bw.Seek(tmp, SeekOrigin.Begin);
        	}
        	bw.Write(attrs.Count);
        	for (int i=0; i<attrs.Count; i++)
        		SaveAttribute(attrs[i]);
        }
        
        private void SaveAttribute(attribute_node attr)
        {
        	WriteTypeReference(attr.attribute_type);
        	WriteMethodReference(attr.attribute_constr);
        	bw.Write((byte)attr.qualifier);
        	bw.Write(attr.args.Count);
        	for (int i=0; i<attr.args.Count; i++)
        		VisitExpression(attr.args[i]);
        	bw.Write(attr.prop_names.Count);
        	for (int i=0; i<attr.prop_names.Count; i++)
        		WritePropertyReference(attr.prop_names[i]);
        	bw.Write(attr.field_names.Count);
        	for (int i=0; i<attr.field_names.Count; i++)
        		WriteFieldReference(attr.field_names[i]);
        	bw.Write(attr.prop_initializers.Count);
        	for (int i=0; i<attr.prop_initializers.Count; i++)
        		VisitExpression(attr.prop_initializers[i]);
        	bw.Write(attr.field_initializers.Count);
        	for (int i=0; i<attr.field_initializers.Count; i++)
        		VisitExpression(attr.field_initializers[i]);
        	WriteDebugInfo(attr.location);
        }
        
        private access_level convert_field_access_level(SemanticTree.field_access_level fal)
        {
            switch (fal)
            {
                case SemanticTree.field_access_level.fal_private:
                    return access_level.al_private;
                case SemanticTree.field_access_level.fal_public:
                    return access_level.al_public;
                case SemanticTree.field_access_level.fal_protected:
                    return access_level.al_protected;
                case SemanticTree.field_access_level.fal_internal:
                    return access_level.al_internal;
                default:
                    return access_level.al_none;
            }
        }

        private void WriteImplementingInterfaces(common_type_node type)
        {
            bw.Write(type.ImplementingInterfaces.Count);
            foreach (type_node interf in type.ImplementingInterfaces)
            {
                WriteTypeReference(interf);
            }
        }

        private void WriteTypeParamsEliminations(List<SemanticTree.ICommonTypeNode> tpars)
        {
            foreach (common_type_node par in tpars)
            {
                //class - value
                if (par.is_class)
                {
                    bw.Write((byte)GenericParamKind.Class);
                }
                else
                {
                    if (par.is_value)
                    {
                        bw.Write((byte)GenericParamKind.Value);
                    }
                    else
                    {
                        bw.Write((byte)GenericParamKind.None);
                    }
                }
                //предок
                WriteTypeReference(par.base_type);
                //интерфейсы
                WriteImplementingInterfaces(par);
                //конструктор по умолчанию
                if (par.methods.Count > 0)
                {
                    bw.Write((byte)1);
                }
                else
                {
                    bw.Write((byte)0);
                }
                //if (CanWriteObject(par.runtime_initialization_marker))
                //{
                //    WriteFieldReference(par.runtime_initialization_marker);
                //}
            }
        }

        private int GetSizeOfReference(type_node tn)
        {
            if (tn == null)
                return sizeof(byte);
            if (tn.is_generic_parameter)
            {
                return sizeof(byte) + GetSizeOfReference(tn.generic_type_container as type_node) + sizeof(int);
            }
            generic_instance_type_node gitn = tn as generic_instance_type_node;
            if (gitn != null)
            {
                int rez = sizeof(byte) + GetSizeOfReference(gitn.original_generic) + sizeof(int);
                foreach (type_node par in gitn.instance_params)
                {
                    rez += GetSizeOfReference(par);
                }
                return rez;
            }
            internal_interface ii = tn.get_internal_interface(internal_interface_kind.unsized_array_interface);
            if (ii != null)
            {
                return sizeof(byte) + 4 * 4 + GetSizeOfReference(tn.element_type) + 4;
            }
            return sizeof(byte) + sizeof(int);
        }

        private void VisitTypeDefinition(common_type_node type)
        {
            int offset = 0;
            if (class_info.ContainsKey(type))
                return;
            if (is_interface == true) offset = SavePositionAndConstPool(type);
            else offset = SavePositionAndImplementationPool(type);
            bw.Write((byte)type.semantic_node_type);
            bw.Write(is_interface);
            bw.Write(type_entity_index++);
            if (is_interface == true)
                bw.Write(GetNameIndex(type));
            else
                bw.Write(type.name);

            if (type.IsInterface)
            {
                bw.Write((byte)1);
            }
            else
            {
                bw.Write((byte)0);
            }

            if (type.is_class)
            {
                bw.Write((byte)1);
            }
            else
            {
                bw.Write((byte)0);
            }

            if (type.IsDelegate)
            {
                bw.Write((byte)1);
            }
            else
            {
                bw.Write((byte)0);
            }

            //Является ли тип описанием дженерика
            if (type.is_generic_type_definition)
            {
                bw.Write((byte)1);
                //Число типов-параметров
                bw.Write(type.generic_params.Count);
                //Имена параметров
                foreach (common_type_node par in type.generic_params)
                {
                    bw.Write(par.name);
                }
            }
            else
            {
                bw.Write((byte)0);
            }
            bw.Write((byte)type.type_special_kind);
            int base_class_off = (int)bw.BaseStream.Position;

            bw.Seek(GetSizeOfReference(type.base_type), SeekOrigin.Current);

            bw.Write(type.internal_is_value);

            //Пишем поддерживаемые интерфейсы
            //eto nepravilno!!! a vdrug bazovye interfejsy eshe ne projdeny.
            //WriteImplementingInterfaces(type);
            int interface_impl_off = (int)bw.BaseStream.Position;
            int seek_off = sizeof(int);
            for (int k = 0; k < type.ImplementingInterfaces.Count; k++)
                seek_off += GetSizeOfReference(type.ImplementingInterfaces[k] as TreeRealization.type_node);
            bw.Seek(seek_off, SeekOrigin.Current);
            bw.Write((byte)type.type_access_level);
            
            bw.Write(type.IsSealed);
            bw.Write(type.IsAbstract);
            bw.Write(type.IsStatic);
            bw.Write(type.IsPartial);

            if (type.type_special_kind == SemanticTree.type_special_kind.diap_type)
            {
                ordinal_type_interface oti = type.get_internal_interface(internal_interface_kind.ordinal_interface) as ordinal_type_interface;
                VisitExpression(oti.lower_value);
                VisitExpression(oti.upper_value);
            }

            if (type.is_generic_type_definition)
            {
                //Ограничители параметров
                WriteTypeParamsEliminations(type.generic_params);
            }
            if (CanWriteObject(type.element_type))
                WriteTypeReference(type.element_type);

            bw.Write(GetUnitReference(type.comprehensive_namespace));
            SaveOffsetForAttribute(type);
            bw.Write(0);//attributes;
            if (type.default_property != null)
                bw.Write((byte)1);
            else
                bw.Write((byte)0);
            int def_prop_off = (int)bw.BaseStream.Position;
            if (type.default_property != null)
                bw.Write(0);//default_property
            WriteDebugInfo(type.loc);
            //заполнение списка имен членов этого класса
            int num = type.const_defs.Count + type.fields.Count + type.properties.Count + type.methods.Count + type.events.Count;
            NameRef[] names = new NameRef[num];
            int pos = (int)bw.BaseStream.Position;
            int int_size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(int));
            int size = int_size;
            int i = 0, j = 0;
            for (i = j; i < type.const_defs.Count + j; i++)
            {
                names[i] = new NameRef(type.const_defs[i - j].name, i);
                name_pool[type.const_defs[i - j]] = names[i];
                size += names[i].Size;
            }
            j = i;
            for (i = j; i < type.fields.Count + j; i++)
            {
                names[i] = new NameRef(type.fields[i - j].name, i, convert_field_access_level(type.fields[i - j].field_access_level), type.fields[i - j].semantic_node_type);
                name_pool[type.fields[i - j]] = names[i];
                names[i].is_static = type.fields[i - j].polymorphic_state == SemanticTree.polymorphic_state.ps_static;
                if (HasPCUAlwaysRestoreAttribute(type.fields[i - j]))
                    names[i].always_restore = true;
                size += names[i].Size;
            }
            j = i;
            for (i = j; i < type.properties.Count + j; i++)
            {
                names[i] = new NameRef(type.properties[i - j].name, i, convert_field_access_level(type.properties[i - j].field_access_level), type.properties[i - j].semantic_node_type);
                name_pool[type.properties[i - j]] = names[i];
                names[i].is_static = type.properties[i - j].polymorphic_state == SemanticTree.polymorphic_state.ps_static;
                if (HasPCUAlwaysRestoreAttribute(type.properties[i - j]))
                    names[i].always_restore = true;
                size += names[i].Size;
            }
            j = i;
            for (i = j; i < type.methods.Count + j; i++)
            {
                names[i] = new NameRef(type.methods[i - j].name, i, convert_field_access_level(type.methods[i - j].field_access_level), type.methods[i - j].semantic_node_type);
                name_pool[type.methods[i - j]] = names[i];
                if (type.methods[i - j].is_overload)
                    names[i].symbol_kind = symbol_kind.sk_overload_function;
                names[i].always_restore = type.methods[i - j].newslot_awaited || type.methods[i - j].polymorphic_state == SemanticTree.polymorphic_state.ps_virtual || type.methods[i - j].polymorphic_state == SemanticTree.polymorphic_state.ps_virtual_abstract || type.methods[i - j].is_constructor;
                names[i].is_static = type.methods[i - j].polymorphic_state == SemanticTree.polymorphic_state.ps_static && !type.methods[i - j].is_constructor;
                if (!names[i].always_restore && HasPCUAlwaysRestoreAttribute(type.methods[i - j]))
                    names[i].always_restore = true;
                size += names[i].Size;
            }
            j = i;
            for (i = j; i < type.events.Count + j; i++)
            {
                names[i] = new NameRef(type.events[i - j].name, i, convert_field_access_level(type.events[i - j].field_access_level), type.events[i - j].semantic_node_type);
                name_pool[type.events[i - j]] = names[i];
                size += names[i].Size;
            }
            bw.BaseStream.Seek(size, SeekOrigin.Current);
            /*VisitConstantInTypeDefinitions(type);
			VisitFieldDefinitions(type);
			VisitMethodDefinitions(type);
			VisitPropertyDefinitions(type);
			int tmp = (int)bw.BaseStream.Position;
            if (type.default_property != null)
            {
                bw.Seek(def_prop_off, SeekOrigin.Begin);
                bw.Write(GetMemberOffset(type.default_property));
                bw.Seek(tmp, SeekOrigin.Begin);
            }
            bw.BaseStream.Seek(pos,SeekOrigin.Begin);
            bw.Write(names.Length);
            for (i=0; i<names.Length; i++)
			{
				bw.Write(names[i].name);
				bw.Write(names[i].offset);
			}
			bw.BaseStream.Seek(tmp,SeekOrigin.Begin);*/
            ClassInfo ci = new ClassInfo(pos, def_prop_off, base_class_off, interface_impl_off, names);
            class_info[type] = ci;
        }

        //ssyy
        private void VisitLabelDeclaration(label_node ln)
        {
            members[ln] = (int)bw.BaseStream.Position;
            bw.Write(ln.name);
            WriteDebugInfo(ln.Location);
        }

        private void VisitTemplateClassDefinition(template_class tclass)
        {
            int offset = 0;
            offset = SavePositionAndConstPool(tclass);//SavePosition(tclass);
            bw.Write((byte)tclass.semantic_node_type);
            //Пишем название шаблонного класса
            bw.Write(tclass.name);
            if (tclass.is_synonym)
                bw.Write((byte)1);
            else
                bw.Write((byte)0);
            //bw.Write(GetUnitReference(tclass.cnn)); //(ssyy) Нужно ли это?
            //Пишем имена подключаемых сборок
            bw.Write(tclass.using_list.Count);
            foreach (using_namespace un in tclass.using_list)
            {
                bw.Write(un.namespace_name);
            }
            if (tclass.using_list2 == null)
            {
                bw.Write((byte)0);
            }
            else
            {
                bw.Write((byte)1);
                bw.Write(tclass.using_list2.Count);
                foreach (using_namespace un in tclass.using_list2)
                {
                    bw.Write(un.namespace_name);
                }
            }
            //Пишем имя файла, где описан шаблон
            if (tclass.cur_document == null)
                bw.Write((byte)0);
            else
            {
                bw.Write((byte)1);
                bw.Write(Path.GetFileName(tclass.cur_document.file_name));
            }
            SyntaxTree.SyntaxTreeStreamWriter stw = new SyntaxTree.SyntaxTreeStreamWriter();
            stw.bw = bw;
            if (tclass.type_dec == null)
            {
                bw.Write((byte)0);
            }
            else
            {
                bw.Write((byte)1);
                //Пишем синтаксическое дерево шаблона
                tclass.type_dec.visit(stw);
            }
            bw.Write(tclass.external_methods.Count);
            foreach (procedure_definition_info pdi in tclass.external_methods)
            {
                if (pdi.nspace.scope is SymbolTable.UnitImplementationScope)
                {
                    bw.Write((byte)1);
                }
                else
                {
                    bw.Write((byte)0);
                }
                if (pdi.proc == null)
                {
                    bw.Write((byte)0);
                }
                else
                {
                    bw.Write((byte)1);
                    pdi.proc.visit(stw);
                }
            }
        }

        private void VisitTypeSynonym(type_synonym synonym)
        {
            bw.Write(synonym.name);
            int pos = (int)bw.BaseStream.Position;
            bw.Write(0);
            WriteTypeReference(synonym.original_type);
            WriteDebugInfo(synonym.Location);
            int next_pos = (int)bw.BaseStream.Position;
            bw.BaseStream.Seek(pos, SeekOrigin.Begin);
            bw.Write(next_pos-pos);
            bw.BaseStream.Seek(next_pos, SeekOrigin.Begin);
        }
        //\ssyy

        private void VisitCompiledTypeDefinition(compiled_type_node type)
        {
            int offset = 0;
            if (is_interface == true) offset = SavePositionAndConstPool(type);
            else offset = SavePositionAndImplementationPool(type);
            bw.Write((byte)type.semantic_node_type);
            bw.Write(is_interface);
            if (is_interface == true)
                bw.Write(GetNameIndex(type));
            else
                bw.Write(type.name);
            WriteTypeReference(type);
            WriteDebugInfo(type.Location);
        }

        private void VisitTypeMemberDefinition(common_type_node ctn)
        {
            VisitConstantInTypeDefinitions(ctn);
            VisitFieldDefinitions(ctn);
            VisitMethodDefinitions(ctn);
            VisitPropertyDefinitions(ctn);
            VisitEventDefinitions(ctn);
            int tmp = (int)bw.BaseStream.Position;
            ClassInfo ci = class_info[ctn];
            int def_prop_off = ci.def_prop_off;
            int pos = ci.names_pos;
            int base_class_off = ci.base_class_off;
            NameRef[] names = ci.names;
            if (ctn.base_type != null)
            {
                bw.Seek(base_class_off, SeekOrigin.Begin);
                WriteTypeReference(ctn.base_type);
                bw.Seek(tmp, SeekOrigin.Begin);
            }
            bw.Seek(ci.interf_impl_off, SeekOrigin.Begin);
            WriteImplementingInterfaces(ctn);
            bw.Seek(tmp, SeekOrigin.Begin);
            if (ctn.default_property != null)
            {
                bw.Seek(def_prop_off, SeekOrigin.Begin);
                bw.Write(GetMemberOffset(ctn.default_property));
                bw.Seek(tmp, SeekOrigin.Begin);
            }
            bw.BaseStream.Seek(pos, SeekOrigin.Begin);
            bw.Write(names.Length);
            for (int i = 0; i < names.Length; i++)
            {
                bw.Write(names[i].name);
                bw.Write(names[i].offset);
                bw.Write((byte)names[i].access_level);
                bw.Write((byte)names[i].symbol_kind);
                bw.Write((byte)names[i].semantic_node_type);
                bw.Write(names[i].always_restore);
                bw.Write(names[i].is_static);
            }
            bw.BaseStream.Seek(tmp, SeekOrigin.Begin);
        }

        //сохранение полей
		private void VisitFieldDefinitions(common_type_node ctn)
		{
			bw.Write(ctn.fields.Count);
			int offset = GetMemberOffset(ctn);
			for (int i=0; i<ctn.fields.Count; i++)
				VisitFieldDefinition(ctn.fields[i],offset);
		}
		
        //сохранение классовых констант
		private void VisitConstantInTypeDefinitions(common_type_node ctn)
		{
			int offset = GetMemberOffset(ctn);
			bw.Write(ctn.const_defs.Count);
			for (int i=0; i<ctn.const_defs.Count; i++)
				VisitConstantInTypeDefinition(ctn.const_defs[i],offset);
		}
		
        //сохранение методов
		private void VisitMethodDefinitions(common_type_node ctn)
		{
			int offset = GetMemberOffset(ctn);
			bw.Write(ctn.methods.Count);
			for (int i=0; i<ctn.methods.Count; i++)
				VisitMethodDefinition(ctn.methods[i],offset);
		}

        //сохранение свойств
		private void VisitPropertyDefinitions(common_type_node ctn)
		{
			int offset = GetMemberOffset(ctn);
			bw.Write(ctn.properties.Count);
			for (int i=0; i<ctn.properties.Count; i++)
				VisitPropertyDefinition(ctn.properties[i],offset);
		}
		
		private void VisitEventDefinitions(common_type_node ctn)
		{
			int offset = GetMemberOffset(ctn);
			bw.Write(ctn.events.Count);
			for (int i=0; i<ctn.events.Count; i++)
				VisitEventDefinition(ctn.events[i],offset);
		}
		
		private void VisitFieldDefinition(class_field field, int offset)
		{
            SavePositionAndConstPool(field);
			bw.Write((byte)field.semantic_node_type);
			bw.Write(GetNameIndex(field));
			WriteTypeReference(field.type);
            if (CanWriteObject(field.inital_value))
            {
            	SaveExpressionAndOffset(field.inital_value);
            	bw.Write(0);
            	//VisitExpression(field.inital_value);
            }
			bw.Write(offset);//comprehensive type
			bw.Write((byte)field.field_access_level);
			bw.Write((byte)field.polymorphic_state);
			SaveOffsetForAttribute(field);
            bw.Write(0);//attributes;
			WriteDebugInfo(field.loc);
		}
		
		private void VisitEventDefinition(common_event _event, int offset)
		{
			SavePositionAndConstPool(_event);
			bw.Write((byte)_event.semantic_node_type);
			bw.Write(GetNameIndex(_event));
			WriteTypeReference(_event.delegate_type);
			if (CanWriteObject(_event.add_method))
			bw.Write(GetMemberOffset(_event.add_method));
			if (CanWriteObject(_event.remove_method))
			bw.Write(GetMemberOffset(_event.remove_method));
			if (CanWriteObject(_event.raise_method))
			bw.Write(GetMemberOffset(_event.raise_method));
            if (!_event.cont_type.IsInterface)
                bw.Write(GetMemberOffset(_event.field));
            else
                bw.Write(0);
			bw.Write(offset);
			bw.Write((byte)_event.field_access_level);
			bw.Write((byte)_event.polymorphic_state);
			SaveOffsetForAttribute(_event);
            bw.Write(0);//attributes;
			WriteDebugInfo(_event.loc);
		}
		
		private void VisitConstantInTypeDefinition(class_constant_definition cdn, int offset)
		{
            SavePositionAndConstPool(cdn);
			bw.Write((byte)cdn.semantic_node_type);
			bw.Write(GetNameIndex(cdn));
			WriteTypeReference(cdn.type);
			bw.Write(offset);
			SaveExpressionAndOffset(cdn.const_value);
			bw.Write(0);
            bw.Write((byte)cdn.field_access_level);
			WriteDebugInfo(cdn.loc);
		}
		
		private void VisitMethodDefinition(common_method_node meth, int offset)
		{
            //DarkStar это вроде не нужно, он и так сохраняется ???
            //Вероятно здесь ошибка!!!
            if (IsDefined(meth))
                return;
            SavePositionAndConstPool(meth);
			bw.Write((byte)meth.semantic_node_type);
			bw.Write(GetNameIndex(meth));

            if (meth.is_final)
            {
                bw.Write((byte)1);
            }
            else
            {
                bw.Write((byte)0);
            }

            if (meth.newslot_awaited)
            {
                bw.Write((byte)1);
            }
            else
            {
                bw.Write((byte)0);
            }

            //Является ли метод описанием дженерика
            WriteGenericFunctionInformation(meth);
            //if (meth.is_generic_function)
            //{
            //    bw.Write((byte)1);
            //    //Число типов-параметров
            //    bw.Write(meth.generic_params.Count);
            //    //Имена параметров
            //    foreach (common_type_node par in meth.generic_params)
            //    {
            //        bw.Write(par.name);
            //    }
            //    WriteTypeParamsEliminations(meth.generic_params);
            //}
            //else
            //{
            //    bw.Write((byte)0);
            //}

			if (meth.return_value_type == null) 
                bw.Write((byte)0);
			else 
			{
				bw.Write((byte)1);
				WriteTypeReference(meth.return_value_type);
                if (meth.return_variable != null)
                {
                    bw.Write((byte)1);
                    VisitLocalVariable(meth.return_variable);
                }
                else 
                    bw.Write((byte)0);
            }
			bw.Write(meth.parameters.Count);
			foreach (common_parameter p in meth.parameters)
				VisitParameter(p);
			bw.Write(offset);
			SaveOffsetForAttribute(meth);
            bw.Write(0);//attributes;
			bw.Write(meth.is_constructor);
			bw.Write(meth.is_forward);
			bw.Write(meth.is_overload);
			bw.Write((byte)meth.field_access_level);
			bw.Write((byte)meth.polymorphic_state);
			bw.Write(meth.num_of_default_variables);
			bw.Write(meth.num_of_for_cycles);
			bw.Write(meth.overrided_method != null);
            
            //ssyy-
			//if (meth.pascal_associated_constructor != null)
			//{
			//	bw.Write((byte)1);
			//	bw.Write(GetMemberOffset(meth.pascal_associated_constructor));
			//}
			//else
			//	bw.Write((byte)0);
            //\ssyy
			if (meth.return_variable!=null)
                bw.Write(meth.var_definition_nodes_list.Count-1);
            else
                bw.Write(meth.var_definition_nodes_list.Count); 
            foreach (local_variable var in meth.var_definition_nodes_list)
            {
                if (var != meth.return_variable) 
                    VisitLocalVariable(var);
            }
            bw.Write(meth.constants.Count);
            foreach (function_constant_definition cnst in meth.constants)
            	VisitFunctionConstant(cnst);
            bw.Write(meth.functions_nodes_list.Count);
			foreach (common_in_function_function_node nest_func in meth.functions_nodes_list)
			    VisitNestedFunctionDefinition(nest_func);
			WriteDebugInfo(meth.loc);
			SaveCodeReference(meth);
			bw.Write(0);//????
		}
		
		private void VisitPropertyDefinition(common_property_node prop, int offset)
		{
            if (members.ContainsKey(prop)) return;
            SavePositionAndConstPool(prop);
			bw.Write((byte)prop.semantic_node_type);
			bw.Write(GetNameIndex(prop));
			WriteTypeReference(prop.property_type);
            if (prop.get_function != null)
            {
                bw.Write((byte)1);
                bw.Write(GetMemberOffset(prop.get_function));
            }
            else bw.Write((byte)0);
            if (prop.set_function != null)
            {
                bw.Write((byte)1);
                bw.Write(GetMemberOffset(prop.set_function));
            }
            else bw.Write((byte)0);
            bw.Write(prop.parameters.Count);
			foreach (common_parameter p in prop.parameters)
				VisitParameter(p);
			bw.Write(offset);
			bw.Write((byte)prop.field_access_level);
			bw.Write((byte)prop.polymorphic_state);
			SaveOffsetForAttribute(prop);
            bw.Write(0);//attributes;
			WriteDebugInfo(prop.loc);
		}
		
        //сохранение функций
		private void VisitFunctionDefinitions()
		{
			bw.Write(cur_cnn.functions.Count);
			for (int i=0; i<cur_cnn.functions.Count; i++)
				VisitFunctionDefinition(cur_cnn.functions[i]);
		}

        private void WriteGenericFunctionInformation(common_function_node func)
        {
            if (func.is_generic_function)
            {
                bw.Write((byte)1);
                //Число типов-параметров
                bw.Write(func.generic_params.Count);
                //Имена параметров
                foreach (common_type_node par in func.generic_params)
                {
                    bw.Write(par.name);
                }
                WriteTypeParamsEliminations(func.generic_params);
            }
            else
            {
                bw.Write((byte)0);
            }
        }

        private void VisitFunctionDefinition(common_namespace_function_node func)
        {
            int pos;
            if (is_interface == true)
                pos = SavePositionAndConstPool(func);
            else
                pos = SavePositionAndImplementationPool(func);

            //Переделать это!
            if (cun.main_function == func)
                InitializationMethodOffset = pos;
            if (cun.finalization_method == func)
                FinalizationMethodOffset = pos;

            bw.Write((byte)func.semantic_node_type);
            int connected_to_type_pos = (int)bw.BaseStream.Position;
            bw.Write(0);
            if (CanWriteObject(func.ConnectedToType))
                WriteTypeReference(func.ConnectedToType);
            int cur_pos = (int)bw.BaseStream.Position;
            bw.Seek(connected_to_type_pos, SeekOrigin.Begin);
            bw.Write(cur_pos);
            bw.Seek(cur_pos, SeekOrigin.Begin);
            bw.Write(is_interface);
            if (is_interface == true)
                bw.Write(GetNameIndex(func));
            else
                bw.Write(func.name);

            //Является ли метод описанием дженерика
            WriteGenericFunctionInformation(func);
            //if (func.is_generic_function)
            //{
            //    bw.Write((byte)1);
            //    //Число типов-параметров
            //    bw.Write(func.generic_params.Count);
            //    //Имена параметров
            //    foreach (common_type_node par in func.generic_params)
            //    {
            //        bw.Write(par.name);
            //    }
            //    WriteTypeParamsEliminations(func.generic_params);
            //}
            //else
            //{
            //    bw.Write((byte)0);
            //}

            if (func.return_value_type == null) bw.Write((byte)0);
            else
            {
                bw.Write((byte)1);
                WriteTypeReference(func.return_value_type);
                VisitLocalVariable(func.return_variable);
            }
            bw.Write(func.parameters.Count);
            foreach (common_parameter p in func.parameters)
                VisitParameter(p);
            bw.Write(GetUnitReference(func.namespace_node));
            SaveOffsetForAttribute(func);
            bw.Write(0);//attributes;
            bw.Write(func.is_forward);
            bw.Write(func.is_overload);
            bw.Write(func.num_of_default_variables);
            bw.Write(func.num_of_for_cycles);
            bw.Write(func.var_definition_nodes_list.Count);
            foreach (local_variable var in func.var_definition_nodes_list)
                if (var != func.return_variable)
                    VisitLocalVariable(var);
            bw.Write(func.constants.Count);
            foreach (function_constant_definition cnst in func.constants)
            	VisitFunctionConstant(cnst);
            bw.Write(func.functions_nodes_list.Count);
            foreach (common_in_function_function_node nest_func in func.functions_nodes_list)
                VisitNestedFunctionDefinition(nest_func);
            WriteDebugInfo(func.loc);
            SaveCodeReference(func);
            bw.Write(0);
        }

        //сохранение initialization и finalization частей
        private int VisitFunctionWithImplementation(common_namespace_function_node func)
        {
            int pos = SavePosition(func);
            bw.Write((byte)func.semantic_node_type);
            bw.Write(func.name);
            bw.Write(GetUnitReference(func.namespace_node));
            WriteDebugInfo(func.loc);
            if (func.function_code != null)
            {
                bw.Write((byte)1);
                VisitStatement(func.function_code);
            }
            else bw.Write((byte)0);
            return pos;
        }

        private void VisitTypeImplementation(common_type_node type)
        {
            foreach (common_method_node meth in type.methods)
                VisitMethodImplementation(meth);
        }

        //сохранение реализации метода
        private void VisitMethodImplementation(common_method_node meth)
        {
            foreach (common_in_function_function_node nested in meth.functions_nodes_list)
                VisitNestedFunctionImplementation(nested);
            //(ssyy) метки
            VisitLabelDeclarations(meth.label_nodes_list);
            FixupCode(meth);
            //if (meth.function_code == null)
            //{
            //    bw.Write((byte)0);
            //}
            //else
            //{
            //    bw.Write((byte)1);
            if (meth.function_code == null || meth.name.IndexOf("<yield_helper_error_checkerr>") != -1)
            {
                VisitStatement(new empty_statement(null));
            }
            else
            {
                VisitStatement(meth.function_code);
            }
            if (meth.overrided_method != null)
                WriteMethodReference(meth.overrided_method);
            //}
        }

        //(ssyy) Может быть, соединить следующие 2 метода в один?
		private void VisitFunctionImplementation(common_namespace_function_node func)
		{
			foreach (common_in_function_function_node nested in func.functions_nodes_list)
				VisitNestedFunctionImplementation(nested);
            //(ssyy) метки
            VisitLabelDeclarations(func.label_nodes_list);
			FixupCode(func);
            if (func.name.IndexOf("<yield_helper_error_checkerr>") != -1)
                VisitStatement(new empty_statement(null));
            else            
                VisitStatement(func.function_code);
		}
		
		private void VisitNestedFunctionImplementation(common_in_function_function_node func)
		{
			foreach (common_in_function_function_node nested in func.functions_nodes_list)
				VisitNestedFunctionImplementation(nested);
            //(ssyy) метки
            VisitLabelDeclarations(func.label_nodes_list);
            FixupCode(func);
			VisitStatement(func.function_code);
		}
		
		private void VisitFunctionConstant(function_constant_definition cnst)
		{
			SavePosition(cnst);
			bw.Write(cnst.name);
			SaveExpressionAndOffset(cnst.const_value);
			bw.Write(0);
			//VisitExpression((expression_node)cnst.const_value);
		}
		
        //сохранение лок. переменной
		private void VisitLocalVariable(local_variable var)
		{
			SavePosition(var);
			//bw.Write((byte)var.semantic_node_type);
			bw.Write(var.name);
			WriteTypeReference(var.type);
			bw.Write(var.is_used_as_unlocal);
            if (CanWriteObject(var.inital_value))
            {
            	SaveExpressionAndOffset(var.inital_value);
            	bw.Write(0);
            	//VisitExpression((expression_node)var.inital_value);
            }
        }
		
        //сохранение параметра
		private void VisitParameter(common_parameter p)
		{
			SavePosition(p);
			bw.Write((byte)p.semantic_node_type);
            if (p.name == null)
                bw.Write("");
            else
			    bw.Write(p.name);
			WriteTypeReference(p.type);
			bw.Write((byte)p.concrete_parameter_type);
			bw.Write(p.is_used_as_unlocal);
            bw.Write(p.is_params);
            if (CanWriteObject(p.default_value))
            {
            	SaveExpressionAndOffset(p.default_value);
            	bw.Write(0);
            	//VisitExpression((expression_node)var.inital_value);
            }
            SaveOffsetForAttribute(p);
            bw.Write(0);//attributes;
		}
		
        //сохранение вложенной функций
		private void VisitNestedFunctionDefinition(common_in_function_function_node func)
		{
			SavePosition(func);
			bw.Write((byte)func.semantic_node_type);
			bw.Write(func.name);
			if (func.return_value_type == null) bw.Write((byte)0);
			else 
			{
				bw.Write((byte)1);
				WriteTypeReference(func.return_value_type);
				VisitLocalVariable(func.return_variable);
			}
            bw.Write(func.parameters.Count);
            foreach (common_parameter p in func.parameters)
                VisitParameter(p);
            bw.Write(0);
			bw.Write(func.is_forward);
			bw.Write(func.is_overload);
			bw.Write(func.num_of_default_variables);
			bw.Write(func.num_of_for_cycles);
			bw.Write(func.var_definition_nodes_list.Count);
			foreach (local_variable var in func.var_definition_nodes_list)
			 if (var != func.return_variable) VisitLocalVariable(var);
			bw.Write(func.constants.Count);
            foreach (function_constant_definition cnst in func.constants)
            	VisitFunctionConstant(cnst);
			bw.Write(func.functions_nodes.Length);
			foreach (common_in_function_function_node nest_func in func.functions_nodes_list)
			 VisitNestedFunctionDefinition(nest_func);
            WriteDebugInfo(func.loc);
            SaveCodeReference(func);
			bw.Write(0);
		}
		
		//сохранение отлад. информации
		private void WriteDebugInfo(SemanticTree.ILocation loc)
		{
            WriteDebugInfo(bw,loc);
		}
        
		//сохранение отлад. информации
        private void WriteDebugInfo(BinaryWriter bw,SemanticTree.ILocation loc)
        {
            if (pcu_file.IncludeDebugInfo)
            {
                if (loc != null)
                {
                    bw.Write(loc.begin_line_num);
                    bw.Write(loc.begin_column_num);
                    bw.Write(loc.end_line_num);
                    bw.Write(loc.end_column_num);
                }
                else
                {
                    bw.Write((int)-1);
                    bw.Write((int)-1);
                    bw.Write((int)-1);
                    bw.Write((int)-1);
                }
            }
        }

        private bool CanWriteObject(object obj)
        {
            if (obj == null)
                bw.Write((byte)0);
            else
                bw.Write((byte)1);
            return obj != null;
        }

        private void VisitTryBlock(try_block tryblock)
        {
            VisitStatement(tryblock.try_statements);
            if (CanWriteObject(tryblock.finally_statements))
                VisitStatement(tryblock.finally_statements);
            bw.Write(tryblock.filters.Count);
            foreach (exception_filter ef in tryblock.filters)
            {
                if (CanWriteObject(ef.filter_type))
                    WriteTypeReference(ef.filter_type);
                if (CanWriteObject(ef.exception_var))
                {
                    VisitLocalBlockVariable(ef.exception_var.var);
                    VisitLocalBlockVariableReference(ef.exception_var);
                }
                VisitStatement(ef.exception_handler);
                WriteDebugInfo(ef.location);
            }
        }
		
		private void VisitStatement(statement_node sn)
		{
			if (sn is expression_node)
			{
				VisitExpression((expression_node)sn);
                WriteDebugInfo(sn.location);
				return;
			}
			bw.Write((byte)sn.semantic_node_type);
			switch (sn.semantic_node_type) {
				case semantic_node_type.if_node : VisitIf((if_node)sn); break;
				case semantic_node_type.while_node : VisitWhile((while_node)sn); break;
				case semantic_node_type.repeat_node : VisitRepeat((repeat_node)sn); break;
				case semantic_node_type.for_node : VisitFor((for_node)sn); break;
				case semantic_node_type.statements_list : VisitStatementList((statements_list)sn); break;
				case semantic_node_type.empty_statement : VisitEmpty((empty_statement)sn); break;
				case semantic_node_type.return_node : VisitReturnNode((return_node)sn); break;
                case semantic_node_type.switch_node : VisitSwitchNode((switch_node)sn); break;
                case semantic_node_type.external_statement_node: VisitExternalStatementNode((external_statement)sn); break;
                case semantic_node_type.throw_statement: VisitThrow((throw_statement_node)sn); break;
                case semantic_node_type.runtime_statement: VisitRuntimeStatement((runtime_statement)sn); break;
                case semantic_node_type.try_block: VisitTryBlock((try_block)sn); break;
                case semantic_node_type.labeled_statement: VisitLabeledStatement((labeled_statement)sn); break;
                case semantic_node_type.goto_statement: VisitGoto((goto_statement)sn); break;
                case semantic_node_type.foreach_node: VisitForeach((foreach_node)sn); break;
                case semantic_node_type.lock_statement: VisitLock((lock_statement)sn); break;
                case semantic_node_type.rethrow_statement: VisitRethrow((rethrow_statement_node)sn); break;
                case semantic_node_type.pinvoke_node : VisitPInvokeStatement((pinvoke_statement)sn); break;
                
            }
		    WriteDebugInfo(sn.location);
		}
		
		private void VisitForeachBreak(foreach_break_node sn)
		{
				
		}
		
		private void VisitForeachContinue(foreach_continue_node sn)
		{
				
		}
		
		private void VisitRethrow(rethrow_statement_node sn)
		{
			
		}
		
        private void VisitForeach(foreach_node fn)
        {
            //VisitVariableDefinition(fn.ident);
            bw.Write(GetMemberOffset(fn.ident));
            VisitExpression(fn.in_what);
            VisitStatement(fn.what_do);
            if (CanWriteObject(fn.element_type))
                WriteTypeReference(fn.element_type);
            bw.Write(fn.is_generic);
        }

        private void VisitLock(lock_statement node)
        {
            VisitExpression(node.lock_object);
            VisitStatement(node.body);
        }

        private void VisitLabeledStatement(labeled_statement ls)
        {
            bw.Write(members[ls.label]);
            WriteDebugInfo(ls.location);
            VisitStatement(ls.statement);
        }

        private void VisitGoto(goto_statement gs)
        {
            bw.Write(members[gs.label]);
            WriteDebugInfo(gs.location);
        }

        private void VisitThrow(throw_statement_node stmt)
        {
            VisitExpression(stmt.excpetion);
        }
		
		private void VisitIf(if_node stmt)
		{
			VisitExpression(stmt.condition);
			VisitStatement(stmt.then_body);
			if (stmt.else_body != null)
			{
				bw.Write((byte)1);
				VisitStatement(stmt.else_body);
			}
			else
			{
				bw.Write((byte)0);
			}
		}
		
		private void VisitWhile(while_node stmt)
		{
			VisitExpression(stmt.condition);
			VisitStatement(stmt.body);
		}
		
		private void VisitRepeat(repeat_node stmt)
		{
			VisitStatement(stmt.body);
			VisitExpression(stmt.condition);
		}
		
		private void VisitFor(for_node stmt)
		{
            if(CanWriteObject(stmt.initialization_statement))
			    VisitStatement(stmt.initialization_statement);
			VisitExpression(stmt.while_expr);
			if (CanWriteObject(stmt.init_while_expr))
				VisitExpression(stmt.init_while_expr);
			VisitStatement(stmt.increment_statement);
			VisitStatement(stmt.body);
			bw.Write(stmt.bool_cycle);
		}

        private void VisitSwitchNode(switch_node stmt)
        {
            VisitExpression(stmt.condition);
            bw.Write(stmt.case_variants.Count);
            foreach (case_variant_node cvn in stmt.case_variants)
                VisitCaseVariantNode(cvn);
            if (stmt.default_statement != null)
            {
                bw.Write((byte)1);
                VisitStatement(stmt.default_statement);
            }
            else
                bw.Write((byte)0);
        }

        private void VisitCaseVariantNode(case_variant_node cvn)
        {
            bw.Write(cvn.case_constants.Count);
            foreach (constant_node cnst in cvn.case_constants)
                VisitExpression(cnst);
            bw.Write(cvn.case_ranges.Count);
            foreach (case_range_node crn in cvn.case_ranges)
            {
                VisitExpression(crn.lower_bound);
                VisitExpression(crn.high_bound);
            }
            VisitStatement(cvn.case_statement);
        }

        private void VisitRuntimeStatement(runtime_statement rts)
        {
            bw.Write(((int)(rts.runtime_statement_type)));
        }

        private void VisitExternalStatementNode(external_statement stmt)
        {
            bw.Write(stmt.module_name);
            bw.Write(stmt.name);
        }
		
        private void VisitPInvokeStatement(pinvoke_statement stmt)
        {
        	
        }
        
		private void VisitStatementList(statements_list stmt)
        {
            bw.Write(stmt.local_variables.Count);
            foreach (local_block_variable lv in stmt.local_variables)
                VisitLocalBlockVariable(lv);
			bw.Write(stmt.statements.Count);
			foreach (statement_node sn in stmt.statements)
				VisitStatement(sn);
            WriteDebugInfo(stmt.LeftLogicalBracketLocation);
            WriteDebugInfo(stmt.RightLogicalBracketLocation);
		}

        private void VisitLocalBlockVariable(local_block_variable lv)
        {
            SavePosition(lv);			
            bw.Write(lv.name);
            WriteTypeReference(lv.type);
            if(CanWriteObject(lv.inital_value))
            {
            	SaveExpressionAndOffset(lv.inital_value);
            	bw.Write(0);
            	//VisitExpression(lv.inital_value);
            }
            WriteDebugInfo(lv.loc);
        }
		
		private void VisitEmpty(empty_statement stmt)
		{
		}

        private void VisitIsNode(is_node node)
        {
            VisitExpression(node.left);
            WriteTypeReference(node.right);
        }
        
        private void VisitAsNode(as_node node)
        {
            VisitExpression(node.left);
            WriteTypeReference(node.right);
        }

        private void VisitTypeOfOperator(typeof_operator node)
        {
            WriteTypeReference(node.oftype);
        }
        
        private void VisitSizeOfOperator(sizeof_operator node)
        {
            WriteTypeReference(node.oftype);           
        }
        
        private void VisitArrayConst(array_const node)
        {
            bw.Write(node.element_values.Count);
            foreach (constant_node cn in node.element_values)
                VisitExpression(cn);
            WriteTypeReference(node.type);
        }
        
        private void VisitArrayInitializer(array_initializer node)
        {
        	bw.Write(node.element_values.Count);
            foreach (expression_node en in node.element_values)
                VisitExpression(en);
            WriteTypeReference(node.type);
        }
        
        private void VisitRecordConst(record_constant node)
        {
            bw.Write(node.field_values.Count);
            foreach (constant_node cn in node.field_values)
                VisitExpression(cn);
            WriteTypeReference(node.type);
        }
        
        private void VisitQuestionColonExpression(question_colon_expression node)
        {
            VisitExpression(node.internal_condition);
            VisitExpression(node.internal_ret_if_true);
            VisitExpression(node.internal_ret_if_false);
        }

        private void VisitDoubleQuestionColonExpression(double_question_colon_expression node)
        {
            VisitExpression(node.internal_condition);
            VisitExpression(node.internal_ret_if_null);
        }

        private void VisitStatementsExpressionNode(statements_expression_node node)
        {
            bw.Write(node.internal_statements.Count);
            foreach (statement_node stmt in node.internal_statements)
                VisitStatement(stmt);
            VisitExpression(node.internal_expression);
        }

        private void VisitFloatConst(float_const_node node)
        {
            bw.Write((Single)node.constant_value);
        }

        private void VisitCompiledStaticMethodCallNodeAsConstant(compiled_static_method_call_as_constant node)
        {
            VisitCompiledStaticMethodCall(node.method_call);
        }

        private void VisitCommonStaticMethodCallNodeAsConstant(common_static_method_call_as_constant node)
        {
            VisitCommonStaticMethodCall(node.method_call);
        }

        private void VisitCompiledConstructorCallAsConstant(compiled_constructor_call_as_constant node)
        {
            VisitCompiledConstructorCall(node.method_call);
        }

        private void VisitSizeOfOperatorAsConstant(sizeof_operator_as_constant node)
        {
            VisitSizeOfOperator(node.sizeof_operator);
        }

        private void VisitCommonNamespaceFunctionCallNodeAsConstant(common_namespace_function_call_as_constant node)
        {
            VisitCommonNamespaceFunctionCall(node.method_call);
            WriteTypeReference(node.type);
        }

		private void VisitExpression(expression_node en)
		{
			bw.Write((byte)en.semantic_node_type);
            //WriteDebugInfo(en.location);
			switch (en.semantic_node_type) {
                case semantic_node_type.exit_procedure:
                    /*ничего писать не надо*/ break;
                case semantic_node_type.typeof_operator:
                    VisitTypeOfOperator((typeof_operator)en); break;
                case semantic_node_type.statement_expression_node:
                    VisitStatementsExpressionNode((statements_expression_node)en); break;
                case semantic_node_type.question_colon_expression:
                    VisitQuestionColonExpression((question_colon_expression)en); break;
                case semantic_node_type.double_question_colon_expression:
                    VisitDoubleQuestionColonExpression((double_question_colon_expression)en); break;
                case semantic_node_type.sizeof_operator:
                    VisitSizeOfOperator((sizeof_operator)en); break;
                case semantic_node_type.is_node:
                    VisitIsNode((is_node)en); break;
                case semantic_node_type.as_node:
                    VisitAsNode((as_node)en); break;
                case semantic_node_type.compiled_static_method_call_node_as_constant:
                    VisitCompiledStaticMethodCallNodeAsConstant((compiled_static_method_call_as_constant)en); break;
                case semantic_node_type.common_static_method_call_node_as_constant:
                    VisitCommonStaticMethodCallNodeAsConstant((common_static_method_call_as_constant)en); break;
                case semantic_node_type.common_namespace_function_call_node_as_constant:
                    VisitCommonNamespaceFunctionCallNodeAsConstant((common_namespace_function_call_as_constant)en); break;
                case semantic_node_type.compiled_constructor_call_as_constant:
                    VisitCompiledConstructorCallAsConstant((compiled_constructor_call_as_constant)en); break;
                case semantic_node_type.sizeof_operator_as_constant:
                    VisitSizeOfOperatorAsConstant((sizeof_operator_as_constant)en); break;
                case semantic_node_type.array_const:
                    VisitArrayConst((array_const)en); break;
                case semantic_node_type.record_const:
                    VisitRecordConst((record_constant)en); break;
                case semantic_node_type.float_const_node:
                    VisitFloatConst((float_const_node)en);break;
                case semantic_node_type.byte_const_node:
                    VisitByteConstNode((byte_const_node)en); break;
                case semantic_node_type.int_const_node:
					VisitIntConstNode((int_const_node)en); break;
                case semantic_node_type.sbyte_const_node:
                    VisitSByteConstNode((sbyte_const_node)en); break;
                case semantic_node_type.short_const_node:
                    VisitShortConstNode((short_const_node)en); break;
                case semantic_node_type.ushort_const_node:
                    VisitUShortConstNode((ushort_const_node)en); break;
                case semantic_node_type.uint_const_node:
                    VisitUIntConstNode((uint_const_node)en); break;
                case semantic_node_type.ulong_const_node:
                    VisitULongConstNode((ulong_const_node)en); break;
                case semantic_node_type.long_const_node:
                    VisitLongConstNode((long_const_node)en); break;
                case semantic_node_type.double_const_node:
					VisitDoubleConstNode((double_const_node)en); break;
				case semantic_node_type.char_const_node :
					VisitCharConstNode((char_const_node)en); break;
				case semantic_node_type.bool_const_node :
					VisitBoolConstNode((bool_const_node)en); break;
				case semantic_node_type.string_const_node :
					VisitStringConstNode((string_const_node)en); break;
				case semantic_node_type.local_variable_reference :
					VisitLocalVariableReference((local_variable_reference)en); break;
                case semantic_node_type.local_block_variable_reference:
                    VisitLocalBlockVariableReference((local_block_variable_reference)en); break;
                case semantic_node_type.namespace_variable_reference:
					VisitNamespaceVariableReference((namespace_variable_reference)en); break;
				case semantic_node_type.basic_function_call :
					VisitBasicFunctionCall((basic_function_call)en); break;
				case semantic_node_type.common_parameter_reference :
					VisitCommonParameterReference((common_parameter_reference)en); break;
				case semantic_node_type.common_namespace_function_call :
					VisitCommonNamespaceFunctionCall((common_namespace_function_call)en); break;
				case semantic_node_type.common_in_function_function_call :
					VisitCommonInFuncFuncCall((common_in_function_function_call)en); break;
				case semantic_node_type.while_break_node:
					VisitWhileBreakNode((while_break_node)en); break;
				case semantic_node_type.while_continue_node:
					VisitWhileContinueNode((while_continue_node)en); break;
				case semantic_node_type.for_break_node:
					VisitForBreakNode((for_break_node)en); break;
				case semantic_node_type.for_continue_node:
					VisitForContinueNode((for_continue_node)en); break;
				case semantic_node_type.repeat_break_node:
					VisitRepeatBreakNode((repeat_break_node)en); break;
				case semantic_node_type.repeat_continue_node:
					VisitRepeatContinueNode((repeat_continue_node)en); break;
				case semantic_node_type.compiled_static_method_call:
					VisitCompiledStaticMethodCall((compiled_static_method_call)en); break;
				case semantic_node_type.class_field_reference:
					VisitClassFieldReference((class_field_reference)en); break;
				case semantic_node_type.deref_node:
					VisitDerefNode((dereference_node)en); break;
				case semantic_node_type.common_method_call:
					VisitCommonMethodCall((common_method_call)en); break;
				case semantic_node_type.compiled_function_call:
					VisitCompiledFunctionCall((compiled_function_call)en); break;
				case semantic_node_type.get_addr_node:
					VisitGetAddrNode((get_addr_node)en); break;
				case semantic_node_type.common_constructor_call:
					VisitCommonConstructorCall((common_constructor_call)en); break;
				case semantic_node_type.compiled_constructor_call:
					VisitCompiledConstructorCall((compiled_constructor_call)en); break;
				case semantic_node_type.compiled_variable_reference:
					VisitCompiledVariableReference((compiled_variable_reference)en); break;
				case semantic_node_type.static_compiled_variable_reference:
					VisitStaticCompiledVariableReference((static_compiled_variable_reference)en); break;
				case semantic_node_type.static_class_field_reference:
					VisitStaticClassFieldReference((static_class_field_reference)en); break;
                case semantic_node_type.non_static_property_reference:
                    VisitNonStaticPropertyReference((non_static_property_reference)en); break;
				case semantic_node_type.simple_array_indexing:
					VisitSimpleArrayIndexing((simple_array_indexing)en); break;
				case semantic_node_type.this_node:
					VisitThisNode((this_node)en); break;
                case semantic_node_type.null_const_node:
                    VisitNullConstNode((null_const_node)en); break;
                case semantic_node_type.enum_const:
                    VisitEnumConstNode((enum_const_node)en); break;
                case semantic_node_type.foreach_break_node: 
                    VisitForeachBreak((foreach_break_node)en); break;
                case semantic_node_type.foreach_continue_node: 
                    VisitForeachContinue((foreach_continue_node)en); break;
                case semantic_node_type.namespace_constant_reference:
                    VisitNamespaceConstantReference((namespace_constant_reference)en); break;
                case semantic_node_type.function_constant_reference:
                    VisitFunctionConstantReference((function_constant_reference)en); break;
                case semantic_node_type.common_constructor_call_as_constant:
                    VisitCommonConstructorCallAsConstant((common_constructor_call_as_constant)en); break;
                case semantic_node_type.basic_function_call_node_as_constant:
                    VisitBasicFunctionCallAsConstant((basic_function_call_as_constant)en); break;
                case semantic_node_type.default_operator_node_as_constant:
                    VisitDefaultOperatorAsConstant((default_operator_node_as_constant)en); break;
                case semantic_node_type.array_initializer:
                    VisitArrayInitializer((array_initializer)en); break;
                case semantic_node_type.record_initializer:
                    VisitRecordInitializer((record_initializer)en); break;
                case semantic_node_type.common_static_method_call:
                    VisitCommonStaticMethodCall((common_static_method_call)en); break;
                case semantic_node_type.default_operator:
                    VisitDefaultOperator((default_operator_node)en); break;
                case semantic_node_type.compiled_static_field_reference_as_constant:
                    VisitCompiledStaticFieldReferenceAsConstant((compiled_static_field_reference_as_constant)en); break;
				case semantic_node_type.nonstatic_event_reference:
                    VisitNonStaticEventReference((nonstatic_event_reference)en); break;
                case semantic_node_type.static_event_reference:
                    VisitStaticEventReference((static_event_reference)en); break;
                default : throw new Exception("Unknown expression");
			}
		}

        private void VisitStaticEventReference(static_event_reference en)
        {
            WriteEventReference(en.en);
        }

        private void VisitNonStaticEventReference(nonstatic_event_reference en)
        {
            VisitExpression(en.obj);
            WriteEventReference(en.en as common_event);
        }

        private void VisitCompiledStaticFieldReferenceAsConstant(compiled_static_field_reference_as_constant en)
        {
            VisitStaticCompiledVariableReference(en.field_reference);
        }

		private void VisitDefaultOperator(default_operator_node expr)
		{
			WriteTypeReference(expr.type);
		}
		
		private void VisitRecordInitializer(record_initializer expr)
		{
			bw.Write(expr.field_values.Count);
			foreach (expression_node en in expr.field_values)
				VisitExpression(en);
			WriteTypeReference(expr.type);
		}
		
		private void VisitCommonConstructorCallAsConstant(common_constructor_call_as_constant expr)
		{
			VisitCommonConstructorCall(expr.constructor_call);
		}
		
		private void VisitBasicFunctionCallAsConstant(basic_function_call_as_constant expr)
		{
			VisitBasicFunctionCall(expr.method_call);
		}
		
        private void VisitDefaultOperatorAsConstant(default_operator_node_as_constant expr)
        {
            VisitDefaultOperator(expr.default_operator);
        }

        private void VisitNamespaceConstantReference(namespace_constant_reference expr)
		{
			WriteConstantReference(expr.constant);
		}
		
		private void VisitFunctionConstantReference(function_constant_reference expr)
		{
			bw.Write(GetMemberOffset(expr.constant));
		}
		
        private void VisitNullConstNode(null_const_node expr)
        {
            if (expr.type != null && !(expr.type is null_type_node) && !(expr.type is delegated_methods))
            {
                bw.Write((byte)1);
                WriteTypeReference(expr.type);
            }
            else
            {
                bw.Write((byte)0);
            }
        }

        private void VisitEnumConstNode(enum_const_node en)
        {
            bw.Write(en.constant_value);
            WriteTypeReference(en.type);
        }

        private void VisitByteConstNode(byte_const_node expr)
        {
            bw.Write(expr.constant_value);
        }
        
        private void VisitIntConstNode(int_const_node expr)
        {
            bw.Write(expr.constant_value);
        }

        private void VisitSByteConstNode(sbyte_const_node expr)
        {
            bw.Write(expr.constant_value);
        }

        private void VisitShortConstNode(short_const_node expr)
        {
            bw.Write(expr.constant_value);
        }

        private void VisitUShortConstNode(ushort_const_node expr)
        {
            bw.Write(expr.constant_value);
        }

        private void VisitUIntConstNode(uint_const_node expr)
        {
            bw.Write(expr.constant_value);
        }

        private void VisitULongConstNode(ulong_const_node expr)
        {
            bw.Write(expr.constant_value);
        }

        private void VisitLongConstNode(long_const_node expr)
        {
            bw.Write(expr.constant_value);
        }
		
		private void VisitDoubleConstNode(double_const_node expr)
		{
			bw.Write(expr.constant_value);
		}
		
		private void VisitCharConstNode(char_const_node expr)
		{
			bw.Write(expr.constant_value);
		}
		
		private void VisitBoolConstNode(bool_const_node expr)
		{
			bw.Write(expr.constant_value);
		}
		
		private void VisitStringConstNode(string_const_node expr)
		{
			bw.Write(expr.constant_value);
		}
		
		private void VisitLocalVariableReference(local_variable_reference expr)
		{
			bw.Write(GetMemberOffset(expr.var));
			//bw.Write(expr.static_depth);
		}

        private void VisitLocalBlockVariableReference(local_block_variable_reference expr)
        {
            bw.Write(GetMemberOffset(expr.var));
            //bw.Write(expr.static_depth);
        }
        
        private void VisitNamespaceVariableReference(namespace_variable_reference expr)
		{
			WriteVariableReference(expr.var);
		}
		
		private void VisitCommonParameterReference(common_parameter_reference expr)
		{
            bw.Write(GetMemberOffset(expr.par));
		}
		
		private void VisitBasicFunctionCall(basic_function_call expr)
		{
			bw.Write((System.Int16)expr.function_node.basic_function_type);
            WriteTypeReference(expr.ret_type);
            if (expr.conversion_type is delegated_methods)
                WriteTypeReference(null);
            else
                WriteTypeReference(expr.conversion_type);
            bw.Write(expr.parameters.Count);
			for (int i=0; i<expr.parameters.Count; i++)
			    VisitExpression(expr.parameters[i]);
		}
		
		private void VisitCommonNamespaceFunctionCall(common_namespace_function_call expr)
		{
			WriteFunctionReference(expr.function_node);
			bw.Write(expr.parameters.Count);
			foreach (expression_node e in expr.parameters)
				VisitExpression(e);
		}
		
		private void VisitCommonInFuncFuncCall(common_in_function_function_call expr)
		{
			bw.Write(GetMemberOffset(expr.function_node));
			bw.Write(expr.parameters.Count);
			foreach (expression_node e in expr.parameters)
				VisitExpression(e);
		}
		
		private void VisitCompiledStaticCall(compiled_static_method_call expr)
		{
			bw.Write(GetMemberOffset(expr.function_node));
            /*bw.Write(expr.template_parametres_list.Count);
            foreach (type_node tn in expr.template_parametres_list)
                WriteTypeReference(tn);*/
            bw.Write(expr.parameters.Count);
			foreach (expression_node e in expr.parameters)
				VisitExpression(e);
		}
		
		private void VisitReturnNode(return_node expr)
		{
			VisitExpression(expr.return_expr);
		}
		
		private void VisitCompiledStaticMethodCall(compiled_static_method_call expr)
		{
			WriteCompiledMethod(expr.function_node);
            bw.Write(expr.template_parametres_list.Count);
            foreach (type_node tn in expr.template_parametres_list)
                WriteTypeReference(tn);
			bw.Write(expr.parameters.Count);
			foreach (expression_node e in expr.parameters)
				VisitExpression(e);
		}
		
		private void VisitWhileBreakNode(while_break_node expr)
		{
			
		}
		
		private void VisitWhileContinueNode(while_continue_node expr)
		{
			
		}
		
		private void VisitForBreakNode(for_break_node expr)
		{
			
		}
		
		private void VisitForContinueNode(for_continue_node expr)
		{
			
		}
		
		private void VisitRepeatBreakNode(repeat_break_node expr)
		{
			
		}
		
		private void VisitRepeatContinueNode(repeat_continue_node expr)
		{
			
		}
		
		private void VisitClassFieldReference(class_field_reference expr)
		{
			VisitExpression(expr.obj);
			WriteFieldReference(expr.field);
		}

        private void VisitNonStaticPropertyReference(non_static_property_reference expr)
        {
            VisitExpression(expr.expression);
            WritePropertyReference((common_property_node)expr.property);
            bw.Write(expr.fact_parametres.Count);
            for (int i = 0; i < expr.fact_parametres.Count; i++)
                VisitExpression(expr.fact_parametres[i]);
        }

        private void VisitDerefNode(dereference_node expr)
		{
			VisitExpression(expr.deref_expr);
		}
		
		private void VisitCommonMethodCall(common_method_call expr)
		{
			VisitExpression(expr.obj);
			WriteMethodReference(expr.function_node);
            //ssyy
            if (expr.last_result_function_call)
            {
                bw.Write((byte)1);
            }
            else
            {
                bw.Write((byte)0);
            }
            //\ssyy
            bw.Write(expr.virtual_call);
            bw.Write(expr.parameters.Count);
			foreach (expression_node e in expr.parameters)
				VisitExpression(e);
		}
		
		private void VisitCommonStaticMethodCall(common_static_method_call expr)
		{
			WriteMethodReference(expr.function_node);
            //ssyy
            if (expr.last_result_function_call)
            {
                bw.Write((byte)1);
            }
            else
            {
                bw.Write((byte)0);
            }
            //\ssyy
            bw.Write(expr.parameters.Count);
			foreach (expression_node e in expr.parameters)
				VisitExpression(e);
		}
		
		private void VisitCompiledFunctionCall(compiled_function_call expr)
		{
			VisitExpression(expr.obj);
			WriteCompiledMethod(expr.function_node);
            //ssyy
            if (expr.last_result_function_call)
            {
                bw.Write((byte)1);
            }
            else
            {
                bw.Write((byte)0);
            }
            //\ssyy
            bw.Write(expr.virtual_call);
            bw.Write(expr.parameters.Count);
			foreach (expression_node e in expr.parameters)
				VisitExpression(e);
		}
		
		private void VisitGetAddrNode(get_addr_node expr)
		{
			VisitExpression(expr.addr_of);
		}
		
		private void VisitCommonConstructorCall(common_constructor_call expr)
		{
			WriteMethodReference(expr.function_node);
            //ssyy добавил
            if (expr._new_obj_awaited)
            {
                bw.Write((byte)1);
            }
            else
            {
                bw.Write((byte)0);
            }
            //\ssyy
            bw.Write(expr.parameters.Count);
			foreach (expression_node e in expr.parameters)
				VisitExpression(e);
		}
		
		private void VisitCompiledConstructorCall(compiled_constructor_call expr)
		{
			WriteCompiledConstructor(expr.function_node);
            //ssyy добавил
            if (expr._new_obj_awaited)
            {
                bw.Write((byte)1);
            }
            else
            {
                bw.Write((byte)0);
            }
            //\ssyy
            bw.Write(expr.parameters.Count);
			foreach (expression_node e in expr.parameters)
				VisitExpression(e);
		}
		
		private void VisitCompiledVariableReference(compiled_variable_reference expr)
		{
			VisitExpression(expr.obj);
			WriteCompiledVariable(expr.var);
		}
		
		private void VisitStaticCompiledVariableReference(static_compiled_variable_reference expr)
		{
			WriteCompiledVariable(expr.var);
		}
		
		private void VisitStaticClassFieldReference(static_class_field_reference expr)
		{
			WriteFieldReference(expr.static_field);
		}
		
		private void VisitThisNode(this_node expr)
		{
            WriteTypeReference(expr.type);
		}
		
		private void VisitSimpleArrayIndexing(simple_array_indexing expr)
		{
            VisitExpression(expr.simple_arr_expr);
            VisitExpression(expr.ind_expr);
            if (CanWriteObject(expr.expr_indices))
            {
            	bw.Write(expr.expr_indices.Length);
            	for (int i=0; i<expr.expr_indices.Length; i++)
            		VisitExpression(expr.expr_indices[i]);
            }
            WriteTypeReference(expr.type);
		}
	}

}

