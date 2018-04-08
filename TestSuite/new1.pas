function f(l: sequence of integer): integer;
begin
  Result := l.Sum;
end;
begin
var i: integer := f(new integer[](1,2));
assert(i = 3);
var j: integer := f(new List<integer>);
assert(j = 0);
end.