uses MLABC, PlotML;

begin
  var ds := Datasets.UsedCarsPrice;
  var df := ds.Data.Filter(r -> r.Float('price_k_rub') <= 6000);

  var year := df.ToVector('year');
  var price := df.ToVector('price_k_rub');
  var make := df.EncodeLabels('Make');

  Plot.Points(year, price, make, size := 4);
  Plot.XLabel := 'Год выпуска';
  Plot.YLabel := 'Цена, тыс. руб.';
  Plot.Title := 'UsedCars: цена по годам и маркам';
end.
