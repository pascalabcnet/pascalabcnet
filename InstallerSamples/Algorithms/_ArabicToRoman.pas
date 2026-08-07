/// Преобразует число в римское число, записанное в виде строки
function ToRoman(x: integer): string; 
begin
  if x >= 4000 then
    RaiseArgumentException('Метод ToRoman нельзя применять для чисел >= 4000',x);
  var k := [1, 4, 5, 9, 10, 40, 50, 90, 100, 400, 500, 900, 1000];
  var v := ['I', 'IV', 'V', 'IX', 'X', 'XL', 'L', 'XC', 'C', 'CD', 'D', 'CM', 'M'];
  Result := '';
  while x > 0 do
  begin
    var j := k.BinarySearch(x);
    if j < 0 then j := -2 - j;
    x -= k[j];
    Result += v[j] 
  end
end;

begin
  // Генерируем 20 случайных целых чисел на отрезке [1;3999] и переводим в римские
  var a := ArrRandom(5, 1, 9) + ArrRandom(5, 10, 99) + ArrRandom(5, 100, 999) +
           ArrRandom(5, 1000, 3999);
  a.Shuffle.Select(n -> (n, ToRoman(n))).Print
end.