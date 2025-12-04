type
  
  c1<T> = class where T: c1<T>;
    
  end;
  
  c2 = class(c1<List<integer>>)
    
  end;
  
begin 
  new c2;
end.