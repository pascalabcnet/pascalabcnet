uses MLABC, PlotML;

begin
  var ds := Datasets.UsedCarsPrice;
  var df := ds.Data.Filter(r -> r.Float('price_k_rub') <= 6000);

  df.Points('mileage_km', 'price_k_rub', size := 4);
  Plot.XLabel := 'Пробег, км';
  Plot.YLabel := 'Цена, тыс. руб.';
  Plot.Title := 'UsedCars: цена и пробег';
end.
