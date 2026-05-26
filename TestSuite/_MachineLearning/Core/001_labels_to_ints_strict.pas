uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var y := new Vector(Arr(0.0, 0.9999999999999, 2.0000000000001));
  var labels := LabelsToInts(y);

  Check(labels.Length = 3, 'Length mismatch');
  Check(labels[0] = 0, 'First label mismatch');
  Check(labels[1] = 1, 'Second label mismatch');
  Check(labels[2] = 2, 'Third label mismatch');

  var bad := new Vector(Arr(0.0, 1.2, 2.0));
  CheckRaises(procedure -> begin var tmp := LabelsToInts(bad); end, 'LabelsToInts must reject non-integer labels');
end.
