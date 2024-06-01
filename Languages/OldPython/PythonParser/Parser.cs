using System;
using System.IO;
using System.Text;
using System.Reflection;
using GPPGTools;
using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Parsers;
using PascalABCCompiler.Errors;
using System.Collections.Generic;


namespace PascalABCCompiler.PythonABCParser
{
    public class PythonLanguageParser : IParser
    {
        private Parser parser = new Parser();
        
        public PythonLanguageParser()
        {
            filesExtensions = new string[1];
            filesExtensions[0] = ".py";
        }

        string[] filesExtensions;
        public string[] FilesExtensions
        {
            get
            {
                return filesExtensions;
            }
        }

        public ILanguageInformation LanguageInformation
        {
            get
            {
                return new DefaultLanguageInformation(this);
            }
        }

        public void Reset()
        {
            GPPGParser.max_errors = 30;
            GPPGParser.CompilationUnit = null;
            GPPGParser.errors = Errors;
            //GPPGParser.CompilerDirectives = new List<compiler_directive>();
        }

        public bool CaseSensitive
        {
            get
            {
                return true;
            }
        }

        public List<compiler_directive> compilerDirectives = new List<compiler_directive>();
        public List<compiler_directive> CompilerDirectives
        {
            get
            {
                return compilerDirectives;
            }
        }

        SourceFilesProviderDelegate sourceFilesProvider = null;
        public SourceFilesProviderDelegate SourceFilesProvider
        {
            get
            {
                return sourceFilesProvider;
            }
            set
            {
                sourceFilesProvider = value;
            }
        }

        List<Error> errors = new List<Error>();
        public List<Error> Errors
        {
            get
            {
                return errors;
            }
            set
            {
                errors = value;
            }
        }

        public Keyword[] Keywords
        {
            get
            {
                return new Keyword[0];
            }
        }

        public syntax_tree_node BuildTree(string FileName, string Text, ParseMode ParseMode)
        {
            //if (parser == null)
            Reset();
            
            GPPGParser.current_file_name = FileName;
            GPPGParser.CompilerDirectives = compilerDirectives;
            
            Scanner scn = new Scanner();
            scn.SetSource(Text, 0);

            parser.scanner = scn;

            bool b = parser.Parse();

            if (!b)
            {
                // if we can't recognize error :( or brain.dll not loaded
                Errors.ErrorMsg err = new Errors.ErrorMsg(GPPGParser.current_file_name,new SourceContext(0,0,0,0),"PROGRAM_ERROR");
                GPPGParser.errors.Add(err);
            }

            Errors = GPPGParser.errors;

            syntax_tree_node cu = null;

            switch (ParseMode)
            {
                case ParseMode.Normal:
                    cu = GPPGParser.CompilationUnit;
                    break;
                case ParseMode.Expression:
                case ParseMode.Statement:
                    return null;
            }

            if (cu != null && cu is compilation_unit)
            {
                (cu as compilation_unit).file_name = FileName;
                (cu as compilation_unit).compiler_directives = CompilerDirectives;
            }
            return cu;
        }

        public string Name
        {
            get
            {
                return "PythonABC";
            }
        }

        public string Version
        {
            get
            {
                return "0.1";
            }
        }

        public string Copyright
        {
            get
            {
                return "(c) Andrew Podkovyrin, 2009";
            }
        }

        public override string ToString()
        {
            return "Python Language Parser v" + Version;
        }

        public IPreprocessor Preprocessor
        {
            get
            {
                return null;
            }
        }
    }
}
