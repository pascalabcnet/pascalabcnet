unit u_methodcall1;

uses u_methodcall2;

var
  i: integer;
  
procedure p1;
begin
  var a: t2 := new t2;
  var p := procedure->
  begin
    a.p1;
    Inc(i);
  end;
  p;
end;

begin
end.