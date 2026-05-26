uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var yTrue := new Vector(Arr(0.49, 1.0));
  var yPred := new Vector(Arr(0.51, 1.0));

  CheckRaises(procedure -> begin
    var a := Metrics.Accuracy(yTrue, yPred);
  end, 'Accuracy must reject non-integer class labels');
end.
