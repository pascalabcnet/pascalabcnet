uses MLABC;

begin
  var ds := Datasets.MoscowHousing;
  var df := ds.Data;

  var features := ['rooms','area','kitchen_area','floor','floors_total','metro_minutes'];
  var target := 'price';

  var X := df.ToMatrix(features);
  var y := df.ToVector(target);

  var (Xtrain, Xtest, ytrain, ytest) := Validation.TrainTestSplit(X, y, 0.2, 42);

  var model := new LinearRegression;

  model.Fit(Xtrain, ytrain);

  var pred := model.Predict(Xtest);

  Println('R²:', Metrics.R2(ytest, pred):0:3);
end.