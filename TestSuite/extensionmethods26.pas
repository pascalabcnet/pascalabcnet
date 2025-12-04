begin
  var a:array of char;
  a := Arr('A','B','C');
  var b := a + 'D';//'D' is char
  assert(b.First = 'A');
  assert(b is IEnumerable<char>);
  var c := a + 'DE';//'DE' is string
  assert(c is IEnumerable<char>);
  assert(c.Last = 'E');
  var e := 'D' + a;
  assert(e is IEnumerable<char>);
  assert(e.Last = 'C');
end.