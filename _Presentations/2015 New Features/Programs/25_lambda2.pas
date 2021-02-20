begin
  var a := Seq(1,2,3,4,5);
  a.Println;
  var z := 10;
  var b := a.Select(x->x+z);
  b.Println;
  z := 20;
  b.Println;
end.