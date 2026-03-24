// По log_population, density KMeans выделил три группы городов с разным масштабом и характером.
//
// Кластер 0 — крупнейшие и наиболее значимые города.
// Здесь сосредоточены мегаполисы и крупные региональные центры.
// Для них характерны большое население, сравнительно высокая плотность и в среднем более крупная площадь.
//
// Кластер 1 — компактные, но плотные города.
// Их немного, население у них умеренное, зато площадь очень мала, поэтому плотность населения максимальна.
// Это особая группа "сжатых" городов с городской концентрацией.
//
// Кластер 2 — малые и средние города.
// Это самый массовый кластер.
// Для него характерны небольшое население, низкая плотность и сравнительно умеренная площадь.
// Именно он отражает "обычный" фон большинства российских городов.

uses MLABC;

begin
  var ds := Datasets.RussianCities;
  var df := ds.Data;

  // --- признаки
  df := df.WithColumnFloat('density', row -> row.Float('population') / row.Float('area'));
  df := df.WithColumnInt('age', row -> 2025 - row.Int('foundation_year'));
  df := df.WithColumnFloat('log_population', row -> Ln(row.Float('population')));

  var features := ['log_population', 'density'{, 'lat', 'lon', 'age'}];

  Println('Подбор числа кластеров для KMeans');
  Println;
  Println('k    inertia         silhouette      calinski_harabasz    davies_bouldin');
  Println('-' * 78);

  var sils := new List<real>;
  var chs  := new List<real>;
  var dbis := new List<real>;
  
  foreach var k in 2..8 do
  begin
    var model := new KMeans(k, seed := 42);

    var pipe :=
      UDataPipeline.Build(
        features,
        new StandardScaler,
        model
      );

    pipe.Fit(df);

    var labels := pipe.PredictLabels(df);
    
    var dfT := pipe.Transform(df);
    var X := dfT.ToMatrix(features);

    var inertia := model.Inertia;
    var sil := Metrics.SilhouetteScore(X, labels);
    var ch := Metrics.CalinskiHarabaszScore(X, labels);
    var dbi := Metrics.DaviesBouldinScore(X, labels);
    
    sils.Add(sil);
    chs.Add(ch);
    dbis.Add(dbi);

    Println($'{k,-4} {inertia,12:F3} {sil,14:F4} {ch,20:F3} {dbi,18:F4}');
  end;
  
  // границы
  var sil_min := sils.Min; var sil_max := sils.Max;
  var ch_min  := chs.Min;  var ch_max  := chs.Max;
  var dbi_min := dbis.Min; var dbi_max := dbis.Max;
  
  // второй проход
  var bestScore := -1e9;
  var bestK := -1;
  
  for var i := 0 to sils.Count - 1 do
  begin
    var sil_norm := (sils[i] - sil_min) / (sil_max - sil_min);
    var ch_norm  := (chs[i]  - ch_min)  / (ch_max  - ch_min);
    var dbi_norm := (dbi_max - dbis[i]) / (dbi_max - dbi_min);
  
    var score := 0.5 * sil_norm + 0.3 * ch_norm + 0.2 * dbi_norm;
  
    var k := i + 2;
  
    if score > bestScore then
    begin
      bestScore := score;
      bestK := k;
    end;
  end;
  
  Println;
  Println($'Лучший k: {bestK}');
  Println;
  
  var model := new KMeans(bestK, seed := 42);

  // --- финальная модель
  var pipe :=
    UDataPipeline.Build(
      features,
      new StandardScaler,
      model
    );

  pipe.Fit(df);

  var labels := pipe.PredictLabels(df);
  var X := df.ToMatrix(features);

  df.AddIntColumn('cluster', labels, nil);

  Println('Финальные метрики для выбранного k:');
  Println($'Inertia:            {model.Inertia:F3}');
  Println($'SilhouetteScore:    {Metrics.SilhouetteScore(X, labels):F4}');
  Println($'CalinskiHarabasz:   {Metrics.CalinskiHarabaszScore(X, labels):F3}');
  Println($'DaviesBouldin:      {Metrics.DaviesBouldinScore(X, labels):F4}');
  Println;

  Println('Центры кластеров (в стандартизованном пространстве признаков):');
  model.ClusterCenters.Print;
  Println;

  // --- анализ кластеров
  for var c := 0 to bestK - 1 do
  begin
    var sub := df.Filter(r -> r.Int('cluster') = c);

    Println;
    Println('=' * 70);
    Println($'Кластер {c}');
    Println('=' * 70);

    Println($'Число городов: {sub.RowCount}');
    Println($'Среднее население: {sub.Mean(''population''),0:F3}');
    Println($'Средняя площадь:   {sub.Mean(''area''),0:F3}');
    Println($'Средняя плотность: {sub.Mean(''density''),0:F3}');
    Println($'Средний возраст:   {sub.Mean(''age''),0:F3}');

    Println;
    Println('Топ-10 по населению:');

    sub.SortBy('population', descending := True)
      .Select(['city', 'population', 'area', 'density', 'age'])
      .Head(10)
      .Print;
  end;
end.