// В этом примере сравниваются две регрессионные модели:
// линейная регрессия и KNNRegressor.
//
// Данные специально сделаны нелинейными. Для такой задачи
// LinearRegression обычно слишком проста, а KNNRegressor
// может лучше учитывать локальную форму зависимости.
//
// Если R² отрицателен, это означает, что модель работает
// даже хуже, чем очень простой прогноз по среднему значению.

uses MLABC;

begin
  var (X, y) := Datasets.MakeRegression(
    n := 300,
    nFeatures := 2,
    nInformative := 2,
    noise := 0.15,
    coefScale := 1.0,
    bias := 0.0,
    nonlinearStrength := 4.0,
    shuffle := True,
    seed := 42
  );

  var (XTrain, XTest, yTrain, yTest) := Validation.TrainTestSplit(
    X, y, testRatio := 0.3, seed := 42
  );

  var linear := new LinearRegression;
  linear.Fit(XTrain, yTrain);

  var knn := new KNNRegressor(7, KNNWeighting.Distance);
  knn.Fit(XTrain, yTrain);

  var yPredLinear := linear.Predict(XTest);
  var yPredKNN := knn.Predict(XTest);

  var r2Linear := RegressionMetrics.R2(yTest, yPredLinear);
  var r2KNN := RegressionMetrics.R2(yTest, yPredKNN);

  Println('Сравнение моделей на нелинейной зависимости');
  Println;
  Println($'Линейная регрессия: R² = {r2Linear:F3}');
  Println($'KNN-регрессия:      R² = {r2KNN:F3}');
  Println;
  Println('Интерпретация результата:');
  Println('- Линейная регрессия здесь слишком проста для задачи.');
  Println('- Отрицательный R² означает, что её прогноз хуже среднего значения.');
  Println('- KNN-регрессия лучше описывает локальную нелинейную зависимость.');
end.
