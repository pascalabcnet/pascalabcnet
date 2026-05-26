uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var ds := Datasets.Iris;
  var X := ds.Data.ToMatrix(ds.Features);

  var classes: array of string;
  var y := ds.Data.EncodeLabels(ds.Target, classes);

  var model := new GradientBoostingClassifier(20, learningRate := 0.1, maxDepth := 3, seed := 1);
  model.Fit(X, y);

  var pred := model.Predict(X);
  var labels := model.PredictLabels(X);
  var modelClasses := model.GetClassLabels;

  Check(modelClasses.Length = classes.Length, 'class count mismatch');
  Check(pred.Length = X.RowCount, 'Predict length mismatch');
  Check(labels.Length = X.RowCount, 'PredictLabels length mismatch');

  for var i := 0 to X.RowCount - 1 do
  begin
    var pi := Round(pred[i]);
    Check(Abs(pred[i] - pi) < 1e-12, $'Predict[{i}] is not an original integer label');
    Check((labels[i] >= 0) and (labels[i] < modelClasses.Length), $'PredictLabels[{i}] out of range');
    Check(modelClasses[labels[i]] = pi.ToString, $'Predict[{i}] and PredictLabels[{i}] are inconsistent');
  end;
end.
