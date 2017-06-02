// Строки. Методы класса string
var 
  s: string := '  Pascal__NET  ';
  s1: string := 'NET';
    
begin
  writeln('Исходная строка: ''',s,'''');
  s := s.Trim;
  writeln('После вызова s.Trim: ''',s,'''');
  var p := s.IndexOf(s1); // Индексация - с нуля
  writelnFormat('Позиция подстроки ''{0}'' в строке ''{1}'' равна {2}',s1,s,p);
  s := s.Remove(6,2);
  writeln('После удаления символов __: ',s);
  s := s.Insert(6,'ABC.');
  writeln('После вставки подстроки ''ABC.'': ',s);
  writeln('Первая часть строки: ',s.Substring(0,9));
  writeln('Последняя часть строки: ',s.Substring(10,3));
end.