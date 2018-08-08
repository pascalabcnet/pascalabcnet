begin
  var d:=new Dictionary<byte,integer>;
  d.Add(1, 2);
  var s:sequence of integer := d.Values + 5;
  var s2: sequence of byte := d.Keys + byte(5);
  var a := s.ToArray();
  assert(a[0] = 2);
  assert(a[1] = 5);
  var a2 := s2.ToArray();
  assert(a2[0] = 1);
  assert(a2[1] = 5);
end.