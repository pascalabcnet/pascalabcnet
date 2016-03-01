// Автоопределение типа переменной
// Описание переменной в заголовке цикла for
begin
  var x := 2;
  for var i:=1 to 10 do
  begin
    write(x,' ');
    x += 2;
  end;  
end.