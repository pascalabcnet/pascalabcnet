var s:='ddd';
    i:=1;
    r:=1.1;
    c:='c';
    b:=true;

procedure test(b:boolean);
begin
  writeln(b?'OK':'ERROR');
end;

begin
  test(s='ddd');
  test(i=1);
  test(r=1.1);
  test(c='c');
  test(b=true);
  readln;
end.