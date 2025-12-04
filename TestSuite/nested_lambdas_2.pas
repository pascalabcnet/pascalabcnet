procedure p(f: Func<integer, IEnumerable<integer>>);
begin
  var res := f(5).ToList();
  assert(res.Count = 5);
  assert(res[0] = 2);
  assert(res[1] = 3);
  assert(res[2] = 4);
  assert(res[3] = 5);
  assert(res[4] = 6);
end;

begin
  p(x -> Seq(1,2,3,4,5).Select(y -> Range(0, y).Select(z -> z).Count()));
end.