begin
  var a:=MatrRandom(4,5); 
  var b{@var b: sequence of IEnumerable<integer>;@}:=a.Rows.SelectMany(x->x);
  
end.