// В этом примере сравниваются два алгоритма кластеризации:
// KMeans и DBSCAN.
//
// KMeans хорошо работает, когда кластеры компактны и похожи по форме.
// DBSCAN лучше справляется со сложной геометрией кластеров и не требует
// заранее задавать их число.

uses MLABC;

begin
  var (X, yTrue) := Datasets.MakeMoons(
    n := 400,
    noise := 0.08,
    shuffle := True,
    seed := 1
  );

  var kmeans := new KMeans(2, seed := 42);
  kmeans.Fit(X);
  var yKMeans := kmeans.PredictLabels(X);

  var dbscan := new DBSCAN(0.22, 5);
  dbscan.Fit(X);
  var yDBSCAN := dbscan.PredictLabels(X);

  var ariKMeans := ClusteringMetrics.AdjustedRandIndex(yTrue, yKMeans);
  var ariDBSCAN := ClusteringMetrics.AdjustedRandIndex(yTrue, yDBSCAN);

  Println('Сравнение KMeans и DBSCAN');
  Println;
  Println($'Число кластеров у KMeans:   {kmeans.ClustersCount}');
  Println($'Число кластеров у DBSCAN:   {dbscan.ClustersCount}');
  Println;
  Println($'KMeans:  ARI = {ariKMeans:F3}');
  Println($'DBSCAN:  ARI = {ariDBSCAN:F3}');
  Println;
  Println('Интерпретация результата:');
  Println('- KMeans разбивает данные на компактные области вокруг центров.');
  Println('- DBSCAN умеет находить кластеры сложной формы.');
  Println('- Для двух "лун" DBSCAN обычно лучше отражает реальную структуру данных.');
end.
