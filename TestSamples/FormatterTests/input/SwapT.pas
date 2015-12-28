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
  writelnFormat('До Swap a={0}, b={1}',a,b);
  Swap(a,b);
  writelnFormat('После Swap a={0}, b={1}',a,b);
  var c := 2.5;
  var d := 3.3;
  writeln;
  writelnFormat('До Swap c={0}, d={1}',c,d);
  Swap(c,d);
  writelnFormat('После Swap c={0}, d={1}',c,d);
end.