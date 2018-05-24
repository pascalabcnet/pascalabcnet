using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
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

            // If it is instance of derived typelass than it should have template parameters
            var templateArgs = new ident_list();
            where_definition_list whereSection = null;
            var typeclassParents = (instancesAndRestrictedFunctions.typeclasses[instanceName.name].type_def as typeclass_definition).additional_restrictions;
            if (typeclassParents != null && typeclassParents.Count > 0)
            {
                whereSection = new where_definition_list();

                for (int i = 0; i < typeclassParents.Count; i++)
                {
                    ident template_name;
                    if (typeclassParents[i] is typeclass_reference tr)
                    {
                        string name = tr.names[0].name;
                        template_name = TypeclassRestrctionToTemplateName(name, tr.restriction_args);

                        whereSection.Add(GetWhereRestriction(
                            TypeclassReferenceToInterfaceName(name, instanceName.restriction_args),
                            template_name));
                    }
                    else
                    {
                        throw new NotImplementedException("Should be syntactic error");
                    }
                    templateArgs.Add(template_name);
                }
            }

            List<type_definition> templateLists = instanceName.restriction_args.params_list.Concat(templateArgs.idents.Select(x => new named_type_reference(x.name)).OfType<type_definition>()).ToList();
            var parents = new named_type_reference_list(new List<named_type_reference> {
                new template_type_reference(instanceName.name, new template_param_list(templateLists)),
                new template_type_reference("I" + instanceName.name, instanceName.restriction_args)});
            var instanceDefTranslated =
                SyntaxTreeBuilder.BuildClassDefinition(
                    parents,
                    null, instanceDefinition.body.class_def_blocks.ToArray());
            instanceDefTranslated.template_args = templateArgs;
            instanceDefTranslated.where_section = whereSection;

            for (int i = 0; i < instanceDefTranslated.body.class_def_blocks.Count; i++)
            {
                var cm = instanceDefTranslated.body.class_def_blocks[i].members;

                for (int j = 0; j < cm.Count; j++)
                {
                    procedure_header header = null;
                    if (cm[j] is procedure_header ph)
                    {
                        cm[j] = ph;
                    }
                    else if (cm[j] is procedure_definition pd)
                    {
                        header = pd.proc_header;
                    }
                    header.proc_attributes.Add(new procedure_attribute("override", proc_attribute.attr_override));
                    ConvertOperatorNameIdent(header);
                }
            }
            /*
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
            */
            var typeName = new ident(CreateInstanceName(instanceName.restriction_args as typeclass_param_list, instanceName.name));

            var instanceDeclTranslated = new type_declaration(typeName, instanceDefTranslated, instanceDeclaration.source_context);
            instanceDeclTranslated.attributes = instanceDeclaration.attributes;
            AddAttribute(instanceDeclTranslated, "__TypeclassInstanceAttribute");
            AddAttribute(instanceDeclTranslated, "__TypeclassAttribute");

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

            // Creating interface

            // Get members for typeclass interface
            var interfaceMembers = new List<class_members>();
            foreach (var cm in typeclassDefinition.body.class_def_blocks)
            {
                var cmNew = (class_members)cm.Clone();

                for (int i = 0; i < cmNew.members.Count; i++)
                {
                    var member = cmNew.members[i];
                    if (member is function_header || member is procedure_header)
                    {
                        cmNew.members[i] = member;
                    }
                    else if (member is procedure_definition procDef)
                    {
                        cmNew.members[i] = procDef.proc_header;
                    }
                    AddAttribute(cmNew.members[i], "__TypeclassMemberAttribute");
                    if (cmNew.members[i] is procedure_header ph)
                    {
                        ConvertOperatorNameIdent(ph);
                    }
                }

                interfaceMembers.Add(cmNew);
            }
            var interfaceInheritance = (named_type_reference_list)typeclassDefinition.additional_restrictions?.Clone();
            if (interfaceInheritance != null)
            {
                interfaceInheritance.source_context = null;
                for (int i = 0; i < interfaceInheritance.types.Count; i++)
                {
                    if (interfaceInheritance.types[i] is typeclass_reference tr)
                    {
                        interfaceInheritance.types[i] = TypeclassReferenceToInterfaceName(tr.names[0].name, tr.restriction_args);
                    }
                    else
                    {
                        throw new NotImplementedException("Syntactic Error");
                    }
                }
            }
            var typeclassInterfaceDef =
                SyntaxTreeBuilder.BuildClassDefinition(
                    interfaceInheritance,
                    null, interfaceMembers.ToArray());
            typeclassInterfaceDef.keyword = class_keyword.Interface;
            var typeclassInterfaceName = new template_type_name("I" + typeclassName.name, RestrictionsToIdentList(typeclassName.restriction_args));
            var typeclassInterfaceDecl = new type_declaration(typeclassInterfaceName, typeclassInterfaceDef);
            typeclassInterfaceDecl.attributes = typeclassDeclaration.attributes;
            AddAttribute(typeclassInterfaceDecl, "__TypeclassAttribute");


            // Creating class

            var typeclassDefTranslated =
                SyntaxTreeBuilder.BuildClassDefinition(
                    new named_type_reference_list(new template_type_reference(typeclassInterfaceName.name, typeclassName.restriction_args)),
                    null, typeclassDefinition.body.class_def_blocks.ToArray());

            typeclassDefTranslated.attribute = class_attribute.Abstract;
            for (int i = 0; i < typeclassDefTranslated.body.class_def_blocks.Count; i++)
            {
                var cm = typeclassDefTranslated.body.class_def_blocks[i].members;

                for (int j = 0; j < cm.Count; j++)
                {
                    procedure_header header = null;
                    if (cm[j] is procedure_header ph)
                    {
                        header = ph;
                        header.proc_attributes.Add(new procedure_attribute("abstract", proc_attribute.attr_abstract));
                    }
                    else if (cm[j] is procedure_definition pd)
                    {
                        header = pd.proc_header;
                        header.proc_attributes.Add(new procedure_attribute("virtual", proc_attribute.attr_virtual));
                    }

                    ConvertOperatorNameIdent(header);
                }
            }
            /*
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
            */
            // Add template parameters for typeclass class(derived typeclasses)
            ident_list templates = RestrictionsToIdentList(typeclassName.restriction_args);
            if (typeclassDefinition.additional_restrictions != null)
            {
                for (int i = 0; i < typeclassDefinition.additional_restrictions.types.Count; i++)
                {
                    string name;
                    string templateName;
                    if (typeclassDefinition.additional_restrictions.types[i] is typeclass_reference tr)
                    {
                        name = tr.names[0].name;
                        templateName = TypeclassRestrctionToTemplateName(name, tr.restriction_args).name;
                    }
                    else
                    {
                        throw new NotImplementedException("SyntaxError");
                    }

                    // Add template parameter
                    templates.Add(templateName);

                    // Add where restriction
                    if (typeclassDefTranslated.where_section == null)
                    {
                        typeclassDefTranslated.where_section = new where_definition_list();
                    }
                    typeclassDefTranslated.where_section.Add(GetWhereRestriction(
                        interfaceInheritance.types[i],
                        templateName));

                    // Add methods from derived typeclasses
                    var body = (instancesAndRestrictedFunctions.typeclasses[name].type_def as typeclass_definition).body;
                    foreach (var cdb in body.class_def_blocks)
                    {
                        var cdbNew = new class_members(cdb.access_mod == null ? access_modifer.none : cdb.access_mod.access_level);
                        foreach (var member in cdb.members)
                        {
                            procedure_header memberHeaderNew;

                            if (member is procedure_header || member is function_header)
                            {
                                memberHeaderNew = (procedure_header)member.Clone();
                                memberHeaderNew.source_context = null;
                            }
                            else if (member is procedure_definition procDefinition)
                            {
                                memberHeaderNew = (procedure_header)procDefinition.proc_header.Clone();
                                memberHeaderNew.Parent = null;
                                memberHeaderNew.source_context = null;
                            }
                            else
                            {
                                continue;
                            }

                            var variableName = templateName + "Instance";
                            var parameters = memberHeaderNew.parameters.params_list.Aggregate(new expression_list(), (x, y) => new expression_list(x.expressions.Concat(y.idents.idents).ToList()));

                            expression methodCall = null;
                            if (memberHeaderNew.name.meth_name is operator_name_ident oni)
                            {
                                ConvertOperatorNameIdent(memberHeaderNew);
                                Debug.Assert(parameters.expressions.Count == 2, "Parameters count for operation should be equal to 2");
                                //methodCall = new bin_expr(parameters.expressions[0], parameters.expressions[1], oni.operator_type);
                            }
                            var callName = new dot_node(variableName, memberHeaderNew.name.meth_name.name);
                            methodCall = new method_call(callName, parameters);
                            statement exec = null;
                            if (memberHeaderNew is function_header)
                            {
                                exec = new assign("Result", methodCall);
                            }
                            else if (memberHeaderNew is procedure_header)
                            {
                                exec = new procedure_call(methodCall as method_call);
                            }
                            var procDef = new procedure_definition(
                                memberHeaderNew,
                                new statement_list(
                                        GetInstanceSingletonVarStatement(templateName),
                                        exec));
                            cdbNew.Add(procDef);
                        }
                        typeclassDefTranslated.body.class_def_blocks.Add(cdbNew);
                    }
                }
            }

            var typeclassNameTanslated = new template_type_name(typeclassName.name, templates, typeclassName.source_context);

            var typeclassDeclTranslated = new type_declaration(typeclassNameTanslated, typeclassDefTranslated, typeclassDeclaration.source_context);
            typeclassDeclTranslated.attributes = typeclassDeclaration.attributes;
            AddAttribute(typeclassDeclTranslated, "__TypeclassAttribute");

            Replace(typeclassDeclaration, typeclassDeclTranslated);
            UpperNodeAs<type_declarations>().InsertBefore(typeclassDeclTranslated, typeclassInterfaceDecl);
            visit(typeclassInterfaceDecl);
            visit(typeclassDeclTranslated);

            return true;
        }

        private static void ConvertOperatorNameIdent(procedure_header ph)
        {
            if (ph.name.meth_name is operator_name_ident oni)
            {
                var methName = "$typeclass" + PascalABCCompiler.TreeConverter.name_reflector.get_name(oni.operator_type);
                ph.name.meth_name = methName;
            }
        }

        private static void AddAttribute(declaration typeclassInterfaceDecl, string newAttribute, expression_list expressionList = null)
        {
            if (expressionList == null)
            {
                expressionList = new expression_list();
            }
            if (typeclassInterfaceDecl.attributes == null)
            {
                typeclassInterfaceDecl.attributes = new attribute_list();
            }
            typeclassInterfaceDecl.attributes.Add(new simple_attribute_list(new attribute(null, new named_type_reference(newAttribute), expressionList)));
        }

        private static template_type_reference TypeclassReferenceToInterfaceName(string name, template_param_list restriction_args)
        {
            return new template_type_reference(
                                        new named_type_reference("I" + name), restriction_args);
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
                    var newName = TypeclassRestrctionToTemplateName(typeclassWhere.restriction.name, typeclassWhere.restriction.restriction_args);
                    AddAttribute(
                        newName, "__TypeclassGenericParameter",
                        new expression_list(new string_const(GetInstanceSingletonName(newName.name))));
                    additionalTemplateArgs.Add(newName);

                    // Create name for template that replaces typeclass(for ex. SumTC)
                    headerTranslated.where_defs.defs.Add(
                        GetWhereRestriction(
                            new template_type_reference(new named_type_reference("I" + typeclassWhere.restriction.name), typeclassWhere.restriction.restriction_args),
                            newName));
                }
                else
                {
                    headerTranslated.where_defs.defs.Add(where);
                }
            }

            // Add new templates devoted to constraints to template list
            // TODO: template_args can be empty, if there is no <T> or smth, need to check
            headerTranslated.template_args.idents.AddRange(additionalTemplateArgs.idents);

            var blockProc = (_procedure_definition.proc_body as block);
            foreach (var arg in additionalTemplateArgs.idents)
            {
                blockProc.program_code.AddFirst(GetInstanceSingletonVarStatement(arg.name));
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
            procedureDefTranslated.proc_header.attributes = _procedure_definition.proc_header.attributes;
            AddAttribute(procedureDefTranslated.proc_header, "__TypeclassRestrictedFunctionAttribute");
            Replace(_procedure_definition, procedureDefTranslated);
            visit(procedureDefTranslated);
        }

        private static var_statement GetInstanceSingletonVarStatement(string typeName)
        {
            return new var_statement(new var_def_statement(
                                GetInstanceSingletonName(typeName),
                                new dot_node(
                                    new ident_with_templateparams(new ident("__ConceptSingleton"), new template_param_list(new List<type_definition> { new named_type_reference(typeName) })),
                                    new ident("Instance")
                            )));
        }

        private static string GetInstanceSingletonName(string typeName)
        {
            return typeName + "Instance";
        }

        private static where_definition GetWhereRestriction(type_definition restriction, ident templateName)
        {
            return  // where
                new where_definition(
                    //      ConstraintTC :
                    new ident_list(templateName),
                    new where_type_specificator_list(new List<type_definition> {
                    //                  IConstraint<T, C>,
                    restriction,
                    //                                        constructor
                    new declaration_specificator(DeclarationSpecificator.WhereDefConstructor, "constructor")
                }));
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

            List<type_definition> newParams = GetTranslatedTypeclassParameters(paramList, typeclasses);

            paramList.AddRange(newParams);

            var methodCallTranslated = new method_call(
                new ident_with_templateparams(methodName.name, new template_param_list(paramList), methodName.source_context),
                methodCall.parameters,
                methodCall.source_context);
            Replace(methodCall, methodCallTranslated);
        }

        private List<type_definition> GetTranslatedTypeclassParameters(List<type_definition> paramList, IEnumerable<string> typeclasses)
        {
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
                        var derived_typeclass =
                            (instancesAndRestrictedFunctions.typeclasses[typeclassOverloadings.Key].type_def as typeclass_definition);

                        var base_typeclasses = derived_typeclass.additional_restrictions?.types.OfType<typeclass_reference>().Select(x => x.names[0].name);
                        var typeName = new named_type_reference(CreateInstanceName(possibleParamList, typeclassOverloadings.Key));
                        if (base_typeclasses == null || base_typeclasses.Count() == 0)
                        {
                            newParams.Add(typeName);

                        }
                        else
                        {
                            var translatedParams = GetTranslatedTypeclassParameters(paramList, base_typeclasses);

                            newParams.Add(new template_type_reference(
                                typeName,
                                new template_param_list(translatedParams)));
                        }

                    }

                }
            }

            return newParams;
        }

        private static ident TypeclassRestrctionToTemplateName(string name, template_param_list restriction_args)
        {

            // Concatenate type constraint into new type name
            // For example:
            //      Constaint[T, C] => ConstraintTC
            return RestrictionsToIdentList(restriction_args).idents.Aggregate(
                new ident(name), (x, y) => x.name + y.name);
        }
    }



}
