begin
   var a := Seq(1,3,7);
   var actions := new List<Action<integer>>;
   var b := 5;
   var ggg := Seq(2).ToList();
   ggg[0] := 3;
   var i := 4;
   writeln(i);
   for i := 1 to Seq(3).First(x->x = ggg[0]) do
     for var j := 1 to i + ggg[0] do
     begin
       var q := i;
       actions.Add(procedure(x)->writeln(x + i + b + q + ggg[0] + j));
     end;
   
   for var j := 0 to actions.Count - 1 do
     actions[j](j);
end.