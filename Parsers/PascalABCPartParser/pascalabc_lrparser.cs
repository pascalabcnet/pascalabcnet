
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.PascalABCParser.Errors;
using PascalABCCompiler.TestParserTools;

namespace PascalABCCompiler.PascalABCParser
{
    public class ParserConfig
    {
        public int[] pas_tokens = new int[21];
        public ParserConfig()
        {
            pas_tokens[0] = (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_EOF;
            pas_tokens[1] = (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKBEGIN;
            pas_tokens[2] = (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKEND;
            pas_tokens[3] = (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKROUNDOPEN;
            pas_tokens[4] = (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKROUNDCLOSE;
            pas_tokens[5] = (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKIF;
            pas_tokens[6] = (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKTHEN;
            pas_tokens[7] = (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKWHILE;
            pas_tokens[8] = (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKDO;
            pas_tokens[9] = (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKREPEAT;
            pas_tokens[10] = (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKUNTIL;
            pas_tokens[11] = (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKIDENTIFIER;
            pas_tokens[12] = (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKPOINT;
            pas_tokens[13] = (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_WHITESPACE;
            pas_tokens[14] = (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKSEMICOLON;
            pas_tokens[15] = (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKINTEGER;
            pas_tokens[16] = (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKCASE;
            pas_tokens[17] = (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKPROCEDURE;
            pas_tokens[18] = (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKCLASS;
            pas_tokens[19] = (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKRECORD;
            pas_tokens[20] = (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKFUNCTION;
        }
    };
    public partial class GPBParser_PascalABC : GPBParser
    {
        // /////////////////////////////////////////
        public Dictionary<string, ParserConfig> Configs = new Dictionary<string, ParserConfig>();
        public ParserConfig GetPascalConfig()
        {
            if (Configs.ContainsKey("Pascal"))
                return Configs["Pascal"];
            ParserConfig config = new ParserConfig();
            Configs.Add("Pascal", config);
            return config;
        }
        // /////////////////////////////////////////
        private Stack NodesStack;
        private PascalABCCompiler.Errors.SyntaxError error;
        public bool build_tree_for_brackets = false;

        public GPBParser_PascalABC(Stream stream)
            : base()
        {
            LanguageGrammar = new PascalABCCompiler.TestGoldParserEngine.Grammar(new BinaryReader(stream));
            parsertools = new pascalabc_parsertools();
        }

        public object Parse(string source)
        {
            NodesStack = new Stack();
            LRParser = new PascalABCCompiler.TestGoldParserEngine.Parser(new StringReader(source), LanguageGrammar);
            // //////////////////////////////
            LRParser.PartModeON = true;
            ParserConfig config = GetPascalConfig();
            LRParser.PartConfig = config;
            // //////////////////////////////
            parsertools.parser = LRParser;
            LRParser.TrimReductions = true;
            //
            LRParser.SaveParserState();
            //
            try
            {
                while (true)
                {
                    switch (LRParser.Parse())
                    {
                        case PascalABCCompiler.TestGoldParserEngine.ParseMessage.LexicalError:
                            errors.Add(new TokenReadError(this));
                            LRParser.PopInputToken();
                            if (errors.Count >= max_errors)
                                return null;
                            break;

                        //sdes nugno vesti chtoto vrode steka poslednix oshibok. podumat
                        case PascalABCCompiler.TestGoldParserEngine.ParseMessage.SyntaxError:
                            //
                            if (LRParser.ps.Count!=0)
                            {
                                PABCNETUnexpectedToken error_t = new PABCNETUnexpectedToken(this);//tasha
                                LRParser.error_token = error_t.error_token;//tasha
                                LRParser.RestoreParserState();
                                error = new PABCNETUnexpectedToken(this);
                                //LRParser.PopInputToken();
                                PascalABCCompiler.TestGoldParserEngine.Symbol sym1 = error_t.error_token;
                                if (sym1.SymbolType == PascalABCCompiler.TestGoldParserEngine.SymbolType.End && errors.Count > 0)
                                    return null;
                                errors.Add(error);
                                if (errors.Count >= max_errors)
                                    return null;
                                break;
                            }
                            //
                            PascalABCCompiler.TestGoldParserEngine.Symbol sym = LRParser.PopInputToken();
                            error = new PABCNETUnexpectedToken(this);
                            //error = new UnexpectedToken(this, parsertools.symbol_collection_to_string(this.LRParser.GetExpectedTokens()));
                            if (sym.SymbolType == PascalABCCompiler.TestGoldParserEngine.SymbolType.End && errors.Count > 0)
                                return null;
                            errors.Add(error);
                            if (errors.Count >= max_errors)
                                return null;
                            break;
                        case PascalABCCompiler.TestGoldParserEngine.ParseMessage.Reduction:
                            try
                            {
                                LRParser.TokenSyntaxNode = CreateNonTerminalObject(LRParser.ReductionRule.Index);
                                if ((LRParser.TokenSyntaxNode as syntax_tree_node) != null)
                                    prev_node = LRParser.TokenSyntaxNode;
                            }
                            catch (Exception e)
                            {
                                if (LRParser.ps.Count != 0)
                                {
                                    LRParser.RestoreParserState();
                                    break;
                                }
                            }
                            break;

                        case PascalABCCompiler.TestGoldParserEngine.ParseMessage.Accept:
                            return LRParser.TokenSyntaxNode;

                        case PascalABCCompiler.TestGoldParserEngine.ParseMessage.TokenRead:
                            LRParser.TokenSyntaxNode = CreateTerminalObject(LRParser.TokenSymbol.Index);
                            break;

                        case PascalABCCompiler.TestGoldParserEngine.ParseMessage.InternalError:
                            errors.Add(new PascalABCCompiler.Errors.CompilerInternalError("PascalABCParser", new Exception("ParseMessage.InternalError")));
                            return null;

                        case PascalABCCompiler.TestGoldParserEngine.ParseMessage.NotLoadedError:
                            errors.Add(new PascalABCCompiler.Errors.CompilerInternalError("PascalABCParser", new Exception("ParseMessage.NotLoadedError")));
                            return null;

                        case PascalABCCompiler.TestGoldParserEngine.ParseMessage.CommentError:
                            errors.Add(new UnexpectedToken(this, "(EOF)"));
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
