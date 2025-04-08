// Вычисление числа Pi методом Монте-Карло

begin
  var n := 10000000;
  var pp := (1..n).Select(x->(Random(),Random())).Where(p->sqr(p[0])+sqr(p[1])<1).Count/n*4;
  Print(pp);
end.