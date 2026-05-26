uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var X := new Matrix(4, 2);
  X[0,0] := 1.0;   X[0,1] := 10.0;
  X[1,0] := 2.0;   X[1,1] := 20.0;
  X[2,0] := 5.0;   X[2,1] := 40.0;
  X[3,0] := 9.0;   X[3,1] := 80.0;

  var scaler := new MinMaxScaler(-1.0, 1.0);
  scaler.Fit(X);

  var scaled := scaler.Transform(X);
  var restored := scaler.InverseTransform(scaled);

  for var i := 0 to X.RowCount - 1 do
    for var j := 0 to X.ColCount - 1 do
      Check(Abs(restored[i,j] - X[i,j]) < 1e-9, $'InverseTransform mismatch at [{i},{j}]');
end.
