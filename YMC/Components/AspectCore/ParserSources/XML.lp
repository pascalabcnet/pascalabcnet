%CaseSensitive
%Extension "xml" "xaml"

Skip Begin "<!--" End "-->"

Token ID [[:IsLetterOrDigit:]_]+
Token Sign [[:IsPunctuation:][:IsSymbol:]]

Rule TagStart: "<"
Rule TagEnd: ">"

Rule Program: [#XMLDecl|#DoctypeDcecl|#Tag]+

Rule XMLDecl : TagStart "?" @"xml" #Attribute* "?" TagEnd
Rule DoctypeDcecl: TagStart "!" @"DOCTYPE" #Attribute* TagEnd

Rule Tag: 	TagStart @ID #Attribute* TagEnd [#Tag|#Text]* CloseTag
		| 	TagStart @ID #Attribute* "/" TagEnd
Rule CloseTag :	TagStart "/" @ID TagEnd
Rule Attribute: @AnyExcept(">")+ "=" #String
Rule Text :	@AnyExcept("<",">")+
Rule String : @'\"' @Any* @'\"'
