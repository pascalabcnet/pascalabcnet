var i: integer;

begin
  var l := new List<Action0>;
  l += ()->Inc(i);
  l[0]();
  assert(i = 1);
end.