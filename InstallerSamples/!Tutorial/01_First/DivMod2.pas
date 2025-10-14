// Операции div и mod

begin 
  var a: integer := 247;
  Print('Цифры числа в обратном порядке:');
  // Выводим последнюю цифру
  Print(a mod 10);
  // Отбрасываем последнюю цифру
  a := a div 10;
  Print(a mod 10);
  a := a div 10;
  Print(a mod 10);
end.