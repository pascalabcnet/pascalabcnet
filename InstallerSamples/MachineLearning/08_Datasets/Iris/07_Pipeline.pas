uses MLABC;

begin
  var ds := Datasets.Iris;
  var df := ds.Data;

  var pipe :=
    DataPipeline.Build(
      ds.Target,
      ds.Features,
      new LabelEncoder(ds.Target), // Data preprocessor
      new StandardScaler,          // Matrix transformer
      new LogisticRegression       // Model
    );

  var (trainDf, testDf) := df.TrainTestSplit(0.2, seed := 3);

  pipe.Fit(trainDf);

  var pred := pipe.Predict(testDf);

  var y := testDf.EncodeLabels(ds.Target);

  Println('Accuracy:', Metrics.Accuracy(y, pred):0:3);
end.