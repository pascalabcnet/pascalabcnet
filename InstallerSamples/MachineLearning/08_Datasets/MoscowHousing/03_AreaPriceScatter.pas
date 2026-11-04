uses MLABC, PlotML;

begin
  var ds := Datasets.MoscowHousing;

  ds.Points('area', ds.Target, size := 3);

  Plot.XLabel := 'Площадь (м²)';
  Plot.YLabel := 'Цена (руб)';
  Plot.Title := 'Цена квартиры vs площадь';
end.
