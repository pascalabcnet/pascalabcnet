// Полный пример задачи классификации:
// загружаем датасет, делим его на выборки,
// обучаем pipeline и оцениваем качество.

uses MLABC;

begin
  var ds := Datasets.Iris;
  var (trainDs, testDs) := ds.StratifiedTrainTestSplit(testRatio := 0.3, seed := 42);

  var pipe := DataPipeline.Build(
    TaskKind.tkClassification,
    ds.Target,
    ds.Features,
    new StandardScaler,
    new LogisticRegression(learningRate := 0.05, epochs := 1000)
  );

  pipe.Fit(trainDs.Data);

  var pred := pipe.Predict(testDs.Data);
  var yTest := pipe.GetEncodedLabels(testDs.Data);
  var acc := ClassificationMetrics.Accuracy(yTest, pred);

  Println('Классификация Iris: полный пример');
  Println;
  Println($'Accuracy = {acc:F3}');
end.
