begin
  var o := new class(field := default(byte?));
  o := nil;
  var a := o?.field = nil;
  assert(a);
  a := nil = o?.field;
  assert(a);
  a := o?.field <> nil;
  assert(not a);
  a := nil <> o?.field;
  assert(not a);
  var b: byte?;
  a := o?.field = b;
  assert(a);
  a := o?.field <> b;
  assert(not a);
end.