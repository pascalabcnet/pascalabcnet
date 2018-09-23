uses System;
type
  TA<T> = class
  end;
  
  TB<T> = class(TA<T>)
  end;
  
begin
  var x{@var x: TA<Nullable<byte>>;@} := TA&<byte?>(new TB<byte?>());
end.