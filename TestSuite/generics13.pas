begin
  var a := Arr(0).Cast&<integer>.ToArray;
  assert(a[0] = 0);
  a := Arr(0).Cast&<integer>().ToArray();
  assert(a[0] = 0);
end.