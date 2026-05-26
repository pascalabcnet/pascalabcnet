// В этом примере LogisticRegression возвращает не только класс,
// но и вероятности принадлежности к каждому классу.

uses MLABC;

begin
  var ds := Datasets.Iris;
  var (trainDs, testDs) := ds.StratifiedTrainTestSplit(testRatio := 0.2, seed := 42);

  var pipe :=
    DataPipeline.Build(
      TaskKind.tkClassification,
      ds.Target,
      ds.Features,
      new StandardScaler,
      new LogisticRegression(learningRate := 0.01, epochs := 2000)
    );

  pipe.Fit(trainDs.Data);

  var pred := pipe.Predict(testDs.Data);
  var yTest := pipe.GetEncodedLabels(testDs.Data);
  var acc := ClassificationMetrics.Accuracy(yTest, pred);

  var proba := pipe.PredictProba(testDs.Data);
  var classes := pipe.GetClassLabels;

  Println('Логистическая регрессия с вероятностями классов');
  Println;
  Println($'Точность: {acc:F3}');
  Println;
  Println('Вероятности классов для первого объекта тестовой выборки:');

  for var j := 0 to classes.Length - 1 do
    Println($'  {classes[j]}: {proba[0, j]:F3}');
end.
