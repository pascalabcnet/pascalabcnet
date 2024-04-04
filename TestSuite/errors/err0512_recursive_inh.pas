//!Тип 't1<T>' наследует от самого себя
type
  t1<T> = partial class
    
  end;
  t2<T> = class(t1<T>) end;
  t1<T> = partial class(t2<byte>)
    
  end;
  
begin end.