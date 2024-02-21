using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeConverter;
using YATLTreeConversion;


namespace PascalABCCompiler.TreeConverter.TreeConversion
{
    class SyntaxTreeVisitorsController
    {
        private List<syntax_tree_visitor> syntaxTreeVisitors = new List<syntax_tree_visitor>();
        public List<syntax_tree_visitor> SyntaxTreeVisitors
        {
            get
            {
                return syntaxTreeVisitors;
            }
        }
        public SyntaxTreeVisitorsController()
        {
            AddConverters();
        }
        public void AddConverters()
        {
            var passv = new syntax_tree_visitor();
            var yatlsv = new yatl_syntax_tree_visitor();
            syntaxTreeVisitors.Add(passv);
            syntaxTreeVisitors.Add(yatlsv);
        }
        public syntax_tree_visitor SelectVisitor(string ext)
        {
            foreach (syntax_tree_visitor stv in SyntaxTreeVisitors)
                foreach (string stvfext in stv.FilesExtensions)
                    if (stvfext.ToLower() == ext)
                        return stv;
            return null;
        }
    }
}
