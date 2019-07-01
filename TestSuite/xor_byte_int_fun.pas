function f1: integer := 256;

begin
  var b: byte := 255; 
  var i := f1 xor b; 
  Assert(i=511);
end.