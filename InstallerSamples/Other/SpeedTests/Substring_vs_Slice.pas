begin
  var s := Range(#32,#1000).JoinIntoString;
  var s1: string;
  Milliseconds;
  var n := 100000000;
  loop n do
    s1 := s.Substring(10,10);
  MillisecondsDelta.Println;
  loop n do
    s1 := s[1:11];
  MillisecondsDelta.Println;
end.