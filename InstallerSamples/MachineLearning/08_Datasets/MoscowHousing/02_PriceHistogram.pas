uses MLABC, PlotML;

begin
  var ds := Datasets.MoscowHousing;

  ds.Hist(ds.Target, bins := 40);
  Plot.Title := 'Распределение цен на квартиры в Москве';
  Plot.XLabel := 'Цена (руб)';
  Plot.YLabel := 'Количество';
end.
