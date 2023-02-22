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
  
  var l1 := new List<integer>;
  for var i:=1 to 0 step 1 do
    l1.Add(i);
  Assert(l1.Count = 0);

  var l2 := new List<integer>;
  for var i:=1 to 0 step 2 do
    l2.Add(i);
  Assert(l2.Count = 0);

  var l3 := new List<integer>;
  for var i:=0 to 1 step -1 do
    l3.Add(i);
  Assert(l3.Count = 0);

  var l4 := new List<integer>;
  for var i:=0 to 1 step -2 do
    l4.Add(i);
  Assert(l4.Count = 0);
end.