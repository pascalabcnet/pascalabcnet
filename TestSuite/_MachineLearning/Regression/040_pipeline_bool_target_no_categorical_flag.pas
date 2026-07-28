uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddFloatColumn('X', Arr(0.0, 1.0, 2.0, 3.0));
  df.AddBoolColumn('Target', Arr(false, false, true, true));

  var pipe :=
    DataPipeline.BuildClassification(
      'Target',
      Arr($'X'),
      new DecisionTreeClassifier(maxDepth := 2)
    );

  pipe.Fit(df);

  var pred := pipe.Predict(df);
  var labels := pipe.PredictLabels(df);
  var classes := pipe.GetClassLabels;
  var encoded := pipe.GetEncodedLabels(df);

  Check(pred.Length = df.RowCount, 'Bool-target prediction length mismatch');
  Check(labels.Length = df.RowCount, 'Bool-target label prediction length mismatch');
  Check(encoded.Length = df.RowCount, 'Bool-target encoded label length mismatch');
  Check(classes.Length = 2, 'Bool-target class count mismatch');
end.
