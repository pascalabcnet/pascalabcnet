uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var ok := new Vector(Arr(1.0, 1.9999999999999, -3.0000000000001));
  var a := ok.ToIntArray;

  Check(a.Length = 3, 'Length mismatch');
  Check(a[0] = 1, 'First value mismatch');
  Check(a[1] = 2, 'Second value mismatch');
  Check(a[2] = -3, 'Third value mismatch');

  var bad := new Vector(Arr(1.2, 2.0));
  CheckRaises(procedure -> begin var tmp := bad.ToIntArray; end, 'ToIntArray must reject non-integer values');
end.
