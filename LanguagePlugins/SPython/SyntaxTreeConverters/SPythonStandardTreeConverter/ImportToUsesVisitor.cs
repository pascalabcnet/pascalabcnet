using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
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

        private void AddName(string name)
        {
            string nameToImport = GetNameToImport(name);
            if (!modulesNames.Contains(nameToImport))
                modulesNames.Add(nameToImport);
        }

        public override void visit(import_statement _import_statement)
        {
            foreach (as_statement as_Statement in _import_statement.modules_names.as_statements)
                AddName(as_Statement.real_name.name);
        }

        public override void visit(from_import_statement _from_import_statement)
        {
            AddName(_from_import_statement.module_name.name);
        }

        public void AddUsesToUsesList(uses_list _uses_list)
        {
            foreach (string module_name in modulesNames)
                _uses_list.AddUsesList(new uses_list(module_name));
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
