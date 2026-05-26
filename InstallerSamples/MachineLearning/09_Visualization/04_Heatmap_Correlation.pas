// В этом примере строится heatmap матрицы корреляций
// для числовых признаков датасета Iris.
//
// Тепловая карта помогает быстро увидеть,
// какие признаки связаны друг с другом сильнее.

uses MLABC, PlotML;

begin
  var ds := Datasets.Iris;
  var df := ds.Data;

  // Вычисляем матрицу корреляций для числовых столбцов.
  var corrDf := Statistics.CorrelationMatrix(df);
  var corrNames := corrDf.Schema.ColumnNames.Skip(1).ToArray;
  var corr := corrDf.ToMatrix(corrNames);

  Println('Heatmap матрицы корреляций для Iris');
  Println;
  Println('В строках и столбцах расположены признаки.');
  Println('Внутри каждой клетки показан коэффициент корреляции.');

  Plot.Heatmap(corr, corrNames);
  Plot.Title := 'Iris: correlation heatmap';
end.
