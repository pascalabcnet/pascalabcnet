program prog1;

procedure p1;
begin
  
  var a: array of byte;
  a.ForEach((b, i{@parameter i: integer;@})-> begin end);
  a.FindAll(x,y->x<2);
end;

begin end.