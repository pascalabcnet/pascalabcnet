begin
  var y := 2;
  var x := (y<1)?3:4;
  assert(x=4);
  x := (y>1)?3:4;
  assert(x=3);
end.