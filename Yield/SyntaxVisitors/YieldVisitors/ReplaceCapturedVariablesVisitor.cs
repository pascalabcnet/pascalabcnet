using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

using PascalABCCompiler.YieldHelpers;

namespace SyntaxVisitors
{
    public class ReplaceCapturedVariablesVisitor : BaseChangeVisitor
    {
        private ISet<string> CollectedLocals = new HashSet<string>();
        private ISet<string> CollectedFormalParams = new HashSet<string>();
        private ISet<string> CollectedClassFields = new HashSet<string>();
        private ISet<string> CollectedUnitGlobals = new HashSet<string>();

        // Maps :: idName -> capturedName
        private IDictionary<string, string> CapturedLocalsMap = new Dictionary<string, string>();
        private IDictionary<string, string> CapturedFormalParamsMap = new Dictionary<string, string>();

        private bool IsInClassMethod = true;

        private ident ClassName;


        public ReplaceCapturedVariablesVisitor(IEnumerable<string> locals,
            IEnumerable<string> formalParams,
            IEnumerable<string> classFields,
            IEnumerable<string> classMethods,
            IEnumerable<string> classProperties,
            IEnumerable<string> unitGlobals,
            IDictionary<string, string> localsMap,
            IDictionary<string, string> formalParamsMap,
            bool isInClassMethod, ident className)
        {
            CollectedLocals = new HashSet<string>(locals);
            CollectedFormalParams = new HashSet<string>(formalParams);
            CollectedClassFields = new HashSet<string>(classFields);
            CollectedUnitGlobals = new HashSet<string>(unitGlobals);

            CapturedLocalsMap = new Dictionary<string, string>(localsMap);
            CapturedFormalParamsMap = new Dictionary<string, string>(formalParamsMap);

            IsInClassMethod = isInClassMethod;
            ClassName = className;

            // Methods hack
            CollectedClassFields.UnionWith(classMethods);
            // Properties hack
            CollectedClassFields.UnionWith(classProperties);

        }

        public override void visit(labeled_statement ls)
        {
            ProcessNode(ls.to_statement);
        }

        public override void visit(goto_statement gt)
        {
            // Empty
        }

        // frninja 12/05/16 - фикс для yield_unknown_ident
        public override void visit(yield_unknown_ident unk)
        {
            yield_var_def_statement_with_unknown_type x;
            // Empty
        }

        public override void visit(ident id)
        {
            // Check dot node
            var upper = UpperNode(1);
            //if (upper is dot_node)
            //    return;

            var idName = id.name;
            var idSourceContext = id.source_context;

            // frninja 31/03/16 - фикс селфа для extensionmethod
            if (idName.ToLower() == "self" && !CollectedFormalParams.Contains(idName))
            {
                var newSelf = new dot_node(new ident("self"), new ident(YieldConsts.Self));
                Replace(id, newSelf);
                return;
            }

            // Detect where is id from
            if (CollectedLocals.Contains(idName))
            {
                Replace(id, new ident(CapturedLocalsMap[idName], idSourceContext));
            }
            else if (CollectedFormalParams.Contains(idName))
            {
                Replace(id, new ident(CapturedFormalParamsMap[idName], idSourceContext));
            }
            else if (IsInClassMethod)
            {
                // In class -> check fields
                if (CollectedClassFields.Contains(idName))
                {
                    // Good 
                    // Name in class fields -> capture as class field


                    var capturedId = new dot_node(new dot_node(new ident("self"), new ident(YieldConsts.Self)), id);
                    Replace(id, capturedId);
                }
                else
                {
                    // Bad
                    // At syntax we don't know if the className is class field or not coz of e.g. base .NET classes
                    // HERE WE SHOULD REPLACE TO yield_unknown_reference -> so decision is passed to semantic 
                    // Check for globals will be processed at semantic, too

                    if (!id.name.StartsWith("<")) // Check for already captured
                    {
                        Replace(id, new yield_unknown_ident(id, ClassName));
                    }
                }
            }
            else
            {
                // Not in class -> check globals
                if (CollectedUnitGlobals.Contains(idName))
                {
                    // Global -> just do nothing
                }
                else
                {
                    // What about static classes - search at semantic
                    // HERE WE SHOULD REPLACE TO yield_unknown_reference -> so decision is passed to semantic 
                }
            }
        }


        /*public override void visit(procedure_call pc)
        {
            ProcessNode(pc.func_name);
            var methCall = pc.func_name as method_call;
            if ((object)methCall != null)
            {
                foreach (var param in methCall.parameters.expressions)
                {
                    param.visit(this);
                }
            }
        }*/

        public override void visit(dot_node dn)
        {
            var rid = dn.right as ident;
            if ((object)rid != null && rid.name != YieldConsts.Self)
                ProcessNode(dn.left);

            // Most nested
            // DotNode (DLeft, DRight) -> DotNode(DotNode("self", "<>__self"), DRight)


            // LEFT self -> captured self (self.captured_self)
            /*var id = dn.left as ident;
            if ((object)id != null && (id.className == "self" || CollectedClassFields.Contains(id.className)))
            {
                // Some magic for blocking back-traverse from BaseChangeVisitor redoin' work
                //var rid = dn.right as ident;
                //if ((object)rid != null && rid.className != Consts.Self)
                {
                    var newDotNode = new dot_node(new dot_node(new ident("self"), new ident(Consts.Self)), dn.right);
                    // Change right?
                    //var capturedRight = new dot_node(new ident(Consts.Self), dn.right);
                    //var newDotNode = new dot_node(dn.left, capturedRight);
                    Replace(dn, newDotNode);
                }
                

                // Some magic for blocking back-traverse from BaseChangeVisitor redoin' work
                //var rid = dn.right as ident;
                //if ((object)rid != null && rid.className != Consts.Self)
               // {
                //    var capturedSelf = new dot_node(new ident("self"), new ident(Consts.Self));
                //    Replace(dn.left, capturedSelf);
               // }
            }
            else
            {
                ProcessNode(dn.left);
            }*/

            //if (dn.right.GetType() != typeof(ident))
            //    ProcessNode(dn.right);
        }
    }
}
