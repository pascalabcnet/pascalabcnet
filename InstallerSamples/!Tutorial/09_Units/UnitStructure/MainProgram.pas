uses MyUnit; // подключили модуль

const Sz = 10;

begin
  var a: IntArr := FillArr(Sz);
  Println('Содержимое массива:');
  a.Println;
  Println('Минимальный элемент:',Min(a));
end.
