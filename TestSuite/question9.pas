begin
  var x := true ? byte(3) : byte(2);
  assert(x.GetType = typeof(byte));
  var x1 := true ? word(4) : shortint(3);
  assert(x1.GetType() = typeof(integer));
end.