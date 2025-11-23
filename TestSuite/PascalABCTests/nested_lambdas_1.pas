procedure p(f: Func<string, IEnumerable<string>>);
begin

end;

procedure p(f: Func<integer, IEnumerable<integer>>);
begin
  var res := f(3).ToList();
  assert(res.Count = 4);
  assert(res[0] = 0);
  assert(res[1] = 1);
  assert(res[2] = 4);
  assert(res[3] = 9);
end;

begin
   p(x -> Range(0, x).Select(y -> y * y));
end.