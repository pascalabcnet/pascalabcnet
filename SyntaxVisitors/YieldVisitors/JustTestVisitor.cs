using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class JustTestVisitor : BaseChangeVisitor
    {
        private void CollectFormalParams(procedure_definition pd, ISet<var_def_statement> collectedFormalParams)
        {
            if ((object)pd.proc_header.parameters != null)
                collectedFormalParams.UnionWith(pd.proc_header.parameters.params_list.Select(tp => new var_def_statement(tp.idents, tp.vars_type)));
        }

        private void CollectFormalParamsNames(procedure_definition pd, ISet<string> collectedFormalParamsNames)
        {
            if ((object)pd.proc_header.parameters != null)
                collectedFormalParamsNames.UnionWith(pd.proc_header.parameters.params_list.SelectMany(tp => tp.idents.idents).Select(id => id.name));
        }

        private void CollectClassFieldsNames(procedure_definition pd, ISet<string> collectedFields, out bool isInClassMethod)
        {
            isInClassMethod = false;

            ident className = null;
            if ((object)pd.proc_header.name.class_name != null)
            {
                // Объявление вне класса его метода
                className = pd.proc_header.name.class_name;
            }
            else
            {
                // Объявление функции в классе?
                var classDef = UpperNode(3) as class_definition;
                if ((object)(UpperNode(3) as class_definition) != null)
                {
                    var td = UpperNode(4) as type_declaration;
                    if ((object)td != null)
                    {
                        className = td.type_name;
                    }
                }
            }

            if ((object)className != null)
            {
                isInClassMethod = true;

                CollectClassFieldsVisitor fieldsVis = new CollectClassFieldsVisitor(className);
                var cu = UpperTo<compilation_unit>();
                if ((object)cu != null)
                {
                    cu.visit(fieldsVis);
                    // Collect
                    collectedFields.UnionWith(fieldsVis.CollectedFields.Select(id => id.name));
                }
            }
        }

        private void CollectUnitGlobalsNames(procedure_definition pd, ISet<string> collectedUnitGlobalsName)
        {
            var cu = UpperTo<compilation_unit>();
            if ((object)cu != null)
            {
                var ugVis = new CollectUnitGlobalsVisitor();
                cu.visit(ugVis);
                // Collect
                collectedUnitGlobalsName.UnionWith(ugVis.CollectedGlobals.Select(id => id.name));
            }
        }

        private void CreateCapturedLocalsNamesMap(ISet<string> localsNames, IDictionary<string, string> capturedLocalsNamesMap)
        {
            foreach (var localName in localsNames)
            {
                capturedLocalsNamesMap.Add(localName, CapturedNamesHelper.MakeCapturedLocalName(localName));
            }
        }

        private void CreateCapturedFormalParamsNamesMap(ISet<string> formalParamsNames, IDictionary<string, string> captueedFormalParamsNamesMap)
        {
            foreach (var formalParamName in formalParamsNames)
            {
                captueedFormalParamsNamesMap.Add(formalParamName, CapturedNamesHelper.MakeCapturedFormalParameterName(formalParamName));
            }
        }

        public override void visit(procedure_definition pd)
        {
            // frninja

            // Classification
            ISet<string> CollectedLocalsNames = new HashSet<string>();
            ISet<string> CollectedFormalParamsNames = new HashSet<string>();
            ISet<string> CollectedClassFieldsNames = new HashSet<string>();
            ISet<string> CollectedUnitGlobalsNames = new HashSet<string>();

            ISet<var_def_statement> CollectedLocals = new HashSet<var_def_statement>();
            ISet<var_def_statement> CollectedFormalParams = new HashSet<var_def_statement>();

            // Map from ident idName -> captured ident idName
            IDictionary<string, string> CapturedLocalsNamesMap = new Dictionary<string, string>();
            IDictionary<string, string> CapturedFormalParamsNamesMap = new Dictionary<string, string>();

            var dld = new DeleteAllLocalDefs(); // mids.vars - все захваченные переменные
            pd.visit(dld); // Удалить в локальных и блочных описаниях этой процедуры все переменные и вынести их в отдельный список var_def_statement

            // frninja 08/12/15
            bool isInClassMethod;

            // Collect locals
            CollectedLocals.UnionWith(dld.LocalDeletedDefs);
            CollectedLocalsNames.UnionWith(dld.LocalDeletedDefs.SelectMany(vds => vds.vars.idents).Select(id => id.name));
            // Collect formal params
            CollectFormalParams(pd, CollectedFormalParams);
            CollectFormalParamsNames(pd, CollectedFormalParamsNames);
            // Collect class fields
            CollectClassFieldsNames(pd, CollectedClassFieldsNames, out isInClassMethod);
            
            // Collect unit globals
            CollectUnitGlobalsNames(pd, CollectedUnitGlobalsNames);

            // Create maps :: idName -> captureName
            CreateCapturedLocalsNamesMap(CollectedLocalsNames, CapturedLocalsNamesMap);
            CreateCapturedFormalParamsNamesMap(CollectedFormalParamsNames, CapturedFormalParamsNamesMap);

            // AHAHA test!
            ReplaceCapturedVariablesVisitor rcapVis = new ReplaceCapturedVariablesVisitor(
                CollectedLocalsNames,
                CollectedFormalParamsNames,
                CollectedClassFieldsNames,
                new HashSet<string>(),
                new HashSet<string>(),
                CollectedUnitGlobalsNames,
                CapturedLocalsNamesMap,
                CapturedFormalParamsNamesMap,
                isInClassMethod
                );
            // Replace
            (pd.proc_body as block).program_code.visit(rcapVis);
        }
    }
}
