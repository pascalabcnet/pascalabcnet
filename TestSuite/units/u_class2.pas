unit u_class2;
type TClass = class
function Test1 : integer;
begin
Result := 1;
end;

function Test2 : integer;
begin
Result := 3;
end;
end;

function Test : array of integer;
const arr : array of integer = (1,2,3,4);
begin
Result := arr;
end;

function Test1 : integer;
begin
Result := 1;
end;

function Test2 : integer;
begin
Result := 5;
end;

function Test3 : boolean;
begin
Result := true;
end;

function Test4 : ^integer;
begin
New(Result);
Result^ := 10;
end;

begin
assert(Test[1]=2);
foreach i : integer in Test do
begin
end;
for i : integer := Test1 to Test2 do;
with Test1 do ToString();
var t := Test1;
assert(t=1);
if Test3 then
begin
end;
while not Test3 do;
var set1: set of integer := [Test1,Test2];
assert(Test1 in set1);
var set2 := [Test1..Test2];
assert(3 in set2);
assert(Test4^=10);
var obj := new TClass();
set1 := [obj.Test1..obj.Test2];
assert(2 in set1);
end.
