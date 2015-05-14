begin
  var c := Seq(2, 5, 8);
  var a:= Seq(2,5,3).Select(x -> x * c[0]);
  c[0] := 5;
  a.Print;
end.