begin
   var a := Seq(1,3,7);
   var actions := new List<Func<integer, integer>>;
   var t := 5;
   foreach var i in a do
   begin
     actions.Add(x -> x + i + t);
   end;
   
   t := 7;
   
   var ll := new List<integer>;
   
   foreach var aa in actions do
     ll.Add(aa(1));
   
   assert(t = 7); 
   assert(ll.Count = 3);
   assert(ll[0] = 9);
   assert(ll[1] = 11);
   assert(ll[2] = 15);
end.