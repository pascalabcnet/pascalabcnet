//!Неизвестный именованный аргумент Вася
procedure p(s: string; z: string := ''; x: integer := 2);
begin
  Print(x);
end;

procedure p(x: real := 2);
begin
  
end;

// Должно быть - не могу найти подходящую функцию
begin
  p('dsfh', Вася := 3);
end.