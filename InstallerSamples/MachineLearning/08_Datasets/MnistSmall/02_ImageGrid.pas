uses MLABC, PlotML;

begin
  var ds := Datasets.MnistSmall;
  var X := ds.Data.ToMatrix(ds.Features);

  Plot.Title := 'MnistSmall: первые 100 изображений';
  Plot.ImageGrid(X, 28, 28,
    count := 100,
    cols := 12,
    invert := True,
    spacing := 0.01);
end.
