// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using PascalABCCompiler.Errors;
using PascalABCCompiler.SyntaxTree;

namespace PascalABCCompiler.Parsers
{

    public abstract class BaseParser: IParser
    {

        public BaseParser(string name, string version, string copyright,
            bool caseSensitive, string[] filesExtensions,
            StandardModule[] standardModules)
        {
            this.name = name;
            this.version = version;
            this.copyright = copyright;
            this.caseSensitive = caseSensitive;
            this.filesExtensions = filesExtensions;
            this.StandardModules = standardModules;
        }

        List<Error> errors = new List<Error>();
        public virtual List<Error> Errors
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

        List<CompilerWarning> warnings = new List<CompilerWarning>();
        public virtual List<CompilerWarning> Warnings
        {
            get
            {
                return warnings;
            }
            set
            {
                warnings = value;
            }
        }

        List<compiler_directive> compilerDirectives = new List<compiler_directive>();
        public virtual List<compiler_directive> CompilerDirectives
        {
            get
            {
                return compilerDirectives;
            }
            set
            {
            	compilerDirectives = value;
            }
        }
         

        private bool caseSensitive;
        public virtual bool CaseSensitive
        {
            get { return caseSensitive; }
        }

        string[] filesExtensions;
        public string[] FilesExtensions
        {
            get { return filesExtensions; }
        }

        string name;
        public string Name
        {
            get { return name; }
        }

        string version;
        public string Version
        {
            get { return version; }
        }

        string copyright;
        public string Copyright
        {
            get { return copyright; }
        }

        /// <summary>
        /// Вспомогательная структура для хранения данных о стандартном модуле
        /// </summary>
        public struct StandardModule
        {
            public string name;
            public bool isHidden;
            public Action<compilation_unit> syntaxTreePostProcessor;
        }

        public StandardModule[] StandardModules { get; }

        public SourceFilesProviderDelegate sourceFilesProvider = null;
        public virtual SourceFilesProviderDelegate SourceFilesProvider
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

        private ILanguageInformation languageInformation;
        public virtual ILanguageInformation LanguageInformation
        {
            get
            {
                if (languageInformation == null)
                    languageInformation = new DefaultLanguageInformation(this);
                return languageInformation;
            }
        }

        public virtual PascalABCCompiler.SyntaxTree.syntax_tree_node BuildTree(string FileName, string Text, ParseMode ParseMode, List<string> DefinesList = null)
        {
            syntax_tree_node root = null;

            PreBuildTree(FileName);
            switch (ParseMode)
            {
                case ParseMode.Normal:
                    root = BuildTreeInNormalMode(FileName, Text);
                    break;
                case ParseMode.Expression:
                    root = BuildTreeInExprMode(FileName, Text);
                    break;
                case ParseMode.Special:
                    root = BuildTreeInSpecialMode(FileName, Text);
                    break;
                case ParseMode.ForFormatter:
                    root = BuildTreeInFormatterMode(FileName, Text);
                    break;
                case ParseMode.Statement:
                    root = BuildTreeInStatementMode(FileName, Text);
                    break;
                default:
                    break;
            }

            if (root != null && root is compilation_unit)
            {
                (root as compilation_unit).file_name = FileName;
                (root as compilation_unit).compiler_directives = CompilerDirectives;
                if (root is unit_module)
                    if ((root as unit_module).unit_name.HeaderKeyword == UnitHeaderKeyword.Library)
                        (root as compilation_unit).compiler_directives.Add(new compiler_directive(new token_info("apptype"), new token_info("dll")));
            }

            return root;
        }

        public virtual void PreBuildTree(string FileName)
        {

        }

        public virtual syntax_tree_node BuildTreeInNormalMode(string FileName, string Text, List<string> DefinesList = null)
        {
            return null;
        }

        public virtual syntax_tree_node BuildTreeInTypeExprMode(string FileName, string Text)
        {
            return null;
        }

        public virtual syntax_tree_node BuildTreeInExprMode(string FileName, string Text)
        {
            return null;
        }

        public virtual syntax_tree_node BuildTreeInSpecialMode(string FileName, string Text)
        {
            return null;
        }

        public virtual syntax_tree_node BuildTreeInFormatterMode(string FileName, string Text)
        {
            return null;
        }

        public virtual syntax_tree_node BuildTreeInStatementMode(string FileName, string Text)
        {
            return null;
        }

        public virtual void Reset()
        {
            // если нужно - переопределяйте
        }        

        public virtual IPreprocessor Preprocessor
        {
            get { return null; }
        }

        public override string ToString()
        {
            return Name + " Language Parser v" + Version;
        }

    }
}
