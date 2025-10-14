// Вложенные циклы for. Таблица умножения
const n = 9;

begin
  Println('Таблица умножения');
  for var i := 1 to n do
  begin
    for var j := 1 to n do
      Print($'{i*j,4}');
    Println;
  end;
end.
