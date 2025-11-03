// Параметры-переменные

procedure DivMod(a,b: integer; var d,m: integer);
begin
  d := a div b;
  m := a mod b;
end;

begin
  var a := 7;
  var b := 3;
  var d,m: integer;
  DivMod(a,b,d,m);
  Println($'{a} div {b} = {d};  {a} mod {b} = {m}');
  a := 23;
  b := 5;
  DivMod(a,b,d,m);
  Println($'{a} div {b} = {d};  {a} mod {b} = {m}');
end.  