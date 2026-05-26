uses MLABC;

function BuildPipe(features: array of string): DataPipeline;
begin
  Result :=
    DataPipeline.Build(
      TaskKind.tkClassification,
      'Выжил',
      features,
      new Imputer(['Возраст']),
      new Imputer('Саутгемптон', ['ПортПосадки']),
      new OneHotEncoder('Пол'),
      new OneHotEncoder('ПортПосадки'),
      new StandardScaler,
      new RandomForestClassifier(nTrees := 100, maxDepth := 6, minSamplesLeaf := 3, minSamplesSplit := 6)
    );
end;

begin
  var ds := Datasets.TitanicRu;
  var features := ['Класс', 'Пол', 'Возраст', 'БратьяИСупруги', 'РодителиИДети', 'ЦенаБилета', 'ПортПосадки'];
  var df := ds.Data.Drop(['Id', 'Имя']);

  var (trainDf, testDf) :=
    df.StratifiedTrainTestSplit(ds.Target, testRatio := 0.2, seed := 42);

  var pipe := BuildPipe(features);
  pipe.Fit(trainDf);

  var pred := pipe.Predict(testDf);
  var y := pipe.GetEncodedLabels(testDf);
  var testAccuracy := Metrics.Accuracy(y, pred);

  var total := 0.0;
  var folds := Validation.StratifiedKFold(df.Int('Выжил'), 5, seed := 1);
  var foldsCount := 0;

  foreach var (trainIdx, testIdx) in folds do
  begin
    var foldTrain := df.TakeRows(trainIdx);
    var foldTest := df.TakeRows(testIdx);

    var foldPipe := BuildPipe(features);
    foldPipe.Fit(foldTrain);

    var foldPred := foldPipe.Predict(foldTest);
    var foldY := foldPipe.GetEncodedLabels(foldTest);

    total += Metrics.Accuracy(foldY, foldPred);
    foldsCount += 1;
  end;

  var cvAccuracy := total / foldsCount;

  Println('Оценка RandomForestClassifier двумя способами');
  Println($'Accuracy на тестовой выборке: {testAccuracy:F3}');
  Println($'Средняя Accuracy по кросс-валидации: {cvAccuracy:F3}');
end.
