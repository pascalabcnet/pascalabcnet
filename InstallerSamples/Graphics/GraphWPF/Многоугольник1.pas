uses GraphWPF;

begin
  var (n,r) := (9,250);
  var a := ArrGen(n, i -> Window.Center + r * Vect(Cos(i*2*Pi/n),Sin(i*2*Pi/n)));
  foreach var p in a.Combinations(2) do
    Line(p[0], p[1]);
  //a.Combinations(2).ForEach(p -> Line(p[0], p[1]));
end.