begin
  var n := 300000000;
  var a := ArrFill(n,1);
  var l := Lst(a);
  MillisecondsDelta;
  for var i:=0 to n-1 do
    a[i] := 2;
  MillisecondsDelta.Println; // 143 ms
  for var i:=0 to n-1 do
    l[i] := 2;
  MillisecondsDelta.Println; // 357 ms
end.