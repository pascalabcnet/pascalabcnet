begin
  var l := new List<integer>;
  foreach var x in Arr(1,3,5) index i do
  begin
    l.Add(x);
    l.Add(i);
  end;
  Assert(l.SequenceEqual(Arr(1,0,3,1,5,2)));
  
  l.Clear;
  //TODO #2852
//  foreach var x in 1..3 index i do
//  begin
//    l.Add(x);
//    l.Add(i);
//  end;
//  Assert(l.SequenceEqual(Arr(1,0,2,1,3,2)));
end.