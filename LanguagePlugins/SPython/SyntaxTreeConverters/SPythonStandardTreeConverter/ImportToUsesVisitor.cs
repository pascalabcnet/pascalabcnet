using System.Collections.Generic;
using System.Data;
using System.Linq;
using PascalABCCompiler.SyntaxTree;
using SyntaxVisitors;

namespace Languages.SPython.Frontend.Converters
{
    internal class ImportToUsesVisitor : BaseChangeVisitor
    {
        HashSet<string> modulesNames = new HashSet<string>();

        public ImportToUsesVisitor() { }

        private Dictionary<string, string> specialModulesAliases = new Dictionary<string, string>
        {
            { "time", "time1" },
            { "random", "random1" },
        };

        private string GetNameToImport(string nameToImport)
        {
            if (!specialModulesAliases.ContainsKey(nameToImport))
                return nameToImport;
            else 
                return specialModulesAliases[nameToImport];
        }

        public override void visit(import_statement _import_statement)
        {
            foreach (as_statement as_Statement in _import_statement.modules_names.as_statements)
                modulesNames.Add(GetNameToImport(as_Statement.real_name.name));
        }

        public override void visit(from_import_statement _from_import_statement)
        {
            modulesNames.Add(GetNameToImport(_from_import_statement.module_name.name));
        }

        public override void Exit(syntax_tree_node stn)
        {
            if (stn is program_module pm)
            {
                foreach (string module_name in modulesNames)
                    pm.used_units.Add(new unit_or_namespace(new ident_list(new ident(module_name))));
            }
        }
    }
}
