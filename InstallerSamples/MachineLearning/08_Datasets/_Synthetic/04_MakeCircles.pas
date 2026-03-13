// MakeCircles синтетический датасет.
// Демонстрирует генерацию данных в виде двух концентрических окружностей.
uses MLABC, PlotML;

begin
  var (X,y) := Datasets.MakeCircles(
    n := 600,
    noise := 0.05,
    factor := 0.5,
    seed := 1
  );

  Plot.Title('MakeCircles синтетический датасет');

  Plot.Points(X.Col(0), X.Col(1), LabelsToInts(y), size := 3);
end.