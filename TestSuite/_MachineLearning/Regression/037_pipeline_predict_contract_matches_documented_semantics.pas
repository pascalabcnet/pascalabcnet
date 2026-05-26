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

  var pred := pipe.Predict(testDf);
  var predLabels := pipe.PredictLabels(testDf);
  var classes := pipe.GetClassLabels;

  Check(pred.Length = testDf.RowCount, 'Predict length mismatch');
  Check(predLabels.Length = testDf.RowCount, 'PredictLabels length mismatch');
  Check(classes.Length > 0, 'Class labels must not be empty');

  for var i := 0 to pred.Length - 1 do
  begin
    var pi := Round(pred[i]);
    Check(Abs(pred[i] - pi) < 1e-12, $'Predict[{i}] must be an internal class index');
    Check((pi >= 0) and (pi < classes.Length), $'Predict[{i}] out of range');
    Check(predLabels[i] = classes[pi], $'PredictLabels[{i}] must decode Predict[{i}]');
  end;
end.
