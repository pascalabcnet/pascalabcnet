begin
  var s: string := '12 765 765 76';
  // Разбиение строки на массив подстрок с заданным разелителем
  var str_arr: array of string := s.Split;
  
  // Соединение массива подстрок с новым разделителем
  s := str_arr.JoinToString(' + ');
  Write(s, ' = ');
  
  // Формирование целочисленного массива по массиву подстрок
  var int_arr := new integer[str_arr.Length];
  for var i:=0 to int_arr.Length-1 do
    integer.TryParse(str_arr[i], int_arr[i]);
  
  // Вычисление суммы элементов целочисленного массива
  var sum := 0;
  foreach var v: integer in int_arr do
    sum += v;
  Writeln(sum);
end.