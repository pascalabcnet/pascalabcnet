// В этом примере строится гистограмма одного признака датасета Iris.
//
// Гистограмма показывает, как распределены значения признака
// и в каких диапазонах они встречаются чаще.

uses MLABC, PlotML;

begin
  var ds := Datasets.Iris;
  var df := ds.Data;

  // Берем длину лепестка.
  var petalLength := df.ToVector('petal_length');

  Println('Гистограмма признака petal_length');
  Println;
  Println('Гистограмма помогает увидеть распределение значений признака.');

  Plot.Hist(petalLength, bins := 12);
  Plot.SetLabels('Iris: histogram', 'petal_length', 'count');
end.
