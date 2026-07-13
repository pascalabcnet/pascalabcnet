uses MLABC;

begin
  var ds := Datasets.Iris;

  var (train, test) := ds.StratifiedTrainTestSplit(0.2, seed := 42);

  var Xtrain := train.Data.ToMatrix(train.Features);
  var Xtest  := test.Data.ToMatrix(test.Features);

  var target := train.Data.EncodeTarget(train.Target);
  var ytrain := target.Labels;
  var ytest  := test.Data.TransformLabels(test.Target, target.ClassNames);

  var model := new LogisticRegression;
  model.Fit(Xtrain, ytrain);

  var pred := model.Predict(Xtest);

  Println('Accuracy:', Metrics.Accuracy(ytest, pred):0:3);
end.
