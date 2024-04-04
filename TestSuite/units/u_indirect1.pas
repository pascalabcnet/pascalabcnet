unit u_indirect1;

uses u_indirect2;

procedure p1<T>(o: t1<T>) := exit;

begin
  p1(new t2);
  p1(new t3);
end.