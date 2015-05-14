begin
   var a := Seq(1,3,7);
   var actions := new List<Action<integer>>;
   var b := 5;
   for var i := 1 to 2 do
   begin
     var q := i;
     actions.Add(procedure(x)->writeln(i + b + q));
   end;
   
   foreach var aa in actions do
     aa(1);
end.