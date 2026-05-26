// В этом примере KMeans применяется к реальным данным о российских городах.
//
// Для кластеризации используются два признака:
// log_population и density.
// Это позволяет группировать города не по координатам,
// а по масштабу и плотности.

uses MLABC;

begin
  var ds := Datasets.RussianCities;
  var df := ds.Data;

  df := df.WithColumnFloat('density', row -> row.Float('population') / row.Float('area'));
  df := df.WithColumnFloat('log_population', row -> Ln(row.Float('population')));

  var features := ['log_population', 'density'];

  var pipe :=
    UDataPipeline.Build(
      features,
      new StandardScaler,
      new KMeans(3, seed := 42)
    );

  var labels := pipe.FitPredict(df);
  df.AddIntColumn('cluster', labels, nil);

  Println('Кластеризация российских городов');
  Println;
  Println('Признаки: log_population, density');
  Println('Число кластеров: 3');

  for var cluster := 0 to 2 do
  begin
    Println;
    Println($'Кластер {cluster + 1}:');

    df.Filter(row -> row.Int('cluster') = cluster)
      .SortBy('population', descending := True)
      .Select(['city', 'population', 'density'])
      .Head(5)
      .Print;
  end;
end.
