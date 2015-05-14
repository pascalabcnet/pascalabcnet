begin
  var c := Arr(2, 5, 8);
  var a:= Seq(2,5,3).Select(x -> x * c[0]).ToList();
  c[0] := 5;
  
  assert(c[0] = 5);
  assert(a.Count = 3);
  assert(a[0] = 4);
  assert(a[1] = 10);
  assert(a[2] = 6);
  
end.