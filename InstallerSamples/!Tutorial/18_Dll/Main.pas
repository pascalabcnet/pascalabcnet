// Основная программа, использующая библиотеку MyLib.dll
// MyLib.pas должен быть предварительно откомпилирован
{$reference 'MyLib.dll'}

var f: Frac;

begin
  writeln('Сумма чисел 2 и 3 равна ',add(2,3));
  writeln('MyPi = ',MyPi);
  f := new Frac(2,3);
  f.Print;
end.