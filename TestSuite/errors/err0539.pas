//!Количество полей не совпадает с количеством полей в записи
type
  r1 = record
    a: byte;
    static b: word;
  end;
  
begin
  // Работает, но не должно
  var a: r1 := (a: 1; b: 2);
end.