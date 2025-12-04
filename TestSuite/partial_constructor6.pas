type
  t1 = partial class end;
  
  t0 = class
    public constructor := exit;
    public constructor(x: byte) := exit;
  end;
  
  t1 = partial class(t0) end;
  t1 = partial class(t0) end;
  
begin
  Assert( 2 = typeof(t1).GetConstructors.Length );
end.