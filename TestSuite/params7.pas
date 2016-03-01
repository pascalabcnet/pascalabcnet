begin
  var s := 'abc def';
  var arr : array of string := s.ToWords;
  assert(arr[0] = 'abc');
end.