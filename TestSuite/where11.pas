type
  
  t1<T> = class where T: t1<T>;
    
    function p1: T;
    begin
      var a: t1<T>;
      var b := a as T;
    end;
    
  end;
  
begin 
end.