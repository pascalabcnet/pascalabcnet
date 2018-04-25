using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler;
using PascalABCCompiler.Errors;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors.TypeclassVisitors
{
    public class ReplaceTypeclassVisitor: BaseChangeVisitor
    {
        FindInstancesAndRestrictedFunctionsVisitor instancesAndRestrictedFunctions;

        public ReplaceTypeclassVisitor(FindInstancesAndRestrictedFunctionsVisitor instancesAndRestrictedFunctions)
        {
            this.instancesAndRestrictedFunctions = instancesAndRestrictedFunctions;
        }

        public static ReplaceTypeclassVisitor New(FindInstancesAndRestrictedFunctionsVisitor instancesAndRestrictedFunctions)
        {
            return new ReplaceTypeclassVisitor(instancesAndRestrictedFunctions);
        }

        bool VisitInstanceDeclaration(type_declaration instanceDeclaration)
        {
            var instanceDefinition = instanceDeclaration.type_def as instance_definition;
            if (instanceDefinition == null)
            {
                return false;
            }
            var instanceName = instanceDeclaration.type_name as typeclass_restriction;

            var parents = new named_type_reference_list(new template_type_reference(
                instanceName.name, instanceName.restriction_args));
            var instanceDefTranslated =
                SyntaxTreeBuilder.BuildClassDefinition(
                    parents,
                    null, instanceDefinition.body.class_def_blocks.ToArray());


            for (int i = 0; i < instanceDefTranslated.body.class_def_blocks.Count; i++)
            {
                var cm = instanceDefTranslated.body.class_def_blocks[i].members;

                for (int j = 0; j < cm.Count; j++)
                {
                    // TODO: or override if implementation exists
                    (cm[j] as procedure_header)?.proc_attributes.Add(new procedure_attribute("override", proc_attribute.attr_override));
                    (cm[j] as procedure_definition)?.proc_header.proc_attributes.Add(new procedure_attribute("override", proc_attribute.attr_override));
                }
            }

            {
                // Add constructor
                var cm = instanceDefTranslated.body.class_def_blocks[0];
                var def = new procedure_definition(
                    new constructor(),
                    new statement_list(new empty_statement()));
                def.proc_body.Parent = def;
                def.proc_header.proc_attributes = new procedure_attributes_list();

                cm.Add(def);
            }

            var typeName = CreateInstanceName(instanceName.restriction_args as typeclass_param_list, instanceName.name);

            var typeclassNameTanslated = new ident(typeName);

            var instanceDeclTranslated = new type_declaration(typeclassNameTanslated, instanceDefTranslated, instanceDeclaration.source_context);
            Replace(instanceDeclaration, instanceDeclTranslated);
            visit(instanceDeclTranslated);

            return true;
        }

        private static string CreateInstanceName(typeclass_param_list restriction_args, string typeName)
        {
            for (int i = 0; i < restriction_args.Count; i++)
            {
                typeName += "_" + (restriction_args.params_list[i] as named_type_reference).names[0];
            }

            return typeName;
        }

        bool VisitTypeclassDeclaration(type_declaration typeclassDeclaration)
        {
            var typeclassDefinition = typeclassDeclaration.type_def as typeclass_definition;
            if (typeclassDefinition == null)
            {
                return false;
            }

            var typeclassName = typeclassDeclaration.type_name as typeclass_restriction;

            // TODO: typeclassDefinition.additional_restrictions - translate to usual classes

            var typeclassDefTranslated =
                SyntaxTreeBuilder.BuildClassDefinition(
                    typeclassDefinition.additional_restrictions,
                    null, typeclassDefinition.body.class_def_blocks.ToArray());

            typeclassDefTranslated.attribute = class_attribute.Abstract;
            for (int i = 0; i < typeclassDefTranslated.body.class_def_blocks.Count; i++)
            {
                var cm = typeclassDefTranslated.body.class_def_blocks[i].members;

                for (int j = 0; j < cm.Count; j++)
                {
                    // TODO: or override if implementation exists
                    (cm[j] as procedure_header)?.proc_attributes.Add(new procedure_attribute("abstract", proc_attribute.attr_abstract));
                }
            }

            {
                // Add constructor
                var cm = typeclassDefTranslated.body.class_def_blocks[0];
                var def = new procedure_definition(
                    new constructor(),
                    new statement_list(new empty_statement()));
                def.proc_body.Parent = def;
                def.proc_header.proc_attributes = new procedure_attributes_list();

                cm.Add(def);
            }

            ident_list templates = RestrictionsToIdentList(typeclassName.restriction_args);

            var typeclassNameTanslated = new template_type_name(typeclassName.name, templates, typeclassName.source_context);

            var typeclassDeclTranslated = new type_declaration(typeclassNameTanslated, typeclassDefTranslated, typeclassDeclaration.source_context);
            Replace(typeclassDeclaration, typeclassDeclTranslated);
            visit(typeclassDeclTranslated);

            return true;
        }

        private static ident_list RestrictionsToIdentList(template_param_list restrictions)
        {
            var templates = new ident_list();
            templates.source_context = restrictions.source_context;
            for (int i = 0; i < restrictions.Count; i++)
            {
                templates.Add((restrictions.params_list[i] as named_type_reference).names[0]);
            }

            return templates;
        }

        public override void visit(type_declaration _type_declaration)
        {
            if (VisitInstanceDeclaration(_type_declaration))
            {
                return;
            }

            if (VisitTypeclassDeclaration(_type_declaration))
            {
                return;
            }

            DefaultVisit(_type_declaration);
        }


        public override void visit(procedure_definition _procedure_definition)
        {
            bool isConstrainted = _procedure_definition.proc_header.where_defs != null &&
                _procedure_definition.proc_header.where_defs.defs.Any(x => x is where_typeclass_constraint);
            if (!isConstrainted)
            {
                DefaultVisit(_procedure_definition);
                return;
            }
            
            var header = _procedure_definition.proc_header;
            var headerTranslated = header.Clone() as procedure_header;
            headerTranslated.where_defs = new where_definition_list();

            var additionalTemplateArgs = new ident_list();

            for (int i = 0; i < header.where_defs.defs.Count; i++)
            {
                var where = header.where_defs.defs[i];

                if (where is where_typeclass_constraint)
                {
                    var typeclassWhere = where as where_typeclass_constraint;
                    var newName = TypeclassRestrctionToTemplateName(typeclassWhere.restriction);

                    additionalTemplateArgs.Add(newName);

                    // Create name for template that replaces typeclass(for ex. SumTC)
                    headerTranslated.where_defs.defs.Add(
                        // where
                        new where_definition(
                            //      ConstraintTC :
                            new ident_list(newName),
                            new where_type_specificator_list(new List<type_definition> {
                                //                  Constraint<T, C>,
                                new template_type_reference(new named_type_reference(typeclassWhere.restriction.name), typeclassWhere.restriction.restriction_args),
                                //                                        constructor
                                new declaration_specificator(DeclarationSpecificator.WhereDefConstructor, "constructor")
                        })));
                }
                else
                {
                    headerTranslated.where_defs.defs.Add(where);
                }
            }

            // Add new templates devoted to constraints to template list
            headerTranslated.template_args.idents.AddRange(additionalTemplateArgs.idents);

            var blockProc = (_procedure_definition.proc_body as block);
            foreach (var arg in additionalTemplateArgs.idents)
            {
                blockProc.program_code.AddFirst(new var_statement(new var_def_statement(
                    arg.name + "Instance",
                    new dot_node(
                        new ident_with_templateparams(new ident("__ConceptSingleton"), new template_param_list(new List<type_definition> { new named_type_reference((ident)arg.Clone()) })),
                        new ident("Instance")
                ))));
            }

            //var list = _procedure_definition.proc_body.DescendantNodes().OfType<typeclass_param_list>();
            foreach (var tcr in _procedure_definition.proc_body.DescendantNodes().OfType<ident_with_templateparams>())
            {
                if (tcr.template_params is typeclass_param_list)
                {
                    // TODO: Check tcr.name - typeclass from where
                    // TODO: Check - that there is such typeclass with such args at where
                    // TODO: Ensure that we don't replace another constraint funciton call
                    var str = tcr.template_params.params_list
                        .Select(x => (x as named_type_reference).names[0].name)
                        .Aggregate((tcr.name as ident).name, (x, y) => x + y);
                    var id = new ident(str + "Instance");
                    var parent = tcr.Parent;
                    parent.ReplaceDescendant((addressed_value)tcr, (addressed_value)id, Desc.All);
                }
                //var id = new ident(TypeclassRestrctionToTemplateName(tcr) + "Instance");
                
            }

            var procedureDefTranslated = new procedure_definition(
                headerTranslated, _procedure_definition.proc_body,
                _procedure_definition.is_short_definition, _procedure_definition.source_context);

            Replace(_procedure_definition, procedureDefTranslated);
        }

        public override void visit(method_call methodCall)
        {
            var methodName = methodCall.dereferencing_value as ident_with_templateparams;
            if (methodName == null)
            {
                DefaultVisit(methodCall);
                return;
            }
            var typeclassRestrictions = methodName.template_params as typeclass_param_list;
            if (typeclassRestrictions == null)
            {
                DefaultVisit(methodCall);
                return;
            }

            var paramList = new List<type_definition>();
            paramList.AddRange(typeclassRestrictions.params_list);

            // TODO: Add template types for typeclass instances
            var methodStrName = (methodName.name as ident).name;
            var typeclasses = instancesAndRestrictedFunctions.restrictedFunctions[methodStrName];
            
            var possibleOverloadingsForEachTypeclass = typeclasses.Select(x => 
                new KeyValuePair<string, List<typeclass_param_list>>(x, instancesAndRestrictedFunctions.instances[x]));

            // Find current overloading in possible
            var newParams = new List<type_definition>();
            foreach (var typeclassOverloadings in possibleOverloadingsForEachTypeclass)
            {
                foreach (var possibleParamList in typeclassOverloadings.Value)
                {
                    bool isEqual = true;
                    for (int i = 0; i < possibleParamList.params_list.Count; i++)
                    {
                        if ((possibleParamList.params_list[i] as named_type_reference).names[0].name !=
                            (paramList[i] as named_type_reference).names[0].name)
                        {
                            isEqual = false;
                            break;
                        }
                    }
                    
                    // TODO: need many checks
                    // Typeclass params and function restriction params are mixed up
                    // Fix it as soon as it possible
                    // Btw for 1 argument it's ok.
                    if (isEqual)
                    {
                        newParams.Add(new named_type_reference(CreateInstanceName(possibleParamList, typeclassOverloadings.Key)));
                    }

                }
            }

            paramList.AddRange(newParams);

            var methodCallTranslated = new method_call(
                new ident_with_templateparams(methodName.name, new template_param_list(paramList), methodName.source_context),
                methodCall.parameters,
                methodCall.source_context);
            Replace(methodCall, methodCallTranslated);
        }

        private static ident TypeclassRestrctionToTemplateName(typeclass_restriction typeclassWhere)
        {

            // Concatenate type constraint into new type name
            // For example:
            //      Constaint[T, C] => ConstraintTC
            return RestrictionsToIdentList(typeclassWhere.restriction_args).idents.Aggregate(
                new ident(typeclassWhere.name), (x, y) => x.name + y.name);
        }
    }



}
