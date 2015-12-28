// Вычисление числа Pi методом Монте-Карло

begin
  var n := 10000000;
  var pp := Range(1,n).Select(x->Rec(Random(),Random())).Where(p->sqr(p.Item1)+sqr(p.Item2)<1).Count/n*4;
  Print(pp);
end.