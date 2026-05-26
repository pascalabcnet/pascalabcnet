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
      new RandomForestClassifier(nTrees := 100, maxDepth := 6, minSamplesLeaf := 3, minSamplesSplit := 6)
    );

  pipe.Fit(trainDf);

  var pred := pipe.Predict(testDf);
  var y := pipe.GetEncodedLabels(testDf);

  Println('TitanicRu: Precision, Recall и F1');
  Println;
  Println('Precision — какая доля пассажиров, предсказанных как "выжил", действительно выжила.');
  Println('Recall — какую долю реально выживших пассажиров модель нашла.');
  Println('F1 — общий баланс между Precision и Recall.');
  Println;
  Println($'Accuracy  = {Metrics.Accuracy(y, pred):F3}');
  Println($'Precision = {Metrics.Precision(y, pred):F3}');
  Println($'Recall    = {Metrics.Recall(y, pred):F3}');
  Println($'F1        = {Metrics.F1(y, pred):F3}');
end.
