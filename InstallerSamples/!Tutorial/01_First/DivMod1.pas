// Операции div и mod

begin
  var a := ReadInteger('Введите a:');
  Println('Последняя цифра числа:', a mod 10);
  Println('Число без последней цифры:', a div 10);
  Println('Если число a чётно, то 0:', a mod 2);
end.