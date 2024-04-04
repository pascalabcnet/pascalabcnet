unit u_generics11;

type
  t1<T> = class
    
    public static function operator=(a, b: t1<T>): boolean := false;
    public static function operator<>(a, b: t1<T>): boolean := not (a=b);
    
  end;
  
end.