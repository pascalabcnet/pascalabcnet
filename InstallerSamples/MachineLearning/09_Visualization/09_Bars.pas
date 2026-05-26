uses PlotML;

begin
  var fig := Plot.Grid(2, 2);

  var labels := ['sedan', 'suv', 'hatchback', 'wagon'];

  fig[0, 0].Bar(labels, [120.0, 95.0, 70.0, 40.0]);
  fig[0, 0].Title := 'Cars A';
  fig[0, 0].XLabel := 'count';

  fig[0, 1].Bar(labels, [80.0, 110.0, 55.0, 30.0]);
  fig[0, 1].Title := 'Cars B';
  fig[0, 1].XLabel := 'count';

  fig[1, 0].Bar(labels, [45.0, 65.0, 100.0, 25.0]);
  fig[1, 0].Title := 'Cars C';
  fig[1, 0].XLabel := 'count';

  fig[1, 1].Bar(labels, [30.0, 50.0, 75.0, 90.0]);
  fig[1, 1].Title := 'Cars D';
  fig[1, 1].XLabel := 'count';

  Plot.Title := 'Bar plots 2x2';
end.
