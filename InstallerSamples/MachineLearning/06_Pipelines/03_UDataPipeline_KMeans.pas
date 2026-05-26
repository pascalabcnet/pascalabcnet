// В этом примере показан unsupervised pipeline на DataFrame.
//
// Мы сначала готовим признаки в таблице,
// а затем передаём их в UDataPipeline:
// StandardScaler -> KMeans.
//
// Такой конвейер удобен, когда нужно явно сохранить
// шаги подготовки данных и кластеризации в одном месте.

uses MLABC;

begin
  var ds := Datasets.RussianCities;
  var df := ds.Data;

  // Добавляем два полезных признака для кластеризации городов.
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

  Println('Кластеризация городов с помощью UDataPipeline');
  Println;
  Println('Используемые признаки: log_population, density');
  Println('Число найденных кластеров: 3');

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
