unit u_sokrbool1;
var a : integer;

begin
  if true then
    a := 2
  else
    a := 1;
  assert(a=2);
  if (a = 2) or (a = 0) then a := 3;
  assert(a=3);
  if (a=3) and (a=2) then a := 4;
  assert(a=3);
  if not (a=1) and not (a=2) then a := 5;
  assert(a=5);
end.