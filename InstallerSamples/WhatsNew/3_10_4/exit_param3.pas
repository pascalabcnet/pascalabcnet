begin
  var IsPrime: integer → boolean := n → begin
    for var i:=2 to n.Sqrt.Round do
    if n.Divs(i) then
      exit(False);
    exit(True);
  end;
  
  IsPrime(17).Print;
end.