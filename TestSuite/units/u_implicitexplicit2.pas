unit u_implicitexplicit2;

type
  
  t0<T> = class
    // Обязательно operator= или operator<>
    static function operator=(a, b: t0<T>) := false;
  end;
  
  t1 = class
    // Обязательно operator implicit
    static function operator implicit<T>(a: t0<T>): t1 := nil;
  end;
  
end.