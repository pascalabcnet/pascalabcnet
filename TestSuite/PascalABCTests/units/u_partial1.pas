unit u_partial1;

interface

type
  t0<T> = class end;
  t1<T> = partial class(t0<T>)
    
  end;
  
  
implementation

type
  t1<T> = partial class(t0<T>)
    
  end;
  
end.