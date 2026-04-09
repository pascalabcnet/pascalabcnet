// MakeBlobs синтетический датасет.
// Демонстрирует генерацию кластеризованных данных и их визуализацию.
uses MLABC, PlotML;

begin
  var centers := 3;
  var (X,y) := Datasets.MakeBlobs(
    n := 600,
    centers := centers,
    clusterStd := 1.2,
    seed := 1
  );

  Plot.Title := 'MakeBlobs синтетический датасет';
    
  var xs := X.Col(0);
  var ys := X.Col(1);
  
  Plot.Points(xs, ys, LabelsToInts(y), size := 4);
end.