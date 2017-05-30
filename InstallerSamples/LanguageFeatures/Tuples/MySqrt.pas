function MySqrt(x: real): real;
begin
  var eps := 1e-15;
  (var a, var b) := (x, real.MaxValue);
  while abs(b-a) >= eps do
    (a,b) := (b,(a + x / a) / 2);
  Result := b;  
end;

begin
  Println(MySqrt(2));
  Println(MySqrt(3));
  Println(MySqrt(4));
end.