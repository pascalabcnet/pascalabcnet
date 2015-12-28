// Использование длинных целых. Вычисление 100!
{$reference 'System.Numerics.dll'}
uses System.Numerics;

var n := 100;

begin
  var f := new BigInteger(1);
  for var i:=2 to n do
    f := f * i;
  writelnFormat('{0}! = {1}',n,f);
end.