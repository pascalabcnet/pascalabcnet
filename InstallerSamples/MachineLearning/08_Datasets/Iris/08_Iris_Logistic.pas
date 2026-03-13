uses MLABC;

begin
  var ds := Datasets.Iris;

  var (X, y) := ds.ToXY;

  var model := new LogisticRegression;

  model.Fit(X, y);

  var pred := model.Predict(X);

  var acc := Metrics.Accuracy(y, pred);

  Println('Accuracy:', acc:0:3);
end.