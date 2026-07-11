uses MLABC, PlotML;

begin
  var (X, trueLabels) := Datasets.MakeBlobs(
    n := 300,
    centers := 3,
    clusterStd := 0.8,
    seed := 42);

  var model := new KMeans(3, seed := 42);

  var labels := model.FitPredict(X);

  var (xs,ys) := X.Cols(0,1);
  
  Plot.Points(xs, ys, labels, size := 4);
  Plot.Title := 'KMeans: найденные кластеры';
  
  Vector.DefaultStringFormat := 'G6';
  Println(model.Centers);

  Plot.Points(model.Centers, color := Colors.Black, size := 6, marker := MarkerType.Diamond);
end.