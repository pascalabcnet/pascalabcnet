begin
  var n := 60000;
  var t1 := Milliseconds;
  var sm := 0.0;
  var i := 1.0;
  while i < n do
  begin
    var j := 1.0;
    while j < n do
    begin
      sm += 1.0 / i / j;
      j += 1;
    end;
    i += 1;
  end;
  var t2 := Milliseconds;
  print(t2 - t1);
end.