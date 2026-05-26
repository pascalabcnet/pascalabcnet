// В этом примере сравниваются две модели на нелинейной задаче:
// линейная регрессия и дерево решений для регрессии.
//
// Линейная модель умеет строить только одну общую прямую зависимость.
// Дерево решений разбивает пространство признаков на области
// и в каждой области даёт свой локальный прогноз.

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

  // При minSamplesLeaf = 5 узел должен содержать хотя бы 10 объектов,
  // чтобы его можно было разделить на два листа не меньше чем по 5 объектов.
  var tree := new DecisionTreeRegressor(maxDepth := 6, minSamplesSplit := 10, minSamplesLeaf := 5);
  tree.Fit(XTrain, yTrain);

  var yPredLinear := linear.Predict(XTest);
  var yPredTree := tree.Predict(XTest);

  var r2Linear := RegressionMetrics.R2(yTest, yPredLinear);
  var r2Tree := RegressionMetrics.R2(yTest, yPredTree);

  Println('Сравнение моделей на нелинейной зависимости');
  Println;
  Println($'Линейная регрессия: R² = {r2Linear:F3}');
  Println($'Дерево решений:     R² = {r2Tree:F3}');
  Println;
  Println('Интерпретация результата:');
  Println('- Линейная регрессия пытается описать всю зависимость одной формулой.');
  Println('- Дерево решений умеет подстраиваться под разные участки данных.');
  Println('- Поэтому на нелинейной задаче дерево обычно работает лучше.');
end.
