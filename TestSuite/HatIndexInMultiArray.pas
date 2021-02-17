begin
  var a := Matr(2, 3, 1, 2, 3, 4, 5, 6);
  var b := a[^1, ^1];
  Assert(b = 6);
end.