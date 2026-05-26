// В этом примере строится confusion matrix для классификации Iris.

uses MLABC, PlotML;

begin
  var ds := Datasets.Iris;

  var (train, test) := ds.StratifiedTrainTestSplit(0.2, 42);

  var Xtrain := train.Data.ToMatrix(train.Features);
  var Xtest := test.Data.ToMatrix(test.Features);

  var encoder := new LabelEncoder;
  var ytrain := encoder.FitTransform(train);
  var ytest := encoder.Transform(test);

  var model := new LogisticRegression;
  model.Fit(Xtrain, ytrain);

  var pred := model.Predict(Xtest);
  var cm := new ConfusionMatrix(ytest, pred);
  var acc := Metrics.Accuracy(ytest, pred);

  Plot.ConfusionMatrix(cm, encoder.Classes, normalize := MatrixNormalization.Rows);
  Plot.Title := $'Iris: LogisticRegression, accuracy = {acc:F3}';
  Plot.XLabel := 'Predicted';
  Plot.YLabel := 'Actual';
end.
