uses MLABC, PlotML;

begin
  var ds := Datasets.UsedCarsPrice;
  var df := ds.Data.Filter(r -> r.Float('price_k_rub') <= 6000);

  var names := Arr('year', 'mileage_1000km', 'l_100km', 'engineSize');
  var X := df.ToMatrix(Arr('year', 'mileage_km', 'l_100km', 'engineSize'));
  
  for var i := 0 to X.RowCount - 1 do
    X[i, 1] := X[i, 1] / 1000.0;
  
  var price := df.ToVector('price_k_rub');

  Plot.PairPlot(X.Data, price.Data, names, bins := 6, maxPoints := 2000);
  Plot.Title := 'UsedCars: признаки, окрашенные по цене';
end.
