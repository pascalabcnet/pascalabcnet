// В этом примере сравниваются три модели на нелинейной задаче:
// линейная регрессия, дерево решений и градиентный бустинг.
//
// Градиентный бустинг строит много небольших деревьев последовательно.
// Каждое следующее дерево старается исправить ошибки предыдущих,
// поэтому на сложных зависимостях такая модель часто работает очень хорошо.

uses MLABC;

begin
  var (X, y) := Datasets.MakeRegression(
    n := 300,
    nFeatures := 8,
    nInformative := 2,
    noise := 0.35,
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

  var tree := new DecisionTreeRegressor(maxDepth := 6, minSamplesSplit := 10, minSamplesLeaf := 5);
  tree.Fit(XTrain, yTrain);

  var boosting := new GradientBoostingRegressor(
    nEstimators := 120,
    learningRate := 0.1,
    maxDepth := 3,
    minSamplesSplit := 10,
    minSamplesLeaf := 5,
    subsample := 1.0,
    seed := 42
  );
  boosting.Fit(XTrain, yTrain);

  var yPredLinear := linear.Predict(XTest);
  var yPredTree := tree.Predict(XTest);
  var yPredBoosting := boosting.Predict(XTest);

  var r2Linear := RegressionMetrics.R2(yTest, yPredLinear);
  var r2Tree := RegressionMetrics.R2(yTest, yPredTree);
  var r2Boosting := RegressionMetrics.R2(yTest, yPredBoosting);

  Println('Сравнение моделей на нелинейной зависимости');
  Println;
  Println($'Линейная регрессия:   R² = {r2Linear:F3}');
  Println($'Дерево решений:       R² = {r2Tree:F3}');
  Println($'Градиентный бустинг:  R² = {r2Boosting:F3}');
  Println;
  Println('Интерпретация результата:');
  Println('- Линейная модель плохо подходит для сложной нелинейной зависимости.');
  Println('- Одно дерево решений уже может уловить форму данных.');
  Println('- Градиентный бустинг последовательно исправляет ошибки и часто даёт самый точный прогноз.');
end.
