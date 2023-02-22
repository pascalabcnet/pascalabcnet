// Литералы BigInteger
begin
  Println(25bi ** 25 + 17bi ** 17);
  
  var p := 1bi;
  for var i:=2 to 50 do
    p *= i;
  Println($'50! = {p}');  
end.
