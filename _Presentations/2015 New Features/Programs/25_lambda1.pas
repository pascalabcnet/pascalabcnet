begin
  var a := Seq(1,5,2,6,7);
  a.Println;
  var b := a.Select(x->x*x);
  b.Println;
end.