const n=10;

var i,j: integer;

begin
  writeln('Таблица умножения');
  for i:=1 to n do
  begin
    for j:=1 to n do
      write(i*j:4);
    writeln;
  end;
  readln;
end.
