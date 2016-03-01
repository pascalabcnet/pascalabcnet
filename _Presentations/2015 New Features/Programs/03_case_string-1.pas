var Country: string;

begin
  read(Country);
  write('Столица: ');
  
  if Country = 'Россия' then
    writeln('Москва')
  else if Country = 'Франция' then
    writeln('Париж')
  else if Country = 'Италия' then
    writeln('Рим')
  else if Country = 'Германия' then
    writeln('Берлин')
  else writeln('Нет в базе данных'); 
end.