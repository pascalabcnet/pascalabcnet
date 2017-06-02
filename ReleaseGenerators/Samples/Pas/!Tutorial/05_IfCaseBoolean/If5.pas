// Вложенные условные операторы. Антонимы
var s: string;

begin
  writeln('Введите слово из списка (черный,высокий,свет,радость,умный): ');
  readln(s);
  write('Антоним: ');
  if (s='черный') then 
    writeln('белый')
  else if (s='высокий') then 
    writeln('низкий')
  else if (s='свет') then 
    writeln('тьма')
  else if (s='радость') then 
    writeln('горе')
  else if (s='умный') then 
    writeln('глупый')
  else writeln('Такого слова в списке нет')  
end.