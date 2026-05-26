// В этом примере сравниваются три модели на нелинейной задаче:
// линейная регрессия, дерево решений и случайный лес.
//
// Случайный лес усредняет предсказания многих деревьев.
// Благодаря этому он обычно работает устойчивее и точнее,
// чем одно дерево решений, особенно если в данных есть шум
// и лишние признаки.
//
// В этой задаче для леса используется режим AllFeatures.
// Для регрессии это часто даёт более сильный результат,
// чем случайный выбор только части признаков в каждом узле.

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

  var tree := new DecisionTreeRegressor(maxDepth := 8, minSamplesSplit := 10, minSamplesLeaf := 5);
  tree.Fit(XTrain, yTrain);

  var forest := new RandomForestRegressor(
    nTrees := 150,
    maxDepth := 8,
    minSamplesSplit := 10,
    minSamplesLeaf := 5,
    maxFeaturesMode := TMaxFeaturesMode.AllFeatures
  );
  forest.Fit(XTrain, yTrain);

  var yPredLinear := linear.Predict(XTest);
  var yPredTree := tree.Predict(XTest);
  var yPredForest := forest.Predict(XTest);

  var r2Linear := RegressionMetrics.R2(yTest, yPredLinear);
  var r2Tree := RegressionMetrics.R2(yTest, yPredTree);
  var r2Forest := RegressionMetrics.R2(yTest, yPredForest);

  Println('Сравнение моделей на нелинейной зависимости');
  Println;
  Println($'Линейная регрессия: R² = {r2Linear:F3}');
  Println($'Дерево решений:     R² = {r2Tree:F3}');
  Println($'Случайный лес:      R² = {r2Forest:F3}');
  Println;
  Println('Интерпретация результата:');
  Println('- Линейная модель плохо подходит для сложной нелинейной зависимости.');
  Println('- Одно дерево решений уже может уловить форму данных.');
  Println('- Случайный лес усредняет много деревьев и обычно даёт более устойчивый прогноз.');
end.
