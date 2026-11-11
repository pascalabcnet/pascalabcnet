uses ReexportTestSecond in 'usesunits/ReexportTest/ReexportTestSecond.pas';

begin
  var n: int := ReexportTestSecond.InitialValue;
  assert(n = 21);
  assert(DoubleIt(n) = 42);
  SharedValue := 5;
  assert(ReexportTestSecond.SharedValue = 5);
  assert(ReadSharedValue() = 5);
end.
