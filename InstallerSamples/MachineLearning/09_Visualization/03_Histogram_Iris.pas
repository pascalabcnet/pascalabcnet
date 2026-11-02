// В этом примере строится гистограмма одного признака датасета Iris.
//
// Гистограмма показывает, как распределены значения признака
// и в каких диапазонах они встречаются чаще.

uses MLABC, PlotML;

begin
  var ds := Datasets.Iris;

  Println('Гистограмма признака petal_length');
  Println;
  Println('Гистограмма помогает увидеть распределение значений признака.');

  ds.Hist('petal_length', bins := 12);
  Plot.Title := 'Iris: histogram';
end.
