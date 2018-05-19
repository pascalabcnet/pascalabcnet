// Source
match e with
    Person(var value): <ACTION>
end;

// Desugared code

var <>genVarExpr := e;
var <>genVar: Person;
if IsTest(<>genVarExpr, <>genVar) then
begin
  // always the same code
  var value: ?; // should be filled during semantic analysis
  <>genVar.Deconstruct(value);
  
  <ACTION>
end;

{
info we need to fill the tree:
1. Syntactic nodes of parameters' types    
}

// ****************************************************************************

// Source
if e is Person(var value) then
    <ACTION>

// Desugared code

var <>genVar: Person;
if IsTest(e, <>genVar) then
begin
  // always the same code
  var value: ?; // should be filled during semantic analysis
  <>genVar.Deconstruct(value);
  
  <ACTION>
end;

// ****************************************************************************

// Source
var res := e is Person(var value);

// Desugared code

var <>genVar: Person;
if IsTest(e, <>genVar) then
begin
  // always the same code
  var value: ?; // should be filled during semantic analysis
  <>genVar.Deconstruct(value);
  
  <ACTION>
end;