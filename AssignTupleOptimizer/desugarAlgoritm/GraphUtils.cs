using AssignTupleDesugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace AssignTupleDesugarAlgorithm
{
    internal static class GraphUtils
    {
        static private void dfs(this AssignGraph graph,
            Action<AssignGraph> OnEnterGraph,
            Func<AssignGraph, SymbolNode, bool> OnProcessVertex,
            Action<AssignGraph, SymbolNode> OnEnterNode,
            Func<AssignGraph, SymbolNode, SymbolNode, bool> OnProcessNodeChild,
            Action<AssignGraph, SymbolNode> OnLeaveNode,
            Action<AssignGraph> OnLeaveGraph,
            Func<AssignGraph, SymbolNode, IEnumerable<SymbolNode>> vertexTraversalOrderFunc = null,
            IEnumerable<SymbolNode> graphTraversalOrder = null)
        {
            void dfs(SymbolNode start_node,
                Action<AssignGraph, SymbolNode> OnEnter,
                Func<AssignGraph, SymbolNode, SymbolNode, bool> OnProcessChild,
                Action<AssignGraph, SymbolNode> OnLeave
            )
            {
                OnEnter(graph, start_node);
                IEnumerable<SymbolNode> vertexTraversalOrder;
                if (vertexTraversalOrderFunc == null) vertexTraversalOrder = graph.OutAdjStructure[start_node];
                else vertexTraversalOrder = vertexTraversalOrderFunc(graph, start_node);
                foreach (SymbolNode current_node in vertexTraversalOrder)
                {
                    if (OnProcessChild(graph, start_node, current_node))
                        dfs(current_node, OnEnter, OnProcessChild, OnLeave);
                }

                OnLeave(graph, start_node);
            }


            if (graphTraversalOrder == null) graphTraversalOrder = graph.vertexes;

            OnEnterGraph(graph);
            foreach (SymbolNode v in graphTraversalOrder)
            {
                if (OnProcessVertex(graph, v))
                    dfs(v, OnEnter: OnEnterNode, OnProcessChild: OnProcessNodeChild, OnLeave: OnLeaveNode);
            }

            OnLeaveGraph(graph);
        }

        public static AssignGraph createAssignGraph(List<Symbol> left, List<Symbol> right)
        {
            if (right.Count < left.Count) throw new Exception("AAAA");

            List<SymbolNode> v_left = new List<SymbolNode>();
            List<SymbolNode> v_right = new List<SymbolNode>();
            HashSet<Symbol> left_visited = new HashSet<Symbol>();

            //delete loops and repeated assigns
            for (int i = left.Count - 1; i > -1; i--)
            {
                if (left_visited.Contains(left[i]) || left[i].Equals(right[i]))
                {
                    left.RemoveAt(i);
                    right.RemoveAt(i);
                }
                else left_visited.Add(left[i]);
            }

            List<Edge> assign_first = new List<Edge>();
            List<Edge> assign_last = new List<Edge>();

            int leftFromOuterCount = left.Count(symbol => symbol.fromOuterScope);
            int rightFromOuterCount = right.Count(symbol => symbol.fromOuterScope);


            //handle expressions
            for (int i = 0; i < left.Count(); i++)
            {
                if (right[i].isExpr)
                {
                    var temp_symbol = new TempSymbol("");
                    assign_first.Add(new Edge(new SymbolNode(right[i]), new TempSymbolNode(temp_symbol)));
                    right[i] = temp_symbol;
                }
            }

            //handle non-local symbols
            if (leftFromOuterCount > 0 && rightFromOuterCount > 0)
            {
                if (leftFromOuterCount < rightFromOuterCount)
                {
                    for (int i = 0; i < left.Count; i++)
                    {
                        if (left[i].fromOuterScope)
                        {
                            if (left.Contains(right[i]) || right[i].fromOuterScope)
                            {

                                var temp_symbol = new TempSymbol(left[i].name);
                                assign_last.Add(new Edge(new TempSymbolNode(temp_symbol), new SymbolNode(left[i])));
                                left[i] = temp_symbol;
                            }
                            else
                            {
                                var to = new SymbolNode(left[i]);
                                var from = new SymbolNode(right[i]);
                                left.RemoveAt(i);
                                right.RemoveAt(i);
                                i--;

                                var unnecessaryAssign = assign_last.Find(assign => assign.to.Equals(to));
                                if (unnecessaryAssign != null) assign_last.Remove(unnecessaryAssign);

                                assign_last.Add(new Edge(from, to));
                            }
                        }
                    }
                }
                else
                {

                    for (int i = 0; i < left.Count; i++)
                    {
                        if (right[i].fromOuterScope)
                        {
                            if (right.Contains(left[i]) || left[i].fromOuterScope)
                            {
                                var temp_symbol = new TempSymbol(right[i].name);
                                assign_first.Add(new Edge(new SymbolNode(right[i]), new TempSymbolNode(temp_symbol)));
                                right[i] = temp_symbol;
                            }
                            else
                            {
                                var to = new SymbolNode(left[i]);
                                var from = new SymbolNode(right[i]);
                                left.RemoveAt(i);
                                right.RemoveAt(i);
                                i--;

                                var unnecessaryAssign = assign_first.Find(assign => assign.to.Equals(to));
                                if (unnecessaryAssign != null) assign_first.Remove(unnecessaryAssign);

                                assign_first.Add(new Edge(from, to));
                            }
                        }
                    }
                }
            }



            var symbols = new Dictionary<string, SymbolNode>();

            foreach (Symbol sym in left)
            {
                if (!symbols.ContainsKey(sym.name)) symbols[sym.name] = new SymbolNode(sym);
                v_left.Add(symbols[sym.name]);
            }

            foreach (Symbol sym in right)
            {
                if (!symbols.ContainsKey(sym.name)) symbols[sym.name] = new SymbolNode(sym);
                v_right.Add(symbols[sym.name]);
            }

            List<Edge> assigns = new List<Edge>();

            for (int i = 0; i < v_left.Count; i++) assigns.Add(new Edge(v_right[i], v_left[i], i));


            var graph = new AssignGraph(assigns);
            graph.assignFirst = assign_first;
            graph.assignLast = assign_last;
            return graph;
        }



        private static AssignGraph createReversedGraph(this AssignGraph g)
        {
            List<Edge> newEdges = new List<Edge>();
            foreach (var edge in g.edges)
            {
                newEdges.Add(new Edge(f: edge.to, t: edge.from));
            }

            return new AssignGraph(newEdges, g.vertexes.ToList());
        }

        public static List<List<SymbolNode>> findStronglyConnectedComponets(this AssignGraph g)
        {
            List<List<SymbolNode>> res = new List<List<SymbolNode>>();

            Stack<SymbolNode> route = new Stack<SymbolNode>();
            HashSet<SymbolNode> visited = new HashSet<SymbolNode>();

            Action<AssignGraph> onEnterGraph = (_) =>
            {
                route.Clear();
                visited.Clear();
            };

            Action<AssignGraph, SymbolNode> onEnterNode = (graph, node) => { visited.Add(node); };

            Action<AssignGraph, SymbolNode> onLeaveNode = (graph, node) => { route.Push(node); };

            Func<AssignGraph, SymbolNode, SymbolNode, bool> onProcessChildNode = (graph, parent, childNode) =>
            {
                if (!visited.Contains(childNode))
                {
                    return true;
                }
                else return false;
            };

            Func<AssignGraph, SymbolNode, bool> onProcessVertex = (graph, vert) => { return !visited.Contains(vert); };

            g.dfs(OnEnterGraph: onEnterGraph, OnProcessVertex: onProcessVertex,
                OnEnterNode: onEnterNode, OnProcessNodeChild: onProcessChildNode,
                OnLeaveNode: onLeaveNode,
                OnLeaveGraph: (_) => { }
            );

            Action<AssignGraph> onEnterGraphReversed = (_) => { visited.Clear(); };

            Action<AssignGraph, SymbolNode> onEnterNodeReversed = (graph, node) =>
            {
                visited.Add(node);
                res.Last().Add(node);
            };

            Action<AssignGraph, SymbolNode> onLeaveNodeReversed = (graph, node) => { };

            Func<AssignGraph, SymbolNode, SymbolNode, bool> onProcessChildNodeReversed = (graph, parent, childNode) =>
            {
                if (!visited.Contains(childNode))
                {
                    return true;
                }
                else return false;
            };

            Func<AssignGraph, SymbolNode, bool> onProcessVertexReversed = (graph, vert) =>
            {
                if (visited.Contains(vert)) return false;
                else
                {
                    res.Add(new List<SymbolNode>());
                    return true;
                }
            };

            var graphTraversalOrder = route.ToList();

            var g_rev = g.createReversedGraph();

            g_rev.dfs(OnEnterGraph: onEnterGraphReversed, OnProcessVertex: onProcessVertexReversed,
                OnEnterNode: onEnterNodeReversed, OnProcessNodeChild: onProcessChildNodeReversed,
                OnLeaveNode: onLeaveNodeReversed,
                OnLeaveGraph: (_) => { },
                graphTraversalOrder: graphTraversalOrder);

            return res;
        }

        public static List<Cycle> findAllUniqueElementaryCycles(this AssignGraph g)
        {
            var res = new List<Cycle>();
            var components = g.findStronglyConnectedComponets();
            foreach (var component in components)
            {
                if (component.Count() > 1) res.Add(new Cycle(component));
            }
            return res;
        }

        public static void LogGraph(this AssignGraph graph)
        {
            foreach (var vert in graph.vertexes)
            {
                Console.Write("{0}->[", vert);
                graph.OutAdjStructure[vert].ForEach(v => Console.Write("{0}, ", v));
                Console.WriteLine("]");
            }
        }

        public static void drawGraph(this AssignGraph g)
        {
            var writer = File.CreateText("graph.dot");
            writer.WriteLine("digraph _graph {");

            Action<AssignGraph> enterGraph = (graph) => { graph.resetVertexesColor(); };

            Func<AssignGraph, SymbolNode, bool> ProcessNode = (graph, node) => node.color != SymbolNode.Color.GREY;

            Action<AssignGraph, SymbolNode> EnterNode = (graph, vert) => { vert.color = SymbolNode.Color.GREY; };

            Func<AssignGraph, SymbolNode, SymbolNode, bool> ProcessNodeChild = (graph, parent, child) =>
            {
                writer.WriteLine(parent.label + "->" + child.label);
                return child.color == SymbolNode.Color.WHITE;
            };

            g.dfs(OnEnterGraph: enterGraph, OnEnterNode: EnterNode, OnProcessNodeChild: ProcessNodeChild,
                OnProcessVertex: ProcessNode, OnLeaveGraph: (graph) => { graph.resetVertexesColor(); }, OnLeaveNode: (graph, vertex) => { });
            writer.WriteLine("}");
            writer.Close();
        }
    }
}