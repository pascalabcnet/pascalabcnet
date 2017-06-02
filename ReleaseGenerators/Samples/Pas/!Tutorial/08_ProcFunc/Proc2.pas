// Параметры-переменные

procedure DivMod(a,b: integer; var d,m: integer);
begin
  d := a div b;
  m := a mod b;
end;

var 
  a,b: integer;
  d,m: integer;

begin
  a := 7;
  b := 3;
  DivMod(a,b,d,m);
  writelnFormat('{0} div {1} = {2};  {0} mod {1} = {3}',a,b,a div b,a mod b);
  a := 23;
  b := 5;
  DivMod(a,b,d,m);
  writelnFormat('{0} div {1} = {2};  {0} mod {1} = {3}',a,b,a div b,a mod b);
end.  