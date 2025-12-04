begin
  var arr := Range(1,5).Select(i->i.ToString).ToArray;
  assert(arr[0] = '1');
  assert(arr[4] = '5');
end.