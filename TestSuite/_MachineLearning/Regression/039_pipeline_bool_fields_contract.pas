uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddBoolColumn('Flag', Arr(true, false, true, false, true, false));
  df.AddBoolColumn('IsNew', Arr(false, false, true, true, false, true));
  df.AddFloatColumn('Mileage', Arr(10.0, 20.0, 30.0, 40.0, 15.0, 35.0));
  df.AddStrColumn('Target', Arr($'A', $'B', $'A', $'B', $'A', $'B'));
  df := df.SetCategorical(['Target']);

  var pipe :=
    DataPipeline.BuildClassification(
      'Target',
      ['Flag', 'IsNew', 'Mileage'],
      new DecisionTreeClassifier(maxDepth := 3)
    );

  pipe.Fit(df);

  var pred := pipe.Predict(df);
  var predLabels := pipe.PredictLabels(df);
  var classes := pipe.GetClassLabels;
  var X := pipe.Transform(df).ToMatrix(['Flag', 'IsNew', 'Mileage']);

  Check(Abs(X[0,0] - 1.0) < 1e-12, 'Pipeline bool true must encode as 1');
  Check(Abs(X[1,0] - 0.0) < 1e-12, 'Pipeline bool false must encode as 0');
  Check(Abs(X[0,1] - 0.0) < 1e-12, 'Second bool false must encode as 0');
  Check(Abs(X[2,1] - 1.0) < 1e-12, 'Second bool true must encode as 1');

  Check(pred.Length = df.RowCount, 'Pipeline bool-feature prediction length mismatch');
  Check(predLabels.Length = df.RowCount, 'Pipeline bool-feature PredictLabels length mismatch');
  Check(classes.Length = 2, 'Pipeline bool-feature classes length mismatch');

  for var i := 0 to pred.Length - 1 do
  begin
    var pi := Round(pred[i]);
    Check(Abs(pred[i] - pi) < 1e-12, $'Predict[{i}] must be an internal class index');
    Check((pi >= 0) and (pi < classes.Length), $'Predict[{i}] out of range');
    Check(predLabels[i] = classes[pi], $'PredictLabels[{i}] must decode Predict[{i}]');
  end;
end.

