type
  t0 = class end;
  
  t1<T> = class
  where T: t0;
    
  end;
  
begin
  // Компилируется, а не должно
  var a: t1<byte>;
end.