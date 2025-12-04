var x := 1;

type
  t1 = partial class end;
  
  t0 = class
    constructor := x := 2;
  end;
  t1 = partial class(t0) end;
  
begin
  new t1;
  Assert(x = 2);
end.