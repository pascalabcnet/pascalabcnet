function test(a:integer; b: integer:=3): integer;
begin
  Result := b;
end;

function test2(a: integer := 4): integer;
begin
  Result := a;
end;

function string.test(a: integer := 6): integer;
begin
  Result := a;
end;

begin
assert(test(2)=3);
assert(test(2,4)=4);
var j : integer := test2;
assert(j=4);
var i := test2;
assert(i = 4);
var s := 'abcd';
assert(s.test=6);
var k := s.test;
assert(k = 6);
end.