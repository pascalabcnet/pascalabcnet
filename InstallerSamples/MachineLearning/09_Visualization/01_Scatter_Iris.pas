// В этом примере строится scatter plot для двух признаков датасета Iris.
//
// По оси X откладывается длина чашелистика,
// по оси Y — ширина чашелистика.
//
// Цвет точки показывает класс цветка.

uses MLABC, PlotML;

begin
  var ds := Datasets.Iris;

  Println('Scatter plot для датасета Iris');
  Println;
  Println('По оси X: sepal_length');
  Println('По оси Y: sepal_width');

  ds.Points('sepal_length', 'sepal_width', ds.Target, size := 6);
  Plot.Title := 'Iris: scatter plot';
end.
