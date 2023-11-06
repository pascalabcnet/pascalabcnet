procedure pr1(act: (sequence of integer)-> ());
begin 
  act(Arr(1,2,3));
end;
var l: integer;

begin 
  pr1(x-> begin l := x.Count end);
  assert(l = 3);
end.