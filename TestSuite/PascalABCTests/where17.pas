type
  
  c1<T1, T2> = class where T1: List<T2>;
    
  end;
  
  c2<T2, T3> = class(c1<T2, T3>) where T2: List<T3>;
    
  end;
    
begin 
  new c2<List<string>, string>;
end.