function f1: byte := 0;

begin
  var l := new List<byte>;
  l += f1;
  assert(l[0] = 0);
end.