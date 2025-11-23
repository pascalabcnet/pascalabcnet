begin
  var o: object := byte(5);
  var ok := (o is byte(var b)) and (b{@var b: byte;@}=5);
  ok.Print;
  writeln(b{@var b: byte;@});
end.