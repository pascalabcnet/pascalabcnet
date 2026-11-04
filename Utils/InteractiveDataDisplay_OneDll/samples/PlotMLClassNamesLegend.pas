uses PlotML;

begin
  var weights := [20.0, 22.0, 24.0, 14.0, 16.0, 18.0];
  var heights := [33.0, 34.0, 35.0, 39.0, 41.0, 42.0];
  var labels := [0, 0, 0, 1, 1, 1];

  Plot.Title := 'Бульдоги и спаниели';
  Plot.XLabel := 'Вес';
  Plot.YLabel := 'Высота в холке';
  Plot.Points(weights, heights, labels,
    classNames := ['бульдог', 'спаниель']);
  Plot.Line(1.0, -1.0, 20.0, Colors.Red);
end.
