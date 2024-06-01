using System;
using System.Collections.Generic;
using System.Text;
using PascalABCCompiler.Errors;
using PascalABCCompiler.SyntaxTree;

namespace PascalABCCompiler.PythonABCParser.Errors
{
    public class UnexpectedToken : SyntaxError
    {
        private string _message;

        public UnexpectedToken(string _file_name,SourceContext _source_context,syntax_tree_node _node)
            : base("",_file_name,_source_context,_node)
        {
            _message = string.Format(StringResources.Get("EXPECTED{0}"),"!");
        }

        public override string Message
        {
            get
            {
                return _message;
            }
        }
    }

    public class UnexpectedStmt : SyntaxError
    {
        private string _message;

        public UnexpectedStmt(string _file_name,SourceContext _source_context,syntax_tree_node _node)
            : base("",_file_name,_source_context,_node)
        {
            _message = StringResources.Get("ONE_STMT");
        }

        public override string Message
        {
            get
            {
                return _message;
            }
        }
    }

    public class ErrorMsg : SyntaxError
    {
        private string _message;

        public ErrorMsg(string _file_name,SourceContext _source_context,string _msg)
            : base("",_file_name,_source_context,new syntax_tree_node())
        {
            _message = StringResources.Get(_msg);
        }

        public override string Message
        {
            get
            {
                return _message;
            }
        }
    }
}
