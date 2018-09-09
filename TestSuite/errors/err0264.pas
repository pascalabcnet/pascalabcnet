type
  t0=record end;
  
  t1<T>=class
  where T: record;
  end;
  
  t2<T>=class(t1<T>) where T: class; end;

begin
  //var a := new t2<t0>;//не обязательно чтоб вызвать ошибку
end.