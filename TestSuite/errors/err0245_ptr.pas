begin
  var p: ^integer;  
  new(p);
  p^ := 2;
  var i1: integer;
  i1 := ^p;
  Print(i1);
end.