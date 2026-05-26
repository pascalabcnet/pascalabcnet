// В этом примере сравнивается KNNClassifier
// без масштабирования и с MatrixPipeline.
//
// Важно: в pipeline шаг StandardScaler выполняется автоматически.
// То есть MatrixPipeline здесь эквивалентен цепочке:
// StandardScaler -> KNNClassifier.

uses MLABC;

begin
  var (X, y) := Datasets.MakeClassification(
    n := 400,
    nFeatures := 4,
    nInformative := 2,
    nRedundant := 0,
    noise := 0.15,
    classSep := 1.0,
    seed := 42
  );

  for var i := 0 to X.RowCount - 1 do
  begin
    X[i, 2] := X[i, 2] * 100;
    X[i, 3] := X[i, 3] * 1000;
  end;

  var (XTrain, XTest, yTrain, yTest) := Validation.TrainTestSplit(X, y, 0.25, seed := 42);

  var knn := new KNNClassifier(5);
  knn.Fit(XTrain, yTrain);
  var acc1 := ClassificationMetrics.Accuracy(yTest, knn.Predict(XTest));

  // Здесь масштабирование выполняется внутри pipeline автоматически.
  var pipe :=
    MatrixPipeline.Build(
      new StandardScaler,
      new KNNClassifier(5)
    );

  pipe.Fit(XTrain, yTrain);
  var acc2 := ClassificationMetrics.Accuracy(yTest, pipe.Predict(XTest));

  Println('KNNClassifier без масштабирования и с масштабированием');
  Println;
  Println($'Без масштабирования                : Accuracy = {acc1:F3}');
  Println($'С MatrixPipeline и масштабированием: Accuracy = {acc2:F3}');
end.
