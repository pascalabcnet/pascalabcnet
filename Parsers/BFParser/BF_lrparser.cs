
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Collections;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;
using PascalABCCompiler.ParserTools;
using GoldParser;

namespace  PascalABCCompiler.BFParser
{
    
    public class GPBParser_BF:GPBParser
    {
        private Stack NodesStack;
        private int operation_value=0;
        private object left_node,right_node;

        public GPBParser_BF(Stream stream)
            :base(stream)
        {
            parsertools = new BF_parsertools();
        }

        public object Parse(string source)
        {
            NodesStack = new Stack();
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
			prev_node = LRParser.TokenSyntaxNode;
                        Error er = new UnexpectedToken(this,parsertools.symbol_collection_to_string(this.LRParser.GetExpectedTokens()));
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
                        errors.Add(new CompilerInternalError("PascalABCParser", new Exception("ParseMessage.InternalError")));
                        return null;

                    case ParseMessage.NotLoadedError:
                        errors.Add(new CompilerInternalError("PascalABCParser", new Exception("ParseMessage.NotLoadedError")));
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
        SYMBOL_TKDEC                =  3, // 'tkDec'
        SYMBOL_TKDECVALUE           =  4, // 'tkDecValue'
        SYMBOL_TKINC                =  5, // 'tkInc'
        SYMBOL_TKINCVALUE           =  6, // 'tkIncValue'
        SYMBOL_TKJMPPAST            =  7, // 'tkJMPPast'
        SYMBOL_TKJMPRET             =  8, // 'tkJMPRet'
        SYMBOL_TKREAD               =  9, // 'tkRead'
        SYMBOL_TKWRITE              = 10, // 'tkWrite'
        SYMBOL_BF                   = 11, // <BF>
        SYMBOL_BF_EMPTY_INSTRUCTION = 12, // <bf_empty_instruction>
        SYMBOL_BF_INSTRUCTION       = 13, // <bf_instruction>
        SYMBOL_BF_INSTRUCTIONS      = 14, // <bf_instructions>
        SYMBOL_BF_INSTRUCTIONS_LIST = 15, // <bf_instructions_list>
        SYMBOL_CHANGE_POS           = 16, // <change_pos>
        SYMBOL_CHANGE_POS_LIST      = 17, // <change_pos_list>
        SYMBOL_CHANGE_VALUE         = 18, // <change_value>
        SYMBOL_CHANGE_VALUE_LIST    = 19  // <change_value_list>
    };

    public enum RuleConstants : int
    {
        RULE_BF                                =  0, // <BF> ::= <bf_instructions>
        RULE_BF_EMPTY_INSTRUCTION              =  1, // <bf_empty_instruction> ::= 
        RULE_BF_INSTRUCTIONS                   =  2, // <bf_instructions> ::= <bf_instructions_list>
        RULE_BF_INSTRUCTIONS2                  =  3, // <bf_instructions> ::= <bf_empty_instruction>
        RULE_BF_INSTRUCTIONS_LIST              =  4, // <bf_instructions_list> ::= <bf_instruction>
        RULE_BF_INSTRUCTIONS_LIST2             =  5, // <bf_instructions_list> ::= <bf_instructions_list> <bf_instruction>
        RULE_CHANGE_POS_LIST                   =  6, // <change_pos_list> ::= <change_pos>
        RULE_CHANGE_POS_LIST2                  =  7, // <change_pos_list> ::= <change_pos_list> <change_pos>
        RULE_CHANGE_VALUE_LIST                 =  8, // <change_value_list> ::= <change_value>
        RULE_CHANGE_VALUE_LIST2                =  9, // <change_value_list> ::= <change_value_list> <change_value>
        RULE_CHANGE_POS_TKINC                  = 10, // <change_pos> ::= 'tkInc'
        RULE_CHANGE_POS_TKDEC                  = 11, // <change_pos> ::= 'tkDec'
        RULE_CHANGE_VALUE_TKINCVALUE           = 12, // <change_value> ::= 'tkIncValue'
        RULE_CHANGE_VALUE_TKDECVALUE           = 13, // <change_value> ::= 'tkDecValue'
        RULE_BF_INSTRUCTION_TKJMPPAST_TKJMPRET = 14, // <bf_instruction> ::= 'tkJMPPast' <bf_instructions> 'tkJMPRet'
        RULE_BF_INSTRUCTION                    = 15, // <bf_instruction> ::= <change_pos_list>
        RULE_BF_INSTRUCTION2                   = 16, // <bf_instruction> ::= <change_value_list>
        RULE_BF_INSTRUCTION_TKWRITE            = 17, // <bf_instruction> ::= 'tkWrite'
        RULE_BF_INSTRUCTION_TKREAD             = 18  // <bf_instruction> ::= 'tkRead'
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

		case SymbolConstants.SYMBOL_TKDEC :
		//'tkDec'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKDECVALUE :
		//'tkDecValue'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKINC :
		//'tkInc'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKINCVALUE :
		//'tkIncValue'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKJMPPAST :
		//'tkJMPPast'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKJMPRET :
		//'tkJMPRet'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKREAD :
		//'tkRead'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_TKWRITE :
		//'tkWrite'

		{
			token_info _token_info=new token_info(LRParser.TokenText);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}


		case SymbolConstants.SYMBOL_BF :
		//<BF>
		//TERMINAL:BF
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_BF_EMPTY_INSTRUCTION :
		//<bf_empty_instruction>
		//TERMINAL:bf_empty_instruction
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_BF_INSTRUCTION :
		//<bf_instruction>
		//TERMINAL:bf_instruction
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_BF_INSTRUCTIONS :
		//<bf_instructions>
		//TERMINAL:bf_instructions
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_BF_INSTRUCTIONS_LIST :
		//<bf_instructions_list>
		//TERMINAL:bf_instructions_list
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_CHANGE_POS :
		//<change_pos>
		//TERMINAL:change_pos
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_CHANGE_POS_LIST :
		//<change_pos_list>
		//TERMINAL:change_pos_list
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_CHANGE_VALUE :
		//<change_value>
		//TERMINAL:change_value
		return null;
		//ENDTERMINAL

		case SymbolConstants.SYMBOL_CHANGE_VALUE_LIST :
		//<change_value_list>
		//TERMINAL:change_value_list
		return null;
		//ENDTERMINAL

            }
            throw new SymbolException("Unknown symbol");
        }

        public Object CreateNonTerminalObject()
        {
            switch ((RuleConstants) LRParser.ReductionRule.Index)
            {
		case RuleConstants.RULE_BF :
		//<BF> ::= <bf_instructions>
         
		{
			program_module _program_module=new program_module();
			
							  _program_module.Language = LanguageId.PascalABCNET;
							  ident BFSystem=new ident("BFSystem");
							  unit_or_namespace unit=new unit_or_namespace();
							  unit.name = new ident_list();
							  unit.name.idents.Add(BFSystem);
							  uses_list used_units=new uses_list();
							  used_units.units.Add(unit);
							  block program_block=new block(null,(statement_list)LRParser.GetReductionSyntaxNode(0));
							  _program_module.used_units=used_units;
							  _program_module.program_block=program_block;
							  used_units.source_context=unit.source_context=BFSystem.source_context=new SourceContext(1,1,1,1,1,1);
                                                          parsertools.create_source_context(program_block,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
                                                          parsertools.create_source_context(_program_module,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
							  operation_value=0;
							
			return _program_module;
		}


		case RuleConstants.RULE_BF_EMPTY_INSTRUCTION :
		//<bf_empty_instruction> ::= 
		//NONTERMINAL:<bf_empty_instruction> ::= 
		return null;
		//ENDNONTERMINAL

		case RuleConstants.RULE_BF_INSTRUCTIONS :
		//<bf_instructions> ::= <bf_instructions_list>
return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_BF_INSTRUCTIONS2 :
		//<bf_instructions> ::= <bf_empty_instruction>
         
		{
			statement_list _statement_list=new statement_list();
			 _statement_list.subnodes.Add(new empty_statement());
			return _statement_list;
		}


		case RuleConstants.RULE_BF_INSTRUCTIONS_LIST :
		//<bf_instructions_list> ::= <bf_instruction>
         
		//TemplateList for statement_list (create) 
		{
			statement_list _statement_list=new statement_list();
			_statement_list.source_context=((statement)LRParser.GetReductionSyntaxNode(0)).source_context;
			_statement_list.subnodes.Add((statement)LRParser.GetReductionSyntaxNode(0));
			return _statement_list;
		}


		case RuleConstants.RULE_BF_INSTRUCTIONS_LIST2 :
		//<bf_instructions_list> ::= <bf_instructions_list> <bf_instruction>

		//TemplateList for statement_list (add)         
		{
			statement_list _statement_list=(statement_list)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_statement_list,_statement_list,LRParser.GetReductionSyntaxNode(1));
			_statement_list.subnodes.Add((statement)LRParser.GetReductionSyntaxNode(1));
			return _statement_list;
		}


		case RuleConstants.RULE_CHANGE_POS_LIST :
		//<change_pos_list> ::= <change_pos>
 left_node=right_node=LRParser.GetReductionSyntaxNode(0); return null;

		case RuleConstants.RULE_CHANGE_POS_LIST2 :
		//<change_pos_list> ::= <change_pos_list> <change_pos>
 right_node=LRParser.GetReductionSyntaxNode(1); return null;

		case RuleConstants.RULE_CHANGE_VALUE_LIST :
		//<change_value_list> ::= <change_value>
 left_node=right_node=LRParser.GetReductionSyntaxNode(0); return null;

		case RuleConstants.RULE_CHANGE_VALUE_LIST2 :
		//<change_value_list> ::= <change_value_list> <change_value>
 right_node=LRParser.GetReductionSyntaxNode(1); return null;

		case RuleConstants.RULE_CHANGE_POS_TKINC :
		//<change_pos> ::= 'tkInc'
 operation_value++; return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_CHANGE_POS_TKDEC :
		//<change_pos> ::= 'tkDec'
 operation_value--; return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_CHANGE_VALUE_TKINCVALUE :
		//<change_value> ::= 'tkIncValue'
 operation_value++; return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_CHANGE_VALUE_TKDECVALUE :
		//<change_value> ::= 'tkDecValue'
 operation_value--; return LRParser.GetReductionSyntaxNode(0);

		case RuleConstants.RULE_BF_INSTRUCTION_TKJMPPAST_TKJMPRET :
		//<bf_instruction> ::= 'tkJMPPast' <bf_instructions> 'tkJMPRet'
         
		{       
			indexer indx=new indexer(new expression_list());
			indx.indexes.expressions.Add(new ident("BFCaretPos"));
			indx.dereferencing_value=new ident("BFField");
			bin_expr be=new bin_expr(indx,new int32_const(0),Operators.NotEqual);
			while_node wn=new while_node();
                        wn.expr=be;
			wn.statements=(statement_list)LRParser.GetReductionSyntaxNode(1);
                        parsertools.create_source_context(wn,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
                        be.right.source_context=indx.dereferencing_value.source_context=indx.source_context=indx.indexes.expressions[0].source_context=be.source_context=wn.source_context;
                        return wn;
		}


		case RuleConstants.RULE_BF_INSTRUCTION :
		//<bf_instruction> ::= <change_pos_list>
         
		{
			if (operation_value!=0)
			{
			ident caret=new ident("BFCaretPos");
			int32_const caret_n=new int32_const(operation_value);
			operation_value=0;
			bin_expr expr=new bin_expr(caret,caret_n,Operators.Plus);
			assign _assign=new assign(caret,expr,Operators.Assignment);
			parsertools.create_source_context(caret,left_node,right_node);
			caret_n.source_context=expr.source_context=_assign.source_context=caret.source_context;
			return _assign;
			}
			syntax_tree_node tn=new empty_statement();
			parsertools.create_source_context(tn,left_node,right_node);
			return tn;
		}


		case RuleConstants.RULE_BF_INSTRUCTION2 :
		//<bf_instruction> ::= <change_value_list>
         
		{
			if (operation_value!=0)
			{
				indexer indx=new indexer(new expression_list());
				indx.indexes.expressions.Add(new ident("BFCaretPos"));
				indx.dereferencing_value=new ident("BFField");
				bin_expr be=new bin_expr(indx,new int32_const(operation_value),Operators.Plus);
				assign assg=new assign(indx,be,Operators.Assignment);
				operation_value=0;				
				parsertools.create_source_context(assg,left_node,right_node);
                                indx.dereferencing_value.source_context=indx.source_context=indx.indexes.expressions[0].source_context=be.right.source_context=be.source_context=assg.source_context;
				return assg;
			}	
			syntax_tree_node tn=new empty_statement();
			parsertools.create_source_context(tn,left_node,right_node);
			return tn;	
		}


		case RuleConstants.RULE_BF_INSTRUCTION_TKWRITE :
		//<bf_instruction> ::= 'tkWrite'
         
		{
			ident _ident=new ident("WriteCaretValue");
			procedure_call _procedure_call=new procedure_call(_ident);
			parsertools.create_source_context(_ident,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			parsertools.create_source_context(_procedure_call,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
                        return _procedure_call;
		}


		case RuleConstants.RULE_BF_INSTRUCTION_TKREAD :
		//<bf_instruction> ::= 'tkRead'
         
		{
			ident _ident=new ident("ReadCaretValue");
			procedure_call _procedure_call=new procedure_call(_ident);
			parsertools.create_source_context(_ident,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			parsertools.create_source_context(_procedure_call,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
                        return _procedure_call;
		}


            }
            throw new RuleException("Unknown rule");
        }
	}
}
