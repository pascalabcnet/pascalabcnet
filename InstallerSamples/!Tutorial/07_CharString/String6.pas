// Строки. Методы класса string
var 
  s: string := '  Pascal__NET  ';
  s1: string := 'NET';
    
begin
  Writeln('Исходная строка: ''',s,'''');
  s := s.Trim;
  Writeln('После вызова s.Trim: ''',s,'''');
  var p := s.IndexOf(s1); // Индексация - с нуля
  WritelnFormat('Позиция подстроки ''{0}'' в строке ''{1}'' равна {2}',s1,s,p);
  s := s.Remove(6,2);
  Writeln('После удаления символов __: ',s);
  s := s.Insert(6,'ABC.');
  Writeln('После вставки подстроки ''ABC.'': ',s);
  Writeln('Первая часть строки: ',s.Substring(0,9));
  Writeln('Последняя часть строки: ',s.Substring(10,3));
end.