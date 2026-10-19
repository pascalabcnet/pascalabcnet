{$package MathNet.Numerics.MKL.Win-x64 3.0.0}
uses MathNet.Numerics, MathNet.Numerics.LinearAlgebra.Double,
  MathNet.Numerics.Providers.LinearAlgebra;

begin
  // Fail if native MKL is unavailable; do not silently use managed math.
  Control.UseNativeMKL;
  var provider := LinearAlgebraControl.Provider;
  Assert(provider.GetType.FullName =
    'MathNet.Numerics.Providers.MKL.LinearAlgebra.MklLinearAlgebraProvider');
  Println(provider);

  var a := DenseMatrix.Create(2, 2, 0.0);
  a[0,0] := 1; a[0,1] := 2;
  a[1,0] := 3; a[1,1] := 4;
  var b := DenseMatrix.Create(2, 2, 0.0);
  b[0,0] := 5; b[0,1] := 6;
  b[1,0] := 7; b[1,1] := 8;
  var c := a.Multiply(b);
  Assert(Abs(c[0,0] - 19) < 1e-12);
  Assert(Abs(c[0,1] - 22) < 1e-12);
  Assert(Abs(c[1,0] - 43) < 1e-12);
  Assert(Abs(c[1,1] - 50) < 1e-12);
  Println(c[0,0], c[0,1], c[1,0], c[1,1]);
end.
