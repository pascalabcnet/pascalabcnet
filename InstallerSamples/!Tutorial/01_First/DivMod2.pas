// Операции div и mod
var a: integer := 247;

begin 
  Write('Цифры числа в обратном порядке: ');
  // Выводим последнюю цифру
  Print(a mod 10);
  // Отбрасываем последнюю цифру
  a := a div 10;
  Print(a mod 10);
  a := a div 10;
  Print(a mod 10);
end.