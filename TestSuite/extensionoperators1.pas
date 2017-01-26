begin
  var seq1 := Range(1, 3)+2;
  var arr := seq1.ToArray;
  assert(arr[3] = 2);
  seq1 := 2+Range(1, 3);
  assert(seq1.First = 2);
  seq1 := Range(1, 3)+Range(4, 6);
  assert(seq1.Last = 6);
end.