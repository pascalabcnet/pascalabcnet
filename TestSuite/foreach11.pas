begin
  var l := new List<string>();
  foreach var s: string in 'abc' do
    l.Add(s);
  assert(l[0] = 'a');
  assert(l[1] = 'b');
  assert(l[2] = 'c');
end.