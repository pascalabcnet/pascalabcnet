type
  r1 = record
    a: byte;
    static b: word;
  end;
  
begin
  //Ошибка: Количество полей не совпадает с количеством полей в записи
  var a: r1 := (a: 1);
  assert(a.a = 1);
end.