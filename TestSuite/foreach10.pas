type
  // Обязательно пользовательский шаблонный тип
  t1<T> = class end;
  
begin
  // Обязательно массив
  // foreach по "a.AsEnumerable" не падает
  var a: array of t1<byte> := Arr(new t1<byte>);
  var i := 0;     
  foreach var x in a do
    begin
      Inc(i);
    end;
  assert(i = 1);
end.