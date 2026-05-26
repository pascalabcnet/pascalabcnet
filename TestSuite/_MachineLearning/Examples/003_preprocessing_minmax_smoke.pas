uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var X := new Matrix(5, 2);
  X[0,0] := 1;  X[1,0] := 2;  X[2,0] := 3;  X[3,0] := 10; X[4,0] := 20;
  X[0,1] := 100; X[1,1] := 120; X[2,1] := 150; X[3,1] := 300; X[4,1] := 500;

  var scaler := new MinMaxScaler;
  scaler.Fit(X);
  var Xscaled := scaler.Transform(X);

  Check(Abs(Xscaled.ColumnMin(0) - 0.0) < 1e-12, 'First column min must be 0');
  Check(Abs(Xscaled.ColumnMax(0) - 1.0) < 1e-12, 'First column max must be 1');
  Check(Abs(Xscaled.ColumnMin(1) - 0.0) < 1e-12, 'Second column min must be 0');
  Check(Abs(Xscaled.ColumnMax(1) - 1.0) < 1e-12, 'Second column max must be 1');
end.
