// Вложенные условные операторы. Антонимы

begin
  var s := ReadString('Введите слово из списка (черный,высокий,свет,радость,умный):');
  Print('Антоним: ');
  if s = 'черный' then
    Println('белый')
  else if s = 'высокий' then
    Println('низкий')
  else if s = 'свет' then
    Println('тьма')
  else if s = 'радость' then
    Println('горе')
  else if s = 'умный' then
    Println('глупый')
  else Println('Такого слова в списке нет');
end.