// Подбор параметра eps для DBSCAN методом перебора с оценкой качества кластеризации.
//
// Для каждого eps вычисляются: число кластеров, доля крупнейшего кластера,
// доля шума и silhouette, после чего считается итоговый score.
//
// Score балансирует качество разделения (silhouette) и структуру кластеров,
// штрафуя решения с одним большим кластером или слишком большим шумом.
//
// Это позволяет избежать вырожденных случаев и выбрать eps,
// дающий наиболее устойчивое и интерпретируемое разбиение.
//
// Важно: перед применением DBSCAN данные масштабируются,
// так как значение eps зависит от масштаба признаков.

uses MLABC;

begin
  // --- данные (сложный случай: разная плотность + шум)
  var (X, yTrue) := Datasets.MakeBlobs(
    n := 400,
    centers := 3,
    nFeatures := 2,
    clusterStd := 0.9,
    clusterStdVar := 1.0,
    centerBox := 6.0,
    classBalance := 1.0,
    noisePoints := 60,
    shuffle := True,
    seed := 1
  );

  // --- подготовка данных
  
  var scaler := new StandardScaler;
  scaler.Fit(X);
  X := scaler.Transform(X);
  
  // --- сетка eps
  var epsList := Arr(0.2,0.3,0.4,0.5,0.6,0.7,0.8,0.9,1.0);
  
  var bestEps := -1.0;
  var bestScore := -1e9;
  
  Println('eps    k    max%    noise%    silhouette    score');
  Println('----------------------------------------------------------------------');
  
  foreach var eps in epsList do
  begin
    var model := new DBSCAN(eps, 5);
    model.Fit(X);
  
    var labels := model.PredictLabels(X);
  
    // --- статистика кластеров
    var counts := new Dictionary<integer, integer>();
    foreach var l in labels do
      counts[l] := counts.Get(l, 0) + 1;
  
    var n := X.RowCount;
  
    var k := 0;
    var maxCluster := 0;
    var noise := counts.Get(-1, 0);
  
    var sizes := new List<integer>;
  
    foreach var kv in counts do
      if kv.Key <> -1 then
      begin
        k += 1;
        sizes.Add(kv.Value);
        if kv.Value > maxCluster then
          maxCluster := kv.Value;
      end;
  
    sizes.Sort;
    sizes.Reverse;
  
    var secondCluster :=
      if sizes.Count >= 2 then sizes[1] else 0;
  
    var maxFrac := maxCluster / n;
    var noiseFrac := noise / n;
    var secondFrac := secondCluster / n;
  
    // --- silhouette
    var sil := -1.0;
    if k >= 2 then
      sil := Metrics.SilhouetteScore(X, labels);
  
    // --- нормализация silhouette (упрощённо)
    var silNorm := (sil + 1) / 2; // [-1,1] → [0,1]
  
    // --- штрафы (ключевая часть!)
    var penalty :=
        0.4 * maxFrac          // доминирующий кластер
      + 0.3 * noiseFrac        // шум
      + 0.3 * (1 - secondFrac);// слишком маленький второй кластер
  
    // --- штраф за плохое k
    if k < 2 then
      penalty += 0.5;
  
    // --- итоговый score
    var score := silNorm - penalty;
  
    if score > bestScore then
    begin
      bestScore := score;
      bestEps := eps;
    end;
  
    Println($'{eps,4:F1} {k,4} {maxFrac,7:F2} {noiseFrac,8:F2} {sil,12:F4} {score,10:F4}');
  end;
  
  Println;
  Println($'Выбран eps: {bestEps,0:F2}');
  Println;
end.