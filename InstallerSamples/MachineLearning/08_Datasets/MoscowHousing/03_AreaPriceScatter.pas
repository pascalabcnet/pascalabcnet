uses MLABC, PlotML;

begin
  var ds := Datasets.MoscowHousing;
  var df := ds.Data;

  var area := df.ToVector('area');  
  var price := df.ToVector(ds.Target);

  Plot.Points(area, price, size := 3);

  Plot.XLabel('Площадь (м²)');
  Plot.YLabel('Цена (руб)');
  Plot.Title('Цена квартиры vs площадь');
end.