begin
  var seq1: sequence of integer;
  seq1 := ((ArrGen(5,i->i).Reverse)+(ArrGen(5,i->i+5).Reverse));
  assert(seq1.ToArray[0] = 4);
end.