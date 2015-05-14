begin
   var a := Seq(1,3,7);
   var actions := new List<Action<integer>>;
   var b := 5;
   var j := -15;
   for var k := 0 to 5 do
     writeln(j);
   for var i := 1 to 2 do
    for j := j to 2 do
    begin
     writeln(j);
     actions.Add(procedure(x)->writeln(i + j));
    end;
   
   writeln(j);
   for var t := 0 to 1 do
     actions[t](1);
end.