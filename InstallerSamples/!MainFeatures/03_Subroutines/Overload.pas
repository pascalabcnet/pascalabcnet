// Перегрузка имен подпрограмм
procedure proc(i: integer);
begin
  Println('integer');
end;

procedure proc(c: char);
begin
  Println('char');
end;

procedure proc(r: real);
begin
  Println('real');
end;

begin
  proc(1);
  proc(2.5);
  proc('d');
end.  