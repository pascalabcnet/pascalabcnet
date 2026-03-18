uses MLABC, PlotML;

begin
  var ds := Datasets.Iris;
  var df := ds.Data;

  var X := df.ToMatrix(ds.Features);
  var labels := df.EncodeLabels(ds.Target);

  Plot.PairPlot(X, labels, ds.Features);

  Plot.Title('Iris: пары признаков');
end.