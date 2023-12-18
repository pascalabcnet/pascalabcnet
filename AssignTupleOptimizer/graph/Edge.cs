using System;

namespace AssignTupleDesugar
{
    internal class Edge : IEquatable<Edge>, IComparable<Edge>
    {
        public SymbolNode from;
        public SymbolNode to;
        public int weight;

        public Edge(SymbolNode f, SymbolNode t, int w = 0)
        {
            if (weight < 0) throw new ArgumentException("weight can't be negative!");
            from = f;
            to = t;
            weight = w;
        }

        public int CompareTo(Edge other)
        {
            return weight - other.weight;
        }

        public bool Equals(Edge other)
        {
            if (other is null) return false;
            return from == other.from && to == other.to;
        }
    }
}