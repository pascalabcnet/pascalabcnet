uses MLABC, PlotML;

begin
  var (X, trueLabels) := Datasets.MakeBlobs(
    n := 300,
    centers := 3,
    clusterStd := 0.7,
    seed := 8
  );

  // Искажаем вторую координату
  for var i := 0 to X.RowCount - 1 do
    X[i, 1] *= 1000;

  var Xscaled := X.Clone;
  var scaler := new StandardScaler;
  Xscaled := scaler.FitTransform(Xscaled);

  var model1 := new KMeans(3, seed := 42);
  var labels1 := model1.FitPredict(X);

  var model2 := new KMeans(3, seed := 42);
  var labels2 := model2.FitPredict(Xscaled);

  var (xs1, ys1) := X.Cols(0, 1);
  var (xs2, ys2) := Xscaled.Cols(0, 1);

  fig[0, 0].Points(xs1, ys1, labels1, size := 4);
  fig[0, 0].Points(model1.Centers, color := Colors.Black, size := 12, marker := MarkerType.Cross);
  fig[0, 0].Title := 'Без масштабирования';

  fig[0, 1].Points(xs2, ys2, labels2, size := 4);
  fig[0, 1].Points(model2.Centers, color := Colors.Black, size := 12, marker := MarkerType.Cross);
  fig[0, 1].Title := 'После StandardScaler';
end.