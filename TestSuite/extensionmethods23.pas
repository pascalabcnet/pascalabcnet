type 
  RR = record
    x,y: integer;
  end;
begin  
  var a: array of RR;
  a := new RR[2];
  a[0].x := -1;
  var i := a.Count(x->x.x<0);
  assert(i = 1);
end.