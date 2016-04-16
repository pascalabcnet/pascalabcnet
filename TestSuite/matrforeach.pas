begin
  var a := new integer[2,3]((1,2,3),(4,5,6));
  var s := 0;
  foreach var x in a do
    s += x;
  Print(s);  
  
  var s1 := 0.0;
  var b := Lst(2.5,3.5,4.5);
  foreach var x in b do
    s1 += x;
end.