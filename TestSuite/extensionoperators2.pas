var i: integer;
procedure p;
begin
  Inc(i);
end;

procedure test(self: procedure); extensionmethod;
begin
  Inc(i);
end;

begin
  var w := p * 2;
  var w1 := w * 3;
  w1;
  assert(i = 6);
  w1.test;
  assert(i = 7);
end.