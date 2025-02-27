// Использование длинных целых. Вычисление 100!

begin
  var n := 100;
  var f: BigInteger := 1;
  for var i:=2 to n do
    f := f * i;
  Print($'{n}! = {f}');
end.