// Source
match e with
    Person(var value): <ACTION>
end;

// Desugared code

var <>genVar: Person;
if IsTest(e, <>genVar) then
begin
  // one of 3 cases
  
  // *********************************************************
  
  // 1 - instance deconstruct
  var value: ?; // should be filled during semantic analysis
  <>genVar.Deconstruct(value);
  
  // 2 - extension deconstruct
  var value: ?; // should be filled during semantic analysis
  PersonExt.Deconstruct(<>genVar, value);
  
  // 3 - typecast - when there's only one deconstruction parameter and no Deconstruct methods were found
  var value := <>genVar;
  
  // *********************************************************
  
  <ACTION>
end;

{
info we need to fill the tree:
1. Method type (instance or static) and a qualifier if static
2. Syntactic nodes of parameters' types

General: how to get the type of expression

case 1:
How to check if a certain type has instance method with given name - option 1
Save information about all Deconstruct methods during creation of a semantic tree - option 2

case 2:

    
} 
