uses MLABC;

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

  Println('Классификация выживания на Титанике (логистическая регрессия)');
  Println($'Accuracy = {Metrics.Accuracy(y, pred):F3}');
end.
