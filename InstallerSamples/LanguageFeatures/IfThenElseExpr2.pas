begin
  var (x,y) := (5,-3);
  var q := 
    if x>0 then
      if y>0 then
        1
      else 4
    else
      if y>0 then
        2
      else 3;
        
  Print(q)
end.