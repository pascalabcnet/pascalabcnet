begin
  //var a: array [1..5] of integer := (1,3,5,7,9);
  //var a := Arr(1,2,3,4,5);
  //var a := Arr(1.0,2.0);
  var a := new System.Collections.ArrayList();
  a.Add(2);
  foreach x: integer in a do
    Print(x);
end.