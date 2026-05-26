uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var ds := Datasets.Iris;
  var X := ds.Data.ToMatrix(ds.Features);

  var classes: array of string;
  var y := ds.Data.EncodeLabels(ds.Target, classes);

  var (Xtrain, Xtest, ytrain, ytest) := Validation.TrainTestSplit(X, y, 0.2, 1);

  var model := new LogisticRegression;
  model.Fit(Xtrain, ytrain);

  var pred := model.Predict(Xtest);

  Check(pred.Length = ytest.Length, 'Prediction length mismatch');
  Check(Metrics.Accuracy(ytest, pred) > 0.8, 'TrainTestSplit example accuracy is unexpectedly low');
end.
