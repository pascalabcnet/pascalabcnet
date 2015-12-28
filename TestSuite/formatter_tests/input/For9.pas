// Цикл for. Поиск значения. Оператор break
const n = 10;

var 
  k: integer;
  found: boolean;
  
begin
  writeln('Введите число для поиска: ');
  readln(k);
  writelnFormat('Введите {0} чисел',n);
  found := False;
  for var i:=1 to n do
  begin
    var x: integer;
    read(x);
    if x=k then
    begin
      found := True;
      break;
    end;
  end;
  if found then
    writeln('Найдено')
  else writeln('Не найдено');  
end.