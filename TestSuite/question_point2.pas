begin
  var f: function: object;
  var o := f?.Invoke;
  assert(o = nil);
end.