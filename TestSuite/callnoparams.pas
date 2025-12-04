function f: integer;
begin
  Result := 10;
end;
begin
  var s := 'aaa bbb';
  var arr := s.Split;
  assert(arr[1] = 'bbb');
  var i := f;
  assert(i = 10);
  assert(s.Count = s.Length);
  var j := s.Count;
  assert(j = s.Count);
  s := 'abcdef';
  var q1 := s.Min;
  assert(q1 = 'a');
  assert(s.Min = 'a');
end.