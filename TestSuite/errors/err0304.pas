//!Предописание I2<T> не может быть использовано в качестве базового типа
type
  I1<T> = interface;
  I2<T> = interface;
  
  I1<T> = interface(I2<T>)
    
  end;
  
  I2<T> = interface(I1<T>)
    
  end;

type t1=class(I1<byte>) end;
  
begin
  new t1;
end.