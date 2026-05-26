// В этом примере сравниваются две модели на нелинейной задаче классификации:
// LogisticRegression и DecisionTreeClassifier.

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
  var logregAcc := ClassificationMetrics.Accuracy(yTest, logreg.Predict(XTest));

  var tree := new DecisionTreeClassifier(
    maxDepth := 5,
    minSamplesSplit := 6,
    minSamplesLeaf := 3,
    seed := 42
  );
  tree.Fit(XTrain, yTrain);
  var treeAcc := ClassificationMetrics.Accuracy(yTest, tree.Predict(XTest));

  Println('Сравнение моделей на нелинейной задаче классификации');
  Println;
  Println($'Логистическая регрессия:  Accuracy = {logregAcc:F3}');
  Println($'Дерево решений:          Accuracy = {treeAcc:F3}');
end.
