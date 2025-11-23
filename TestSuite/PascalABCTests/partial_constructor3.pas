var x: string;

type
  t1 = partial class
    constructor := x += '1';
    constructor(p: byte) := x += '2';
  end;
  
  t0 = class
    constructor := x += '3';
  end;
  t1 = partial class(t0) end;
  
begin
  x := '';
  new t1;
  Assert(x = '31');
  x := '';
  new t1(1);
  Assert(x = '32');
end.