type
  t1<T> = class
  where T: record;
    
  end;
  
  t2<T> = class(t1<T>)
  where T: record, constructor;
    
  end;
  
begin 
  Assert(1=1)
end.