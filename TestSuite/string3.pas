var
  x: uint64;
  y: cardinal;
  s: string;

begin
  x := 123456789012345;
  s := 'x = ' + x;  // x.ToString
  assert(s = 'x = 123456789012345');
  y := 4000000000;
  s := 'y = ' + y;  // y.ToString
  assert(s = 'y = 4000000000');
  var i := 4;
  s := 'i = ' + i;  // x.ToString
  assert(s = 'i = 4');
  
  var b := byte(4);
  s := 'b = ' + b;  // x.ToString
  assert(s = 'b = 4');
end.