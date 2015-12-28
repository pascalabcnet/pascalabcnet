begin
  var Country := ReadString('Введите страну:');
  write('Столица: ');
  case Country of
    'Россия': writeln('Москва');
    'Франция': writeln('Париж');
    'Италия': writeln('Рим');
    'Германия': writeln('Берлин');
    else writeln('Страны нет в базе данных'); 
  end;
end.