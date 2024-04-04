unit u_indirect2;

uses u_indirect3;

type
  t1<T> = class end;
  t2 = class(t1<t_err>) end;
  t3 = t1<t_err>;
  
end.