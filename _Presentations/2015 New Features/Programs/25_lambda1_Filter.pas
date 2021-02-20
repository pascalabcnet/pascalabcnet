begin
  var a := Seq(1,5,2,6,7);
  a.Println;
  var b := a.Where(x->x mod 2 <> 0);
  b.Println;
end.