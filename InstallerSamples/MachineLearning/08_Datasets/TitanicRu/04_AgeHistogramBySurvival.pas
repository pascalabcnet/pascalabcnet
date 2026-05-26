uses MLABC, PlotML;

begin
  var ds := Datasets.TitanicRu;
  var df := ds.Data.Filter(row -> row.IsValid('Возраст'));

  var diedAges: array of real := df
    .Filter(row -> row.Int('Выжил') = 0)
    .ToVector('Возраст').Data;

  var survivedAges: array of real := df
    .Filter(row -> row.Int('Выжил') = 1)
    .ToVector('Возраст').Data;

  Println($'Пассажиров с известным возрастом: {df.RowCount}');
  Println($'Не выжили: {diedAges.Length}');
  Println($'Выжили: {survivedAges.Length}');

  Plot.HistMany([diedAges,survivedAges], bins := 20, colors := [Colors.IndianRed, Colors.SteelBlue], alpha := 0.45, legend := ['не выжили','выжили']);
  Plot.Title := 'Титаник: возраст выживших (голубой) и невыживших (красный)';
  Plot.XLabel := 'Возраст';
  Plot.YLabel := 'Число пассажиров';
end.
