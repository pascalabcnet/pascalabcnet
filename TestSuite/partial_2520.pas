// #2520
type
  t0<T> = class
  where T: Exception;
    
  end;
  
  //Ошибка: Невозможно инстанцировать, так как тип T не наследован от Exception
  t1<T> = partial class(t0<T>)
  where T: Exception;
    
  end;
  
begin
  new t1<System.InvalidOperationException>;
end.