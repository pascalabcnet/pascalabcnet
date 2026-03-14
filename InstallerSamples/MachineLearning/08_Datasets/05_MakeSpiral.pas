// MakeSpiral синтетический датасет.
// Демонстрирует генерацию данных в виде спиралей.
uses MLABC, PlotML;

begin
  var (X, y) := Datasets.MakeSpiral(
    n := 600,
    classes := 3,
    turns := 2.5,
    noise := 0.015,
    seed := 1
  );
  
  Plot.Title('MakeSpiral синтетический датасет');
  Plot.Points(X.Col(0), X.Col(1), LabelsToInts(y), size := 3);
end.