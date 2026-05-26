uses MLABC;

begin
  var ds := Datasets.TitanicRu;
  var df := ds.Data.Drop(['Id', 'Имя']);

  var features := ['Класс', 'Пол', 'Возраст', 'БратьяИСупруги', 'РодителиИДети', 'ЦенаБилета', 'ПортПосадки'];
  var target := 'Выжил';

  var (trainDf, testDf) :=
    df.StratifiedTrainTestSplit(ds.Target, testRatio := 0.2, seed := 42);

  var prep :=
    DataPipeline.BuildPreprocessing(
      TaskKind.tkClassification,
      target,
      features,
      new Imputer(['Возраст']),
      new Imputer('Саутгемптон', ['ПортПосадки']),
      new OneHotEncoder('Пол'),
      new OneHotEncoder('ПортПосадки'),
      new StandardScaler
    );

  var pipeLR := prep.WithModel(new LogisticRegression(learningRate := 0.01, epochs := 2000));
  pipeLR.Fit(trainDf);
  var predLR := pipeLR.Predict(testDf);
  var y := pipeLR.GetEncodedLabels(testDf);

  var pipeTree := prep.WithModel(new DecisionTreeClassifier(maxDepth := 5, minSamplesLeaf := 3, minSamplesSplit := 6));
  pipeTree.Fit(trainDf);
  var predTree := pipeTree.Predict(testDf);

  var pipeForest := prep.WithModel(new RandomForestClassifier(nTrees := 100, maxDepth := 6, minSamplesLeaf := 3, minSamplesSplit := 6));
  pipeForest.Fit(trainDf);
  var predForest := pipeForest.Predict(testDf);

  Println('Сравнение моделей на TitanicRu');
  Println($'LogisticRegression:      Accuracy = {Metrics.Accuracy(y, predLR):F3}');
  Println($'DecisionTreeClassifier:  Accuracy = {Metrics.Accuracy(y, predTree):F3}');
  Println($'RandomForestClassifier:  Accuracy = {Metrics.Accuracy(y, predForest):F3}');
end.
