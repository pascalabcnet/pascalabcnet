begin
  var x := new class(r := 1..2, r2 := 3..4);
  Assert(x.r = 1..2)
end.  