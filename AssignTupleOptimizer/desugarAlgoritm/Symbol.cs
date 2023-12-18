using System.Collections.Generic;
using System.Diagnostics;

namespace AssignTupleDesugarAlgorithm
{
    public class Symbol
    {
        public readonly string name;

        public bool fromOuterScope = false;

        public bool isExpr = false;

        public Symbol(string n)
        {
            name = n;
        }


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


    }

    public class TempSymbol : Symbol
    {

        private static int counter = 0;

        private static string getTempPrefix => "$temp_" + counter++;
        public TempSymbol(string name) : base(getTempPrefix + name)
        {
            fromOuterScope = false;
            isExpr = false;
        }

    }

}