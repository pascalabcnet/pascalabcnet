unit u_generics13;

type
  t1<T> = class end;
  
  t2<T1, T2> = class(t1<t2<T1, T2>>) end;
  
  t3 = class end;
  t5 = class end;
  t4 = class
    
    static procedure p1<T1, T2>;
    begin
      new t2<T1, T2>;
    end;
    
  end;

end.