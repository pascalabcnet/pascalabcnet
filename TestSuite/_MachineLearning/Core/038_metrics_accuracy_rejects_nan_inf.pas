uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var yTrue := new Vector(Arr(0.0, 1.0));
  var yPred := new Vector(Arr(real.NaN, 1.0));

  CheckRaises(procedure -> begin
    var a := Metrics.Accuracy(yTrue, yPred);
  end, 'Accuracy must reject NaN and Infinity in class labels');
end.
