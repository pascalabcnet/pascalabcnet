%Extension "cs"
%CaseSensitive
%Preprocessor Define="define" Undef="undef" IfDef="if" ElIf="elif" Else="else" End="endif"

Skip Begin "//"
Skip Begin "#" Preprocessor
Skip Begin "/*" End "*/"
Skip BeginEnd "'" EscapeSymbol "'"
Skip BeginEnd '\"' EscapeSymbol '\\'
Skip Begin '@\"' End '\"'

Token LetterDigits [[:IsLetterOrDigit:]_&]*
Token Sign [[:IsPunctuation:][:IsSymbol:]]

Rule tkClassNamespace : @"class" | @"namespace"
Rule @Block : "{" [Any | Block]* "}"
Rule Tk : @AnyExcept(tkClassNamespace, "{", "}", ";")

Rule Program :			#ProgramNode*
Rule ProgramNode:	 	#ClassOrNamespace
						| #Field
						| #Method
						| #error

Rule ClassOrNamespace :	@Tk* @tkClassNamespace @Tk* "{" #ProgramNode* "}"
Rule Field :			@Tk* ";"
Rule Method	:			@Tk* Block

//На все про все 20 минут
//Начиная с создания папки для проекта, заканчивая готовым и работающим парсером