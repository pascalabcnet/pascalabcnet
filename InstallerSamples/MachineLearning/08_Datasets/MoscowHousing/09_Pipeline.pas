uses MLABC;

begin
  var ds := Datasets.MoscowHousing;
  var df := ds.Data;

  var features := ['rooms', 'area', 'kitchen_area', 'floor', 'floors_total', 'metro_minutes', 'renovation'];

  var target := 'price';

  var (trainDf, testDf) := df.TrainTestSplit(0.2, 42);

  var pipe :=
    DataPipeline.Build(
      target,
      features,
      new OneHotEncoder('renovation'),
      new StandardScaler,
      new LinearRegression
    );

  pipe.Fit(trainDf);

  var pred := pipe.Predict(testDf);
  var y := testDf.ToVector(target);

  Println('R²:', Metrics.R2(y, pred):0:3);
end.