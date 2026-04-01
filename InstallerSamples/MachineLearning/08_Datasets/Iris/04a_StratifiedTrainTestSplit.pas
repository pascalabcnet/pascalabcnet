uses MLABC;

begin
  var ds := Datasets.Iris;

  // --- stratified split
  var (train, test) := ds.StratifiedTrainTestSplit(0.2, 42);

  var Xtrain := train.Data.ToMatrix(train.Features);
  var Xtest  := test.Data.ToMatrix(test.Features);

  // неудобно, но правильно
  var classes: array of string;

  var ytrain := train.Data.EncodeLabels(train.Target, classes);
  var ytest  := test.Data.TransformLabels(test.Target, classes);

  // --- модель
  var model := new LogisticRegression;
  model.Fit(Xtrain, ytrain);

  var pred := model.Predict(Xtest);

  Println('Accuracy:', Metrics.Accuracy(ytest, pred):0:3);
end.