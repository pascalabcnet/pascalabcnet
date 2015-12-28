// Использование вспомогательных переменных
var r: real;

begin
  write('Введите r: ');
  readln(r);
  var r2,r4,r8: real; // вспомогательные переменные
  r2 := r * r;
  r4 := r2 * r2;
  r8 := r4 * r4;
  writeln(r,' в степени 8 = ',r8);
end.