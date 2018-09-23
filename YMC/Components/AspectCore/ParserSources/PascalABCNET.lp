%Extension "pas"
%CaseInsensitive

Skip Begin "//"
Skip Begin "#"
Skip Begin "{" End "}"
Skip Begin "/*" End "*/"
Skip BeginEnd "'" EscapeSymbol "'"

Token LetterDigits [[:IsLetterOrDigit:]_&]*
Token Sign [[:IsPunctuation:][:IsSymbol:]]

Rule tkCodeOpen : @"begin" | @"case" | @"try"
Rule tkClassOpen : @"class" | @"interface" | @"record"
Rule tkDefs : @"var" | @"const" | @"label"
Rule tkClassVisModifier : @"internal" | @"private" | @"protected" | @"public" 
Rule tkMethod : @"procedure" | @"function" | @"constructor" | @"destructor"
Rule tkDirective : @"forward" | @"external"
Rule tkDirectiveHeader : @"virtual" | @"override" | @"reintroduce"
Rule tkInitFinal: @"initialization" | @"finalization"
Rule tkInterf: @"interface"
Rule tkImpl: @"implementation"

Rule Params : @"(" [@Any|@Params]* @")"
Rule @Block : tkCodeOpen [Any | Block]* "end"
Rule Attribute : @"[" [@Any | @Attribute]* @"]"

Rule Tk: @LetterDigits | @Sign | @Params | @"."
Rule Tk2: @LetterDigits | @Sign | @Params | @"." | @tkMethod | @"type" | @"[" |@"]"

Rule Program :		List0(ProgramNode) [#Block "." | "end" "."] Any* 

Rule ProgramNode:	 	  #Defs
						| #Field
						| #Method
						| #InitFinal
						| #InterfacePart
						| #Attribute
						| #error

Rule ClassNode :		tkClassVisModifier
						| "class"? #Field
						| #ClassMethod
						| #Attribute
						| #error
						
Rule InterfacePart:		tkInterf #InterfNode* tkImpl

Rule InterfNode:		#Defs
						| #Field
						| #MethodHeader
						| #Attribute
						| #error

Rule Field : 			@Tk @Tk2* ";"
Rule Class : 			@Tk @Tk2* @tkClassOpen @Params? List0(ClassNode) "end" ";"
	
Rule Method: 			@MethodHeader #MethodBody
Rule MethodHeader: 		@"class"? @tkMethod @Tk* ";" @DirectiveHeader*
Rule MethodBody: 		Directive
						| List0(MethodLocals) #Block ";"
Rule MethodLocals:		#Defs
						| #Method
				
Rule ClassMethod: 		@MethodHeader #ClassMethodBody
Rule ClassMethodBody: 	//empty
						| Directive
						| List0(Defs) #Block ";"
Rule Defs :				[tkDefs|"type"] List1(FieldOrClass)
Rule FieldOrClass:		#Field | #Class
					
Rule Directive: 		@tkDirective @Any* ";"
Rule DirectiveHeader:	@tkDirectiveHeader ";"

Rule InitFinal :		@tkInitFinal CodeToken*
Rule CodeToken: 		AnyExcept(tkInitFinal, "end", tkCodeOpen) | Block
