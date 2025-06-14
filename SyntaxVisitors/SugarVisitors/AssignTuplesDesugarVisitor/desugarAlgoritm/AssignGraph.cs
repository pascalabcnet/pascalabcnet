using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AssignTupleDesugarAlgorithm
{
    internal class AssignGraph : IEnumerable<SymbolNode>
    {
        public readonly HashSet<SymbolNode> vertexes = new HashSet<SymbolNode>();

        public readonly List<Edge> edges = new List<Edge>();
        private bool AdjStructuresNeedSync = false;

        public List<Edge> assignOrder = new List<Edge>();
        public List<Edge> assignLast = new List<Edge>();
        public List<Edge> assignFirst = new List<Edge>();

        private Dictionary<SymbolNode, List<SymbolNode>> outAdjStructure = null;

        public Dictionary<SymbolNode, List<SymbolNode>> OutAdjStructure
        {
            get
            {
                if (outAdjStructure == null || AdjStructuresNeedSync)
                {
                    outAdjStructure = createOutAdjStructure();
                    inAdjStructure = createInAdjStructure();
                    AdjStructuresNeedSync = false;
                }

                return outAdjStructure;
            }
        }

        private Dictionary<SymbolNode, List<SymbolNode>> createOutAdjStructure()
        {
            Dictionary<SymbolNode, List<SymbolNode>> res = new Dictionary<SymbolNode, List<SymbolNode>>();

            foreach (var vert in vertexes) res.Add(vert, new List<SymbolNode>());

            foreach (Edge edge in edges) res[edge.from].Add(edge.to);

            return res;
        }

        private Dictionary<SymbolNode, List<SymbolNode>> inAdjStructure = null;

        public Dictionary<SymbolNode, List<SymbolNode>> InAdjStructure
        {
            get
            {
                if (inAdjStructure == null || AdjStructuresNeedSync)
                {
                    outAdjStructure = createOutAdjStructure();
                    inAdjStructure = createInAdjStructure();
                    AdjStructuresNeedSync = false;
                }

                return inAdjStructure;
            }
        }

        private Dictionary<SymbolNode, List<SymbolNode>> createInAdjStructure()
        {
            Dictionary<SymbolNode, List<SymbolNode>> res = new Dictionary<SymbolNode, List<SymbolNode>>();

            foreach (var vert in vertexes) res.Add(vert, new List<SymbolNode>());

            foreach (Edge edge in edges) res[edge.to].Add(edge.from);

            return res;
        }


        public List<Edge> GetInEdgesForVertex(SymbolNode v) => edges.FindAll(edge => edge.to == v);
        public List<Edge> GetOutEdgesForVertex(SymbolNode v) => edges.FindAll(edge => edge.from == v);

        public void resetVertexesColor()
        {
            foreach (SymbolNode v in vertexes) v.resetColor();
        }

        public bool EnsureThatEveryVertexHasOneOrZeroInEdge() =>
            vertexes.All(vert => GetInEdgesForVertex(vert).Count <= 1);

        internal List<Edge> GetAssignOrder()
        {
            List<Edge> assignOrder = new List<Edge>();
            assignOrder.AddRange(assignFirst);

            if (vertexes.Count == 0)
            {
                assignOrder.AddRange(assignLast);
                this.assignOrder = assignOrder;
                return this.assignOrder;
            }

            var s = vertexes.First();

            if (!EnsureThatEveryVertexHasOneOrZeroInEdge()) throw new Exception("Invalid assign graph!");

            var cycles = this.findAllUniqueElementaryCycles();

            foreach (var cycle in cycles)
            {
                SymbolNode cut_place = cycle[0];
                Edge edge_to_cut = GetInEdgesForVertex(cut_place).First();

                SymbolNode temp_assign_from = edge_to_cut.from;
                SymbolNode temp_vertex = new TempSymbolNode(new TempSymbol());

                assignOrder.Add(new Edge(temp_assign_from, temp_vertex));
                assignLast.Add(new Edge(temp_vertex, cut_place));

                edges.Remove(edge_to_cut);
                AdjStructuresNeedSync = true;
            }

            List<SymbolNode> roots = vertexes.ToList().FindAll(vertex => GetInEdgesForVertex(vertex).Count == 0);

            foreach (SymbolNode root in roots) treeTraversal(root, assignOrder);

            assignOrder.AddRange(assignLast);
            this.assignOrder = assignOrder;
            return this.assignOrder;
        }

        void treeTraversal(SymbolNode root, List<Edge> assignOrder)
        {
            foreach (SymbolNode node in OutAdjStructure[root]) treeTraversal(node, assignOrder);

            var inEdges = GetInEdgesForVertex(root);
            if (inEdges.Count() > 0) assignOrder.Add(inEdges.First());
        }

        public AssignGraph(List<Edge> edges, List<SymbolNode> vertexes) : this(edges)
        {
            foreach (SymbolNode vert in vertexes) this.vertexes.Add(vert);
        }

        public AssignGraph()
        {

        }

        public AssignGraph(List<Edge> e)
        {
            edges = e;
            foreach (var edge in edges)
            {
                vertexes.Add(edge.from);
                vertexes.Add(edge.to);
            }
        }
        public IEnumerator<SymbolNode> GetEnumerator()
        {
            return ((IEnumerable<SymbolNode>)vertexes).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)vertexes).GetEnumerator();
        }
    }
}