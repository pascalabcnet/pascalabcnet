// Цикл repeat. Алгоритм Евклида нахождения наибольшего общего делителя
var A,B: integer;

begin
  write('Введите два целых числа: ');
  readln(A,B);
  repeat
    var C := A mod B;
    A := B;
    B := C;
  until B=0;
  write('Наибольший общий делитель = ',A);
end.