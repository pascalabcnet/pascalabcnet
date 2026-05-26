uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddFloatColumn('X', Arr(0.0, 1.0, 0.0, 1.0));
  df.AddIntColumn('Target', Arr(10, 20, 10, 20));
  df := df.SetCategorical(['Target']);

  var pipe := DataPipeline.Build(
    TaskKind.tkClassification,
    'Target',
    Arr($'X'),
    new LogisticRegression
  );

  pipe.Fit(df);

  var pred := pipe.Predict(df);
  var predLabels := pipe.PredictLabels(df);
  var enc := pipe.GetEncodedLabels(df);
  var classes := pipe.GetClassLabels;

  Check(classes.Length = 2, 'Class count mismatch');
  Check((classes[0] = '10') or (classes[0] = '20'), 'Unexpected first class label');
  Check((classes[1] = '10') or (classes[1] = '20'), 'Unexpected second class label');
  Check(classes[0] <> classes[1], 'Class labels must be distinct');

  Check(pred.Length = df.RowCount, 'Predict length mismatch');
  Check(predLabels.Length = df.RowCount, 'PredictLabels length mismatch');
  Check(enc.Length = df.RowCount, 'GetEncodedLabels length mismatch');

  for var i := 0 to df.RowCount - 1 do
  begin
    var pi := Round(pred[i]);
    var ei := Round(enc[i]);
    Check((pi >= 0) and (pi < classes.Length), $'Predict[{i}] out of range');
    Check((ei >= 0) and (ei < classes.Length), $'GetEncodedLabels[{i}] out of range');
    Check(predLabels[i] = classes[pi], $'PredictLabels[{i}] mismatch');
  end;
end.
