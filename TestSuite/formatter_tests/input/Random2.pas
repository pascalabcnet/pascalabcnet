// Бросание кубиков

var k1,k2: integer;

begin
  k1 := Random(1,6);
  k2 := Random(1,6);
  writeln('Очки на кубиках: ',k1,' ',k2);
  writeln('Сумма очков: ',k1+k2);
end.