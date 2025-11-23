type TProc = procedure;

var i: integer;

begin
  var l := new List<integer>;
  l += ()->Inc(i);
end.