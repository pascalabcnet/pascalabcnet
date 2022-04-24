begin
  var a := Arr(1..10);
  a.Println;
  var ps := a.PartialSum;
  ps.Println;
  ps.Incremental.Println; // Incremental - "почти" обратная к PartialSum
end.
