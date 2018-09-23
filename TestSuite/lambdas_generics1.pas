var o: object;
procedure p<X,Y>(a: System.Func<X, Y>; b:System.Func<Y, X>; c: X);
begin
  o := a(c);
end;

begin
  p(x -> x, x -> 1.0, 4.0);
  assert(Round(real(o)) = 4);
end.
