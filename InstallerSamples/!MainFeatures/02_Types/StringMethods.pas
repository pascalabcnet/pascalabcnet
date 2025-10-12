// Строки. Методы класса string
var 
  s: string := '  Pascal__NET  ';
  s1: string := 'NET';
    
begin
  Println($'Исходная строка: ''{s}''');
  s := s.Trim;
  Println($'После вызова s.Trim: ''{s}''');
  var p := s.IndexOf(s1); // Индексация - с нуля
  Println($'Позиция подстроки ''{s1}'' в строке ''{s}'' равна {p}');
  s := s.Remove(6,2);
  Println('После удаления символов __:',s);
  s := s.Insert(6,'ABC.');
  Println('После вставки подстроки ''ABC.'':',s);
  Println('Первая часть строки:',s.Substring(0,9));
  Println('Последняя часть строки:',s.Substring(10,3));
end.