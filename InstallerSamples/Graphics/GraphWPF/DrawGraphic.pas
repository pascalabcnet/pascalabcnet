uses GraphWPF;

begin
  Window.Title := 'Графики функций';
  var ww := Window.Width / 2;
  var hh := Window.Height / 2;
  DrawGraph(x -> sin(4 * x) + cos(3 * x), -5, 5, 0, 0, ww, hh, 'sin(4 * x) + cos(3 * x)');
  DrawGraph(x -> x * x, -5, 5, ww - 1, 0, ww, hh, 'x * x');
  DrawGraph(x -> exp(x), -3, 3, 0, 10, 0, hh-1, ww, hh, 'exp(x)');
  DrawGraph(x -> x*cos(2*x-1), -15, 15, ww - 1, hh-1, ww, hh, 'x * cos(2*x-1)');
end.