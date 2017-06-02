// Использование комплексных чисел
{$reference 'System.Numerics.dll'}
uses System.Numerics;

begin
  var a := new Complex(0,1);
  writeln('Мнимая единица: ', a);
  
  var b := Complex.Sqrt(a);
  writeln('Главный корень из мнимой единицы: ', b);
  
  var c := a*a;
  writeln('Квадрат мнимой единицы: ', c);
  
  var d := new Complex(1,2);
  writelnFormat('Модуль {0} равен {1}', d,d.Magnitude);
end.