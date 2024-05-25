// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TreeConverter;
using PascalABCCompiler.TreeConverter;
using PascalABCCompiler.TreeRealization;

namespace PascalABCCompiler.PCU
{
    public enum PCUReaderWriterState
    {
        BeginReadTree, EndReadTree, BeginSaveTree, EndSaveTree, ErrorSaveTree
    }

    public class InvalidPCUFile: PascalABCCompiler.Errors.LocatedError
    {
        internal string UnitName;
        public InvalidPCUFile(string UnitName)
        {
            this.UnitName = UnitName;
        }
        public override string ToString()
        {
            return "Invalid unit " + UnitName;
        }
    }

    public class PCUReader : BasePCUReader {
		internal Compiler comp;
		private BinaryReader br;
		//private FileStream fs;
        private MemoryStream ms;
        public string FileName;
		private PCUFile pcu_file = new PCUFile();
		private CompilationUnit unit;
        private common_unit_node cun;
		private document cur_doc;
		private string unit_name;
		private string dir;
        private int start_pos;//смещение относительно начала PCU-файла, по которому находятся описание всех сущностей модуля
		private int ext_pos;//смещение относительно начала PCU-файла, по которому находится список импорт. сущностей
        private Dictionary<int, definition_node> members = new Dictionary<int, definition_node>(64);//таблица<int,definition_node> для связывания смещений с сущностями
        private List<definition_node> impl_members = new List<definition_node>(64);//список, хранящий десериализованные имплементационные сущности
        private List<definition_node> int_members = new List<definition_node>(64);//список, хранящий десериализованные интерфейсные сущности
        private Dictionary<int, definition_node> ext_members = new Dictionary<int, definition_node>(64);//таблица<int,definition_node> для связывания смещений с импортируемыми сущностями
        private Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();
        private Dictionary<common_type_node, string[]> class_names = new Dictionary<common_type_node, string[]>();
        private Hashtable used_units = new Hashtable();
        public static Hashtable units = new Hashtable(StringComparer.OrdinalIgnoreCase);
        public static List<PCUReader> AllReaders = new List<PCUReader>();
        public bool need=false;
        private Hashtable already_compiled = new Hashtable();
        private bool readDebugInfo=true;
        private int InitializationMethodOffset = 0;
        private int FinalizationMethodOffset = 0;
        internal List<common_type_node> waited_types_to_restore_fields = new List<common_type_node>();
        //ivan
        private Dictionary<int, MemberInfo> dot_net_cache = new Dictionary<int, MemberInfo>();
        private Dictionary<int, Type> dot_net_type_cache = new Dictionary<int, Type>();
        private SortedDictionary<int, var_definition_node> interf_var_list = new SortedDictionary<int, var_definition_node>();
        private SortedDictionary<int, var_definition_node> impl_var_list = new SortedDictionary<int, var_definition_node>();

        private SortedDictionary<int, common_type_node> interf_type_list = new SortedDictionary<int, common_type_node>();
        private SortedDictionary<int, common_type_node> impl_type_list = new SortedDictionary<int, common_type_node>();
        private Dictionary<common_method_node, int> waited_method_codes = new Dictionary<common_method_node, int>();
        private bool waited_method_restoring = false;
        internal static Dictionary<definition_node, int> AllReadOrWritedDefinitionNodesOffsets = new Dictionary<definition_node, int>();
        internal void AddMember(definition_node dn, int offset)
        {
            members[offset] = dn;
            AddReadOrWritedDefinitionNode(dn, offset);
        }

        internal void RemoveMember(int offset, definition_node dn)
        {
            members.Remove(offset);
            AllReadOrWritedDefinitionNodesOffsets.Remove(dn);
        }

        internal void AddVarToOrderList(var_definition_node vdn, int ind)
        {
            if (!interf_var_list.ContainsKey(ind))
                interf_var_list.Add(ind, vdn);
            else
                interf_var_list[ind] = vdn;
        }

        internal void AddTypeToOrderList(common_type_node ctn, int ind)
        {
            interf_type_list.Add(ind, ctn);
        }

        internal void AddImplVarToOrderList(var_definition_node vdn, int ind)
        {
        	impl_var_list.Add(ind,vdn);
        }

        internal void AddImplTypeToOrderList(common_type_node ctn, int ind)
        {
            impl_type_list.Add(ind, ctn);
        }

        internal static void AddReadOrWritedDefinitionNode(definition_node dn, int offset)
        {
            if(!AllReadOrWritedDefinitionNodesOffsets.ContainsKey(dn))
                AllReadOrWritedDefinitionNodesOffsets.Add(dn, offset);
        }


        public delegate void ChangeStateDelegate(object Sender, PCUReaderWriterState State, object obj);

        public event ChangeStateDelegate ChangeState;


        /// <summary>
        /// Читает начало загаловка PCU файла
        /// </summary>
        /// <param name="fileName">
        /// Имя PCU файла
        /// </param>
        /// <returns>
        /// Возвращает состояние заголовка PCU файла
        /// </returns>
        public class PCUFileHeadState
        {
            public bool IsPCUFile = false;
            public bool SupportedVersion = false;
            public bool IncludetDebugInfo = false;
            public PCUFile FileHead=null;
        }

        public static PCUFileHeadState GetPCUFileHeadState(string fileName)
        {
            FileStream fs = File.OpenRead(fileName);
            BinaryReader br = new BinaryReader(fs);
            PCUFileHeadState State = new PCUFileHeadState();
            PCUFile pcuHead = new PCUFile();
            State.IsPCUFile=ReadPCUHead(pcuHead, br);
            br.Close();
            fs.Close();
            if (!State.IsPCUFile)
                return State;
            State.SupportedVersion = PCUFile.SupportedVersion == pcuHead.Version;
            State.IncludetDebugInfo= pcuHead.IncludeDebugInfo;
            State.FileHead = pcuHead;
            return State;
        }

        public PCUReader(Compiler comp, ChangeStateDelegate changeState)
		{
			this.comp = comp;
            AllReaders.Add(this);
            ChangeState += changeState;
		}
        public PCUReader(PCUReader Reader)
        {
            this.comp = Reader.comp;
            AllReaders.Add(this);
            ChangeState += Reader.ChangeState;
        }

		~PCUReader()
		{
			if (br != null)
            br.Close();
            if (ms != null)
			ms.Close();
		}

		public static void AddUsedMembersInAllUnits()
		{
			foreach (PCUReader pr in units.Values)
			{
                pr.AddMembersToNamespace();
			}

		}

        public void AddAlreadyCompiledUnit(string name)
        {
            already_compiled[name] = name;
        }

        public bool AlreadyCompiled(string name)
        {
            return already_compiled[name] != null;
        }

        public static void CloseUnits()
        {
            AllReadOrWritedDefinitionNodesOffsets.Clear();
            foreach (PCUReader pr in units.Values)
            {
                pr.CloseUnit();
            }
            units.Clear();
        }

        public void CloseUnit()
        {
            //SystemLibrary.SystemLibInitializer.RestoreStandardFunctions();
            AllReaders.Remove(this);
            if (br != null)
                br.Close();
            if (ms != null)
                ms.Close();
            br = null;
            ms = null;
        }

        public void OpenUnit()
        {
            ms = new MemoryStream(File.ReadAllBytes(FileName));
            br = new BinaryReader(ms);
            ReadPCUHeader();
        }

        public string GetFullUnitName(string UnitName)
        {

            var FullUnitName = comp.FindPCUFileName(UnitName, dir, out _);
            if (FullUnitName == null) throw new FileNotFound(UnitName, null);

            return FullUnitName;
        }

        private PCUReader GetPCUReaderForUnitId(int id)
        {
            if (id == -1) return this;
            string s = GetFullUnitName(pcu_file.incl_modules[id]);
            var pr = (PCUReader)units[s];
            if (pr == null)
            {
                pr = new PCUReader(this);
                pr.GetCompilationUnit(s, this.readDebugInfo);
            }
            return pr;
        }

        //десериализация модуля
        //читается только шапка PCU и заполняются имена сущностей модуля в таблицу символов
        public CompilationUnit GetCompilationUnit(string FileName, bool readDebugInfo)
		{
            try
            {
                // Compiler.CombinePath всегда должно быть применено к имени файла, перед тем как кидать его сюда
                if (!Path.IsPathRooted(FileName)) throw new InvalidOperationException();

                FileName = GetFullUnitName(FileName);
                dir = Path.GetDirectoryName(FileName);

                this.FileName = FileName;
                this.readDebugInfo = readDebugInfo;
                unit_name = System.IO.Path.GetFileNameWithoutExtension(FileName);
                PCUReader pr = (PCUReader)units[FileName];
                if (pr != null) return pr.unit;
                if (!File.Exists(FileName)) return null;
                //fs = new FileStream(file_name, FileMode.Open, FileAccess.Read);
                ms = new MemoryStream(File.ReadAllBytes(FileName));
                br = new BinaryReader(ms);
                ReadPCUHeader();

                units[FileName] = this;
                unit = new CompilationUnit();
                unit.UnitFileName = FileName;
                cun = new common_unit_node();
                cun.compiler_directives = pcu_file.compiler_directives;
                unit.SemanticTree = cun;
                if (NeedRecompiled())
                {
                    CloseUnit();
                    this.unit = null;
                    need = true;
                    return null; // return comp.RecompileUnit(file_name);
                }
                ChangeState(this, PCUReaderWriterState.BeginReadTree, unit);
                cun.scope = new WrappedUnitInterfaceScope(this);

                //TODO сохранить в PCU
                cun.scope.CaseSensitive = false;
                if (string.Compare(unit_name, StringConstants.pascalSystemUnitName, true)==0)
                	PascalABCCompiler.TreeConverter.syntax_tree_visitor.init_system_module(cun);
                //ssyy
                //Создаём область видимости для implementation - части
                cun.implementation_scope = new WrappedUnitImplementationScope(this, cun.scope);
                //\ssyy

                //TODO сохранить в PCU
                cun.implementation_scope.CaseSensitive = false;

                string SourceFileName = pcu_file.SourceFileName;
                if (Path.GetDirectoryName(SourceFileName) == "")
                {
                    SourceFileName = Path.Combine(Path.GetDirectoryName(FileName), SourceFileName);
                    if (!File.Exists(SourceFileName))
                        SourceFileName = Path.Combine(Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(FileName)),"LibSource"), pcu_file.SourceFileName);
                }
                cur_doc = new document(SourceFileName);

                AddNamespaces();
                AddInterfaceNames();
                //ssyy
                AddImplementationNames();
                //\ssyy

                //AddInitFinalMethods();
                //ProcessWaitedToRestoreFields();
                unit.State = UnitState.Compiled;
				//ssyy
                AddTypeSynonyms(pcu_file.interface_synonyms_offset, cun.scope);
                AddTypeSynonyms(pcu_file.implementation_synonyms_offset, cun.implementation_scope);
                //\ssyy
                for (int i = 0; i < pcu_file.names.Length; i++)
                {
                    if (pcu_file.names[i].always_restore)
                    {
                        cun.scope.Find(pcu_file.names[i].name);
                    }
                }
                for (int i = 0; i < pcu_file.implementation_names.Length; i++)
                {
                    if (pcu_file.implementation_names[i].always_restore)
                    {
                        cun.implementation_scope.Find(pcu_file.implementation_names[i].name);
                    }
                }
                ChangeState(this, PCUReaderWriterState.EndReadTree, unit);
                return unit;
            }
            catch (Exception)
            {
                CloseUnit();
                throw;

            }
		}

        public void AddInitFinalMethods()
        {
            AddInitializationMethod();
            AddFinalizationMethod();
        }

        //нужно ли перекомпилировать PCU
        //проходимся по подключ. модулям и проверяем, нужно ли их перекомпилировать
        //так рекурсивно, пока не пройдем все модули. Если хотя бы один из них изменился
        //то модуль нужно перекомпилировать
        public bool NeedRecompiled()
        {
            /*if (pcu_file.UseRtlDll != comp.CompilerOptions.UseDllForSystemUnits)
            {
                need = true;
                return need;
            }*/
            if (comp.NeedRecompiled(FileName, pcu_file.incl_modules, this))
            {
                //comp.RecompileList.Add(unit_name,unit_name);
                comp.RecompileList[FileName] = FileName;
                need = true;
                return need;
            }

            for (int i = 0; i < pcu_file.incl_modules.Length; i++)
            {
                //if (pcu_file.incl_modules[i].Contains("$"))
                //	continue;
                var used_unit_fname = comp.GetUnitFileName(Path.GetFileNameWithoutExtension(pcu_file.incl_modules[i]), pcu_file.incl_modules[i], dir, new SyntaxTree.SourceContext(0,0,0,0, FileName));
                if (Path.GetExtension(used_unit_fname) != comp.CompilerOptions.CompiledUnitExtension) return true;

                PCUReader pr = (PCUReader)units[used_unit_fname];
                if (pr == null)
                    pr = new PCUReader(this);
                pr.AddAlreadyCompiledUnit(FileName);
                if (used_units[used_unit_fname] == null)
                {
                    used_units[used_unit_fname] = used_units;
                    if (already_compiled[used_unit_fname] == null)
                    {
                        var sub_u = pr.GetCompilationUnit(used_unit_fname, this.readDebugInfo);
                        if (sub_u == null) return true;
                        this.unit.InterfaceUsedDirectUnits.Add(sub_u.SemanticTree, sub_u);
                        this.unit.InterfaceUsedUnits.AddElement(sub_u.SemanticTree, pcu_file.incl_modules[i]);
                    }
                }
                if (need == false) need = pr.need;
            }
            return need;
        }

        //процедура добавления десериализ. сущностей в common_namespace_node модуля
        public void AddMembersToNamespace()
		{
			foreach (definition_node dn in int_members)
			{
				common_namespace_node cnn = cun.namespaces[0];
                cnn.from_pcu = true;
				switch (dn.semantic_node_type) {
//					case semantic_node_type.namespace_variable :
//						cnn.variables.AddElement((namespace_variable)dn); break;
//					case semantic_node_type.common_type_node :
//						cnn.types.AddElement((common_type_node)dn); break;
					case semantic_node_type.common_namespace_function_node :
                        if (!((dn as common_namespace_function_node).function_code is wrapped_function_body))
						    cnn.functions.AddElement((common_namespace_function_node)dn);
                        break;
					case semantic_node_type.namespace_constant_definition :
						cnn.constants.AddElement((namespace_constant_definition)dn); break;
                    case semantic_node_type.common_namespace_event:
                        cnn.events.AddElement((common_namespace_event)dn); break;
                    //ssyy
                    case semantic_node_type.template_type:
                        cnn.templates.AddElement((template_class)dn); break;
                    //\ssyy
				}
			}
            foreach (common_type_node ctn in interf_type_list.Values)
                cun.namespaces[0].types.AddElement(ctn);
			foreach (namespace_variable nv in interf_var_list.Values)
                cun.namespaces[0].variables.AddElement(nv);
			foreach (definition_node dn in impl_members)
			{
				common_namespace_node cnn = cun.namespaces[1];
                cnn.from_pcu = true;
				switch (dn.semantic_node_type) {
//					case semantic_node_type.namespace_variable :
//						cnn.variables.AddElement((namespace_variable)dn); break;
//					case semantic_node_type.common_type_node :
//						cnn.types.AddElement((common_type_node)dn); break;
					case semantic_node_type.common_namespace_function_node :
                        if (!((dn as common_namespace_function_node).function_code is wrapped_function_body))
						    cnn.functions.AddElement((common_namespace_function_node)dn); break;
                    case semantic_node_type.namespace_constant_definition:
						cnn.constants.AddElement((namespace_constant_definition)dn); break;
                    case semantic_node_type.common_namespace_event:
                        cnn.events.AddElement((common_namespace_event)dn); break;
                    //ssyy
                    case semantic_node_type.template_type:
                        cnn.templates.AddElement((template_class)dn); break;
                    //\ssyy
                }
			}
            foreach (common_type_node ctn in impl_type_list.Values)
                cun.namespaces[1].types.AddElement(ctn);
			foreach (namespace_variable nv in impl_var_list.Values)
                	cun.namespaces[1].variables.AddElement(nv);
            if (cun != null)
            cun.used_namespaces.AddRange(pcu_file.used_namespaces);
		}

        private void AddNamespaces()
		{
            cun.unit_name=br.ReadString();

            common_namespace_node cnn = new common_namespace_node(null, cun, cun.unit_name, cun.scope, ReadDebugInfo());
			cun.namespaces.AddElement(cnn);

            cun.add_unit_name_to_namespace();

            cnn = new common_namespace_node(cnn, cun, br.ReadString(), cun.implementation_scope, ReadDebugInfo());
			cun.namespaces.AddElement(cnn);
            var attributes = GetAttributes();
            cun.attributes.AddRange(attributes);
            cun.namespaces[0].attributes.AddRange(attributes);
		}

        private void AddInitializationMethod()
        {
            if(InitializationMethodOffset!=0)
                cun.main_function = GetNamespaceFunction(InitializationMethodOffset);
        }

        private void AddFinalizationMethod()
        {
            if (FinalizationMethodOffset != 0)
                cun.finalization_method = GetNamespaceFunction(FinalizationMethodOffset);
        }

        private common_namespace_function_node GetNamespaceFunctionWithImplementation(int offset)
        {
            int tmp = (int)br.BaseStream.Position;
            string name = br.ReadString();
            br.ReadInt32();
            location loc = ReadDebugInfo();
            common_namespace_function_node cnfn = new common_namespace_function_node(name, loc, cun.namespaces[0], null);
            if (br.ReadByte() == 1)
                cnfn.function_code = CreateStatement();
            return cnfn;
        }

        //процедура добавления имен в пространство имен модуля
		private void AddInterfaceNames()
		{
            AddNames(pcu_file.names, cun.scope);
		}

        //(ssyy) процедура добавления имен в implementation-пространство имен модуля
        private void AddImplementationNames()
        {
            AddNames(pcu_file.implementation_names, cun.implementation_scope);
        }

        private void AddNames(NameRef[] names, SymbolTable.Scope Scope)
        {
            for (int i = 0; i < names.Length; i++)
            {
                SymbolInfo si = new SymbolInfo();

                si.symbol_kind = names[i].symbol_kind;
                wrapped_definition_node wdn = new wrapped_definition_node(names[i].offset, this);
                si.sym_info = wdn;
                if (names[i].special_scope == 0)
                {
                    //PCUReturner.AddPCUReader((wrapped_definition_node)si.sym_info, this);
                    //si.access_level = access_level.al_public;
                    List<SymbolInfo> si2 = (cun.scope as WrappedUnitInterfaceScope).FindWithoutCreation(names[i].name);
                    //si.Add(si2);
                    Scope.AddSymbol(names[i].name, si);
                }
                else
                {

                    type_node tn = GetSpecialTypeReference(names[i].offset);
                    if (tn is compiled_type_node)
                    {
                        compiled_type_node ctn = tn as compiled_type_node;
                        if (ctn.scope == null)
                            ctn.init_scope();
                        ctn.scope.AddSymbol(names[i].name, si);
                        if (ctn.original_generic != null && ctn.original_generic.Scope != null)
                            ctn.original_generic.Scope.AddSymbol(names[i].name, si);
                    }
                    else if (tn is generic_instance_type_node)
                        tn.Scope.AddSymbol(names[i].name, si);
                    else if (tn is common_type_node)
                    {
                        (tn as common_type_node).scope.AddSymbol(names[i].name, si);
                        if (tn.IsDelegate)
                            SystemLibrary.SystemLibrary.system_delegate_type.Scope.AddSymbol(names[i].name, si);
                    }

                    else
                        throw new NotSupportedException();
                }
            }
        }

        private void InvalidUnitDetected()
        {
            CloseUnit();
            throw new InvalidPCUFile(unit_name);
        }

        private static bool ReadPCUHead(PCUFile pcu_file, BinaryReader br)
        {
            char[] Header = br.ReadChars(PCUFile.Header.Length);
            for (int i = 0; i < PCUFile.Header.Length; i++)
                if (Header[i] != PCUFile.Header[i])
                    return false;
            pcu_file.Version = br.ReadInt16();
            pcu_file.Revision = br.ReadInt32();
            pcu_file.CRC = br.ReadInt64();
            pcu_file.UseRtlDll = br.ReadBoolean();
            pcu_file.IncludeDebugInfo = br.ReadBoolean();
            return true;
        }

        //чтение заголовка PCU
		private void ReadPCUHeader()
		{
            if (!ReadPCUHead(pcu_file, br) || PCUFile.SupportedVersion != pcu_file.Version || PCUFile.SupportedRevision != pcu_file.Revision)
                InvalidUnitDetected();

            if(pcu_file.IncludeDebugInfo)
                pcu_file.SourceFileName = br.ReadString();
            else
                pcu_file.SourceFileName = Path.GetFileNameWithoutExtension(FileName);
            if (Path.GetDirectoryName(pcu_file.SourceFileName) == "")
                cur_doc = new document(Path.Combine(Path.GetDirectoryName(FileName), pcu_file.SourceFileName));
            else
                cur_doc = new document(pcu_file.SourceFileName);

            int num_names = br.ReadInt32();
			pcu_file.names = new NameRef[num_names];
			for (int i=0; i<num_names; i++)
			{
				pcu_file.names[i] = new NameRef(br.ReadString(),i);
				pcu_file.names[i].offset = br.ReadInt32();
                pcu_file.names[i].symbol_kind = (symbol_kind)br.ReadByte();
                pcu_file.names[i].special_scope = br.ReadByte();
                pcu_file.names[i].always_restore = br.ReadBoolean();
            }
            //ssyy
            num_names = br.ReadInt32();
            pcu_file.implementation_names = new NameRef[num_names];
            for (int i = 0; i < num_names; i++)
            {
                pcu_file.implementation_names[i] = new NameRef(br.ReadString(), i);
                pcu_file.implementation_names[i].offset = br.ReadInt32();
                pcu_file.implementation_names[i].symbol_kind = (symbol_kind)br.ReadByte();
                pcu_file.implementation_names[i].special_scope = br.ReadByte();
                pcu_file.implementation_names[i].always_restore = br.ReadBoolean();
            }
            //\ssyy
			int num_incl = br.ReadInt32();
			pcu_file.incl_modules = new string[num_incl];
			for (int i=0; i<num_incl; i++)
			{
				pcu_file.incl_modules[i] = br.ReadString();
			}
			int num_used_ns = br.ReadInt32();
			pcu_file.used_namespaces = new string[num_used_ns];
			for (int i=0; i<num_used_ns; i++)
			{
				pcu_file.used_namespaces[i] = br.ReadString();
			}
            //ssyy
            pcu_file.interface_uses_count = br.ReadInt32();
            //\ssyy
			int num_ref_ass = br.ReadInt32();
			pcu_file.ref_assemblies = new string[num_ref_ass];
			for (int i=0; i<num_ref_ass; i++)
			{
				pcu_file.ref_assemblies[i] = br.ReadString();

			}

            int num_directives = br.ReadInt32();
            pcu_file.compiler_directives = new List<compiler_directive>();
            for (int i = 0; i < num_directives; i++)
            {
                pcu_file.compiler_directives.Add(new compiler_directive(br.ReadString(),br.ReadString(),ReadDebugInfo(), this.FileName));
            }
            ReadAllAssemblies();
			int num_imp_entity = br.ReadInt32();
			ext_pos = (int)br.BaseStream.Position;
			pcu_file.imp_entitles = new ImportedEntity[num_imp_entity];
			br.BaseStream.Seek(num_imp_entity*ImportedEntity.GetClassSize(),SeekOrigin.Current);

            //ssyy
            /*int num_int_syn = br.ReadInt32();
            pcu_file.interface_type_synonyms = new List<type_synonym>(num_int_syn);
            for (int i = 0; i < num_int_syn; i++)
            {
                pcu_file.interface_type_synonyms.Add(CreateTypeSynonym());
            }
            int num_impl_syn = br.ReadInt32();
            pcu_file.implementation_type_synonyms = new List<type_synonym>(num_impl_syn);
            for (int i = 0; i < num_impl_syn; i++)
            {
                pcu_file.implementation_type_synonyms.Add(CreateTypeSynonym());
            }*/
            //\ssyy

            pcu_file.interface_synonyms_offset = br.ReadInt32();
            pcu_file.implementation_synonyms_offset = br.ReadInt32();

            InitializationMethodOffset = br.ReadInt32();
            FinalizationMethodOffset = br.ReadInt32();
            //ivan
            int num_net_entities = br.ReadInt32();
            pcu_file.dotnet_names = new DotNetNameRef[num_net_entities];
            for (int i = 0; i < num_net_entities; i++)
            {
                DotNetNameRef dnnr = new DotNetNameRef();
                dnnr.kind = (DotNetKind)br.ReadByte();
                dnnr.name = br.ReadString();
                dnnr.addit = new DotAdditInfo[br.ReadInt32()];
                for (int j = 0; j < dnnr.addit.Length; j++)
                {
                    dnnr.addit[j] = new DotAdditInfo();
                    dnnr.addit[j].offset = br.ReadInt32();
                }
                pcu_file.dotnet_names[i] = dnnr;
            }
			start_pos = (int)br.BaseStream.Position;
		}

		private void ReadAllAssemblies()
		{
            for (int i = 0; i < pcu_file.ref_assemblies.Length; i++)
            {
                string s = pcu_file.ref_assemblies[i];
                string tmp = s.Substring(0, s.IndexOf(','));
                //if (tmp != "mscorlib")
                //{
                string name_with_path = Compiler.GetReferenceFileName(tmp + ".dll", Path.GetDirectoryName(this.FileName));
                //Assembly a = Assembly.LoadFrom(name_with_path);
                /*if (pcu_file.compiler_directives != null)
                foreach (compiler_directive cd in pcu_file.compiler_directives)
                {
                    if (cd.name == "reference" && cd.directive != null && cd.directive.IndexOf("\\") != -1 && cd.directive.IndexOf(tmp + ".dll") != -1)
                    {
                        name_with_path = Compiler.GetReferenceFileName(cd.directive);
                        if (name_with_path == null)
                            throw new AssemblyNotFound(unit_name, cd.directive, null);
                    }
                }*/
                Assembly a = NetHelper.NetHelper.LoadAssembly(name_with_path);
                NetHelper.NetHelper.init_namespaces(a);
                //}
                //else
                //  a = Assembly.Load(s);
                if (!assemblies.ContainsKey(s))
                    assemblies[s] = a;
            }
		}

        public void AddWaitedMethodCode(common_method_node cmn, int offset)
        {
            if (!waited_method_codes.ContainsKey(cmn))
                waited_method_codes.Add(cmn, offset);
        }

        public void RestoreWaitedMethodCodes()
        {
            waited_method_restoring = true;
            foreach (common_method_node cmn in waited_method_codes.Keys)
                cmn.function_code = GetCode(waited_method_codes[cmn]);
            waited_method_restoring = false;
        }

        //получение кода метода
        public statement_node GetCode(int offset)
		{
			int tmp = (int)br.BaseStream.Position;
			br.BaseStream.Seek(start_pos+offset,SeekOrigin.Begin);
			statement_node sn = CreateStatement();
			br.BaseStream.Seek(tmp,SeekOrigin.Begin);
			return sn;
		}

        public statement_node GetCodeWithOverridedMethod(common_method_node meth, int offset)
		{
			int tmp = (int)br.BaseStream.Position;
			br.BaseStream.Seek(start_pos+offset,SeekOrigin.Begin);
			statement_node sn = CreateStatement();
            meth.overrided_method = GetMethodReference();
			br.BaseStream.Seek(tmp,SeekOrigin.Begin);
			return sn;
		}

        //перейти на указанную позицию в списке импорт. сущ-тей
		private void SeekInExternal(int pos)
		{
			br.BaseStream.Seek(ext_pos+pos,SeekOrigin.Begin);
		}

        //получаем имя сущности
		private string GetString(int index)
		{
			return pcu_file.names[index].name;
		}

        //получаем имя сущности-члена класса
        private string GetStringInClass(common_type_node type, int name_off)
        {
            return class_names[type][name_off];
        }

        class TypeSpec
        {
            public Type t;
            public string name;
        }

        private Type FindTypeByHandle(int off)
        {
            Type t = null;
            if (!dot_net_type_cache.TryGetValue(off, out t))
            {
                t = NetHelper.NetHelper.FindTypeOrCreate(pcu_file.dotnet_names[off].name);
                dot_net_type_cache[off] = t;
            }
            Type[] template_types = new Type[pcu_file.dotnet_names[off].addit.Length];
            for (int i = 0; i < template_types.Length; i++)
            {
                Type tt = FindTypeByHandle(pcu_file.dotnet_names[off].addit[i].offset);
                if (tt == null)
                    return t;
                template_types[i] = tt;
            }
            if (template_types.Length > 0 && t.IsGenericTypeDefinition)
                return t.MakeGenericType(template_types);
            return t;
        }

        private TypeSpec FindSpecTypeByHandle(int off)
        {
            TypeSpec ts = new TypeSpec();
            Type t = null;
            string type_name = pcu_file.dotnet_names[off].name;

            //if (!type_name.EndsWith("]"))
                t = NetHelper.NetHelper.FindTypeOrCreate(type_name);
            if (t == null || type_name.IndexOf('.') == -1)
            {
                ts.name = type_name.Remove(0, type_name.LastIndexOf('.') + 1);
                return ts;
            }
            Type[] template_types = new Type[pcu_file.dotnet_names[off].addit.Length];
            bool pure_template = true;
            for (int i = 0; i < template_types.Length; i++)
            {
                Type tt = FindTypeByHandle(pcu_file.dotnet_names[off].addit[i].offset);
                if (tt != null && pcu_file.dotnet_names[pcu_file.dotnet_names[off].addit[i].offset].name.IndexOf(".") == -1)
                    tt = null;//generic parameter
                if (tt == null)
                {
                    tt = t.GetGenericArguments()[i];
                    if (!tt.IsGenericParameter)
                        pure_template = false;
                    /*ts.name = type_name.Remove(0, type_name.LastIndexOf('.') + 1);
                    return ts;*/
                }
                else
                    pure_template = false;
                template_types[i] = tt;
            }
            if (template_types.Length > 0)
            {
                if (!pure_template)
                {
                    if (t.IsGenericTypeDefinition)
                        t = t.MakeGenericType(template_types);
                }
                else
                {
                    ts.name = type_name.Remove(0, type_name.LastIndexOf('.') + 1);
                    return ts;
                }
            }
            ts.t = t;
            return ts;
        }

        private bool compareTypesDeeply(Type t1, Type t2)
        {
            if (t1 == t2)
                return true;
            if (t1.IsGenericType && t2.IsGenericType)
            {
                if (t1.IsGenericParameter && t2.IsGenericParameter)
                    return true;
                if (t1.IsGenericParameter && !t2.IsGenericParameter || !t1.IsGenericParameter && t2.IsGenericParameter)
                    return false;
                int gen_len = t1.GetGenericArguments().Length;
                if (gen_len != t2.GetGenericArguments().Length)
                {
                    return false;
                }
                else
                {
                    for (int k = 0; k < t1.GetGenericArguments().Length; k++)
                    {
                        if (!compareTypesDeeply(t1.GetGenericArguments()[k], t2.GetGenericArguments()[k]))
                            return false;
                    }
                    return true;
                }
            }
            if (t1.IsGenericParameter && t2.IsGenericParameter)
                return true;
            if (t1.FullName == t2.FullName && t1.AssemblyQualifiedName == t2.AssemblyQualifiedName)
                return true;
            return false;
        }

        private MethodInfo ChooseMethod(Type t, IList<MemberInfo> mis, TypeSpec[] param_types)
        {
            MethodInfo res_mi = null;
            for (int i = 0; i < mis.Count; i++)
            {
                MethodInfo mi = mis[i] as MethodInfo;
                if (mi == null) continue;
                ParameterInfo[] prms = mi.GetParameters();
                bool eq = true;
                if (prms.Length != param_types.Length) continue;
                for (int j=0; j<prms.Length; j++)
                    if (param_types[j].t != null)
                    {
                        if (!compareTypesDeeply(prms[j].ParameterType, param_types[j].t))
                        {
                            eq = false; break;
                        }
                    }
                    else
                    {
                        if (prms[j].ParameterType.Name != param_types[j].name)
                        {
                            eq = false; break;
                        }
                        if (prms[j].ParameterType.IsGenericType && !prms[j].ParameterType.GetGenericArguments()[0].IsGenericParameter)
                        {
                            eq = false; break;
                        }
                    }

                if (eq && mi.DeclaringType == t) res_mi = mi;
            }
            return res_mi;
        }

        private ConstructorInfo ChooseConstructor(Type t, TypeSpec[] param_types)
        {
            //ConstructorInfo[] mis = t.GetConstructors();
            System.Reflection.ConstructorInfo[] mis=t.GetConstructors();
			System.Reflection.ConstructorInfo[] prot_consttr_arr = t.GetConstructors(BindingFlags.FlattenHierarchy|BindingFlags.Instance|BindingFlags.Public | BindingFlags.NonPublic);
			List<System.Reflection.ConstructorInfo> cnstrs = new List<System.Reflection.ConstructorInfo>();
			cnstrs.AddRange(mis);
			for (int i=0; i<prot_consttr_arr.Length; i++)
				if (!cnstrs.Contains(prot_consttr_arr[i]))
				cnstrs.Add(prot_consttr_arr[i]);
			mis = cnstrs.ToArray();

            ConstructorInfo res_mi = null;
            for (int i = 0; i < mis.Length; i++)
            {
                ConstructorInfo mi = mis[i] as ConstructorInfo;
                //if (mi == null) continue;
                ParameterInfo[] prms = mi.GetParameters();

                bool eq = true;
                if (prms.Length != param_types.Length) continue;
                for (int j = 0; j < prms.Length; j++)
                    if (param_types[j].t != null)
                    {
                        if (prms[j].ParameterType != param_types[j].t)
                        {
                            eq = false; break;
                        }
                    }
                    else
                        if (prms[j].ParameterType.Name != param_types[j].name)
                        {
                            eq = false; break;
                        }
                if (eq) res_mi = mi;
            }
            return res_mi;
        }

        private MethodInfo FindMethodByHandle(Type t, int off)
        {
            //MemberInfo[] mis = NetHelper.NetHelper.GetMembers(t,pcu_file.dotnet_names[off].name);
            MemberInfo mbi = null;
            if (dot_net_cache.TryGetValue(off, out mbi))
                return mbi as MethodInfo;
            TypeSpec[] param_types = new TypeSpec[pcu_file.dotnet_names[off].addit.Length];
            for (int i = 0; i < param_types.Length; i++)
            {
                param_types[i] = FindSpecTypeByHandle(pcu_file.dotnet_names[off].addit[i].offset);
            }
            MethodInfo mi = null;
            if (pcu_file.dotnet_names[off].name != "op_Explicit")
            {
                //mi = t.GetMethod(pcu_file.dotnet_names[off].name, param_types);
                List<MemberInfo> mis = NetHelper.NetHelper.GetMembers(t, pcu_file.dotnet_names[off].name);
                mi = ChooseMethod(t, mis, param_types);
            }
            else
            {
                MethodInfo[] mis = t.GetMethods();
                foreach (MethodInfo mi2 in mis)
                    if (mi2.Name == "op_Explicit" && mi2.GetParameters()[0].ParameterType == param_types[0].t && mi2.ReturnType == param_types[1].t) mi = mi2;
                if (mi == null)
                {
                    List<MemberInfo> mis2 = NetHelper.NetHelper.GetMembers(t, pcu_file.dotnet_names[off].name);
                    mi = ChooseMethod(t, mis2, param_types);

                    //mi = t.GetMethod(pcu_file.dotnet_names[off].name, new Type[1] { param_types[0].t });
                }
            }
            dot_net_cache[off] = mi;
            return mi;
        }

        private FieldInfo FindFieldByHandle(Type t, int off)
        {
            //MemberInfo[] mis = NetHelper.NetHelper.GetMembers(t,pcu_file.dotnet_names[off].name);
            MemberInfo mi = null;
            if (dot_net_cache.TryGetValue(off, out mi))
                return mi as FieldInfo;
            //FieldInfo mi = t.GetField(pcu_file.dotnet_names[off].name);
            List<MemberInfo> mis = NetHelper.NetHelper.GetMembers(t, pcu_file.dotnet_names[off].name);
            FieldInfo fi = null;
            foreach (MemberInfo mi2 in mis)
                if (mi2 is FieldInfo)
                {
                    fi = (FieldInfo)mi2;
                    break;
                }
            dot_net_cache[off] = fi;
            return fi;
        }

        private ConstructorInfo FindConstructorByHandle(Type t, int off)
        {
            //MemberInfo[] mis = NetHelper.NetHelper.GetMembers(t,pcu_file.dotnet_names[off].name);
            MemberInfo mi = null;
            if (dot_net_cache.TryGetValue(off, out mi))
                return mi as ConstructorInfo;
            TypeSpec[] param_types = new TypeSpec[pcu_file.dotnet_names[off].addit.Length];
            for (int i = 0; i < param_types.Length; i++)
            {
                param_types[i] = FindSpecTypeByHandle(pcu_file.dotnet_names[off].addit[i].offset);
            }
            //ConstructorInfo mi = t.GetConstructor(param_types);
            //MemberInfo[] mis = NetHelper.NetHelper.GetMembers(t, pcu_file.dotnet_names[off].name);
            ConstructorInfo ci = ChooseConstructor(t, param_types);
            dot_net_cache[off] = ci;
            return ci;
        }

        private PropertyInfo FindPropertyByHandle(Type t, int off)
        {
            //MemberInfo[] mis = NetHelper.NetHelper.GetMembers(t,pcu_file.dotnet_names[off].name);
            MemberInfo mi = null;
            if (dot_net_cache.TryGetValue(off, out mi))
                return mi as PropertyInfo;
            //FieldInfo mi = t.GetField(pcu_file.dotnet_names[off].name);
            List<MemberInfo> mis = NetHelper.NetHelper.GetMembers(t, pcu_file.dotnet_names[off].name);
            PropertyInfo pi = null;
            foreach (MemberInfo mi2 in mis)
                if (mi2 is PropertyInfo)
                {
                    pi = (PropertyInfo)mi2;
                    break;
                }
            dot_net_cache[off] = pi;
            return pi;
        }

        private EventInfo FindEventByHandle(Type t, int off)
        {
            MemberInfo mi = null;
            if (dot_net_cache.TryGetValue(off, out mi))
                return mi as EventInfo;
            //FieldInfo mi = t.GetField(pcu_file.dotnet_names[off].name);
            List<MemberInfo> mis = NetHelper.NetHelper.GetMembers(t, pcu_file.dotnet_names[off].name);
            EventInfo ei = (EventInfo)mis[0];
            dot_net_cache[off] = ei;
            return ei;
        }

        //чтение отлад. инф.
		private location ReadDebugInfo()
		{
            if (pcu_file.IncludeDebugInfo)
            {
                if (readDebugInfo)
                {
                    location loc = new location(br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), cur_doc);
                    if (loc.begin_column_num == -1)
                        return null;
                    else
                        return loc;
                }
                else
                {
                    br.BaseStream.Seek(sizeof(Int32) * 4, SeekOrigin.Current);
                    return null;
                }
            }
            return null;
		}

        //получение импорт. типа
		private type_node ReadCommonExtType()
        {
            var pr = GetPCUReaderForUnitId(br.ReadInt32());
            return pr.GetTypeReference(br.ReadInt32());
		}

        //(ssyy) Получение шаблонного класса
        private template_class ReadTemplateExtClass()
        {
            var pr = GetPCUReaderForUnitId(br.ReadInt32());
            return pr.GetTemplateClass(br.ReadInt32());
        }

        //получение откомпил. типа
		private compiled_type_node ReadNetExtType()
		{
			int pos = br.ReadInt32();
			string s = pcu_file.ref_assemblies[pos];
            Assembly a = null;
			if (!assemblies.TryGetValue(s, out a))
			{
                string tmp = s.Substring(0, s.IndexOf(','));
                //if (tmp != "mscorlib")
                //{
                string name_with_path = Compiler.GetReferenceFileName(tmp + ".dll");
                    //a = Assembly.LoadFrom(name_with_path);
                    a = NetHelper.NetHelper.LoadAssembly(name_with_path);
                    NetHelper.NetHelper.init_namespaces(a);
                //}
                //else
                  //  a = Assembly.Load(s);
                assemblies[s] = a;
			}
			//Type t = NetHelper.NetHelper.FindTypeByHandle(a,br.ReadInt32());//находим его по токену
            Type t = FindTypeByHandle(br.ReadInt32());
            compiled_type_node ctn = compiled_type_node.get_type_node(t);

            //Исправление ошибки 0000186
            if (ctn.scope == null)
            // это условие вроде нужно чтоб не персоздавать стандартные сопы
                ctn.scope = new NetHelper.NetTypeScope(t, SystemLibrary.SystemLibrary.symtab);
            return ctn;

		}

        //получение откомпил. типа
		private compiled_type_node GetNetExtType(int offset)
		{
            definition_node dn = null;
            if (ext_members.TryGetValue(offset, out dn))
                return dn as compiled_type_node;
			compiled_type_node ctn = null;
			int tmp = (int)br.BaseStream.Position;
			br.BaseStream.Seek(ext_pos+offset,SeekOrigin.Begin);
			br.ReadByte();
			int pos = br.ReadInt32();
			string s = pcu_file.ref_assemblies[pos];
            Assembly a = null;
			if (!assemblies.TryGetValue(s, out a))
			{
                string str_tmp = s.Substring(0, s.IndexOf(','));
                //if (str_tmp != "mscorlib")
                //{
                string name_with_path = Compiler.GetReferenceFileName(str_tmp + ".dll");
                    //a = Assembly.LoadFrom(name_with_path);
                    a = NetHelper.NetHelper.LoadAssembly(name_with_path);
                //}
                //else a = Assembly.Load(s);
                NetHelper.NetHelper.init_namespaces(a);
                assemblies[s] = a;
			}
			Type t = FindTypeByHandle(br.ReadInt32());
			ctn = compiled_type_node.get_type_node(t);
			if (ctn.scope == null)
            ctn.scope = new NetHelper.NetTypeScope(t, SystemLibrary.SystemLibrary.symtab);
			ext_members[offset] = ctn;
			br.BaseStream.Seek(tmp,SeekOrigin.Begin);
			return ctn;
		}

        //получение импортируемой функции
        private common_namespace_function_node ReadCommonExtNamespaceFunc()
		{
			br.ReadByte();
            var pr = GetPCUReaderForUnitId(br.ReadInt32());
            return pr.GetNamespaceFunction(br.ReadInt32());
		}

		private namespace_variable ReadExtNamespaceVariable()
		{
			br.ReadByte();
            var pr = GetPCUReaderForUnitId(br.ReadInt32());
            return pr.GetNamespaceVariable(br.ReadInt32());
		}

		private namespace_constant_definition ReadExtNamespaceConstant()
		{
			br.ReadByte();
            var pr = GetPCUReaderForUnitId(br.ReadInt32());
            return pr.GetConstantDefinition(br.ReadInt32());
		}

        private common_namespace_event ReadCommonNamespaceExtEvent()
        {
            br.ReadByte();
            int offset = br.ReadInt32();
            int tmp = (int)br.BaseStream.Position;
            br.BaseStream.Seek(ext_pos + offset, SeekOrigin.Begin);
            br.ReadByte();//DS Changed
            var pr = GetPCUReaderForUnitId(br.ReadInt32());
            br.BaseStream.Seek(tmp, SeekOrigin.Begin);
            return pr.GetNamespaceEventNode(br.ReadInt32());
        }

        private common_event ReadCommonExtEvent()
        {
            br.ReadByte();
            int offset = br.ReadInt32();
            int tmp = (int)br.BaseStream.Position;
            br.BaseStream.Seek(ext_pos + offset, SeekOrigin.Begin);
            br.ReadByte();//DS Changed
            var pr = GetPCUReaderForUnitId(br.ReadInt32());
            br.BaseStream.Seek(tmp, SeekOrigin.Begin);
            return pr.GetEventNode(br.ReadInt32());
        }

        private class_field ReadCommonExtField()
        {
            br.ReadByte();
            int offset = br.ReadInt32();
            int tmp = (int)br.BaseStream.Position;
            br.BaseStream.Seek(ext_pos + offset, SeekOrigin.Begin);
            br.ReadByte();//DS Changed
            var pr = GetPCUReaderForUnitId(br.ReadInt32());
            br.BaseStream.Seek(tmp, SeekOrigin.Begin);
            return pr.GetClassField(br.ReadInt32());
        }

        private common_method_node ReadCommonExtMethod()
        {
            br.ReadByte();
            int offset = br.ReadInt32();
            int tmp = (int)br.BaseStream.Position;
            br.BaseStream.Seek(ext_pos + offset, SeekOrigin.Begin);
            br.ReadByte();//DS Changed
            var pr = GetPCUReaderForUnitId(br.ReadInt32());
            br.BaseStream.Seek(tmp, SeekOrigin.Begin);
            return pr.GetClassMethod(br.ReadInt32());
        }

        private common_property_node ReadCommonExtProperty()
        {
            br.ReadByte();
            int offset = br.ReadInt32();
            int tmp = (int)br.BaseStream.Position;
            br.BaseStream.Seek(ext_pos + offset, SeekOrigin.Begin);
            br.ReadByte();//DS Changed
            var pr = GetPCUReaderForUnitId(br.ReadInt32());
            br.BaseStream.Seek(tmp, SeekOrigin.Begin);
            return pr.GetPropertyNode(br.ReadInt32());
        }

        //получение типа, описанного в этом модуле (может вызываться извне)
		public type_node GetTypeReference(int offset)
		{
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as type_node;
            //если тип еще не десериализован, то восстанавливаем его
            return GetCommonType(offset);
		}

		private function_node GetMethodReference()
		{
			byte flag = br.ReadByte();
			if (flag == 0)
			{
				return GetMethodByOffset();
			}
			else if (flag == 1)
			{
				return GetCompiledConstructor(br.ReadInt32());
			}
			else if (flag == 2)
			{
				return GetCompiledMethod(br.ReadInt32());
			}
			return null;
		}

		private property_node GetPropertyReference()
		{
			byte flag = br.ReadByte();
			if (flag == 0)
			{
				return GetPropertyByOffset();
			}
			else if (flag == 1)
			{
				return GetCompiledProperty(br.ReadInt32());
			}
			return null;
		}

		private var_definition_node GetFieldReference()
		{
			byte flag = br.ReadByte();
			if (flag == 0)
			{
				return GetClassFieldByOffset();
			}
			else if (flag == 1)
			{
				return GetCompiledVariable(br.ReadInt32());
			}
			return null;
		}

        //получение типа
		private type_node GetTypeReference()
		{
			byte b = br.ReadByte();
            //(ssyy) Вставил switch вместо условий
            type_node tn = null;
            definition_node dn = null;
            switch (b)
            {
                case 255:
                	return null;
                case 1://если тип описан в модуле
                	int offset = br.ReadInt32();

                    if (members.TryGetValue(offset, out dn))
                        tn = (type_node)dn;
                	if (tn == null) return GetCommonType(offset);
                	return tn;
                case 0://если это импортир. тип
                    tn = null;
					int pos = br.ReadInt32();
                    if (ext_members.TryGetValue(pos, out dn))
                        return (type_node)dn;
					int tmp = (int)br.BaseStream.Position;
					br.BaseStream.Seek(ext_pos+pos,SeekOrigin.Begin);
					if ((ImportKind)br.ReadByte() == ImportKind.Common)
					{
						tn = ReadCommonExtType();
						ext_members[pos] = tn;
					}
					else // это нетовский тип
					{
						tn = ReadNetExtType();
						ext_members[pos] = tn;
					}
					br.BaseStream.Seek(tmp,SeekOrigin.Begin);
					return tn;
                case 2://это массив
                	simple_array type = new simple_array(GetTypeReference(),br.ReadInt32());
                	return type;
                case 3://это указатель
                	type_node pointed_type = GetTypeReference();
                	return pointed_type.ref_type;
                case 4://это динамический массив
                	location loc = null;
                	loc = ReadDebugInfo();
                	type_node elem_type = GetTypeReference();
                	int rank = br.ReadInt32();
                	return type_constructor.instance.create_unsized_array(elem_type, null, rank, loc);
                case 6:
                    return GetTemplateInstance();
                case 7:
                    return GetGenericInstance();
                case 8:
                    return GetShortStringType();
                case 9:
                    return GetGenericParameterOfType();
                case 10:
                    return GetGenericParameterOfFunction();
                case 11:
                    return GetGenericParameterOfMethod();
                case 12:
                    return new lambda_any_type_node();
            }
			return null;
		}

		private short_string_type_node GetShortStringType()
		{
			return compilation_context.instance.create_short_string_type(br.ReadInt32(),null);
		}

        private List<type_node> GetTypesList()
        {
            int params_count = br.ReadInt32();
            List<type_node> parameters = new List<type_node>(params_count);
            for (int i = 0; i < params_count; i++)
            {
                parameters.Add(GetTypeReference());
            }
            return parameters;
        }

        private common_type_node GetTemplateInstance()
        {
            template_class tc = GetTemplateClassReference();
            //int params_count = br.ReadInt32();
            //List<type_node> parameters = new List<type_node>(params_count);
            //for (int i = 0; i < params_count; i++)
            //{
            //    parameters.Add(GetTypeReference());
            //}
            List<type_node> parameters = GetTypesList();
            return SystemLibrary.SystemLibrary.syn_visitor.instance(tc, parameters, null);
        }

        private generic_instance_type_node GetGenericInstance()
        {
            type_node compn = GetTypeReference();
            //int params_count = br.ReadInt32();
            //List<type_node> parameters = new List<type_node>(params_count);
            //for (int i = 0; i < params_count; i++)
            //{
            //    parameters.Add(GetTypeReference());
            //}
            List<type_node> parameters = GetTypesList();
            int tmp = (int)br.BaseStream.Position;
            generic_instance_type_node tn = compn.get_instance(parameters) as generic_instance_type_node;
            br.BaseStream.Seek(tmp, SeekOrigin.Begin);
            return tn;
        }

        private generic_namespace_function_instance_node GetGenericNamespaceFunctionReference()
        {
            common_namespace_function_node orig = GetNamespaceFunctionByOffset();
            List<type_node> parameters = GetTypesList();
            int tmp_pos = (int)br.BaseStream.Position;
            var instance = orig.get_instance(parameters, false, null) as generic_namespace_function_instance_node;
            br.BaseStream.Seek(tmp_pos, SeekOrigin.Begin);
            return instance;
        }

        private function_node GetCompiledGenericMethodInstanceReference()
        {
            compiled_function_node orig = GetCompiledMethod(br.ReadInt32());
            List<type_node> parameters = GetTypesList();
            int tmp_pos = (int)br.BaseStream.Position;
            var instance = orig.get_instance(parameters, false, null);
            br.BaseStream.Seek(tmp_pos, SeekOrigin.Begin);
            return instance;
        }

        private generic_method_instance_node GetCommonGenericMethodInstanceReference()
        {
            common_method_node orig = GetMethodByOffset();
            List<type_node> parameters = GetTypesList();
            int tmp_pos = (int)br.BaseStream.Position;
            var instance = orig.get_instance(parameters, false, null) as generic_method_instance_node;
            br.BaseStream.Seek(tmp_pos, SeekOrigin.Begin);
            return instance;
        }

        private common_type_node GetGenericParameterOfType()
        {
            common_type_node generic_def = GetTypeReference() as common_type_node;
            return generic_def.generic_params[br.ReadInt32()] as common_type_node;
        }

        private common_type_node GetGenericParameterOfMethod()
        {
            common_method_node generic_meth = GetMethodByOffset();
            return generic_meth.generic_params[br.ReadInt32()] as common_type_node;
        }

        private common_type_node GetGenericParameterOfFunction()
        {
            common_namespace_function_node generic_func = GetNamespaceFunctionByOffset();
            return generic_func.generic_params[br.ReadInt32()] as common_type_node;
        }

        private compiled_event GetCompiledEvent(int offset)
        {
            int tmp = (int)br.BaseStream.Position;
            br.BaseStream.Seek(ext_pos + offset, SeekOrigin.Begin);
            br.ReadByte();
            compiled_type_node ctn = GetNetExtType(br.ReadInt32());
            int handle = br.ReadInt32();
            compiled_event cvd = NetHelper.NetHelper.GetEvent(FindEventByHandle(ctn.compiled_type, handle));
            br.BaseStream.Seek(tmp, SeekOrigin.Begin);
            return cvd;
        }

        private compiled_function_node GetCompiledMethod(int offset)
		{
            if (offset == PCUConsts.method_instance_as_compiled_function_node)
            {
                br.ReadByte();
                return GetCompiledGenericMethodInstanceReference() as compiled_function_node;
            }
			int tmp = (int)br.BaseStream.Position;
			br.BaseStream.Seek(ext_pos+offset,SeekOrigin.Begin);
			br.ReadByte();
			compiled_type_node ctn = GetNetExtType(br.ReadInt32());
            int handle = br.ReadInt32();
			compiled_function_node cfn = PascalABCCompiler.TreeRealization.compiled_function_node.get_compiled_method(FindMethodByHandle(ctn.compiled_type,handle));
			br.BaseStream.Seek(tmp,SeekOrigin.Begin);
			return cfn;
		}

		private compiled_constructor_node GetCompiledConstructor(int offset)
		{
			int tmp = (int)br.BaseStream.Position;
			br.BaseStream.Seek(ext_pos+offset,SeekOrigin.Begin);
			br.ReadByte();
			compiled_type_node ctn = GetNetExtType(br.ReadInt32());
			int handle = br.ReadInt32();
			compiled_constructor_node ccn = NetHelper.NetHelper.GetConstructorNode(FindConstructorByHandle(ctn.compiled_type,handle));
			br.BaseStream.Seek(tmp,SeekOrigin.Begin);
			return ccn;
		}

		private compiled_variable_definition GetCompiledVariable(int offset)
		{
			int tmp = (int)br.BaseStream.Position;
			br.BaseStream.Seek(ext_pos+offset,SeekOrigin.Begin);
			br.ReadByte();
			compiled_type_node ctn = GetNetExtType(br.ReadInt32());
			int handle = br.ReadInt32();
			compiled_variable_definition cvd = NetHelper.NetHelper.GetFieldNode(FindFieldByHandle(ctn.compiled_type,handle));
			br.BaseStream.Seek(tmp,SeekOrigin.Begin);
			return cvd;
		}

        private compiled_property_node GetCompiledProperty(int offset)
        {
            int tmp = (int)br.BaseStream.Position;
            br.BaseStream.Seek(ext_pos + offset, SeekOrigin.Begin);
            br.ReadByte();
            compiled_type_node ctn = GetNetExtType(br.ReadInt32());
            int handle = br.ReadInt32();
            compiled_property_node cpn = NetHelper.NetHelper.GetPropertyNode(FindPropertyByHandle(ctn.compiled_type, handle));
            br.BaseStream.Seek(tmp, SeekOrigin.Begin);
            return cpn;
        }

        private attributes_list GetAttributes()
        {
        	int pos = br.ReadInt32();
        	attributes_list attrs = new attributes_list();
        	int tmp = (int)br.BaseStream.Position;
        	br.BaseStream.Seek(pos+start_pos, SeekOrigin.Begin);
        	int count = br.ReadInt32();
        	for (int i=0; i<count; i++)
        		attrs.AddElement(GetAttribute());
        	br.BaseStream.Seek(tmp, SeekOrigin.Begin);
        	return attrs;
        }

        private attribute_node GetAttribute()
        {
        	type_node tn = GetTypeReference();
        	function_node fn = GetMethodReference();
        	SemanticTree.attribute_qualifier_kind quaifier = (SemanticTree.attribute_qualifier_kind)br.ReadByte();
        	int count = br.ReadInt32();
        	List<constant_node> args = new List<constant_node>();
        	for (int i=0; i<count; i++)
        		args.Add((constant_node)CreateExpression());
        	count = br.ReadInt32();
        	List<property_node> prop_names = new List<property_node>();
        	for (int i=0; i<count; i++)
        		prop_names.Add(GetPropertyReference());
        	count = br.ReadInt32();
        	List<var_definition_node> field_names = new List<var_definition_node>();
        	for (int i=0; i<count; i++)
        		field_names.Add(GetFieldReference());
        	count = br.ReadInt32();
        	List<constant_node> prop_values = new List<constant_node>();
        	for (int i=0; i<count; i++)
        		prop_values.Add((constant_node)CreateExpression());
        	count = br.ReadInt32();
			List<constant_node> field_values = new List<constant_node>();
        	for (int i=0; i<count; i++)
        		field_values.Add((constant_node)CreateExpression());
        	location loc = ReadDebugInfo();
        	attribute_node attr = new attribute_node(fn,tn,loc);
        	attr.qualifier = quaifier;
        	attr.args.AddRange(args);
        	attr.prop_names.AddRange(prop_names);
        	attr.field_names.AddRange(field_names);
        	attr.prop_initializers.AddRange(prop_values);
        	attr.field_initializers.AddRange(field_values);
        	return attr;
        }

        //восстановление сущности из модуля
        //вызывается только при поиске имени находится SymbolInfo с заглушкой
        //эта заглушка должна быть заменена настоящей сущностью
		public override definition_node CreateInterfaceMember(int offset, string name)
		{
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
            {
                //DarkStar Addes 06/03/07
                PCUWriter.AddExternalMember(dn, offset);
                return dn;
            }
			br.BaseStream.Seek(start_pos+offset,SeekOrigin.Begin);
			semantic_node_type snt = (semantic_node_type)br.ReadByte();
			switch (snt)
			{
				case semantic_node_type.namespace_variable:
					dn = CreateInterfaceNamespaceVariable(name,offset); break;
                case semantic_node_type.common_namespace_event:
                    dn = CreateInterfaceNamespaceEvent(name, offset); break;
				case semantic_node_type.common_namespace_function_node:
					dn = CreateInterfaceNamespaceFunction(name,offset); break;
				case semantic_node_type.common_type_node:
					dn = CreateInterfaceCommonType(name,offset); break;
                case semantic_node_type.namespace_constant_definition:
                    dn = CreateInterfaceConstantDefinition(name,offset); break;
                case semantic_node_type.compiled_type_node:
                    dn = CreateCompiledTypeNode(offset); break;
                //ssyy
                case semantic_node_type.template_type:
                    dn = CreateTemplateClass(offset); break;
                //\ssyy
                case semantic_node_type.ref_type_node:
                    dn = CreateRefType(offset); break;

            }
			if (dn == null)
			{

			}
			br.BaseStream.Seek(start_pos,SeekOrigin.Begin);
            PCUWriter.AddExternalMember(dn,offset);//сообщаем врайтеру новое смещение сущности
            //если будет сериализовываться модуль в котором используются сущности .pcu, он должен знать их смещения
            //ProcessWaitedToRestoreFields();
            return dn;
		}

        //ssyy
        public override definition_node CreateImplementationMember(int offset, bool restore_code = true)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
            {
                PCUWriter.AddExternalMember(dn, offset);
                return dn;
            }
			br.BaseStream.Seek(start_pos+offset,SeekOrigin.Begin);
			semantic_node_type snt = (semantic_node_type)br.ReadByte();
			switch (snt)
			{
				case semantic_node_type.namespace_variable:
					dn = GetNamespaceVariable(offset); break;
                case semantic_node_type.common_namespace_event:
                    dn = GetNamespaceEvent(offset); break;
				case semantic_node_type.common_namespace_function_node:
					dn = GetNamespaceFunction(offset, restore_code); break;
				case semantic_node_type.common_type_node:
					dn = GetCommonType(offset); break;
                case semantic_node_type.namespace_constant_definition:
                    dn = GetConstantDefinition(offset); break;
                case semantic_node_type.compiled_type_node:
                    dn = CreateCompiledTypeNode(offset); break;
                //ssyy
                case semantic_node_type.template_type:
                    dn = CreateTemplateClass(offset); break;
                //\ssyy
                case semantic_node_type.ref_type_node:
                    dn = CreateRefType(offset); break;
            }
			br.BaseStream.Seek(start_pos,SeekOrigin.Begin);
            PCUWriter.AddExternalMember(dn,offset);//сообщаем врайтеру новое смещение сущности
            //если будет сериализовываться модуль в котором используются сущности .pcu, он должен знать их смещения
            //ProcessWaitedToRestoreFields();
            return dn;
        }
        //\ssyy

        //восстановление сущности- члена класса
        public override definition_node CreateInterfaceInClassMember(int offset, string name)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn;
            br.BaseStream.Seek(start_pos + offset, SeekOrigin.Begin);
            semantic_node_type snt = (semantic_node_type)br.ReadByte();
            switch (snt)
            {
                case semantic_node_type.class_field:
                    dn = CreateInterfaceClassField(name, offset); break;
                case semantic_node_type.common_property_node:
                    dn = CreateInterfaceProperty(name, offset); break;
                case semantic_node_type.common_method_node:
                    dn = CreateInterfaceMethod(name, offset); break;
                case semantic_node_type.class_constant_definition:
                    dn = CreateInterfaceClassConstantDefinition(name, offset); break;
                case semantic_node_type.common_event:
                    dn = CreateInterfaceEvent(name, offset); break;
                case semantic_node_type.common_namespace_function_node:
                    dn = CreateInterfaceNamespaceFunction(name, offset); break;
            }
            br.BaseStream.Seek(start_pos, SeekOrigin.Begin);
            PCUWriter.AddExternalMember(dn, offset);
            //ProcessWaitedToRestoreFields();
            return dn;
        }

        private class_constant_definition CreateInterfaceClassConstantDefinition(string name, int offset)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as class_constant_definition;
            class_constant_definition ccd = null;
            br.ReadInt32();//writer->GetNameIndex(cdn)
            type_node type = GetTypeReference();
            common_type_node cont = (common_type_node)GetTypeReference(br.ReadInt32());
            constant_node expr = (constant_node)CreateExpressionWithOffset();
            expr.type = type;
            SemanticTree.field_access_level fal = (SemanticTree.field_access_level)br.ReadByte();
            location loc = ReadDebugInfo();
            ccd = new class_constant_definition(name, expr, loc, cont, fal);
            AddMember(ccd, offset);
            //members[offset] = ccd;
            cont.const_defs.AddElement(ccd);
            return ccd;
        }

        private common_method_node CreateInterfaceMethod(string name, int offset, bool not_restore_code = false)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as common_method_node;
            common_method_node cmn = new common_method_node(name,null,null);
            //members[offset] = cmn;
            AddMember(cmn, offset);

            int name_ref = br.ReadInt32();

            cmn.is_final = br.ReadByte() == 1;
            cmn.newslot_awaited = br.ReadByte() == 1;

            ReadGenericFunctionInformation(cmn);

            if (br.ReadByte() == 1) //return_value_type
            {
                cmn.return_value_type = GetTypeReference();
                if (br.ReadByte() == 1)
                {
                    cmn.return_variable = GetLocalVariable(cmn);
                    cmn.var_definition_nodes_list.AddElement(cmn.return_variable);
                }
            }
            int num_par = br.ReadInt32();
            for (int i = 0; i < num_par; i++)
                cmn.parameters.AddElement(GetParameter(cmn));
            cmn.cont_type = (common_type_node)GetTypeReference(br.ReadInt32());
            if (name==null || true)
                cmn.SetName(GetStringInClass(cmn.cont_type, name_ref));
            cmn.attributes.AddRange(GetAttributes());
            cmn.is_constructor = br.ReadBoolean();
            cmn.is_forward = br.ReadBoolean();
            cmn.is_overload = br.ReadBoolean();
            cmn.set_access_level((SemanticTree.field_access_level)br.ReadByte());
            cmn.polymorphic_state = (SemanticTree.polymorphic_state)br.ReadByte();
            if (cmn.is_constructor && cmn.polymorphic_state == SemanticTree.polymorphic_state.ps_static)
                cmn.cont_type.static_constr = cmn;
            cmn.num_of_default_variables = br.ReadInt32();
            cmn.num_of_for_cycles = br.ReadInt32();
            bool has_overrided_method = br.ReadBoolean();

            int num_var = br.ReadInt32();
            GetVariables(cmn, num_var);
            int num_consts = br.ReadInt32();
            for (int i = 0; i < num_consts; i++)
            {
                function_constant_definition fcd = GetFunctionConstant(cmn);
                cmn.constants.AddElement(fcd);
            }
            int num_nest_funcs = br.ReadInt32();
            for (int i = 0; i < num_nest_funcs; i++)
                cmn.functions_nodes_list.AddElement(GetNestedFunction());
            //br.ReadInt32();//code;
            cmn.loc = ReadDebugInfo();

            if (has_overrided_method)
                cmn.function_code = GetCodeWithOverridedMethod(cmn, br.ReadInt32());
            else if (not_restore_code && cmn.name != "get_val" && cmn.name != "set_val")//ignore pascal array property accessors
                AddWaitedMethodCode(cmn, br.ReadInt32());
            else if (cmn.name == "<>") // for conform_basic_function
                AddWaitedMethodCode(cmn, br.ReadInt32());
            else
                cmn.function_code = GetCode(br.ReadInt32());
            cmn.cont_type.methods.AddElement(cmn);
            if (cmn.name == "op_Equality")
                cmn.cont_type.scope.AddSymbol(StringConstants.eq_name, new SymbolInfo(cmn));
            else if (cmn.name == "op_Inequality")
                cmn.cont_type.scope.AddSymbol(StringConstants.noteq_name, new SymbolInfo(cmn));
            return cmn;
        }

        private void GetVariables(common_function_node cfn, int num_var)
        {
            var local_list = new List<Tuple<local_variable, int>>();
            for (int i = 0; i < num_var; i++)
            {
                var tup = GetLocalVariableLazy(cfn);
                local_variable lv = tup.Item1;
                if (lv != cfn.return_variable)
                    cfn.var_definition_nodes_list.AddElement(lv);
                if (tup.Item2 != -1)
                    local_list.Add(tup);
            }
            foreach (var tup in local_list)
            {
                tup.Item1.inital_value = CreateExpressionWithOffset(tup.Item2);
            }
        }

        private ref_type_node CreateRefType(int offset)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as ref_type_node;
            ref_type_node node = null;
            string name;
            bool is_interface = br.ReadBoolean();
            if (is_interface)//пропускаем флаг - интерфейсности
            {
                name = GetString(br.ReadInt32());
            }
            else
            {
                name = br.ReadString();
            }
            type_node type = GetTypeReference();
            node = type.ref_type;
            node.SetName(name);
            AddMember(node, offset);
            return node;

        }

        //все методы с именем на Get вызываются в процессе восстановления других сущностей
        //например, когда восст. код ф-ии, в нем вызывается другая ф-я, которая еще не восстановлена
        private common_method_node GetClassMethod(int offset, bool not_restore_code = false)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as common_method_node;
            common_method_node cmn = null;
            int tmp = (int)br.BaseStream.Position;
            br.BaseStream.Seek(start_pos + offset, SeekOrigin.Begin);
            br.ReadByte();

            cmn = (common_method_node)CreateInterfaceMethod(null, offset, not_restore_code);

            br.BaseStream.Seek(tmp, SeekOrigin.Begin);
            return cmn;
        }

        private definition_node CreateInterfaceEvent(string name, int offset)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as common_event;
        	common_event ce = null;
        	int name_ref = br.ReadInt32();
            type_node type = GetTypeReference();
            common_method_node add_meth = null;
            if (CanReadObject())
            add_meth = GetClassMethod(br.ReadInt32());
            common_method_node remove_meth = null;
            if (CanReadObject())
            remove_meth = GetClassMethod(br.ReadInt32());
            common_method_node raise_meth = null;
            if (CanReadObject())
            raise_meth = GetClassMethod(br.ReadInt32());
            class_field cf = GetClassField(br.ReadInt32());
            common_type_node cont = (common_type_node)GetTypeReference(br.ReadInt32());
            if (name==null)
                name = GetStringInClass(cont, name_ref);
            SemanticTree.field_access_level fal = (SemanticTree.field_access_level)br.ReadByte();
            SemanticTree.polymorphic_state ps = (SemanticTree.polymorphic_state)br.ReadByte();
            attributes_list attrs = GetAttributes();
            location loc = ReadDebugInfo();
            ce = new common_event(name,type,cont,add_meth,remove_meth,raise_meth,fal,ps,loc);
            ce.field = cf;
            ce.attributes.AddRange(attrs);
            cont.events.AddElement(ce);
            AddMember(ce, offset);
            return ce;
        }

        public common_event GetEventNode(int offset)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as common_event;
            common_event ce = null;
            int tmp = (int)br.BaseStream.Position;
            br.BaseStream.Seek(start_pos + offset, SeekOrigin.Begin);
            br.ReadByte();

            ce = (common_event)CreateInterfaceEvent(null, offset);

            br.BaseStream.Seek(tmp, SeekOrigin.Begin);
            return ce;
        }

        public common_namespace_event GetNamespaceEventNode(int offset)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as common_namespace_event;
            common_namespace_event ce = null;
            int tmp = (int)br.BaseStream.Position;
            br.BaseStream.Seek(start_pos + offset, SeekOrigin.Begin);
            br.ReadByte();

            ce = (common_namespace_event)CreateInterfaceNamespaceEvent(null, offset);

            br.BaseStream.Seek(tmp, SeekOrigin.Begin);
            return ce;
        }

        private definition_node CreateInterfaceProperty(string name, int offset)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as common_property_node;
            common_property_node prop = null;
            int name_ref = br.ReadInt32();
            type_node type = GetTypeReference();
            common_method_node get_meth = null;
            if (br.ReadByte() == 1)
                get_meth = GetClassMethod(br.ReadInt32(), !waited_method_restoring);
            common_method_node set_meth = null;
            if (br.ReadByte() == 1)
                set_meth = GetClassMethod(br.ReadInt32(), !waited_method_restoring);
            int num = br.ReadInt32();
            parameter_list pl = new parameter_list();
            for (int i = 0; i < num; i++)
                pl.AddElement(GetParameter());
            common_type_node cont = (common_type_node)GetTypeReference(br.ReadInt32());
            SemanticTree.field_access_level fal = (SemanticTree.field_access_level)br.ReadByte();
            SemanticTree.polymorphic_state ps = (SemanticTree.polymorphic_state)br.ReadByte();
            attributes_list attrs = GetAttributes();
            location loc = ReadDebugInfo();
            if (name==null)
                name = GetStringInClass(cont, name_ref);
            prop = new common_property_node(name, cont, type, get_meth, set_meth, loc, fal, ps);
            prop.attributes.AddRange(attrs);
            prop.parameters.AddRange(pl);
            cont.properties.AddElement(prop);
            //members[offset] = prop;
            AddMember(prop, offset);

            return prop;
        }

        private common_property_node GetPropertyNode(int offset)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as common_property_node;
            common_property_node prop = null;
            int tmp = (int)br.BaseStream.Position;
            br.BaseStream.Seek(start_pos + offset, SeekOrigin.Begin);
            br.ReadByte();

            prop = (common_property_node)CreateInterfaceProperty(null, offset);

            br.BaseStream.Seek(tmp, SeekOrigin.Begin);
            return prop;
        }

        private definition_node CreateInterfaceClassField(string name, int offset)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as class_field;
            class_field field = null;
            int name_off = br.ReadInt32();
            type_node type = GetTypeReference();
            field = new class_field(name,type,null,SemanticTree.polymorphic_state.ps_common,SemanticTree.field_access_level.fal_internal,null);
            AddMember(field, offset);
            expression_node initv = null;
            if (CanReadObject())
                initv = CreateExpressionWithOffset();
            common_type_node cont = (common_type_node)GetTypeReference(br.ReadInt32());
            if (name==null)
                name = GetStringInClass(cont, name_off);
            SemanticTree.field_access_level fal = (SemanticTree.field_access_level)br.ReadByte();
            SemanticTree.polymorphic_state ps = (SemanticTree.polymorphic_state)br.ReadByte();
            attributes_list attrs = GetAttributes();
            location loc = ReadDebugInfo();
            field.name = name;
            field.attributes.AddRange(attrs);
            field.cont_type = cont;
            field.field_access_level = fal;
            field.polymorphic_state = ps;
            field.loc = loc;

            //field = new class_field(name,type,cont,ps,fal,loc);
            field.inital_value = initv;
            cont.fields.AddElement(field);
            //members[offset] = field;
            AddMember(field, offset);
            return field;
        }

        public class_field GetClassField(int offset)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as class_field;
            class_field field = null;
            int tmp = (int)br.BaseStream.Position;
            br.BaseStream.Seek(start_pos + offset, SeekOrigin.Begin);
            br.ReadByte();

            field = (class_field)CreateInterfaceClassField(null, offset);

            br.BaseStream.Seek(tmp, SeekOrigin.Begin);
            return field;
        }

        private void AddEnumOperators(common_type_node tctn)
        {
        	/*basic_function_node enum_gr = SystemLibrary.SystemLibrary.make_binary_operator(StringConstants.gr_name,tctn,SemanticTree.basic_function_type.enumgr,SystemLibrary.SystemLibrary.bool_type);
            basic_function_node enum_greq = SystemLibrary.SystemLibrary.make_binary_operator(StringConstants.greq_name,tctn,SemanticTree.basic_function_type.enumgreq,SystemLibrary.SystemLibrary.bool_type);
            basic_function_node enum_sm = SystemLibrary.SystemLibrary.make_binary_operator(StringConstants.sm_name,tctn,SemanticTree.basic_function_type.enumsm,SystemLibrary.SystemLibrary.bool_type);
            basic_function_node enum_smeq = SystemLibrary.SystemLibrary.make_binary_operator(StringConstants.smeq_name,tctn,SemanticTree.basic_function_type.enumsmeq,SystemLibrary.SystemLibrary.bool_type);*/
            compilation_context.add_convertions_to_enum_type(tctn);
        }

        private List<SemanticTree.ITypeNode> ReadImplementingInterfaces()
        {
            int interf_count = br.ReadInt32();
            List<SemanticTree.ITypeNode> interf_implemented = new List<SemanticTree.ITypeNode>(interf_count);
            for (int i = 0; i < interf_count; i++)
            {
                interf_implemented.Add(GetTypeReference());
            }
            return interf_implemented;
        }

        private List<SemanticTree.ICommonTypeNode> ReadGenericParams(common_namespace_node cur_nn)
        {
            if (SemanticRulesConstants.RuntimeInitVariablesOfGenericParameters)
            {
                if (!SystemLibrary.SystemLibInitializer.NeedsToRestore.Contains(
                    SystemLibrary.SystemLibInitializer.RuntimeInitializeFunction))
                {
                    SystemLibrary.SystemLibInitializer.NeedsToRestore.Add(
                        SystemLibrary.SystemLibInitializer.RuntimeInitializeFunction);
                }
            }
            //common_namespace_node cur_nn = (is_interface) ? cun.namespaces[0] : cun.namespaces[1];
            int param_count = br.ReadInt32();
            List<SemanticTree.ICommonTypeNode> type_params = new List<PascalABCCompiler.SemanticTree.ICommonTypeNode>(param_count);
            for (int i = 0; i < param_count; i++)
            {
                string par_name = br.ReadString();
                common_type_node par = new common_type_node(
                    par_name, SemanticTree.type_access_level.tal_public, cur_nn,
                    SystemLibrary.SystemLibrary.syn_visitor.convertion_data_and_alghoritms.symbol_table.CreateInterfaceScope(null, SystemLibrary.SystemLibrary.object_type.Scope, null),
                    null);
                SystemLibrary.SystemLibrary.init_reference_type(par);
                par.SetBaseType(SystemLibrary.SystemLibrary.object_type);
                type_params.Add(par);
            }
            return type_params;
        }

        private void ReadTypeParameterEliminations(common_type_node par)
        {
            switch ((GenericParamKind)(br.ReadByte()))
            {
                case GenericParamKind.Class:
                    par.is_class = true;
                    break;
                case GenericParamKind.Value:
                    par.internal_is_value = true;
                    break;
            }
            par.SetBaseType(GetTypeReference());
            par.SetImplementingInterfaces(ReadImplementingInterfaces());
            if (CanReadObject())
            {
                generic_parameter_eliminations.add_default_ctor(par);
            }
            //if (CanReadObject())
            //{
            //    par.runtime_initialization_marker = GetClassFieldByOffset();
            //}
        }

        private definition_node CreateInterfaceCommonType(string name, int offset)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as common_type_node;
            common_type_node ctn = null;

            bool is_interface = br.ReadBoolean();
            int ind = br.ReadInt32();

            if (is_interface)//пропускаем флаг - интерфейсности
            {
                name = GetString(br.ReadInt32());
            }
            else
            {
                name = br.ReadString();
            }
            //br.ReadInt32();
            //Читаем, является ли тип интерфейсом
            bool type_is_interface = (br.ReadByte() == 1);

            bool type_is_class = (br.ReadByte() == 1);

            //Читаем, является ли тип делегатом
            bool type_is_delegate = (br.ReadByte() == 1);

            //Читаем, является ли тип описанием дженерика
            bool type_is_generic_definition = (br.ReadByte() == 1);

            List<SemanticTree.ICommonTypeNode> type_params = null;
            if (type_is_generic_definition)
            {
                type_params = ReadGenericParams((is_interface) ? cun.namespaces[0] : cun.namespaces[1]);
            }

            WrappedInterfaceScope iscope = null;
            WrappedClassScope scope;
            if (type_is_interface)
            {
                iscope = new WrappedInterfaceScope(this, cun.scope, null);
                scope = iscope;
            }
            else
            {
                scope = new WrappedClassScope(this, cun.scope, null);
            }
            ctn = new wrapped_common_type_node(this, null, name, SemanticTree.type_access_level.tal_public, cun.namespaces[0], scope, null, offset);
            scope.class_type = ctn;
            scope.ctn = ctn;
            if (is_interface)
                AddTypeToOrderList(ctn, ind);
            else
                AddImplTypeToOrderList(ctn, ind);
            if (type_is_generic_definition)
            {
                ctn.is_generic_type_definition = true;
                ctn.generic_params = type_params;
            }
            AddMember(ctn, offset);

            int_members.Insert(0, ctn);
            SemanticTree.type_special_kind tsk = (SemanticTree.type_special_kind)br.ReadByte();
            ctn.type_special_kind = tsk;
            common_type_node saved_ctn = ctn;
            if (ctn.full_name == "PABCSystem.BinaryFile")
                ctn.type_special_kind = SemanticTree.type_special_kind.binary_file;
            if (ctn.type_special_kind != SemanticTree.type_special_kind.set_type)
            {
                SystemLibrary.SystemLibrary.init_reference_type(ctn);
            }
            type_node base_type = GetTypeReference();
            bool is_value_type = br.ReadBoolean();
            ctn.SetBaseType(base_type);
            ctn.internal_is_value = is_value_type;


            List<SemanticTree.ITypeNode> interf_implemented = ReadImplementingInterfaces();

            constant_node low_val = null;
            constant_node upper_val = null;
            SemanticTree.type_access_level tal = (SemanticTree.type_access_level)br.ReadByte();

            ctn.SetIsSealed(br.ReadBoolean());
            ctn.SetIsAbstract(br.ReadBoolean(), null); // Причина null, потому что проблема пересечения sealed и abstract не может произойти после загрузки из .pcu
            ctn.SetIsStatic(br.ReadBoolean());
            ctn.IsPartial = br.ReadBoolean();

            if (tsk == SemanticTree.type_special_kind.diap_type)
            {
                low_val = CreateExpression() as constant_node;
                upper_val = CreateExpression() as constant_node;
            }

            if (type_is_interface)
            {
                //Добавляем ссылки на области видимости предков интерфейса
                List<SymbolTable.Scope> interf_scopes = new List<SymbolTable.Scope>();
                foreach (type_node tnode in interf_implemented)
                {
                    interf_scopes.Add(tnode.Scope);
                }
                iscope.TopInterfaceScopeArray = interf_scopes.ToArray();
            }

            ctn.IsInterface = type_is_interface;
            ctn.IsDelegate = type_is_delegate;
            ctn.is_class = type_is_class;
            ctn.ImplementingInterfaces.AddRange(interf_implemented);

            if (type_is_generic_definition)
            {
                foreach (common_type_node par in type_params)
                {
                    par.generic_type_container = ctn;
                    ReadTypeParameterEliminations(par);
                }
            }
            type_node element_type = null;
            if (CanReadObject())
                element_type = GetTypeReference();
            ctn.element_type = element_type;

            if (ctn.type_special_kind == SemanticTree.type_special_kind.set_type)
            {
                ctn = compilation_context.AddTypeToSetTypeList(ctn);
                if (saved_ctn != ctn)
                {
                    RemoveMember(offset, saved_ctn);
                    int_members.Remove(saved_ctn);
                    AddMember(ctn, offset);
                }
            }
            if (ctn.type_special_kind == SemanticTree.type_special_kind.typed_file)
            {
                ctn = compilation_context.AddTypeToTypedFileList(ctn);
                if (saved_ctn != ctn)
                {
                    RemoveMember(offset, saved_ctn);
                    int_members.Remove(saved_ctn);
                    AddMember(ctn, offset);
                }
            }
            br.ReadInt32();//comprehensive unit;
            ctn.attributes.AddRange(GetAttributes());
            byte flag = br.ReadByte();
            int def_prop_off = 0;
            if (flag == 1)
            {
                def_prop_off = br.ReadInt32();
            }
            location loc = ReadDebugInfo();
            ctn.loc = loc;
            if (type_is_delegate)
            {
                SystemLibrary.SystemLibrary.type_constructor.AddOperatorsToDelegate(ctn, loc);
            }

            class_names[ctn] = AddClassMemberNames(scope);
            if (flag == 1) ctn.default_property = GetPropertyNode(def_prop_off);
            //ivan
            if (ctn.IsEnum)
            {
                AddEnumOperators(ctn);
                MakeTypeAsOrdinal(ctn, 0, class_names.Count);
                ctn.add_additional_enum_operations();
            }
            if (ctn.type_special_kind == SemanticTree.type_special_kind.diap_type)
            {
                type_constructor.add_convertions_to_diap(ctn, low_val, upper_val);
            }
            if (ctn.type_special_kind == SemanticTree.type_special_kind.array_kind)
            {
                if (!(ctn.element_type is compiled_type_node))
                {
                    type_constructor.make_array_interface(ctn);
                }
            }
            if (ctn.is_value_type)
            {

            }
            //RestoreAllFields(ctn);
            if (!waited_types_to_restore_fields.Contains(ctn))
                waited_types_to_restore_fields.Add(ctn);

            if (type_is_delegate)
            {
                SymbolInfo sim = ctn.find_first_in_type(StringConstants.invoke_method_name);
                common_method_node invoke_method = sim.sym_info as common_method_node;
                sim = ctn.find_first_in_type(StringConstants.default_constructor_name);
                common_method_node constructor = sim.sym_info as common_method_node;
                delegate_internal_interface dii = new delegate_internal_interface(invoke_method.return_value_type, invoke_method, constructor);
                dii.parameters.AddRange(invoke_method.parameters);
                ctn.add_internal_interface(dii);
            }

            if (type_is_interface)
            {
                RestoreAllFields(ctn);
            }
            if (ctn.is_value)
            {
                RestoreAllFields(ctn);
            }
            if (ctn.is_generic_type_definition)
            {
                foreach (common_type_node par in ctn.generic_params)
                {
                    SymbolInfo tsi = ctn.find_first_in_type(StringConstants.generic_param_kind_prefix + par.name);
                    if (tsi != null)
                    {
                        par.runtime_initialization_marker = tsi.sym_info as class_field;
                    }
                }
            }

            return ctn;
        }

        private void MakeTypeAsOrdinal(common_type_node ctn, int low_val, int upper_val)
        {
            internal_interface ii = SystemLibrary.SystemLibrary.integer_type.get_internal_interface(internal_interface_kind.ordinal_interface);
            ordinal_type_interface oti_old = (ordinal_type_interface)ii;
            enum_const_node lower_value = new enum_const_node(low_val, ctn, ctn.loc);
            enum_const_node upper_value = new enum_const_node(upper_val - 1, ctn, ctn.loc);
            ordinal_type_interface oti_new = new ordinal_type_interface(oti_old.inc_method, oti_old.dec_method,
                oti_old.inc_value_method, oti_old.dec_value_method,
                oti_old.lower_eq_method, oti_old.greater_eq_method,
                oti_old.lower_method, oti_old.greater_method,
                lower_value, upper_value, oti_old.value_to_int, oti_old.ordinal_type_to_int);

            ctn.add_internal_interface(oti_new);
        }

        private template_class GetTemplateClass(int offset)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as template_class;
            template_class tc = null;

            int pos = (int)br.BaseStream.Position;
            br.BaseStream.Seek(start_pos + offset, SeekOrigin.Begin);
            br.ReadByte();

            tc = CreateTemplateClass(offset);

            br.BaseStream.Seek(pos, SeekOrigin.Begin);
            return tc;
        }

        private template_class GetTemplateClassReference()
        {
            byte b = br.ReadByte();
            if (b == 1)//если тип описан в модуле
            {
                int offset = br.ReadInt32();
                return GetTemplateClass(offset);
            }
            if (b == 0)//если это импортир. тип
            {
                template_class tc = null;
                int pos = br.ReadInt32();
                definition_node dn = null;
                if (ext_members.TryGetValue(pos, out dn))
                    return (template_class)dn;
                int tmp = (int)br.BaseStream.Position;
                br.BaseStream.Seek(ext_pos + pos, SeekOrigin.Begin);
                if ((ImportKind)br.ReadByte() == ImportKind.Common)
                {
                    tc = ReadTemplateExtClass();
                    ext_members[pos] = tc;
                }
                else // это нетовский тип
                {
                    return null;
                }
                br.BaseStream.Seek(tmp, SeekOrigin.Begin);
                return tc;
            }
            return null;
        }

        private template_class CreateTemplateClass(int offset)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as template_class;
            template_class tc = null;

            string name = br.ReadString();

            bool is_syn = br.ReadByte() == 1;

            using_namespace_list unl = new using_namespace_list();
            int using_count = br.ReadInt32();
            for (int i = 0; i < using_count; i++)
            {
                unl.AddElement(new using_namespace(br.ReadString()));
            }
            using_namespace_list unl2 = null;
            if (CanReadObject())
            {
                unl2 = new using_namespace_list();
                int using_count2 = br.ReadInt32();
                for (int i = 0; i < using_count2; i++)
                {
                    unl2.AddElement(new using_namespace(br.ReadString()));
                }
            }
            document doc = null;
            if (br.ReadByte() == 1)
            {
                doc = new document(Path.Combine(Path.GetDirectoryName(cur_doc.file_name),br.ReadString()));
            }

            SyntaxTree.SyntaxTreeStreamReader str = new SyntaxTree.SyntaxTreeStreamReader();
            str.br = br;
            SyntaxTree.type_declaration t_d = str._read_node() as SyntaxTree.type_declaration;

            int ext_count = br.ReadInt32();
            List<procedure_definition_info> pdi_list = new List<procedure_definition_info>(ext_count);
            for (int i = 0; i < ext_count; i++)
            {
                byte num = br.ReadByte();
                common_namespace_node c_m_n = cun.namespaces[num];
                SyntaxTree.syntax_tree_node stn = str._read_node();
                SyntaxTree.procedure_definition p_d = stn as SyntaxTree.procedure_definition;
                pdi_list.Add(new procedure_definition_info(c_m_n, p_d));
            }

            //(ssyy) Далее формируем список областей видимости, которые
            //подключаются к модулю. Вообще-то шаблоны классов этим
            //заниматься не должны, но кроме них этот список
            //никому не нужен (по состоянию на 01.06.2007).
            if (cun.scope.TopScopeArray.Length == 0)
            {
                cun.scope.TopScopeArray = MakeTopScopeArray(unl, pcu_file.interface_uses_count);
            }

            if (cun.implementation_scope.TopScopeArray.Length == 0 && unl2 != null)
            {
                //формируем implementation - область
                cun.implementation_scope.TopScopeArray = MakeTopScopeArray(unl2, pcu_file.incl_modules.Length);
            }

            tc = new template_class(t_d, name, cun.namespaces[0], doc, unl);
            tc.external_methods = pdi_list;
            tc.using_list2 = unl2;
            tc.is_synonym = is_syn;

            //members[offset] = tc;
            AddMember(tc, offset);

            cun.namespaces[0].scope.AddSymbol(name, new SymbolInfo(tc));

            return tc;
        }

        /*private type_synonym CreateTypeSynonym()
        {
            string name = br.ReadString();
            type_node tn = GetTypeReference();
            location loc = ReadDebugInfo();
            type_synonym ts = new type_synonym(name, tn, loc);
            return ts;
        }*/

        private wrapped_type_synonym CreateTypeSynonym()
        {
            string name = br.ReadString();
            int off = br.ReadInt32();
            wrapped_type_synonym wts = new wrapped_type_synonym(this,name,(int)br.BaseStream.Position);
            br.BaseStream.Seek(off-4, SeekOrigin.Current);
            return wts;
        }

        public override definition_node CreateTypeSynonim(int offset, string name)
        {
        	int tmp = (int)br.BaseStream.Position;
        	br.BaseStream.Seek(offset, SeekOrigin.Begin);
        	type_node tn = GetTypeReference();
        	br.BaseStream.Seek(tmp, SeekOrigin.Begin);
            return tn;
        }

        private void AddTypeSynonyms(int offset, SymbolTable.Scope scope)
        {
            int pos = (int)br.BaseStream.Position;
            br.BaseStream.Seek(start_pos + offset, SeekOrigin.Begin);
            int count = br.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                //type_synonym ts = CreateTypeSynonym();//ploho!! nado sozdavat zaglushki
                //scope.AddSymbol(ts.name, new SymbolInfo(ts.original_type));
                wrapped_type_synonym wts = CreateTypeSynonym();
                SymbolInfo si = new SymbolInfo();
                si.sym_info = wts;
                scope.AddSymbol(wts.name, si);
            }
            br.BaseStream.Seek(pos, SeekOrigin.Begin);
        }

        public SymbolTable.Scope[] MakeTopScopeArray(using_namespace_list unl, int uses_count)
        {
            List<SymbolTable.Scope> top_scopes = new List<SymbolTable.Scope>();

            //Добавляем внутренний системный модуль
            top_scopes.Add(SystemLibrary.SystemLibrary.system_unit.scope);

            //Добавляем .NET - пространство
            System.Reflection.Assembly _as = typeof(string).Assembly;
            NetHelper.NetScope ns = new NetHelper.NetScope(unl, _as, SystemLibrary.SystemLibrary.symtab);
            top_scopes.Add(ns);

            //Добавляем подключенные модули
            for (int i = 0; i < uses_count; i++)
            {
                PCUReader pcu_r = units[GetFullUnitName(pcu_file.incl_modules[i])] as PCUReader;
                if (pcu_r != null)
                {
                    top_scopes.Add(pcu_r.cun.scope);
                }
            }

            //Добавляем внутренний системный модуль
            //top_scopes.Add(SystemLibrary.SystemLibrary.system_unit.scope);

            return top_scopes.ToArray();
        }

        public common_type_node GetCommonType(int offset)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as common_type_node;
            common_type_node ctn = null;
            int pos = (int)br.BaseStream.Position;
            br.BaseStream.Seek(start_pos + offset, SeekOrigin.Begin);
            br.ReadByte();

            ctn = (common_type_node)CreateInterfaceCommonType(null, offset);

            if (ctn.fields.Count > 0)
            {
                if (ctn.fields[0].type is simple_array)
                {
                    ctn.find(StringConstants.upper_array_const_name);
                    ctn.find(StringConstants.lower_array_const_name);
                    constant_node lower_bound = ctn.const_defs[1].const_value;
                    constant_node upper_bound = ctn.const_defs[0].const_value;

                    ordinal_type_interface q = (ordinal_type_interface)lower_bound.type.get_internal_interface(internal_interface_kind.ordinal_interface);
                    ordinal_type_interface n = new ordinal_type_interface(q.inc_method, q.dec_method,
                        q.inc_value_method, q.dec_value_method, q.lower_eq_method, q.greater_eq_method,
                        q.lower_method, q.greater_method,
                        lower_bound, upper_bound, q.value_to_int, q.ordinal_type_to_int);

                    simple_array sa = ctn.fields[0].type as simple_array;

                    bounded_array_interface bai = new bounded_array_interface(n, sa.element_type, ctn.properties[0],
                        lower_bound.type, ctn.fields[0]);
                    ctn.add_internal_interface(bai);
                    //ctn.internal_type_special_kind = SemanticTree.type_special_kind.bounded_array;
                }
            }

            br.BaseStream.Seek(pos, SeekOrigin.Begin);
            return ctn;
        }

        public void ProcessWaitedToRestoreFields()
        {
            while (waited_types_to_restore_fields.Count > 0)
            {
                common_type_node ctn = waited_types_to_restore_fields[0];
                waited_types_to_restore_fields.Remove(ctn);
                RestoreAllFields(ctn);
            }
        }

        private void RestoreOperators(common_type_node ctn)
        {
            string[] mnames = class_names[ctn];
            WrappedClassScope wcs = ctn.scope as WrappedClassScope;
            foreach (string mname in mnames)
                wcs.RestoreMembers(mname);
        }

        private void RestoreAllFields(common_type_node ctn)
        {
            //восстанавливаем все методы
            string[] mnames = class_names[ctn];
            WrappedClassScope wcs = ctn.scope as WrappedClassScope;
            foreach (string mname in mnames)
                wcs.RestoreMembers(mname);
        }

        //добавление в таблицу символов имена членов класса
        private string[] AddClassMemberNames(WrappedClassScope scope)
        {
            int num = br.ReadInt32();
            string[] names = new string[num];
            for (int i = 0; i < num; i++)
            {
                PCUSymbolInfo si = new PCUSymbolInfo();
                string name = br.ReadString();
                si.sym_info = new wrapped_definition_node(br.ReadInt32(),this);
                //PCUReturner.AddPCUReader((wrapped_definition_node)si.sym_info, this);
                si.access_level = (access_level)br.ReadByte();

                si.symbol_kind = (symbol_kind)br.ReadByte();
                si.semantic_node_type = (semantic_node_type)br.ReadByte();
                si.always_restore = br.ReadBoolean();
                si.is_static = br.ReadBoolean();
                //Вроде это ненужно
                //SymbolInfo si2 = scope.FindWithoutCreation(name);
                //si.Next = si2;
                scope.AddSymbol(name, si);
                names[i] = name;
            }
            return names;
        }


        private compiled_type_node CreateCompiledTypeNode(int offset)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as compiled_type_node;
            compiled_type_node ctn = null;
            //br.ReadBoolean();
            //br.ReadInt32();
            bool is_interface = br.ReadBoolean();
            if (is_interface)//пропускаем флаг - интерфейсности
            {
                br.ReadInt32();
            }
            else
            {
                br.ReadString();
            }
            ctn = (compiled_type_node)GetTypeReference();
            ctn.Location = ReadDebugInfo();
            //members[offset] = ctn;
            AddMember(ctn, offset);

            //int_members.Add(ctn);
            return ctn;
        }

        private common_namespace_event CreateInterfaceNamespaceEvent(string name, int offset)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as common_namespace_event;
            common_namespace_event cne = null;
            cne = new common_namespace_event(name, null, cun.namespaces[0], null, null, null, null);
            br.ReadBoolean();
            br.ReadInt32();
            cne.set_event_type(GetTypeReference());
            if (CanReadObject())
                cne.set_add_function(GetNamespaceFunction(br.ReadInt32()));
            if (CanReadObject())
                cne.set_remove_function(GetNamespaceFunction(br.ReadInt32()));
            if (CanReadObject())
                cne.set_raise_function(GetNamespaceFunction(br.ReadInt32()));
            cne.field = GetNamespaceVariable(br.ReadInt32());
            br.ReadInt32();
            cne.loc = ReadDebugInfo();
            AddMember(cne, offset);
            int_members.Add(cne);
            return cne;
        }

        public common_namespace_event GetNamespaceEvent(int offset)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as common_namespace_event;
            common_namespace_event cne = null;
            int pos = (int)br.BaseStream.Position;
            br.BaseStream.Seek(start_pos + offset, SeekOrigin.Begin);
            br.ReadByte();
            string name;
            bool is_interface = br.ReadBoolean();
            if (is_interface)//пропускаем флаг - интерфейсности
            {
                name = GetString(br.ReadInt32());
            }
            else
            {
                name = br.ReadString();
            }
            cne = new common_namespace_event(name, null, cun.namespaces[0], null, null, null, null);
            cne.set_event_type(GetTypeReference());
            if (CanReadObject())
                cne.set_add_function(GetNamespaceFunction(br.ReadInt32()));
            if (CanReadObject())
                cne.set_remove_function(GetNamespaceFunction(br.ReadInt32()));
            if (CanReadObject())
                cne.set_raise_function(GetNamespaceFunction(br.ReadInt32()));
            cne.field = GetNamespaceVariable(br.ReadInt32());
            br.ReadInt32();
            cne.loc = ReadDebugInfo();
            AddMember(cne, offset);
            if (is_interface)
            {
                cne.namespace_node = cun.namespaces[0];
                int_members.Add(cne);
            }
            else
            {
                cne.namespace_node = cun.namespaces[1];
                impl_members.Add(cne);
            }
            br.BaseStream.Seek(pos, SeekOrigin.Begin);
            return cne;
        }

        private namespace_variable CreateInterfaceNamespaceVariable(string name, int offset)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as namespace_variable;
            namespace_variable nv = null;
            nv = new namespace_variable(name, null, cun.namespaces[0], null);
            int ind = br.ReadInt32();
            AddVarToOrderList(nv,ind);
            br.ReadBoolean();
            br.ReadInt32();
            nv.type = GetTypeReference();
            br.ReadInt32();//namespace
            nv.loc = ReadDebugInfo();
            if (CanReadObject())
                nv.inital_value = CreateExpressionWithOffset();
            //members[offset] = nv;
            AddMember(nv, offset);
            int_members.Add(nv);
            return nv;
        }

        public namespace_variable GetNamespaceVariable(int offset)
		{
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as namespace_variable;
			namespace_variable nv = null;
			int pos = (int)br.BaseStream.Position;
			br.BaseStream.Seek(start_pos+offset,SeekOrigin.Begin);
			br.ReadByte();
			int ind = br.ReadInt32();
			string name;
			bool is_interface = br.ReadBoolean();
			if (is_interface)//пропускаем флаг - интерфейсности
			{
				name = GetString(br.ReadInt32());
			}
			else
			{
				name = br.ReadString();
			}
            nv = new namespace_variable(name, null, null, null);
            if (is_interface)
            	AddVarToOrderList(nv,ind);
            else
            	AddImplVarToOrderList(nv,ind);
            nv.type = GetTypeReference();
            //members[offset] = nv;
            AddMember(nv, offset);
            br.ReadInt32();//namespace
			nv.loc = ReadDebugInfo();
            if (is_interface)
            {
                nv.namespace_node = cun.namespaces[0];
                int_members.Add(nv);
            }
            else
            {
                nv.namespace_node = cun.namespaces[1];
                impl_members.Add(nv);
            }
            if (CanReadObject())
                nv.inital_value = CreateExpressionWithOffset();
			br.BaseStream.Seek(pos,SeekOrigin.Begin);
            return nv;
		}

        private void ReadGenericFunctionInformation(common_function_node func)
        {
            if (CanReadObject())
            {
                func.generic_params = ReadGenericParams(cun.namespaces[0]);
                foreach (common_type_node par in func.generic_params)
                {
                    par.generic_function_container = func;
                    ReadTypeParameterEliminations(par);
                }
            }
        }

        private common_namespace_function_node CreateInterfaceNamespaceFunction(string name, int offset)
		{
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as common_namespace_function_node;
            common_namespace_function_node cnfn = null;
            cnfn = new common_namespace_function_node(name,null,cun.namespaces[0],null);
            //members[offset] = cnfn;
            AddMember(cnfn, offset);

            type_node ConnectedToType = null;
            br.ReadInt32();
            if (CanReadObject())
                ConnectedToType = GetTypeReference();
            if (br.ReadBoolean())
            {
                br.ReadInt32();
            }
            else
            {
                br.ReadString();
            }

            ReadGenericFunctionInformation(cnfn);
            //if (CanReadObject())
            //{
            //    cnfn.generic_params = ReadGenericParams(cun.namespaces[0]);
            //    foreach (common_type_node par in cnfn.generic_params)
            //    {
            //        par.generic_function_container = cnfn;
            //        ReadTypeParameterEliminations(par);
            //    }
            //}

            if (br.ReadByte() == 1)
			{
				cnfn.return_value_type = GetTypeReference();
				cnfn.return_variable = GetLocalVariable(cnfn);
                cnfn.var_definition_nodes_list.AddElement(cnfn.return_variable);
			}
			int num_par = br.ReadInt32();
			for (int i=0; i<num_par; i++)
				cnfn.parameters.AddElement(GetParameter(cnfn));
			br.ReadInt32(); //namespace
			cnfn.attributes.AddRange(GetAttributes());
			cnfn.is_forward = br.ReadBoolean();
			cnfn.is_overload = br.ReadBoolean();
			cnfn.num_of_default_variables = br.ReadInt32();
			cnfn.num_of_for_cycles = br.ReadInt32();
			int num_var = br.ReadInt32();
			if (cnfn.return_value_type != null) num_var--;
            GetVariables(cnfn, num_var);
            int num_consts = br.ReadInt32();
            for (int i = 0; i < num_consts; i++)
            {
                function_constant_definition fcd = GetFunctionConstant(cnfn);
                cnfn.constants.AddElement(fcd);
            }
            int num_nest_funcs = br.ReadInt32();
			for (int i=0; i<num_nest_funcs; i++)
				cnfn.functions_nodes_list.AddElement(GetNestedFunction());
			//br.ReadInt32();//code;
			cnfn.loc = ReadDebugInfo();
            cnfn.function_code = GetCode(br.ReadInt32());
			int_members.Add(cnfn);
            cnfn.ConnectedToType = ConnectedToType;
            if (cnfn.ConnectedToType != null && cnfn.ConnectedToType.type_special_kind == SemanticTree.type_special_kind.array_kind && cnfn.ConnectedToType.element_type.is_generic_parameter)
                cnfn.ConnectedToType.base_type.Scope.AddSymbol(cnfn.name, new SymbolInfo(cnfn));
            else if (cnfn.ConnectedToType != null && cnfn.ConnectedToType.is_generic_parameter)
                cnfn.ConnectedToType.base_type.Scope.AddSymbol(cnfn.name, new SymbolInfo(cnfn));
            else if (cnfn.ConnectedToType is compiled_type_node && cnfn.ConnectedToType.is_generic_type_instance && cnfn.ConnectedToType.original_generic.Scope != null)
                cnfn.ConnectedToType.original_generic.Scope.AddSymbol(cnfn.name, new SymbolInfo(cnfn));
            else if (cnfn.ConnectedToType is compiled_generic_instance_type_node && cnfn.ConnectedToType.original_generic.Scope != null)
            {
                cnfn.ConnectedToType.original_generic.Scope.AddSymbol(cnfn.name, new SymbolInfo(cnfn));
                if (cnfn.ConnectedToType != null && cnfn.ConnectedToType.IsDelegate && cnfn.ConnectedToType.base_type.IsDelegate)
                    compiled_type_node.get_type_node(typeof(Delegate)).Scope.AddSymbol(cnfn.name, new SymbolInfo(cnfn));
            }
            else if (cnfn.ConnectedToType != null && cnfn.ConnectedToType.IsDelegate && cnfn.ConnectedToType.base_type.IsDelegate)
                compiled_type_node.get_type_node(typeof(Delegate)).Scope.AddSymbol(cnfn.name, new SymbolInfo(cnfn));
            else if (cnfn.ConnectedToType != null && cnfn.ConnectedToType.type_special_kind == SemanticTree.type_special_kind.typed_file && cnfn.ConnectedToType.element_type.is_generic_parameter && SystemLibrary.SystemLibInitializer.TypedFileType.sym_info != null)
                (SystemLibrary.SystemLibInitializer.TypedFileType.sym_info as type_node).Scope.AddSymbol(cnfn.name, new SymbolInfo(cnfn));
            return cnfn;
		}


        private type_node GetSpecialTypeReference(int offset)
        {
            int pos = (int)br.BaseStream.Position;
            br.BaseStream.Seek(start_pos + offset, SeekOrigin.Begin);
            br.ReadByte();
            br.ReadInt32();
            type_node tn = null;
            if (CanReadObject())
                tn = GetTypeReference();
            br.BaseStream.Seek(pos, SeekOrigin.Begin);
            return tn;
        }

        private common_namespace_function_node GetNamespaceFunction(int offset, bool restore_code = true)
		{
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as common_namespace_function_node;
			common_namespace_function_node cnfn = null;

            int pos = (int)br.BaseStream.Position;
			br.BaseStream.Seek(start_pos+offset,SeekOrigin.Begin);
			br.ReadByte();
            int func_pos = start_pos + br.ReadInt32();
            int connected_to_type_pos = (int)br.BaseStream.Position;
            br.BaseStream.Seek(func_pos, SeekOrigin.Begin);
			string name;
            type_node ConnectedToType = null;
            /*if (CanReadObject())
                ConnectedToType = GetTypeReference();*/
			bool is_interface = br.ReadBoolean();
			if (is_interface)//пропускаем флаг - интерфейсности
			{
				name = GetString(br.ReadInt32());
			}
			else
			{
				name = br.ReadString();
			}
			if (is_interface)
			{
                cnfn = new common_namespace_function_node(name,null,cun.namespaces[0],null);
                int_members.Add(cnfn);
			}
			else
			{
                cnfn = new common_namespace_function_node(name,null,cun.namespaces[1],null);
                impl_members.Add(cnfn);
			}
            AddMember(cnfn, offset);
            ReadGenericFunctionInformation(cnfn);
            int cur_pos = (int)br.BaseStream.Position;
            br.BaseStream.Seek(connected_to_type_pos, SeekOrigin.Begin);
            if (CanReadObject())
                ConnectedToType = GetTypeReference();
            br.BaseStream.Seek(cur_pos, SeekOrigin.Begin);
            if (br.ReadByte() == 1)
			{
				cnfn.return_value_type = GetTypeReference();
				cnfn.return_variable = GetLocalVariable(cnfn);
				cnfn.var_definition_nodes_list.AddElement(cnfn.return_variable);
			}
            //members[offset] = cnfn;
			int num_par = br.ReadInt32();
			for (int i=0; i<num_par; i++)
				cnfn.parameters.AddElement(GetParameter(cnfn));
			br.ReadInt32(); //namespace
			cnfn.attributes.AddRange(GetAttributes());
			cnfn.is_forward = br.ReadBoolean();
			cnfn.is_overload = br.ReadBoolean();
			cnfn.num_of_default_variables = br.ReadInt32();
			cnfn.num_of_for_cycles = br.ReadInt32();
			int num_var = br.ReadInt32();
			if (cnfn.return_value_type != null) num_var--;
            GetVariables(cnfn, num_var);
            int num_consts = br.ReadInt32();
            for (int i = 0; i < num_consts; i++)
            {
            	cnfn.constants.AddElement(GetFunctionConstant(cnfn));
            }
            int num_nest_funcs = br.ReadInt32();
			for (int i=0; i<num_nest_funcs; i++)
				cnfn.functions_nodes_list.AddElement(GetNestedFunction());
			//br.ReadInt32();//code;
			cnfn.loc = ReadDebugInfo();
            cnfn.function_code = (restore_code /*|| cnfn.is_generic_function*/) ? GetCode(br.ReadInt32()) : new wrapped_function_body(this, br.ReadInt32());
            cnfn.ConnectedToType = ConnectedToType;
            if (cnfn.ConnectedToType != null && cnfn.ConnectedToType.type_special_kind == SemanticTree.type_special_kind.array_kind && cnfn.ConnectedToType.element_type.is_generic_parameter)
                cnfn.ConnectedToType.base_type.Scope.AddSymbol(cnfn.name, new SymbolInfo(cnfn));
            else if (cnfn.ConnectedToType != null && cnfn.ConnectedToType.is_generic_parameter)
                cnfn.ConnectedToType.base_type.Scope.AddSymbol(cnfn.name, new SymbolInfo(cnfn));
            else if (cnfn.ConnectedToType is compiled_type_node && cnfn.ConnectedToType.is_generic_type_instance && cnfn.ConnectedToType.original_generic.Scope != null)
                cnfn.ConnectedToType.original_generic.Scope.AddSymbol(cnfn.name, new SymbolInfo(cnfn));
            else if (cnfn.ConnectedToType is compiled_generic_instance_type_node && cnfn.ConnectedToType.original_generic.Scope != null)
            {
                cnfn.ConnectedToType.original_generic.Scope.AddSymbol(cnfn.name, new SymbolInfo(cnfn));
                if (cnfn.ConnectedToType != null && cnfn.ConnectedToType.IsDelegate && cnfn.ConnectedToType.base_type.IsDelegate)
                    compiled_type_node.get_type_node(typeof(Delegate)).Scope.AddSymbol(cnfn.name, new SymbolInfo(cnfn));
            }
            else if (cnfn.ConnectedToType != null && cnfn.ConnectedToType.IsDelegate && cnfn.ConnectedToType.base_type.IsDelegate)
                compiled_type_node.get_type_node(typeof(Delegate)).Scope.AddSymbol(cnfn.name, new SymbolInfo(cnfn));
            else if (cnfn.ConnectedToType != null && cnfn.ConnectedToType.type_special_kind == SemanticTree.type_special_kind.typed_file && (cnfn.ConnectedToType.element_type == null || cnfn.ConnectedToType.element_type.is_generic_parameter) && SystemLibrary.SystemLibInitializer.TypedFileType.sym_info != null)
                (SystemLibrary.SystemLibInitializer.TypedFileType.sym_info as type_node).Scope.AddSymbol(cnfn.name, new SymbolInfo(cnfn));
            br.BaseStream.Seek(pos,SeekOrigin.Begin);
			return cnfn;
		}

        private namespace_constant_definition CreateInterfaceConstantDefinition(string name, int offset)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as namespace_constant_definition;
            namespace_constant_definition ncd = null;
            br.ReadBoolean();
            br.ReadInt32();
            br.ReadInt32();//namespace
            type_node tn = GetTypeReference();
            constant_node en = (constant_node)CreateExpressionWithOffset();
            en.type = tn;
            location loc = ReadDebugInfo();
            ncd = new namespace_constant_definition(name,en,loc,cun.namespaces[0]);
            cun.namespaces[0].constants.AddElement(ncd);
            //???
            AddMember(ncd, offset);
            return ncd;
        }

        //ssyy
        private namespace_constant_definition GetConstantDefinition(int offset)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as namespace_constant_definition;
            namespace_constant_definition ncd = null;
            int pos = (int)br.BaseStream.Position;
            br.BaseStream.Seek(start_pos + offset, SeekOrigin.Begin);
            br.ReadByte();
            string name;
            bool is_interface = br.ReadBoolean();
            if (is_interface)//пропускаем флаг - интерфейсности
            {
                name = GetString(br.ReadInt32());
            }
            else
            {
                name = br.ReadString();
            }
            br.ReadInt32();//namespace
            type_node cnst_type = GetTypeReference();
            constant_node en = (constant_node)CreateExpressionWithOffset();
            en.type = cnst_type;
            location loc = ReadDebugInfo();
            ncd = new namespace_constant_definition(name, en, loc, cun.namespaces[(is_interface)?0:1]);
            cun.namespaces[(is_interface) ? 0 : 1].constants.AddElement(ncd);
            br.BaseStream.Seek(pos, SeekOrigin.Begin);
            //???
            AddMember(ncd, offset);
            return ncd;
        }
        //\ssyy

		private common_in_function_function_node GetNestedFunction()
		{
			int offset = (int)br.BaseStream.Position-start_pos;
			br.ReadByte();
			common_in_function_function_node cffn = new common_in_function_function_node(br.ReadString(),null,null,null);
			if (br.ReadByte() == 1)
			{
				cffn.return_value_type = GetTypeReference();
				cffn.return_variable = GetLocalVariable(cffn);
				cffn.var_definition_nodes_list.AddElement(cffn.return_variable);
			}
			int num_par = br.ReadInt32();
			for (int i=0; i<num_par; i++)
				cffn.parameters.AddElement(GetParameter(cffn));
			br.ReadInt32(); //namespace
			cffn.is_forward = br.ReadBoolean();
			cffn.is_overload = br.ReadBoolean();
			cffn.num_of_default_variables = br.ReadInt32();
			cffn.num_of_for_cycles = br.ReadInt32();
            //members[offset] = cffn;
            AddMember(cffn, offset);
			int num_var = br.ReadInt32();
			if (cffn.return_value_type != null) num_var--;
            GetVariables(cffn, num_var);
            int num_consts = br.ReadInt32();
            for (int i=0; i < num_consts; i++)
            {
            	cffn.constants.AddElement(GetFunctionConstant(cffn));
            }
            int num_nest_funcs = br.ReadInt32();
            for (int i = 0; i < num_nest_funcs; i++)
            {
                cffn.functions_nodes_list.AddElement(GetNestedFunction());
            }
            //br.ReadInt32();//code;
            cffn.loc = ReadDebugInfo();
            cffn.function_code = GetCode(br.ReadInt32());
			return cffn;
		}

        private Tuple<local_variable, int> GetLocalVariableLazy(common_function_node func)
        {
            int offset = (int)br.BaseStream.Position - start_pos;
            //int tmp=br.ReadByte();
            local_variable lv = new local_variable(br.ReadString(), func, null);
            lv.type = GetTypeReference();
            if (br.ReadBoolean()) lv.set_used_as_unlocal();
            //members[offset] = lv;
            AddMember(lv, offset);
            int pos = -1;
            if (CanReadObject())
            {
                pos = br.ReadInt32();
            }
            return new Tuple<local_variable,int>(lv, pos);
        }

        private local_variable GetLocalVariable(common_function_node func)
		{
			int offset = (int)br.BaseStream.Position-start_pos;
			//int tmp=br.ReadByte();
			local_variable lv = new local_variable(br.ReadString(),func,null);
			lv.type = GetTypeReference();
            if (br.ReadBoolean()) lv.set_used_as_unlocal();
            //members[offset] = lv;
            AddMember(lv, offset);
            if (CanReadObject())
                lv.inital_value = CreateExpressionWithOffset();
            return lv;
		}

        private function_constant_definition GetFunctionConstant(common_function_node func)
        {
        	int offset = (int)br.BaseStream.Position-start_pos;
			//int tmp=br.ReadByte();
			function_constant_definition fcd = new function_constant_definition(br.ReadString(),null,func);
			//members[offset] = lv;
            AddMember(fcd, offset);
            fcd.const_value = CreateExpressionWithOffset() as constant_node;
            return fcd;
        }

		private common_parameter GetParameter(common_function_node func)
		{
			int offset = (int)br.BaseStream.Position-start_pos;
            br.ReadByte();
			string s = br.ReadString();
            type_node tn = GetTypeReference();
			concrete_parameter_type cpt = (concrete_parameter_type)br.ReadByte();
            SemanticTree.parameter_type pt = SemanticTree.parameter_type.value;
            switch (cpt)
            {
                case concrete_parameter_type.cpt_const: pt = SemanticTree.parameter_type.value; break;
                case concrete_parameter_type.cpt_var: pt = SemanticTree.parameter_type.var; break;
                case concrete_parameter_type.cpt_none: pt = SemanticTree.parameter_type.value; break;
            }
            common_parameter p = new common_parameter(s,pt,func,cpt,null);
			p.type = tn;
            if (br.ReadBoolean()) p.set_used_as_unlocal();
            p.intrenal_is_params = br.ReadBoolean();
			if (br.ReadByte() == 1)
			{
				p.default_value = CreateExpressionWithOffset();
			}
			p.attributes.AddRange(GetAttributes());
            //members[offset] = p;
            if (members.ContainsKey(offset))
                return (common_parameter)members[offset];
            AddMember(p, offset);
			return p;
		}

        private common_parameter GetParameter()
        {
            int offset = (int)br.BaseStream.Position - start_pos;
            br.ReadByte();
            string s = br.ReadString();
            type_node tn = GetTypeReference();
            concrete_parameter_type cpt = (concrete_parameter_type)br.ReadByte();
            SemanticTree.parameter_type pt = SemanticTree.parameter_type.value;
            switch (cpt)
            {
                case concrete_parameter_type.cpt_const: pt = SemanticTree.parameter_type.value; break;
                case concrete_parameter_type.cpt_var: pt = SemanticTree.parameter_type.var; break;
                case concrete_parameter_type.cpt_none: pt = SemanticTree.parameter_type.value; break;
            }
            common_parameter p = new common_parameter(s, pt, null, cpt, null);
            p.type = tn;
            if (br.ReadBoolean()) p.set_used_as_unlocal();
            p.intrenal_is_params = br.ReadBoolean();
            if (br.ReadByte() == 1)
            {
                p.default_value = CreateExpressionWithOffset();
            }
            p.attributes.AddRange(GetAttributes());
            if (members.ContainsKey(offset))
                return (common_parameter)members[offset];
            AddMember(p, offset);
            return p;
        }

        private bool CanReadObject()
        {
            return br.ReadByte() == 1;
        }

        private try_block CreateTryBlock()
        {
            statement_node try_statements = CreateStatement();
            statement_node finally_statements = null;
            if (CanReadObject())
                finally_statements = CreateStatement();
            int filters_count = br.ReadInt32();
            exception_filters_list efl=new exception_filters_list();
            for (int i=0;i<filters_count;i++)
            {
                type_node filter_type = null;
                if (CanReadObject())
                    filter_type = GetTypeReference();
                local_block_variable_reference exception_var = null;
                if (CanReadObject())
                {
                    CreateLocalBlockVariable(null);
                    exception_var = (local_block_variable_reference)CreateLocalBlockVariableReference();
                }
                efl.AddElement(new exception_filter(filter_type,exception_var,CreateStatement(),ReadDebugInfo()));
            }
            return new try_block(try_statements, finally_statements, efl, null);
        }

		private statement_node CreateStatement()
		{
			semantic_node_type snt = (semantic_node_type)br.ReadByte();
            statement_node stmt = null;
            switch (snt) {
                case semantic_node_type.if_node: stmt = CreateIf(); break;
                case semantic_node_type.while_node: stmt = CreateWhile(); break;
                case semantic_node_type.repeat_node: stmt = CreateRepeat(); break;
                case semantic_node_type.for_node: stmt = CreateFor(); break;
                case semantic_node_type.statements_list: stmt = CreateStatementList(); break;
                case semantic_node_type.empty_statement: stmt = CreateEmpty(); break;
                case semantic_node_type.return_node: stmt = CreateReturnNode(); break;
                case semantic_node_type.switch_node: stmt = CreateSwitchNode(); break;
                case semantic_node_type.external_statement_node: stmt = CreateExternalStatement(); break;
                case semantic_node_type.throw_statement: stmt = CreateThrow(); break;
                case semantic_node_type.runtime_statement: stmt = CreateRuntimeStatement(); break;
                case semantic_node_type.try_block: stmt = CreateTryBlock(); break;
                case semantic_node_type.labeled_statement: stmt = CreateLabeledStatement(); break;
                case semantic_node_type.goto_statement: stmt = CreateGoto(); break;
                case semantic_node_type.foreach_node: stmt = CreateForeach(); break;
                case semantic_node_type.lock_statement: stmt = CreateLock(); break;
                case semantic_node_type.rethrow_statement: stmt = CreateRethrow(); break;
                case semantic_node_type.pinvoke_node : stmt = CreatePInvokeStatement(); break;
                default: stmt = CreateExpression(snt); break;
			}
            if (stmt != null)
            {
                stmt.location = ReadDebugInfo();
                return stmt;
            }
            throw new Exception("Unknown statement " + snt);
        }

		private statement_node CreatePInvokeStatement()
		{
			return new pinvoke_statement(null);
		}

		private statement_node CreateRethrow()
		{
			return new rethrow_statement_node(null);
		}

        private statement_node CreateForeach()
        {
            var_definition_node vdn = GetLocalOrNamespaceVariableByOffset(br.ReadInt32());
            expression_node expr = CreateExpression();
            statement_node body = CreateStatement();
            return new foreach_node(vdn, expr, body, null);
        }

        private statement_node CreateLock()
        {
            return new lock_statement(CreateExpression(), CreateStatement(), null);
        }

        private statement_node CreateLabeledStatement()
        {
            int label_offset = br.ReadInt32();
            label_node label = GetLabel(label_offset);
            location loc = ReadDebugInfo();
            statement_node stat = CreateStatement();
            return new labeled_statement(label, stat, loc);
        }

        private statement_node CreateGoto()
        {
            int label_offset = br.ReadInt32();
            label_node label = GetLabel(label_offset);
            location loc = ReadDebugInfo();
            return new goto_statement(label, loc);
        }

        private statement_node CreateThrow()
        {
            return new throw_statement_node(CreateExpression(), null);
        }

		private statement_node CreateIf()
		{
			expression_node expr = CreateExpression();
			statement_node then_body = CreateStatement();
			statement_node else_body=null;
			if (br.ReadByte() == 1) else_body = CreateStatement();
			if_node stmt = new if_node(expr,then_body,else_body,null);
			return stmt;
		}

		private statement_node CreateWhile()
		{
			while_node stmt = new while_node(CreateExpression(),CreateStatement(),null);
			return stmt;
		}

		private statement_node CreateRepeat()
		{
			repeat_node stmt = new repeat_node(CreateStatement(),CreateExpression(),null);
			return stmt;
		}

		private statement_node CreateFor()
		{
            statement_node init_stmt=null;
            if (CanReadObject())
                init_stmt = CreateStatement();
            expression_node while_expr = CreateExpression();
            expression_node init_while_expr = null;
            if (CanReadObject())
                init_while_expr = CreateExpression();
			for_node stmt = new for_node(init_stmt,while_expr,init_while_expr,CreateStatement(),CreateStatement(),null);
			stmt.bool_cycle = br.ReadBoolean();
			return stmt;
		}

        private statement_node CreateSwitchNode()
        {
            switch_node stmt = new switch_node(null);
            stmt.condition = CreateExpression();
            int case_nums = br.ReadInt32();
            for (int i = 0; i < case_nums; i++)
                stmt.case_variants.AddElement(CreateCaseVariant());
            if (br.ReadByte() == 1)
            {
                stmt.default_statement = CreateStatement();
            }
            return stmt;
        }

        private case_variant_node CreateCaseVariant()
        {
            case_variant_node cvn = new case_variant_node(null);
            int const_num = br.ReadInt32();
            for (int i = 0; i < const_num; i++)
                cvn.case_constants.AddElement((int_const_node)CreateExpression());
            int range_num = br.ReadInt32();
            for (int i = 0; i < range_num; i++)
                cvn.case_ranges.AddElement(new case_range_node((int_const_node)CreateExpression(), (int_const_node)CreateExpression(), null));
            cvn.case_statement = CreateStatement();
            return cvn;
        }

        private runtime_statement CreateRuntimeStatement()
        {
            return new runtime_statement((SemanticTree.runtime_statement_type)br.ReadInt32(),null);
        }

        private external_statement CreateExternalStatement()
        {
            return new external_statement(br.ReadString(), br.ReadString(), null);
        }

        private statement_node CreateStatementList()
		{
            statements_list stmt = new statements_list(null);
            int num = br.ReadInt32();
            for (int i = 0; i < num; i++)
                stmt.local_variables.Add(CreateLocalBlockVariable(stmt));
            num = br.ReadInt32();
			for (int i=0; i<num; i++)
				stmt.statements.AddElement(CreateStatement());
            stmt.LeftLogicalBracketLocation = ReadDebugInfo();
            stmt.RightLogicalBracketLocation = ReadDebugInfo();
			return stmt;
		}

        private local_block_variable CreateLocalBlockVariable(statements_list stmt)
        {
            int offset = (int)br.BaseStream.Position - start_pos;
            string name = br.ReadString();
            type_node tn = GetTypeReference();
            expression_node initv = null;
            local_block_variable lv = new local_block_variable(name, tn, stmt, null);
            if (members.ContainsKey(offset))
            {
                lv = members[offset] as local_block_variable;
                if (CanReadObject())
                    br.ReadInt32();
                ReadDebugInfo();
            }
            else
            {
                AddMember(lv, offset);
                if (CanReadObject())
                    initv = CreateExpressionWithOffset();
                lv.loc = ReadDebugInfo();
                lv.inital_value = initv;
            }

            return lv;

        }

        public expression_node GetExpression(int offset)
        {
            int tmp = (int)br.BaseStream.Position;
            br.BaseStream.Seek(offset, SeekOrigin.Begin);
            expression_node expr = CreateExpressionWithOffset();
            br.BaseStream.Seek(tmp, SeekOrigin.Begin);
            return expr;
        }

		private statement_node CreateEmpty()
		{
			return new empty_statement(null);
		}

        private is_node CreateIsNode()
        {
            return new is_node(CreateExpression(), GetTypeReference(), null);
        }

        private as_node CreateAsNode()
        {
            return new as_node(CreateExpression(), GetTypeReference(), null);
        }

        private typeof_operator CreateTypeOfOperator()
        {
            return new typeof_operator(GetTypeReference(), null);
        }

        private sizeof_operator CreateSizeOfOperator()
        {
            return new sizeof_operator(GetTypeReference(), null);
        }

        private exit_procedure CreateExitProcedure()
        {
            return new exit_procedure(null);
        }

        private array_const CreateArrayConst()
        {
            List<constant_node> element_values = new List<constant_node>();
            int count = br.ReadInt32();
            for (int i = 0; i < count; i++)
                element_values.Add((constant_node)CreateExpression());
            array_const arrc = new array_const(element_values, null);
            arrc.SetType(GetTypeReference());
            return arrc;
        }

        private array_initializer CreateArrayInitializer()
        {
        	List<expression_node> element_values = new List<expression_node>();
            int count = br.ReadInt32();
            for (int i = 0; i < count; i++)
                element_values.Add(CreateExpression());
            array_initializer arrc = new array_initializer(element_values, null);
            arrc.type = GetTypeReference();
            return arrc;
        }

        private record_initializer CreateRecordInitializer()
        {
        	List<expression_node> element_values = new List<expression_node>();
            int count = br.ReadInt32();
            for (int i = 0; i < count; i++)
                element_values.Add(CreateExpression());
            record_initializer rec = new record_initializer(element_values, null);
            rec.type = GetTypeReference();
            return rec;
        }

        private record_constant CreateRecordConst()
        {
            List<constant_node> field_values = new List<constant_node>();
            int count = br.ReadInt32();
            for (int i = 0; i < count; i++)
                field_values.Add((constant_node)CreateExpression());
            record_constant recc = new record_constant(field_values, null);
            recc.SetType(GetTypeReference());
            return recc;
        }

        private question_colon_expression CreateQuestionColonExpression()
        {
            return new question_colon_expression(CreateExpression(), CreateExpression(), CreateExpression(), null);
        }

        private double_question_colon_expression CreateDoubleQuestionColonExpression()
        {
            return new double_question_colon_expression(CreateExpression(), CreateExpression(), null);
        }

        private statements_expression_node CreateStatementsExpressionNode()
        {
            statement_node_list sl = new statement_node_list();
            int count = br.ReadInt32();
            for (int i = 0; i < count; i++)
                sl.AddElement(CreateStatement());
            return new statements_expression_node(sl, CreateExpression(), null);
        }

        private float_const_node CreateFloatConst()
        {
            return new float_const_node(br.ReadSingle(), null);
        }

        private compiled_static_method_call_as_constant CreateCompiledStaticMethodCallNodeAsConstant()
        {
            return new compiled_static_method_call_as_constant(CreateCompiledStaticMethodCall(), null);
        }

        private common_static_method_call_as_constant CreateCommonStaticMethodCallNodeAsConstant()
        {
            return new common_static_method_call_as_constant(CreateStaticMethodCall(), null);
        }

        private common_namespace_function_call_as_constant CreateCommonNamespaceFunctionCallNodeAsConstant()
        {
            common_namespace_function_call_as_constant cnfcc = new common_namespace_function_call_as_constant((common_namespace_function_call)CreateCommonNamespaceFunctionCall(), null);
            cnfcc.type = GetTypeReference();
            return cnfcc;
        }

        private compiled_constructor_call_as_constant CreateCompiledConstructorCallAsConstant()
        {
            return new compiled_constructor_call_as_constant(CreateCompiledConstructorCall(), null);
        }

        private default_operator_node_as_constant CreateDefaultOperatorAsConstant()
        {
            return new default_operator_node_as_constant(CreateDefaultOperator(), null);
        }

        private sizeof_operator_as_constant CreateSizeOfOperatorAsConstant()
        {
            return new sizeof_operator_as_constant(CreateSizeOfOperator(), null);
        }

        private expression_node CreateExpression(semantic_node_type snt)
		{
            //location loc = ReadDebugInfo();
			switch (snt) {
                case semantic_node_type.exit_procedure:
                    return CreateExitProcedure();
                case semantic_node_type.typeof_operator:
                    return CreateTypeOfOperator();
                case semantic_node_type.statement_expression_node:
                    return CreateStatementsExpressionNode();
                case semantic_node_type.question_colon_expression:
                    return CreateQuestionColonExpression();
                case semantic_node_type.double_question_colon_expression:
                    return CreateDoubleQuestionColonExpression();
                case semantic_node_type.sizeof_operator:
                    return CreateSizeOfOperator();
                case semantic_node_type.is_node:
                    return CreateIsNode();
                case semantic_node_type.as_node:
                    return CreateAsNode();
                case semantic_node_type.compiled_static_method_call_node_as_constant:
                    return CreateCompiledStaticMethodCallNodeAsConstant();
                case semantic_node_type.common_static_method_call_node_as_constant:
                    return CreateCommonStaticMethodCallNodeAsConstant();
                case semantic_node_type.common_namespace_function_call_node_as_constant:
                    return CreateCommonNamespaceFunctionCallNodeAsConstant();
                case semantic_node_type.compiled_constructor_call_as_constant:
                    return CreateCompiledConstructorCallAsConstant();
                case semantic_node_type.default_operator_node_as_constant:
                    return CreateDefaultOperatorAsConstant();
                case semantic_node_type.sizeof_operator_as_constant:
                    return CreateSizeOfOperatorAsConstant();
                case semantic_node_type.array_const:
                    return CreateArrayConst();
                case semantic_node_type.record_const:
                    return CreateRecordConst();
                case semantic_node_type.float_const_node:
                    return CreateFloatConst();
                case semantic_node_type.byte_const_node:
                    return CreateByteConstNode();
                case semantic_node_type.int_const_node:
                    return CreateIntConstNode();
                case semantic_node_type.sbyte_const_node:
                    return CreateSByteConstNode();
                case semantic_node_type.short_const_node:
                    return CreateShortConstNode();
                case semantic_node_type.ushort_const_node:
                    return CreateUShortConstNode();
                case semantic_node_type.uint_const_node:
                    return CreateUIntConstNode();
                case semantic_node_type.ulong_const_node:
                    return CreateULongConstNode();
                case semantic_node_type.long_const_node:
                    return CreateLongConstNode();
                case semantic_node_type.double_const_node:
                    return CreateDoubleConstNode();
                case semantic_node_type.char_const_node:
                    return CreateCharConstNode();
                case semantic_node_type.bool_const_node:
                    return CreateBoolConstNode();
                case semantic_node_type.string_const_node:
                    return CreateStringConstNode();
                case semantic_node_type.local_variable_reference:
                    return CreateLocalVariableReference();
                case semantic_node_type.local_block_variable_reference:
                    return CreateLocalBlockVariableReference();
                case semantic_node_type.namespace_variable_reference:
                    return CreateNamespaceVariableReference();
                case semantic_node_type.namespace_constant_reference:
                    return CreateNamespaceConstantReference();
                case semantic_node_type.function_constant_reference:
                    return CreateFunctionConstantReference();
                case semantic_node_type.basic_function_call:
                    return CreateBasicFunctionCall();
                case semantic_node_type.common_parameter_reference:
                    return CreateCommonParameterReference();
                case semantic_node_type.common_namespace_function_call:
                    return CreateCommonNamespaceFunctionCall();
                case semantic_node_type.common_in_function_function_call:
                    return CreateCommonInFuncFuncCall();
                case semantic_node_type.compiled_static_method_call:
                    return CreateCompiledStaticMethodCall();
                case semantic_node_type.while_break_node:
                    return CreateWhileBreakNode();
                case semantic_node_type.while_continue_node:
                    return CreateWhileContinueNode();
                case semantic_node_type.for_break_node:
                    return CreateForBreakNode();
                case semantic_node_type.for_continue_node:
                    return CreateForContinueNode();
                case semantic_node_type.repeat_break_node:
                    return CreateRepeatBreakNode();
                case semantic_node_type.repeat_continue_node:
                    return CreateRepeatContinueNode();
                case semantic_node_type.compiled_function_call:
                    return CreateCompiledFunctionCall();
                case semantic_node_type.compiled_constructor_call:
                    return CreateCompiledConstructorCall();
                case semantic_node_type.compiled_variable_reference:
                    return CreateCompiledVariableReference();
                case semantic_node_type.static_compiled_variable_reference:
                    return CreateStaticCompiledVariableReference();
                case semantic_node_type.simple_array_indexing:
                    return CreateSimpleArrayIndexing();
                case semantic_node_type.class_field_reference:
                    return CreateClassFieldReference();
                case semantic_node_type.common_constructor_call:
                    return CreateCommonConstructorCall();
                case semantic_node_type.common_method_call:
                    return CreateMethodCall();
                case semantic_node_type.common_static_method_call:
                    return CreateStaticMethodCall();
                case semantic_node_type.non_static_property_reference:
                    return CreateNonStaticPropertyReference();
                case semantic_node_type.static_class_field_reference:
                    return CreateStaticClassFieldReference();
                case semantic_node_type.this_node:
                    return CreateThisNode();
                case semantic_node_type.deref_node:
                    return CreateDerefNode();
                case semantic_node_type.get_addr_node:
                    return CreateGetAddrNode();
                case semantic_node_type.null_const_node:
                    return CreateNullConstNode();
                case semantic_node_type.enum_const:
                    return CreateEnumConstNode();
                case semantic_node_type.foreach_break_node:
                    return CreateForeachBreakNode();
                case semantic_node_type.foreach_continue_node:
                    return CreateForeachContinueNode();
                case semantic_node_type.common_constructor_call_as_constant:
                    return CreateCommonConstructorCallAsConstant();
                case semantic_node_type.basic_function_call_node_as_constant:
                    return CreateBasicFunctionCallAsConstant();
                case semantic_node_type.array_initializer:
                    return CreateArrayInitializer();
                case semantic_node_type.record_initializer:
                    return CreateRecordInitializer();
                case semantic_node_type.default_operator:
                    return CreateDefaultOperator();
                case semantic_node_type.compiled_static_field_reference_as_constant:
                    return CreateCompiledStaticFieldReferenceAsConstant();
                case semantic_node_type.nonstatic_event_reference:
                    return CreateNonStaticEventReference();
                case semantic_node_type.static_event_reference:
                    return CreateStaticEventReference();
			}
			throw new Exception("Unknown expression "+snt);
		}

        private default_operator_node CreateDefaultOperator()
        {
        	return new default_operator_node(GetTypeReference(),null);
        }

        private expression_node CreateBasicFunctionCallAsConstant()
        {
        	return new basic_function_call_as_constant(CreateBasicFunctionCall() as basic_function_call,null);
        }

        private expression_node CreateCommonConstructorCallAsConstant()
        {
        	return new common_constructor_call_as_constant(CreateCommonConstructorCall() as common_constructor_call,null);
        }

        private expression_node CreateCompiledStaticFieldReferenceAsConstant()
        {
            return new compiled_static_field_reference_as_constant(CreateStaticCompiledVariableReference(), null);
        }

		private expression_node CreateExpression()
		{
			return CreateExpression((semantic_node_type)br.ReadByte());
		}

		private expression_node CreateExpressionWithOffset()
		{
			int pos = br.ReadInt32();
			int tmp = (int)br.BaseStream.Position;
			br.BaseStream.Seek(start_pos+pos, SeekOrigin.Begin);
			expression_node en = CreateExpression();
			br.BaseStream.Seek(tmp, SeekOrigin.Begin);
			return en;
		}

        private expression_node CreateExpressionWithOffset(int pos)
        {
            int tmp = (int)br.BaseStream.Position;
            br.BaseStream.Seek(start_pos + pos, SeekOrigin.Begin);
            expression_node en = CreateExpression();
            br.BaseStream.Seek(tmp, SeekOrigin.Begin);
            return en;
        }

        private expression_node CreateEnumConstNode()
        {
            return new enum_const_node(br.ReadInt32(),GetTypeReference(),null);
        }

        //считывание константы nil
        private expression_node CreateNullConstNode()
        {
            bool has_type = br.ReadByte() == 1;
            type_node tn = null_type_node.get_type_node();
            if (has_type)
                tn = GetTypeReference();
            return new null_const_node(tn, null);
        }

        private expression_node CreateNonStaticEventReference()
        {
            return new nonstatic_event_reference(CreateExpression(), GetEventByOffset(), null);
        }

        private expression_node CreateStaticEventReference()
        {
            return new static_event_reference(GetEventByOffset(), null);
        }

        private expression_node CreateStaticClassFieldReference()
        {
            return new static_class_field_reference(GetClassFieldByOffset(), null);
        }

        private expression_node CreateGetAddrNode()
        {
            return new get_addr_node(CreateExpression(), null);
        }

        private expression_node CreateDerefNode()
        {
            return new dereference_node(CreateExpression(), null);
        }

        private expression_node CreateThisNode()
        {
            return new this_node(GetTypeReference(),null);
        }

        private expression_node CreateNonStaticPropertyReference()
        {
            expression_node obj = CreateExpression();
            property_node prop = GetPropertyByOffset();
            non_static_property_reference nspr = new non_static_property_reference(prop,obj,null);
            int num = br.ReadInt32();
            for (int i = 0; i < num; i++)
                nspr.fact_parametres.AddElement(CreateExpression());
            return nspr;
        }

        private common_static_method_call CreateStaticMethodCall()
        {
        	common_method_node meth = GetMethodByOffset();
            common_static_method_call cmc = new common_static_method_call(meth,null);
            //ssyy
            cmc.last_result_function_call = br.ReadByte() == 1;
            int num = br.ReadInt32();
            for (int i = 0; i < num; i++)
                cmc.parameters.AddElement(CreateExpression());
            return cmc;
        }

        private expression_node CreateMethodCall()
        {
            expression_node obj = CreateExpression();
            common_method_node meth = GetMethodByOffset();
            common_method_call cmc = new common_method_call(meth, obj, null);
            //ssyy
            cmc.last_result_function_call = br.ReadByte() == 1;
            //\ssyy
            cmc.virtual_call = br.ReadBoolean();
            int num = br.ReadInt32();
            for (int i = 0; i < num; i++)
                cmc.parameters.AddElement(CreateExpression());
            return cmc;
        }

        private expression_node CreateCommonConstructorCall()
        {
            common_method_node meth = GetMethodByOffset();
            //ssyy moved up
            common_constructor_call cmc = new common_constructor_call(meth, null);
            //\ssyy
            //ssyy добавил
            cmc._new_obj_awaited = (br.ReadByte() == 1);
            //\ssyy
            int num = br.ReadInt32();
            for (int i = 0; i < num; i++)
                cmc.parameters.AddElement(CreateExpression());
            return cmc;
        }

        private expression_node CreateClassFieldReference()
        {
            expression_node obj = CreateExpression();
            class_field field = GetClassFieldByOffset();
            return new class_field_reference(field, obj, null);
        }

        private expression_node CreateByteConstNode()
        {
            return new byte_const_node(br.ReadByte(), null);
        }

        private expression_node CreateIntConstNode()
		{
			return new int_const_node(br.ReadInt32(),null);
		}

        private expression_node CreateSByteConstNode()
        {
            return new sbyte_const_node(br.ReadSByte(), null);
        }

        private expression_node CreateShortConstNode()
        {
            return new short_const_node(br.ReadInt16(), null);
        }

        private expression_node CreateUShortConstNode()
        {
            return new ushort_const_node(br.ReadUInt16(), null);
        }

        private expression_node CreateUIntConstNode()
        {
            return new uint_const_node(br.ReadUInt32(), null);
        }

        private expression_node CreateULongConstNode()
        {
            return new ulong_const_node(br.ReadUInt64(), null);
        }

        private expression_node CreateLongConstNode()
        {
            return new long_const_node(br.ReadInt64(), null);
        }

		private expression_node CreateDoubleConstNode()
		{
			return new double_const_node(br.ReadDouble(),null);
		}

		private expression_node CreateCharConstNode()
		{
			return new char_const_node(br.ReadChar(),null);
		}

		private expression_node CreateBoolConstNode()
		{
			return new bool_const_node(br.ReadBoolean(),null);
		}

		private expression_node CreateStringConstNode()
		{
            string s = br.ReadString();
			return new string_const_node(s,null);
		}

		private expression_node CreateLocalVariableReference()
		{
            int off = 0;
            try
            {
                off = br.ReadInt32();
                local_variable lv = GetLocalVariableByOffset(off);
                local_variable_reference lvr = new local_variable_reference(lv, 0, null);
                return lvr;
            }
			catch
            {
                throw;
            }
		}

        private expression_node CreateLocalBlockVariableReference()
        {
            local_block_variable lv = GetLocalBlockVariableByOffset(br.ReadInt32());
            local_block_variable_reference lvr = new local_block_variable_reference(lv, null);
            return lvr;
        }

        private expression_node CreateNamespaceConstantReference()
        {
        	if (br.ReadByte() == 1)
			{
				namespace_constant_definition nv = GetNamespaceConstantByOffset(br.ReadInt32());
				namespace_constant_reference nvr = new namespace_constant_reference(nv,null);
				return nvr;
			}
			else
			{
				namespace_constant_definition nv = GetExtNamespaceConstantByOffset(br.ReadInt32());
				namespace_constant_reference nvr = new namespace_constant_reference(nv,null);
				return nvr;
			}
        }

        private expression_node CreateFunctionConstantReference()
        {
        	function_constant_definition lv = GetFunctionConstantByOffset(br.ReadInt32());
			function_constant_reference lvr = new function_constant_reference(lv,null);
			return lvr;
        }

        private expression_node CreateNamespaceVariableReference()
		{
			if (br.ReadByte() == 1)
			{
				namespace_variable nv = GetNamespaceVariableByOffset(br.ReadInt32());
				namespace_variable_reference nvr = new namespace_variable_reference(nv,null);
				return nvr;
			}
			else
			{
				namespace_variable nv = GetExtNamespaceVariableByOffset(br.ReadInt32());
				namespace_variable_reference nvr = new namespace_variable_reference(nv,null);
				return nvr;
			}
		}

		private expression_node CreateBasicFunctionCall()
		{
            SemanticTree.basic_function_type bft = (SemanticTree.basic_function_type)br.ReadInt16();
            basic_function_node bfn = PascalABCCompiler.SystemLibrary.SystemLibrary.find_operator(bft);
            if (bfn == null) throw new CompilerInternalError("[PCUREADER] Function not defined: "+bft.ToString());//Console.WriteLine(bft);

            type_node _tn = GetTypeReference();
            type_node _conversion_tn = GetTypeReference();
            br.ReadInt32();
            basic_function_call bfc = new basic_function_call(bfn,null);
            bfc.ret_type = _tn;
            bfc.conversion_type = _conversion_tn;
            int num_param = bfn.parameters.Count;
            for (int i=0; i<num_param; i++)
                bfc.parameters.AddElement(CreateExpression());
            return bfc;
		}

		private expression_node CreateCommonParameterReference()
		{
			common_parameter cp = GetParameterByOffset(br.ReadInt32());
			common_parameter_reference cpr = new common_parameter_reference(cp,0,null);
			return cpr;
		}

        private common_namespace_function_node GetNamespaceFunctionByOffset()
        {
            byte is_def = br.ReadByte();
            switch (is_def)
            {
                case 0:
                    return GetExtNamespaceFunctionByOffset(br.ReadInt32());
                case 1:
                    return GetNamespaceFunctionByOffset(br.ReadInt32());
                case 2:
                    SemanticTree.SpecialFunctionKind sfk = (SemanticTree.SpecialFunctionKind)br.ReadByte();
                    switch (sfk)
                    {
                        case SemanticTree.SpecialFunctionKind.New:
                            return SystemLibrary.SystemLibInitializer.NewProcedureDecl;
                        case SemanticTree.SpecialFunctionKind.Dispose:
                            return SystemLibrary.SystemLibInitializer.DisposeProcedureDecl;
                        case SemanticTree.SpecialFunctionKind.NewArray:
                            return SystemLibrary.SystemLibInitializer.NewArrayProcedureDecl;
                        default:
                            throw new CompilerInternalError("CreateCommonNamespaceFunctionCall: SpecialFunctionKind BAD");
                    }
                case PCUConsts.common_namespace_generic:
                    return GetGenericNamespaceFunctionReference();
            }
            return null;
        }

		private expression_node CreateCommonNamespaceFunctionCall()
		{
            common_namespace_function_node cnf = GetNamespaceFunctionByOffset();
            common_namespace_function_call cnfc = null;
			int num_param = br.ReadInt32();
			cnfc = new common_namespace_function_call(cnf,null);
			for (int i=0; i<num_param; i++)
				cnfc.parameters.AddElement(CreateExpression());
			return cnfc;
		}

		private expression_node CreateCommonInFuncFuncCall()
		{
			common_in_function_function_node cff = GetNestedFunctionByOffset(br.ReadInt32());
			int num_param = br.ReadInt32();
			common_in_function_function_call cffc = new common_in_function_function_call(cff,0,null);
			for (int i=0; i<num_param; i++)
				cffc.parameters.AddElement(CreateExpression());
			return cffc;
		}

        private var_definition_node GetLocalOrNamespaceVariableByOffset(int offset)
        {
            var_definition_node vdn = GetLocalOrBlockVariableByOffset(offset);
            if (vdn != null) return vdn;
            return GetNamespaceVariableByOffset(offset);
        }

        private var_definition_node GetLocalOrBlockVariableByOffset(int offset)
        {
        	return (var_definition_node)members[offset];
        }

        private local_variable GetLocalVariableByOffset(int offset)
		{
			return (local_variable)members[offset];
		}

        private function_constant_definition GetFunctionConstantByOffset(int offset)
        {
        	return (function_constant_definition)members[offset];
        }

        private local_block_variable GetLocalBlockVariableByOffset(int offset)
        {
            if (!members.ContainsKey(offset))
            {
                int tmp = (int)br.BaseStream.Position;
                br.BaseStream.Seek(start_pos + offset, SeekOrigin.Begin);
                var loc_var = CreateLocalBlockVariable(null);
                br.BaseStream.Seek(tmp, SeekOrigin.Begin);
            }
            return (local_block_variable)members[offset];
        }

		private common_parameter GetParameterByOffset(int offset)
		{
            if (!members.ContainsKey(offset))
            {
                int tmp = (int)br.BaseStream.Position;
                br.BaseStream.Seek(start_pos + offset, SeekOrigin.Begin);
                var loc_param = GetParameter();
                br.BaseStream.Seek(tmp, SeekOrigin.Begin);
            }
			return (common_parameter)members[offset];
		}

		private namespace_variable GetNamespaceVariableByOffset(int offset)
		{
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as namespace_variable;
			namespace_variable nv = GetNamespaceVariable(offset);
			return nv;
		}

		private namespace_variable GetExtNamespaceVariableByOffset(int offset)
		{
            definition_node dn = null;
            if (ext_members.TryGetValue(offset, out dn))
                return dn as namespace_variable;
            namespace_variable nv = null;
			{
				int tmp = (int)br.BaseStream.Position;
				br.BaseStream.Seek(ext_pos+offset,SeekOrigin.Begin);
				nv = ReadExtNamespaceVariable();
				br.BaseStream.Seek(tmp,SeekOrigin.Begin);
			}
			return nv;
		}

		private namespace_constant_definition GetNamespaceConstantByOffset(int offset)
		{
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as namespace_constant_definition;
			namespace_constant_definition nv = GetConstantDefinition(offset);
			return nv;
		}

		private namespace_constant_definition GetExtNamespaceConstantByOffset(int offset)
		{
            definition_node dn = null;
            if (ext_members.TryGetValue(offset, out dn))
                return dn as namespace_constant_definition;
            namespace_constant_definition nv = null;
			{
				int tmp = (int)br.BaseStream.Position;
				br.BaseStream.Seek(ext_pos+offset,SeekOrigin.Begin);
				nv = ReadExtNamespaceConstant();
				br.BaseStream.Seek(tmp,SeekOrigin.Begin);
			}
			return nv;
		}

		private common_namespace_function_node GetNamespaceFunctionByOffset(int offset)
		{
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as common_namespace_function_node;
			common_namespace_function_node cnf = GetNamespaceFunction(offset, false);
			return cnf;
		}

		private common_namespace_function_node GetExtNamespaceFunctionByOffset(int offset)
		{
            definition_node dn = null;
            if (ext_members.TryGetValue(offset, out dn))
                return dn as common_namespace_function_node;
            common_namespace_function_node cnf = null;
			{
				int tmp = (int)br.BaseStream.Position;
				br.BaseStream.Seek(ext_pos+offset,SeekOrigin.Begin);
				cnf = ReadCommonExtNamespaceFunc();
				br.BaseStream.Seek(tmp,SeekOrigin.Begin);
			}
			return cnf;
		}

        private common_method_node GetMethodByOffset()
        {
            byte is_def = br.ReadByte();
            switch (is_def)
            {
                case 1:
                    return GetMethodByOffset(br.ReadInt32());
                case 0:
                    return GetExtMethodByOffset(br.ReadInt32());
                case PCUConsts.template_meth:
                    return GetTemplateInstanceMethod();
                case PCUConsts.generic_meth:
                    return GetGenericInstanceMethod();
                case PCUConsts.generic_ctor:
                    return GetGenericInstanceConstructor();
                case PCUConsts.generic_param_ctor:
                    return GetGenericParamConstructor();
                case PCUConsts.compiled_method_generic:
                    return GetCompiledGenericMethodInstanceReference() as common_method_node;
                case PCUConsts.common_method_generic:
                    return GetCommonGenericMethodInstanceReference();
            }
            return null;
        }

        //(ssyy) Получение конструктора аргумента generic-типа
        private common_method_node GetGenericParamConstructor()
        {
            //br.ReadByte();
            common_type_node cnode = GetTypeReference() as common_type_node;//GetGenericParameter();
            return cnode.methods[0];
        }

        //(ssyy) Получение метода инстанции generic-типа
        private common_method_node GetGenericInstanceMethod()
        {
            br.ReadByte(); //Пропускаем знак инстанции generic-типа
            function_node fn;
            generic_instance_type_node gitn = GetGenericInstance();
            if (gitn is compiled_generic_instance_type_node)
            {
                fn = GetCompiledMethod(br.ReadInt32());
            }
            else
            {
                fn = GetMethodByOffset();
            }
            return gitn.ConvertMember(fn) as common_method_node;
        }

        private common_method_node GetGenericInstanceConstructor()
        {
            br.ReadByte(); //Пропускаем знак инстанции generic-типа
            generic_instance_type_node gitn = GetGenericInstance();
            compiled_constructor_node ccn = GetCompiledConstructor(br.ReadInt32());
            return gitn.ConvertMember(ccn) as common_method_node;
        }

        //(ssyy) Получение метода инстанции шаблона
        private common_method_node GetTemplateInstanceMethod()
        {
            br.ReadByte(); //Пропускаем знак инстанции шаблона
            common_type_node ctnode = GetTemplateInstance();
            int meth_num = br.ReadInt32();
            return ctnode.methods[meth_num];
        }

        //(ssyy) Получение поля инстанции шаблона
        private class_field GetTemplateInstanceField()
        {
            br.ReadByte(); //Пропускаем знак инстанции шаблона
            common_type_node ctnode = GetTemplateInstance();
            int field_num = br.ReadInt32();
            return ctnode.fields[field_num];
        }

        //(ssyy) Получение поля инстанции generic-типа
        private class_field GetGenericInstanceField()
        {
            br.ReadByte(); //Пропускаем знак инстанции шаблона
            var_definition_node ccn;
            generic_instance_type_node gitn = GetGenericInstance();
            if (gitn is compiled_generic_instance_type_node)
            {
                ccn = GetCompiledVariable(br.ReadInt32());
            }
            else
            {
                ccn = GetClassFieldByOffset();
            }
            return gitn.ConvertMember(ccn) as class_field;
        }

        //(ssyy) Получение метода инстанции generic-типа
        private common_property_node GetGenericInstanceProperty()
        {
            br.ReadByte(); //Пропускаем знак инстанции generic-типа
            property_node pn;
            generic_instance_type_node gitn = GetGenericInstance();
            if (gitn is compiled_generic_instance_type_node)
            {
                pn = GetCompiledProperty(br.ReadInt32());
            }
            else
            {
                pn = GetPropertyByOffset();
            }
            return gitn.ConvertMember(pn) as common_property_node;
        }

        //(ssyy) Получение свойства инстанции шаблона
        private common_property_node GetTemplateInstanceProperty()
        {
            br.ReadByte(); //Пропускаем знак инстанции шаблона
            common_type_node ctnode = GetTemplateInstance();
            int prop_num = br.ReadInt32();
            return ctnode.properties[prop_num];
        }

        private common_method_node GetExtMethodByOffset(int offset)
        {
            definition_node dn = null;
            if (ext_members.TryGetValue(offset, out dn))
                return dn as common_method_node;
            common_method_node meth = null;
            {
                int tmp = (int)br.BaseStream.Position;
                br.BaseStream.Seek(ext_pos + offset, SeekOrigin.Begin);
                meth = ReadCommonExtMethod();
                br.BaseStream.Seek(tmp, SeekOrigin.Begin);
            }
            return meth;
        }

        private common_method_node GetMethodByOffset(int offset)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as common_method_node;
            common_method_node cmn = GetClassMethod(offset);
            return cmn;
        }

        private common_property_node GetPropertyByOffset()
        {
            byte is_def = br.ReadByte();
            switch (is_def)
            {
                case 1:
                    return GetPropertyByOffset(br.ReadInt32());
                case 0:
                    return GetExtPropertyByOffset(br.ReadInt32());
                case PCUConsts.template_prop:
                    return GetTemplateInstanceProperty();
                case PCUConsts.generic_prop:
                    return GetGenericInstanceProperty();
            }
            return null;
        }

        private common_property_node GetExtPropertyByOffset(int offset)
        {
            definition_node dn = null;
            if (ext_members.TryGetValue(offset, out dn))
                return dn as common_property_node;
            common_property_node prop = null;
            {
                int tmp = (int)br.BaseStream.Position;
                br.BaseStream.Seek(ext_pos + offset, SeekOrigin.Begin);
                prop = ReadCommonExtProperty();
                br.BaseStream.Seek(tmp, SeekOrigin.Begin);
            }
            return prop;
        }

        private common_property_node GetPropertyByOffset(int offset)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as common_property_node;
            common_property_node prop = GetPropertyNode(offset);
            return prop;
        }

        private class_field GetClassFieldByOffset()
        {
            byte is_def = br.ReadByte();
            switch (is_def)
            {
                case 1:
                    return GetClassFieldByOffset(br.ReadInt32());
                case 0:
                    return GetExtClassFieldByOffset(br.ReadInt32());
                case PCUConsts.template_field:
                    return GetTemplateInstanceField();
                case PCUConsts.generic_field:
                    return GetGenericInstanceField();
            }
            return null;
        }

        private event_node GetEventByOffset()
        {
            byte is_def = br.ReadByte();
            switch (is_def)
            {
                case 0: return GetExtEventNode(br.ReadInt32());
                case 1: return GetEventNode(br.ReadInt32());
                case 2: return GetCompiledEvent(br.ReadInt32());
                case 4: return GetNamespaceEventNode(br.ReadInt32());
                case 3: return GetExtNamespaceEvent(br.ReadInt32());
            }
            return null;
        }

        private class_field GetClassFieldByOffset(int offset)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as class_field;
            class_field field = GetClassField(offset);
            return field;
        }

        private class_field GetExtClassFieldByOffset(int offset)
        {
            definition_node dn = null;
            if (ext_members.TryGetValue(offset, out dn))
                return dn as class_field;
            class_field field = null;
            {
                int tmp = (int)br.BaseStream.Position;
                //Console.WriteLine(ext_pos+offset);
                br.BaseStream.Seek(ext_pos + offset, SeekOrigin.Begin);
                field = ReadCommonExtField();
                br.BaseStream.Seek(tmp, SeekOrigin.Begin);
            }
            return field;
        }

        private common_event GetExtEventNode(int offset)
        {
            definition_node dn = null;
            if (ext_members.TryGetValue(offset, out dn))
                return dn as common_event;
            common_event ev = null;
            {
                int tmp = (int)br.BaseStream.Position;
                br.BaseStream.Seek(ext_pos + offset, SeekOrigin.Begin);
                ev = ReadCommonExtEvent();
                br.BaseStream.Seek(tmp, SeekOrigin.Begin);
            }
            return ev;
        }

        private common_namespace_event GetExtNamespaceEvent(int offset)
        {
            definition_node dn = null;
            if (ext_members.TryGetValue(offset, out dn))
                return dn as common_namespace_event;
            common_namespace_event ev = null;
            {
                int tmp = (int)br.BaseStream.Position;
                br.BaseStream.Seek(ext_pos + offset, SeekOrigin.Begin);
                ev = ReadCommonNamespaceExtEvent();
                br.BaseStream.Seek(tmp, SeekOrigin.Begin);
            }
            return ev;
        }

		private common_in_function_function_node GetNestedFunctionByOffset(int offset)
		{
			return (common_in_function_function_node)members[offset];
		}

		private statement_node CreateReturnNode()
		{
			return new return_node(CreateExpression(),null);
		}

		private expression_node CreateWhileBreakNode()
		{
			return new while_break_node(null,null);
		}

		private expression_node CreateWhileContinueNode()
		{
			return new while_continue_node(null,null);
		}

		private expression_node CreateForBreakNode()
		{
			return new for_break_node(null,null);
		}

		private expression_node CreateForContinueNode()
		{
			return new for_continue_node(null,null);
		}

		private expression_node CreateRepeatBreakNode()
		{
			return new repeat_break_node(null,null);
		}

		private expression_node CreateRepeatContinueNode()
		{
			return new repeat_continue_node(null,null);
		}

		private expression_node CreateForeachBreakNode()
		{
			return new foreach_break_node(null,null);
		}

		private expression_node CreateForeachContinueNode()
		{
			return new foreach_continue_node(null,null);
		}

		private compiled_static_method_call CreateCompiledStaticMethodCall()
		{
			compiled_function_node cfn = GetCompiledMethod(br.ReadInt32());
			compiled_static_method_call csmc = new compiled_static_method_call(cfn,null);
            int num_params = br.ReadInt32();
            for (int i = 0; i < num_params; i++)
                csmc.template_parametres_list.AddElement(GetTypeReference());
            num_params = br.ReadInt32();
			for (int i=0; i<num_params; i++)
				csmc.parameters.AddElement(CreateExpression());
			return csmc;
		}

		private compiled_function_call CreateCompiledFunctionCall()
		{
			expression_node obj = CreateExpression();
			compiled_function_node cfn = GetCompiledMethod(br.ReadInt32());
			compiled_function_call cfc = new compiled_function_call(cfn,obj,null);
            //ssyy
            cfc.last_result_function_call = br.ReadByte() == 1;
            //\ssyy
            cfc.virtual_call = br.ReadBoolean();
			int num_params = br.ReadInt32();
			for (int i=0; i<num_params; i++)
				cfc.parameters.AddElement(CreateExpression());
			return cfc;
		}

		private compiled_constructor_call CreateCompiledConstructorCall()
		{
			compiled_constructor_node ccn = GetCompiledConstructor(br.ReadInt32());
			compiled_constructor_call ccc = new compiled_constructor_call(ccn,null);
            //ssyy добавил
            ccc._new_obj_awaited = (br.ReadByte() == 1);
            //\ssyy
            int num_params = br.ReadInt32();
			for (int i=0; i<num_params; i++)
				ccc.parameters.AddElement(CreateExpression());
			return ccc;
		}

		private compiled_variable_reference CreateCompiledVariableReference()
		{
			expression_node obj = CreateExpression();
			compiled_variable_definition cvd = GetCompiledVariable(br.ReadInt32());
			compiled_variable_reference cvr = new compiled_variable_reference(cvd,obj,null);
			return cvr;
		}

		private static_compiled_variable_reference CreateStaticCompiledVariableReference()
		{
			compiled_variable_definition cvd = GetCompiledVariable(br.ReadInt32());
			static_compiled_variable_reference scvr = new static_compiled_variable_reference(cvd,null);
			return scvr;
		}

        private simple_array_indexing CreateSimpleArrayIndexing()
        {
            expression_node arr = CreateExpression();
            expression_node ind_expr = CreateExpression();
            List<expression_node> lst = null;
            if (CanReadObject())
            {
            	int len = br.ReadInt32();
            	lst = new List<expression_node>();
            	for (int i=0; i<len; i++)
            		lst.Add(CreateExpression());
            }
            type_node type = GetTypeReference();
            simple_array_indexing expr = new simple_array_indexing(arr, ind_expr, type, null);
            if (lst != null)
            	expr.expr_indices = lst.ToArray();
            return expr;
        }

        private label_node GetLabel(int offset)
        {
            definition_node dn = null;
            if (members.TryGetValue(offset, out dn))
                return dn as label_node;
            label_node lab = null;

            int pos = (int)br.BaseStream.Position;
            br.BaseStream.Seek(start_pos + offset, SeekOrigin.Begin);

            string s = br.ReadString();
            location loc = ReadDebugInfo();
            lab = new label_node(s, loc);
            //members[offset] = lab;
            AddMember(lab, offset);

            br.BaseStream.Seek(pos, SeekOrigin.Begin);
            return lab;
        }

        public int GetStreamPosition()
        {
            return (int)br.BaseStream.Position;
        }

        public void SetStreamPosition(int pos)
        {
            br.BaseStream.Seek(pos, SeekOrigin.Begin);
        }

        /*private List<label_node> CreateLabels()
        {
            int count = br.ReadInt32();
            List<label_node> labels = new List<label_node>(count);
            for (int i = 0; i < count; i++)
            {
                labels.Add(CreateLabel);
            }
        }*/
    }
}

