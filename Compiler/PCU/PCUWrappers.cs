// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.IO;
using System.Collections;
using System.Reflection;
using TreeConverter;
using SymbolTable;
using PascalABCCompiler.TreeConverter;
using PascalABCCompiler.TreeRealization;
using System.Collections.Generic;

namespace PascalABCCompiler.PCU
{
    /*public interface IWrappedNodeWithOffset
    {
        public PCUReader pcuReader
        {
            get;
            set;
        }

        public int offset
        {
            get;
            set;
        }
    }*/

    public class WrappedUnitInterfaceScope : UnitInterfaceScope
    {
        protected PCUReader pr;

        public WrappedUnitInterfaceScope(PCUReader pr)
            : base(PascalABCCompiler.SystemLibrary.SystemLibrary.symtab, null, new Scope[0] { }, "")
        {
            this.pr = pr;
            Name = Path.GetFileName(pr.FileName);
        }

        public override List<SymbolInfo> Find(string name)
        {
            List<SymbolInfo> sil = SymbolTable.Find(this, name);
            if (sil == null) return null;
            foreach(SymbolInfo tsi in sil)
            {
                if (tsi.sym_info.semantic_node_type == semantic_node_type.wrap_def)
                {
                    wrapped_definition_node wdn = (wrapped_definition_node)tsi.sym_info;
                    if (wdn.is_synonim)
                    	tsi.sym_info = wdn.PCUReader.CreateTypeSynonim(wdn.offset, name);
                    else
                    	tsi.sym_info = wdn.PCUReader.CreateInterfaceMember(wdn.offset, name);
                }
            }
            return sil;
        }

        public override List<SymbolInfo> FindOnlyInScope(string name)
        {
            List<SymbolInfo> sil = SymbolTable.FindOnlyInScope(this, name, false);
            
            if (sil == null) return sil;
            foreach(SymbolInfo tsi in sil)
            {
                if (tsi.sym_info.semantic_node_type == semantic_node_type.wrap_def)
                {
                    wrapped_definition_node wdn = (wrapped_definition_node)tsi.sym_info;
                    if (wdn.is_synonim)
                        tsi.sym_info = wdn.PCUReader.CreateTypeSynonim(wdn.offset, name);
                    else
                    	tsi.sym_info = wdn.PCUReader.CreateInterfaceMember(wdn.offset,name);
                }
            }
            return sil;
        }

        //нужно для перегруженных методов
        public List<SymbolInfo> FindWithoutCreation(string name)
        {
            return SymbolTable.FindOnlyInType(this, name);
        }
    }

    //ssyy
    public class WrappedUnitImplementationScope : UnitImplementationScope
    {
        protected PCUReader pr;

        public WrappedUnitImplementationScope(PCUReader pr, Scope TopScope)
            : base(PascalABCCompiler.SystemLibrary.SystemLibrary.symtab, TopScope, new Scope[0] { }, "")
        {
            this.pr = pr;
        }

        public override List<SymbolInfo> Find(string name)
        {
            List<SymbolInfo> sil = SymbolTable.Find(this, name);
            if (sil == null) return null;
            foreach(SymbolInfo tsi in sil)
            {
                if (tsi.sym_info.semantic_node_type == semantic_node_type.wrap_def)
                {
                    wrapped_definition_node wdn = (wrapped_definition_node)tsi.sym_info;
                    if (wdn.is_synonim)
                    	tsi.sym_info = wdn.PCUReader.CreateTypeSynonim(wdn.offset, name);
                    else
                    	tsi.sym_info = wdn.PCUReader.CreateImplementationMember(wdn.offset);
                }
            }
            return sil;
        }

        public override List<SymbolInfo> FindOnlyInScope(string name)
        {
            List<SymbolInfo> sil = SymbolTable.FindOnlyInScope(this, name, false);

            if (sil == null) return sil;
            foreach(SymbolInfo tsi in sil)
            {
                if (tsi.sym_info.semantic_node_type == semantic_node_type.wrap_def)
                {
                    wrapped_definition_node wdn = (wrapped_definition_node)tsi.sym_info;
                    if (wdn.is_synonim)
                        tsi.sym_info = wdn.PCUReader.CreateTypeSynonim(wdn.offset, name);
                    else
                        tsi.sym_info = wdn.PCUReader.CreateImplementationMember(wdn.offset);
                }
            }
            return sil;
        }

        //нужно для перегруженных методов
        public List<SymbolInfo> FindWithoutCreation(string name)
        {
            return SymbolTable.FindOnlyInType(this, name);
        }
    
    }
    //\ssyy

    public class WrappedClassScope : ClassScope
    {
        protected PCUReader pr;
        internal common_type_node ctn;
        
        public WrappedClassScope(PCUReader pr, Scope top_scope, Scope up_scope)
            : base(SystemLibrary.SystemLibrary.symtab, top_scope, up_scope, "")
        {
            this.pr = pr;
        }

        public override List<SymbolInfo> Find(string name)
        {
            List<SymbolInfo> sil = SymbolTable.Find(this, name);
            if (PartialScope != null)
            {
                if (sil == null)
                    sil = SymbolTable.Find(PartialScope, name);
                else
                    sil.AddRange(SymbolTable.Find(PartialScope, name));
            }
            if (sil == null) return sil;
            //если это заглушка, то разворачиваем сущность
            foreach (SymbolInfo tsi in sil)
            {
                if (tsi.sym_info.semantic_node_type == semantic_node_type.wrap_def)
                {
                    wrapped_definition_node wdn = (wrapped_definition_node)tsi.sym_info;
                    tsi.sym_info = wdn.PCUReader.CreateInterfaceInClassMember(wdn.offset, name);
                }
            }
            return sil;
        }
        
        private bool hasNotRestoreAttribute()
        {
            if (ctn == null)
                return false;
            foreach (var attr in ctn.attributes)
            {
                if (string.Compare(attr.attribute_type.full_name, "PABCSystem.PCUNotRestoreAttribute", true) == 0)
                    return true;
            }
            return false;
        }
        
        private bool needRestore(PCUSymbolInfo pcu_tsi, string name)
        {
            if (pcu_tsi == null)
                return true;
            //if (pcu_tsi.semantic_node_type == semantic_node_type.common_method_node && pcu_tsi.virtual_slot)
            //    return true;
            if (pcu_tsi.always_restore)
                return true;
            if (hasNotRestoreAttribute())
                return false;
            if (pcu_tsi.is_static)
                return false;
            if (pcu_tsi.semantic_node_type == semantic_node_type.common_method_node)
                return false;
            return true;
        }
        
        public void RestoreMembers(string name)
        {
            List<SymbolInfo> sil = SymbolTable.FindOnlyInThisClass(this, name);
            //если это заглушка, то разворачиваем сущность
            foreach(SymbolInfo tsi in sil)
            {
                if (tsi.sym_info.semantic_node_type == semantic_node_type.wrap_def)
                {
                    PCUSymbolInfo pcu_tsi = tsi as PCUSymbolInfo;
                    if (needRestore(pcu_tsi, name) || pr.comp.CompilerOptions.OutputFileType == CompilerOptions.OutputType.ClassLibrary || name == "op_Equality" || name == "op_Inequality")
                    {
                        wrapped_definition_node wdn = (wrapped_definition_node)tsi.sym_info;
                        tsi.sym_info = wdn.PCUReader.CreateInterfaceInClassMember(wdn.offset, name);
                    }
                    else
                    {

                    }
                }
            }
        }

        public override List<SymbolInfo> Find(string name, Scope CurrentScope)
        {
            return Find(name);
        }

        public List<SymbolInfo> FindWithoutCreation(string name)
        {
            return SymbolTable.FindOnlyInScope(this,name, false);
        }

    }

    //ssyy
    public class WrappedInterfaceScope : WrappedClassScope, IInterfaceScope
    {
        private Scope[] _TopInterfaceScopeArray;
        public virtual Scope[] TopInterfaceScopeArray
        {
            get
            {
                return _TopInterfaceScopeArray;
            }
            set
            {
                _TopInterfaceScopeArray = value;
            }
        }

        public WrappedInterfaceScope(PCUReader pr, Scope TopScope, Scope[] vTopInterfaceScopeArray)
            :
            base(pr, TopScope, null)
        {
            _TopInterfaceScopeArray = vTopInterfaceScopeArray;
        }

        public WrappedInterfaceScope(PCUReader pr, Scope TopScope, Scope BaseClassScope, Scope[] vTopInterfaceScopeArray)
            :
            base(pr, TopScope, BaseClassScope)
        {
            _TopInterfaceScopeArray = vTopInterfaceScopeArray;
        }
    }
    //\ssyy
	
    public class wrapped_type_synonym : wrapped_definition_node
    {
        public string name;
        
        public wrapped_type_synonym(PCUReader pr, string name, int offset):base(offset,pr)
        {
        	this.name = name;
        	is_synonim = true;
        }
    }

    public class wrapped_expression : expression_node
    {
        private PCUReader pr;

        public int offset;

        public wrapped_expression(PCUReader pr, int offset)
        {
            this.pr = pr;
            this.offset = offset;
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.wrapped_expression;
            }
        }

        public expression_node restore()
        {
            return pr.GetExpression(offset);
        }
    }

    public class wrapped_function_body : wrapped_statement
    {
        private PCUReader pr;

        public int offset;

        public wrapped_function_body(PCUReader pr, int offset):base(null)
        {
            this.pr = pr;
            this.offset = offset;
        }

        public override statement_node restore()
        {
            return pr.GetCode(offset);
        }

    }

    public class wrapped_common_type_node : common_type_node
    {
        private PCUReader pr;

        public int offset;

        public wrapped_common_type_node(PCUReader pr, type_node base_type, string name, SemanticTree.type_access_level type_access_level,
            common_namespace_node comprehensive_namespace, SymbolTable.ClassScope cs, location loc, int offset) :
            base(base_type, name, type_access_level, comprehensive_namespace, cs, loc)
        {
            this.pr = pr;
            this.offset = offset;
        }

        public override List<SymbolInfo> find_in_type(string name, bool no_search_in_extension_methods = false)
        {
            return find_in_type(name, null, null, no_search_in_extension_methods);
        }
        public override List<SymbolInfo> find_in_type(string name, Scope CurrentScope, type_node orig_generic_or_null = null, bool no_search_in_extension_methods = false)
        {
            List<SymbolInfo> sil = scope.FindOnlyInType(name, CurrentScope);
            if (sil == null)
            {
                if (base_type != null && base_type.IsDelegate)
                    return base_type.find_in_type(name, CurrentScope, null, no_search_in_extension_methods);
                else if (name == StringConstants.deconstruct_method_name)
                    return SystemLibrary.SystemLibrary.object_type.find_in_type(name, CurrentScope, null, no_search_in_extension_methods);

                // SSM перенес из common_type_node (types.cs 2116) - без этого не работали методы расширения последовательностей для типов в pcu, реализующих IEnumerable<T>
                if (ImplementingInterfaces != null)
                {
                    Dictionary<definition_node, definition_node> cache = new Dictionary<definition_node, definition_node>();
                    List<SymbolInfo> props = new List<SymbolInfo>();
                    foreach (type_node ii_tn in ImplementingInterfaces)
                    {
                        List<SymbolInfo> isi = ii_tn.find_in_type(name, CurrentScope);
                        if (isi != null)
                        {
                            if (sil == null)
                                sil = new List<SymbolInfo>();
                            foreach (SymbolInfo si in isi)
                            {
                                if (!cache.ContainsKey(si.sym_info))
                                {
                                    if (si.sym_info is function_node && (si.sym_info as function_node).is_extension_method
                                        && sil.FindIndex(ssi => ssi.sym_info == si.sym_info) == -1)  // SSM 12.12.18 - за счёт методов интерфейсов тоже могут добавляться одинаковые - исключаем их
                                        sil.Add(si);
                                    cache.Add(si.sym_info, si.sym_info);
                                }
                            }
                        }
                    }
                    if (sil != null && sil.Count == 0)
                        sil = null;
                }

                return sil;
            }
            if (this.base_generic_instance != null && sil != null)
            {
                var bsil = base_generic_instance.ConvertSymbolInfo(sil);
                if (orig_generic_or_null == null && name != "op_Implicit")  
                    return bsil;
            }
            foreach (SymbolInfo si in sil)
            {
                if (si.sym_info.semantic_node_type == semantic_node_type.wrap_def)
                {
                    wrapped_definition_node wdn = (wrapped_definition_node)si.sym_info;
                    si.sym_info = wdn.PCUReader.CreateInterfaceInClassMember(wdn.offset, name);
                }
            }
            return sil;
        }

    }
	
}

