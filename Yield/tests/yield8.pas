var aa: real := 555.0;

type R = class
  testR: R;
  testI: integer;
end;

type A = class
  testField: real;
  n: real := 83.3;
  testF: A;

  property testProp: real read n;

  constructor Create;
  begin
    testF := self;
  end;

  procedure p1(r: real);
  begin
    n += r;
  end;

  function f1(xx: real): A;
  begin
    n += xx;
    result := self;
  end;

  function Gen(n: integer): sequence of real;
  var j,k: real;
  begin
    j := 777.0;
    p1(j + k + testProp);
    testField := 444.5;
    testF.testF.f1(j - k).testF.testF.testField := 663.4;
    yield n;
    yield j;
    yield aa;
    yield testField;
    yield self.n;
  end;
end;

begin
  var t := new A();
  foreach var x in t.Gen(5) do
    Print(x);
end.