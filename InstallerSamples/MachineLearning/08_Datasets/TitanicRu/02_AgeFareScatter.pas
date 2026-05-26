uses MLABC, PlotML;

begin
  var ds := Datasets.TitanicRu;
  var df := ds.Data
    .Filter(row -> row.IsValid('Возраст') and row.IsValid('ЦенаБилета'));

  var age := df.ToVector('Возраст');
  var fare := df.ToVector('ЦенаБилета');
  var y := df.Int('Выжил');

  Plot.Points(age, fare, y, size := 5);
  Plot.XLabel := 'Возраст';
  Plot.YLabel := 'Цена билета';
  Plot.Title := 'Титаник: возраст, цена билета и выживание';
end.
