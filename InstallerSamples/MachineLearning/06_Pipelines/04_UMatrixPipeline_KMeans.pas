// В этом примере показан unsupervised pipeline для матрицы признаков.
//
// Данные сначала извлекаются из DataFrame в матрицу,
// а затем pipeline выполняет масштабирование и кластеризацию.
//
// Это удобно, когда табличная подготовка уже сделана заранее
// и хочется работать сразу с числовой матрицей.

uses MLABC;

begin
  var ds := Datasets.RussianCities;
  var df := ds.Data;

  // Добавляем два полезных числовых признака для кластеризации городов.
  df := df.WithColumnFloat('density', row -> row.Float('population') / row.Float('area'));
  df := df.WithColumnFloat('log_population', row -> Ln(row.Float('population')));

  // Для кластеризации берем два числовых признака:
  // логарифм населения и плотность населения.
  var features := ['log_population', 'density'];
  var X := df.ToMatrix(features);

  // Строим matrix pipeline:
  // сначала StandardScaler, затем KMeans.
  var pipe := UMatrixPipeline.Build(
    new StandardScaler,
    new KMeans(3, seed := 42)
  );

  // Обучаем pipeline и получаем метки кластеров.
  pipe.Fit(X);
  var labels := LabelsToInts(pipe.Predict(X));

  // Добавляем метки кластеров в таблицу,
  // чтобы потом удобно вывести представителей каждого кластера.
  df.AddIntColumn('cluster', labels, nil);

  Println('Кластеризация городов с помощью UMatrixPipeline');
  Println;
  Println('Используются признаки log_population и density.');
  Println('Для каждого кластера выводятся самые крупные города.');
  Println;

  for var cluster := 0 to 2 do
  begin
    Println($'Кластер {cluster + 1}:');

    var clusterDf := df
      .Filter(r -> r.Int('cluster') = cluster)
      .SortBy('population', descending := True)
      .Select(['city', 'population', 'density'])
      .Head(5);

    clusterDf.Print;
    Println;
  end;
end.
