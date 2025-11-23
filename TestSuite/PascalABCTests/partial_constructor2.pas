var i: integer;
type
  t1 = partial class 
    constructor;
    begin
      i := 1;
    end;
  end;
  
  t1 = partial class
    
  end;
  
begin
  new t1;
  assert(i = 1);
end.