// В этом примере VarianceThreshold удаляет признак,
// у которого нет разброса значений.

uses MLABC;

begin
  var X := new Matrix(4, 3);
  X[0, 0] := 1; X[0, 1] := 10; X[0, 2] := 5;
  X[1, 0] := 2; X[1, 1] := 10; X[1, 2] := 6;
  X[2, 0] := 3; X[2, 1] := 10; X[2, 2] := 7;
  X[3, 0] := 4; X[3, 1] := 10; X[3, 2] := 8;

  Println('Размер до VarianceThreshold: ', X.RowCount, 'x', X.ColCount);

  var vt := new VarianceThreshold(0.001);
  vt.Fit(X);
  var X2 := vt.Transform(X);

  Println('Размер после VarianceThreshold: ', X2.RowCount, 'x', X2.ColCount);
  Println;
  Println('Преобразованные данные:');
  X2.Println;
end.
