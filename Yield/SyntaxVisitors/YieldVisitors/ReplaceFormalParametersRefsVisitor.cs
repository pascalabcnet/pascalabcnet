using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class ReplaceFormalParametersRefsVisitor : BaseChangeVisitor
    {
        public Dictionary<string, type_definition> CollectedFormalParameters = new Dictionary<string, type_definition>();

        // Типа-стек соответствий paramName -> <>num__paramName для разных уровней вложенности методов
        private List<Dictionary<string, string>> formalParametersStack = new List<Dictionary<string, string>>();

        private bool _isClassMethod = false;


        public ReplaceFormalParametersRefsVisitor()
        {
        }

        // Лучше запретить yield в вложенных функциях и в функциях со вложенными!
        // Запретить-запретить-запретить

        public override void visit(procedure_definition pd)
        {
           // var u = UpperNode(3);
            // В случае отсутствия формальных параметров
            if ((object)pd.proc_header.parameters == null)
            {
                base.visit(pd.proc_body);
                return;
            }

            formalParametersStack.Add(new Dictionary<string, string>());
            int currentLevel = formalParametersStack.Count - 1;

            foreach (var plist in pd.proc_header.parameters.params_list)
            {
                foreach (var id in plist.idents.idents)
                {
                    var paramName = id.name;
                    var hoistedParamName = CapturedNamesHelper.MakeCapturedFormalParameterName(id.name);

                    formalParametersStack[currentLevel].Add(paramName, hoistedParamName);

                    // Захват
                    CollectedFormalParameters.Add(hoistedParamName, plist.vars_type);
                }
            }

            base.visit(pd.proc_body);

            formalParametersStack.RemoveAt(currentLevel);
        }

        


        public override void visit(ident id)
        {
            int? paramNameLevel = null;

            var paramName = id.name;

            // Ищем с какого уровня имя
            for (int level = formalParametersStack.Count - 1; level >= 0; --level)
            {
                if (formalParametersStack[level].ContainsKey(paramName))
                {
                    // Нашли!
                    paramNameLevel = level;
                    break;
                }
            }

            bool isField = false;

            // Локальные параметры обрабатываются в другом визиторе

            // Параметр функции
            if ((object)paramNameLevel != null)
            {
                var upper = UpperNode();
                // Подозреваем обращение к параметру метода
                if ((object)upper == null || (object)upper != null && (upper as dot_node) == null)
                {
                    // Это не self.paramName - поле класса и не что-то другое?
                    // Нашли обращение к параметру?

                    var self = new ident("self", id.source_context);

                    // Заменяем paramName -> self.hoistedParamName: <>num__paramName

                    var hoistedParamName = new ident(formalParametersStack[(int)paramNameLevel][paramName], id.source_context);

                    var selfId = new dot_node(self, hoistedParamName);

                    Replace(id, selfId);
                }
                // Иначе проверить что это поле класса! self.paramName или какой-то другой очень извращенный вариант вроде (someMethod: self).paramName
            }

            // Поле класса

            // Параметр внешней функции (если наша - вложенная)

            // Глобальная переменная
        }

    }
}
