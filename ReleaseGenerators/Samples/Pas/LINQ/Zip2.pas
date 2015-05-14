begin
  var a := Seq(1,5,3,2,4);
  a.Zip(a.Skip(1),(x,y)->y-x).Print;
end.