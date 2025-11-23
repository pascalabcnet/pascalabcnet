unit u_generics4;

type
  
  c1<T> = class
    
    public static function operator implicit(o: T): c1<T> := nil;
    
  end;
  
  
  c2<T1, T2> = class(c1<T2>) end;
  
end.