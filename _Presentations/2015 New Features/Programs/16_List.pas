begin
  var a := SeqRandom();
  a.Println;
  
  var l := new List<integer>;
  foreach var x in a do
    if x mod 2 = 0 then
      l.Add(x);
      
  l.Println;
end.