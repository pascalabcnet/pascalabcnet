// Тест процедурных переменных
procedure p(i: integer);
begin
  writeln(i);
end;

procedure q;
begin
  writeln('q');
end;

function f(r: real): real;
begin
  Result:=sqrt(r);
end;

function g: real;
begin
  Result:=sqrt(2);
end;

var
  pp: procedure (i: integer);
  qq: procedure;
  ff: function (r: real): real;
  gg: function: real;
  r: real;

begin
  cls;
  pp:=p;
  pp(1);
  qq:=q;
  qq;
  ff:=f;
  writeln(ff(4));
  gg:=g;
  writeln(g);
  r:=g;
  writeln(r);
end.
