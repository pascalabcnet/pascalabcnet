procedure p1<T>;
begin
  // Обязательно использовать T - шаблонный параметр p1
  // С пользовательским типом вместо KeyValuePair не воспроизводится
  var a: KeyValuePair<T,word>;
  // Обязательно сравнить на равенство
  assert(a = a);
  assert(not (a <> a));
  var b: KeyValuePair<integer,word>;
  assert(b = b);
  assert(not (b <> b));
end;

begin
  p1&<byte>;
end.
