using System;
using System.Collections.Generic;
using System.Text;
using PascalABCCompiler;
using PascalABCCompiler.Errors;
using System.IO;
using PascalABCCompiler.SyntaxTree;
using System.Text.RegularExpressions;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.Parsers;
using PascalABCCompiler.PascalPreprocessor;

namespace PascalABCCompiler.PascalPreprocessor
{
    public class Preprocessor : IPreprocessor
    {
        public Preprocessor(SourceFilesProviderDelegate sourceFilesProvider)
        {
            this.sourceFilesProvider = sourceFilesProvider;
        }

        SourceFilesProviderDelegate sourceFilesProvider;
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

        List<compiler_directive> compilerDirectives=null;
        public List<compiler_directive> CompilerDirectives
        {
            get
            {
                return compilerDirectives;
            }
        }

        PascalPreprocessorLanguageParser PascalPreprocessorParser;
        public string Build(string[] fileNames,string[] SearchPaches, List<Error> errors, SourceContextMap sourceContextMap)
        {
            if (PascalPreprocessorParser == null)
                PascalPreprocessorParser = new PascalPreprocessorLanguageParser();
            compilerDirectives = new List<compiler_directive>();
            string text = (string)SourceFilesProvider(fileNames[0], SourceFileOperation.GetText);
            PascalPreprocessorParser.Errors = errors;
            compilation_unit cu = PascalPreprocessorParser.BuildTree(fileNames[0], text, SearchPaches, PascalABCCompiler.Parsers.ParseMode.Normal) as compilation_unit;
            if (errors.Count > 0)
                return text;
            if (cu == null)
                errors.Add(new CompilerInternalError("PascalPreprocessor", new Exception("cu==null")));
            if (cu != null)
                compilerDirectives.AddRange(cu.compiler_directives);
            return text;
        }

    }    
}
