using System;

namespace AssignTupleDesugarAlgorithm
{

    internal class SymbolNode : IEquatable<SymbolNode>
    {
        public readonly string label;
        public int? number = null;
        public readonly Symbol symbol;

        public SymbolNode(Symbol s)
        {
            symbol = s;
            label = s.name;
        }

        public enum Color
        {
            WHITE,
            GREY,
            BLACK
        }

        public Color color = Color.WHITE;
        public void resetColor() => color = Color.WHITE;

        public bool Equals(SymbolNode other)
        {
            if (other is null) return false;

            return label == other.label;
        }

        public override int GetHashCode()
        {
            return label.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (GetType() != other.GetType()) return false;
            return Equals((SymbolNode)other);
        }


        public override string ToString()
        {
            string n = "";
            if (number != null)
            {
                n = "(" + number.ToString() + ")";
            }

            return label + n;
        }
    }

    internal class TempSymbolNode : SymbolNode
    {
        public TempSymbolNode(TempSymbol s) : base(s)
        {

        }
    }

}