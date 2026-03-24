// Сравнение KMeans и DBSCAN на данных с шумом и разной плотностью кластеров.
//
// Датасет содержит:
// • несколько кластеров
// • различную плотность
// • дополнительные шумовые точки
//
// KMeans:
// • предполагает компактные и примерно одинаковые по размеру кластеры
// • плохо работает при наличии шума и различной плотности
// • стремится "размазать" шум по кластерам
//
// DBSCAN:
// • выделяет кластеры как области высокой плотности
// • естественно отделяет шум (метка -1)
// • лучше справляется с неоднородной структурой данных
//
// По метрике ARI DBSCAN показывает лучший результат,
// так как точнее восстанавливает истинную структуру кластеров.
//
// Этот пример демонстрирует различие двух подходов:
// • центроидный (KMeans)
// • плотностный (DBSCAN)

// KMeans  ARI: 0.9073
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
  var kmeans := new KMeans(3);
  kmeans.Fit(X);
  var yKM := kmeans.PredictLabels(X);

  var db := new DBSCAN(0.7, 5);
  db.Fit(X);
  var yDB := db.PredictLabels(X);

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