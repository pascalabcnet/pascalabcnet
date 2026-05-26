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

  var classes := pipe.GetClassLabels;
  var proba := pipe.PredictProba(testDf);

  Check(proba.RowCount = testDf.RowCount, 'Probability row count mismatch');
  Check(proba.ColCount = classes.Length, 'Probability class count mismatch');
  CheckProbabilityRowsSumToOne(proba);
end.
