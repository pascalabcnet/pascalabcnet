uses MLABC;

begin
  var ds := Datasets.MoscowHousing;
  var df := ds.Data;

  var features := ['rooms','area','kitchen_area','floor','floors_total','metro_minutes'];
  var target := 'price';

  var (trainDf, testDf) := df.TrainTestSplit(0.2, 42);

  var Xtrain := trainDf.ToMatrix(features);
  var ytrain := trainDf.ToVector(target);

  var Xtest := testDf.ToMatrix(features);
  var ytest := testDf.ToVector(target);

  var model := new RandomForestRegressor(seed := 42);

  model.Fit(Xtrain, ytrain);

  var pred := model.Predict(Xtest);

  Println('R²:', Metrics.R2(ytest, pred):0:3);
end.