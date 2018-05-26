using PascalABCCompiler.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SyntaxVisitors.SugarVisitors
{
    public class AutoClassDesugaringVisitor : BaseChangeVisitor
    {
        private Dictionary<string, class_definition> autoClassDefinitions = new Dictionary<string, class_definition>();

        public static AutoClassDesugaringVisitor New => new AutoClassDesugaringVisitor();

        public override void visit(type_declaration typeDeclaration)
        {
            var isAutoClass = typeDeclaration.type_def is class_definition classDefinition && IsAutoClass(classDefinition);
            if (!isAutoClass)
                return;

            // Добавляем автокласс в словарь
            classDefinition = typeDeclaration.type_def as class_definition;
            autoClassDefinitions[typeDeclaration.type_name.name] = classDefinition;

            CheckAutoClassInheritance(classDefinition);
            CheckAutoClassBody(classDefinition);

            var fieldNames = new List<ident>();
            var fieldTypes = new List<type_definition>();
            CollectAutoClassRegularFields(classDefinition, ref fieldNames, ref fieldTypes);
            var constructor = SyntaxTreeBuilder.BuildSimpleConstructorSection(fieldNames, fieldNames.Select(x => new ident('_' + x.name)).ToList(), fieldTypes);
            classDefinition.body.Add(constructor);
        }

        private bool IsAutoClass(class_definition classDefinition) => (classDefinition.attribute & class_attribute.Auto) == class_attribute.Auto;

        private bool AutoClassExists(string className) => className != null && autoClassDefinitions.ContainsKey(className);

        private void CheckAutoClassInheritance(class_definition classDefinition)
        {
            if (classDefinition?.class_parents?.Count > 1)
                throw new SyntaxVisitorError("AUTO_CLASS_CAN_ONLY_HAVE_ONE_PARENT_CLASS", classDefinition.source_context);

            if (classDefinition?.class_parents?.Count > 0 && !AutoClassExists(GetAutoClassParentName(classDefinition)))
                throw new SyntaxVisitorError("AUTO_CLASS_CAN_BE_INHERITED_ONLY_FROM_ANOTHER_AUTO_CLASS", classDefinition.source_context);
        }

        private void CheckAutoClassBody(class_definition classDefinition)
        {
            // Запрещаем все кроме полей
            if (classDefinition.body.class_def_blocks.Any(block => block.members.Any(member => !(member is var_def_statement))))
                throw new SyntaxVisitorError("AUTO_CLASS_CAN_ONLY_CONTAIN_FIELD_DECLARATIONS", classDefinition.source_context);
        }

        private void CollectAutoClassRegularFields(class_definition classDefinition, ref List<ident> names, ref List<type_definition> types)
        {
            if (classDefinition?.class_parents?.Count > 0)
            {
                var hasParent = GetAutoClassParent(classDefinition, out var definition, out _);
                CollectAutoClassRegularFields(classDefinition, ref names, ref types);
            }

            var varDefs = CollectAutoClassVarDefs(classDefinition);
            names.AddRange(varDefs.Select(x => x.vars).SelectMany(x => x.list));
            types.AddRange(varDefs.SelectMany(x => Enumerable.Repeat(x.vars_type, x.vars.Count)));
        }

        private IEnumerable<var_def_statement> CollectAutoClassVarDefs(class_definition classDefinition) =>
            classDefinition.body.class_def_blocks.SelectMany(block => block.members.OfType<var_def_statement>());

        private string GetAutoClassParentName(class_definition classDefinition)
        {
            GetAutoClassParent(classDefinition, out _, out string parentName);
            return parentName;
        }

        /// <summary>
        /// Получает предка класса, только если он тоже автокласс
        /// </summary>
        /// <param name="classDefinition"></param>
        /// <param name="definition"></param>
        /// <param name="parentName"></param>
        /// <returns></returns>
        private bool GetAutoClassParent(class_definition classDefinition, out class_definition definition, out string parentName)
        {
            parentName = null;
            definition = null;

            if (classDefinition?.class_parents?.Count > 0)
            {
                parentName = classDefinition.class_parents.types
                    .Where(x => autoClassDefinitions.ContainsKey(x.names[0].name))
                    .Select(x => x.names[0].name)
                    .FirstOrDefault();

                if (parentName != null)
                {
                    definition = autoClassDefinitions[parentName];
                    return true;
                }
            }

            return false;
        }
    }
}
