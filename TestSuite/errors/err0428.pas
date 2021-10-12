type
  t0 = partial abstract class
    
  end;
  
  t1 = sealed class(t0)
    
  end;
  t0 = partial abstract class
    procedure p1; abstract;
  end;
  
begin
  new t1;
end.