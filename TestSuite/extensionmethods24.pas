var i: integer;
procedure p1(Self: sequence of real); extensionmethod;
begin
  i := 1;
end;

begin
  var r := new List<real>;
  r.p1;
  assert(i = 1);
end.