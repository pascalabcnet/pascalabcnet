begin
  var a := Arr(1,2,3);
  a := a[:][:2];
  a := a?[:]?[:2];
  Print(a);
end.