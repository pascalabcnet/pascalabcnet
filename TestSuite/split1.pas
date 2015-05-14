begin
  var s: string := 'a b c';
  var arr: array of string := s.Split;
  assert(arr[0]='a');
  arr := s.Split();
  assert(arr[1]='b');
end.