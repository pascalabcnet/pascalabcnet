unit u_staticconstr2;

type
  t1<T> = partial class
    static i: integer;
  end;
  
  t1<T> = partial class
    
    static constructor;
    begin
      Inc(i);
    end;
    
  end;
begin
  
end.