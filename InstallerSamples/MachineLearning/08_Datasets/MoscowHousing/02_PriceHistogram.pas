uses MLABC, PlotML;

begin
  var ds := Datasets.MoscowHousing;
  var df := ds.Data;

  var price := df.ToVector(ds.Target);

  Plot.Hist(price, bins := 40);
  Plot.Title('Распределение цен на квартиры в Москве');
  Plot.XLabel('Цена (руб)');
  Plot.YLabel('Количество');
end.