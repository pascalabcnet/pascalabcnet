uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var yTrue := new Vector(Arr(5.0, 5.0, 5.0));
  var yPred := new Vector(Arr(5.0, 5.0, 5.0));

  var r2 := Metrics.R2(yTrue, yPred);
  Check(Abs(r2 - 1.0) < 1e-12, 'R2 must return 1.0 for a perfect prediction of a constant target');
end.
