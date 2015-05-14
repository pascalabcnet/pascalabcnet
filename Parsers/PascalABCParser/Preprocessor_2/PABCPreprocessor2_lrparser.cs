
///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////
//������ ���� ��������� �������, �� ��������� �������������!!!
///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Collections;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;
using PascalABCCompiler.PascalABCParser.Errors;
using PascalABCCompiler.Preprocessor_2.Errors;
using PascalABCCompiler.ParserTools;
using GoldParser;
using PascalABCCompiler.PascalABCParser;

namespace  PascalABCCompiler.Preprocessor2
{
public class GPB_PABCPreprocessor2 : GPBParser
{


public PascalABCCompiler.Preprocessor2.Preprocessor2 prepr;

private pascalabc_parsertools pascal_parsertools;

public GPB_PABCPreprocessor2(Stream stream, PascalABCCompiler.Preprocessor2.Preprocessor2 prepr1)
            : base(stream)
        {
            pascal_parsertools = new pascalabc_parsertools();
            parsertools = pascal_parsertools;

            prepr = prepr1;
        }

        public GPB_PABCPreprocessor2(Stream stream, GoldParser.Grammar grammar, PascalABCCompiler.Preprocessor2.Preprocessor2 prepr1)
            : base(grammar)
        {
            pascal_parsertools = new pascalabc_parsertools();
            parsertools = pascal_parsertools;

            prepr = prepr1;
        }

        public object Parse(string source)
        {
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
                            if ((LRParser.TokenSyntaxNode as syntax_tree_node) != null)
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
                            errors.Add(new CompilerInternalError("PABCPreprocessor2", new Exception("ParseMessage.InternalError")));
                            return null;

                        case ParseMessage.NotLoadedError:
                            errors.Add(new CompilerInternalError("PABCPreprocessor2", new Exception("ParseMessage.NotLoadedError")));
                            return null;

                        case ParseMessage.CommentError:
                            errors.Add(new UnexpectedToken(this, "(EOF)"));
                            return null;

                        case ParseMessage.CommentBlockRead:
                            {
                                SourceContext sc1 = parsertools.GetTokenSourceContext(this.LRParser);
                                string comment = this.LRParser.CommentText;
                                SourceContext sc2 = new SourceContext(sc1, parsertools.GetTokenSourceContext(this.LRParser));
                                if (comment[0] == '{' && comment[1] == '$')
                                    prepr.Errors.Add(new SyntaxErrorInDirective(prepr.CurrentFileName, sc2, comment));
                                //if (prepr.sm.AllowWrite())
                                //prepr.WriteToThread(sc2, comment.Replace(Environment.NewLine, " "));
                                //prepr.WriteToStream(sc2, comment.Replace("\r\n", " "));                                    
                            }
                            break;

                        case ParseMessage.CommentLineRead:
                            {
                                SourceContext sc1 = parsertools.GetTokenSourceContext(this.LRParser);
                                string comment = this.LRParser.CommentText;
                                SourceContext sc2 = new SourceContext(sc1, sc1);
                                if (prepr.sm.AllowWrite())
                                    prepr.WriteToStream(sc1, comment.Replace("\r\n", " "));
                                //prepr.WriteToStream(sc1, comment);                                 
                            }
                            break;
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

///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////
//SymbolConstants
///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////

public enum SymbolConstants : int
{
	SYMBOL_EOF                  =  0, // (EOF)
	SYMBOL_ERROR                =  1, // (Error)
	SYMBOL_WHITESPACE           =  2, // (Whitespace)
	SYMBOL_COMMENTEND           =  3, // (Comment End)
	SYMBOL_COMMENTLINE          =  4, // (Comment Line)
	SYMBOL_COMMENTSTART         =  5, // (Comment Start)
	SYMBOL_TK_AP                =  6, // 'TK_AP'
	SYMBOL_TK_DIRECTIVE         =  7, // 'TK_DIRECTIVE'
	SYMBOL_TK_KAV               =  8, // 'TK_KAV'
	SYMBOL_TK_NEWLINE           =  9, // 'TK_NEWLINE'
	SYMBOL_TK_PROGRAM_LINE      = 10, // 'TK_PROGRAM_LINE'
	SYMBOL_TK_SLASH             = 11, // 'TK_SLASH'
	SYMBOL_TK_SLASHEKV          = 12, // 'TK_SLASHEKV'
	SYMBOL__PREPROCESSOR_LEXEMS = 13, // <_preprocessor_lexems>
	SYMBOL_EMPTY                = 14, // <empty>
	SYMBOL_PREPROCESSOR_LEXEM   = 15, // <preprocessor_lexem>
	SYMBOL_PREPROCESSOR_LEXEMS  = 16, // <preprocessor_lexems>
	SYMBOL_PREPROCESSOR_PROGRAM = 17  // <preprocessor_program>
};














///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////
//RuleConstants
///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////

public enum RuleConstants : int
{
	RULE_PREPROCESSOR_PROGRAM               =  0, // <preprocessor_program> ::= <preprocessor_lexems> <empty>
	RULE_PREPROCESSOR_LEXEMS                =  1, // <preprocessor_lexems> ::= <_preprocessor_lexems>
	RULE_PREPROCESSOR_LEXEMS2               =  2, // <preprocessor_lexems> ::= 
	RULE__PREPROCESSOR_LEXEMS               =  3, // <_preprocessor_lexems> ::= <preprocessor_lexem> <empty>
	RULE__PREPROCESSOR_LEXEMS2              =  4, // <_preprocessor_lexems> ::= <preprocessor_lexems> <preprocessor_lexem>
	RULE_PREPROCESSOR_LEXEM_TK_NEWLINE      =  5, // <preprocessor_lexem> ::= 'TK_NEWLINE'
	RULE_PREPROCESSOR_LEXEM_TK_SLASH        =  6, // <preprocessor_lexem> ::= 'TK_SLASH'
	RULE_PREPROCESSOR_LEXEM_TK_SLASHEKV     =  7, // <preprocessor_lexem> ::= 'TK_SLASHEKV'
	RULE_PREPROCESSOR_LEXEM_TK_AP           =  8, // <preprocessor_lexem> ::= 'TK_AP'
	RULE_PREPROCESSOR_LEXEM_TK_KAV          =  9, // <preprocessor_lexem> ::= 'TK_KAV'
	RULE_PREPROCESSOR_LEXEM_TK_PROGRAM_LINE = 10, // <preprocessor_lexem> ::= 'TK_PROGRAM_LINE'
	RULE_PREPROCESSOR_LEXEM_TK_DIRECTIVE    = 11, // <preprocessor_lexem> ::= 'TK_DIRECTIVE' <empty>
	RULE_EMPTY                              = 12  // <empty> ::= 
};













///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////
//CreateTerminalObject
///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////

private Object CreateTerminalObject()
{
switch ((SymbolConstants)LRParser.TokenSymbol.Index)
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
	case SymbolConstants.SYMBOL_TK_AP :
    	//'TK_AP'
			{
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }
	case SymbolConstants.SYMBOL_TK_DIRECTIVE :
    	//'TK_DIRECTIVE'
			{
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;    
                    }
	case SymbolConstants.SYMBOL_TK_KAV :
    	//'TK_KAV'
			{
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }
	case SymbolConstants.SYMBOL_TK_NEWLINE :
    	//'TK_NEWLINE'
 /*Console.Error.WriteLine("\n\rwarning: TerminalToken 'TK_NEWLINE' return NULL! \n\r");*/return null;
	case SymbolConstants.SYMBOL_TK_PROGRAM_LINE :
    	//'TK_PROGRAM_LINE'
			{
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }
	case SymbolConstants.SYMBOL_TK_SLASH :
    	//'TK_SLASH'
			{
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }
	case SymbolConstants.SYMBOL_TK_SLASHEKV :
    	//'TK_SLASHEKV'
			{
                        token_info _token_info = new token_info(LRParser.TokenText);
                        _token_info.source_context = parsertools.GetTokenSourceContext();

                        return _token_info;
                    }
	case SymbolConstants.SYMBOL__PREPROCESSOR_LEXEMS :
    	//<_preprocessor_lexems>
	//TERMINAL:_preprocessor_lexems
	return null;
	//ENDTERMINAL
	case SymbolConstants.SYMBOL_EMPTY :
    	//<empty>
	//TERMINAL:empty
	return null;
	//ENDTERMINAL
	case SymbolConstants.SYMBOL_PREPROCESSOR_LEXEM :
    	//<preprocessor_lexem>
	//TERMINAL:preprocessor_lexem
	return null;
	//ENDTERMINAL
	case SymbolConstants.SYMBOL_PREPROCESSOR_LEXEMS :
    	//<preprocessor_lexems>
	//TERMINAL:preprocessor_lexems
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















///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////
//CreateNonTerminalObject
///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////

public Object CreateNonTerminalObject()
{
switch ((RuleConstants)LRParser.ReductionRule.Index)
{
	case RuleConstants.RULE_PREPROCESSOR_PROGRAM :
	//<preprocessor_program> ::= <preprocessor_lexems> <empty>
			{
                        compilation_unit _compilation_unit = new compilation_unit();

                        _compilation_unit.Language = LanguageId.PascalABCNET;
                        if (LRParser.GetReductionSyntaxNode(0) != null)
                        {
                            parsertools.create_source_context(_compilation_unit, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0));
                            CompilerDirectives.AddRange(((compiler_directive_list)LRParser.GetReductionSyntaxNode(0)).directives);
                            if (CompilerDirectives.Count != 0)
                                prepr.CompilerDirectives.AddRange(CompilerDirectives);
                        }

                        return _compilation_unit;
                    }
	case RuleConstants.RULE_PREPROCESSOR_LEXEMS :
	//<preprocessor_lexems> ::= <_preprocessor_lexems>
			return LRParser.GetReductionSyntaxNode(0);
	case RuleConstants.RULE_PREPROCESSOR_LEXEMS2 :
	//<preprocessor_lexems> ::= 
	//NONTERMINAL:<preprocessor_lexems> ::= 
	return null;
	//ENDNONTERMINAL
	case RuleConstants.RULE__PREPROCESSOR_LEXEMS :
	//<_preprocessor_lexems> ::= <preprocessor_lexem> <empty>
			{
                        compiler_directive_list _compiler_directive_list = new compiler_directive_list();

                        if (LRParser.GetReductionSyntaxNode(0) is compiler_directive)
                            _compiler_directive_list.directives.Add((compiler_directive)LRParser.GetReductionSyntaxNode(0));
                        parsertools.create_source_context(_compiler_directive_list, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(0));
                        return _compiler_directive_list;
                    }
	case RuleConstants.RULE__PREPROCESSOR_LEXEMS2 :
	//<_preprocessor_lexems> ::= <preprocessor_lexems> <preprocessor_lexem>
			{
                        compiler_directive_list _compiler_directive_list = (compiler_directive_list)LRParser.GetReductionSyntaxNode(0);
                        if (LRParser.GetReductionSyntaxNode(1) is compiler_directive)
                            _compiler_directive_list.directives.Add((compiler_directive)LRParser.GetReductionSyntaxNode(1));
                        parsertools.create_source_context(_compiler_directive_list, LRParser.GetReductionSyntaxNode(0), LRParser.GetReductionSyntaxNode(1));
                        return _compiler_directive_list;
                    }
	case RuleConstants.RULE_PREPROCESSOR_LEXEM_TK_NEWLINE :
	//<preprocessor_lexem> ::= 'TK_NEWLINE'
			return LRParser.GetReductionSyntaxNode(0);
	case RuleConstants.RULE_PREPROCESSOR_LEXEM_TK_SLASH :
	//<preprocessor_lexem> ::= 'TK_SLASH'
			{
                        if (prepr.sm.AllowWrite())
                            prepr.WriteToStream(((token_info)LRParser.GetReductionSyntaxNode(0)).source_context, ((token_info)LRParser.GetReductionSyntaxNode(0)).text);
                        return LRParser.GetReductionSyntaxNode(0);
                    }
	case RuleConstants.RULE_PREPROCESSOR_LEXEM_TK_SLASHEKV :
	//<preprocessor_lexem> ::= 'TK_SLASHEKV'
			{
                        if (prepr.sm.AllowWrite())
                            prepr.WriteToStream(((token_info)LRParser.GetReductionSyntaxNode(0)).source_context, ((token_info)LRParser.GetReductionSyntaxNode(0)).text);
                        return LRParser.GetReductionSyntaxNode(0);
                    }
	case RuleConstants.RULE_PREPROCESSOR_LEXEM_TK_AP :
	//<preprocessor_lexem> ::= 'TK_AP'
			{
                        if (prepr.sm.AllowWrite())
                            prepr.WriteToStream(((token_info)LRParser.GetReductionSyntaxNode(0)).source_context, ((token_info)LRParser.GetReductionSyntaxNode(0)).text);
                        return LRParser.GetReductionSyntaxNode(0);
                    }
	case RuleConstants.RULE_PREPROCESSOR_LEXEM_TK_KAV :
	//<preprocessor_lexem> ::= 'TK_KAV'
			{
                        if (prepr.sm.AllowWrite())
                            prepr.WriteToStream(((token_info)LRParser.GetReductionSyntaxNode(0)).source_context, ((token_info)LRParser.GetReductionSyntaxNode(0)).text);
                        return LRParser.GetReductionSyntaxNode(0);
                    }
	case RuleConstants.RULE_PREPROCESSOR_LEXEM_TK_PROGRAM_LINE :
	//<preprocessor_lexem> ::= 'TK_PROGRAM_LINE'
			{
                        if (prepr.sm.AllowWrite())
                            prepr.WriteToStream(((token_info)LRParser.GetReductionSyntaxNode(0)).source_context, ((token_info)LRParser.GetReductionSyntaxNode(0)).text);
                        return LRParser.GetReductionSyntaxNode(0);
                    }
	case RuleConstants.RULE_PREPROCESSOR_LEXEM_TK_DIRECTIVE :
	//<preprocessor_lexem> ::= 'TK_DIRECTIVE' <empty>
			{
                bool inline = ( ((token_info)LRParser.GetReductionSyntaxNode(0)).source_context.begin_position.line_num - ((token_info)LRParser.GetReductionSyntaxNode(0)).source_context.end_position.line_num == 0 );
                string token_text = ((token_info)LRParser.GetReductionSyntaxNode(0)).text;

                if (token_text[1] == '$' && inline)
                {
                        compiler_directive _compiler_directive = (compiler_directive)pascal_parsertools.MakeDirective((token_info)LRParser.GetReductionSyntaxNode(0));
                        _compiler_directive.source_context.FileName = prepr.CurrentFileName;
                        prepr.CurrentFileName = _compiler_directive.source_context.FileName;

                        //INCLUDE
                        if (_compiler_directive.Name.text == prepr.INCLUDE)
                        {
                            if (prepr.sm.AllowWrite())
                            {
                                string IncludeFileName = _compiler_directive.Directive.text;
                                if (prepr.ProcessedFileNames.Contains(IncludeFileName) || Path.GetFileName(prepr.CurrentFileName) == Path.GetFileName(IncludeFileName))
                                {
                                    prepr.Errors.Add(new CircularInclude(prepr.CurrentFileName, _compiler_directive.source_context, null, IncludeFileName));
                                }
                                else
                                {
                                    if (!(bool)prepr.SourceFilesProvider(IncludeFileName, SourceFileOperation.Exists))
                                        prepr.Errors.Add(new IncludeFileNotFound(prepr.CurrentFileName, _compiler_directive.source_context, null, IncludeFileName));
                                    string b = prepr.CurrentFileName;
                                    //string t = prepr.SourceText;
                                    //string[] sl = prepr.SourceLines;
                                    prepr.ProcessFile(IncludeFileName);
                                    prepr.CurrentFileName = b;
                                    //prepr.SourceText = t;
                                    //prepr.SourceLines = sl;
                                    prepr.ProcessedFileNames.Remove(IncludeFileName);
                                }
                            }
                            return _compiler_directive;
                        }
                        //DEFINE                        
                        if (_compiler_directive.Name.text == prepr.DEFINE)
                        {
                            if (prepr.sm.AllowWrite())
                            {
                                string directive = _compiler_directive.Directive.text;
                                string name = prepr.getFirstIdent(directive);
                                string text = prepr.getSecondIdent(directive);
                                prepr.AddDefineDirective(name, text, _compiler_directive);
                            }
                            return _compiler_directive;
                        }
                        //UNDEF                        
                        if (_compiler_directive.Name.text == prepr.UNDEF)
                        {
                            if (prepr.sm.AllowWrite())
                            {
                                string directive = _compiler_directive.Directive.text;
                                string name = prepr.getFirstIdent(directive);
                                if (name != null)
                                    if (prepr.DefineDirectiveDefinded(name))
                                        prepr.DeleteDefineDirective(name, _compiler_directive);
                            }
                            return _compiler_directive;
                        }
                        //IFDEF                        
                        if (_compiler_directive.Name.text == prepr.IFDEF)
                        {
                            string directive = _compiler_directive.Directive.text;
                            if (prepr.sm.AllowWrite())
                            {

                                if (directive != null)
                                {
                                    if (!prepr.DefineDirectiveDefinded(directive))
                                        prepr.sm.ChangeState(false, 1);
                                    else
                                        prepr.sm.ChangeState(true, 1);
                                }

                            }
                            else
                                prepr.sm.ChangeState(false, -1);
                            return _compiler_directive;
                        }
                        //IFNDEF                        
                        if (_compiler_directive.Name.text == prepr.IFNDEF)
                        {
                            string directive = _compiler_directive.Directive.text;
                            if (prepr.sm.AllowWrite())
                            {

                                if (directive != null)
                                {
                                    if (prepr.DefineDirectiveDefinded(directive))
                                        prepr.sm.ChangeState(false, 1);
                                    else
                                        prepr.sm.ChangeState(true, 1);
                                }

                            }
                            else
                                prepr.sm.ChangeState(false, -1);
                            return _compiler_directive;
                        }
                        //ELSE                        
                        if (_compiler_directive.Name.text == prepr.ELSE)
                        {
                            if (prepr.sm.GetEndifCount() == 0 || prepr.sm.AllowElse() == false)
                            {
                                prepr.Errors.Add(new UnexpectedElse(prepr.CurrentFileName, _compiler_directive.source_context));
                            }
                            else
                            {
                                if (prepr.sm.AllowWrite())
                                    prepr.sm.ChangeState(false, false);
                                else
                                {
                                    if (prepr.sm.LastCondition() == 1)
                                        prepr.sm.ChangeState(true, false);
                                    else
                                        prepr.sm.ChangeState(false, false);
                                }
                            }
                            return _compiler_directive;
                        }
                        //ENDIF                        
                        if (_compiler_directive.Name.text == prepr.ENDIF)
                        {
                            if (prepr.sm.GetEndifCount() == 0)
                            {
                                prepr.Errors.Add(new UnexpectedEndif(prepr.CurrentFileName, _compiler_directive.source_context));
                            }
                            else
                            {
                                if (prepr.sm.AllowWrite())
                                    prepr.sm.ChangeState(true, true);
                                else
                                {
                                    if (prepr.sm.LastEndifCondition() == 1)
                                        prepr.sm.ChangeState(true, true);
                                    else
                                        prepr.sm.ChangeState(false, true);
                                }
                            }
                            return _compiler_directive;
                        }
                        return _compiler_directive;
                  }
                return LRParser.GetReductionSyntaxNode(0);
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
