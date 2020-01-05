begin
  var diap := 'a'..'z';
  Println(diap);
  var g := 3 in 1..5;
  Println(g);
  foreach var x in 1..10 do
    Print(x);
  var a := Arr(1..5);
  var h := HSet(1..5);
  Println(a,h);
  (1..9).Select(x->x*x).Println;
end.