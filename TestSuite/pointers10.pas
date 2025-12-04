begin
  var i: int64 := $FF22FF3322;
  var p := pointer(i);
  i := int64(p);
  if System.Environment.Is64BitProcess then
  begin
    assert(i = $FF22FF3322);
    assert(PointerToString(p) = '$FF22FF3322');
  end;
  i := $FF22FF;
  p := pointer(i);
  i := int64(p);
  assert(i = $FF22FF);
  assert(PointerToString(p) = '$FF22FF');
end.