begin 
  var a := Arr(1, 2, 3);
  
  var g: Func<integer, boolean> := x -> x > 2;
  a.Find(g).Println;
  
  var g2: Predicate<integer> := x -> x > 2;
  g2(456).Println;
  a.Find(g2).Println;
end.