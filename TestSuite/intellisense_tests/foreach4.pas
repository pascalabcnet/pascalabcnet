begin
  foreach var el{@var el: byte;@} in IEnumerable&<byte>(Seq&<byte>(1, 2, 3)) do;
  foreach var el{@var el: byte;@} in IEnumerable&<byte>(Seq&<byte>(1, 2, 3)) + byte(4) do;
end.