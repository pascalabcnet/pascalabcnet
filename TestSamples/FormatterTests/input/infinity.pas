label a;

procedure p;
label b;
begin
  goto b;
  writeln('ERROR!');
  b:
end;

begin
  a: writeln('Infinity :)');
  p;
  goto a;
end.