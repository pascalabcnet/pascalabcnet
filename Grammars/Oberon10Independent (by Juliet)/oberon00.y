%{
// ��� ���������� ����������� � ����� GPPGParser, �������������� ����� ������, ������������ �������� gppg
    public syntax_tree_node root; // �������� ���� ��������������� ������ 
    public GPPGParser(AbstractScanner<ValueType, LexLocation> scanner) : base(scanner) { }
%}

%output=oberon00yacc.cs 

%parsertype GPPGParser

%union  
	{ 
		public bool bVal;  
		public string sVal; 
		public int iVal; 
		public long lVal;								// ������� �����
		public char cVal;
		public double rVal;
		public pascal_set_constant sc;					// ��������� - ���������
		public named_type_reference ntr;				// ����������� ����������� ����
		public type_definition tdef;					// ��� ���		
		public diapason dpsn;							// ��� ��������
		public array_type arrt;							// ��� ������
		public class_definition cldef;					// ����������� ������
		public indexers_types indts;					// ��� ���� �������� �������
		public ref_type rft;							// ��� ���������
		public ident_list il;							// ������ ���������������
		public ident id;								// �������������
		public oberon_ident_with_export_marker obrid;	// ������������ ���������� �������������
		public oberon_export_marker obrem;				// ������������ ���������� �����
		public var_def_statement vds;					// �������� ����������
		public variable_definitions vdss;				// ������ �������� ����������
		public type_declarations td;					// ������ ����������� �����
		public type_declaration tdec;					// �������� ����
		public expression ex;							// ���������
		public expression_list el;						// ������ ���������
		public Operators ops;							// ��������� (��������)
		public block bl;								// ����������� ����
		public statement st;							// �������� �����������
		public statement_list sl;						// ������ ����������
		public case_variants cvars;						// ������ ��������� ��������� CASE
		public case_variant cvar;							// ������� ��������� CASE
		public declaration decsec;						// ��������
		public declarations decl;						// ������ ��������
		public simple_const_definition scd;				// ����������� ���������
		public consts_definitions_list cdl;				// ������ �������� ��������		
		public procedure_definition pd;					// �������� ���������
		public dot_node dn;								// ���� � �������� �������
		public addressed_value adrv;					// ������������ ��������		
	}

%using PascalABCCompiler.SyntaxTree
%using PascalABCCompiler.Errors
%using PascalABCCompiler.Oberon00Parser

%namespace GPPGParserScanner

%start module											// ��������� ������ - ����������� ������

%token <sVal> ID STRING_CONST
%token <iVal> INTNUM 
%token <rVal> REALNUM
%token <lVal> LONGINTNUM
%token <bVal> TRUE FALSE
%token <cVal> CHAR_CONST
%token <op> PLUS MINUS MULT DIVIDE AND OR LT GT LE GE EQ NE DIV MOD
%token NOT 
%token ASSIGN SEMICOLUMN LPAREN RPAREN COLUMN COMMA COLON EXCLAMATION 
%token LBRACE RBRACE DOUBLEPOINT LBRACKET RBRACKET PIPE
%token TRUE FALSE
%token IF THEN ELSE ELSEIF BEGIN END WHILE DO MODULE CONST VAR TYPE
%token INVISIBLE
%token PROCEDURE ARRAY OF RECORD REPEAT UNTIL FOR TO BY CASE POINTER
%token ODD

%type <id> ident specifiedIdent identDef
%type <obrid> identDef
%type <obrem> ExportLabel
%type <il> IDList
%type <ntr> complexTypeIdent
%type <sc> SetConstant SetElemList 
%type <ex> expr ConstExpr SetElem  SimpleExpr ForStep CaseVariantLabels
%type <ex> term signedTerm factor AddList FactorList 
%type <st> Assignment IfStatement ElseIfStatements ElseStatements WhileStatement WriteStatement
%type <st> RepeatStatement Statement ForStatement  CaseStatement ElseBranch
%type <st> EmptyStatement ProcCallStatement
%type <sl> StatementSequence
%type <cvars> CaseVariantList
%type <cvar> CaseVariant
%type <decl> Declarations
%type <decsec> DeclarationsSect
%type <vds> VarDecl
%type <vdss> VarDeclarations VarDeclarationsSect
%type <scd> ConstDecl
%type <cdl> ConstDeclarations ConstDeclarationsSect
%type <td> TypeDeclarationsSect TypeDeclarations
%type <tdec> TypeDecl
%type <tdef> TypeDef ArrayType
%type <rft> PointerType
%type <cldef> RecordType
%type <pd> ProcedureDeclarationSect
%type <bl> mainblock
%type <el> factparams ExprList CaseVariantLabelList
%type <dpsn> Length
%type <indts> LengthList
%type <ops> AddOperator MultOperator Relation
%type <adrv> specifiedIdent ComplexDesignator Designator
%type <bVal> maybevar

%left LT GT LE GE EQ NE
%left PLUS MINUS OR
%left MULT DIVIDE AND 
%left NOT
%left UMINUS UPLUS

%%

module  			// ����������� ������
	: MODULE ident SEMICOLUMN mainblock ident COMMA 
    {
		if ($2.name != $5.name)
			PT.AddError("��� " + $5.name + " ������ ��������� � ������ ������ " + $2.name, @5);
		
		// ����������� ������������ ������ Oberon00System, ����������� �� PascalABC.NET
		var ul = new uses_list("Oberon00System");
		
		// ������������ ������ �������� ��������� (������������ ��������� ����� ������ ������������)
		root = program_module.create($2, ul, $4, @$);
    }
	| INVISIBLE expr { // ��� Intellisense
		root = $2;
	}
    ;
	
ident 				// �������������
	: ID {
		$$ = new ident($1,@$); 
	}
	;
	
specifiedIdent		// ���������� �������������
	: ident COMMA ident {
		$$ = new dot_node($1, $3, @$);
	}
	| ident {
		$$ = $1;
	}
	;
	
identDef			// ������������ ������������� (�������� ���������� �����) TODO
	: ident ExportLabel {
		$$ = new oberon_ident_with_export_marker($1.name, $2, @$);
	}
	;
	
ExportLabel			// ���������� ����� TODO
	: MULT {
		$$ = oberon_export_marker.export;
	}
	| MINUS {
		$$ = oberon_export_marker.export_readonly;
	}
	| {
		//$$ = oberon_export_marker.nonexport;
		$$ = oberon_export_marker.export;
	}
	;
	
mainblock			// ����������� ����
	: Declarations BEGIN StatementSequence END
	{
		$$ = new block($1, $3, @$);
	}
	;
	
SetConstant			// ���������-��������� ��������� TODO
	: LBRACE SetElemList RBRACE {
		$$ = $2;
		$$.source_context = @$;
	}
	;
	
SetElemList			// ������ ��������� ��������� TODO
	: SetElem {
		$$ = new pascal_set_constant();
		$$.Add($1);
		$$.source_context = @$;
		
	}
	| SetElemList COLUMN SetElem {
		$$ = $1;
		$$.Add($3);
		$$.source_context = @$;
	}
	| {
		$$ = new pascal_set_constant();
	}
	;

SetElem				// ������� ��������� TODO
	: expr
	| expr DOUBLEPOINT expr {
		$$ = new diapason_expr($1, $3, @$);
	}
	;

expr 				// ���������
	: SimpleExpr {
		$$ = $1;
	}
	| SimpleExpr Relation SimpleExpr {
		$$ = new bin_expr($1, $3, $2, @$);
	}
	;
	
Relation			// ���������
	: EQ {
		$$ = Operators.Equal;
	}
	| NE {
		$$ = Operators.NotEqual;
	}
	| LT {
		$$ = Operators.Less;
	}
	| LE {
		$$ = Operators.LessEqual;
	}
	| GT {
		$$ = Operators.Greater;
	}
	| GE {
		$$ = Operators.GreaterEqual;
	}
	;

SimpleExpr			// ������� ���������
	: signedTerm {
		$$ = $1;
		$$.source_context = @$;
	}
	| signedTerm AddOperator AddList {
		$$ = new bin_expr($1, $3, $2, @$);
	}
	;
	
signedTerm			// ��������� �� ������ 
	: term 
	| MINUS term %prec UMINUS {
		$$ = new un_expr($2, Operators.Minus, @$);
	}
	| PLUS term %prec UPLUS {
		$$ = new un_expr($2, Operators.Plus, @$);
	}
	;
	
AddList				// ������ �� ����������
	: term 
	| AddList AddOperator term {
		$$ = new bin_expr($1, $3, $2, @$);
	}
	;
	
term				// ���������
	: factor 	
	| factor MultOperator FactorList {
		$$ = new bin_expr($1, $3, $2, @$);
	}
	;
	
FactorList			// ������ �� ����������� 
	: factor 
	| FactorList MultOperator factor {
		$$ = new bin_expr($1, $3, $2, @$);
	}
	;
	
factor				// ��������� 
	: Designator {
		$$ = $1;
	}
	| INTNUM { 
		$$ = new int32_const($1,@$); 		
	}
	| LONGINTNUM{
		$$ = new int64_const($1, @$); 		
	}
	| REALNUM {
		$$ = new double_const($1, @$);
	}
	| TRUE {
		$$ = new bool_const(true,@$);
	}
	| FALSE {
		$$ = new bool_const(false,@$);
	}
	| CHAR_CONST {
		$$ = new char_const($1, @$);
	}
	| STRING_CONST {
		$$ = new string_const($1, @$);
	}
	| LPAREN expr RPAREN { 
		$$ = $2;
	}
	| NOT factor {
		$$ = new un_expr($2,Operators.LogicalNOT,@$);
	}	
	| SetConstant
	;
	
Designator					// ����������� (��������� � ������� �������, ...) TODO
	: specifiedIdent {
		$$ = $1;
	}
	| ComplexDesignator {
		$$ = $1;
	}
	;
	
ComplexDesignator			// ��������� ����������� TODO
	: specifiedIdent COMMA ident {
		$$ = new dot_node($1, $3, @$);
	}
	| specifiedIdent LBRACKET ExprList RBRACKET {
		indexer indxr = new indexer();
		indxr.dereferencing_value = $1;
		indxr.indexes = $3;
		$$ = indxr;
		$$.source_context = @$;
	}
	| ComplexDesignator COMMA ident {
		$$ = new dot_node($1, $3, @$);
	}
	| ComplexDesignator LBRACKET ExprList RBRACKET {
		indexer indxr = new indexer();
		indxr.dereferencing_value = $1;
		indxr.indexes = $3;
		$$ = indxr;
		$$.source_context = @$;
	}
	;
	
ExprList					// ������ ���������
	: expr {
		$$ = new expression_list($1, @$);
	}
	| ExprList COLUMN expr {
		$$ = $1;
		$$.Add($3, @3);
		$$.source_context = @$;
	}
	;
	
AddOperator  		// �������� ��������
	: MINUS {
		$$ = Operators.Minus;
	}
	| PLUS {
		$$ = Operators.Plus;
	}
	| OR {
		$$ = Operators.LogicalOR;
	}
	;
	
MultOperator  		// �������� ��������� 
	: MULT {
		$$ = Operators.Multiplication;
	}
	| DIVIDE {
		$$ = Operators.Division;
	}
	| DIV {
		$$ = Operators.IntegerDivision;
	}
	| MOD {
		$$ = Operators.ModulusRemainder;
	}
	| AND {
		$$ = Operators.LogicalAND;
	}
	;
	
Statement 					// �������� TODO
	: Assignment
	| IfStatement
	| WhileStatement
	| RepeatStatement
	| ForStatement
	| CaseStatement
	| WriteStatement
	| ProcCallStatement
	| EmptyStatement
	;

StatementSequence 			// ������������������ (����) ����������
	: Statement {
		$$ = new statement_list($1,@$);
	}
	| StatementSequence SEMICOLUMN Statement {
		$1.Add($3,@$);
		$$ = $1;
	}
	;

Assignment 					// ������������
	: Designator ASSIGN expr {
		$$ = new assign($1, $3, Operators.Assignment, @$);
	}
	;

IfStatement 				// �������� ��������
	: IF expr THEN StatementSequence ElseStatements END {
		$$ = new if_node($2, $4, $5, @$);
	}
	| IF expr THEN StatementSequence ElseIfStatements END {
		$$ = new if_node($2, $4, $5, @$);
	}
	;

ElseStatements				// Else-����� ��������� ��������� 
	: ELSE StatementSequence {
		$$ = $2;
		$$.source_context = @$;
	} 
	| {
		$$ = null;
	}
	;
	
ElseIfStatements			// ElseIf-����� ��������� ���������
	: ELSEIF expr THEN StatementSequence ElseStatements {
		$$ = new if_node($2, $4, $5, @$);
	}
	| ELSEIF expr THEN StatementSequence ElseIfStatements{
		$$ = new if_node($2, $4, $5, @$);
	}
	;

WhileStatement 				// ���� � ������������ 
	: WHILE expr DO StatementSequence END {
		$$ = new while_node($2, $4, WhileCycleType.While, @$);
	}
	;
	
RepeatStatement				// ���� � ������������ 
	: REPEAT StatementSequence UNTIL expr {
		$$ = new repeat_node($2, $4, @$);
	}
	;
	
ForStatement				// ����
	: FOR ident ASSIGN expr TO expr ForStep DO StatementSequence END {
		expression step;
		if ($7 != null) 	step = $7;
		else 				step = new int32_const(1, @7);
		// ����� ������������ ���� ����� while
		statement_list forStmnt = GetForThroughWhile($2, $4, $6, $9, step, @3, @$);
		$$ = forStmnt;
		$$.source_context = @$;
	}
	;
	
ForStep						// ��� �����
	: BY ConstExpr {
		int32_const step32 = $2 as int32_const;
		if (step32 != null) {	// ��� �������� ������ ����� ������ (��� �����)
			if (step32.val == 0)	PT.AddError("��� ����� �� ����� ���� �������", @1);
			else					$$ = new int32_const(step32.val, @$);
		}
		else {					// � �������� ���� ���� ������������ ����, ���� ��� ������ �� �����
			un_expr stepMinus = $2 as un_expr;
			int32_const stepMinusTerm = stepMinus.subnode as int32_const;
			bool signMinus = (stepMinusTerm != null) &&	(stepMinus.operation_type == Operators.Minus);
			if (signMinus)
				if (stepMinusTerm.val == 0)
					PT.AddError("��� ����� �� ����� ���� �������", @1);
				else	
					$$ = new un_expr(stepMinusTerm, Operators.Minus, @2);
			else
				PT.AddError("��� ����� ������ ���� ����� ������", @1);
		}
	}
	| {
		$$ = null;
	}
	;
	
CaseStatement				// �������� ������ 
	: CASE expr OF CaseVariantList ElseBranch END {
		$$ = new case_node($2, $4, $5, @$);
	}
	;
	
CaseVariantList				// ������ ��������� ��������� CASE 
	: CaseVariant {
		$$ = new case_variants();
		$$.Add($1);
		$$.source_context = @$;
	}
	| CaseVariantList PIPE CaseVariant {
		$$ = $1;
		$$.Add($3);
		$$.source_context = @$;
	}
	| {
		$$ = new case_variants();
	}
	;
	
CaseVariant					// ������� ��������� CASE 
	: CaseVariantLabelList COLON StatementSequence {
		$$ = new case_variant($1, $3, @$);
	}
	;
	
CaseVariantLabelList		// ������ ����� ��������
	: CaseVariantLabels {
		$$ = new expression_list($1, @$);
	}
	| CaseVariantLabelList COLUMN CaseVariantLabels {
		$$ = $1;
		$$.Add($3, @3);
		$$.source_context = @$;
	}
	;
	
CaseVariantLabels			// ����� �������� 
	: ConstExpr {
		$$ = $1;
	}
	| ConstExpr DOUBLEPOINT ConstExpr {
		$$ = new diapason_expr($1, $3, @$);
	}
	;
	
ElseBranch					// ����� ELSE ��������� CASE 
	: ELSE StatementSequence {
		$$ = $2;
		$$.source_context = @$;
	}
	| {
		$$ = null;
	}
	;

WriteStatement 
	: EXCLAMATION expr {
		expression_list el = new expression_list($2,@$);
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
	
EmptyStatement				// ������ ��������
	: {
		$$ = new empty_statement();		
	}
	;

IDList 						// ������ ���������������
	: identDef {
		$$=new ident_list($1,@$);
		
	}
	| IDList COLUMN identDef {
		$1.Add($3,@$);
		$$ = $1;
	}
	;

VarDecl 					// �������� ���������� ������ ����
	: IDList COLON TypeDef SEMICOLUMN {
		$$  = new var_def_statement($1, $3, null, definition_attribute.None, false, @$);
	}
	;

VarDeclarations 			// ������ �������� ����������
	: VarDecl {
		$$ = new variable_definitions($1,@$);
	}
	| VarDeclarations VarDecl {
		$1.Add($2,@$);
		$$ = $1;
	}
	;

	
ConstDecl 					// �������� ����� ���������
	: identDef EQ ConstExpr SEMICOLUMN {
		$$ = new simple_const_definition($1,$3,@$);
	}
	;

ConstExpr 					// ����������� ���������
	: expr 
	;
	
ConstDeclarations 			// ������ �������� ��������
	: ConstDecl {
		$$ = new consts_definitions_list($1,@$);
	}
	| ConstDeclarations ConstDecl {
		$1.Add($2,@$); 
		$$ = $1;
	}
	;
	
ConstDeclarationsSect 		// ���� �������� ��������
	: CONST ConstDeclarations {
		$$ = $2;
		$$.source_context = @$;
	}
	;

VarDeclarationsSect 		// ���� �������� ����������
	: VAR VarDeclarations {
		$$ = $2;
		$$.source_context = @$;
	}
	;
	
TypeDeclarationsSect		// ���� ����������� ����� �����
	: TYPE TypeDeclarations {
		$$ = $2;
		$$.source_context = @$;
	}
	;
	
TypeDeclarations			// ������ ����������� �����
	: TypeDecl {
		$$ = new  type_declarations();
		$$.Add($1);
		$$.source_context = @$;
	}
	| TypeDeclarations TypeDecl {
		$$ = $1;
		$$.Add($2);
		$$.source_context = @$;
	}
	;
	
TypeDecl 					// ����������� ����
	: identDef EQ TypeDef SEMICOLUMN {
		$$ = new type_declaration($1, $3, @$);
	}
	;
	
TypeDef						// ��� TODO
	: complexTypeIdent {
		$$ = $1;
	}
	| ArrayType {
		$$ = $1;
		$$.source_context = @$;
	}
	| RecordType {
		$$ = $1;
		$$.source_context = @$;
	}
	| PointerType {
		$$ = $1;
		$$.source_context = @$;
	}
	;
	
ArrayType					// ��� ������ TODO
	: ARRAY OF TypeDef {
		// �������� ������
		diapason dp = new diapason(new int32_const(0), new int32_const(0));
		indexers_types inxr = new indexers_types();
		inxr.Add(dp);
		$$ = new array_type(inxr, $3, @$);
	}
	| ARRAY LengthList OF TypeDef {
		// ����� ������ array n of char, ����� �������� �� ������
		named_type_reference ntr = $4 as named_type_reference;
		bool complexArr = 	(ntr != null) && (ntr.names.Count == 1) && 
							(ntr.names[0].name == "char");
		if (complexArr) 
			$$ = GetArrWithStrInsteadCharArr($2, @2, @$);
		else
			$$ = new array_type($2, $4, @$); 
	}
	;
	
RecordType					// ������ TODO
	: RECORD END COLUMN {
	
	}
	;
	
LengthList					// ������ �������� �������
	: Length {
		$$ = new indexers_types();
		$$.source_context = @$;
		$$.Add($1);
	}
	| LengthList COLUMN Length {
		$$ = $1;
		$$.source_context = @$;
		$$.Add($3);
	}
	;
	
Length 						// ����� �������
	: ConstExpr {
		// � ������� � ������� ����������� ������������� �����
		// ������� ���� ���������, ��� ��������� ��������� � ������ ����
		object[] types = {$1};
		bool isLength = typeof(int32_const) == System.Type.GetTypeArray(types)[0];
		if (isLength)	
			$$ = GetDiapasonByArrLength($1, @$);
		else
			PT.AddError("����� ������ ���� ����� ������", @1);
	}
	;

PointerType					// ��� ���������
	: POINTER TO TypeDef {
		$$ = new ref_type($3, @$);
	}
	;
	
complexTypeIdent			// ��������� ������������� ���� (id1.id2.id3)
	: ident {
		$$ = new named_type_reference(PT.InternalTypeName($1.name), @$);
	}
	| complexTypeIdent COMMA ident {
		$$ = $1;
		$$.Add(PT.InternalTypeName($3.name));
		$$.source_context = @$;
	}
	;
	
DeclarationsSect			// ���� ��������	TODO
	: VarDeclarationsSect {
		$$ = $1;
	}
	| ConstDeclarationsSect	{
		$$ = $1;
	}	
	| TypeDeclarationsSect {
		$$ = $1;
	}
	| ProcedureDeclarationSect {
		$$ = $1;
	}	
	;
	
Declarations 				// ������ ������ ��������
	: {
	  $$ = new declarations();
	}
	| Declarations DeclarationsSect {
		if ($2 != null)
			$1.Add($2);
		$$ = $1;
		$$.source_context = @$;			// ���������� �������� ����� � ���������, �.�. ������ ��� �� �������
										// (��������, � ������������)
	}
	;
	
ProcedureDeclarationSect
	: PROCEDURE ident maybeformalparams maybereturn SEMICOLUMN mainblock ident SEMICOLUMN {
	
	}
	;
	
maybeformalparams
	: {
		//$$ = null;
	}
	| LPAREN FPList RPAREN {
		//$$ = $2;
	}
	;
	
maybereturn					// TODO
	: {
		
	}
	| COLUMN TypeDef {
	
	}
	;
	
FPList
	: FPSect {
	
	}
	| FPList SEMICOLUMN FPSect {
	
	}
	;
	
FPSect
	: maybevar IDList COLON TypeDef {
	
	}
	;
	
maybevar
	: {
		$$ = false;
	}
	| VAR {
		$$ = true;
	}
	;
%%

/* 	���������� ������������������ ����������, ��������������
	����� for, ������������� ����� while */
public static statement_list GetForThroughWhile(ident id, expression expr, expression endExpr, statement_list stmnts,
		expression step, SourceContext assignSC, SourceContext wholeSC){
	// ����� ������������ ���� ����� while
	statement_list forStmnt = new statement_list();
	ident endFor = new ident("_oberon_for_end_");
	forStmnt.Add(new var_statement(new var_def_statement(
		new ident_list("_oberon_for_end_"),
		new named_type_reference("integer", null),
		endExpr,
		definition_attribute.None,
		false))
	);
	forStmnt.Add( new assign(id, expr, Operators.Assignment, assignSC) );
	statement_list posStatementList = new statement_list();		// ��� ������������� ����
		posStatementList.Add(stmnts);
		posStatementList.Add( new assign(id, step, Operators.AssignmentAddition) );
	while_node posWhile = new while_node(
		new bin_expr(id, endFor, Operators.LessEqual),
		posStatementList,
		WhileCycleType.While);
	statement_list negStatementList = new statement_list();		// ��� ������������� ����
		negStatementList.Add(stmnts);
		negStatementList.Add( new assign(id, step, Operators.AssignmentAddition) );
	while_node negWhile = new while_node(
		new bin_expr(id, endFor, Operators.GreaterEqual),
		negStatementList,
		WhileCycleType.While);
	forStmnt.Add( new if_node(
		new bin_expr(step, new int32_const(0), Operators.Greater),
		posWhile,
		negWhile, 
		wholeSC));
	return forStmnt;
}

/* 	���������� ����������� ������, � ������� ��� �������������
	������ �������� ������� �� ������ */	
public static type_definition GetArrWithStrInsteadCharArr(indexers_types lenList,
		SourceContext lenListSC, SourceContext wholeSC){
	List<type_definition> indxrs = lenList.indexers;
	diapason dp = indxrs[indxrs.Count - 1] as diapason;
	int32_const len = new int32_const();
	len.val = (dp.right as int32_const).val + 1;
	type_definition result = new string_num_definition(len, new ident("string", lenListSC), wholeSC);
	if (indxrs.Count > 1) {			// ���� ������ ������ �������� ... �����
		indxrs.RemoveAt(indxrs.Count - 1);
		result = new array_type(lenList, result, wholeSC); 
	}
	return result;
}

/* 	���������� �������� 0..Length-1, ���������� �� ������������ �����
	�������, � ����������� ��� ������������� ��������������� ���� */	
public static diapason GetDiapasonByArrLength(expression lenExpr, SourceContext lenSC){
	int32_const zero = new int32_const();
	zero.val = 0;
	int32_const max = (int32_const)lenExpr;
	max.val = max.val - 1;
	return new diapason(zero, max, lenSC);
}
