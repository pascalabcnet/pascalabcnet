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

  var pipe :=
    DataPipeline.Build(
      TaskKind.tkRegression,
      target,
      features,
      new OrdinalEncoder('model'),
      new OneHotEncoder('transmission'),
      new OneHotEncoder('fuelType'),
      new OneHotEncoder('Make'),
      new StandardScaler,
      new DecisionTreeRegressor(10)
    );


  pipe.Fit(trainDf);

  var pred := pipe.Predict(testDf);
  var y := testDf.ToVector(target);

  Println('Прогнозирование цены автомобилей (DecisionTreeRegressor)');
  Println($'MAE  = {Metrics.MAE(y, pred):F0}');
  Println($'RMSE = {Metrics.RMSE(y, pred):F0}');
  Println($'R²   = {Metrics.R2(y, pred):F3}');
end.
