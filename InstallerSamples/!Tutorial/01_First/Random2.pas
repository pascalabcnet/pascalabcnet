// Бросание кубиков

begin
  var (k1,k2) := Random2(1,6);
  Println($'Очки на кубиках: {k1} {k2}');
  Println($'Сумма очков: {k1+k2}');
end.