
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;
using PascalABCCompiler.CParser.Errors;
using PascalABCCompiler.ParserTools;
using GoldParser;
using PascalABCCompiler.CParser;

namespace  PascalABCCompiler.CPreprocessor
{
    
    public class GPBPreprocessor_C:GPBParser
    {
        private int operation_value=0;
        private object left_node,right_node;
        private Stack<compiler_directive> if_part = new Stack<compiler_directive>();
        private Stack<compiler_directive> first_if = new Stack<compiler_directive>();

        public GPBPreprocessor_C(Stream stream)
            :base(stream)
        {
            parsertools = new C_parsertools();
        }

        public object Parse(string source)
        {
            if_part.Clear();
	    first_if.Clear();
            LRParser = new Parser(new StringReader(source), LanguageGrammar);
            parsertools.parser = LRParser;
            LRParser.TrimReductions = false;
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

    public enum SymbolConstants : int
    {
        SYMBOL_EOF                  =  0, // (EOF)
        SYMBOL_ERROR                =  1, // (Error)
        SYMBOL_WHITESPACE           =  2, // (Whitespace)
        SYMBOL_COMMENTEND           =  3, // (Comment End)
        SYMBOL_COMMENTLINE          =  4, // (Comment Line)
        SYMBOL_COMMENTSTART         =  5, // (Comment Start)
        SYMBOL_TKPNDELIF            =  6, // 'TKPNDELIF'
        SYMBOL_TKPNDELSE            =  7, // 'TKPNDELSE'
        SYMBOL_TKPNDENDIF           =  8, // 'TKPNDENDIF'
        SYMBOL_TKPNDIF              =  9, // 'TKPNDIF'
        SYMBOL_TKPNDIFDEF           = 10, // 'TKPNDIFDEF'
        SYMBOL_TKPNDIFNDEF          = 11, // 'TKPNDIFNDEF'
        SYMBOL_TKPNDUNKNOW          = 12, // 'TKPNDUNKNOW'
        SYMBOL_TKPROGRAM_LINE       = 13, // 'TKPROGRAM_LINE'
        SYMBOL_TKPROGRAM_LINE_PART  = 14, // 'TKPROGRAM_LINE_PART'
        SYMBOL__DIRECTIVE_GROUPS    = 15, // <_directive_groups>
        SYMBOL_CONDITIONAL_GROUP    = 16, // <conditional_group>
        SYMBOL_CONTROL_LINE         = 17, // <control_line>
        SYMBOL_DIRECTIVE_GROUP      = 18, // <directive_group>
        SYMBOL_DIRECTIVE_GROUPS     = 19, // <directive_groups>
        SYMBOL_ELIF_LINE            = 20, // <elif_line>
        SYMBOL_ELIF_PART            = 21, // <elif_part>
        SYMBOL_ELSE_LINE            = 22, // <else_line>
        SYMBOL_ELSE_PART            = 23, // <else_part>
        SYMBOL_ENDIF_LINE           = 24, // <endif_line>
        SYMBOL_ENDIF_PART           = 25, // <endif_part>
        SYMBOL_IF_LINE              = 26, // <if_line>
        SYMBOL_IF_PART              = 27, // <if_part>
        SYMBOL_IFDEF_LINE           = 28, // <ifdef_line>
        SYMBOL_IFNDEF_LINE          = 29, // <ifndef_line>
        SYMBOL_PREPROCESSOR_PROGRAM = 30, // <preprocessor_program>
        SYMBOL_UNKNOW_LINE          = 31  // <unknow_line>
    };

    public enum RuleConstants : int
    {
        RULE_PREPROCESSOR_PROGRAM                     =  0, // <preprocessor_program> ::= <directive_groups>
        RULE_PREPROCESSOR_PROGRAM_TKPROGRAM_LINE_PART =  1, // <preprocessor_program> ::= <directive_groups> 'TKPROGRAM_LINE_PART'
        RULE_DIRECTIVE_GROUPS                         =  2, // <directive_groups> ::= <_directive_groups>
        RULE_DIRECTIVE_GROUPS2                        =  3, // <directive_groups> ::= 
        RULE__DIRECTIVE_GROUPS                        =  4, // <_directive_groups> ::= <directive_group>
        RULE__DIRECTIVE_GROUPS2                       =  5, // <_directive_groups> ::= <directive_groups> <directive_group>
        RULE_DIRECTIVE_GROUP                          =  6, // <directive_group> ::= <control_line>
        RULE_DIRECTIVE_GROUP2                         =  7, // <directive_group> ::= <conditional_group>
        RULE_DIRECTIVE_GROUP_TKPROGRAM_LINE           =  8, // <directive_group> ::= 'TKPROGRAM_LINE'
        RULE_CONDITIONAL_GROUP                        =  9, // <conditional_group> ::= <if_part> <directive_groups> <endif_part>
        RULE_CONDITIONAL_GROUP2                       = 10, // <conditional_group> ::= <if_part> <elif_part> <directive_groups> <endif_part>
        RULE_CONDITIONAL_GROUP3                       = 11, // <conditional_group> ::= <if_part> <else_part> <directive_groups> <endif_part>
        RULE_IF_PART                                  = 12, // <if_part> ::= <if_line>
        RULE_IF_PART2                                 = 13, // <if_part> ::= <ifdef_line>
        RULE_IF_PART3                                 = 14, // <if_part> ::= <ifndef_line>
        RULE_ELSE_PART                                = 15, // <else_part> ::= <directive_groups> <else_line>
        RULE_ELSE_PART2                               = 16, // <else_part> ::= <elif_part> <directive_groups> <else_line>
        RULE_ELIF_PART                                = 17, // <elif_part> ::= <directive_groups> <elif_line>
        RULE_ELIF_PART2                               = 18, // <elif_part> ::= <elif_part> <directive_groups> <elif_line>
        RULE_ENDIF_PART                               = 19, // <endif_part> ::= <endif_line>
        RULE_IF_LINE_TKPNDIF_TKPROGRAM_LINE           = 20, // <if_line> ::= 'TKPNDIF' 'TKPROGRAM_LINE'
        RULE_IFDEF_LINE_TKPNDIFDEF_TKPROGRAM_LINE     = 21, // <ifdef_line> ::= 'TKPNDIFDEF' 'TKPROGRAM_LINE'
        RULE_IFNDEF_LINE_TKPNDIFNDEF_TKPROGRAM_LINE   = 22, // <ifndef_line> ::= 'TKPNDIFNDEF' 'TKPROGRAM_LINE'
        RULE_ELIF_LINE_TKPNDELIF_TKPROGRAM_LINE       = 23, // <elif_line> ::= 'TKPNDELIF' 'TKPROGRAM_LINE'
        RULE_ELSE_LINE_TKPNDELSE                      = 24, // <else_line> ::= 'TKPNDELSE'
        RULE_ENDIF_LINE_TKPNDENDIF                    = 25, // <endif_line> ::= 'TKPNDENDIF'
        RULE_CONTROL_LINE                             = 26, // <control_line> ::= <unknow_line>
        RULE_UNKNOW_LINE_TKPNDUNKNOW_TKPROGRAM_LINE   = 27  // <unknow_line> ::= 'TKPNDUNKNOW' 'TKPROGRAM_LINE'
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

		case SymbolConstants.SYMBOL_TKPNDELIF :
		//'TKPNDELIF'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKPNDELSE :
		//'TKPNDELSE'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKPNDENDIF :
		//'TKPNDENDIF'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKPNDIF :
		//'TKPNDIF'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKPNDIFDEF :
		//'TKPNDIFDEF'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKPNDIFNDEF :
		//'TKPNDIFNDEF'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKPNDUNKNOW :
		//'TKPNDUNKNOW'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKPROGRAM_LINE :
		//'TKPROGRAM_LINE'

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

		case SymbolConstants.SYMBOL_CONDITIONAL_GROUP :
		//<conditional_group>
		//TERMINAL:conditional_group
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_CONTROL_LINE :
		//<control_line>
		//TERMINAL:control_line
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

		case SymbolConstants.SYMBOL_ELIF_LINE :
		//<elif_line>
		//TERMINAL:elif_line
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_ELIF_PART :
		//<elif_part>
		//TERMINAL:elif_part
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_ELSE_LINE :
		//<else_line>
		//TERMINAL:else_line
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_ELSE_PART :
		//<else_part>
		//TERMINAL:else_part
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_ENDIF_LINE :
		//<endif_line>
		//TERMINAL:endif_line
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_ENDIF_PART :
		//<endif_part>
		//TERMINAL:endif_part
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_IF_LINE :
		//<if_line>
		//TERMINAL:if_line
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_IF_PART :
		//<if_part>
		//TERMINAL:if_part
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_IFDEF_LINE :
		//<ifdef_line>
		//TERMINAL:ifdef_line
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_IFNDEF_LINE :
		//<ifndef_line>
		//TERMINAL:ifndef_line
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_PREPROCESSOR_PROGRAM :
		//<preprocessor_program>
		//TERMINAL:preprocessor_program
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_UNKNOW_LINE :
		//<unknow_line>
		//TERMINAL:unknow_line
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
		//<preprocessor_program> ::= <directive_groups>
         
		{
			compilation_unit _compilation_unit=new compilation_unit();
			 
				_compilation_unit.Language=LanguageId.C;
                                if(LRParser.GetReductionSyntaxNode(0)!=null){
                                	parsertools.create_source_context(_compilation_unit,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
					CompilerDirectives.AddRange(((compiler_directive_list)LRParser.GetReductionSyntaxNode(0)).directives);
				}
				
			return _compilation_unit;
		}


		case RuleConstants.RULE_PREPROCESSOR_PROGRAM_TKPROGRAM_LINE_PART :
		//<preprocessor_program> ::= <directive_groups> 'TKPROGRAM_LINE_PART'
         
		{
			compilation_unit _compilation_unit=new compilation_unit();
			 
				_compilation_unit.Language=LanguageId.C;
                                parsertools.create_source_context(_compilation_unit,parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1)),LRParser.GetReductionSyntaxNode(1));
				if(LRParser.GetReductionSyntaxNode(0)!=null)
					CompilerDirectives.AddRange(((compiler_directive_list)LRParser.GetReductionSyntaxNode(0)).directives);
				
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
		//<_directive_groups> ::= <directive_group>
         
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


		case RuleConstants.RULE_DIRECTIVE_GROUP :
		//<directive_group> ::= <control_line>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_DIRECTIVE_GROUP2 :
		//<directive_group> ::= <conditional_group>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_DIRECTIVE_GROUP_TKPROGRAM_LINE :
		//<directive_group> ::= 'TKPROGRAM_LINE'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_CONDITIONAL_GROUP :
		//<conditional_group> ::= <if_part> <directive_groups> <endif_part>
         
		{
			compiler_directive_if _compiler_directive_if=(compiler_directive_if)LRParser.GetReductionSyntaxNode(0);
								_compiler_directive_if.if_part=(compiler_directive_list)LRParser.GetReductionSyntaxNode(1);
								parsertools.create_source_context(_compiler_directive_if,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _compiler_directive_if;
		}


		case RuleConstants.RULE_CONDITIONAL_GROUP2 :
		//<conditional_group> ::= <if_part> <elif_part> <directive_groups> <endif_part>
         
		{
			compiler_directive_if _compiler_directive_if=(compiler_directive_if)LRParser.GetReductionSyntaxNode(0);
								_compiler_directive_if.if_part=if_part.Pop();
								(LRParser.GetReductionSyntaxNode(1) as compiler_directive_if).if_part=(compiler_directive)LRParser.GetReductionSyntaxNode(2);
								_compiler_directive_if.elseif_part=first_if.Pop();
								parsertools.create_source_context(_compiler_directive_if,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
			return _compiler_directive_if;
		}


		case RuleConstants.RULE_CONDITIONAL_GROUP3 :
		//<conditional_group> ::= <if_part> <else_part> <directive_groups> <endif_part>
         
		{
			compiler_directive_if _compiler_directive_if=(compiler_directive_if)LRParser.GetReductionSyntaxNode(0);
								_compiler_directive_if.if_part=if_part.Pop();
								if(LRParser.GetReductionSyntaxNode(1) is compiler_directive_if)
								{
									(LRParser.GetReductionSyntaxNode(1) as compiler_directive_if).elseif_part=(compiler_directive)LRParser.GetReductionSyntaxNode(2);
									_compiler_directive_if.elseif_part=first_if.Pop();
								}else{
									first_if.Pop();
									_compiler_directive_if.elseif_part=(compiler_directive)LRParser.GetReductionSyntaxNode(2);
								}
								parsertools.create_source_context(_compiler_directive_if,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
			return _compiler_directive_if;
		}


		case RuleConstants.RULE_IF_PART :
		//<if_part> ::= <if_line>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_IF_PART2 :
		//<if_part> ::= <ifdef_line>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_IF_PART3 :
		//<if_part> ::= <ifndef_line>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_ELSE_PART :
		//<else_part> ::= <directive_groups> <else_line>
if_part.Push((compiler_directive)LRParser.GetReductionSyntaxNode(0)); 
								first_if.Push(null); return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_ELSE_PART2 :
		//<else_part> ::= <elif_part> <directive_groups> <else_line>
         
		{
			compiler_directive_if _compiler_directive_if=(compiler_directive_if)LRParser.GetReductionSyntaxNode(0);
								_compiler_directive_if.if_part=(compiler_directive)LRParser.GetReductionSyntaxNode(1);
								parsertools.create_source_context(_compiler_directive_if,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _compiler_directive_if;
		}


		case RuleConstants.RULE_ELIF_PART :
		//<elif_part> ::= <directive_groups> <elif_line>
if_part.Push((compiler_directive)LRParser.GetReductionSyntaxNode(0)); 
								first_if.Push((compiler_directive)LRParser.GetReductionSyntaxNode(1)); return LRParser.GetReductionSyntaxNode(1);

		case RuleConstants.RULE_ELIF_PART2 :
		//<elif_part> ::= <elif_part> <directive_groups> <elif_line>
         
		{
			compiler_directive_if _compiler_directive_if=(compiler_directive_if)LRParser.GetReductionSyntaxNode(0);
								_compiler_directive_if.if_part=(compiler_directive)LRParser.GetReductionSyntaxNode(1);
								_compiler_directive_if.elseif_part=(compiler_directive)LRParser.GetReductionSyntaxNode(2);
								parsertools.create_source_context(_compiler_directive_if,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
								_compiler_directive_if=(compiler_directive_if)LRParser.GetReductionSyntaxNode(2);
			return _compiler_directive_if;
		}


		case RuleConstants.RULE_ENDIF_PART :
		//<endif_part> ::= <endif_line>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_IF_LINE_TKPNDIF_TKPROGRAM_LINE :
		//<if_line> ::= 'TKPNDIF' 'TKPROGRAM_LINE'
         
		{
			compiler_directive_if _compiler_directive_if=new compiler_directive_if();
			
					_compiler_directive_if.Name=(token_info)LRParser.GetReductionSyntaxNode(0);
					_compiler_directive_if.Directive=(token_info)LRParser.GetReductionSyntaxNode(1);
					parsertools.create_source_context(_compiler_directive_if,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1)); 
			return _compiler_directive_if;
		}


		case RuleConstants.RULE_IFDEF_LINE_TKPNDIFDEF_TKPROGRAM_LINE :
		//<ifdef_line> ::= 'TKPNDIFDEF' 'TKPROGRAM_LINE'
         
		{
			compiler_directive_if _compiler_directive_if=new compiler_directive_if();
			
					_compiler_directive_if.Name=(token_info)LRParser.GetReductionSyntaxNode(0);
					_compiler_directive_if.Directive=(token_info)LRParser.GetReductionSyntaxNode(1);
					parsertools.create_source_context(_compiler_directive_if,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1)); 
			return _compiler_directive_if;
		}


		case RuleConstants.RULE_IFNDEF_LINE_TKPNDIFNDEF_TKPROGRAM_LINE :
		//<ifndef_line> ::= 'TKPNDIFNDEF' 'TKPROGRAM_LINE'
         
		{
			compiler_directive_if _compiler_directive_if=new compiler_directive_if();
			
					_compiler_directive_if.Name=(token_info)LRParser.GetReductionSyntaxNode(0);
					_compiler_directive_if.Directive=(token_info)LRParser.GetReductionSyntaxNode(1);
					parsertools.create_source_context(_compiler_directive_if,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1)); 
			return _compiler_directive_if;
		}


		case RuleConstants.RULE_ELIF_LINE_TKPNDELIF_TKPROGRAM_LINE :
		//<elif_line> ::= 'TKPNDELIF' 'TKPROGRAM_LINE'
         
		{
			compiler_directive_if _compiler_directive_if=new compiler_directive_if();
			
					_compiler_directive_if.Name=(token_info)LRParser.GetReductionSyntaxNode(0);
					_compiler_directive_if.Directive=(token_info)LRParser.GetReductionSyntaxNode(1);
					parsertools.create_source_context(_compiler_directive_if,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1)); 
			return _compiler_directive_if;
		}


		case RuleConstants.RULE_ELSE_LINE_TKPNDELSE :
		//<else_line> ::= 'TKPNDELSE'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_ENDIF_LINE_TKPNDENDIF :
		//<endif_line> ::= 'TKPNDENDIF'
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_CONTROL_LINE :
		//<control_line> ::= <unknow_line>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_UNKNOW_LINE_TKPNDUNKNOW_TKPROGRAM_LINE :
		//<unknow_line> ::= 'TKPNDUNKNOW' 'TKPROGRAM_LINE'
         
		{
			compiler_directive _compiler_directive=new compiler_directive((token_info)LRParser.GetReductionSyntaxNode(0),(token_info)LRParser.GetReductionSyntaxNode(1));
			
						parsertools.create_source_context(_compiler_directive,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1)); 
			return _compiler_directive;
		}


            }
            throw new RuleException("Unknown rule");
        }
	}
}
