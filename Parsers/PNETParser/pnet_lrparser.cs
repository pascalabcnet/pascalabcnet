
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Collections;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;
using PascalABCCompiler.PNETParser.Errors;
using PascalABCCompiler.ParserTools;

namespace PascalABCCompiler.PNETParser
{
    public partial class GPBParser_PNET : GPBParser
    {
        private Stack NodesStack;
        private SyntaxError error;

        Stream GrammarStream;

        public GPBParser_PNET(Stream stream)
            : base(stream)
        {
            GrammarStream = stream;
            parsertools = new pnet_parsertools();
        }

        public object Parse(string source)
        {
            NodesStack = new Stack();
            LRParser = new GoldParser.Parser(new StringReader(source), LanguageGrammar);
            parsertools.parser  = LRParser;
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
                            error = new PABCNETUnexpectedToken(this);
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
                            errors.Add(new CompilerInternalError("PascalABCParser", new Exception("ParseMessage.InternalError")));
                            return null;

                        case GoldParser.ParseMessage.NotLoadedError:
                            errors.Add(new CompilerInternalError("PascalABCParser", new Exception("ParseMessage.NotLoadedError")));
                            return null;

                        case GoldParser.ParseMessage.CommentError:
                            errors.Add(new UnexpectedToken(this, "(EOF)"));
                            return null;

                        /*case ParseMessage.CommentBlockRead:
                            break;

                        case ParseMessage.CommentLineRead:
                            break;*/
                    }
                }
            }
            catch (GoldParser.UnexpectedEOFinParseCommentBlock)
            {
                throw new TokenReadError(this);
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
