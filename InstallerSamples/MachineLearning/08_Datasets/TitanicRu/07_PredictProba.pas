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
      'Выжил',
      features,
      new Imputer(['Возраст']),
      new Imputer('Саутгемптон', ['ПортПосадки']),
      new OneHotEncoder('Пол'),
      new OneHotEncoder('ПортПосадки'),
      new StandardScaler,
      new LogisticRegression(learningRate := 0.01, epochs := 2000)
    );

  pipe.Fit(trainDf);

  var pred := pipe.Predict(testDf);
  var y := pipe.GetEncodedLabels(testDf);
  var proba := pipe.PredictProba(testDf);
  var classes := pipe.GetClassLabels;

  Println('TitanicRu: вероятности классов');
  Println($'Accuracy = {Metrics.Accuracy(y, pred):F3}');
  Println;
  Println('Вероятности для первого пассажира тестовой выборки:');

  for var j := 0 to classes.Length - 1 do
    Println($'  {classes[j]}: {proba[0, j]:F3}');
end.
