using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PascalABCCompiler.Errors;
using PascalABCCompiler.SyntaxTree;

namespace VeryBasicParser
{
    public class SPythonUnexpectedToken : SyntaxError
    {
        private string _message;
        private string _expected;
        public SPythonUnexpectedToken(string _file_name, string expected, SourceContext _source_context, syntax_tree_node _node)
            : base("", _file_name, _source_context, _node)
        {
            _message = string.Format(StringResources.Get("EXPECTED{0}"), expected);
            _expected = expected;
        }

        public override string Message
        {
            get
            {
                return _message;
            }
        }

        public string Expected
        {
            get
            {
                return _expected;
            }
            set
            {
                _expected = value;
                _message = string.Format(StringResources.Get("EXPECTED{0}"), _expected);
            }
        }
    }
}
