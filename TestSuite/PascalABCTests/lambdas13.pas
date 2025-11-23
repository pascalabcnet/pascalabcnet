type TProc = procedure;

var i: integer;

begin
  var l := new List<TProc>;
  l += ()->Inc(i);
  l[0];
  assert(i = 1);
end.