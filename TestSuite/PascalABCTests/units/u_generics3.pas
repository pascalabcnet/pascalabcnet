unit u_generics3;
type
  t0<T> = class
    i: integer;
    procedure p1;
    begin
      i := 2;
    end;
  end;
  t1 = class(t0<byte>) end;
 
 end.