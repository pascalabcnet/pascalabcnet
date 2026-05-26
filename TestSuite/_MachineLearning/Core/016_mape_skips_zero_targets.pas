uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var yTrue := new Vector(Arr(0.0, 2.0, 4.0));
  var yPred := new Vector(Arr(100.0, 1.0, 5.0));

  var m := Metrics.MAPE(yTrue, yPred);

  Check(not real.IsNaN(m), 'MAPE must not become NaN when yTrue contains zeros');
  Check(not real.IsInfinity(m), 'MAPE must not become Infinity when yTrue contains zeros');
  Check(Abs(m - 0.375) < 1e-12, 'MAPE must ignore zero targets in the average');
end.
