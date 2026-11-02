uses MLABC, PlotML;

begin
  var ds := Datasets.UsedCarsPrice;
  var df := ds.Data.Filter(r -> r.Float('price_k_rub') <= 6000);

  df.Hist('price_k_rub', bins := 30);
  Plot.XLabel := 'Цена, тыс. руб.';
  Plot.YLabel := 'Число автомобилей';
  Plot.Title := 'UsedCars: распределение цен до 6000 тыс. руб.';
end.
