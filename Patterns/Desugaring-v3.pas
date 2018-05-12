// Source

match x with
    My(string(s), integer(t)) when s = 'asd':
end

// Desugared code (match, 1st stage)

var <>matchDone := false;
var <>genVarExpr := x;
if not matchDone and <>genVarExpr is My(string(s), integer(t)) then
begin
    if s = 'asd' then
    begin
        <>match1Done := true;
        <ACTION>
    end;
end;

// Desugared code (is, 2nd stage)

var <>matchDone := false;
var <>genVarExpr := x;
var <>genVar1: My;

if IsTest(<>genVarExpr, <>genVar1) then
var s: string;
var t: integer;

if <>genVarExpr is My(string(s), integer(t)) then
begin
    if s = 'asd' then
    begin
        <>match1Done := true;
        <ACTION>
    end;
end;
