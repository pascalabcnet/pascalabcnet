// Функция, возвращающая кортеж

function DivMod(a,b: integer): (integer,integer) := (a div b, a mod b);

begin
  var (a,b) := (7,3);
  var (d,m) := DivMod(a,b);
  Println($'{a} div {b} = {d};  {a} mod {b} = {m}');
  (a,b) := (23,5);
  (d,m) := DivMod(a,b);
  Println($'{a} div {b} = {d};  {a} mod {b} = {m}');
end.  