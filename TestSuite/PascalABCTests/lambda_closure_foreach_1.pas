begin
   var a := Seq(1,3,7);
   var actions := new List<Action<integer>>;
   var t := 5;
   foreach var i in a do
   begin
     actions.Add(procedure(x)->writeln(i + t));
   end;
   
   foreach var aa in actions do
     aa(1);
end.