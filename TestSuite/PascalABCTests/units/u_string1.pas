unit u_string1;
procedure Test;
var s : string := 'abc';
begin
assert(s='abc');
end;

procedure Test2;
var s : string := 'abc';
procedure Nested;
var s1 : string := 'efg';
procedure Nested2;
begin
assert(s1='efg');
assert(s='abc');
end;

begin
assert(s='abc');
assert(s1='efg');
Nested2;
end;

begin
assert(s='abc');
Nested;
end;

var s : string := 'abc';
begin
assert(s='abc');
Test;
Test2;
end.