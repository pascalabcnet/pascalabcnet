begin
  var s := 'abcd';
  var l: sequence of char := false ? s : s.Take(3);
  assert(l is IEnumerable<char>);
  assert(not (l is string));
end.