// В этом примере MatrixPipeline используется
// для задачи регрессии.

uses MLABC;

begin
  var ds := Datasets.MoscowHousing;

  var features := ['area', 'rooms', 'floor', 'max_floor', 'distance_to_metro', 'building_age'];

  var (trainDs, testDs) := ds.TrainTestSplit(testRatio := 0.2, seed := 42);

  var XTrain := trainDs.Data.ToMatrix(features);
  var yTrain := trainDs.Data.ToVector(ds.Target);

  var XTest := testDs.Data.ToMatrix(features);
  var yTest := testDs.Data.ToVector(ds.Target);

  var pipe :=
    MatrixPipeline.Build(
      new StandardScaler,
      new LinearRegression
    );

  pipe.Fit(XTrain, yTrain);
  var pred := pipe.Predict(XTest);
  var r2 := RegressionMetrics.R2(yTest, pred);

  Println('MatrixPipeline для задачи регрессии');
  Println;
  Println($'R² на тестовой выборке: {r2:F3}');
end.
