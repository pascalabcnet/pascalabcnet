uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var ds := Datasets.Iris;
  var df := ds.Data;
  var (trainDf, testDf) := df.TrainTestSplit(0.2, seed := 3);

  var pipe :=
    DataPipeline.Build(
      TaskKind.tkClassification,
      ds.Target,
      ds.Features,
      new StandardScaler,
      new LogisticRegression
    );

  pipe.Fit(trainDf);

  var classes := pipe.GetClassLabels;
  var pred := pipe.Predict(testDf);
  var predLabels := pipe.PredictLabels(testDf);
  var y := pipe.GetEncodedLabels(testDf);
  var trueLabels := testDf.GetStrColumn(ds.Target);

  Check(classes.Length > 0, 'classes must not be empty');
  Check(pred.Length = testDf.RowCount, 'Predict length mismatch');
  Check(predLabels.Length = testDf.RowCount, 'PredictLabels length mismatch');
  Check(y.Length = testDf.RowCount, 'GetEncodedLabels length mismatch');

  for var i := 0 to testDf.RowCount - 1 do
  begin
    var pi := Round(pred[i]);
    var yi := Round(y[i]);

    Check(Abs(pred[i] - pi) < 1e-12, $'Predict[{i}] is not an encoded integer');
    Check((pi >= 0) and (pi < classes.Length), $'Predict[{i}] out of range');
    Check(predLabels[i] = classes[pi], $'PredictLabels[{i}] does not decode Predict[{i}]');

    Check(Abs(y[i] - yi) < 1e-12, $'GetEncodedLabels[{i}] is not an encoded integer');
    Check((yi >= 0) and (yi < classes.Length), $'GetEncodedLabels[{i}] out of range');
    Check(classes[yi] = trueLabels[i], $'GetEncodedLabels[{i}] does not decode to true target');
  end;
end.
