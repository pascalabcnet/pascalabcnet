uses MLABC, PlotML;

begin
  var ds := Datasets.UsedCarsPrice;
  var df := ds.Data.Filter(r -> r.Float('price_k_rub') <= 6000);

  df.Points('year', 'price_k_rub', 'Make', size := 4);
  Plot.XLabel := 'Год выпуска';
  Plot.YLabel := 'Цена, тыс. руб.';
  Plot.Title := 'UsedCars: цена по годам и маркам';
end.
