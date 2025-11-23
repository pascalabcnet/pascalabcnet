begin
  var o := new object;
  var s := o ?? nil;
  assert(s = o);
  o := nil;
  s := o ?? new object;
  assert(s <> nil);
  var x := nil ?? 'abc';
  assert(x = 'abc');
end.