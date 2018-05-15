// Source

match x with
    My(My1(s), My2(t)) when s = 'asd':
    My(s, t):
end

// Desugared code (match, 1st stage)

var <>genVarExpr := x;
if <>genVarExpr is My(My1(s), My2(t)) and s = 'asd' then
begin
    <ACTION>
end;
else
if <>genVarExpr is My(s, t) then
...
end;

// Desugared code (is, 2nd stage)

var <>genVarExpr := x;
var <>genVar1: My1;
var <>genVar2: My2;

if  <>genVarExpr is My(<>genVar1, <>genVar2) and 
    <>genVar1 is My1(s) and 
    <>genVar2 is My2(t) and 
    s = 'asd' then
begin
    <ACTION>
end;


// If desugaring (lvl 1)

if e is P(s) then
    <ACTION>
end;

// ----

var <>gen: P;
if IsTest(e, <>gen) then
    var s: ?;
    <>gen.Deconstruct(s);
    
    <ACTION>
end;

//-------------------------------------

// If desugaring (lvl 2)

if e is P1(s) and s is P2(t) then
    <ACTION>
else 
    <ACTION_ELSE>
end;

// ----

begin
var <>genVar1: P1;
var s: ?;
var <>success1 := false;
if IsTest(e, <>genVar1) then
    <>genVar1.Deconstruct(s);
    <>success1 := true;
    
var <>genVar2: P2;
var t: ?;
var <>success2 := false;
if IsTest(t, <>genVar2) then
    <>genVar2.Deconstruct(t);
    <>success2 := true;

if (<>success and <>success2)
    <ACTION>
    goto empty_statement;
end
<ACTION_ELSE>
end_if_label: empty_statement
