using System;
using System.IO;
using System.Runtime.Serialization;
using System.Collections;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.KuMirParser.Errors;

namespace PascalABCCompiler.KuMirParser
{
    public partial class GPBParser_KuMir : GPBParser
    {
        private Stack NodesStack;
        private SyntaxError error;
        private int unit_number;
        public ArrayList _units;

        Stream GrammarStream;

        public GPBParser_KuMir(Stream stream)
            : base(stream)
        {
            GrammarStream = stream;
            parsertools = new KuMir_parsertools();
            unit_number = 0;
            _units = new ArrayList();
        }

        public object Parse(string source)
        {
            _units.Add(new unit_data());
            unit_number++;

            NodesStack = new Stack();
            LRParser = new GoldParser.Parser(new StringReader(source), LanguageGrammar);
            parsertools.parser = LRParser;
            LRParser.TrimReductions = true;
            try
            {
                while (true)
                {
                    switch (LRParser.Parse())
                    {
                        case GoldParser.ParseMessage.LexicalError:
                            errors.Add(new TokenReadError(this));
                            LRParser.PopInputToken();
                            if (errors.Count >= max_errors)
                                return null;
                            break;

                        //sdes nugno vesti chtoto vrode steka poslednix oshibok. podumat
                        case GoldParser.ParseMessage.SyntaxError:
                            GoldParser.Symbol sym = LRParser.PopInputToken();
                            error = new KuMirParser.Errors.UnexpectedToken(this);
                            //error = new UnexpectedToken(this, parsertools.symbol_collection_to_string(this.LRParser.GetExpectedTokens()));
                            if (sym.SymbolType == GoldParser.SymbolType.End && errors.Count > 0)
                                return null;
                            errors.Add(error);
                            if (errors.Count >= max_errors)
                                return null;
                            break;
                        case GoldParser.ParseMessage.Reduction:
                            LRParser.TokenSyntaxNode = CreateNonTerminalObject(LRParser.ReductionRule.Index);
                            if ((LRParser.TokenSyntaxNode as syntax_tree_node) != null)
                                prev_node = LRParser.TokenSyntaxNode;
                            break;

                        case GoldParser.ParseMessage.Accept:
                            return LRParser.TokenSyntaxNode;

                        case GoldParser.ParseMessage.TokenRead:
                            LRParser.TokenSyntaxNode = CreateTerminalObject(LRParser.TokenSymbol.Index);
                            break;

                        case GoldParser.ParseMessage.InternalError:
                            errors.Add(new CompilerInternalError("KuMirParser", new Exception("ParseMessage.InternalError")));
                            return null;

                        case GoldParser.ParseMessage.NotLoadedError:
                            errors.Add(new CompilerInternalError("KuMirParser", new Exception("ParseMessage.NotLoadedError")));
                            return null;

                        case GoldParser.ParseMessage.CommentError:
                            errors.Add(new PascalABCCompiler.Errors.UnexpectedToken(this, "(EOF)"));
                            return null;

                        /*case ParseMessage.CommentBlockRead:
                            break;

                        case ParseMessage.CommentLineRead:
                            break;*/
                    }
                }
            }
            catch (Exception e)
            {
                if (errors.Count > 0)
                    return null;
                else
                    throw;
            }

        }
    }

}