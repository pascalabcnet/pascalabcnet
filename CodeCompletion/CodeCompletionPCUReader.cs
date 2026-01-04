// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using PascalABCCompiler.SyntaxTree;
//using ICSharpCode.SharpDevelop.Dom;
using System.Reflection;
using PascalABCCompiler;
using PascalABCCompiler.TreeConverter;
using PascalABCCompiler.TreeRealization;
//using SymbolTable;
using PascalABCCompiler.PCU;
using PascalABCCompiler.Parsers;

namespace CodeCompletion
{
	public class IntellisensePCUReader
	{
		private string unit_name;
		private string FileName;
		private string dir;
		private FileStream fs;
		private BinaryReader br;
		private bool readDebugInfo;
		private PCUFile pcu_file = new PCUFile();
		private SymScope root_scope;
		private SymScope cur_scope;
		private int ext_pos;
		private int start_pos;
		private Hashtable assemblies = new Hashtable();
		private Hashtable members = new Hashtable();
		private Hashtable ext_members = new Hashtable();
		
		private static Hashtable unit_cache = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
		
		public IntellisensePCUReader()
		{
			
		}
		
		public SymScope GetUnit(string FileName)
		{
			try
			{
				root_scope = unit_cache[FileName] as SymScope;
				if (root_scope != null) return root_scope;
				unit_name = System.IO.Path.GetFileNameWithoutExtension(FileName);
				this.readDebugInfo = true;
				this.FileName = FileName;
				if (!File.Exists(FileName)) return null;
            	dir = System.IO.Path.GetDirectoryName(FileName);
            	fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            	br = new BinaryReader(fs);
            	ReadPCUHeader();
            	root_scope = new InterfaceUnitScope(new SymInfo(unit_name, SymbolKind.Namespace,unit_name),null);
            	unit_cache[FileName] = root_scope;
            	cur_scope = root_scope;
            	AddReferencedAssemblies();
            	ReadInterfacePart();
            	fs.Close();
			}
			catch (Exception e)
			{
				fs.Close();
				return root_scope;
			}
            return root_scope;
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
		
		private void CloseUnit()
		{
			fs.Close();
		}
		
		private void AddMember(SymScope ss, int offset)
		{
			members[offset] = ss;
		}
		
		private void InvalidUnitDetected()
        {
            //(ssyy) DarkStar - Ïî÷åìó áû â ýòîì ñëó÷àå ïðîñòî íå ïåðåêîìïèëèðîâàòü ìîäóëü?
            CloseUnit();
            throw new InvalidPCUFile(unit_name);
        }
		
		private void ReadPCUHeader()
		{
            if (!ReadPCUHead(pcu_file, br) || PCUFile.SupportedVersion != pcu_file.Version)
                InvalidUnitDetected();
            if(pcu_file.IncludeDebugInfo)
                pcu_file.SourceFileName = br.ReadString();
            else
                pcu_file.SourceFileName = Path.GetFileNameWithoutExtension(FileName);

            pcu_file.languageName = br.ReadString();
            
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
            //pcu_file.compiler_directives = new List<compiler_directive>();
            for (int i = 0; i < num_directives; i++)
            {
                br.ReadString(); 
                br.ReadString(); 
                ReadDebugInfo();
            }

			int num_imp_entity = br.ReadInt32();
			ext_pos = (int)br.BaseStream.Position;
			pcu_file.imp_entitles = new ImportedEntity[num_imp_entity];
			br.BaseStream.Seek(num_imp_entity*ImportedEntity.GetClassSize(),SeekOrigin.Current);

            pcu_file.interface_synonyms_offset = br.ReadInt32();
            pcu_file.implementation_synonyms_offset = br.ReadInt32();

            br.ReadInt32();
            br.ReadInt32();
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
		
		private void ReadInterfacePart()
		{
			for (int i=0; i<pcu_file.names.Length; i++)
			{
				ReadInterfaceEntity(pcu_file.names[i]);
			}
		}
		
		private void ReadDebugInfo()
		{
            if (pcu_file.IncludeDebugInfo)
            {
                if (readDebugInfo)
                {
                    br.ReadInt32();
                    br.ReadInt32();
                    br.ReadInt32();
                    br.ReadInt32();
					return;
                }
                else
                {
                    br.BaseStream.Seek(sizeof(Int32) * 4, SeekOrigin.Current);
                    return;
                }
            }
            return;
		}
		
		private void ReadInterfaceEntity(NameRef nr)
		{
			SymScope ss = CreateInterfaceMember(nr.offset, nr.name);
			if (ss != null)
			cur_scope.AddName(nr.name,ss);
		}
		
		public SymScope CreateInterfaceMember(int offset, string name)
		{
			/*definition_node dn = (definition_node)members[offset];
            if (dn != null)
            {
                //DarkStar Addes 06/03/07
                PCUWriter.AddExternalMember(dn, offset);
                return dn;
            }*/
			SymScope dn = null;
			br.BaseStream.Seek(start_pos+offset,SeekOrigin.Begin);
			semantic_node_type snt = (semantic_node_type)br.ReadByte();
			switch (snt)
			{
				case semantic_node_type.namespace_variable:
					dn = CreateInterfaceNamespaceVariable(name,offset); break;
				case semantic_node_type.common_namespace_function_node:
					dn = CreateInterfaceNamespaceFunction(name,offset); break;
				case semantic_node_type.common_type_node:
					dn = CreateInterfaceCommonType(name,offset); break;
                case semantic_node_type.namespace_constant_definition:
                    dn = CreateInterfaceConstantDefinition(name,offset); break;
                /*case semantic_node_type.compiled_type_node:
                    dn = CreateCompiledTypeNode(offset); break;
                //ssyy
                case semantic_node_type.template_type:
                    dn = CreateTemplateClass(offset); break;
                //\ssyy
                case semantic_node_type.ref_type_node:
                    dn = CreateRefType(offset); break;*/

            }
			br.BaseStream.Seek(start_pos,SeekOrigin.Begin);
            //PCUWriter.AddExternalMember(dn,offset);//ñîîáùàåì âðàéòåðó íîâîå ñìåùåíèå ñóùíîñòè
            //åñëè áóäåò ñåðèàëèçîâûâàòüñÿ ìîäóëü â êîòîðîì èñïîëüçóþòñÿ ñóùíîñòè .pcu, îí äîëæåí çíàòü èõ ñìåùåíèÿ
            //ProcessWaitedToRestoreFields();
            return dn;
		}
		
		private bool CanReadObject()
        {
            return br.ReadByte() == 1;
        }
		
		private string GetString(int index)
		{
			return pcu_file.names[index].name;
		}
		
		private TypeScope CreateInterfaceCommonType(string name, int offset)
		{
            TypeScope ctn = (TypeScope)members[offset];
            if (ctn != null) return ctn;
			bool in_scope = false;
            if (name != null) in_scope = true;
            bool is_interface = br.ReadBoolean();                        
            if (is_interface)//ïðîïóñêàåì ôëàã - èíòåðôåéñíîñòè
            {
                name = GetString(br.ReadInt32());
            }
            else
            {
                name = br.ReadString();
            }
			//br.ReadInt32();
            //ssyy
            //×èòàåì, ÿâëÿåòñÿ ëè òèï èíòåðôåéñîì
            bool type_is_interface = (br.ReadByte() == 1);

            //×èòàåì, ÿâëÿåòñÿ ëè òèï äåëåãàòîì
            bool type_is_delegate = (br.ReadByte() == 1);
            //\ssyy
			
            bool type_is_generic_definition = (br.ReadByte() == 1);
            
            if (type_is_generic_definition)
            {
            	throw new Exception();
            }
            TypeScope base_type = GetTypeReference();
            bool is_value_type = br.ReadBoolean();

            //ssyy
            //×èòàåì ïîääåðæèâàåìûå èíòåðôåéñû
            int interf_count = br.ReadInt32();
            List<TypeScope> interfaces = new List<TypeScope>();
            for (int i = 0; i < interf_count; i++)
            {
                interfaces.Add(GetTypeReference());
            }
            //\ssyy
			object low_val=null;
			object upper_val=null;
            PascalABCCompiler.SemanticTree.type_access_level tal = (PascalABCCompiler.SemanticTree.type_access_level)br.ReadByte();
            PascalABCCompiler.SemanticTree.type_special_kind tsk = (PascalABCCompiler.SemanticTree.type_special_kind)br.ReadByte();
            bool is_sealed = br.ReadBoolean();
            bool is_abstract = br.ReadBoolean();
            if (tsk == PascalABCCompiler.SemanticTree.type_special_kind.diap_type)
            {
            	low_val = CreateExpression();
            	upper_val = CreateExpression();
            }
	
            TypeScope element_type = null;
            if (CanReadObject())
                element_type = GetTypeReference();
            switch (tsk)
            {
            	case PascalABCCompiler.SemanticTree.type_special_kind.none_kind : ctn = new TypeScope(SymbolKind.Class,cur_scope,base_type); break;
            	case PascalABCCompiler.SemanticTree.type_special_kind.record : ctn = new TypeScope(SymbolKind.Struct,cur_scope,base_type); break;
            	case PascalABCCompiler.SemanticTree.type_special_kind.array_wrapper : ctn = new ArrayScope(); break;
            	case PascalABCCompiler.SemanticTree.type_special_kind.enum_kind : ctn = new EnumScope(SymbolKind.Enum,cur_scope,base_type); break;
            	case PascalABCCompiler.SemanticTree.type_special_kind.set_type : ctn = new SetScope(element_type); break;
            	case PascalABCCompiler.SemanticTree.type_special_kind.array_kind : if (!in_scope) ctn = new ArrayScope(); else return null; break;
            	case PascalABCCompiler.SemanticTree.type_special_kind.diap_type : ctn = new DiapasonScope(low_val,upper_val);break;
            	case PascalABCCompiler.SemanticTree.type_special_kind.typed_file : ctn = new FileScope(element_type,null);break;
            	case PascalABCCompiler.SemanticTree.type_special_kind.binary_file : ctn = new FileScope(null,null);break;
           		
            }
            ctn.declaringUnit = root_scope;
            ctn.si.name = name;
            ctn.is_abstract = is_abstract;
            ctn.is_final = is_sealed;
            AddMember(ctn, offset);
            ctn.elementType = element_type;
			
            br.ReadInt32();
            br.ReadInt32();//attributes
            //common_namespace_node ns = cun.namespaces[0];
            byte flag = br.ReadByte();
            int def_prop_off=0;
            if (flag == 1)
            {
                def_prop_off = br.ReadInt32();
            }
            ReadDebugInfo();
            ctn.si.description = ctn.GetDescription();
            //ñîçäàåì scope äëÿ êëàññà
            //ctn = new wrapped_common_type_node(this, base_type, name, tal, ns, scope, loc, offset);
            //members[offset] = ctn;
            //AddMember(ctn, offset);
            RestoreAllMembers(ctn);

            return ctn;
		}
		
		public SymScope CreateInterfaceInClassMember(string name, int offset)
        {
            SymScope dn = members[offset] as SymScope;
            if (dn != null) return dn;
            int tmp = (int)br.BaseStream.Position;
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
                //case semantic_node_type.class_constant_definition:
                //    dn = CreateInterfaceClassConstantDefinition(name, offset); break;
            }
            br.BaseStream.Seek(tmp, SeekOrigin.Begin);
            //ProcessWaitedToRestoreFields();
            return dn;
        }
		
		private ElementScope CreateInterfaceClassField(string name, int offset)
        {
            ElementScope field = members[offset] as ElementScope;
            if (field != null) return field;
            int name_off = br.ReadInt32();
            TypeScope type = GetTypeReference();
            field = new ElementScope(new SymInfo(name, SymbolKind.Field,name),type,cur_scope);
            field.declaringUnit = root_scope;
            AddMember(field, offset);
            object initv = null;
            if (CanReadObject())
                initv = CreateExpression();
            br.ReadInt32();
            PascalABCCompiler.SemanticTree.field_access_level fal = (PascalABCCompiler.SemanticTree.field_access_level)br.ReadByte();
            PascalABCCompiler.SemanticTree.polymorphic_state ps = (PascalABCCompiler.SemanticTree.polymorphic_state)br.ReadByte();
           	
            switch (fal)
            {
            	case PascalABCCompiler.SemanticTree.field_access_level.fal_internal : field.acc_mod = access_modifer.internal_modifer; break;
            	case PascalABCCompiler.SemanticTree.field_access_level.fal_private : field.acc_mod = access_modifer.private_modifer; return null;
            	case PascalABCCompiler.SemanticTree.field_access_level.fal_protected : field.acc_mod = access_modifer.protected_modifer; break;
            	case PascalABCCompiler.SemanticTree.field_access_level.fal_public : field.acc_mod = access_modifer.public_modifer; break;
            	
            }
            
            switch (ps)
            {
            	case PascalABCCompiler.SemanticTree.polymorphic_state.ps_static : field.is_static = true; break;
            	case PascalABCCompiler.SemanticTree.polymorphic_state.ps_virtual : field.is_virtual = true; break;
            }
           
            //field = new class_field(name,type,cont,ps,fal,loc);
            field.cnst_val = initv;
            //members[offset] = field;
            AddMember(field, offset);            
            return field;
        }
		
		private ElementScope CreateInterfaceProperty(string name, int offset)
        {
            ElementScope prop = members[offset] as ElementScope;
            if (prop != null) return prop;
            int name_ref = br.ReadInt32();
            TypeScope type = GetTypeReference();
            if (br.ReadByte() == 1) br.ReadInt32();

            if (br.ReadByte() == 1) br.ReadInt32();
            int num = br.ReadInt32();
            List<ElementScope> prms = new List<ElementScope>();
            for (int i = 0; i < num; i++)
            	prms.Add(GetParameter(null));
            br.ReadInt32();
            PascalABCCompiler.SemanticTree.field_access_level fal = (PascalABCCompiler.SemanticTree.field_access_level)br.ReadByte();
            PascalABCCompiler.SemanticTree.polymorphic_state ps = (PascalABCCompiler.SemanticTree.polymorphic_state)br.ReadByte();
            //ReadDebugInfo();
            prop = new ElementScope(new SymInfo(name, SymbolKind.Property,name),type,cur_scope);
            prop.declaringUnit = root_scope;
            switch (fal)
            {
            	case PascalABCCompiler.SemanticTree.field_access_level.fal_internal : prop.acc_mod = access_modifer.internal_modifer; break;
            	case PascalABCCompiler.SemanticTree.field_access_level.fal_private : prop.acc_mod = access_modifer.private_modifer; return null;
            	case PascalABCCompiler.SemanticTree.field_access_level.fal_protected : prop.acc_mod = access_modifer.protected_modifer; break;
            	case PascalABCCompiler.SemanticTree.field_access_level.fal_public : prop.acc_mod = access_modifer.public_modifer; break;
            	
            }
            
            switch (ps)
            {
            	case PascalABCCompiler.SemanticTree.polymorphic_state.ps_static : prop.is_static = true; break;
            	case PascalABCCompiler.SemanticTree.polymorphic_state.ps_virtual : prop.is_virtual = true; break;
            }
            //members[offset] = prop;
            AddMember(prop, offset);
            
            return prop;
        }
		
		private ProcScope CreateInterfaceMethod(string name, int offset)
        {
            ProcScope cmn = members[offset] as ProcScope;
            if (cmn != null) return cmn;
            cmn = new ProcScope(name,cur_scope);
            cmn.declaringUnit = root_scope;
            //members[offset] = cmn;
            AddMember(cmn, offset);
            
            int name_ref = br.ReadInt32();
            br.ReadByte();
            br.ReadByte();
            bool is_generic = br.ReadBoolean();
            if (is_generic)
            {
            	throw new NotSupportedException();
            }
            //ssyy
           
            //\ssyy

            if (br.ReadByte() == 1) //return_value_type
            {
                cmn.return_type = GetTypeReference();
                if (br.ReadByte() == 1)
                {
                    GetLocalVariable(cmn);
                }
            }
            int num_par = br.ReadInt32();
            for (int i = 0; i < num_par; i++)
                cmn.parameters.Add(GetParameter(cmn));
            br.ReadInt32();
            br.ReadInt32();
            cmn.is_constructor = br.ReadBoolean();
            cmn.is_forward = br.ReadBoolean();
            br.ReadBoolean();
            PascalABCCompiler.SemanticTree.field_access_level fal = (PascalABCCompiler.SemanticTree.field_access_level)br.ReadByte();
            PascalABCCompiler.SemanticTree.polymorphic_state ps = (PascalABCCompiler.SemanticTree.polymorphic_state)br.ReadByte();
            switch (fal)
            {
            	case PascalABCCompiler.SemanticTree.field_access_level.fal_internal : cmn.acc_mod = access_modifer.internal_modifer; break;
            	case PascalABCCompiler.SemanticTree.field_access_level.fal_private : cmn.acc_mod = access_modifer.private_modifer; return null;
            	case PascalABCCompiler.SemanticTree.field_access_level.fal_protected : cmn.acc_mod = access_modifer.protected_modifer; break;
            	case PascalABCCompiler.SemanticTree.field_access_level.fal_public : cmn.acc_mod = access_modifer.public_modifer; break;
            }
            
            switch (ps)
            {
            	case PascalABCCompiler.SemanticTree.polymorphic_state.ps_static : cmn.is_static = true; break;
            	case PascalABCCompiler.SemanticTree.polymorphic_state.ps_virtual : cmn.is_virtual = true; break;
            }
            br.ReadInt32(); br.ReadInt32();
            cmn.is_override = br.ReadBoolean() == true;
            cmn.Complete();
            return cmn;
        }
		
		private void RestoreAllMembers(TypeScope scope)
        {
            int num = br.ReadInt32();
            Hashtable ht = new Hashtable();
            for (int i = 0; i < num; i++)
            {
                string name = br.ReadString();
                int offset = br.ReadInt32();
                SymScope tmp = cur_scope;
                cur_scope = scope;
                SymScope ss = CreateInterfaceInClassMember(name,offset);
                br.ReadByte();
                br.ReadByte();
                if (ht.ContainsKey(name) && ss is ProcScope)
                {
                	(ht[name] as ProcScope).nextProc = ss as ProcScope;
                	ht[name] = ss;
                }
                else if (ss is ProcScope)
                {
                	ht[name] = ss;
                }
                cur_scope = tmp;
                if (ss != null)
                scope.AddName(name,ss);
            }
        }
		
		private ProcScope CreateInterfaceNamespaceFunction(string name, int offset)
		{
			//Console.WriteLine(name);
			ProcScope cnfn = new ProcScope(name,cur_scope);
			cnfn.declaringUnit = root_scope;
            if (CanReadObject())
            	br.ReadInt32();
            br.ReadBoolean();//ïðîïóñêàåì ôëàã - èíòåðôåéñíîñòè
			br.ReadInt32();//name index
			bool is_generic = br.ReadBoolean();
            if (is_generic)
            {
            	throw new NotSupportedException();
            }
			if (br.ReadByte() == 1)
			{
				cnfn.return_type = GetTypeReference();
				GetLocalVariable(cnfn);
			}
			int num_par = br.ReadInt32();
			for (int i=0; i<num_par; i++)
				cnfn.parameters.Add(GetParameter(cnfn));
			br.ReadInt32(); //namespace
			br.ReadInt32(); //attributes
			cnfn.is_forward = br.ReadBoolean();
			cnfn.Complete();
			AddMember(cnfn, offset);
			return cnfn;
		}
		
		private ElementScope CreateInterfaceConstantDefinition(string name, int offset)
        {
            ElementScope ncd = members[offset] as ElementScope;
            if (ncd != null) return ncd;
            br.ReadBoolean();
            br.ReadInt32();
            //type_node tn = GetTypeReference();
            br.ReadInt32();//namespace
            //object en = CreateExpression();
            br.ReadInt32();//init_value
            ReadDebugInfo();
            ncd = new ElementScope(new SymInfo(name, SymbolKind.Constant,name),null,cur_scope);
            ncd.declaringUnit = root_scope;
            //???
            AddMember(ncd, offset);
            return ncd;
        }
		
		private ElementScope CreateInterfaceNamespaceVariable(string name, int offset)
        {
			br.ReadInt32();
			br.ReadBoolean();
            br.ReadInt32();
            TypeScope type = GetTypeReference();
            ElementScope nv = new ElementScope(new SymInfo(name, SymbolKind.Variable,name),type,cur_scope);
            nv.declaringUnit = root_scope;
            AddMember(nv, offset);
            return nv;
        }
		
		private void GetLocalVariable(ProcScope func)
		{
			int offset = (int)br.BaseStream.Position-start_pos;
			//int tmp=br.ReadByte();
			br.ReadString();
			GetTypeReference();
            br.ReadBoolean();
            //members[offset] = lv;          
            if (CanReadObject())
            	br.ReadInt32();
            	//CreateExpression();
            return;
		}
		
		private ElementScope GetParameter(ProcScope func)
		{
			int offset = (int)br.BaseStream.Position-start_pos;
			br.ReadByte();
			string s = br.ReadString();
			TypeScope tn = GetTypeReference();
			concrete_parameter_type cpt = (concrete_parameter_type)br.ReadByte();
            ElementScope p = new ElementScope(new SymInfo(s, SymbolKind.Parameter,s),tn,null,null);
            bool is_params = br.ReadBoolean();
            if (is_params) p.param_kind = parametr_kind.params_parametr;
            else
            switch (cpt)
            {
                case concrete_parameter_type.cpt_const: p.param_kind = parametr_kind.const_parametr; break;
                case concrete_parameter_type.cpt_var: p.param_kind = parametr_kind.var_parametr; break;
                case concrete_parameter_type.cpt_none: p.param_kind = parametr_kind.none; break;
            }
            br.ReadBoolean();
            if (CanReadObject())
			{
				br.ReadInt32();//CreateExpression();
			}
            br.ReadInt32();//attributes
			//members[offset] = p;           
			return p;
		}
		
		private TypeScope GetTypeReference()
		{
			byte b = br.ReadByte();
            //(ssyy) Âñòàâèë switch âìåñòî óñëîâèé
            TypeScope tn;
            switch (b)
            {
                case 255:
                	return null;
                case 1://åñëè òèï îïèñàí â ìîäóëå
                	int offset = br.ReadInt32();
                    tn = (TypeScope)members[offset];
                	if (tn == null) return GetCommonType(offset);
                	return tn;
                case 0://åñëè ýòî èìïîðòèð. òèï
                    tn = null;
					int pos = br.ReadInt32();
					tn = ext_members[pos] as TypeScope;
					if (tn != null) return tn;
					int tmp = (int)br.BaseStream.Position;
					br.BaseStream.Seek(ext_pos+pos,SeekOrigin.Begin);
					if ((ImportKind)br.ReadByte() == ImportKind.Common)
					{
						//tn = ReadCommonExtType();
						//ext_members[pos] = tn;
						tn = new UnknownScope(new SymInfo("$", SymbolKind.Type,"$"));
						ext_members[pos] = tn;
					}
					else // ýòî íåòîâñêèé òèï
					{
						tn = ReadNetExtType();
						ext_members[pos] = tn;
					}
					br.BaseStream.Seek(tmp,SeekOrigin.Begin);
					return tn;
                case 2://ýòî ìàññèâ
                	//simple_array type = new simple_array(GetTypeReference(),br.ReadInt32());
                	TypeScope type = new ArrayScope(GetTypeReference(),new TypeScope[0]);
                	return type;
                case 3://ýòî óêàçàòåëü
                	TypeScope pointed_type = GetTypeReference();
                	return new PointerScope(pointed_type);
                case 4://ýòî äèíàìè÷åñêèé ìàññèâ
					ReadDebugInfo();
               	 	TypeScope elem_type = GetTypeReference();
               	 	return new ArrayScope(elem_type,null);
                case 6:
                	//return GetTemplateInstance();
                	return null;
                case 7:
                    return GetGenericInstance();
                case 8:
                    //return GetShortStringType();
                    return null;
            }
			return null;
		}
		
		private TypeScope GetGenericInstance()
        {
            TypeScope ts = GetTypeReference();
            int params_count = br.ReadInt32();
            ts.ClearInstances();
            for (int i = 0; i < params_count; i++)
            {
               ts.AddGenericInstanciation(GetTypeReference());
            }
            ts.si.description = ts.ToString();
            return ts;
        }
		
		private TypeScope GetTemplateInstance()
        {
            TypeScope tc = null;//GetTemplateClassReference();
            int params_count = br.ReadInt32();
            for (int i = 0; i < params_count; i++)
            {
                tc.AddGenericInstanciation(GetTypeReference());
            }
            return tc;
        }
		
		public TypeScope GetCommonType(int offset)
        {
            TypeScope ctn = members[offset] as TypeScope;
            if (ctn != null) return ctn;
            int pos = (int)br.BaseStream.Position;
            br.BaseStream.Seek(start_pos + offset, SeekOrigin.Begin);
            br.ReadByte();
            ctn = CreateInterfaceCommonType(null, offset);
            br.BaseStream.Seek(pos, SeekOrigin.Begin);
            return ctn;
        }
		
		private CompiledScope ReadNetExtType()
		{
			int pos = br.ReadInt32();
			string s = pcu_file.ref_assemblies[pos];
			Assembly a = (Assembly)assemblies[s];
			if (a == null)
			{
                string tmp = s.Substring(0, s.IndexOf(','));
                string name_with_path = Compiler.GetReferenceFileName(tmp+".dll");
                    //a = Assembly.LoadFrom(name_with_path);
                a = PascalABCCompiler.NetHelper.NetHelper.LoadAssembly(name_with_path);
                PascalABCCompiler.NetHelper.NetHelper.init_namespaces(a);
                assemblies[s] = a;
			}
			//Type t = NetHelper.NetHelper.FindTypeByHandle(a,br.ReadInt32());//íàõîäèì åãî ïî òîêåíó
            Type t = FindTypeByHandle(br.ReadInt32());
            CompiledScope cs = TypeTable.get_compiled_type(new SymInfo(t.Name, SymbolKind.Class,t.FullName),t);
            return cs;

		}
		
		private Type FindTypeByHandle(int off)
        {
            Type t = PascalABCCompiler.NetHelper.NetHelper.FindTypeOrCreate(pcu_file.dotnet_names[off].name);
            Type[] template_types = new Type[pcu_file.dotnet_names[off].addit.Length];
            for (int i = 0; i < template_types.Length; i++)
                template_types[i] = FindTypeByHandle(pcu_file.dotnet_names[off].addit[i].offset);
            if (template_types.Length > 0)
                return t.MakeGenericType(template_types);
            return t;
        }
		
		private void AddReferencedAssemblies()
		{
			for (int i=0; i<pcu_file.ref_assemblies.Length; i++)
			{
				string s = pcu_file.ref_assemblies[i];
				string tmp = s.Substring(0, s.IndexOf(','));
				tmp = Compiler.GetReferenceFileName(tmp+".dll");
				System.Reflection.Assembly assm = PascalABCCompiler.NetHelper.NetHelper.LoadAssembly(tmp);
            	PascalABCCompiler.NetHelper.NetHelper.init_namespaces(assm);
            	AssemblyDocCache.Load(assm,tmp);
            	//namespaces.AddRange(PascalABCCompiler.NetHelper.NetHelper.GetNamespaces(assm));
            	(cur_scope as InterfaceUnitScope).AddReferencedAssembly(assm);
			}
		}
		
		private object CreateExpression()
		{
			return CreateExpression((semantic_node_type)br.ReadByte());
		}
		
		private object CreateExpression(semantic_node_type snt)
		{
            //location loc = ReadDebugInfo();
			switch (snt) {
                /*case semantic_node_type.typeof_operator:
                    return CreateTypeOfOperator();
                case semantic_node_type.question_colon_expression:
                    return CreateQuestionColonExpression();
                case semantic_node_type.sizeof_operator:
                    return CreateSizeOfOperator();
                case semantic_node_type.is_node:
                    return CreateIsNode(); 
                case semantic_node_type.as_node:
                    return CreateAsNode();
                case semantic_node_type.compiled_static_method_call_node_as_constant:
                    return CreateCompiledStaticMethodCallNodeAsConstant();
                case semantic_node_type.common_namespace_function_call_node_as_constant:
                    return CreateCommonNamespaceFunctionCallNodeAsConstant();
                case semantic_node_type.compiled_constructor_call_as_constant:
                    return CreateCompiledConstructorCallAsConstant();
                case semantic_node_type.array_const:
                    return CreateArrayConst();
                case semantic_node_type.record_const:
                    return CreateRecordConst();*/
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
                /*case semantic_node_type.basic_function_call:
                    return CreateBasicFunctionCall();
                case semantic_node_type.enum_const:
                    return CreateEnumConstNode();*/
			}
			//throw new Exception("Unknown expression "+snt);
			return null;
		}
		
		private object CreateFloatConst()
		{
			return br.ReadSingle();
		}
		
		private object CreateByteConstNode()
		{
			return br.ReadByte();
		}
		
		private object CreateIntConstNode()
		{
			return br.ReadInt32();
		}
		
		private object CreateSByteConstNode()
		{
			return br.ReadSByte();
		}
		
		private object CreateShortConstNode()
		{
			return br.ReadInt16();
		}
		
		private object CreateUShortConstNode()
		{
			return br.ReadUInt16();
		}
		
		private object CreateUIntConstNode()
		{
			return br.ReadUInt32();
		}
		
		private object CreateLongConstNode()
		{
			return br.ReadInt64();
		}
		
		private object CreateULongConstNode()
		{
			return br.ReadUInt64();
		}
		
		private object CreateDoubleConstNode()
		{
			return br.ReadDouble();
		}
		
		private object CreateCharConstNode()
		{
			return br.ReadChar();
		}
		
		private object CreateBoolConstNode()
		{
			return br.ReadBoolean();
		}
		
		private object CreateStringConstNode()
		{
			return br.ReadString();
		}
	}
}

