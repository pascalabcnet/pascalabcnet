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
end.