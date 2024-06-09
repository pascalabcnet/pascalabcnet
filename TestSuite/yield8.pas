function V(n: byte) := 1;

function f1: sequence of integer;
begin
  //Ошибка: Ожидалось имя процедуры или функции
  var v := 1;//V(0);
  // Если закомментировать эту строчку - предыдущая строчка работает
  yield v;
end;

begin
  var i := 0;
  foreach var j in f1 do
    i := j;
  assert(i = 1);
end.