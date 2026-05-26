// Confusion matrix для Iris с использованием DataPipeline.
// Рекомендуемый стиль - он проще и явно показывает намерения

uses MLABC, PlotML;

begin
  var ds := Datasets.Iris;
  var (train, test) := ds.StratifiedTrainTestSplit(0.2, 42);

  var pipe := DataPipeline.Build(
    TaskKind.tkClassification,
    ds.Target,
    ds.Features,
    new StandardScaler,
    new LogisticRegression
  );

  pipe.Fit(train.Data);

  var pred := pipe.Predict(test.Data);
  var ytest := pipe.GetEncodedLabels(test.Data);
  var cm := new ConfusionMatrix(ytest, pred);
  var acc := Metrics.Accuracy(ytest, pred);

  Plot.ConfusionMatrix(cm, pipe.GetClassLabels{, normalize := MatrixNormalization.Rows});
  Plot.Title := $'Iris: DataPipeline, accuracy = {acc:F3}';
  Plot.XLabel := 'Предсказанный класс';
  Plot.YLabel := 'Истинный класс';
end.
