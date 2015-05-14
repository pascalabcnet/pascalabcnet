function test(i: integer): integer;
begin
  result := i+1;
end;
begin
  var pp: function(x: integer): integer := test;
  var pp1: System.Func<integer,integer>;
  pp1 := pp;
  assert(pp(2)=3);
  assert(pp1(2)=3);
  pp := pp1;
  assert(pp(2)=3);
end.