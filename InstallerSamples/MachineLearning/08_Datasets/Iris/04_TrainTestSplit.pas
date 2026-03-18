uses MLABC;

begin
  var ds := Datasets.Iris;
  var df := ds.Data;

  var X := df.ToMatrix(ds.Features);
  var y := df.EncodeLabels(ds.Target);

  var (Xtrain, Xtest, ytrain, ytest) :=
    Validation.TrainTestSplit(X, y, 0.2, 1);

  var model := new LogisticRegression;

  model.Fit(Xtrain, ytrain);

  var pred := model.Predict(Xtest);

  Println('Accuracy:', Metrics.Accuracy(ytest, pred):0:3);
end.