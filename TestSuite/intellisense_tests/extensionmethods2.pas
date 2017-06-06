begin
  var a:=MatrRandom(4,5); 
  var b{@var b: sequence of IEnumerable<integer>;@}:=a.Rows.SelectMany(x->x);
  var arr1: array of integer;
  var i{@var i: integer;@} := arr1.Find(x->x=2);
end.