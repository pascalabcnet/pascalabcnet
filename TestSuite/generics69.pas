procedure p2<T>;
begin
  var a := new T[1];
  //Ошибка: Невозможно преобразовать выражение типа array of T к не реализуемому этим типом интерфейсу IReadOnlyCollection<T>
  var c := a as System.Collections.Generic.IReadOnlyCollection<T>;
  assert(c.Count = 1);
end;

begin 
  p2&<integer>;
end.