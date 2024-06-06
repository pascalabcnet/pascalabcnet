using System.Collections.Generic;
using System.Diagnostics;
using PascalABCCompiler.SyntaxTree;

namespace AssignTupleDesugarAlgorithm
{
    public class Symbol
    {
        public enum Type
        {
            VAR_PARAM, FROM_OUTER_SCOPE, INDEXER, DOT_NODE, LOCAL, EXPR, POINTER
        }
        public string name;
        public string last_name;

        //public bool canBeSynonym = false;
        //public bool isIndexer = false;
        //public bool isExpr = false;
        public expression node;
        public Type type;

        public Symbol(expression ex, BindCollectLightSymInfo binder)
        {
            node = ex;
            if (node is indexer ind)
            {
                name = ind.ToString();
                last_name = name;
                type = Type.INDEXER;
            }
            else if (node is dot_node dn)
            {
                var last = dn.right;
                while (last is dot_node dn1)
                    last = dn1.right;

                name = node.ToString();
                last_name = last.ToString();
                type = Type.DOT_NODE;
                if (last is indexer)
                    type = Type.INDEXER;
            }
            else if (node is ident id)
            {
                name = id.name;
                last_name = name;
                if (binder != null)
                {
                    var info = binder.bind(id);
                    if (isVarParam(info))
                    {
                        type = Type.VAR_PARAM;
                    }
                    else if (isFromOuterScope(info))
                    {
                        type = Type.FROM_OUTER_SCOPE;
                    }
                    else
                    {
                        type = Type.LOCAL;
                    }
                }
            }

            else if (node is get_address || node is roof_dereference)
            {
                type = Type.POINTER;
            }
            else
            {
                type = Type.EXPR;
                name = node.ToString();
                last_name = name;
            }
        }

        public bool StructurallyEquals(Symbol sym) => node.ToString() == sym.node.ToString();

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj.GetType() != GetType()) return false;
            return name == (obj as Symbol).name;
        }

        public override int GetHashCode()
        {
            return 363513814 + EqualityComparer<string>.Default.GetHashCode(name);
        }


        static bool isFromOuterScope(BindResult bindRes)
        {
            var symbol = bindRes.symInfo;

            if (symbol == null)
                return true;

            if (symbol.SK == SymKind.var && symbol.Attr.HasFlag(SymbolAttributes.varparam_attr))
                return true;

            if (bindRes.path[bindRes.path.Count - 1] is GlobalScopeSyntax)
                return true;

            for (var i = 0; i < bindRes.path.Count - 1; i++)
                if (bindRes.path[i] is NamedScopeSyntax)
                    return true;

            return false;
        }

        static bool isVarParam(BindResult res)
        {
            if (res == null)
                return true;
            return res.symInfo.Attr.HasFlag(SymbolAttributes.varparam_attr);
        }
    }

    public class TempSymbol : Symbol
    {
        private static int counter = 0;
        private static string getTempPrefix => "<tup_opt>" + counter++;
        public TempSymbol() : base(new ident(getTempPrefix), null)
        {
            type = Type.LOCAL;
        }
    }
}