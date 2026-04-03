uses MLABC, PlotML;

begin
  var ds := Datasets.MoscowHousing;
  var df := ds.Data;

  var price := df.ToVector(ds.Target);

  var logPrice := price.Data.Select(x -> Ln(x)).ToArray;

  var fig := Plot.Grid(1,2);
  Plot.Title := 'Распределение цен на квартиры в Москве';

  fig[0,0].Hist(price.Data, bins := 40);
  fig[0,0].Title := 'Цена';

  fig[0,1].Hist(logPrice, bins := 40);
  fig[0,1].Title := 'log(Цена)';
end.