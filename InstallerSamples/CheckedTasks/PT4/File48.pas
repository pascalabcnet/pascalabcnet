// Пример решения задачи File48 из электронного задачника Programming Taskbook
uses PT4;
var
  f: array [1..4] of file of integer;
begin
  Task('File48');
  for var i:=1 to 4 do
  begin
    var s: string;
    read(s);
    Assign(f[i], s);
    if i < 4 then Reset(f[i])
    else Rewrite(f[i]);
  end;
  while not Eof(f[1]) do
    for var i:=1 to 3 do
    begin
      var a: integer;
      read(f[i],a);
      write(f[4],a);
    end;
  for var i := 1 to 4 do
    Close(f[i]);
end.  
