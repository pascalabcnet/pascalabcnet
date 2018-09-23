%CaseSensitive
%Extension "java"

Skip Begin "//"
Skip Begin "/*" End "*/"
Skip BeginEnd "'" EscapeSymbol "'"
Skip BeginEnd '\"' EscapeSymbol '\\'

Token ID [[:IsLetterOrDigit:]_]+
Token Sign [[:IsPunctuation:][:IsSymbol:]]

//TODO - field initializer: String [] Names = {"1", "2"};

Rule @Code	: "{" [Any|Code]* "}"

Rule TKList : @AnyExcept("class", "{", "}", ";")*

Rule Program: #Imports* #Class+
Rule Imports: @TKList ";"
Rule Class	: @TKList @"class" @ID "{" #ClassEntity* "}"
Rule ClassEntity	: #Field | #Method
Rule Field	: @TKList ";"
Rule Method	: @TKList #Code
