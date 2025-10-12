// Обобщенные функции
// Выведение типа T по типам параметров

procedure Swap<T>(var a,b: T);
begin
  var v := a;
  a := b;
  b := v;
end;

begin
  var a := 2;
  var b := 3;
  Println($'До Swap a={a}, b={b}');
  Swap(a,b);
  Println($'После Swap a={a}, b={b}');
  var c := 2.5;
  var d := 3.3;
  Println;
  Println($'До Swap c={c}, d={d}');
  Swap(c,d);
  Println($'После Swap c={c}, d={d}');
end.