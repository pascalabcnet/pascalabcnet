type
  c1 = class end;

begin
  var v1: array [,] of c1 := ((new c1, new c1),(new c1, new c1));
  v1.Print;
end.