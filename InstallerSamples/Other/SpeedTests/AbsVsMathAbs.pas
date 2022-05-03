// Abs для целых работает в 2.5 раза быстрее Math.Abs
begin
  var s := 0;
  var n := MaxInt;
  loop n do
    s := Abs(-2);
  Print(MillisecondsDelta);
  loop n do
    s := System.Math.Abs(-2);
  Print(MillisecondsDelta);
end.