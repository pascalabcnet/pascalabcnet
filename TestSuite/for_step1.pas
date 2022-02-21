type Days = (Mon,Tue,Wed,Thi,Fri,sat);

begin
  var l := new List<integer>;
  for var i:=1 to 5 step 2 do
    l.Add(i);
  Assert(l.SequenceEqual(Arr(1,3,5)));
  
  var lc := new List<char>;
  for var i:='5' to '2' step -2 do
    lc.Add(i);
  Assert(lc.SequenceEqual(Arr('5','3')));
  
  var ld := new List<Days>;
  for var i:=Mon to Sat step 2 do
    ld.Add(i);
  Assert(ld.SequenceEqual(Arr(Mon,Wed,Fri)));
end.