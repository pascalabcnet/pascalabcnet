begin
  var l := Lst(0);
  foreach var x in |0,1,5| index i do
  begin
    loop 2 do
      l += i
  end;
  Assert(l.SequenceEqual(|0,0,0,1,1,2,2|));
end.  