// В этом примере строится pair plot для всех признаков датасета Iris.
//
// На диагонали расположены гистограммы отдельных признаков,
// а вне диагонали — scatter plot для всех пар признаков.
//
// Цвет точки показывает класс цветка.

uses MLABC, PlotML;

begin
  var ds := Datasets.Iris;

  Println('Pair plot для датасета Iris');
  Println;
  Println('На диагонали показаны гистограммы признаков.');
  Println('Вне диагонали показаны scatter plot для всех пар признаков.');

  ds.PairPlot(ds.Features);
end.
