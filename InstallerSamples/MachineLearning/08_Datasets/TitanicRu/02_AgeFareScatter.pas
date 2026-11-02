uses MLABC, PlotML;

begin
  var ds := Datasets.TitanicRu;

  ds.Points('Возраст', 'ЦенаБилета', ds.Target, size := 5);
  Plot.XLabel := 'Возраст';
  Plot.YLabel := 'Цена билета';
  Plot.Title := 'Титаник: возраст, цена билета и выживание';
end.
