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


        private class SynonymsGraph {

            public enum  Type { SAME_NAME, INDEXER}

            public HashSet<int> left = new HashSet<int>();
            public HashSet<int> right = new HashSet<int>();

            public Type type;

            public SynonymsGraph(Type type)
            {
                this.type = type;
            }

            public override string ToString()
            {
                var res = "";
                if (type == Type.INDEXER)
                    res += "Indexer";
                else
                    res += "Same_Name";
                res += "graph(left[";
                foreach (var i in left) res += i.ToString() + ",";
                res += "], right[";
                foreach(var i in right) res += i.ToString() + ",";
                res += "])";
                return res;
            }

        }

        public static AssignGraph createAssignGraph(List<Symbol> left, List<Symbol> right)
        {
            if (right.Count < left.Count) throw new Exception("AAAA");

         
            List<SymbolNode> v_left = new List<SymbolNode>();
            List<SymbolNode> v_right = new List<SymbolNode>();
            List<Symbol> left_visited = new List<Symbol>();
            List<Edge> assign_first = new List<Edge>();
            List<Edge> assign_last = new List<Edge>();

            void addTemp(int i, bool toFirst)
            {
                if(toFirst)
                {
                    var temp_symbol = new TempSymbol();
                    assign_first.Add(new Edge(new SymbolNode(right[i]), new TempSymbolNode(temp_symbol)));
                    right[i] = temp_symbol;
                }
                else
                {
                    var temp_s = new TempSymbol();
                    assign_last.Add(new Edge(new TempSymbolNode(temp_s), new SymbolNode(left[i])));
                    left[i] = temp_s;
                }
            }

            //handle expressions and var params
            for (int i = 0; i < left.Count(); i++)
            {
                if (right[i].type == Symbol.Type.EXPR || right[i].type == Symbol.Type.VAR_PARAM)
                {
                    addTemp(i, true);
                }
                if (left[i].type == Symbol.Type.VAR_PARAM)
                {
                    addTemp(i, false);
                }
            }

            for (int i = left.Count - 1; i > -1; i--)
            {
                //delete loops
                if (left[i].StructurallyEquals(right[i]))
                {
                    var f = new SymbolNode(right[i]);
                    var t = new SymbolNode(left[i]);
                    assign_first.Add(new Edge(f, t));
                    left.RemoveAt(i);
                    right.RemoveAt(i);
                }
                // if repeated assigns or pointers -> 2n
                else if (left_visited.Find(it => it.StructurallyEquals(left[i])) != null
                    || right[i].type == Symbol.Type.POINTER
                    || left[i].type == Symbol.Type.POINTER)
                {
                    var res = new AssignGraph();
                    res.assignFirst = make2n(left, right);
                    return res;
                }
                else
                    left_visited.Add(left[i]);
            }


            var indexerGraph = new SynonymsGraph(SynonymsGraph.Type.INDEXER);
            var nameToGraph = new Dictionary<string, SynonymsGraph>();



            for (int i = 0; i < left.Count; i++)
            {
                if (left[i].type == Symbol.Type.INDEXER)
                    indexerGraph.left.Add(i);
                else if (left[i].type == Symbol.Type.FROM_OUTER_SCOPE || left[i].type == Symbol.Type.DOT_NODE)
                {
                    var name = left[i].last_name;
                    if (!nameToGraph.ContainsKey(name))
                        nameToGraph.Add(name, new SynonymsGraph(SynonymsGraph.Type.SAME_NAME));
                    nameToGraph[name].left.Add(i);
                }
                if (right[i].type == Symbol.Type.INDEXER)
                    indexerGraph.right.Add(i);
                else if (right[i].type == Symbol.Type.FROM_OUTER_SCOPE || right[i].type == Symbol.Type.DOT_NODE)
                {
                    var name = right[i].last_name;
                    if (!nameToGraph.ContainsKey(name))
                        nameToGraph.Add(name, new SynonymsGraph(SynonymsGraph.Type.SAME_NAME));
                    nameToGraph[name].right.Add(i);
                }
            }


            var leftSet = new HashSet<Symbol>(left);
            var rightSet = new HashSet<Symbol>(right);
            var graphs = new List<SynonymsGraph>();
            graphs.AddRange(nameToGraph.Values);
            graphs.Add(indexerGraph);
            var moved_first = new List<Edge>();
            var moved_last = new LinkedList<Edge>();
            var movedAssigns = new HashSet<int>();


            void moveAssign(int i, bool toFirst )
            {
                Edge assign = new Edge(new SymbolNode(right[i]), new SymbolNode(left[i]));
                if (toFirst)
                    moved_first.Add(assign);
                else
                    moved_last.AddFirst(assign);
                movedAssigns.Add(i);
            }

            void resolveGraph(SynonymsGraph sGraph)
            {
          
                foreach (var i in sGraph.left)
                {
                    var type = right[i].type;
                    if (type == Symbol.Type.LOCAL)
                    {
                        if (!leftSet.Contains(right[i]))
                            moveAssign(i, false);
                    }
                    else if (type == Symbol.Type.FROM_OUTER_SCOPE)
                    {
                        if (!(leftSet.Contains(right[i]) || nameToGraph[right[i].last_name].left.Count() != 0))
                            moveAssign(i, false);
                    }
                    else if (type == Symbol.Type.DOT_NODE)
                    {
                        if (nameToGraph[right[i].last_name].left.Count() == 0)
                            moveAssign(i, false);
                    }
                    else if (type == Symbol.Type.INDEXER)
                    {
                        if (indexerGraph.left.Count() == 0)
                            moveAssign(i, false);
                    }
                }

                foreach (var i in sGraph.right)
                {
                    var type = left[i].type;
                    if (type == Symbol.Type.LOCAL)
                    {
                        if (!right.Contains(left[i]))
                           moveAssign(i, true);
                    }
                    else if (type == Symbol.Type.FROM_OUTER_SCOPE)
                    {
                        if (!(rightSet.Contains(left[i]) || nameToGraph[left[i].last_name].right.Count() != 0))
                            moveAssign(i, true);
                    }
                    else if (type == Symbol.Type.DOT_NODE)
                    {
                        if (nameToGraph[left[i].last_name].right.Count() == 0)
                            moveAssign(i, true);
                    }
                    else if (type == Symbol.Type.INDEXER)
                    {
                        if (indexerGraph.right.Count() == 0)
                            moveAssign(i, true);
                    }
                }
                foreach(int i in movedAssigns)
                {
                    sGraph.left.Remove(i);
                    sGraph.right.Remove(i);
                }

                var l_size = sGraph.left.Count;
                var r_size = sGraph.right.Count;
                if (r_size == 0 || l_size == 0)
                    return;

                Edge movedFromThisGraph = null;
                if (l_size < r_size)
                {
                    foreach (var i in sGraph.left)
                    {
                        if (sGraph.right.Contains(i))
                        {
                            if (movedFromThisGraph == null)
                            {
                                movedFromThisGraph = new Edge(new SymbolNode(right[i]), new SymbolNode(left[i]));
                                movedAssigns.Add(i);
                            }
                            else
                                addTemp(i, false);

                        }
                        else
                            addTemp(i, false);
                    }
                    if (movedFromThisGraph != null)
                        moved_last.AddFirst(movedFromThisGraph);
                    sGraph.left.Clear();
                }
                else
                {
                    foreach (var i in sGraph.right)
                    {
                        if (sGraph.left.Contains(i))
                        {

                            if (movedFromThisGraph == null)
                            {
                                movedFromThisGraph = new Edge(new SymbolNode(right[i]), new SymbolNode(left[i]));
                                movedAssigns.Add(i);
                            }
                            else
                                addTemp(i, true);
                        }
                        else
                            addTemp(i, true);
                        
                    }
                    if (movedFromThisGraph != null)
                        moved_first.Add(movedFromThisGraph);
                    sGraph.right.Clear();
                }
            }
            


            foreach (var g in graphs)
                resolveGraph(g);

            Console.WriteLine("graphs resolved");
            for (int i = left.Count - 1; i >= 0; i--)
            {
                if (movedAssigns.Contains(i))
                {
                    left.RemoveAt(i);
                    right.RemoveAt(i);
                    movedAssigns.Remove(i);
                }
            }
            foreach (var s in left) Console.Write(s.name + ", ");
            Console.WriteLine();
            foreach (var s in right) Console.Write(s.name + ", ");
            
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


            assign_first.AddRange(moved_first);
            foreach(var assign in assign_last)
                moved_last.AddLast(assign);
            
            assign_last = moved_last.ToList();

            var graph = new AssignGraph(assigns);
            graph.assignFirst = assign_first;
            graph.assignLast = assign_last;
            return graph;
        }
        



        private static List<Edge> make2n(List<Symbol> left, List<Symbol> right)
        {
            var temps = new List<SymbolNode>();
            var res = new List<Edge>();
            
            for(int i = 0; i < right.Count; i++)
            {
                var to = new SymbolNode(new TempSymbol());
                var from = new SymbolNode(right[i]); 
                temps.Add(to);
                res.Add(new Edge(from, to));
            }

            
            for (int i = 0; i < left.Count; i++)
            {
                var to = new SymbolNode(left[i]);
                res.Add(new Edge(temps[i], to));
            }
            return res;
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