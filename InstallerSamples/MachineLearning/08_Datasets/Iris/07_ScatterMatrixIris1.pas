uses MLABC, PlotML;

begin
  var ds := Datasets.Iris;

  var (X, labels) := ds.ToXYInt;

  Plot.PairPlot(X.Data, labels, ds.Features);

  Plot.Title('Iris: пары признаков');
end.