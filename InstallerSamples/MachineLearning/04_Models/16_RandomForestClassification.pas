// В этом примере сравниваются две модели на нелинейной задаче классификации:
// LogisticRegression и RandomForestClassifier.
//
// Логистическая регрессия строит линейную границу между классами.
// Поэтому на вложенных окружностях она принципиально ограничена.
//
// Случайный лес умеет учитывать более сложную структуру данных
// и обычно лучше справляется с нелинейной геометрией.

uses MLABC;

begin
  var (X, y) := Datasets.MakeCircles(
    n := 450,
    noise := 0.18,
    factor := 0.5,
    classBalance := 0.5,
    flipProb := 0.04,
    scale := 3.0,
    seed := 42
  );

  var (XTrain, XTest, yTrain, yTest) := Validation.TrainTestSplit(X, y, 0.25, seed := 42);

  var logreg := new LogisticRegression(learningRate := 0.05, epochs := 1000);
  logreg.Fit(XTrain, yTrain);
  var logregPred := logreg.Predict(XTest);
  var logregAcc := ClassificationMetrics.Accuracy(yTest, logregPred);

  var forest := new RandomForestClassifier(
    nTrees := 150,
    maxDepth := 8,
    minSamplesSplit := 6,
    minSamplesLeaf := 3,
    seed := 42
  );
  forest.Fit(XTrain, yTrain);
  var forestPred := forest.Predict(XTest);
  var forestAcc := ClassificationMetrics.Accuracy(yTest, forestPred);

  Println('Сравнение моделей на нелинейной задаче классификации');
  Println;
  Println($'Логистическая регрессия:  Accuracy = {logregAcc:F3}');
  Println($'Случайный лес:            Accuracy = {forestAcc:F3}');
  Println;
  Println('Интерпретация результата:');
  Println('- Логистическая регрессия строит только линейную границу.');
  Println('- Случайный лес лучше улавливает нелинейную форму классов.');
end.
