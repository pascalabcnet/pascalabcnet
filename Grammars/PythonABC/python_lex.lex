%using PascalABCCompiler.SyntaxTree;
%using System.Globalization;
%using GPPGTools;

%namespace PascalABCCompiler.PythonABCParser

%{
	PythonParserTools PT = new PythonParserTools();
%}

Alpha [a-zA-Z_]
INTNUM [0-9]+
FLOATNUM {INTNUM}\.{INTNUM}
ID [a-zA-Z_][a-zA-Z0-9_]* 
STR \'[^'\n]*\'|\"[^"\n]*\"
MLSTR \'\'\'[^']*\'\'\'|\"\"\"[^"]*\"\"\"
ENDL \n|\r\n?

LINECOMMENT #.*[\n\r]


%%

{LINECOMMENT} { 
		BEGIN(INITIAL);
	}

{ENDL} { 
		return (int)Tokens.ENDLINE;
	}
	
[ \n\t] ;

";" { 
		return (int)Tokens.SEMICOLUMN; 
	}

":" { 
		yylval._token_info = PT.create_token_info(yytext, yylloc);
		
		return (int)Tokens.COLON; 
	}

"." { 
		yylval._token_info = PT.create_token_info(yytext, yylloc);
		
		return (int)Tokens.DOT; 
	}

"->" { 
		yylval._token_info = PT.create_token_info(yytext, yylloc);
		
		return (int)Tokens.FUNCRETSYMB; 
	}

"," { 
		yylval._token_info = PT.create_token_info(yytext, yylloc);
		
		return (int)Tokens.COLUMN; 
	}
	
"(" { 
		yylval._token_info = PT.create_token_info(yytext, yylloc);
		
		return (int)Tokens.LPAREN; 
	}
	
")" { 
		yylval._token_info = PT.create_token_info(yytext, yylloc);
		
		return (int)Tokens.RPAREN; 
	}
	
"[" { 
		yylval._token_info = PT.create_token_info(yytext, yylloc);
		
		return (int)Tokens.LSQUARE; 
	}
	
"]" { 
		yylval._token_info = PT.create_token_info(yytext, yylloc);
		
		return (int)Tokens.RSQUARE; 
	}
	

// Opeartors:

"not" {
        yylval._op_type_node = PT.create_op_type_node(Operators.LogicalNOT, yylloc);
		
        return (int)Tokens.NOT;
    }

"and" {
        yylval._op_type_node = PT.create_op_type_node(Operators.LogicalAND, yylloc);
		
        return (int)Tokens.AND;
    }

"or" {
        yylval._op_type_node = PT.create_op_type_node(Operators.LogicalOR, yylloc);
		
        return (int)Tokens.OR;
    }    
    
"!=" { 
        yylval._op_type_node = PT.create_op_type_node(Operators.NotEqual, yylloc);
		
        return (int)Tokens.NE;
    }

"==" { 
        yylval._op_type_node = PT.create_op_type_node(Operators.Equal, yylloc);
		
        return (int)Tokens.EQ;
    }

">=" { 
        yylval._op_type_node = PT.create_op_type_node(Operators.GreaterEqual, yylloc);
		
        return (int)Tokens.GE;
    }
    
"<=" { 
        yylval._op_type_node = PT.create_op_type_node(Operators.LessEqual, yylloc);
		
        return (int)Tokens.LE;
    }
    
">" { 
        yylval._op_type_node = PT.create_op_type_node(Operators.Greater, yylloc);
		
        return (int)Tokens.GT;
    }
    
"<" {
        yylval._op_type_node = PT.create_op_type_node(Operators.Less, yylloc);
		
        return (int)Tokens.LT;
    }

"/" { 
        yylval._op_type_node = PT.create_op_type_node(Operators.Division, yylloc);
		
        return (int)Tokens.DIVIDE;
    }

"*" {
        yylval._op_type_node = PT.create_op_type_node(Operators.Multiplication, yylloc);
		
        return (int)Tokens.MULT;
    }

"+" { 
        yylval._op_type_node = PT.create_op_type_node(Operators.Plus, yylloc);
		
        return (int)Tokens.PLUS;
    }

"-" {
        yylval._op_type_node = PT.create_op_type_node(Operators.Minus, yylloc);
		
        return (int)Tokens.MINUS;
    }

"=" {
        yylval._op_type_node = PT.create_op_type_node(Operators.Assignment, yylloc);
		
        return (int)Tokens.ASSIGN;
    }

{STR} {
		yylval._const_node = PT.create_string_const(yytext, yylloc);
		
		return (int)Tokens.STRINGLITERAL;
	}

{MLSTR} {
		yylval._const_node = PT.create_string_const(yytext, yylloc);
		
		return (int)Tokens.MULTILINESTRING;
	}
		
"True" {
		yylval._const_node = PT.create_bool_const(true, yylloc);
		
		return (int)Tokens.TRUECONST;
	}
	
"False" {
		yylval._const_node = PT.create_bool_const(false, yylloc);
		
		return (int)Tokens.FALSECONST;
	}

{ID} { 
		if (ScannerHelper.keywords.ContainsKey(yytext))
		{
			yylval._token_info = PT.create_token_info(yytext, yylloc);
			
			return ScannerHelper.keywords[yytext];
		}
		else
		{
			yylval._ident = PT.create_directive_name(yytext, yylloc);
			
			return (int)Tokens.ID;
		}
	}

{INTNUM} { 
		yylval._const_node = PT.create_int_const(yytext, yylloc);
		
		return (int)Tokens.INTNUM;
	}

{FLOATNUM} { 
		yylval._const_node = PT.create_double_const(yytext, yylloc); 
		
		return (int)Tokens.FLOATNUM; 
	}


%{
    yylloc = new LexLocation(tokLin, tokCol, tokELin, tokECol);
%}


%%


class ScannerHelper 
{
	public static Dictionary<string, int> keywords;

	static ScannerHelper() 
	{
		keywords = new Dictionary<string, int>();
		
		keywords.Add("python",(int)Tokens.kINDENT);
		keywords.Add("nohtyp",(int)Tokens.kDEDENT);
		
		keywords.Add("if",(int)Tokens.kIF);
		keywords.Add("else",(int)Tokens.kELSE);
		keywords.Add("while",(int)Tokens.kWHILE);
		
		keywords.Add("and",(int)Tokens.AND);
		keywords.Add("or",(int)Tokens.OR);
		keywords.Add("not",(int)Tokens.NOT);
		
		keywords.Add("def",(int)Tokens.kDEF);
		keywords.Add("fed",(int)Tokens.kFED);
		
		keywords.Add("return",(int)Tokens.kRETURN);
		
		keywords.Add("int", (int)Tokens.bINT);
		keywords.Add("float", (int)Tokens.bFLOAT);
		keywords.Add("str", (int)Tokens.bSTR);		
		keywords.Add("bool", (int)Tokens.bBOOL);		
	}
}

