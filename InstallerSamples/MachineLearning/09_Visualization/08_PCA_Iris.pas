uses MLABC, PlotML;

begin
  var ds := Datasets.Iris;
  var X := ds.Data.ToMatrix(ds.Features);
  var y := ds.Data.EncodeLabels(ds.Target);

  var scaler := new StandardScaler;
  scaler.Fit(X);
  X := scaler.Transform(X);

  var pca := new PCATransformer(2);
  pca.Fit(X);
  var X2 := pca.Transform(X);

  Plot.Points(X2.Col(0), X2.Col(1), y, size := 5);
  Plot.XLabel := 'Первая главная компонента';
  Plot.YLabel := 'Вторая главная компонента';
  Plot.Title := 'Iris: проекция на две главные компоненты';
end.
