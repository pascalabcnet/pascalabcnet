// Вложенные циклы for. Таблица умножения
const n = 9;

begin
  writeln('Таблица умножения');
  for var i:=1 to n do
  begin
    for var j:=1 to n do
      write(i*j:4);
    writeln;  
  end;  
end.