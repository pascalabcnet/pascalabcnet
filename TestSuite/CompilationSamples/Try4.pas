// Исключения. Генерация исключения
function f(x: integer): integer;
begin
  if x=0 then
    raise new System.Exception('Аргумент функции f не может быть равен 0');
  Result := 10 mod x;
end;

begin
  writeln(f(0));    
end.
