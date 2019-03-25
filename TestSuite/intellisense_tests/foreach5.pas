begin
  foreach var item{@var item: procedure;@}: procedure in Arr&<procedure>() do; // ()
  foreach var item{@var item: procedure(x: byte);@}: procedure(x: byte) in Arr&<procedure(x: byte)>() do; // (x: byte)
end.