type
  I6<T> = interface end;
  I0 = interface end;
  
  I1 = interface(I0) end;
  I2 = interface(I1,I6<I2>,I6<object>) end;
  
  t1 = class(I0, I2) end;
  t2 = class(I1, I2) end;
  t3 = class(I0, I1) end;
begin 
  Assert(1=1)
end.