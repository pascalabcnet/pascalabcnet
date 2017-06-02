// Строки. Стандартные подпрограммы работы со строками
var 
  s: string := '  Pascal__NET  ';
  s1: string := 'NET';
    
begin
  writeln('Исходная строка: ''',s,'''');
  s := Trim(s);
  writeln('После вызова функции Trim: ''',s,'''');
  var p := Pos(s1,s);
  writelnFormat('Позиция подстроки ''{0}'' в строке ''{1}'' равна {2}',s1,s,p);
  Delete(s,7,2);
  writeln('После удаления символов __: ',s);
  Insert('ABC.',s,7);
  writeln('После вставки подстроки ''ABC.'': ',s);
  writeln('Первая часть строки: ',Copy(s,1,9));
  writeln('Последняя часть строки: ',Copy(s,11,3));
end.