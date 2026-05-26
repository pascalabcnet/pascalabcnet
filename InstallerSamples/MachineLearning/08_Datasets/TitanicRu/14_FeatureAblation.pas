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

function TestAccuracy(df: DataFrame; features: array of string): real;
begin
  var (trainDf, testDf) :=
    df.StratifiedTrainTestSplit('Выжил', testRatio := 0.2, seed := 42);

  var pipe := BuildPipe(features);
  pipe.Fit(trainDf);

  var pred := pipe.Predict(testDf);
  var y := pipe.GetEncodedLabels(testDf);
  Result := Metrics.Accuracy(y, pred);
end;

begin
  var ds := Datasets.TitanicRu;
  var df := ds.Data.Drop(['Id', 'Имя']);

  var allFeatures :=
    SetOf('Класс', 'Пол', 'Возраст', 'БратьяИСупруги', 'РодителиИДети', 'ЦенаБилета', 'ПортПосадки');
  var noSex := (allFeatures - ['Пол']).ToArray;
  var noClass := (allFeatures - ['Класс']).ToArray;

  var accAll := TestAccuracy(df, allFeatures.ToArray);
  var accNoSex := TestAccuracy(df, noSex);
  var accNoClass := TestAccuracy(df, noClass);

  Println('TitanicRu: влияние признаков на качество модели');
  Println;
  Println('Идея примера: убираем один важный признак и смотрим, насколько хуже становится классификация.');
  Println;
  Println($'Все признаки: Accuracy = {accAll:F3}');
  Println($'Без признака "Пол": Accuracy = {accNoSex:F3}');
  Println($'Без признака "Класс": Accuracy = {accNoClass:F3}');
end.
