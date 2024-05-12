begin

var i: integer := 3;
var pi: ^integer;



(pi, i) := (@i, 4);

(pi^, i) := (3, 6);


end.