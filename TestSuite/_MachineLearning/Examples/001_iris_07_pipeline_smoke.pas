uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var ds := Datasets.Iris;
  var df := ds.Data;

  var pipe :=
    DataPipeline.Build(
      TaskKind.tkClassification,
      ds.Target,
      ds.Features,
      new StandardScaler,
      new LogisticRegression
    );

  var (trainDf, testDf) := df.TrainTestSplit(0.2, seed := 3);

  pipe.Fit(trainDf);

  var pred := pipe.Predict(testDf);
  var y := pipe.GetEncodedLabels(testDf);

  Check(pred.Length = testDf.RowCount, 'Predict length mismatch');
  Check(y.Length = testDf.RowCount, 'Encoded labels length mismatch');
  Check(Metrics.Accuracy(y, pred) > 0.8, 'Pipeline accuracy is unexpectedly low');
end.
