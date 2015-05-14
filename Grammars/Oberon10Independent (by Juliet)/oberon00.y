%{
// Эти объявления добавляются в класс GPPGParser, представляющий собой парсер, генерируемый системой gppg
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
		public long lVal;								// Длинное целое
		public char cVal;
		public double rVal;
		public pascal_set_constant sc;					// Константа - множество
		public named_type_reference ntr;				// Именованное определение типа
		public type_definition tdef;					// Сам тип		
		public diapason dpsn;							// Тип диапазон
		public array_type arrt;							// Тип массив
		public class_definition cldef;					// Определение записи
		public indexers_types indts;					// Тип типы индексов массива
		public ref_type rft;							// Тип указатель
		public ident_list il;							// Список идентификаторов
		public ident id;								// Идентификатор
		public oberon_ident_with_export_marker obrid;	// Обероновский уточненный идентификатор
		public oberon_export_marker obrem;				// Обероновская экспортная метка
		public var_def_statement vds;					// Описание переменных
		public variable_definitions vdss;				// Секция описания переменных
		public type_declarations td;					// Секция определения типов
		public type_declaration tdec;					// Описание типа
		public expression ex;							// Выражение
		public expression_list el;						// Список выражений
		public Operators ops;							// Операторы (операции)
		public block bl;								// Программный блок
		public statement st;							// Оператор программный
		public statement_list sl;						// Список операторов
		public case_variants cvars;						// Список вариантов оператора CASE
		public case_variant cvar;							// Вариант оператора CASE
		public declaration decsec;						// Описание
		public declarations decl;						// Список описаний
		public simple_const_definition scd;				// Определение константы
		public consts_definitions_list cdl;				// Список описаний констант		
		public procedure_definition pd;					// Описание процедуры
		public dot_node dn;								// Узел в точечной нотации
		public addressed_value adrv;					// Адресованное значение		
	}

%using PascalABCCompiler.SyntaxTree
%using PascalABCCompiler.Errors
%using PascalABCCompiler.Oberon00Parser

%namespace GPPGParserScanner

%start module											// Стартовый символ - программный модуль

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

module  			// Программный модуль
	: MODULE ident SEMICOLUMN mainblock ident COMMA 
    {
		if ($2.name != $5.name)
			PT.AddError("Имя " + $5.name + " должно совпадать с именем модуля " + $2.name, @5);
		
		// Подключение стандартного модуля Oberon00System, написанного на PascalABC.NET
		var ul = new uses_list("Oberon00System");
		
		// Формирование модуля основной программы (используется фабричный метод вместо конструктора)
		root = program_module.create($2, ul, $4, @$);
    }
	| INVISIBLE expr { // Для Intellisense
		root = $2;
	}
    ;
	
ident 				// Идентификатор
	: ID {
		$$ = new ident($1,@$); 
	}
	;
	
specifiedIdent		// Уточненный идентификатор
	: ident COMMA ident {
		$$ = new dot_node($1, $3, @$);
	}
	| ident {
		$$ = $1;
	}
	;
	
identDef			// Определенный идентификатор (возможна экспортная метка) TODO
	: ident ExportLabel {
		$$ = new oberon_ident_with_export_marker($1.name, $2, @$);
	}
	;
	
ExportLabel			// Экспортная метка TODO
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
	
mainblock			// Программный блок
	: Declarations BEGIN StatementSequence END
	{
		$$ = new block($1, $3, @$);
	}
	;
	
SetConstant			// Выражение-константа множество TODO
	: LBRACE SetElemList RBRACE {
		$$ = $2;
		$$.source_context = @$;
	}
	;
	
SetElemList			// Список элементов множества TODO
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

SetElem				// Элемент множества TODO
	: expr
	| expr DOUBLEPOINT expr {
		$$ = new diapason_expr($1, $3, @$);
	}
	;

expr 				// Выражение
	: SimpleExpr {
		$$ = $1;
	}
	| SimpleExpr Relation SimpleExpr {
		$$ = new bin_expr($1, $3, $2, @$);
	}
	;
	
Relation			// Отношение
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

SimpleExpr			// Простое выражение
	: signedTerm {
		$$ = $1;
		$$.source_context = @$;
	}
	| signedTerm AddOperator AddList {
		$$ = new bin_expr($1, $3, $2, @$);
	}
	;
	
signedTerm			// Слагаемое со знаком 
	: term 
	| MINUS term %prec UMINUS {
		$$ = new un_expr($2, Operators.Minus, @$);
	}
	| PLUS term %prec UPLUS {
		$$ = new un_expr($2, Operators.Plus, @$);
	}
	;
	
AddList				// Список со слагаемыми
	: term 
	| AddList AddOperator term {
		$$ = new bin_expr($1, $3, $2, @$);
	}
	;
	
term				// Слагаемое
	: factor 	
	| factor MultOperator FactorList {
		$$ = new bin_expr($1, $3, $2, @$);
	}
	;
	
FactorList			// Список со множителями 
	: factor 
	| FactorList MultOperator factor {
		$$ = new bin_expr($1, $3, $2, @$);
	}
	;
	
factor				// Множитель 
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
	
Designator					// Обозначение (обращение у элемету массива, ...) TODO
	: specifiedIdent {
		$$ = $1;
	}
	| ComplexDesignator {
		$$ = $1;
	}
	;
	
ComplexDesignator			// Составное обозначение TODO
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
	
ExprList					// Список выражений
	: expr {
		$$ = new expression_list($1, @$);
	}
	| ExprList COLUMN expr {
		$$ = $1;
		$$.Add($3, @3);
		$$.source_context = @$;
	}
	;
	
AddOperator  		// Операция сложения
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
	
MultOperator  		// Операция умножения 
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
	
Statement 					// Оператор TODO
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

StatementSequence 			// Последовательность (блок) операторов
	: Statement {
		$$ = new statement_list($1,@$);
	}
	| StatementSequence SEMICOLUMN Statement {
		$1.Add($3,@$);
		$$ = $1;
	}
	;

Assignment 					// Присваивание
	: Designator ASSIGN expr {
		$$ = new assign($1, $3, Operators.Assignment, @$);
	}
	;

IfStatement 				// Условный оператор
	: IF expr THEN StatementSequence ElseStatements END {
		$$ = new if_node($2, $4, $5, @$);
	}
	| IF expr THEN StatementSequence ElseIfStatements END {
		$$ = new if_node($2, $4, $5, @$);
	}
	;

ElseStatements				// Else-часть условного оператора 
	: ELSE StatementSequence {
		$$ = $2;
		$$.source_context = @$;
	} 
	| {
		$$ = null;
	}
	;
	
ElseIfStatements			// ElseIf-часть условного оператора
	: ELSEIF expr THEN StatementSequence ElseStatements {
		$$ = new if_node($2, $4, $5, @$);
	}
	| ELSEIF expr THEN StatementSequence ElseIfStatements{
		$$ = new if_node($2, $4, $5, @$);
	}
	;

WhileStatement 				// Цикл с предусловием 
	: WHILE expr DO StatementSequence END {
		$$ = new while_node($2, $4, WhileCycleType.While, @$);
	}
	;
	
RepeatStatement				// Цикл с постусловием 
	: REPEAT StatementSequence UNTIL expr {
		$$ = new repeat_node($2, $4, @$);
	}
	;
	
ForStatement				// Цикл
	: FOR ident ASSIGN expr TO expr ForStep DO StatementSequence END {
		expression step;
		if ($7 != null) 	step = $7;
		else 				step = new int32_const(1, @7);
		// Будем моделировать цикл через while
		statement_list forStmnt = GetForThroughWhile($2, $4, $6, $9, step, @3, @$);
		$$ = forStmnt;
		$$.source_context = @$;
	}
	;
	
ForStep						// Шаг цикла
	: BY ConstExpr {
		int32_const step32 = $2 as int32_const;
		if (step32 != null) {	// шаг является просто целым числом (без знака)
			if (step32.val == 0)	PT.AddError("Шаг цикла не может быть нулевым", @1);
			else					$$ = new int32_const(step32.val, @$);
		}
		else {					// в значении шага либо присутствует знак, либо это вообще не целое
			un_expr stepMinus = $2 as un_expr;
			int32_const stepMinusTerm = stepMinus.subnode as int32_const;
			bool signMinus = (stepMinusTerm != null) &&	(stepMinus.operation_type == Operators.Minus);
			if (signMinus)
				if (stepMinusTerm.val == 0)
					PT.AddError("Шаг цикла не может быть нулевым", @1);
				else	
					$$ = new un_expr(stepMinusTerm, Operators.Minus, @2);
			else
				PT.AddError("Шаг цикла должен быть целым числом", @1);
		}
	}
	| {
		$$ = null;
	}
	;
	
CaseStatement				// Оператор выбора 
	: CASE expr OF CaseVariantList ElseBranch END {
		$$ = new case_node($2, $4, $5, @$);
	}
	;
	
CaseVariantList				// Список вариантов оператора CASE 
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
	
CaseVariant					// Вариант оператора CASE 
	: CaseVariantLabelList COLON StatementSequence {
		$$ = new case_variant($1, $3, @$);
	}
	;
	
CaseVariantLabelList		// Список меток варианта
	: CaseVariantLabels {
		$$ = new expression_list($1, @$);
	}
	| CaseVariantLabelList COLUMN CaseVariantLabels {
		$$ = $1;
		$$.Add($3, @3);
		$$.source_context = @$;
	}
	;
	
CaseVariantLabels			// Метки варианта 
	: ConstExpr {
		$$ = $1;
	}
	| ConstExpr DOUBLEPOINT ConstExpr {
		$$ = new diapason_expr($1, $3, @$);
	}
	;
	
ElseBranch					// Ветка ELSE оператора CASE 
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
	
EmptyStatement				// Пустой оператор
	: {
		$$ = new empty_statement();		
	}
	;

IDList 						// Список идентификаторов
	: identDef {
		$$=new ident_list($1,@$);
		
	}
	| IDList COLUMN identDef {
		$1.Add($3,@$);
		$$ = $1;
	}
	;

VarDecl 					// Описание переменных одного типа
	: IDList COLON TypeDef SEMICOLUMN {
		$$  = new var_def_statement($1, $3, null, definition_attribute.None, false, @$);
	}
	;

VarDeclarations 			// Список описаний переменных
	: VarDecl {
		$$ = new variable_definitions($1,@$);
	}
	| VarDeclarations VarDecl {
		$1.Add($2,@$);
		$$ = $1;
	}
	;

	
ConstDecl 					// Описание одной константы
	: identDef EQ ConstExpr SEMICOLUMN {
		$$ = new simple_const_definition($1,$3,@$);
	}
	;

ConstExpr 					// Константное выражение
	: expr 
	;
	
ConstDeclarations 			// Список описаний констант
	: ConstDecl {
		$$ = new consts_definitions_list($1,@$);
	}
	| ConstDeclarations ConstDecl {
		$1.Add($2,@$); 
		$$ = $1;
	}
	;
	
ConstDeclarationsSect 		// Блок описания констант
	: CONST ConstDeclarations {
		$$ = $2;
		$$.source_context = @$;
	}
	;

VarDeclarationsSect 		// Блок описания переменных
	: VAR VarDeclarations {
		$$ = $2;
		$$.source_context = @$;
	}
	;
	
TypeDeclarationsSect		// Блок определения типов типов
	: TYPE TypeDeclarations {
		$$ = $2;
		$$.source_context = @$;
	}
	;
	
TypeDeclarations			// Список определений типов
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
	
TypeDecl 					// Определение типа
	: identDef EQ TypeDef SEMICOLUMN {
		$$ = new type_declaration($1, $3, @$);
	}
	;
	
TypeDef						// Тип TODO
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
	
ArrayType					// Тип массив TODO
	: ARRAY OF TypeDef {
		// открытый массив
		diapason dp = new diapason(new int32_const(0), new int32_const(0));
		indexers_types inxr = new indexers_types();
		inxr.Add(dp);
		$$ = new array_type(inxr, $3, @$);
	}
	| ARRAY LengthList OF TypeDef {
		// Будем искать array n of char, чтобы заменить на строку
		named_type_reference ntr = $4 as named_type_reference;
		bool complexArr = 	(ntr != null) && (ntr.names.Count == 1) && 
							(ntr.names[0].name == "char");
		if (complexArr) 
			$$ = GetArrWithStrInsteadCharArr($2, @2, @$);
		else
			$$ = new array_type($2, $4, @$); 
	}
	;
	
RecordType					// Запись TODO
	: RECORD END COLUMN {
	
	}
	;
	
LengthList					// Список размеров массива
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
	
Length 						// Длина массива
	: ConstExpr {
		// В обероне в массиве указывается целочисленная длина
		// Поэтому надо проверить, что выражение относится к целому типу
		object[] types = {$1};
		bool isLength = typeof(int32_const) == System.Type.GetTypeArray(types)[0];
		if (isLength)	
			$$ = GetDiapasonByArrLength($1, @$);
		else
			PT.AddError("Длина должна быть целым числом", @1);
	}
	;

PointerType					// Тип указатель
	: POINTER TO TypeDef {
		$$ = new ref_type($3, @$);
	}
	;
	
complexTypeIdent			// Составной идентификатор типа (id1.id2.id3)
	: ident {
		$$ = new named_type_reference(PT.InternalTypeName($1.name), @$);
	}
	| complexTypeIdent COMMA ident {
		$$ = $1;
		$$.Add(PT.InternalTypeName($3.name));
		$$.source_context = @$;
	}
	;
	
DeclarationsSect			// Блок описаний	TODO
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
	
Declarations 				// Список блоков описаний
	: {
	  $$ = new declarations();
	}
	| Declarations DeclarationsSect {
		if ($2 != null)
			$1.Add($2);
		$$ = $1;
		$$.source_context = @$;			// Необходимо показать место в программе, т.к. неявно это не сделано
										// (например, в конструкторе)
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

/* 	Возвращает последовательность операторов, соответсвующую
	циклу for, моделируемому через while */
public static statement_list GetForThroughWhile(ident id, expression expr, expression endExpr, statement_list stmnts,
		expression step, SourceContext assignSC, SourceContext wholeSC){
	// Будем моделировать цикл через while
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
	statement_list posStatementList = new statement_list();		// при положительном шаге
		posStatementList.Add(stmnts);
		posStatementList.Add( new assign(id, step, Operators.AssignmentAddition) );
	while_node posWhile = new while_node(
		new bin_expr(id, endFor, Operators.LessEqual),
		posStatementList,
		WhileCycleType.While);
	statement_list negStatementList = new statement_list();		// при отрицательном шаге
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

/* 	Возвращает многомерный массив, в котором при необходимости
	массив символов заменен на строку */	
public static type_definition GetArrWithStrInsteadCharArr(indexers_types lenList,
		SourceContext lenListSC, SourceContext wholeSC){
	List<type_definition> indxrs = lenList.indexers;
	diapason dp = indxrs[indxrs.Count - 1] as diapason;
	int32_const len = new int32_const();
	len.val = (dp.right as int32_const).val + 1;
	type_definition result = new string_num_definition(len, new ident("string", lenListSC), wholeSC);
	if (indxrs.Count > 1) {			// надо делать массив массивов ... строк
		indxrs.RemoveAt(indxrs.Count - 1);
		result = new array_type(lenList, result, wholeSC); 
	}
	return result;
}

/* 	Возвращает диапазон 0..Length-1, полученный по обероновской длине
	массива, и необходимый для паскалевского синтаксического узла */	
public static diapason GetDiapasonByArrLength(expression lenExpr, SourceContext lenSC){
	int32_const zero = new int32_const();
	zero.val = 0;
	int32_const max = (int32_const)lenExpr;
	max.val = max.val - 1;
	return new diapason(zero, max, lenSC);
}
