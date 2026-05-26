// В этом примере PCA уменьшает число признаков
// с двух до одного.

uses MLABC;

begin
  var X := new Matrix(5, 2);
  X[0, 0] := 1; X[0, 1] := 2;
  X[1, 0] := 2; X[1, 1] := 4;
  X[2, 0] := 3; X[2, 1] := 6;
  X[3, 0] := 4; X[3, 1] := 8;
  X[4, 0] := 5; X[4, 1] := 10;

  Println('Размер до PCA: ', X.RowCount, 'x', X.ColCount);

  var pca := new PCATransformer(1);
  pca.Fit(X);
  var X1 := pca.Transform(X);

  Println('Размер после PCA: ', X1.RowCount, 'x', X1.ColCount);
  Println;
  Println('Преобразованные данные:');
  X1.Println;
end.
