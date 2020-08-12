// Быстрая сортировка Ч. Хоара
// Неэффективный код, иллюстрирующий суть алгоритма
function QS(a: array of integer): array of integer := 
  if a.Length < 2 then 
    a
  else 
    QS(a[1:].FindAll(y->y<=a[0])) + a[:1] + QS(a[1:].FindAll(y->y>a[0]));

begin
  var a := ArrRandom(20);
  a.Println;
  var b := QS(a);
  b.Println;
end.
