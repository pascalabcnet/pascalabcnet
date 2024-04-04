type
  i1<T> = interface end;
  i2<T> = interface
    where T: i1<byte>;
  end;
  
  t1<T> = class(i1<T>, i2<t1<byte>>) end;
  t2<T> = class(i2<t1<byte>>, i1<T>) end;
  
begin end.