procedure p1(b: sequence of byte) := exit;

begin
  var hs := new HashSet<byte>;
  p1(hs.Select(a -> a{@parameter a: byte;@}));

end.