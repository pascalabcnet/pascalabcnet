uses MLABC, PlotML;

begin
  var ds := Datasets.MoscowHousing;

  ds.Points('metro_minutes', ds.Target, size := 3);

  Plot.XLabel := 'Минуты до метро';
  Plot.YLabel := 'Цена (руб)';
  Plot.Title := 'Зависимость цены квартиры от расстояния до метро';
end.
