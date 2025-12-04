procedure p<X,Y>(a: System.Func<X, Y>; b:System.Func<Y, X>; c: X);
begin
  writeln(a(c));
end;

begin
  p(x->x,x->1.0,4.0);
end.