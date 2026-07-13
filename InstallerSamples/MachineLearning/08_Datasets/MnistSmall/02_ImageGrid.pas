uses MLABC, PlotML;

begin
  var ds := Datasets.MnistSmall;
  var df := ds.Data;

  var X := df.ToMatrix(ds.Features);
  var y := df.ToVector(ds.Target);

  var rows := new List<integer>;
  for var digit := 0 to 9 do
    rows.AddRange(y.Indices(v -> v = digit).Take(12));

  var sample := X.TakeRows(rows);

  Plot.Title := 'MnistSmall: по 12 изображений каждой цифры';
  Plot.ImageGrid(sample, 28, 28,
    count := 120,
    cols := 12,
    invert := True,
    spacing := 0);
end.
