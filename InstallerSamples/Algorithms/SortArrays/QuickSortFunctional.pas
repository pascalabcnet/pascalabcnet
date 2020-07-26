// Быстрая сортировка Ч. Хоара
// Неэффективный алгоритм, иллюстрирующмй суть
function QS(a: array of integer): array of integer;
begin
  if a.Length < 2 then 
    Result := a
  else Result := QS(a[1:].Where(y->y<=a[0]).ToArray) + |a[0]| + QS(a[1:].Where(y->y>a[0]).ToArray)
                 //QS(ArrGen(a[1:],y->y<=a[0]))
                 //QS(|y:y in a[1:] where y<=a[0]|)
end;

begin
  var a := ArrRandom(20);
  a.Println;
  var b := QS(a);
  b.Println;
end.
