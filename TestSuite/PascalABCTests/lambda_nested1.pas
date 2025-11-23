begin
  var s: sequence of integer := Seq(1,2);
  
  Assert(s.SelectMany(
    t1->
    s.Select(t2->t1+t2).Where(t->true)
  ).First = 2)
  
end.