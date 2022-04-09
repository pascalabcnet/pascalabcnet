type
  r1<T> = record
    where T: record;
  end;
  
//Ошибка: Невозможно инстанцировать, так как тип T не является размерным
procedure p1<T>; where T: record;
begin
  // Обязательно лямбда
  // Обязательно возвращаемое значение, но неважно какое
  var f: r1<T>->byte := r->0;
end;

begin end.