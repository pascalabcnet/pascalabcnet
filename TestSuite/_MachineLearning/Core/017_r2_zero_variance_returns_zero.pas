uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var yTrue := new Vector(Arr(5.0, 5.0, 5.0));
  var yPred := new Vector(Arr(5.0, 5.0, 5.0));

  var r2 := Metrics.R2(yTrue, yPred);
  Check(Abs(r2 - 0.0) < 1e-12, 'R2 must return 0.0 when target variance is zero');
end.
