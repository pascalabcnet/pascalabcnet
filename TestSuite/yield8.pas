function V(n: byte) := 1;

function f1: sequence of integer;
begin
  //Ошибка: Ожидалось имя процедуры или функции
  var v := V(0);
  // Если закомментировать эту строчку - предыдущая строчка работает
  yield v;
end;

function f2: sequence of integer;
begin
  //Ошибка: Ожидалось имя процедуры или функции
  var v := V(0);
  var w := v + 1;
  // Если закомментировать эту строчку - предыдущая строчка работает
  yield w;
end;

begin
  var i := 0;
  foreach var j in f1 do
    i := j;
  assert(i = 1);
  
  foreach var j in f2 do
    i := j;
  assert(i = 2);
end.