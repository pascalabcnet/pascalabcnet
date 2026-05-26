// В этом примере подбирается параметр eps для DBSCAN.
//
// eps задаёт радиус окрестности точки.
// Если eps слишком мал, алгоритм считает много точек шумом.
// Если eps слишком велик, разные группы могут слиться в один кластер.

uses MLABC;

begin
  var (X, yTrue) := Datasets.MakeMoons(
    n := 400,
    noise := 0.08,
    shuffle := True,
    seed := 1
  );

  Println('Подбор параметра eps для DBSCAN');
  Println;
  Println('eps    кластеры   шум     ARI');
  Println('-' * 34);

  foreach var eps in [0.10, 0.14, 0.18, 0.22, 0.26, 0.30] do
  begin
    var model := new DBSCAN(eps, 5);
    model.Fit(X);

    var labels := model.PredictLabels(X);

    var noiseCount := 0;
    foreach var clusterLabel in labels do
      if clusterLabel = -1 then
        noiseCount += 1;

    var ari := ClusteringMetrics.AdjustedRandIndex(yTrue, labels);

    Println($'{eps,4:F2} {model.ClustersCount,10} {noiseCount,6} {ari,8:F3}');
  end;

  Println;
  Println('Интерпретация результата:');
  Println('- При слишком маленьком eps шумовых точек становится слишком много.');
  Println('- При слишком большом eps разные группы могут сливаться.');
  Println('- Хороший eps даёт разумное число кластеров и высокое значение ARI.');
end.
