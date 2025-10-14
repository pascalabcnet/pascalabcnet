// Цикл for. Поиск значения. Оператор break
const n = 10;

begin
  var k := ReadInteger('Введите число для поиска:');
  Println('Введите', n, 'чисел');
  var found := False;
  for var i := 1 to n do
  begin
    var x := ReadInteger;
    if x = k then
    begin
      found := True;
      break;
    end;
  end;
  if found then
    Println('Найдено') 
  else Println('Не найдено');
end.

