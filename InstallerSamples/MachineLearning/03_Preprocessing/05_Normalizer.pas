// В этом примере показано,
// как Normalizer нормализует строки матрицы.

uses MLABC;

begin
  var X := new Matrix(3, 2);
  X[0, 0] := 3;  X[0, 1] := 4;
  X[1, 0] := 1;  X[1, 1] := 2;
  X[2, 0] := 5;  X[2, 1] := 12;

  Println('До Normalizer:');
  X.Println;
  Println;

  var norm := new Normalizer(NormType.L2);
  norm.Fit(X);
  var Xn := norm.Transform(X);

  Println('После Normalizer:');
  Xn.Println;
end.
