function f(p: integer->integer) := (3,p(1));

begin
  var pair := (3,5);
  (var a, var b) := f(x->begin
    var pair := (7,9);
    (var c, var d) := pair;
    Result := c;
  end);
  
  Print(a,b)
end.