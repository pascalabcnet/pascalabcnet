
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;
using PascalABCCompiler.PascalABCParser.Errors;
using PascalABCCompiler.ParserTools;
using GoldParser;
using PascalABCCompiler.PascalABCParser;


namespace  PascalABCCompiler.PascalPreprocessor
{
    
    public class GPBPreprocessor_Pascal:GPBParser
    {
        private int operation_value=0;
        private object left_node,right_node;
        private Stack<compiler_directive> if_part = new Stack<compiler_directive>();
        private Stack<compiler_directive> first_if = new Stack<compiler_directive>();
        private pascalabc_parsertools pascal_parsertools;

        public GPBPreprocessor_Pascal(Stream stream)
            :base(stream)
        {
	    pascal_parsertools = new pascalabc_parsertools();
            parsertools = pascal_parsertools;
        }
		
        public GPBPreprocessor_Pascal(Stream stream, GoldParser.Grammar grammar):base(grammar)
        {
        	pascal_parsertools = new pascalabc_parsertools();
            parsertools = pascal_parsertools;
        }
        
        public object Parse(string source)
        {
            if_part.Clear();
	    first_if.Clear();
            LRParser = new Parser(new StringReader(source), LanguageGrammar);
            parsertools.parser = LRParser;
            LRParser.TrimReductions = true;
		try
{
            while (true)
            {
                switch (LRParser.Parse())
                {
                    case ParseMessage.LexicalError:
                        errors.Add(new TokenReadError(this));
                        LRParser.PopInputToken();
                        if (errors.Count > max_errors)
                            return null;
                        break;

                    case ParseMessage.SyntaxError:
                        if ((LRParser.TokenSyntaxNode as syntax_tree_node)!= null) 
                            prev_node = LRParser.TokenSyntaxNode;                            
                        Error er = new PABCNETUnexpectedToken(this);
                        Symbol sym = LRParser.PopInputToken();
                        if (sym.SymbolType == SymbolType.End && errors.Count > 0)
                            return null;
                        errors.Add(er);
                        if (errors.Count > max_errors)
                            return null;
                        break;
                    case ParseMessage.Reduction:
                        LRParser.TokenSyntaxNode = CreateNonTerminalObject();
                        break;

                    case ParseMessage.Accept:
                        return LRParser.TokenSyntaxNode;

                    case ParseMessage.TokenRead:
                        LRParser.TokenSyntaxNode = CreateTerminalObject();
                        break;

                    case ParseMessage.InternalError:
                        errors.Add(new CompilerInternalError("CParser", new Exception("ParseMessage.InternalError")));
                        return null;

                    case ParseMessage.NotLoadedError:
                        errors.Add(new CompilerInternalError("CParser", new Exception("ParseMessage.NotLoadedError")));
                        return null;

                    case ParseMessage.CommentError:
                        errors.Add(new UnexpectedToken(this,"(EOF)"));
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

    public enum SymbolConstants : int
    {
        SYMBOL_EOF                  =  0, // (EOF)
        SYMBOL_ERROR                =  1, // (Error)
        SYMBOL_WHITESPACE           =  2, // (Whitespace)
        SYMBOL_COMMENTEND           =  3, // (Comment End)
        SYMBOL_COMMENTLINE          =  4, // (Comment Line)
        SYMBOL_COMMENTSTART         =  5, // (Comment Start)
        SYMBOL_TKDIRECTIVE          =  6, // 'TKDIRECTIVE'
        SYMBOL_TKNEWLINE            =  7, // 'TKNEWLINE'
        SYMBOL_TKPROGRAM_LINE_PART  =  8, // 'TKPROGRAM_LINE_PART'
        SYMBOL__DIRECTIVE_GROUPS    =  9, // <_directive_groups>
        SYMBOL_DIRECTIVE_GROUP      = 10, // <directive_group>
        SYMBOL_DIRECTIVE_GROUPS     = 11, // <directive_groups>
        SYMBOL_EMPTY                = 12, // <empty>
        SYMBOL_PREPROCESSOR_PROGRAM = 13  // <preprocessor_program>
    };

    public enum RuleConstants : int
    {
        RULE_PREPROCESSOR_PROGRAM                = 0, // <preprocessor_program> ::= <directive_groups> <empty>
        RULE_DIRECTIVE_GROUPS                    = 1, // <directive_groups> ::= <_directive_groups>
        RULE_DIRECTIVE_GROUPS2                   = 2, // <directive_groups> ::= 
        RULE__DIRECTIVE_GROUPS                   = 3, // <_directive_groups> ::= <directive_group> <empty>
        RULE__DIRECTIVE_GROUPS2                  = 4, // <_directive_groups> ::= <directive_groups> <directive_group>
        RULE_DIRECTIVE_GROUP_TKNEWLINE           = 5, // <directive_group> ::= 'TKNEWLINE'
        RULE_DIRECTIVE_GROUP_TKPROGRAM_LINE_PART = 6, // <directive_group> ::= 'TKPROGRAM_LINE_PART'
        RULE_DIRECTIVE_GROUP_TKDIRECTIVE         = 7, // <directive_group> ::= 'TKDIRECTIVE' <empty>
        RULE_EMPTY                               = 8  // <empty> ::= 
    };

        private Object CreateTerminalObject()
        {
            switch ((SymbolConstants) LRParser.TokenSymbol.Index)
            {
		case SymbolConstants.SYMBOL_EOF :
		//(EOF)
		//TERMINAL:EOF
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_ERROR :
		//(Error)
		//TERMINAL:Error
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_WHITESPACE :
		//(Whitespace)
 /*Console.Error.WriteLine("\n\rwarning: TerminalToken 'Whitespace' return NULL! \n\r");*/return null;

		case SymbolConstants.SYMBOL_COMMENTEND :
		//(Comment End)
 /*Console.Error.WriteLine("\n\rwarning: TerminalToken 'Comment End' return NULL! \n\r");*/return null;

		case SymbolConstants.SYMBOL_COMMENTLINE :
		//(Comment Line)
 /*Console.Error.WriteLine("\n\rwarning: TerminalToken 'Comment Line' return NULL! \n\r");*/return null;

		case SymbolConstants.SYMBOL_COMMENTSTART :
		//(Comment Start)
 /*Console.Error.WriteLine("\n\rwarning: TerminalToken 'Comment Start' return NULL! \n\r");*/return null;

		case SymbolConstants.SYMBOL_TKDIRECTIVE :
		//'TKDIRECTIVE'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKNEWLINE :
		//'TKNEWLINE'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKPROGRAM_LINE_PART :
		//'TKPROGRAM_LINE_PART'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL__DIRECTIVE_GROUPS :
		//<_directive_groups>
		//TERMINAL:_directive_groups
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_DIRECTIVE_GROUP :
		//<directive_group>
		//TERMINAL:directive_group
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_DIRECTIVE_GROUPS :
		//<directive_groups>
		//TERMINAL:directive_groups
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_EMPTY :
		//<empty>
		//TERMINAL:empty
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_PREPROCESSOR_PROGRAM :
		//<preprocessor_program>
		//TERMINAL:preprocessor_program
		return null;
		//ENDTERMINAL

            }
            throw new SymbolException("Unknown symbol");
        }

	

        public Object CreateNonTerminalObject()
        {
            switch ((RuleConstants) LRParser.ReductionRule.Index)
            {
		case RuleConstants.RULE_PREPROCESSOR_PROGRAM :
		//<preprocessor_program> ::= <directive_groups> <empty>
         
		{
			compilation_unit _compilation_unit=new compilation_unit();
			 
				_compilation_unit.Language=LanguageId.PascalABCNET;
                                if(LRParser.GetReductionSyntaxNode(0)!=null){
                                	parsertools.create_source_context(_compilation_unit,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
					CompilerDirectives.AddRange(((compiler_directive_list)LRParser.GetReductionSyntaxNode(0)).directives);
				}
				
			return _compilation_unit;
		}


		case RuleConstants.RULE_DIRECTIVE_GROUPS :
		//<directive_groups> ::= <_directive_groups>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_DIRECTIVE_GROUPS2 :
		//<directive_groups> ::= 
		//NONTERMINAL:<directive_groups> ::= 
		return null;
		//ENDNONTERMINAL

		case RuleConstants.RULE__DIRECTIVE_GROUPS :
		//<_directive_groups> ::= <directive_group> <empty>
         
		{
			compiler_directive_list _compiler_directive_list=new compiler_directive_list();
			
						  if(LRParser.GetReductionSyntaxNode(0) is compiler_directive)
							_compiler_directive_list.directives.Add((compiler_directive)LRParser.GetReductionSyntaxNode(0));
						    parsertools.create_source_context(_compiler_directive_list,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			return _compiler_directive_list;
		}


		case RuleConstants.RULE__DIRECTIVE_GROUPS2 :
		//<_directive_groups> ::= <directive_groups> <directive_group>
         
		{
			compiler_directive_list _compiler_directive_list=(compiler_directive_list)LRParser.GetReductionSyntaxNode(0); 
						  if(LRParser.GetReductionSyntaxNode(1) is compiler_directive)
							_compiler_directive_list.directives.Add((compiler_directive)LRParser.GetReductionSyntaxNode(1));
						  parsertools.create_source_context(_compiler_directive_list,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _compiler_directive_list;
		}


		case RuleConstants.RULE_DIRECTIVE_GROUP_TKNEWLINE :
		//<directive_group> ::= 'TKNEWLINE'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_DIRECTIVE_GROUP_TKPROGRAM_LINE_PART :
		//<directive_group> ::= 'TKPROGRAM_LINE_PART'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_DIRECTIVE_GROUP_TKDIRECTIVE :
		//<directive_group> ::= 'TKDIRECTIVE' <empty>
         
		{
			compiler_directive _compiler_directive=(compiler_directive)pascal_parsertools.MakeDirective((token_info)LRParser.GetReductionSyntaxNode(0));
			return _compiler_directive;
		}


		case RuleConstants.RULE_EMPTY :
		//<empty> ::= 
		//NONTERMINAL:<empty> ::= 
		return null;
		//ENDNONTERMINAL

            }
            throw new RuleException("Unknown rule");
        }
	}
}
