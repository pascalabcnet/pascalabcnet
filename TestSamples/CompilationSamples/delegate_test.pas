procedure p1;
begin
  writeln('p1');
end;

procedure p2;
begin
  writeln('p2');
end;

var p:procedure;

begin
  p += p1;
  p += p2;
  p;
  readln;
end.