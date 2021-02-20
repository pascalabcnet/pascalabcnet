%{
    public syntax_tree_node root; // Корневой узел синтаксического дерева 
    public GPPGParser(AbstractScanner<ValueType, LexLocation> scanner) : base(scanner) { }
%}

%output=oberon00yacc.cs 

%parsertype GPPGParser

%union  
	{ 
		public bool bVal; 
		public string sVal; 
		public int iVal; 
		public named_type_reference ntr;
		public ident_list il;
		public var_def_statement vds;
		public variable_definitions vdss;
		public expression ex;
		public expression_list el;
		public ident id;
		public statement st;
		public statement_list sl;
		public declarations decl;
		public Operators op;
		public simple_const_definition scd;
		public consts_definitions_list cdl;
	}

%using PascalABCCompiler.SyntaxTree
%using PascalABCCompiler.Errors
%using PascalABCCompiler.Oberon00Parser

%namespace GPPGParserScanner

%start module

%token <sVal> ID
%token <iVal> INTNUM 
%token <bVal> TRUE FALSE
%token <op> PLUS MINUS MULT DIVIDE AND OR LT GT LE GE EQ NE 
%token NOT 
%token ASSIGN SEMICOLUMN LPAREN RPAREN COLUMN COMMA COLON EXCLAMATION
%token TRUE FALSE ODD BOOLEAN INTEGER
%token IF THEN ELSE BEGIN END WHILE DO MODULE CONST VAR
%token INVISIBLE

%type <id> ident
%type <ntr> type
%type <ex> expr ConstExpr
%type <st> Assignment IfStatement WhileStatement WriteStatement Statement  
%type <st> EmptyStatement ProcCallStatement
%type <sl> StatementSequence
%type <decl> Declarations
%type <el> factparams
%type <il> IDList
%type <vds> VarDecl
%type <vdss> VarDeclarations VarDeclarationsSect
%type <scd> ConstDecl
%type <cdl> ConstDeclarations ConstDeclarationsSect

%left LT GT LE GE EQ NE
%left PLUS MINUS OR
%left MULT DIVIDE AND 
%left NOT
%left UMINUS

%%

module  
	: MODULE ident SEMICOLUMN Declarations BEGIN StatementSequence END ident COMMA 
    {
		if ($2.name != $8.name)
			PT.AddError("Имя "+$8.name+" должно совпадать с именем модуля "+$2.name,@8);
		
		// Подключение стандартной библиотеки
		ident_list il = new ident_list();
		il.Add(new ident("Oberon00System"));
		unit_or_namespace un = new unit_or_namespace(il);
		uses_list ul = new uses_list();
		ul.units.Insert(0, un);
		
		// Формирование главного модуля
		var b = new block($4, $6, @4.Merge(@8));
		var r = new program_module(null, ul, b, null,@$);
		r.Language = LanguageId.Oberon00;
		root = r;
    }
	| INVISIBLE expr { // Для Intellisense
		root = $2;
	}
    ;

ident 
	: ID {
		$$ = new ident($1,@$); 
	}
	;
	
expr 	
	: ident {
		$$ = $1;
	}
	| INTNUM { 
		$$ = new int32_const($1,@$); 		
	}
	| TRUE {
		$$ = new bool_const(true,@$);
	}
	| FALSE {
		$$ = new bool_const(false,@$);
	}
	| MINUS expr %prec UMINUS {
		$$ = new un_expr($2,Operators.Minus,@$);
	}
	| LPAREN expr RPAREN {$$ = $2;}
	| NOT expr {
		$$ = new un_expr($2,Operators.LogicalNOT,@$);
	}
	| expr PLUS expr {
		$$ = new bin_expr($1,$3,Operators.Plus,@$);
	}
	| expr MINUS expr {
		$$ = new bin_expr($1,$3,Operators.Minus,@$);
	}
	| expr MULT expr {
		$$ = new bin_expr($1,$3,Operators.Multiplication,@$);
	}
	| expr DIVIDE expr {
		$$ = new bin_expr($1,$3,Operators.IntegerDivision,@$);
	}
	| expr AND expr {
		$$ = new bin_expr($1,$3,Operators.LogicalAND,@$);
	}
	| expr OR expr {
		$$ = new bin_expr($1,$3,Operators.LogicalOR,@$);
	}
	| expr EQ expr {
		$$ = new bin_expr($1,$3,Operators.Equal,@$);
	}
	| expr NE expr {
		$$ = new bin_expr($1,$3,Operators.NotEqual,@$);
	}
	| expr LT expr {
		$$ = new bin_expr($1,$3,Operators.Less,@$);
	}
	| expr LE expr {
		$$ = new bin_expr($1,$3,Operators.LessEqual,@$);
	}
	| expr GT expr {
		$$ = new bin_expr($1,$3,Operators.Greater,@$);
	}
	| expr GE expr {
		$$ = new bin_expr($1,$3,Operators.GreaterEqual,@$);
	}
	;

Assignment 
	: ident ASSIGN expr {
		$$ = new assign($1, $3, Operators.Assignment,@$);
	}
	;

IfStatement 
	: IF expr THEN StatementSequence END {
		$$ = new if_node($2, $4, null, @$);
	}
	| IF expr THEN StatementSequence ELSE StatementSequence END {
		$$ = new if_node($2, $4, $6, @$);
	}
	;

WhileStatement 
	: WHILE expr DO StatementSequence END {
		$$ = new while_node($2, $4, WhileCycleType.While, @$);
	}
	;

WriteStatement 
	: EXCLAMATION expr {
		expression_list el = new expression_list($2);
		method_call mc = new method_call(el);
		mc.dereferencing_value = new ident("print");
		$$ = mc;
	}
	;
	
factparams
	: expr {
		$$ = new expression_list($1,@$);
	}
	| factparams COLUMN expr {
		$1.Add($3,@$);
		$$ = $1;
	}
	;
	
ProcCallStatement
	: ident LPAREN factparams RPAREN {
		$$ = new method_call($1,$3,@$);
	}
	;
	
EmptyStatement
	: {
		$$ = new empty_statement();		
	}
	;
	
Statement 
	: Assignment
	| IfStatement
	| WhileStatement
	| WriteStatement
	| ProcCallStatement
	| EmptyStatement
	;

StatementSequence 
	: Statement {
		$$ = new statement_list($1,@$);
	}
	| StatementSequence SEMICOLUMN Statement {
		$1.Add($3,@$);
		$$ = $1;
	}
	;

type 	
	: BOOLEAN {
		$$ = new named_type_reference("boolean",@$);
	}
	| INTEGER {
		$$ = new named_type_reference("integer",@$);
	}
	;

IDList 
	: ident {
		$$=new ident_list($1,@$);
	}
	| IDList COLUMN ident {
		$1.Add($3,@$);
		$$ = $1;
	}
	;

VarDecl 
	: IDList COLON type SEMICOLUMN {
	  $$  = new var_def_statement($1,$3,null,definition_attribute.None,false,@$);
	}
	;

VarDeclarations 
	: VarDecl {
		$$ = new variable_definitions($1,@$);
	}
	| VarDeclarations VarDecl {
		$1.Add($2,@$);
		$$ = $1;
	}
	;

	
ConstDecl 
	: ident EQ ConstExpr SEMICOLUMN {
		$$ = new simple_const_definition($1,$3,@$);
	}
	;

ConstExpr 
	: expr 
	;
	
ConstDeclarations 
	: ConstDecl {
		$$ = new consts_definitions_list($1,@$);
	}
	| ConstDeclarations ConstDecl {
		$1.Add($2,@$); 
		$$ = $1;
	}
	;
	
ConstDeclarationsSect 
	: CONST ConstDeclarations {
		$$ = $2;
		$$.source_context = @$;
	}
	;

VarDeclarationsSect 
	: VAR VarDeclarations {
		$$ = $2;
		$$.source_context = @$;
	}
	;

Declarations 
	: VarDeclarationsSect {
		$$ = new declarations($1,@$);
	}
	| ConstDeclarationsSect VarDeclarationsSect {
		$$ = new declarations($2,@$);
		$$.Add($2);
//		$$.source_context = @$;
	}
	;

%%
