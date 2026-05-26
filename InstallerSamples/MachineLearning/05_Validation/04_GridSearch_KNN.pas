// В этом примере подбирается число соседей k
// для KNNClassifier с помощью GridSearch.

uses MLABC;

begin
  var (X, y) := Datasets.MakeMoons(
    n := 400,
    noise := 0.18,
    seed := 42
  );

  var scaler := new StandardScaler;
  scaler.Fit(X);
  X := scaler.Transform(X);

  var kValues := [1, 3, 5, 7, 9, 11, 15];

  Println('Подбор параметра k для KNNClassifier');
  Println;
  Println('Проверяемые значения k:');

  foreach var k in kValues do
  begin
    var score := Validation.StratifiedCrossValidate(
      new KNNClassifier(k, KNNWeighting.Distance),
      X, y,
      5,
      ClassificationMetrics.Accuracy,
      seed := 42
    );

    Println($'  k = {k,2}  ->  средняя Accuracy = {score:F3}');
  end;

  var (bestK, bestScore, bestModel) := GridSearch.Search(
    k -> new KNNClassifier(k, KNNWeighting.Distance),
    kValues,
    X, y,
    5,
    ClassificationMetrics.Accuracy,
    maximize := True,
    stratified := True,
    seed := 42
  );

  Println;
  Println($'Лучшее значение k:   {bestK}');
  Println($'Лучшая средняя Accuracy:   {bestScore:F3}');
end.
