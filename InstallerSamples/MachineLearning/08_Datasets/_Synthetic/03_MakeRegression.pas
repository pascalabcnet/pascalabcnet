// MakeRegression синтетический датасет.
// Демонстрирует генерацию данных для задачи линейной регрессии.
uses MLABC, PlotML;

begin
  // --- генерируем синтетические данные
  var (X,y) := Datasets.MakeRegression(
    n := 500,
    nFeatures := 1,
    noise := 0.2,
    seed := 1
  );

  // --- визуализация
  Plot.Title('MakeRegression синтетический датасет');
  Plot.XLabel('признак');
  Plot.YLabel('целевая переменная');

  Plot.Points(X.Col(0), y.ToArray, size := 3);
end.