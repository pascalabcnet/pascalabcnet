begin
  var a := SeqRandom(10,1,9);
  a.Println;
  Println(a.Min,a.Max,a.Sum);
  a.Take(5).Println;
  a.Skip(5).Println;
  
  a.Where(x->x mod 2 = 0).Select(x->x*x).Println;
end.