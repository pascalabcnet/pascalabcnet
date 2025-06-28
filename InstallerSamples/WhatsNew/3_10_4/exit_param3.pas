begin
  var IsPrime: integer → boolean := n → begin
    if n < 2 then exit(False);
    for var i:=2 to n.Sqrt.Trunc do
    if n.Divs(i) then
      exit(False);
    exit(True);
  end;
  
  IsPrime(17).Print;
  IsPrime(1).Print;
end.
