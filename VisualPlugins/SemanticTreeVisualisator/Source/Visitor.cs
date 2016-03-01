using PascalABCCompiler.SemanticTree;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;

namespace VisualPascalABCPlugins
{            
    public class SematicTreeVisitor : ISemanticVisitor
    {

        public class myTreeNode : TreeNode
        {
            public bool is_built = false;
            public bool is_leaf = false;
        }

        public System.Windows.Forms.TreeNodeCollection nodes;

        public static Dictionary<ISemanticNode, TreeNode> table_subnodes;      
        public static Dictionary<TreeNode, ISemanticNode> table_up_rows;


        public static Dictionary<TreeNode, TreeNode> table_links;



        public static Dictionary<TreeNode, TreeNode> table_for_up_links;


        public static List<ISemanticNode> compiled_types;
        public static Dictionary<ISemanticNode, myTreeNode> compiled_types_dictionary;

        public static List<ISemanticNode> builded_nodes;

        public static TreeView treeView;       

        public void visitSelectedNode(object obj)
        {
            if (table_for_up_links.ContainsKey(treeView.SelectedNode))
            {
                treeView.SelectedNode = table_for_up_links[treeView.SelectedNode];                

                treeView.Invalidate();
                treeView.Refresh();
                treeView.Update();
                
            }
            else
            {
                if (treeView.SelectedNode == null) treeView.SelectedNode = treeView.Nodes[0];
                if (treeView.SelectedNode.Tag != null)
                {
                    if (!(treeView.SelectedNode as SematicTreeVisitor.myTreeNode).is_built)// && !(treeView.SelectedNode as SematicTreeVisitor.myTreeNode).is_leaf)
                    {
                        //MessageBox.Show(treeView.SelectedNode.Tag.GetType().ToString());

                        if ((treeView.SelectedNode.Tag is PascalABCCompiler.SemanticTree.ISemanticNode))
                        {

                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.common_namespace_function_node")                            
                                visit(treeView.SelectedNode.Tag as ICommonNamespaceFunctionNode);
                                                          

                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.local_variable")
                                visit(treeView.SelectedNode.Tag as ILocalVariableNode);        


                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.statements_list")                           
                                visit(treeView.SelectedNode.Tag as IStatementsListNode);
                              

                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.common_namespace_node")                            
                                visit(treeView.SelectedNode.Tag as ICommonNamespaceNode);
                                

                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.namespace_constant_definition")                            
                                visit(treeView.SelectedNode.Tag as INamespaceConstantDefinitionNode);                                


                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.basic_function_call")                            
                                visit(treeView.SelectedNode.Tag as IBasicFunctionCallNode);
                                

                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.common_namespace_function_call")
                                visit(treeView.SelectedNode.Tag as ICommonNamespaceFunctionCallNode);                               


                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.local_block_variable")
                                visit(treeView.SelectedNode.Tag as ILocalBlockVariableNode);
                                

                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.parameter")
                                visit(treeView.SelectedNode.Tag as IParameterNode);

                               
                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.PCU.wrapped_common_type_node")
                                visit(treeView.SelectedNode.Tag as ICommonTypeNode);
                               

                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.common_method_node")
                                visit(treeView.SelectedNode.Tag as ICommonMethodNode);

                               
                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.class_field")
                                visit(treeView.SelectedNode.Tag as ICommonClassFieldNode);
                               

                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.return_node")
                            {
                                visit(treeView.SelectedNode.Tag as IReturnNode);
                                //(treeView.SelectedNode as SematicTreeVisitor.myTreeNode).is_built = true;
                                //(treeView.SelectedNode as SematicTreeVisitor.myTreeNode).is_leaf = true;
                            }

                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.for_node")
                                visit(treeView.SelectedNode.Tag as IForNode);
                               

                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.switch_node")
                                visit(treeView.SelectedNode.Tag as ISwitchNode);
                                

                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.while_node")
                                visit(treeView.SelectedNode.Tag as IWhileNode);
                                

                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.repeat_node")
                                visit(treeView.SelectedNode.Tag as IRepeatNode);
                                

                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.if_node")
                                visit(treeView.SelectedNode.Tag as IIfNode);
                                

                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.namespace_variable_reference")
                                visit(treeView.SelectedNode.Tag as INamespaceVariableReferenceNode);
                                

                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.common_parameter")
                                visit(treeView.SelectedNode.Tag as ICommonParameterNode);
                                

                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.common_parameter_reference")
                                visit(treeView.SelectedNode.Tag as ICommonParameterReferenceNode);
                                

                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.basic_function_node")
                                visit(treeView.SelectedNode.Tag as IBasicFunctionNode);
                               

                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.bool_const_node")
                                visit(treeView.SelectedNode.Tag as IBoolConstantNode);
                               

                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.common_method_call")
                                visit(treeView.SelectedNode.Tag as ICommonMethodCallNode);
                               

                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.throw_stetement")
                                visit(treeView.SelectedNode.Tag as IThrowNode);

                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.common_property_node")
                                visit(treeView.SelectedNode.Tag as ICommonPropertyNode);
                                

                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.namespace_variable")
                            {
                                if (treeView.SelectedNode.Parent.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.namespace_variable[]")
                                {
                                    visit(treeView.SelectedNode.Tag as ICommonNamespaceVariableNode);                                    
                                }
                                else
                                {
                                    myTreeNode sel_node = treeView.SelectedNode as myTreeNode;
                                    // список неймспейсов
                                        treeView.SelectedNode = treeView.Nodes[2];
                                    // ищем нужный неймспейс
                                        int i = 0;
                                        while (i < treeView.SelectedNode.Nodes.Count && treeView.SelectedNode.Nodes[i].Tag as ICommonNamespaceNode != (sel_node.Tag as ICommonNamespaceVariableNode).comprehensive_namespace)
                                        {
                                            i++;
                                        }
                                        treeView.SelectedNode = treeView.SelectedNode.Nodes[i];
                                    // переходим к списку переменных неймспейса
                                        treeView.SelectedNode = treeView.SelectedNode.Nodes[5];
                                    // ищем нужную переменную
                                        i = 0;
                                        while (i < treeView.SelectedNode.Nodes.Count && (treeView.SelectedNode.Nodes[i].Tag as ICommonNamespaceVariableNode) != sel_node.Tag as ICommonNamespaceVariableNode)
                                        {
                                            i++;
                                        }
                                    // и переходим к ней
                                        treeView.SelectedNode = treeView.SelectedNode.Nodes[i];
                                }
                            }                            



                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.compiled_type_node")
                            {
                                if (!getCompiledTypes().Contains(treeView.SelectedNode.Tag as ICompiledTypeNode)
                                    && !(treeView.SelectedNode as SematicTreeVisitor.myTreeNode).is_built)
                                {           
                                    myTreeNode from = treeView.SelectedNode as myTreeNode;
                                    ICompiledTypeNode ct = treeView.SelectedNode.Tag as ICompiledTypeNode;
                                    compiled_types.Add(treeView.SelectedNode.Tag as ICompiledTypeNode);
                                    prepare_string_node_with_tag2("compiled_type", (treeView.SelectedNode.Tag as ICompiledTypeNode).name, treeView.SelectedNode.Tag as ICompiledTypeNode);
                                    treeView.SelectedNode = treeView.Nodes[0].Nodes[treeView.Nodes[0].Nodes.Count - 1];
                                    myTreeNode to = treeView.SelectedNode as myTreeNode;
                                    compiled_types_dictionary.Add(ct, treeView.SelectedNode as myTreeNode);
                                    table_for_up_links.Add(from, to);                                     
                                }
                                else
                                {
                                    if (getCompiledTypes().Contains(treeView.SelectedNode.Tag as ICompiledTypeNode)
                                        && !(treeView.SelectedNode as SematicTreeVisitor.myTreeNode).is_built
                                        && treeView.SelectedNode.Parent == treeView.Nodes[0])
                                    {
                                        visit(treeView.SelectedNode.Tag as ICompiledTypeNode);                                                                              
                                    }
                                    else
                                    {
                                        treeView.SelectedNode = compiled_types_dictionary[treeView.SelectedNode.Tag as ICompiledTypeNode];
                                    }
                                    
                                }                                

                            }



                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.common_type_node")
                            {
                                // тип у вара в вариэвблс неймспейса                            
                                if (treeView.SelectedNode.Parent.Tag is ICommonNamespaceVariableNode)
                                {
                                    if (treeView.SelectedNode.Parent.Parent.Tag is ICommonNamespaceVariableNode[])
                                    {                                        
                                        myTreeNode tr = treeView.SelectedNode.Parent.Parent.Parent.Nodes[4] as myTreeNode;

                                        myTreeNode prom = treeView.SelectedNode as myTreeNode;
                                        treeView.SelectedNode = tr;
                                        if (!tr.is_built)
                                            prepare_collection(tr.Tag as ICommonTypeNode[], "types", "types", (tr.Tag as ICommonTypeNode[]).Length);

                                        treeView.SelectedNode = prom;

                                        int i = 0;
                                        while (i < tr.Nodes.Count && tr.Nodes[i].Tag != treeView.SelectedNode.Tag)
                                        {
                                            i++;
                                        }
                                        table_for_up_links.Add(treeView.SelectedNode as myTreeNode, tr.Nodes[i] as myTreeNode);
                                        treeView.SelectedNode = tr.Nodes[i];

                                    }
                                }
                                else
                                    // тип из списка типов неймспейса
                                    if (treeView.SelectedNode.Parent.Tag is ICommonTypeNode[])
                                    {
                                        if (treeView.SelectedNode.Parent.Parent.Tag is ICommonNamespaceNode)
                                        {
                                            visit(treeView.SelectedNode.Tag as ICommonTypeNode);                                            
                                        }
                                    }
                                    //else
                                    //{
                                    //    MessageBox.Show("poka ne razbiraem etot slychai");
                                    //}
                            }

                            (treeView.SelectedNode as SematicTreeVisitor.myTreeNode).is_built = true;
                            //(treeView.SelectedNode as SematicTreeVisitor.myTreeNode).is_leaf = true;


                        }
                        else

                            if ((treeView.SelectedNode.Tag is PascalABCCompiler.SemanticTree.ISemanticNode[]))
                            {
                                if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.local_variable[]")
                                    prepare_collection(treeView.SelectedNode.Tag as ILocalVariableNode[], "my_collection", "my_coll_element", (treeView.SelectedNode.Tag as ILocalVariableNode[]).Length);

                                if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.function_constant_definition[]")
                                    prepare_collection(treeView.SelectedNode.Tag as IConstantDefinitionNode[], "my_collection", "my_coll_element", (treeView.SelectedNode.Tag as IConstantDefinitionNode[]).Length);

                                if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.common_namespace_node[]")
                                    prepare_collection(treeView.SelectedNode.Tag as ICommonNamespaceNode[], "my_collection", "my_coll_element", (treeView.SelectedNode.Tag as ICommonNamespaceNode[]).Length);

                                if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.namespace_constant_definition[]")
                                    prepare_collection(treeView.SelectedNode.Tag as INamespaceConstantDefinitionNode[], "my_collection", "my_coll_element", (treeView.SelectedNode.Tag as INamespaceConstantDefinitionNode[]).Length);

                                if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.common_namespace_function_node[]")
                                    prepare_collection(treeView.SelectedNode.Tag as ICommonNamespaceFunctionNode[], "my_collection", "my_coll_element", (treeView.SelectedNode.Tag as ICommonNamespaceFunctionNode[]).Length);

                                if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.common_type_node[]")
                                    prepare_collection(treeView.SelectedNode.Tag as ICommonTypeNode[], "my_collection", "my_coll_element", (treeView.SelectedNode.Tag as ICommonTypeNode[]).Length);

                                if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.namespace_variable[]")
                                    prepare_collection(treeView.SelectedNode.Tag as ICommonNamespaceVariableNode[], "my_collection", "my_coll_element", (treeView.SelectedNode.Tag as ICommonNamespaceVariableNode[]).Length);

                                if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.local_block_variable[]")
                                    prepare_collection(treeView.SelectedNode.Tag as ILocalBlockVariableNode[], "my_collection", "my_coll_element", (treeView.SelectedNode.Tag as ILocalBlockVariableNode[]).Length);

                                if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.statement_node[]")
                                    prepare_collection(treeView.SelectedNode.Tag as IStatementNode[], "my_collection", "my_coll_element", (treeView.SelectedNode.Tag as IStatementNode[]).Length);

                                if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.expression_node[]")
                                    prepare_collection(treeView.SelectedNode.Tag as IExpressionNode[], "my_collection", "my_coll_element", (treeView.SelectedNode.Tag as IExpressionNode[]).Length);

                                if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.parameter[]")
                                    prepare_collection(treeView.SelectedNode.Tag as IParameterNode[], "my_collection", "my_coll_element", (treeView.SelectedNode.Tag as IParameterNode[]).Length);


                                if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.common_method_node[]")
                                    prepare_collection(treeView.SelectedNode.Tag as ICommonMethodNode[], "my_collection", "my_coll_element", (treeView.SelectedNode.Tag as ICommonMethodNode[]).Length);

                                if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.class_field[]")
                                    prepare_collection(treeView.SelectedNode.Tag as ICommonClassFieldNode[], "my_collection", "my_coll_element", (treeView.SelectedNode.Tag as ICommonClassFieldNode[]).Length);

                                if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.common_property_node[]")
                                    prepare_collection(treeView.SelectedNode.Tag as ICommonPropertyNode[], "my_collection", "my_coll_element", (treeView.SelectedNode.Tag as ICommonPropertyNode[]).Length);

                            }
                            else
                                // для implementingionterfaces
                                if (treeView.SelectedNode.Tag is System.Collections.Generic.List<PascalABCCompiler.SemanticTree.ITypeNode>)
                                    prepare_collection_up_links_list(treeView.SelectedNode.Tag as List<ITypeNode>, "my_collection", "my_coll_element", (treeView.SelectedNode.Tag as System.Collections.Generic.List<PascalABCCompiler.SemanticTree.ITypeNode>).Count);

                    }
                    else
                        // не build'нутый лист
                        if (!(treeView.SelectedNode as myTreeNode).is_built && (treeView.SelectedNode as myTreeNode).is_leaf)
                        {
                            if (treeView.SelectedNode.Tag.GetType().ToString() == "PascalABCCompiler.TreeRealization.compiled_type_node")
                            {
                                // если тип ещё не содержится в списке компайлед типов
                                    if (!compiled_types.Contains(treeView.SelectedNode.Tag as ICompiledTypeNode))
                                    {
                                        ICompiledTypeNode ct = treeView.SelectedNode.Tag as ICompiledTypeNode;
                                        compiled_types.Add(treeView.SelectedNode.Tag as ICompiledTypeNode);
                                        prepare_string_node_with_tag2("compiled_type", (treeView.SelectedNode.Tag as ICompiledTypeNode).name, treeView.SelectedNode.Tag as ICompiledTypeNode);
                                        treeView.SelectedNode = treeView.Nodes[0].Nodes[treeView.Nodes[0].Nodes.Count - 1];
                                        compiled_types_dictionary.Add(ct, treeView.SelectedNode as myTreeNode);
                                    }
                                    else
                                        // тип уже содержится в этом списке
                                    {
                                        treeView.SelectedNode = compiled_types_dictionary[treeView.SelectedNode.Tag as ICompiledTypeNode];
                                    }
                                
                            }
                        }
                }

            }
           
               

            treeView.SelectedNode.Expand();
            //treeView.Refresh();

            //treeView.Invalidate();
            //treeView.Refresh();
            //treeView.Update();

        }

     
        /////////////////////////////////////////////////////////////////////////////////////////////////////////


        public void CreateMyDictionaries()
        {
            table_subnodes = null;
            table_up_rows = null;
            table_links = null;
            table_subnodes = new Dictionary<ISemanticNode, TreeNode>();
            table_up_rows = new Dictionary<TreeNode, ISemanticNode>();
            table_links = new Dictionary<TreeNode, TreeNode>();
            compiled_types = new List<ISemanticNode>();
            table_for_up_links = new Dictionary<TreeNode, TreeNode>();
            compiled_types_dictionary = new Dictionary<ISemanticNode, myTreeNode>();
        }


        public Dictionary<ISemanticNode, TreeNode> getTableSubnodes()
        {
            return table_subnodes;
        }

        public Dictionary<TreeNode, ISemanticNode> getTableUpRows()
        {
            return table_up_rows;
        }

        public Dictionary<TreeNode, TreeNode> getTableLinks()
        {
            return table_links;
        }

        public List<ISemanticNode> getCompiledTypes()
        {
            return compiled_types;
        }
        


        // конструктор без параметров - на всякий случай
        public SematicTreeVisitor()
        {            
        }

        // конструктор с параметром nodes
        public SematicTreeVisitor(System.Windows.Forms.TreeNodeCollection nodes)
        {            
            this.nodes = nodes;
        }

        // доп. метод
        public void setNodes(System.Windows.Forms.TreeNodeCollection nodes)
        {
            this.nodes = nodes;
        }

        public void setTreeView(TreeView _treeView)
        {
            treeView = _treeView;
        }

        //  установить обратные связи
        public void makeUpRows(TreeView treeView)
        {
            foreach (TreeNode tn in table_up_rows.Keys)
            {
                ISemanticNode sem = table_up_rows[tn];
                TreeNode link;
                if (table_subnodes.ContainsKey(sem))
                {
                    link = table_subnodes[sem];

                    if (link != null)
                        table_links.Add(tn, link);
                    //else
                    //    MessageBox.Show("Exception: null-link");
                }
            }            
        }
                        

        //--------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------


        // для обычного node - потомка ISemanticNode
        public void prepare_node(ISemanticNode subnode, string node_name)
        {
            myTreeNode t;
            if (treeView.SelectedNode == null)
                t = null;
            else
                if (treeView.SelectedNode.Tag == null)
                    t = null;
                else
                    t = treeView.SelectedNode as myTreeNode;


            if (subnode != null)
            {
                myTreeNode tn = new myTreeNode();

                tn.Text = node_name + "   :   " + subnode.GetType().Name;
                tn.Tag = subnode;
                SematicTreeVisitor vs = new SematicTreeVisitor(tn.Nodes);
                
                if (t != null)
                    t.Nodes.Add(tn);
                else
                    nodes.Add(tn);

                try
                {
                    if (!table_subnodes.ContainsKey(subnode))
                        table_subnodes.Add(subnode, tn);
                }
                catch (System.Exception e)
                {
                    MessageBox.Show("Exception was \n" + e.ToString());
                }
            }
        }


        // для одного node из ICollection
        public void prepare_node_in_collection(ISemanticNode subnode, string node_name, myTreeNode tn_parent)
        {
            myTreeNode t;
            if (treeView.SelectedNode == null)
                t = null;
            else
                if (treeView.SelectedNode.Tag == null)
                    t = null;
                else
                    t = treeView.SelectedNode as myTreeNode;

            if (subnode != null)
            {
                myTreeNode tn = new myTreeNode();

                tn.Text = node_name + "   :   " + subnode.GetType().Name;
                tn.Tag = subnode;
                SematicTreeVisitor vs = new SematicTreeVisitor(tn.Nodes);
               
                tn_parent.Nodes.Add(tn);

                try
                {
                    if (!table_subnodes.ContainsKey(subnode))
                        table_subnodes.Add(subnode, tn);
                }
                catch (System.Exception e)
                {
                    MessageBox.Show("Exception was \n" + e.ToString());
                }

                //treeView.Invalidate();
            }
        }

        // для одного node из ICollection
        public void prepare_node_in_collection(ISemanticNode subnode, string node_name, myTreeNode tn_parent, bool is_leaf)
        {
            myTreeNode t;
            if (treeView.SelectedNode == null)
                t = null;
            else
                if (treeView.SelectedNode.Tag == null)
                    t = null;
                else
                    t = treeView.SelectedNode as myTreeNode;

            if (subnode != null)
            {
                myTreeNode tn = new myTreeNode();

                tn.Text = node_name + "   :   " + subnode.GetType().Name;
                tn.Tag = subnode;
                SematicTreeVisitor vs = new SematicTreeVisitor(tn.Nodes);

                tn_parent.Nodes.Add(tn);

                try
                {
                    if (!table_subnodes.ContainsKey(subnode))
                        table_subnodes.Add(subnode, tn);
                }
                catch (System.Exception e)
                {
                    MessageBox.Show("Exception was \n" + e.ToString());
                }

                //treeView.Invalidate();
                //(tn as myTreeNode).is_leaf = is_leaf;
            }
        }


        // для ICollection
        public void prepare_collection(ICollection icol, string collection_name, string item_name, int num)
        {
            myTreeNode t;
            if (treeView.SelectedNode == null)
                t = null;
            else
                if (treeView.SelectedNode.Tag == null)
                    t = null;
                else
                    t = treeView.SelectedNode as myTreeNode;

            if (num != 0)
            {
                int i = 0;
                if (t == null)
                {
                    myTreeNode tn = new myTreeNode();

                    if (icol.Count != 0)
                        tn.is_leaf = false;
                    else
                        tn.is_leaf = true;

                    tn.Text = collection_name + "   Count : " + num.ToString();
                    tn.Tag = icol;
                    if (t != null)
                        t.Nodes.Add(tn);
                    else
                        nodes.Add(tn);
                    foreach (ISemanticNode sn in icol)
                    {
                        prepare_node_in_collection(sn, item_name + "[" + i.ToString() + "]", tn);
                        i++;
                    }
                }
                else
                {                    
                    foreach (ISemanticNode sn in icol)
                    {
                        prepare_node_in_collection(sn, item_name + "[" + i.ToString() + "]", t);
                        i++;
                    }
                }
            }           


            ////if (t != null)
            ////    t.is_built = true;
            ////if (t != null && t.Tag != null)
            ////{
            ////    if ((t.Tag as ISemanticNode[]).Length != 0)
            ////        t.is_leaf = false;
            ////    else
            ////        t.is_leaf = true;
            ////}


        }


        public void prepare_collection(ICollection icol, string collection_name, string item_name, int num, bool is_leaf)
        {
            myTreeNode t;
            if (treeView.SelectedNode == null)
                t = null;
            else
                if (treeView.SelectedNode.Tag == null)
                    t = null;
                else
                    t = treeView.SelectedNode as myTreeNode;

            if (num != 0)
            {
                int i = 0;
                if (t == null)
                {
                    myTreeNode tn = new myTreeNode();

                    if (icol.Count != 0)
                        tn.is_leaf = false;
                    else
                        tn.is_leaf = true;

                    tn.Text = collection_name + "   Count : " + num.ToString();
                    tn.Tag = icol;
                    if (t != null)
                        t.Nodes.Add(tn);
                    else
                        nodes.Add(tn);
                    foreach (ISemanticNode sn in icol)
                    {
                        prepare_node_in_collection(sn, item_name + "[" + i.ToString() + "]", tn, is_leaf);
                        i++;
                    }
                }
                else
                {
                    foreach (ISemanticNode sn in icol)
                    {
                        prepare_node_in_collection(sn, item_name + "[" + i.ToString() + "]", t);
                        i++;
                    }
                }
            }


            ////if (t != null)
            ////    t.is_built = true;
            ////if (t != null && t.Tag != null)
            ////{
            ////    if ((t.Tag as ISemanticNode[]).Length != 0)
            ////        t.is_leaf = false;
            ////    else
            ////        t.is_leaf = true;
            ////}


        }




        // для ICollection
        public void prepare_collection_list(ICollection icol, string collection_name, string item_name, int num)
        {
            myTreeNode t;
            if (treeView.SelectedNode == null)
                t = null;
            else
                if (treeView.SelectedNode.Tag == null)
                    t = null;
                else
                    t = treeView.SelectedNode as myTreeNode;

            if (num != 0)
            {
                int i = 0;
                if (t == null)
                {
                    myTreeNode tn = new myTreeNode();

                    if (icol.Count != 0)
                        tn.is_leaf = false;
                    else
                        tn.is_leaf = true;

                    tn.Text = collection_name + "   Count : " + num.ToString();
                    tn.Tag = icol;
                    if (t != null)
                        t.Nodes.Add(tn);
                    else
                        nodes.Add(tn);
                    foreach (ISemanticNode sn in icol)
                    {
                        prepare_node_in_collection(sn, item_name + "[" + i.ToString() + "]", tn);
                        i++;
                    }

                  
                }
                else
                {                    
                    foreach (ISemanticNode sn in icol)
                    {
                        prepare_node_in_collection(sn, item_name + "[" + i.ToString() + "]", t);
                        i++;
                    }
                }
            }            

            ////if (t != null)
            ////    t.is_built = true;
            ////if (t != null && t.Tag != null)
            ////{
            ////    if ((t.Tag as System.Collections.Generic.List<ITypeNode>).Count != 0)
            ////        t.is_leaf = false;
            ////    else
            ////        t.is_leaf = true;
            ////}


        }



        // для ICollection
        // если коллекция - список
        public void prepare_collection_list(ICollection icol, string collection_name, string item_name, int num, bool is_leaf)
        {
            myTreeNode t;
            if (treeView.SelectedNode == null)
                t = null;
            else
                if (treeView.SelectedNode.Tag == null)
                    t = null;
                else
                    t = treeView.SelectedNode as myTreeNode;

            if (num != 0)
            {
                int i = 0;
                if (t == null)
                {
                    myTreeNode tn = new myTreeNode();

                    if (icol.Count != 0)
                        tn.is_leaf = false;
                    else
                        tn.is_leaf = true;

                    tn.Text = collection_name + "   Count : " + num.ToString();
                    tn.Tag = icol;
                    if (t != null)
                        t.Nodes.Add(tn);
                    else
                        nodes.Add(tn);
                    foreach (ISemanticNode sn in icol)
                    {
                        prepare_node_in_collection(sn, item_name + "[" + i.ToString() + "]", tn, is_leaf);
                        i++;
                    }


                }
                else
                {
                    foreach (ISemanticNode sn in icol)
                    {
                        prepare_node_in_collection(sn, item_name + "[" + i.ToString() + "]", t, is_leaf);
                        i++;
                    }
                }
            }

            ////if (t != null)
            ////    t.is_built = true;
            ////if (t != null && t.Tag != null)
            ////{
            ////    if ((t.Tag as System.Collections.Generic.List<ITypeNode>).Count != 0)
            ////        t.is_leaf = false;
            ////    else
            ////        t.is_leaf = true;
            ////}


        }



        public void prepare_base_node_collection(ICollection icol, string collection_name, string item_name, int num)
        {
            myTreeNode t;
            if (treeView.SelectedNode == null)
                t = null;
            else
                if (treeView.SelectedNode.Tag == null)
                    t = null;
                else
                    t = treeView.SelectedNode as myTreeNode;

            if (num != 0)
            {
                int i = 0;
                if (t == null)
                {
                    myTreeNode tn = new myTreeNode();

                    tn.Text = collection_name + "   Count : " + num.ToString();
                    tn.Tag = icol;
                    
                        nodes.Add(tn);
                   
                }
                else
                {

                    myTreeNode tn = new myTreeNode();

                    tn.Text = collection_name + "   Count : " + num.ToString();
                    tn.Tag = icol;
                   
                    t.Nodes.Add(tn);                    
                }
            }
            else
            {
                myTreeNode tn = new myTreeNode();

                tn.Text = collection_name + "   Count : 0";
                tn.Tag = icol;
 
                if (t != null)
                    t.Nodes.Add(tn);
                else
                    nodes.Add(tn);
            }
        }



        // для ICollection
        public void prepare_collection_up_links(ICollection icol, string collection_name, string item_name, int num)
        {
            myTreeNode t;
            if (treeView.SelectedNode == null)
                t = null;
            else
                if (treeView.SelectedNode.Tag == null)
                    t = null;
                else
                    t = treeView.SelectedNode as myTreeNode;

            if (num != 0)
            {
                int i = 0;

                myTreeNode tn = new myTreeNode();

                tn.Text = collection_name + "   Count : " + num.ToString();
                tn.Tag = icol;
    
                if (t != null)
                    t.Nodes.Add(tn);
                else
                    nodes.Add(tn);
                foreach (ISemanticNode sn in icol)
                {
                    prepare_node_in_collection_up_link(sn, item_name + "[" + i.ToString() + "]", tn);
                    i++;
                }
            }
            else
            {
                myTreeNode tn = new myTreeNode();

                tn.Text = collection_name + "   Count : 0";
                tn.Tag = icol;
      
                if (t != null)
                    t.Nodes.Add(tn);
                else
                    nodes.Add(tn);
            }
        }



        // только для имплементинг интерфейсес

        public void prepare_collection_up_links_list(ICollection icol, string collection_name, string item_name, int num)
        {
            myTreeNode t;
            if (treeView.SelectedNode == null)
                t = null;
            else
                if (treeView.SelectedNode.Tag == null)
                    t = null;
                else
                    t = treeView.SelectedNode as myTreeNode;

            if (num != 0)
            {
                int i = 0;
                if (t == null)
                {
                    myTreeNode tn = new myTreeNode();

                    if (icol.Count != 0)
                        tn.is_leaf = false;
                    else
                        tn.is_leaf = true;

                    tn.Text = collection_name + "   Count : " + num.ToString();
                    tn.Tag = icol;
                    if (t != null)
                        t.Nodes.Add(tn);
                    else
                        nodes.Add(tn);
                    foreach (ISemanticNode sn in icol)
                    {
                        prepare_node_in_collection_up_link(sn, item_name + "[" + i.ToString() + "]", tn);
                        i++;
                    }


                }
                else
                {
                    foreach (ISemanticNode sn in icol)
                    {
                        prepare_node_in_collection_up_link(sn, item_name + "[" + i.ToString() + "]", t);
                        i++;
                    }
                }
            }

            //////if (t != null)
            //////    t.is_built = true;
            //////if (t != null && t.Tag != null)
            //////{
            //////    if ((t.Tag as System.Collections.Generic.List<ITypeNode>).Count != 0)
            //////        t.is_leaf = false;
            //////    else
            //////        t.is_leaf = true;
            //////}


        }



        public void prepare_base_node_collection_up_links(ICollection icol, string collection_name, string item_name, int num)
        {
            myTreeNode t;
            if (treeView.SelectedNode == null)
                t = null;
            else
                if (treeView.SelectedNode.Tag == null)
                    t = null;
                else
                    t = treeView.SelectedNode as myTreeNode;

            if (num != 0)
            {
                int i = 0;

                myTreeNode tn = new myTreeNode();

                tn.Text = collection_name + "   Count : " + num.ToString();
                tn.Tag = icol;

                if (t != null)
                    t.Nodes.Add(tn);
                else
                    nodes.Add(tn);
                
            }
            else
            {
                myTreeNode tn = new myTreeNode();

                tn.Text = collection_name + "   Count : 0";
                tn.Tag = icol;

                if (t != null)
                    t.Nodes.Add(tn);
                else
                    nodes.Add(tn);
            }
        }




        // для строкового поля
        public void prepare_string_node(string str, string text)
        {
            myTreeNode t;
            if (treeView.SelectedNode == null)
                t = null;
            else
                if (treeView.SelectedNode.Tag == null)
                    t = null;
                else
                    t = treeView.SelectedNode as myTreeNode;
            myTreeNode tn = new myTreeNode();

            tn.Text = text + "   :   " + str;
            tn.Tag = str;
      
            if (t != null)
                t.Nodes.Add(tn);
            else
                nodes.Add(tn);
        }


        // для строкового поля
        public void prepare_string_node_with_tag(string str, string text, ISemanticNode sem)
        {
            myTreeNode t;
            if (treeView.SelectedNode == null)
                t = null;
            else
                if (treeView.SelectedNode.Tag == null)
                    t = null;
                else
                    t = treeView.SelectedNode as myTreeNode;


            myTreeNode tn = new myTreeNode();

            tn.Text = text + "   :   " + str;
            tn.Tag = sem;
     
            if (t != null)
                t.Nodes.Add(tn);
            else
                nodes.Add(tn);
        }


        public void prepare_string_node_with_tag2(string str, string text, ISemanticNode sem)
        {           
            myTreeNode tn = new myTreeNode();

           myTreeNode t = treeView.Nodes[0] as myTreeNode;

            tn.Text = text + "   :   " + str;
            tn.Tag = sem;
         
            if (t != null)
                t.Nodes.Add(tn);
            else
                nodes.Add(tn);

            treeView.Nodes[0].Text = "compiled_types  : " + "Count " + compiled_types.Count;
        }


       
        public void prepare_up_link_node(string str, string text, ISemanticNode sem_node)
        {
            myTreeNode t;
            if (treeView.SelectedNode == null)
                t = null;
            else
                if (treeView.SelectedNode.Tag == null)
                    t = null;
                else
                    t = treeView.SelectedNode as myTreeNode;


            myTreeNode tn = new myTreeNode();
            tn.Tag = sem_node;
            tn.BackColor = System.Drawing.Color.Yellow;

            tn.Text = text + "   :   " + str;
            //tn.Tag = str;

            if (t != null)
                t.Nodes.Add(tn);
            else
                nodes.Add(tn);

            try
            {
                if (!table_up_rows.ContainsKey(tn) && tn != null)
                    table_up_rows.Add(tn, sem_node);
            }
            catch (System.Exception e)
            {
                MessageBox.Show("Exception was \n" + e.ToString());
            }
        }



        public void prepare_node_in_collection_up_link(ISemanticNode subnode, string node_name, myTreeNode tn_parent)
        {
            myTreeNode t;
            if (treeView.SelectedNode == null)
                t = null;
            else
                if (treeView.SelectedNode.Tag == null)
                    t = null;
                else
                    t = treeView.SelectedNode as myTreeNode;


            if (subnode != null)
            {
                myTreeNode tn = new myTreeNode();

                tn.Tag = subnode;
                tn.BackColor = System.Drawing.Color.Yellow;
                tn.Text = node_name + "   :   " + subnode.GetType().Name;
                tn.is_leaf = true;
                tn_parent.Nodes.Add(tn);

                try
                {
                    if (!table_up_rows.ContainsKey(tn) && tn != null)
                        table_up_rows.Add(tn, subnode);
                }
                catch (System.Exception e)
                {
                    MessageBox.Show("Exception was \n" + e.ToString());
                }
            }
        }



        

        // -------------------------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------------


        public void visit(ISemanticNode value)
        {          
        }

        public void visit(IDefinitionNode value)
        {
        }

        public void visit(ITypeNode value)
        {
            string s = value.GetType().ToString() + ".";
            prepare_string_node(value.name, s + "name");
            prepare_node(value.base_type, s + "base_type");
            prepare_node(value.element_type, s + "element_type");           
            //value.generic_container
            prepare_base_node_collection_up_links(value.ImplementingInterfaces, s + "ImplementingInterfaces", "ImplementingInterfaces", value.ImplementingInterfaces.Count);
            prepare_string_node(value.is_class.ToString(), s + "is_class");
            prepare_string_node(value.is_generic_parameter.ToString(), s + "is_generic_parameter");
            prepare_string_node(value.is_generic_type_definition.ToString(), s + "is_generic_type_definition");
            prepare_string_node(value.is_value_type.ToString(), s + "is_value_type");
            prepare_string_node(value.IsInterface.ToString(), s + "IsInterface");
            prepare_string_node(value.node_kind.ToString(), s + "node_kind");
            prepare_string_node(value.type_special_kind.ToString(), s + "type_special_kind");

        }

        public void visit(IBasicTypeNode value)
        {
            string s = value.GetType().ToString() + ".";
            prepare_string_node(value.name, s + "name");
            prepare_node(value.base_type, s + "base_type");
            prepare_node(value.element_type, s + "element_type");                 
            //value.generic_container
            prepare_base_node_collection_up_links(value.ImplementingInterfaces, s + "ImplementingInterfaces", "ImplementingInterfaces", value.ImplementingInterfaces.Count);
            prepare_string_node(value.is_class.ToString(), s + "is_class");
            prepare_string_node(value.is_generic_parameter.ToString(), s + "is_generic_parameter");
            prepare_string_node(value.is_generic_type_definition.ToString(), s + "is_generic_type_definition");
            prepare_string_node(value.is_value_type.ToString(), s + "is_value_type");
            prepare_string_node(value.IsInterface.ToString(), s + "IsInterface");
            prepare_string_node(value.node_kind.ToString(), s + "node_kind");
            prepare_string_node(value.type_special_kind.ToString(), s + "type_special_kind");
        }

        public void visit(ICommonTypeNode value)
        {
            string s = value.GetType().ToString() + ".";
            prepare_string_node(value.name, s + "name");           
            prepare_up_link_node(value.comprehensive_namespace.namespace_name.ToString(), s + "comprehensive_namespace", value.comprehensive_namespace);
            //table_for_up_links.Add(treeView.SelectedNode.Nodes[1] as myTreeNode, treeView.SelectedNode.Parent.Parent as myTreeNode);
            
            myTreeNode from = treeView.SelectedNode.Nodes[treeView.SelectedNode.Nodes.Count - 1] as myTreeNode;
            myTreeNode to = table_subnodes[value.comprehensive_namespace] as myTreeNode;
            table_for_up_links.Add(from, to);

            prepare_node(value.base_type, s + "base_type");
            prepare_base_node_collection(value.constants, s + "constants", "constants", value.constants.Length);
            prepare_node(value.default_property, s + "default_property");
            prepare_node(value.element_type, s + "element_type");
            prepare_base_node_collection(value.fields, s + "fields", "fields", value.fields.Length);
            //prepare_node(value.generic_container, s + "generic_container");
            //value.generic_params;
            prepare_string_node(value.has_static_constructor.ToString(), s + "has_static_constructor");
            prepare_base_node_collection_up_links(value.ImplementingInterfaces, s + "ImplementingInterfaces", "ImplementingInterfaces", value.ImplementingInterfaces.Count);            
            prepare_string_node(value.is_class.ToString(), s + "is_class");
            prepare_string_node(value.is_generic_parameter.ToString(), s + "is_generic_parameter");
            prepare_string_node(value.is_generic_type_definition.ToString(), s + "is_generic_type_definition");
            prepare_string_node(value.is_value_type.ToString(), s + "is_value_type");
            prepare_string_node(value.IsInterface.ToString(), s + "IsInterface");
            prepare_string_node(value.IsSealed.ToString(), s + "IsSealed");
            prepare_node(value.lower_value, s + "lower_value");
            prepare_base_node_collection(value.methods, s + "methods", "methods", value.methods.Length);
            prepare_string_node(value.node_kind.ToString(), s + "node_kind");
            prepare_base_node_collection(value.properties, s + "properties", "properties", value.properties.Length);
            //prepare_node(value.runtime_initialization_marker, s + "runtime_initialization_marker");
            prepare_string_node(value.type_access_level.ToString(), s + "type_access_level");
            prepare_string_node(value.type_special_kind.ToString(), s + "type_special_kind");
            prepare_node(value.upper_value, s + "upper_value");
        }
        

        public void visit(ICompiledTypeNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.name, s + "name");
            prepare_node(value.base_type, s + "base_type");
            prepare_string_node(value.compiled_type.ToString(), s + "compiled_type"); // ??????? Как это надо отображать?            
            prepare_node(value.element_type, s + "element_type");
            //prepare_node(value.generic_container, s + "generic_container");
            prepare_base_node_collection_up_links(value.ImplementingInterfaces, s + "ImplementingInterfaces", "ImplementingInterfaces", value.ImplementingInterfaces.Count);
            prepare_string_node(value.is_class.ToString(), s + "is_class");
            prepare_string_node(value.is_generic_parameter.ToString(), s + "is_generic_parameter");
            prepare_string_node(value.is_generic_type_definition.ToString(), s + "is_generic_type_definition");
            prepare_string_node(value.is_value_type.ToString(), s + "is_value_type");
            prepare_string_node(value.IsInterface.ToString(), s + "IsInterface");
            prepare_string_node(value.node_kind.ToString(), s + "node_kind");
            prepare_string_node(value.type_special_kind.ToString(), s + "type_special_kind");
        }

        public void visit(IStatementNode value)
        {
        }

        public void visit(IExpressionNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_node(value.type, s + "");                       
        }

        public void visit(IFunctionCallNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_node(value.function, s + "function");// или ссылка?
            prepare_string_node(value.last_result_function_call.ToString(), s + "last-result_function_call");
            prepare_collection(value.real_parameters, s + "real_parametrs", "real_parametrs", value.real_parameters.Length);
            prepare_node(value.type, s + "type");                        
        }

        public void visit(IBasicFunctionCallNode value)
        {
            string s = value.GetType().Name + ".";
            //prepare_node(value.basic_function, "IBasicFunctionCallNode.basic_function");    // или ссылка?             
            //prepare_node(value.function, s + "function");     // или ссылка?         

            prepare_string_node(value.last_result_function_call.ToString(), s + "last_result_function_call");
            prepare_base_node_collection(value.real_parameters, s + "real_parametrs", "real_parametrs", value.real_parameters.Length);
            prepare_node(value.type, s + "type");            
        }

        public void visit(ICommonNamespaceFunctionCallNode value)
        {
            string s = value.GetType().Name + ".";

            //prepare_up_link_node(value.function.name.ToString(), s + "function", value.function);            

            //myTreeNode from = treeView.SelectedNode.Nodes[treeView.SelectedNode.Nodes.Count - 1] as myTreeNode;
            //myTreeNode to = table_subnodes[value.function] as myTreeNode;
            //table_for_up_links.Add(from, to);

            //prepare_up_link_node(value.namespace_function.name.ToString(), s + "namespace_function", value.namespace_function);           

            //from = treeView.SelectedNode.Nodes[treeView.SelectedNode.Nodes.Count - 1] as myTreeNode;
            //to = table_subnodes[value.namespace_function] as myTreeNode;
            //table_for_up_links.Add(from, to);

            prepare_string_node(value.last_result_function_call.ToString(), s + "last_result_function_call");
            prepare_base_node_collection(value.real_parameters, s + "real_parametrs", "real_parametrs", value.real_parameters.Length);
            prepare_node(value.type, s + "type");            
        }

        public void visit(ICommonNestedInFunctionFunctionCallNode value)
        {
            string s = value.GetType().Name + ".";
            //prepare_up_link_node(value.common_function.ToString(), s + "common_function", value.common_function);
            //prepare_up_link_node(value.function.ToString(), s + "function", value.function);
            prepare_string_node(value.last_result_function_call.ToString(), s + "last_result_function_call");
            prepare_base_node_collection(value.real_parameters, s + "real_parametrs", "real_parametrs", value.real_parameters.Length);
            prepare_string_node(value.static_depth.ToString(), s + "static_depth");
            prepare_node(value.type, s + "type");            
        }

        public void visit(ICommonMethodCallNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_up_link_node(value.function.ToString(), s + "function", value.function);
            prepare_string_node(value.last_result_function_call.ToString(), s + "last_result_function_call");
            prepare_up_link_node(value.method.ToString(), s + "method", value.method);
            prepare_node(value.obj, s + "obj");
            prepare_collection(value.real_parameters, s + "real_parametrs", "real_parametrs", value.real_parameters.Length);
            prepare_node(value.type, s + "type");
            prepare_string_node(value.virtual_call.ToString(), s + "virtual_call");
        }

        public void visit(ICommonStaticMethodCallNode value)
        {
            string s = value.GetType().Name + ".";
            //value.common_type
            //prepare_up_link_node(value.function.ToString(), s + "function", value.function);
            prepare_string_node(value.last_result_function_call.ToString(), s + "last_result_function_call");
            prepare_collection(value.real_parameters, s + "real_parametrs", "real_parametrs", value.real_parameters.Length);
            //value.static_method
            prepare_node(value.type, s + "type");            
        }

        public void visit(ICompiledMethodCallNode value)
        {
            string s = value.GetType().Name + ".";
            //prepare_node(value.compiled_method, "ICompiledMethodCallNode.compiled_method");
            //prepare_node(value.function, "ICompiledMethodCallNode.function");
            prepare_string_node(value.last_result_function_call.ToString(), s + "last_result_function_call");
            //prepare_node(value.obj, "ICompiledMethodCallNode.obj");
            prepare_collection(value.real_parameters, s + "real_parametrs", "real_parametrs", value.real_parameters.Length);
            prepare_node(value.type, s + "type");
            prepare_string_node(value.virtual_call.ToString(), s + "virtual_call");
        }

        public void visit(ICompiledStaticMethodCallNode value)
        {
            string s = value.GetType().Name + ".";
            //prepare_node(value.compiled_type, s + "compiled_type");
            //prepare_node(value.function, s + "function");
            prepare_string_node(value.last_result_function_call.ToString(), s + "last_result_function_call");
            prepare_collection(value.real_parameters, s + "real_parametrs", "real_parametrs", value.real_parameters.Length);
            //prepare_node(value.static_method, s + "static_method");
            //prepare_collection(value.template_parametres, s + "template_parametrs", "template_parametrs", value.template_parametres.Length);                                  
            prepare_node(value.type, s + "type");            
        }

        public void visit(IFunctionNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.name, s + "name");
            prepare_string_node(value.node_kind.ToString(), s + "node_kind");
            prepare_string_node(value.node_location_kind.ToString(), s + "node_location_kind");
            //prepare_collection(value.parameters, s + "parameters", "parameters", value.parameters.Length);
            //value.return_value_type 
            prepare_node(value.return_value_type, s + "return_value_type"); 
        }

        public void visit(IClassMemberNode value)
        {
            string s = value.GetType().ToString() + ".";
            //value.comperehensive_type
            prepare_string_node(value.field_access_level.ToString(), s + "field_access_level");
            prepare_string_node(value.polymorphic_state.ToString(), s + "polymorphic_state");
        }

        public void visit(ICompiledClassMemberNode value)
        {
            string s = value.GetType().ToString() + ".";
            //value.comperehensive_type
            //value.comprehensive_type
            prepare_string_node(value.field_access_level.ToString(), s + "field_access_level");
            prepare_string_node(value.polymorphic_state.ToString(), s + "polymorphic_state");
        }

        public void visit(ICommonClassMemberNode value)
        {
            string s = value.GetType().ToString() + ".";
            


            prepare_node(value.comperehensive_type, s + "comperehensive_type");
            prepare_string_node(value.field_access_level.ToString(), s + "field_access_level");
            prepare_string_node(value.polymorphic_state.ToString(), s + "polymorphic_state");
        }

        public void visit(IFunctionMemberNode value)
        {
            string s = value.GetType().ToString() + ".";
            //prepare_up_link_node(value.function.name.ToString(), s + "function", value.function);            
        }

        public void visit(INamespaceMemberNode value)
        {
            string s = value.GetType().ToString() + ".";
            //value.comprehensive_namespace            
        }

        public void visit(IBasicFunctionNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.name, s + "name");
            //prepare_string_node(value.basic_function_type.ToString(), s + "basic_function_type");
            prepare_string_node(value.node_kind.ToString(), s + "node_kind");
            prepare_string_node(value.node_location_kind.ToString(), s + "node_location_kind");
            prepare_collection(value.parameters, s + "parameters", "parameters", value.parameters.Length);
            prepare_string_node(value.return_value_type.ToString(), s + "return_value_type" + value.return_value_type.name);
            prepare_node(value.return_value_type, s + "return_value_type");
        }

        public void visit(ICommonFunctionNode value)
        {
            string s = value.GetType().ToString() + ".";
            prepare_string_node(value.name, s + "name");
            prepare_collection(value.constants, s + "constants", "constants", value.constants.Length);
            prepare_node(value.function_code, s + "function_code");
            prepare_collection(value.functions_nodes, s + "function_nodes", "function_nodes", value.functions_nodes.Length);
            prepare_string_node(value.node_kind.ToString(), s + "node_kind");
            prepare_string_node(value.node_location_kind.ToString(), s + "node_location_kind");
            prepare_collection(value.parameters, s + "parameters", "parameters", value.parameters.Length);
            prepare_string_node(value.return_value_type.ToString(), s + "return_value_type");
            //prepare_node(value.return_variable, s + "return_variable");  
            prepare_up_link_node(value.return_variable.name, s + "return_variable", value.return_variable);
            prepare_string_node(value.SpecialFunctionKind.ToString(), s + "SpecialFunctionKind");
            prepare_collection(value.var_definition_nodes, s + "var_definition_nodes", "var_definition_nodes", value.var_definition_nodes.Length);
        }

        public void visit(ICommonNamespaceFunctionNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.name, s + "name");
            //if (value.comprehensive_namespace != null)
            //{
            //    prepare_up_link_node(value.comprehensive_namespace.namespace_name.ToString(), s + "comprehensive_namespace", value.comprehensive_namespace);
                
            //    myTreeNode from = treeView.SelectedNode.Nodes[treeView.SelectedNode.Nodes.Count - 1] as myTreeNode;
            //    myTreeNode to = table_subnodes[value.comprehensive_namespace] as myTreeNode;
            //    table_for_up_links.Add(from, to);                
            //}


            prepare_base_node_collection(value.constants, s + "constants", "constants", value.constants.Length);
            prepare_node(value.function_code, s + "function_code");
            prepare_base_node_collection(value.functions_nodes, s + "function_nodes", "function_nodes", value.functions_nodes.Length);
            ////prepare_node(value.namespace_node, s + "namespace_node");
            ////prepare_up_link_node(value.namespace_node.namespace_name, s + "namespace_node", value.namespace_node);
            prepare_string_node(value.node_kind.ToString(), s + "node_kind");
            prepare_string_node(value.node_location_kind.ToString(), s + "node_location_kind");
            prepare_base_node_collection(value.parameters, s + "parametrs", "parametrs", value.parameters.Length);
            prepare_node(value.return_value_type, s + "return_value_type");
            prepare_node(value.return_variable, s + "return_variable");
            prepare_string_node(value.SpecialFunctionKind.ToString(), s + "SpecialFunctionKind");
            prepare_base_node_collection(value.var_definition_nodes, s + "var_definition_nodes", "var_definition_nodes", value.var_definition_nodes.Length);
        }

        public void visit(ICommonNestedInFunctionFunctionNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.name, s + "name");
            prepare_collection(value.constants, s + "constants", "constants", value.constants.Length);
            //value.function
            prepare_node(value.function_code, s + "function_code");
            prepare_collection(value.functions_nodes, s + "function_nodes", "function_nodes", value.functions_nodes.Length);
            prepare_string_node(value.node_kind.ToString(), s + "node_kind");
            prepare_string_node(value.node_location_kind.ToString(), s + "node_location_kind");
            prepare_collection(value.parameters, s + "parametrs", "parametrs", value.parameters.Length);
            prepare_node(value.return_value_type, s + "return_value_type");
            //prepare_node(value.return_variable, s + "return_variable");
            prepare_string_node(value.SpecialFunctionKind.ToString(), s + "SpecialFunctionKind");
            prepare_collection(value.var_definition_nodes, s + "var_definition_nodes", "var_definition_nodes", value.var_definition_nodes.Length);
        }

        public void visit(ICommonMethodNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.name, s + "name");           

            prepare_up_link_node(value.comperehensive_type.name.ToString(), s + "comperehensive_type", value.comperehensive_type);        

            myTreeNode from = treeView.SelectedNode.Nodes[1] as myTreeNode;
            myTreeNode to = table_subnodes[value.comperehensive_type] as myTreeNode;
            table_for_up_links.Add(from, to);



            prepare_base_node_collection(value.constants, s + "constants", "constants", value.constants.Length);
            prepare_string_node(value.field_access_level.ToString(), s + "field_access_level");
            prepare_node(value.function_code, s + "function_code");
            prepare_base_node_collection(value.functions_nodes, s + "function_nodes", "function_nodes", value.functions_nodes.Length);
            prepare_string_node(value.is_constructor.ToString(), s + "is_constructor");
            prepare_string_node(value.is_final.ToString(), s + "is_final");
            prepare_string_node(value.newslot_awaited.ToString(), s + "newslot_awaited");
            prepare_string_node(value.node_kind.ToString(), s + "node_kind");
            prepare_string_node(value.node_location_kind.ToString(), s + "node_location_kind");
            //prepare_node(value.overrided_method, s + "overrided_method");
            prepare_base_node_collection(value.parameters, s + "parametrs", "parametrs", value.parameters.Length);
            prepare_string_node(value.polymorphic_state.ToString(), s + "polymorphic_state");
            prepare_node(value.return_value_type, s + "return_value_type");
            //prepare_node(value.return_variable, s + "return_variable");
            //prepare_up_link_node(value.return_variable.name, s + "return_variable", value.return_variable);
            prepare_string_node(value.SpecialFunctionKind.ToString(), s + "SpecialFunctionKind");
            prepare_base_node_collection(value.var_definition_nodes, s + "var_definition_nodes", "var_definition_nodes", value.var_definition_nodes.Length);
        }

        public void visit(ICompiledMethodNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.name, s + "name");
            //prepare_up_link_node(value.comperehensive_type.ToString(), s + "comperehensive_type", value.comperehensive_type);
            //prepare_up_link_node(value.comprehensive_type.ToString(), s + "comprehensive_type", value.comperehensive_type);
            prepare_string_node(value.field_access_level.ToString(), s + "field_access_level");
            prepare_string_node(value.method_info.ToString(), s + "");
            prepare_string_node(value.node_kind.ToString(), s + "node_kind");
            prepare_string_node(value.node_location_kind.ToString(), s + "node_location_kind");
            prepare_collection(value.parameters, s + "parametrs", "parametrs", value.parameters.Length);
            prepare_string_node(value.polymorphic_state.ToString(), s + "polymorphic_state");
            prepare_node(value.return_value_type, s + "return_value_type");            
        }

        public void visit(IIfNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_node(value.condition, s + "condition ");
            prepare_node(value.then_body, s + "then_body ");
            prepare_node(value.else_body, s + "else_body");
        }

        public void visit(IWhileNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_node(value.condition, s + "condition ");
            prepare_node(value.body, s + "body ");
        }

        public void visit(IRepeatNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_node(value.condition, s + "condition ");
            prepare_node(value.body, s + "body ");
        }

        public void visit(IForNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_node(value.body, s + "body ");
            prepare_node(value.increment_statement, s + "increment_statement ");
            prepare_node(value.initialization_statement, s + "initialization_statemennt ");
            prepare_node(value.while_expr, s + "while_expr ");
        }

        public void visit(IStatementsListNode value)
        {
            string s = value.GetType().Name + ".";
            //prepare_string_node(value.LeftLogicalBracketLocation.ToString(), s + "LeftlogicalBracketLocation");
            prepare_base_node_collection(value.LocalVariables, s + "LocalVariables", "LocalVariables", value.LocalVariables.Length);
            //prepare_string_node(value.RightLogicalBracketLocation.ToString(), s + "RightLogicalBracketLocation");
            prepare_base_node_collection(value.statements, s + "subnodes", "subnodes", value.statements.Length);
        }

        public void visit(INamespaceNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.namespace_name, s + "name");
        }

        public void visit(ICommonNamespaceNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.namespace_name, s + "namespace_name");
            //prepare_node(value.comprehensive_namespace, s + "comprehensive_namespace");                         
            prepare_base_node_collection(value.constants, s + "constants", "constants", value.constants.Length);
            prepare_base_node_collection(value.functions, s + "functions", "functions", value.functions.Length);
            prepare_base_node_collection(value.nested_namespaces, s + "nested_namespaces", "nested_namespaces", value.nested_namespaces.Length);
            prepare_base_node_collection(value.types, s + "types", "types", value.types.Length);
            prepare_base_node_collection(value.variables, s + "variables", "variables", value.variables.Length);
        }

        public void visit(ICompiledNamespaceNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.namespace_name, s + "namespace_name");
        }

        public void visit(IDllNode value)
        {
            string s = value.GetType().Name + ".";
            //value.finalization_function
            //value.initialization_function
            //value.namespaces
        }


        public void visit(IProgramNode value)
        {
            string s = value.GetType().Name + ".";
            ICompiledTypeNode[] ctn = new ICompiledTypeNode[0];
            prepare_base_node_collection(ctn, "compiled types", "->", 0);
            prepare_node(value.main_function, s + "main_function");
            prepare_base_node_collection(value.namespaces, s + value.namespaces[0].GetType().Name + "_list   :   namespaces", "namespaces", value.namespaces.Length);
        }


        public void visit(IProgramNode value, TreeNodeCollection t)
        {
            string s = value.GetType().Name + ".";
            ICompiledTypeNode[] ctn = new ICompiledTypeNode[0];
            prepare_base_node_collection(ctn, "compiled types", "->", 0);
            prepare_node(value.main_function, s + "main_function");
            prepare_base_node_collection(value.namespaces, s + value.namespaces[0].GetType().Name + "_list   :   namespaces", "namespaces", value.namespaces.Length);
        }

        public void visit(IAddressedExpressionNode value)
        {
            string s = value.GetType().Name + ".";
            //value.type                   
        }

        public void visit(ILocalVariableReferenceNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.static_depth.ToString(), s + "static_depth");
            prepare_up_link_node(value.Variable.name.ToString(), s + "Variable", value.Variable);
            prepare_up_link_node(value.variable.name.ToString(), s + "variable", value.variable);
            prepare_node(value.type, s + "type");                             
        }

        public void visit(INamespaceVariableReferenceNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_up_link_node(value.Variable.name.ToString(), s + "Variable", value.Variable);
            prepare_up_link_node(value.variable.name.ToString(), s + "variable", value.variable);
            //value.type
        }

        public void visit(ICommonClassFieldReferenceNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_up_link_node(value.field.name, s + "field", value.field);
            prepare_node(value.obj, s + "obj");
            //value.type           
            prepare_up_link_node(value.Variable.name, s + "Variable", value.Variable);

        }

        public void visit(IStaticCommonClassFieldReferenceNode value)
        {
            string s = value.GetType().ToString() + ".";
            //value.class_type
            //prepare_node(value.static_field, s + "static_field");
            //value.type
            //value.Variable            
        }

        public void visit(ICompiledFieldReferenceNode value)
        {
            string s = value.GetType().ToString() + ".";
            prepare_up_link_node(value.field.name, s + "field", value.field);
            prepare_node(value.obj, s + "obj");
            //value.type     
            prepare_up_link_node(value.Variable.name, s + "Variable", value.Variable);
        }

        public void visit(IStaticCompiledFieldReferenceNode value)
        {
            string s = value.GetType().ToString() + ".";
            //value.class_type
            //prepare_node(value.static_field, s + "static_field");
            //value.type
            //value.Variable 
        }

        public void visit(ICommonParameterReferenceNode value)
        {
            string s = value.GetType().Name + ".";
            //prepare_node(value.parameter, "ICommonParameterReferenceNode.parameter");  
            prepare_string_node(value.static_depth.ToString(), s + "static_depth");
            prepare_node(value.type, s + "type");
            //prepare_node(value.Variable, "ICommonParameterReferenceNode.Variable");            
        }


        public void visit(IConstantNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_node(value.type, s + "type");
            prepare_string_node(value.value.ToString(), s + "value");
        }

        public void visit(IBoolConstantNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.constant_value.ToString(), s + "constant_value");
            prepare_node(value.type, s + "type");
            prepare_string_node(value.value.ToString(), s + "value");
        }

        public void visit(IByteConstantNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.constant_value.ToString(), s + "constant_value");
            prepare_node(value.type, s + "type");
            prepare_string_node(value.value.ToString(), s + "value");
        }

        public void visit(IIntConstantNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.constant_value.ToString(), s + "constant_value");
            prepare_node(value.type, s + "type");
            prepare_string_node(value.value.ToString(), s + "value");
        }

        public void visit(ISByteConstantNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.constant_value.ToString(), s + "constant_value");
            prepare_node(value.type, s + "type");
            prepare_string_node(value.value.ToString(), s + "value");
        }

        public void visit(IShortConstantNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.constant_value.ToString(), s + "constant_value");
            prepare_node(value.type, s + "type");
            prepare_string_node(value.value.ToString(), s + "value");
        }

        public void visit(IUShortConstantNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.constant_value.ToString(), s + "constant_value");
            prepare_node(value.type, s + "type");
            prepare_string_node(value.value.ToString(), s + "value");
        }

        public void visit(IUIntConstantNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.constant_value.ToString(), s + "constant_value");
            prepare_node(value.type, s + "type");
            prepare_string_node(value.value.ToString(), s + "value");
        }

        public void visit(IULongConstantNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.constant_value.ToString(), s + "constant_value");
            prepare_node(value.type, s + "type");
            prepare_string_node(value.value.ToString(), s + "value");
        }

        public void visit(ILongConstantNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.constant_value.ToString(), s + "constant_value");
            prepare_node(value.type, s + "type");
            prepare_string_node(value.value.ToString(), s + "value");
        }

        public void visit(IDoubleConstantNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.constant_value.ToString(), s + "constant_value");
            prepare_node(value.type, s + "type");
            prepare_string_node(value.value.ToString(), s + "value");
        }

        public void visit(IFloatConstantNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.constant_value.ToString(), s + "constant_value");
            prepare_node(value.type, s + "type");
            prepare_string_node(value.value.ToString(), s + "value");
        }

        public void visit(ICharConstantNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.constant_value.ToString(), s + "constant_value");
            prepare_node(value.type, s + "type");
            prepare_string_node(value.value.ToString(), s + "value");
        }

        public void visit(IStringConstantNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.constant_value.ToString(), s + "constant_value");
            prepare_node(value.type, s + "type");
            prepare_string_node(value.value.ToString(), s + "value");
        }


        public void visit(IVAriableDefinitionNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.name, s + "name");
            prepare_node(value.inital_value, s + "initial_value");
            prepare_string_node(value.node_location_kind.ToString(), s + "node_location_kind");
            prepare_node(value.type, s + "type");           
        }

        public void visit(ILocalVariableNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.name, s + "name");
            //prepare_up_link_node(value.function.name.ToString(), s + "function", value.function);
            prepare_node(value.inital_value, s + "initial_value");
            prepare_string_node(value.is_used_as_unlocal.ToString(), s + "is_used_as_unlocal");
            prepare_string_node(value.node_location_kind.ToString(), s + "node_location_kind");
            prepare_node(value.type, s + "type");            
        }

        public void visit(ICommonNamespaceVariableNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.name, s + "name");            

            prepare_up_link_node(value.comprehensive_namespace.namespace_name.ToString(), s + "comprehensive_namespace", value.comprehensive_namespace);

            myTreeNode from = treeView.SelectedNode.Nodes[treeView.SelectedNode.Nodes.Count - 1] as myTreeNode;
            myTreeNode to = table_subnodes[value.comprehensive_namespace] as myTreeNode;
            table_for_up_links.Add(from, to);

            prepare_node(value.inital_value, s + "initial_value");
            prepare_string_node(value.node_location_kind.ToString(), s + "node_location_kind");
            prepare_node(value.type, s + "type");               
        }

        public void visit(ICommonClassFieldNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.name, s + "name");
            //prepare_node(value.comperehensive_type, s + "comperehensive_type");
            //prepare_node(value.comprehensive_type, s + "comprehensive_type");
            prepare_up_link_node(value.comperehensive_type.name.ToString(), s + "comperehensive_type", value.comperehensive_type);

            myTreeNode from = treeView.SelectedNode.Nodes[1] as myTreeNode;
            myTreeNode to = table_subnodes[value.comperehensive_type] as myTreeNode;
            table_for_up_links.Add(from, to);
            
            prepare_string_node(value.field_access_level.ToString(), s + "field_access_level");
            prepare_node(value.inital_value, s + "initial_value");
            prepare_string_node(value.node_location_kind.ToString(), s + "node_location_kind");
            prepare_string_node(value.polymorphic_state.ToString(), s + "polymorphic_state"); 
            prepare_node(value.type, s + "type");
        }

        public void visit(ICompiledClassFieldNode value)
        {
            string s = value.GetType().ToString();
            prepare_string_node(value.name, s + "name");
            //value.comperehensive_type
            prepare_string_node(value.compiled_field.ToString(), s + "compiled_field");
            prepare_node(value.inital_value, s + "initial_value");
            prepare_string_node(value.node_location_kind.ToString(), s + "node_location_kind");
            prepare_string_node(value.polymorphic_state.ToString(), s + "polymorphic_state");
            prepare_node(value.type, s + "type");
        }

        public void visit(IParameterNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.name, s + "name");
            //value.function
            prepare_node(value.inital_value, s + "initial_value");
            prepare_string_node(value.is_params.ToString(), s + "is_params");
            prepare_string_node(value.node_location_kind.ToString(), s + "node_location_kind");
            prepare_string_node(value.parameter_type.ToString(), s + "parameter_type");
            //value.type 
        }

        public void visit(ICommonParameterNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.name, s + "name");            
            
            prepare_up_link_node(value.common_function.name.ToString(), s + "common_finction", value.common_function);

            myTreeNode from = treeView.SelectedNode.Nodes[1] as myTreeNode;
            myTreeNode to = table_subnodes[value.common_function] as myTreeNode;
            table_for_up_links.Add(from, to);

            //value.function == value.common_function            

            prepare_node(value.inital_value, s + "initial_value");
            prepare_string_node(value.is_params.ToString(), s + "is_params");
            prepare_string_node(value.is_used_as_unlocal.ToString(), s + "is_used_as_unlocal");
            prepare_string_node(value.node_location_kind.ToString(), s + "node_location_kind");
            prepare_string_node(value.parameter_type.ToString(), s + "parameter_type");
            prepare_node(value.type, s + "type");            
        }

        public void visit(IBasicParameterNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.name, s + "name");
            //prepare_node(value.function, "IBasicParameterNode.function");
            prepare_node(value.inital_value, s + "initial_value");
            prepare_string_node(value.is_params.ToString(), s + "is_params");
            prepare_string_node(value.node_location_kind.ToString(), s + "node_location_kind");
            prepare_string_node(value.parameter_type.ToString(), s + "parameter_type");
            prepare_node(value.type, s + "type");           
        }

        public void visit(ICompiledParameterNode value)
        {
            string s = value.GetType().ToString() + ".";
            prepare_string_node(value.name, s + "name");
            //prepare_up_link_node(value.compiled_function.name.ToString(), s + "compiled_function_node", value.compiled_function);
            //prepare_up_link_node(value.function.name.ToString(), s + "function_node", value.function);
            prepare_node(value.inital_value, s + "initial_value");
            prepare_string_node(value.is_params.ToString(), s + "is_params");
            prepare_string_node(value.node_location_kind.ToString(), s + "node_location_kind");
            prepare_string_node(value.parameter_type.ToString(), s + "parameter_type");
            prepare_node(value.type, s + "type");           
        }

        public void visit(IConstantDefinitionNode value)
        {
            string s = value.GetType().ToString() + ".";
            prepare_string_node(value.name, s + "name");
            prepare_string_node(value.constant_value.ToString(), s + "constant_value");
            prepare_node(value.type, s + "type");              
        }

        public void visit(IClassConstantDefinitionNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.name, s + "name");            
            
            prepare_up_link_node(value.comperehensive_type.name.ToString(), s + "comperehensive_type", value.comperehensive_type);

            myTreeNode from = treeView.SelectedNode.Nodes[1] as myTreeNode;
            myTreeNode to = table_subnodes[value.comperehensive_type] as myTreeNode;
            table_for_up_links.Add(from, to);


            prepare_node(value.constant_value, s + "constant_value");
            prepare_string_node(value.field_access_level.ToString(), s + "field_access_level");
            prepare_string_node(value.polymorphic_state.ToString(), s + "polymorphic_state");
            prepare_node(value.type, s + "type");                        
        }

        public void visit(ICompiledClassConstantDefinitionNode value)
        {
            string s = value.GetType().ToString() + ".";
            prepare_string_node(value.name, s + "name");
            //value.comperehensive_type
            //value.comprehensive_type
            prepare_node(value.constant_value, s + "constant_value");
            prepare_string_node(value.field_access_level.ToString(), s + "field_access_level");
            prepare_string_node(value.polymorphic_state.ToString(), s + "polymorphic_state");
            prepare_node(value.type, s + "type");
        }

        public void visit(INamespaceConstantDefinitionNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.name, s + "name");
            prepare_up_link_node(value.comprehensive_namespace.namespace_name.ToString(), s + "comprehensive_namespace", value.comprehensive_namespace);

            myTreeNode from = treeView.SelectedNode.Nodes[treeView.SelectedNode.Nodes.Count - 1] as myTreeNode;
            myTreeNode to = table_subnodes[value.comprehensive_namespace] as myTreeNode;
            table_for_up_links.Add(from, to);

            prepare_node(value.constant_value, s + "constant_value");            
            prepare_node(value.type, s + "type");
        }

        public void visit(ICommonFunctionConstantDefinitionNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.name, s + "name");
            //prepare_node(value.comprehensive_function, "ICommonFunctionConstantDefinitionNode.comprehensive_function");
            prepare_node(value.constant_value, s + "constant_value"); ;
            prepare_node(value.type, s + "type");       
        }

        public void visit(IPropertyNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.name, s + "name");
            //value.comprehensive_type
            //value.get_function
            prepare_string_node(value.node_kind.ToString(), s + "node_kind");
            prepare_collection(value.parameters, s + "parameters", "parameters", value.parameters.Length);
            //value.property_type
            //value.set_function
        }

        public void visit(ICommonPropertyNode value)
        {
            string s = value.GetType().ToString() + ".";
            prepare_string_node(value.name, s + "name");
            //value.common_comprehensive_type
            //value.comperehensive_type
            //value.comprehensive_type

            prepare_up_link_node(value.comperehensive_type.name.ToString(), s + "comperehensive_type", value.comperehensive_type);

            myTreeNode from = treeView.SelectedNode.Nodes[1] as myTreeNode;
            myTreeNode to = table_subnodes[value.comperehensive_type] as myTreeNode;
            table_for_up_links.Add(from, to);
            
            prepare_string_node(value.field_access_level.ToString(), s + "field_access_level");
            prepare_node(value.get_function, s + "get_function");
            prepare_string_node(value.node_kind.ToString(), s + "node_kind");
            prepare_collection(value.parameters, s + "parameters", "parameters", value.parameters.Length);
            prepare_string_node(value.polymorphic_state.ToString(), s + "polymorphic_state");
            prepare_node(value.property_type, s + "property_type");
            prepare_node(value.set_function, s + "set_function");
        }

        public void visit(IBasicPropertyNode value)
        {
            string s = value.GetType().ToString() + ".";
            prepare_string_node(value.name, s + "name");
            //value.comprehensive_type
            //value.get_function
            prepare_string_node(value.node_kind.ToString(), s + "node_kind");
            prepare_collection(value.parameters, s + "parameters", "parameters", value.parameters.Length);
            //value.property_type
            //value.set_function            
        }

        public void visit(ICompiledPropertyNode value)
        {
            string s = value.GetType().ToString() + ".";
            prepare_string_node(value.name, s + "name");
            //value.comperehensive_type
            //value.compiled_comprehensive_type
            //value.compiled_get_method
            //value.compiled_set_method
            //value.comprehensive_type
            prepare_string_node(value.field_access_level.ToString(), s + "field_access_level");
            //value.get_function
            prepare_string_node(value.node_kind.ToString(), s + "node_kind");
            prepare_collection(value.parameters, s + "parameters", "parameters", value.parameters.Length);
            prepare_string_node(value.polymorphic_state.ToString(), s + "polymorphic_state");
            prepare_string_node(value.property_info.ToString(), s + "property_info");
            //value.property_type
            //value.set_function            
        }

        public void visit(IThisNode value)
        {
            string s = value.GetType().ToString() + ".";
            prepare_node(value.type, s + "type");           
        }

        public void visit(IReturnNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_node(value.return_value, s + "return_value");            
        }

        public void visit(ICommonConstructorCall value)
        {
            string s = value.GetType().Name + ".";
            //value.common_type
            //value.function
            prepare_string_node(value.last_result_function_call.ToString(), s + "last_result_function_call");
            prepare_base_node_collection(value.real_parameters, s + "real_parameters", "real_parameters", value.real_parameters.Length);
            //value.static_method
            prepare_node(value.type, s + "type");        
        }

        public void visit(ICompiledConstructorCall value)
        {
            string s = value.GetType().Name + ".";
            //value.compiled_type;
            //prepare_node(value.constructor, s + "constructor");
            //value.function;
            prepare_string_node(value.last_result_function_call.ToString(), s + "last_result_function_call");
            prepare_collection(value.real_parameters, s + "real_parametrs", "real_parametrs", value.real_parameters.Length);
            prepare_node(value.type, s + "type");          
        }

        public void visit(IWhileBreakNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_node(value.while_node, s + "while_node");            
        }

        public void visit(IRepeatBreakNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_node(value.repeat_node, s + "repeat_node");    
        }

        public void visit(IForBreakNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_node(value.for_node, s + "for_node");    
        }

        public void visit(IWhileContinueNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_node(value.while_node, s + "while_node");    
        }

        public void visit(IRepeatContinueNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_node(value.repeat_node, s + "repeat_node");    
        }

        public void visit(IForContinueNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_node(value.for_node, s + "for_node");    
        }

        public void visit(ICompiledConstructorNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.name, s + "name");
            //value.comperehensive_type
            //value.comprehensive_type
            //value.constructor_info
            prepare_string_node(value.field_access_level.ToString(), s + "field_access_level");
            prepare_string_node(value.node_kind.ToString(), s + "node_kind");
            prepare_string_node(value.node_location_kind.ToString(), s + "node_location_kind");
            prepare_collection(value.parameters, s + "parameters", "parameters", value.parameters.Length);
            prepare_string_node(value.polymorphic_state.ToString(), s + "polymorphic_state");
            prepare_node(value.return_value_type, s + "return_value_type");
        }

        public void visit(ISimpleArrayNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.name, s + "name");
            prepare_node(value.base_type, s + "base_type");
            prepare_node(value.element_type, s + "element_type");
            //value.generic_container
            prepare_collection_up_links(value.ImplementingInterfaces, s + "ImplementingInterfaces", "ImplementingInterfaces", value.ImplementingInterfaces.Count);
            prepare_string_node(value.is_class.ToString(), s + "is_class");
            prepare_string_node(value.is_generic_parameter.ToString(), s + "");
            prepare_string_node(value.is_generic_type_definition.ToString(), s + "");
            prepare_string_node(value.is_value_type.ToString(), s + "");
            prepare_string_node(value.IsInterface.ToString(), s + "");
            prepare_string_node(value.length.ToString(), s + "");
            prepare_string_node(value.node_kind.ToString(), s + "node_kind");
            prepare_string_node(value.type_special_kind.ToString(), s + "type_special_kind");
        }

        public void visit(ISimpleArrayIndexingNode value)
        {
            string s = value.GetType().Name + ".";
            //prepare_node(value.array, s + "array");
            //prepare_node(value.index, s + "index");
            //prepare_node(value.type, s + "type");
        }

        public void visit(IExternalStatementNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.name, s + "name");
            prepare_string_node(value.module_name.ToString(), s + "module_name");
        }

        public void visit(IRefTypeNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.name, s + "name");
            prepare_node(value.base_type, s + "base_type");
            prepare_node(value.element_type, s + "element_type");
            //value.generic_container
            prepare_collection_up_links(value.ImplementingInterfaces, s + "ImplementingInterfaces", "ImplementingInterfaces", value.ImplementingInterfaces.Count);
            prepare_string_node(value.is_class.ToString(), s + "is_class");
            prepare_string_node(value.is_generic_parameter.ToString(), s + "");
            prepare_string_node(value.is_generic_type_definition.ToString(), s + "");
            prepare_string_node(value.is_value_type.ToString(), s + "");
            prepare_string_node(value.IsInterface.ToString(), s + "");
            prepare_string_node(value.node_kind.ToString(), s + "node_kind");
            //value.pointed_type
            prepare_string_node(value.type_special_kind.ToString(), s + "type_special_kind");
        }

        public void visit(IGetAddrNode value)
        {
            string s = value.GetType().Name + ".";
            //prepare_node(value.addr_of_expr, s + "addr_of_expr");
            prepare_node(value.type, s + "type");
        }

        public void visit(IDereferenceNode value)
        {
            string s = value.GetType().Name + ".";
            //prepare_node(value.derefered_expr, s + "derefered_expr");
            prepare_node(value.type, s + "type");
        }

        public void visit(IThrowNode value)
        {
            string s = value.GetType().Name + ".";
            //prepare_node(value.exception_expresion, s + "exception_expression");            
        }

        public void visit(ISwitchNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_node(value.case_expression, s + "case_expression");
            prepare_base_node_collection(value.case_variants, s + "case_variants", "case_variants", value.case_variants.Length);
            prepare_node(value.default_statement, s + "default_statement");            
        }

        public void visit(ICaseVariantNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_base_node_collection(value.elements, s + "elements", "elements", value.elements.Length);
            prepare_base_node_collection(value.ranges, s + "ranges", "ranges", value.ranges.Length);
            prepare_node(value.statement_to_execute, s + "statement_to_execute");            
        }

        public void visit(ICaseRangeNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_node(value.high_bound, s + "hign_bound");
            prepare_node(value.lower_bound, s + "lower_bound"); 
        }

        public void visit(INullConstantNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_node(value.type, s + "type");
            //value.value    
        }

        public void visit(IUnsizedArray value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.name, s + "name");
            prepare_node(value.base_type, s + "base_type");
            prepare_node(value.element_type, s + "element_type");
            //value.generic_container
            prepare_base_node_collection_up_links(value.ImplementingInterfaces, s + "ImplementingInterfaces", "ImplementingInterfaces", value.ImplementingInterfaces.Count);
            prepare_string_node(value.is_class.ToString(), s + "is_class");
            prepare_string_node(value.is_generic_parameter.ToString(), s + "");
            prepare_string_node(value.is_generic_type_definition.ToString(), s + "");
            prepare_string_node(value.is_value_type.ToString(), s + "");
            prepare_string_node(value.IsInterface.ToString(), s + "");
            prepare_string_node(value.node_kind.ToString(), s + "node_kind");
            prepare_string_node(value.type_special_kind.ToString(), s + "type_special_kind");
        }

        public void visit(IRuntimeManagedMethodBody value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.runtime_statement_type.ToString(), s + "runtime_statement_type");
        }

        public void visit(IAsNode value)
        {
            string s = value.GetType().Name + ".";
            //prepare_node(value.left, s + "left");
            //prepare_node(value.right, s + "right");
            prepare_node(value.type, s + "type");           
        }

        public void visit(IIsNode value)
        {
            string s = value.GetType().Name + ".";
            //prepare_node(value.left, s + "left");
            //prepare_node(value.right, s + "right");
            prepare_node(value.type, s + "type");
        }

        public void visit(ISizeOfOperator value)
        {
            string s = value.GetType().Name + ".";
            //value.oftype
            prepare_node(value.type, s + "type");       
        }

        public void visit(ITypeOfOperator value)
        {
            string s = value.GetType().Name + ".";
            //prepare_node(value.oftype, s + "oftype");
            prepare_node(value.type, s + "type");
        }

        public void visit(IExitProcedure value)
        {
        }

        public void visit(ITryBlockNode value)
        {
            string s = value.GetType().Name + ".";
            //value.ExceptionFilters
            //prepare_node(value.FinallyStatements, s + "FinallyStatements");
            //prepare_node(value.TryStatements, s + "tryStatements");            
        }

        public void visit(IExceptionFilterBlockNode value)
        {
            string s = value.GetType().Name + ".";
            //value.ExceptionHandler
            //value.ExceptionInstance
            //value.ExceptionType            
        }

        public void visit(IArrayConstantNode value)
        {
            string s = value.GetType().Name + ".";
            //value.ElementType
            //value.ElementValues
            prepare_node(value.type, s + "type");
            //value.value
        }

        public void visit(IStatementsExpressionNode value)
        {
            string s = value.GetType().Name + ".";
            //prepare_node(value.expresion, s + "expresion");
            //prepare_collection(value.statements, s + "statements", "statements", value.statements.Length);
            prepare_node(value.type, s + "type");
        }

        public void visit(IQuestionColonExpressionNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_node(value.condition, s + "condition");
            //value.ret_if_false
            //value.ret_if_true
            prepare_node(value.type, s + "type");
        }

        public void visit(IRecordConstantNode value)
        {
            string s = value.GetType().Name + ".";
            //value.FieldValues
            prepare_node(value.type, s + "type");
            //value.value
        }

        public void visit(ILabelNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.name, s + "name");
        }

        public void visit(ILabeledStatementNode value)
        {
            string s = value.GetType().Name + ".";
            //prepare_node(value.label, s + "label");
            //prepare_node(value.statement, s + "statement");
        }

        public void visit(IGotoStatementNode value)
        {
            string s = value.GetType().Name + ".";
            //prepare_node(value.label, s + "label");            
        }

        public void visit(ICompiledStaticMethodCallNodeAsConstant value)
        {
            string s = value.GetType().Name + ".";
            //value.MethodCall
            prepare_node(value.type, s + "type");
            //value.value
        }

        public void visit(ICommonNamespaceFunctionCallNodeAsConstant value)
        {
            string s = value.GetType().Name + ".";
            //value.MethodCall
            prepare_node(value.type, s + "type");
            //value.value
        }

        public void visit(IEnumConstNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.constant_value.ToString(), s + "constant_value");
            prepare_node(value.type, s + "type");
            //value.value
        }

        public void visit(IForeachNode value)
        {
            string s = value.GetType().Name + ".";
            //prepare_node(value.Body, "IForeachNode.Body"); 
            //prepare_node(value.InWhatExpr, "IForeachnode.InWhatExpr"); 
            //prepare_node(value.VarIdent, "IForeachNode.VarIdent");              
        }

        public void visit(ILockStatement value)
        {
            string s = value.GetType().Name + ".";
            //prepare_node(value.Body, "ILockStatement.Body"); 
            //prepare_node(value.LockObject, "ILockStatement.LockObject");          
        }

        public void visit(ILocalBlockVariableNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_string_node(value.name, s + "name");
            prepare_up_link_node(value.Block.ToString(), s + "Block", value.Block);           

            myTreeNode from = treeView.SelectedNode.Nodes[treeView.SelectedNode.Nodes.Count - 1] as myTreeNode;
            myTreeNode to = table_subnodes[value.Block] as myTreeNode;
            table_for_up_links.Add(from, to);

            prepare_node(value.inital_value, s + "initial_value");
            prepare_string_node(value.node_location_kind.ToString(), s + "node_location_kind");
            prepare_node(value.type, s + "type");
        }

        public void visit(ILocalBlockVariableReferenceNode value)
        {
            string s = value.GetType().Name + ".";
            //prepare_node(value.type, s + "type");
            prepare_up_link_node(value.Variable.name.ToString(), s + "Variable", value.Variable);
        }
        public void visit(ICompiledConstructorCallAsConstant value)
        {
            string s = value.GetType().Name + ".";
            //value.MethodCall
            prepare_node(value.type, s + "type");
            //value.value
        }
        public void visit(IRethrowStatement value)
        {
        }

        public void visit(IForeachBreakNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_node(value.foreach_node, s + "foreach_node");
        }

        public void visit(IForeachContinueNode value)
        {
            string s = value.GetType().Name + ".";
            prepare_node(value.foreach_node, s + "foreach_node");
        }


        public void visit(INamespaceConstantReference value)
        {
        }

        public void visit(IFunctionConstantReference value)
        {
        }

        public void visit(ICommonConstructorCallAsConstant value)
        {
        }

        public void visit(IArrayInitializer value)
        {
        }
		
        public void visit(ICommonEventNode value)
		{
			
		}
        
		public void visit(IEventNode value)
		{

		}
    	
		public void visit(ICompiledEventNode value)
		{

		}
    	
		public void visit(IStaticEventReference value)
		{

		}
    	
		public void visit(INonStaticEventReference value)
		{

		}
        
		public void visit(IRecordInitializer value)
		{
			
		}

        public virtual void visit(IDefaultOperatorNode value)
        {
			
        }
        
        public virtual void visit(IAttributeNode value)
        {
        	
        }
        
        public virtual void visit(IPInvokeStatementNode value)
        {
        	
        }
        
        public virtual void visit(IBasicFunctionCallNodeAsConstant value)
        {
        	
        }

        public virtual void visit(ICompiledStaticFieldReferenceNodeAsConstant value)
        {
        }

        public virtual void visit(ILambdaFunctionNode value)
        {

        }
        public virtual void visit(ILambdaFunctionCallNode value)
        {

        }
        public virtual void visit(ICommonNamespaceEventNode value)
        {
        }
    }
}
