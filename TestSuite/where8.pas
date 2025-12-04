type
  t0 = abstract class
    function f0: integer; abstract;
  end;
  t1 = class(t0)
    i: integer;
    function f0: integer; override := i;
    constructor(i: integer) := self.i := i;
  end;

function f1<T>: T;
where T: t0;
begin
  
  Result := T(t0(
    new t1(10)
  ));
  
  assert(Result.f0 = 10); // до #1986 было AccessViolationException
  
end;

begin
  
  f1&<t1>;
  
//  readln;
end.