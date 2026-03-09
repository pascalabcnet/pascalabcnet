// MakeBlobs синтетический датасет.
// Демонстрирует генерацию кластеризованных данных и их визуализацию.
uses MLABC, PlotML;

begin
  // --- генерируем синтетические данные
  var centers := 3;
  var (X,y) := Datasets.MakeBlobs(
    n := 600,
    centers := centers,
    clusterStd := 1.2,
    seed := 1
  );

  var n := X.RowCount;

  // --- визуализация
  Plot.Title('MakeBlobs синтетический датасет');
  Plot.XLabel('признак 1');
  Plot.YLabel('признак 2');

  for var c := 0 to centers-1 do
  begin
    var ind := y.ToArray.Indices(v -> Round(v) = c).ToArray;

    var xs := ind.ConvertAll(i -> X[i,0]);
    var ys := ind.ConvertAll(i -> X[i,1]);

    Plot.Points(xs, ys, size := 3, legend := 'кластер ' + c);
  end;
end.