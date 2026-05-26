// В этом примере GradientBoostingClassifier возвращает
// вероятности принадлежности к каждому классу.

uses MLABC;

begin
  var ds := Datasets.Iris;
  var (trainDs, testDs) := ds.StratifiedTrainTestSplit(testRatio := 0.2, seed := 42);

  var pipe :=
    DataPipeline.Build(
      TaskKind.tkClassification,
      ds.Target,
      ds.Features,
      new GradientBoostingClassifier(
        nEstimators := 80,
        learningRate := 0.1,
        maxDepth := 3,
        minSamplesSplit := 6,
        minSamplesLeaf := 3,
        seed := 42
      )
    );

  pipe.Fit(trainDs.Data);

  var pred := pipe.Predict(testDs.Data);
  var yTest := pipe.GetEncodedLabels(testDs.Data);
  var acc := ClassificationMetrics.Accuracy(yTest, pred);

  var proba := pipe.PredictProba(testDs.Data);
  var classes := pipe.GetClassLabels;

  Println('Градиентный бустинг с вероятностями классов');
  Println;
  Println($'Точность: {acc:F3}');
  Println;
  Println('Вероятности классов для первого объекта тестовой выборки:');

  for var j := 0 to classes.Length - 1 do
    Println($'  {classes[j]}: {proba[0, j]:F3}');
end.
