type
  t1<T> = class
  where T: record, constructor;
    
  end;
  
  t2<T> = class(t1<T>)
  where T: record;
    
  end;
  
begin 
  Assert(1=1)
end.