// Бросание кубиков

begin
  var (k1,k2) := Random2(1,6);
  Writeln($'Очки на кубиках: {k1} {k2}');
  Writeln($'Сумма очков: {k1+k2}');
end.