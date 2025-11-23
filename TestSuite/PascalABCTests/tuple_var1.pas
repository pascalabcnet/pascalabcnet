begin
  var pair := (3,5);
  (var a, var b) := pair;
  Assert((a=3) and (b=5) );
  Assert((a,b)=(3,5));
end. 
