//!Невозможно инстанцировать, так как тип T не наследован от List<integer>
type
  t1<T> = class
  where T: List<integer>;
    
  end;
  
  t2<T> = class(t1<T>)
  where T: List<real>, constructor;
    
  end;
  
begin 
  Assert(1=1)
end.