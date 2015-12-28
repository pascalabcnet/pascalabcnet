// Перегрузка имен подпрограмм
procedure proc(i: integer);
begin
  writeln('integer');
end;

procedure proc(c: char);
begin
  writeln('char');
end;

procedure proc(r: real);
begin
  writeln('real');
end;

begin
  proc(1);
  proc(2.5);
  proc('d');
end.  