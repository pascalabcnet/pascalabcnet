var s1,s2: string;

begin
  // Определение длины строки
  s1 := 'ABCDEFGH';
  Writeln(s1.Length);

  // Сравнение строк без учета регистра символов
  s1 := 'AAA';
  s2 := 'aaa';
  if String.Compare(s1, s2, {ignoreCase - без учета регистра} true) = 0 then
    Writeln('Строки совпадают с точностью до регистра букв');
  
  // Определение вхождений подстрок
  s1 := 'Long string';
  s2 := 'string';
  if s1.EndsWith(s2) then
    Writeln('В конце строки s1 содержится подстрока, совпадающая с s2');
    
  // Поиск индекса вхождения подстроки в строку
  Writeln(s1.IndexOf(s2));
    
  // Извлечение подстрок
  s1 := 'ABCDEFGH';
  s2 := s1.Substring( {начальная_позиция = } 3);
  Writeln(s2);
  
  s2 := s1.Substring(3, {длина_подстроки = } 2);
  Writeln(s2);
end.