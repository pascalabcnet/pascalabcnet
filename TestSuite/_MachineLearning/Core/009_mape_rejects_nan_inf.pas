uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var yTrue := new Vector(Arr(1.0, 2.0, 4.0));
  var good := new Vector(Arr(1.0, 2.2, 3.8));
  var m := Metrics.MAPE(yTrue, good);
  Check(m >= 0.0, 'MAPE must be non-negative');

  var withNaN := new Vector(Arr(1.0, real.NaN, 3.0));
  CheckRaises(procedure -> begin var tmp := Metrics.MAPE(yTrue, withNaN); end,
    'MAPE must reject NaN in predictions');

  var withInf := new Vector(Arr(1.0, real.PositiveInfinity, 3.0));
  CheckRaises(procedure -> begin var tmp := Metrics.MAPE(yTrue, withInf); end,
    'MAPE must reject Infinity in predictions');
end.
