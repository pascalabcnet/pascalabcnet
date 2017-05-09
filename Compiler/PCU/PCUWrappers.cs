// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.IO;
using System.Collections;
using System.Reflection;
using TreeConverter;
using SymbolTable;
using PascalABCCompiler.TreeConverter;
using PascalABCCompiler.TreeRealization;

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
            : base(PascalABCCompiler.SystemLibrary.SystemLibrary.symtab, null, new Scope[0] { })
        {
            this.pr = pr;
        }

        public override SymbolInfoList Find(string name)
        {
            SymbolInfoList si = SymbolTable.Find(this, name);
            if (si == null) return null;
            foreach(SymbolInfoUnit tsi in si.InfoUnitList)
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
            return si;
        }

        public override SymbolInfoList FindOnlyInScope(string name)
        {
            SymbolInfoList si = SymbolTable.FindOnlyInScope(this, name, false);
            
            if (si == null) return si;
            foreach(SymbolInfoUnit tsi in si.InfoUnitList)
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
            return si;
        }

        //нужно для перегруженных методов
        public SymbolInfoList FindWithoutCreation(string name)
        {
            return SymbolTable.FindOnlyInType(this, name);
        }
    }

    //ssyy
    public class WrappedUnitImplementationScope : UnitImplementationScope
    {
        protected PCUReader pr;

        public WrappedUnitImplementationScope(PCUReader pr, Scope TopScope)
            : base(PascalABCCompiler.SystemLibrary.SystemLibrary.symtab, TopScope, new Scope[0] { })
        {
            this.pr = pr;
        }

        public override SymbolInfoList Find(string name)
        {
            SymbolInfoList si = SymbolTable.Find(this, name);
            if (si == null) return null;
            foreach(SymbolInfoUnit tsi in si.InfoUnitList)
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
            return si;
        }

        public override SymbolInfoList FindOnlyInScope(string name)
        {
            SymbolInfoList si = SymbolTable.FindOnlyInScope(this, name, false);

            if (si == null) return si;
            foreach(SymbolInfoUnit tsi in si.InfoUnitList)
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
            return si;
        }

        //нужно для перегруженных методов
        public SymbolInfoList FindWithoutCreation(string name)
        {
            return SymbolTable.FindOnlyInType(this, name);
        }
    
    }
    //\ssyy

    public class WrappedClassScope : ClassScope
    {
        protected PCUReader pr;
        
        public WrappedClassScope(PCUReader pr, Scope top_scope, Scope up_scope)
            : base(PascalABCCompiler.SystemLibrary.SystemLibrary.symtab, top_scope, up_scope)
        {
            this.pr = pr;
        }

        public override SymbolInfoList Find(string name)
        {
            SymbolInfoList si = SymbolTable.Find(this, name);
            if (PartialScope != null)
            {
                if (si == null)
                    si = SymbolTable.Find(PartialScope, name);
                else
                {
                    si.Add(SymbolTable.Find(PartialScope, name));
                }
            }
            if (si == null) return null;
            //если это заглушка, то разворачиваем сущность
            foreach (SymbolInfoUnit tsi in si.InfoUnitList)
            {
                if (tsi.sym_info.semantic_node_type == semantic_node_type.wrap_def)
                {
                    wrapped_definition_node wdn = (wrapped_definition_node)tsi.sym_info;
                    tsi.sym_info = wdn.PCUReader.CreateInterfaceInClassMember(wdn.offset, name);
                }
            }
            return si;
        }

        public void RestoreMembers(string name)
        {
            SymbolInfoList si = SymbolTable.FindOnlyInThisClass(this, name);
            //если это заглушка, то разворачиваем сущность
            foreach(SymbolInfoUnit tsi in si.InfoUnitList)
            {
                if (tsi.sym_info.semantic_node_type == semantic_node_type.wrap_def)
                {
                    PCUSymbolInfo pcu_tsi = tsi.ToSymbolInfo() as PCUSymbolInfo;
                    if (!(pcu_tsi != null && pcu_tsi.semantic_node_type == semantic_node_type.common_method_node && !pcu_tsi.virtual_slot) || pr.comp.CompilerOptions.OutputFileType == CompilerOptions.OutputType.ClassLibrary)
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

        public override SymbolInfoList Find(string name, Scope CurrentScope)
        {
            return Find(name);
        }

        public SymbolInfoList FindWithoutCreation(string name)
        {
            return SymbolTable.FindOnlyInScope(this, name, false);
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

        public override SymbolInfoList find_in_type(string name, bool no_search_in_extension_methods = false)
        {
            var temp = find_in_type(name, null, no_search_in_extension_methods);
            if (temp != null)
                return new SymbolInfoList(temp);
            return null;
        }
        public override SymbolInfo find_in_type(string name, Scope CurrentScope, bool no_search_in_extension_methods = false)
        {
            SymbolInfo si;
            var temp = scope.FindOnlyInType(name, CurrentScope);
            if (temp != null)
                si = temp.ToSymbolInfo();
            else
                si = null;
            if (si == null) return si;
            SymbolInfo tsi = si;
            while (tsi != null)
            {
                if (tsi.sym_info.semantic_node_type == semantic_node_type.wrap_def)
                {
                    wrapped_definition_node wdn = (wrapped_definition_node)tsi.sym_info;
                    tsi.sym_info = wdn.PCUReader.CreateInterfaceInClassMember(wdn.offset, name);
                }
                tsi = tsi.Next;
            }
            return si;
        }

    }
	
}

