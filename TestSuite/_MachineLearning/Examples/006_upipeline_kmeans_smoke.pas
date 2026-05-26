uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var ds := Datasets.Iris;

  var pipe :=
    UDataPipeline.Build(
      ds.Features,
      new StandardScaler,
      new KMeans(3, seed := 1)
    );

  var labels := pipe.FitPredict(ds.Data);

  Check(labels.Length = ds.Data.RowCount, 'UDataPipeline labels length mismatch');
end.
