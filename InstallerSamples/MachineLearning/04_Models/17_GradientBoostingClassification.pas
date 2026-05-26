// В этом примере сравниваются две модели на нелинейной задаче классификации:
// LogisticRegression и GradientBoostingClassifier.
//
// Логистическая регрессия строит линейную границу между классами.
// Градиентный бустинг последовательно улучшает ансамбль деревьев
// и хорошо справляется со сложной формой границы.

uses MLABC;

begin
  var (X, y) := Datasets.MakeMoons(
    n := 450,
    noise := 0.20,
    seed := 42
  );

  var (XTrain, XTest, yTrain, yTest) := Validation.TrainTestSplit(X, y, 0.25, seed := 42);

  var logreg := new LogisticRegression(learningRate := 0.05, epochs := 1000);
  logreg.Fit(XTrain, yTrain);
  var logregPred := logreg.Predict(XTest);
  var logregAcc := ClassificationMetrics.Accuracy(yTest, logregPred);

  var gb := new GradientBoostingClassifier(
    nEstimators := 120,
    learningRate := 0.1,
    maxDepth := 3,
    minSamplesSplit := 6,
    minSamplesLeaf := 3,
    seed := 42
  );
  gb.Fit(XTrain, yTrain);
  var gbPred := gb.Predict(XTest);
  var gbAcc := ClassificationMetrics.Accuracy(yTest, gbPred);

  Println('Сравнение моделей на нелинейной задаче классификации');
  Println;
  Println($'Логистическая регрессия:   Accuracy = {logregAcc:F3}');
  Println($'Градиентный бустинг:       Accuracy = {gbAcc:F3}');
  Println;
  Println('Интерпретация результата:');
  Println('- Логистическая регрессия строит только линейную границу.');
  Println('- Градиентный бустинг лучше улавливает сложную форму классов.');
end.
