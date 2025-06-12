using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SyntaxTreeConverters;
using System;
using System.Collections.Generic;

namespace SyntaxVisitors.SugarVisitors
{
    public class PropertyDesugarVisitor : BaseChangeVisitor, IPipelineVisitor
    {
        public static PropertyDesugarVisitor New
        {
            get { return new PropertyDesugarVisitor(); }
        }

        public void Visit(syntax_tree_node root, VisitorsContext context, Action next)
        {
            ProcessNode(root);

            next();
        }

        public override void visit(class_members _class_members)
        {
            foreach (var member in _class_members.members)
            {
                if (member is simple_property)
                {
                    Desugar(member as simple_property);
                }
            }
        }

        private void Desugar(simple_property _simple_property)
        {
            var property_parent_node = _simple_property.Parent;
            while (property_parent_node != null && property_parent_node.GetType() != typeof(class_members))
            {
                property_parent_node = property_parent_node.Parent;
            }
            class_members property_class_members = property_parent_node as class_members;

            while (property_parent_node != null && property_parent_node.GetType() != typeof(class_body_list))
            {
                property_parent_node = property_parent_node.Parent;
            }
            class_body_list class_body = property_parent_node as class_body_list;

            while (property_parent_node != null && property_parent_node.GetType() != typeof(class_definition))
            {
                property_parent_node = property_parent_node.Parent;
            }
            class_definition class_def = property_parent_node as class_definition;

            var property_type = _simple_property.property_type;
            var read_accessor = _simple_property.accessors?.read_accessor;
            var write_accessor = _simple_property.accessors?.write_accessor;
            var new_class_members = new class_members(access_modifer.private_modifer);

            if (class_def.keyword == class_keyword.Interface && _simple_property.is_auto)
            {
                throw new SyntaxVisitorError(
                    "INVALID_INTERFACE_MEMBER", _simple_property.source_context);
            }

            if (class_def.keyword == class_keyword.Interface &&
                read_accessor != null &&
                (read_accessor.pr != null || read_accessor.accessor_name != null))
            {
                throw new SyntaxVisitorError(
                    "INVALID_INTERFACE_MEMBER", read_accessor.source_context);
            }

            if (class_def.keyword == class_keyword.Interface &&
                write_accessor != null &&
                (write_accessor.pr != null || write_accessor.accessor_name != null))
            {
                throw new SyntaxVisitorError(
                    "INVALID_INTERFACE_MEMBER", write_accessor.source_context);
            }

            if (_simple_property.is_auto)
            {
                if (_simple_property.parameter_list != null)
                {
                    throw new SyntaxVisitorError(
                    "INDEXED_AUTO_PROPERTY", _simple_property.source_context);
                }
                AutoPropertyDesugaring(_simple_property, new_class_members);
            }

            // pr != null => extended read accessor
            if (read_accessor != null && read_accessor.pr != null)
            {
                ExtendedReadAccessorDesugaring(read_accessor, _simple_property, new_class_members);
            }

            // pr != null => extended write accessor
            if (write_accessor != null && write_accessor.pr != null)
            {
                ExtendedWriteAccessorDesugaring(write_accessor, _simple_property, new_class_members);
            }

            if (new_class_members.Count > 0)
            {
                class_body.InsertBefore(property_class_members, new_class_members);
            }
        }

        private void AutoPropertyDesugaring(simple_property simple_property,
                                            class_members members)
        {
            var id = NewId("#PField", simple_property.source_context);
            var accessors = new property_accessors();
            accessors.write_accessor = new write_accessor_name(id, null, null, simple_property.source_context);
            accessors.read_accessor = new read_accessor_name(id, null, null, simple_property.source_context);
            simple_property.accessors = accessors;

            var def_attribute = definition_attribute.None;
            if (simple_property.attr == definition_attribute.Static)
            {
                def_attribute = definition_attribute.Static;
            }

            List<ident> l = new List<ident>() { id };
            var var_def_statement = new var_def_statement(
                new ident_list(l, simple_property.source_context),
                simple_property.property_type,
                simple_property.initial_value,
                def_attribute,
                false,
                simple_property.source_context);

            members.Add(var_def_statement);
        }

        private void ExtendedReadAccessorDesugaring(read_accessor_name read_accessor,
                                                    simple_property simple_property,
                                                    class_members members)
        {
            var read_accessor_procedure = read_accessor.pr as procedure_definition;
            var func_header = (read_accessor_procedure.proc_header as function_header);
            func_header.return_type = simple_property.property_type;

            //extended indexed property
            if (simple_property.parameter_list != null)
            {
                var func_header_params = new List<ident>();
                var func_header_types = new List<type_definition>();
                foreach (var param in simple_property.parameter_list.parameters)
                {
                    foreach (var ident in param.names.idents)
                    {
                        func_header_params.Add(ident);
                        func_header_types.Add(param.type);
                    }
                }
                func_header.parameters =
                     SyntaxTreeBuilder.BuildFormalParameters(
                         func_header_params,
                         func_header_types);
            }

            members.Add(read_accessor_procedure);

            if (simple_property.attr == definition_attribute.Static)
            {
                read_accessor_procedure.proc_header.class_keyword = true;
                var proc_attr = new procedure_attribute(proc_attribute.attr_static);
                proc_attr.source_context = read_accessor_procedure.proc_header.source_context;
                read_accessor_procedure.proc_header.proc_attributes = new procedure_attributes_list(proc_attr);
            }
        }

        private void ExtendedWriteAccessorDesugaring(write_accessor_name write_accessor,
                                                     simple_property simple_property,
                                                     class_members members)
        {
            var write_accessor_procedure = write_accessor.pr as procedure_definition;

            //extended indexed property
            if (simple_property.parameter_list != null)
            {
                var proc_header_params = new List<ident>();
                var proc_header_types = new List<type_definition>();

                foreach (var param in simple_property.parameter_list.parameters)
                {
                    foreach (var ident in param.names.idents)
                    {
                        proc_header_params.Add(ident);
                        proc_header_types.Add(param.type);
                    }
                }
                proc_header_params.Add(new ident("value"));
                proc_header_types.Add(simple_property.property_type);

                write_accessor_procedure.proc_header.parameters =
                     SyntaxTreeBuilder.BuildFormalParameters(
                         proc_header_params,
                         proc_header_types);
            }
            else
            {
                write_accessor_procedure.proc_header.parameters.params_list[0].vars_type = simple_property.property_type;
            }

            members.Add(write_accessor_procedure);

            if (simple_property.attr == definition_attribute.Static)
            {
                write_accessor_procedure.proc_header.class_keyword = true;
                var proc_attr = new procedure_attribute(proc_attribute.attr_static);
                proc_attr.source_context = write_accessor_procedure.proc_header.source_context;
                write_accessor_procedure.proc_header.proc_attributes = new procedure_attributes_list(proc_attr);
            }
        }

        //random id generator
        private int postfix = 0;
        public ident NewId(string prefix, SourceContext sc = null)
        {
            postfix++;
            return new ident(prefix + postfix.ToString(), sc);
        }

    }
}
