begin
   var a := Seq(1,3,7);
   var actions := new List<Func<integer, integer>>;
   var b := 5;
   for var i := 1 to 2 do
    for var j := 1 to 2 do
    begin
     actions.Add(x-> i + j + b);
     b += 1;
    end;
     
   for var t := 0 to 1 do
     assert(actions[t](1) = 13);
end.