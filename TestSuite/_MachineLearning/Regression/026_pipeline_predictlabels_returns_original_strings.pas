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

  var labels := pipe.PredictLabels(testDf);
  var classes := pipe.GetClassLabels;

  Check(labels.Length = testDf.RowCount, 'PredictLabels length mismatch');

  for var i := 0 to labels.Length - 1 do
    Check(labels[i] in classes, $'PredictLabels[{i}] must be one of pipeline class labels');
end.
