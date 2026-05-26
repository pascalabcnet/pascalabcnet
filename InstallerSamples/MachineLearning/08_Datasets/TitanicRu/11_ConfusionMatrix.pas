uses MLABC, PlotML;

begin
  var ds := Datasets.TitanicRu;
  var features := ['Класс', 'Пол', 'Возраст', 'БратьяИСупруги', 'РодителиИДети', 'ЦенаБилета', 'ПортПосадки'];
  var df := ds.Data.Drop(['Id', 'Имя']);

  var (trainDf, testDf) :=
    df.StratifiedTrainTestSplit(ds.Target, testRatio := 0.2, seed := 42);

  var pipe :=
    DataPipeline.Build(
      TaskKind.tkClassification,
      ds.Target,
      features,
      new Imputer(['Возраст']),
      new Imputer('Саутгемптон', ['ПортПосадки']),
      new OneHotEncoder('Пол'),
      new OneHotEncoder('ПортПосадки'),
      new StandardScaler,
      new LogisticRegression(learningRate := 0.1, epochs := 2000)
    );

  pipe.Fit(trainDf);

  var pred := pipe.Predict(testDf);
  var y := pipe.GetEncodedLabels(testDf);
  var cm := new ConfusionMatrix(y, pred);
  var acc := Metrics.Accuracy(y, pred);

  Plot.ConfusionMatrix(cm, ['не выжил', 'выжил']);
  Plot.Title := $'TitanicRu: DataPipeline, accuracy = {acc:F3}';
  Plot.XLabel := 'Предсказанный класс';
  Plot.YLabel := 'Истинный класс';
end.
