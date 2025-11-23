uses u_generics3; 
begin 
  var q: t1 := new t1;
  q.p1;
  assert(q.i = 2);
end.