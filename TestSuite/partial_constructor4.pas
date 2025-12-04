var x := 0;

type
  t1 = partial class end;
  
  t0 = class
    constructor := x := 1;
    constructor(p: byte) := x := 2;
  end;
  t1 = partial class(t0)
    
  end;
  
begin
  new t1(5);
  Assert(x = 2);
end.