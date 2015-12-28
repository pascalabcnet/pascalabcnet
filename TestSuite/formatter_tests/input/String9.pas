// Пример решения задачи String9 из электронного задачника Programming Taskbook
uses PT4;
var
  n: integer;
  c1,c2: char;
  s: string;
begin
  Task('String9');
  read(n,c1,c2);
  s := '';
  for var i := 1 to n div 2 do
    s := s + c1 + c2;
  write(s);
end.

