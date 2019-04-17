begin
  var s: set of string := ['b','d '];
  foreach var o in s do
    Assert(o.GetType = typeof(string));
end.