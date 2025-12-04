type
  t0 = abstract class
    private constructor := exit;
  end;
  
  t1<T> = class
  where T: t0, constructor;
    
  end;
  
begin
  new t1<t0>;
end.