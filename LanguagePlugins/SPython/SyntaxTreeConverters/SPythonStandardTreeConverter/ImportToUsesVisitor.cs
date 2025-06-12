using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SyntaxTreeConverters;
using System;
using System.Collections.Generic;

namespace Languages.SPython.Frontend.Converters
{
    internal class ImportToUsesVisitor : BaseChangeVisitor, IPipelineVisitor
    {
        Dictionary<string, SourceContext> modulesNames = new Dictionary<string, SourceContext>();

        public ImportToUsesVisitor() { }

        private Dictionary<string, string> specialModulesAliases = Facade.LanguageProvider.Instance.SelectLanguageByName("SPython").LanguageInformation.SpecialModulesAliases;

        public void Visit(syntax_tree_node root, VisitorsContext context, Action next)
        {
            if (!context.Get<bool>("forIntellisense"))
                ProcessNode(root);

            next();
        }

        private string GetNameToImport(string nameToImport)
        {
            if (!specialModulesAliases.ContainsKey(nameToImport))
                return nameToImport;
            else 
                return specialModulesAliases[nameToImport];
        }

        private void AddName(ident id)
        {
            string nameToImport = GetNameToImport(id.name);
            if (!modulesNames.ContainsKey(nameToImport))
                modulesNames.Add(nameToImport, id.source_context);
        }

        public override void visit(import_statement _import_statement)
        {
            foreach (as_statement as_Statement in _import_statement.modules_names.as_statements)
                AddName(as_Statement.real_name);
        }

        public override void visit(from_import_statement _from_import_statement)
        {
            AddName(_from_import_statement.module_name);
        }

        public void AddUsesToUsesList(uses_list _uses_list)
        {
            foreach (string module_name in modulesNames.Keys)
            {
                SourceContext sc = modulesNames[module_name];
                uses_list us = new uses_list(new unit_or_namespace(module_name, sc), sc);
                us[0].source_context = sc;
                _uses_list.AddUsesList(us, sc);
            }
        }

        public override void Exit(syntax_tree_node stn)
        {
            if (stn is program_module pm)
            {
                AddUsesToUsesList(pm.used_units);
            }
            if (stn is unit_module um)
            {
                AddUsesToUsesList(um.interface_part.uses_modules);
            }
        }
    }
}
