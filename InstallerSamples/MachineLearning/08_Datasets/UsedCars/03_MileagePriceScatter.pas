uses MLABC, PlotML;

begin
  var ds := Datasets.UsedCarsPrice;
  var df := ds.Data.Filter(r -> r.Float('price_k_rub') <= 6000);

  var mileage := df.ToVector('mileage_km');
  var price := df.ToVector('price_k_rub');

  Plot.Points(mileage, price, size := 4);
  Plot.XLabel := 'Пробег, км';
  Plot.YLabel := 'Цена, тыс. руб.';
  Plot.Title := 'UsedCars: цена и пробег';
end.
