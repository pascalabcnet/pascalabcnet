begin
  var a := Seq(1,5,3,2,4);
  var b := Seq(2,3,4,1,6);
  a.Zip(b,(x,y)->x*y).Print;
end.