// Метрики помогают сравнить разные варианты кластеризации.
//
// В этом примере данные естественно делятся на 3 группы.
// Мы сравниваем несколько значений k для KMeans и смотрим,
// как меняются ARI, Silhouette, Calinski-Harabasz и Davies-Bouldin.

uses MLABC;

begin
  var (X, yTrue) := Datasets.MakeBlobs(
    n := 300,
    centers := 3,
    nFeatures := 2,
    clusterStd := 0.8,
    centerBox := 6.0,
    shuffle := True,
    seed := 42
  );

  Println('Сравнение вариантов кластеризации');
  Println;
  Println('k    ARI    Silhouette    Calinski-Harabasz    Davies-Bouldin');
  Println('----------------------------------------------------------------');

  foreach var k in [2, 3, 4, 5] do
  begin
    var model := new KMeans(k, seed := 42);
    var labels := model.FitPredict(X);

    var ari := ClusteringMetrics.AdjustedRandIndex(yTrue, labels);
    var sil := ClusteringMetrics.SilhouetteScore(X, labels);
    var ch := ClusteringMetrics.CalinskiHarabaszScore(X, labels);
    var db := ClusteringMetrics.DaviesBouldinScore(X, labels);

    Println($'{k,1}   {ari,5:F3}      {sil,5:F3}             {ch,8:F1}           {db,5:F3}');
  end;
end.
