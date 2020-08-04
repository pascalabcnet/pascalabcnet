begin
  var s := Lst(1..1000);
  var s1: List<integer>;
  Milliseconds;
  var n := 10000000;
  MillisecondsDelta.Println;
  loop n do
    s1 := s[1:11];
  MillisecondsDelta.Println;
end.