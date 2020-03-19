var i: integer;

type
  t1 = class
    
    function f1<T>: T; virtual := default(T);
    
  end;
  
  t2 = class(t1)
    
    function f1<T>: T; override;
    begin
      i := 1;
      Result := default(T);
    end;
    
  end;
  
begin 
  var o: t1 := new t2;
  assert(o.f1&<integer> = 0);
  assert(i = 1);
end.