using System;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.PascalABCParser;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.Errors;
using System.Collections.Generic;


namespace PascalABCCompiler.Preprocessor_2.Errors
{
    public static class StringResources
    {
        private static string prefix = "PABCPreprocessor2_";
        public static string Get(string Id)
        {
            string ret = PascalABCCompiler.StringResources.Get(prefix + Id);
            if (ret == prefix + Id)
                return Id;
            else
                return ret;
        }
    }

    public class IncludeFileNotFound : SyntaxError
    {
        public IncludeFileNotFound(string _file_name, SourceContext _source_context, syntax_tree_node _node, string FileName)
            :
            base(String.Format(StringResources.Get("INCLUDE_FILE_{0}_NOTFOUND"), FileName), _file_name, _source_context, _node)
        {
        }
    }

    public class CircularInclude : SyntaxError
    {
        public CircularInclude(string _file_name, SourceContext _source_context, syntax_tree_node _node, string FileName)
            :
            base(String.Format(StringResources.Get("CIRCULAR_INCLUDE_{0}"), FileName), _file_name, _source_context, _node)
        {
        }
    }
    
    public class ErrorInDefineSyntax : SyntaxError
    {
        public ErrorInDefineSyntax(string _file_name, SourceContext _source_context)
            :
            base(StringResources.Get("ERROR_IN_DEFINE_SYNTAX"), _file_name, _source_context, null)
        {
        }
    }

    public class NameDefineError : SyntaxError
    {
        public NameDefineError(string name, string _file_name, SourceContext _source_context)
            :
            base(String.Format(StringResources.Get("NAME_IS_DEFINED_{0}"), name), _file_name, _source_context, null)
        {
        }
    }

    public class NameUndefError : SyntaxError
    {
        public NameUndefError(string name, string _file_name, SourceContext _source_context)
            :
            base(String.Format(StringResources.Get("NAME_WAS_NOT_DEFINED_{0}"), name), _file_name, _source_context, null)
        {
        }
    }

    public class ErrorInIncludeSyntax : SyntaxError
    {
        public ErrorInIncludeSyntax(string _file_name, SourceContext _source_context)
            :
            base(StringResources.Get("ERROR_IN_INCLUDE_SYNTAX"), _file_name, _source_context, null)
        {
        }
    }
    public class UnexpectedEndif : SyntaxError
    {
        public UnexpectedEndif(string _file_name, SourceContext _source_context)
            :
            base(StringResources.Get("UNEXPEXTED_ENDIF_DIRECTIVE"), _file_name, _source_context, null)
        {
        }
    }
    public class UnexpectedElse : SyntaxError
    {
        public UnexpectedElse(string _file_name, SourceContext _source_context)
            :
            base(StringResources.Get("UNEXPEXTED_ELSE_DIRECTIVE"), _file_name, _source_context, null)
        {
        }
    }
    public class UnexpectedEOF : SyntaxError
    {
        public UnexpectedEOF(string _file_name, SourceContext _source_context)
            :
            base(StringResources.Get("ENDIF_DIRECTIVE_EXPEXTED"), _file_name, _source_context, null)
        {
        }
    }
    public class SyntaxErrorInDirective : SyntaxError
    {
        public SyntaxErrorInDirective(string _file_name, SourceContext _source_context, string command)
            :
            base(String.Format(StringResources.Get("SYNTAX_ERROR_IN_DIRECTIVE_{0}"), command), _file_name, _source_context, null)
        {
        }
    }
    public class UnknownDirective : SyntaxError
    {
        public UnknownDirective(string _file_name, SourceContext _source_context, string command)
            :
            base(String.Format(StringResources.Get("UNKNOWN_DIRECTIVE_{0}"), command), _file_name, _source_context, null)
        {
        }
    }

    //aspects
    public class AspSpacesError : SyntaxError
    {
        public AspSpacesError(string _file_name, SourceContext _source_context, string command)
            :
            base(String.Format(StringResources.Get("UNEXPECTED_SPACES_IN_DIRECTIVE_{0}"), command), _file_name, _source_context, null)
        {
        }
    }

    public class UnexpAspData : SyntaxError
    {
        public UnexpAspData(string _file_name, SourceContext _source_context, string command)
            :
            base(String.Format(StringResources.Get("UNEXPECTED_ASPDATA_DIRECTIVE_{0}"), command), _file_name, _source_context, null)
        {
        }
    }

    public class DoubleAspData : SyntaxError
    {
        public DoubleAspData(string _file_name, SourceContext _source_context, string command)
            :
            base(String.Format(StringResources.Get("DOUBLE_ASPDATA_DIRECTIVE_{0}"), command), _file_name, _source_context, null)
        {
        }
    }

    public class ErrorAspData : SyntaxError
    {
        public ErrorAspData(string _file_name, SourceContext _source_context, string command)
            :
            base(String.Format(StringResources.Get("ERROR_ASPDATA_DIRECTIVE_{0}"), command), _file_name, _source_context, null)
        {
        }
    }

    public class AspBeginEndError : SyntaxError
    {
        public AspBeginEndError(string _file_name, SourceContext _source_context, string command)
            :
            base(String.Format(StringResources.Get("ASPBEGINEND_ERROR_IN_DIRECTIVE_{0}"), command), _file_name, _source_context, null)
        {
        }
    }
    //end aspects

}
