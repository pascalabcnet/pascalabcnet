begin
   var a := Seq(1,3,7);
   var actions := new List<Action<integer>>;
   var b := 5;
   for var i := 1 to 2 do
    for var j := 1 to 2 do
     actions.Add(procedure(x)->writeln(i + j));
     
   for var t := 0 to 1 do
     actions[t](1);
end.