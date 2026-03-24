// Подбор параметра eps для DBSCAN с использованием фильтрации и составного скоринга.
//
// Рассматривались различные значения eps и для каждого вычислялись:
// • число кластеров (k)
// • доля крупнейшего кластера (max%)
// • доля шума (noise%)
// • silhouette
//
// Наблюдения:
//
// • При увеличении eps происходит слияние кластеров.
//   Уже при eps ≥ 0.6 практически все объекты попадают в один доминирующий кластер
//   (max% ≈ 0.97–0.99), что делает разбиение вырожденным.
//
// • Несмотря на высокий silhouette (например, при eps = 0.8),
//   такие решения не являются содержательно корректными,
//   так как фактически отражают почти однородную структуру данных.
//
// • Фильтрация отбрасывает варианты с:
//   – одним кластером,
//   – слишком большим доминирующим кластером,
//   – некорректным уровнем шума.
//
// • В результате единственным допустимым вариантом оказался eps = 0.2.
//
// Интерпретация результата:
//
// • При eps = 0.2 алгоритм выделяет несколько кластеров (k = 5),
//   однако структура получается фрагментированной:
//   присутствует один крупный кластер и несколько очень малых,
//   а также заметная доля шума.
//
// • Это говорит о том, что:
//   – DBSCAN плохо согласуется с геометрией данного датасета,
//   – либо признаки недостаточно разделимы по плотности,
//   – либо требуется более аккуратный подбор параметров (eps, minPts),
//     или использование другой модели кластеризации.
//
// Вывод:
//
// • DBSCAN в данном виде не даёт устойчивого и интерпретируемого разбиения.
// • Жёсткая фильтрация позволяет избежать вырожденных решений,
//   но может приводить к выбору некачественного eps.
// • Для данного датасета более предпочтительны методы типа KMeans,
//   либо требуется дополнительная предобработка признаков.
uses MLABC;

begin
  var ds := Datasets.RussianCities;
  var df := ds.Data;

  df := df.WithColumnFloat('density', r -> r.Float('population') / r.Float('area'));
  df := df.WithColumnFloat('log_population', r -> Ln(r.Float('population')));
  df := df.WithColumnInt('age', row -> 2025 - row.Int('foundation_year'));

  var features := ['log_population', 'density'];

  Println('Подбор eps для DBSCAN (с фильтрацией)');
  Println;
  Println('eps    k    max%    noise%    silhouette    score');
  Println('-' * 70);

  var X := df.ToMatrix(features);
  
  var scaler := new StandardScaler;
  scaler.Fit(X);
  X := scaler.Transform(X);

  var bestEps := -1.0;
  var bestScore := -1.0;

  foreach var eps in [0.2,0.3,0.4,0.5,0.6,0.7,0.8,0.9,1.0] do
  begin
    var model := new DBSCAN(eps, 5);
    model.Fit(X);
  
    var labels := model.PredictLabels(X);
  
    var counts := new Dictionary<integer, integer>();
    foreach var l in labels do
      counts[l] := counts.Get(l, 0) + 1;
  
    var n := df.RowCount;

    // --- считаем кластеры (без шума)
    var k := 0;
    var maxCluster := 0;
    var noise := counts.Get(-1, 0);

    foreach var kv in counts do
      if kv.Key <> -1 then
      begin
        k += 1;
        if kv.Value > maxCluster then
          maxCluster := kv.Value;
      end;

    var maxFrac := maxCluster / n;
    var noiseFrac := noise / n;

    var sil := -1.0;
    if k >= 2 then
      sil := Metrics.SilhouetteScore(X, labels);

    // --- фильтрация плохих случаев
    var valid :=
      (k >= 2) and (k <= 5) and
      (maxFrac < 0.85) and
      (noiseFrac > 0.01) and (noiseFrac < 0.4);

    var score := -1.0;

    if valid then
    begin
      // составной скоринг
      score :=
        sil
        - 0.5 * maxFrac        // штраф за доминирующий кластер
        - 0.3 * noiseFrac;     // штраф за шум

      if score > bestScore then
      begin
        bestScore := score;
        bestEps := eps;
      end;
    end;

    Println($'{eps,4:F1} {k,4} {maxFrac,7:F2} {noiseFrac,8:F2} {sil,12:F4} {score,10:F4}');
  end;

  Println;
  Println($'Выбран eps: {bestEps,0:F2}');
  Println;

  // --- финальная модель
  var model := new DBSCAN(bestEps, 5);
  model.Fit(X);

  var labels := model.PredictLabels(X);
  df.AddIntColumn('cluster_db', labels, nil);

  // --- итог
  var counts := new Dictionary<integer, integer>();
  foreach var l in labels do
    counts[l] := counts.Get(l, 0) + 1;

  Println('Финальные кластеры:');
  foreach var kv in counts do
    Println($'Cluster {kv.Key}: {kv.Value}');
end.