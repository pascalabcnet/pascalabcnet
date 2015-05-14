procedure Proc(a : integer);
begin
assert(a=2);
end;

function Func(a : integer) : integer;
begin
Result := 4;
end;

var p := Proc;
    q := Func;
    
begin
p(2);
assert(q(3)=4);
var r := Func;
assert(r(3)=4);
end.