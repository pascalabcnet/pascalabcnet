function MyFun(x: real): real := 3*x-2;

var f: RealFunc;

begin
  f := MyFun;
  writeln(f(2));
  f := sqrt;
  writeln(f(2));
end.