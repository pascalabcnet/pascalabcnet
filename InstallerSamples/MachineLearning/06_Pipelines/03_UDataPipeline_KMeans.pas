// В этом примере показан unsupervised pipeline на DataFrame.
//
// Мы сначала готовим признаки в таблице,
// а затем передаём их в DataPipeline:
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
    DataPipeline.BuildClustering(
      features,
      new StandardScaler,
      new KMeans(3, seed := 42)
    );

  // Модель вычисляет номера кластеров
  var labels := pipe.FitPredict(df);
  
  // Добавляем к DataFrame столбец с номером кластера
  df := df.WithColumnInt('cluster', labels);
  // Группируем города по кластерам
  var clusters := df.GroupBy('cluster').Groups;

  Println('Кластеризация городов с помощью DataPipeline');
  Println;
  Println('Используемые признаки:',features);
  Println('Число найденных кластеров: 3');

  foreach var cluster in clusters do
  begin
    Println;
    Println($'Кластер {cluster.Key.Int + 1}:');

    cluster.Data
      .SortBy('population', descending := True)
      .Select(['city', 'population', 'density'])
      .Head(5)
      .Print;
  end;
end.
