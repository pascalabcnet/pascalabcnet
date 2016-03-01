uses MyUnit; // подключили модуль

const Sz = 10;

var a: IntArr;

begin
  Assert(sz<=Size,'Размер массива должен быть <='+Size.ToString);
  FillArr(a,Sz);
  writeln('Содержимое массива: ');
  WriteArr(a,Sz);
  writeln('Минимальный элемент: ',Min(a,Sz));
end.
