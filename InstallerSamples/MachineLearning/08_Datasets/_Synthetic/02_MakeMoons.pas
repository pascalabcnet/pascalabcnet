// MakeMoons синтетический датасет.
// Демонстрирует генерацию данных в форме двух «лун».
uses MLABC, PlotML;

begin
  var (X,y) := Datasets.MakeMoons(
    n := 600,
    noise := 0.1,
    seed := 1
  );

  Plot.Title('MakeMoons синтетический датасет');

  Plot.Points(X.Col(0), X.Col(1), LabelsToInts(y), size := 4);
end.