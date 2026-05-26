uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var X := new Matrix(6, 1);
  X[0,0] := 0.0;  X[1,0] := 0.1;
  X[2,0] := 1.0;  X[3,0] := 1.1;
  X[4,0] := 2.0;  X[5,0] := 2.1;

  var y := new Vector(Arr(10.0, 10.0, 20.0, 20.0, 30.0, 30.0));

  var model := new GradientBoostingClassifier(20, learningRate := 0.1, maxDepth := 3, seed := 1);
  model.Fit(X, y);

  var pred := model.Predict(X);
  var labels := model.PredictLabels(X);
  var classes := model.GetClassLabels;
  var proba := model.PredictProba(X);

  Check(classes.Length = 3, 'Class count mismatch');
  Check(classes[0] = '10', 'First class label mismatch');
  Check(classes[1] = '20', 'Second class label mismatch');
  Check(classes[2] = '30', 'Third class label mismatch');
  CheckProbabilityRowsSumToOne(proba);

  for var i := 0 to X.RowCount - 1 do
  begin
    var pi := Round(pred[i]);
    Check((labels[i] >= 0) and (labels[i] < classes.Length), $'PredictLabels[{i}] out of range');
    Check((pi = 10) or (pi = 20) or (pi = 30), $'Predict[{i}] must return original sparse label');
    Check(classes[labels[i]] = pi.ToString, $'Predict and PredictLabels mismatch at {i}');
  end;
end.
