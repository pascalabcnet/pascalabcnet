var s1,s2: string;

begin
  // Извлечение подстрок
  s1 := 'ABCDEFGH';
  s2 := s1.Substring( {начальная_позиция = } 3);
  Writeln(s2);
  
  s2 := s1.Substring(3, {длина_подстроки = } 2);
  Writeln(s2);
  
  // Вставка, удаление и замена подстрок
  s1 := 'ABCDEFGH';
  s2 := s1.Insert(2, 'xxx');
  Writeln(s2);
  
  s2 := s2.Replace('x', '!');
  Writeln(s2);
  
  s2 := s2.Remove(2, 3);
  Writeln(s2);
  
  s1 := 'слово слово слово';
  s2 := s1.Replace('слов', 'молок');
  Writeln(s2);
  
  // Удаление пробельных символов в концах строки
  s1 := '    xxx  xxx    ';
  Writeln('|', s1, '|');
  s1 := s1.Trim;
  Writeln('|', s1, '|');
  
  // Смена регистра символов
  s1 := 'абвгд';
  s1 := s1.ToUpper;
  Writeln(s1);
end.