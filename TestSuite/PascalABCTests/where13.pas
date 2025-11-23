type
  
  c1<T> = class where T: c1<T>;
    
  end;
  
  c2 = class(c1<c2>)
    
  end;
  
begin 
  new c2;
end.