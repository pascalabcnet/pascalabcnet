// Сравнение KMeans и DBSCAN на данных с шумом и разной плотностью.
//
// Оба алгоритма находят одинаковое число кластеров (k = 5),
// поэтому сравнение по ARI является корректным.
//
// KMeans предполагает компактные и одинаковые по форме кластеры,
// а также чувствителен к шуму, что снижает качество разбиения.
//
// DBSCAN выделяет кластеры как области высокой плотности
// и автоматически игнорирует шумовые точки (label = -1),
// благодаря чему лучше восстанавливает структуру данных.
//
// В результате DBSCAN показывает более высокий ARI,
// так как точнее отражает реальную геометрию кластеров.

// KMeans  ARI: 0.8973
// DBSCAN  ARI: 0.9496

uses MLABC, PlotML;

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

  // --- модели
  var kmeans := new KMeans(5);
  kmeans.Fit(X);
  var yKM := kmeans.PredictLabels(X);
  kmeans.ClustersCount.Println;

  var db := new DBSCAN(0.7, 5);
  db.Fit(X);
  var yDB := db.PredictLabels(X);
  db.ClustersCount.Println;

  // --- метрики
  var ariKM := Metrics.AdjustedRandIndex(yTrue, yKM);
  var ariDB := Metrics.AdjustedRandIndex(yTrue, yDB);

  Println('--- Clustering ---');
  Println($'KMeans  ARI: {ariKM,0:F4}');
  Println($'DBSCAN  ARI: {ariDB,0:F4}');
  Println;

  // --- координаты
  var x1 := X.Col(0);
  var x2 := X.Col(1);

  // --- визуализация
  var fig := Plot.Grid(1, 2);

  // KMeans
  //fig[0,0].SetPalette(Palettes.Bright);
  fig[0,0].Points(x1, x2, yKM, size := 6);
  fig[0,0].Title := $'KMeans (ARI={ariKM,0:F3})';

  var yDB_plot := new integer[yDB.Length];
  
  for var i := 0 to yDB.Length - 1 do
    yDB_plot[i] := if yDB[i] = -1 then 0 else yDB[i] + 1;
  // DBSCAN
  //fig[0,1].SetPalette(Palettes.Bright);
  fig[0,1].Points(x1, x2, yDB_plot, size := 6);
  fig[0,1].Title := $'DBSCAN (ARI={ariDB,0:F3})';
end.