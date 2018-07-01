%CaseSensitive
%Extension "y"

Skip Begin "//"
Skip Begin "/*" End "*/"
Skip BeginEnd "'" EscapeSymbol "'"
Skip BeginEnd '\"' EscapeSymbol '\\'

Token LetterDigit [[:IsLetterOrDigit:]_]+
Token Sign [[:IsPunctuation:][:IsSymbol:]]

Rule @Action : 		"{" [Any|Action]* "}" 									//Блок кода, ограниченный фигурными скобками

Rule @Program : 	#Section1 "%%" #Section2 "%%" #Section3					//Стартовое правило

Rule @Section1 : 	List0(S1Node)											//Секция 1 и ее подузлы
Rule S1Node :		@"%{" @Any* @"%}" 
					| @"%" @AnyExcept("%%", "%", "{") @AnyExcept("%%", "%")*
					| error

Rule @Section2 : 	List0(yRule)											//Секция 2 и ее подузлы
Rule yRule : 		@LetterDigit ":" List1(SubRule, "|") ";"				//@LetterDigit ":" List1(SubRule, "|") ";"
					| error
Rule SubRule : 		@List0(subRulePart)
Rule subRulePart : 	@AnyExcept("{", "|", ";")
					| @Action

Rule @Section3 : 	Any*