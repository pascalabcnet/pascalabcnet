function test(a:integer; b: integer:=3): integer;
begin
  Result := b;
end;

function test2(a: integer := 4): integer;
begin
  Result := a;
end;

function test3(a: integer := 4; b: real := 2.3): integer;
begin
  Result := a;
end;

function string.test(a: integer := 6): integer;
begin
  Result := a;
end;

function ff(delim: string := ' '): string;
begin
  Result := 'abc';
end;

procedure tt(str: string);
begin
  assert(str = 'abc');
end;

procedure tt(i: integer);
begin
  raise new Exception;
end;

type
  TClass=class

  function f(a: integer := 1; b: integer := 2): integer;
  begin
    Result:=2;
  end;
  
  end;

begin
assert(test3=4);
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
tt(ff);
assert(ArrRandomReal <> nil);
assert(ArrRandomReal.Any = true);
var obj := new TClass;
var l := obj.f;
assert(l = 2);
end.