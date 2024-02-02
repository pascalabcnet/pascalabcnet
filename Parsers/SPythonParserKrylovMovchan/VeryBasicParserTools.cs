using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QUT.Gppg;

using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;
using UniversalParserHelper;

namespace VeryBasicParser
{
    public static class StringResources
    {
        private static string prefix = "PASCALABCPARSER_";
        public static string Get(string Id)
        {
            string ret = PascalABCCompiler.StringResources.Get(prefix + Id);
            if (ret == prefix + Id)
                return Id;
            else
                return ret;
        }
    }

    public class SymbolTable 
    {
        private SymbolTable outerScope;
        private SortedSet<string> symbols = new SortedSet<string>();
        public SymbolTable(SymbolTable outerScope = null) {
            this.outerScope = outerScope;
        }

        public bool Contains(string ident) {
            SymbolTable curr = this;

            while (curr != null) {
                if (curr.symbols.Contains(ident))
                    return true;
                curr = curr.outerScope;
            }

            return false;
        }

        public void Add(string ident) {
            symbols.Add(ident);
        }

        public SymbolTable OuterScope { get { return outerScope; } }
    }

    public class VeryBasicParserTools: UniversalParserHelper.UniversalParserHelper
    {
        public void AddErrorFromResource(string res, PascalABCCompiler.SyntaxTree.SourceContext loc, params string[] pars)
        {
            res = StringResources.Get(res);
            if (pars != null && pars.Length > 0)
                res = string.Format(res, pars);
            errors.Add(new SyntaxError(res, CurrentFileName, loc, null));
        }

        public expression ConvertNamedTypeReferenceToDotNodeOrIdent(named_type_reference ntr) // либо ident либо dot_node
        {
            if (ntr.names.Count == 1)
                return ntr.names[0];
            else
            {
                var dn = new dot_node(ntr.names[0], ntr.names[1], ntr.names[0].source_context.Merge(ntr.names[1].source_context));
                for (var i = 2; i < ntr.names.Count; i++)
                    dn = new dot_node(dn, ntr.names[i],dn.source_context.Merge(ntr.names[i].source_context));
                dn.source_context = ntr.source_context;
                return dn;
            }
        }

        public named_type_reference ConvertDotNodeOrIdentToNamedTypeReference(expression en)
        {
            if (en is ident)
                return new named_type_reference(en as ident, en.source_context);
            if (en is dot_node)
            {
                var dn = en as dot_node;
                var sc = dn.source_context;
                var ids = new List<ident>();
                ids.Add(dn.right as ident);
                while (dn.left is dot_node)
                {
                    dn = dn.left as dot_node;
                    ids.Add(dn.right as ident);
                }
                ids.Add(dn.left as ident);
                ids.Reverse();
                return new named_type_reference(ids, sc);
            }
            this.AddErrorFromResource("TYPE_NAME_EXPECTED",  en.source_context);
            return null;
        }
    }
}
