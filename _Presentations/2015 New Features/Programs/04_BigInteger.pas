begin
  var p: BigInteger := 1;
  for var i:=2 to 100 do
    p *= i;
  writeln('100!=',p)  
end.