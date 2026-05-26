// В этом примере строится scatter plot для двух признаков датасета Iris.
//
// По оси X откладывается длина чашелистика,
// по оси Y — ширина чашелистика.
//
// Цвет точки показывает класс цветка.

uses MLABC, PlotML;

begin
  var ds := Datasets.Iris;
  var df := ds.Data;

  // Берем два признака для двумерного графика.
  var sepalLength := df.ToVector('sepal_length');
  var sepalWidth := df.ToVector('sepal_width');

  // Кодируем классы в числа, чтобы раскрасить точки по видам Iris.
  var y := df.EncodeLabels(ds.Target);

  Println('Scatter plot для датасета Iris');
  Println;
  Println('По оси X: sepal_length');
  Println('По оси Y: sepal_width');

  Plot.Points(sepalLength, sepalWidth, y, size := 6);
  Plot.SetLabels('Iris: scatter plot', 'sepal_length', 'sepal_width');
end.
