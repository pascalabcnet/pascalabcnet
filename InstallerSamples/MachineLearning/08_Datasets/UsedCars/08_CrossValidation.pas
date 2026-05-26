uses MLABC;

begin
  var ds := Datasets.UsedCarsPrice;
  var df := ds.Data;

  var features := [
    'model',
    'year',
    'transmission',
    'mileage_km',
    'fuelType',
    'l_100km',
    'engineSize',
    'Make'
  ];

  var target := 'price_k_rub';
  var (trainDf, testDf) := df.TrainTestSplit(0.2, seed := 42);

  var prep :=
    DataPipeline.BuildPreprocessing(
      TaskKind.tkRegression,
      target,
      features,
      new OrdinalEncoder('model'),
      new OneHotEncoder('transmission'),
      new OneHotEncoder('fuelType'),
      new OneHotEncoder('Make'),
      new StandardScaler
    );

  var pipe := prep.WithModel(new RandomForestRegressor(12, 50));
  pipe.Fit(trainDf);

  var pred := pipe.Predict(testDf);
  var y := testDf.ToVector(target);
  var testR2 := Metrics.R2(y, pred);

  var total := 0.0;
  var folds := Validation.KFold(df.RowCount, 5, seed := 1);
  var foldsCount := 0;

  foreach var (trainIdx, testIdx) in folds do
  begin
    var foldTrain := df.TakeRows(trainIdx);
    var foldTest := df.TakeRows(testIdx);

    var foldPipe := prep.WithModel(new RandomForestRegressor(12, 50));
    foldPipe.Fit(foldTrain);

    var foldPred := foldPipe.Predict(foldTest);
    var foldY := foldTest.ToVector(target);

    total += Metrics.R2(foldY, foldPred);
    foldsCount += 1;
  end;

  var cvR2 := total / foldsCount;

  Println('Оценка RandomForestRegressor двумя способами');
  Println($'R² на тестовой выборке: {testR2:F3}');
  Println($'Средний R² по кросс-валидации: {cvR2:F3}');
end.
