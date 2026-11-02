uses MLABC, PlotML;

begin
  var ds := Datasets.Iris;
  ds.PairPlot(ds.Features);

  Plot.Title := 'Iris: пары признаков';
end.
