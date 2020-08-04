begin
  var s := Arr(1..10);
  Milliseconds;
  var n := 100000000;
  MillisecondsDelta.Println;
  loop n do
    s[0:10] := s;
  MillisecondsDelta.Println;
end.