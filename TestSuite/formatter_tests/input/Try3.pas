// Иллюстрация finally
// Секция finally выполняется независимо от того, произошло исключение или нет
var x: integer := 0;

begin
  try 
    if Random(2)=0 then
      x := 10 div x;
    writeln('Выполнение продолжается');   
  finally
    writeln('Сработала секция finally'); 
  end;
  // Если возникает исключение, то этот оператор не выполняется
  writeln('После секции finally'); 
end.
