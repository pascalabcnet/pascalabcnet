uses MLABC, PlotML;

begin
  var ds := Datasets.MoscowHousing;
  var df := ds.Data;

  var metro := df.ToVector('metro_minutes');
  var price := df.ToVector(ds.Target);

  Plot.Points(metro.Data.ConvertAll(x -> x*1.0), price.Data, size := 3);

  Plot.XLabel('Минуты до метро');
  Plot.YLabel('Цена (руб)');
  Plot.Title := 'Зависимость цены квартиры от расстояния до метро';
end.