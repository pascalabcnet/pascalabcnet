begin
  var l := new List<integer>;
  for var i:=1 to 5 step 2 do
  begin  
    l.Add(i);
  end;  
  Assert(l.SequenceEqual(Arr(1,3,5)));
end.