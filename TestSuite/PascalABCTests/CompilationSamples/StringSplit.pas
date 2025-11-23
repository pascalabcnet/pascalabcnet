uses System;

var 
  str_arr: array of string;
  int_arr: array of integer;
  s: string := '12 765 765 76';
    
begin
  // Разбиение строки на массив подстрок с заданным разелителем
  str_arr := s.Split(' ');
  
  // Соединение массива подстрок с новым разделителем
  s := string.Join('+', str_arr);
  Write(s, '=');
  
  // Формирование целочисленного массива по массиву подстрок
  SetLength(int_arr, str_arr.Length);
  for var i:=0 to int_arr.Length-1 do
    integer.TryParse(str_arr[i], int_arr[i]);
  
  // Вычисление суммы элементов целочисленного массива
  var sum := 0;
  foreach v: integer in int_arr do
    sum += v;
  Writeln(sum);
end.