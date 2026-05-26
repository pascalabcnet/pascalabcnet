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

  var pipeTree := prep.WithModel(new DecisionTreeRegressor(12));

  pipeTree.Fit(trainDf);
  var predTree := pipeTree.Predict(testDf);

  var pipeForest := prep.WithModel(new RandomForestRegressor(12, 50));

  pipeForest.Fit(trainDf);
  var predForest := pipeForest.Predict(testDf);

  var y := testDf.ToVector(target);

  Println('Сравнение моделей регрессии');
  Println($'DecisionTreeRegressor:  R² = {Metrics.R2(y, predTree):F3}');
  Println($'RandomForestRegressor:  R² = {Metrics.R2(y, predForest):F3}');
end.
