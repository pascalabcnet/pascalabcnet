begin
  var a := SeqRandom();
  a.Println;
  a.Sorted().Println(', ');
  Println(a.Min,a.Max);
  Println(a.Sum,a.Average);
end.