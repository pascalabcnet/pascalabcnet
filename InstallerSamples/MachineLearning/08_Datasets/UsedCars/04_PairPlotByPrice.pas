uses MLABC, PlotML;

begin
  var ds := Datasets.UsedCarsPrice;
  var df := ds.Data.Filter(r -> r.Float('price_k_rub') <= 6000);

  df.PairPlot(
    ['year', 'mileage_km', 'l_100km', 'engineSize'],
    'price_k_rub', bins := 6);
  Plot.Title := 'UsedCars: признаки, окрашенные по цене';
end.
