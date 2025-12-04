unit u_stringelem1;


procedure Test(s : string);
begin
s[2] := 'w';
assert(s[2] = 'w');
end;

procedure Test2(var s : string);
begin
s[2] := 'w';
assert(s[2] = 'w');
end;

procedure Test3(s : string);
procedure Nested;
begin
s[2] := 'w';
assert(s[2] = 'w');
end;
begin
Nested;
assert(s='awc');
s[2] := 'z';
assert(s[2] = 'z');
end;

var s: string;
begin
  s := '123';
  Insert('sg',s,50);
  assert(s='123sg');
  assert(Copy(s,6,2)='');
  assert(Copy(s,2,1000)='23sg');
  s := 'abc';
  Test(s); assert(s='abc');
  Test2(s); assert(s='awc');
end.