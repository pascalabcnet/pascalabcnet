uses PlotML;

begin
  var labels := ['cat', 'dog', 'fox'];

  var m1 := Matr(
    [3.0, 0, 0],
    [1.0, 2, 1],
    [0.0, 1, 2]
  );

  var m2 := Matr(
    [4.0, 1, 0],
    [0.0, 3, 1],
    [1.0, 0, 2]
  );

  var m3 := Matr(
    [2.0, 1, 1],
    [0.0, 4, 0],
    [1.0, 1, 3]
  );

  var m4 := Matr(
    [5.0, 0, 1],
    [1.0, 3, 0],
    [0.0, 2, 4]
  );

  var fig := Plot.Grid(2, 2);

  fig[0, 0].ConfusionMatrix(m1, labels);
  fig[0, 0].Title := 'Model A';
  fig[0, 0].YLabel := 'Actual';

  fig[0, 1].ConfusionMatrix(m2, labels);
  fig[0, 1].Title := 'Model B';

  fig[1, 0].ConfusionMatrix(m3, labels);
  fig[1, 0].Title := 'Model C';
  fig[1, 0].XLabel := 'Predicted';
  fig[1, 0].YLabel := 'Actual';

  fig[1, 1].ConfusionMatrix(m4, labels);
  fig[1, 1].Title := 'Model D';
  fig[1, 1].XLabel := 'Predicted';

  Plot.Title := 'Confusion Matrices';
end.
