unit u_where2;

type
  
  t0<T> = class
  where T: t0<T>;
    
    procedure p1(a: T); virtual := exit;
    
  end;
  
end.