##

function f1 := (1,2);
function f2 := System.ValueTuple.Create(1,2);

// OK
var (a1,b1) := f1;
// OK
var (a2,b2) := f2;

// OK
(a1,b1) := f1;
(a2,b2) := f2;
Assert((a1,b1)=(1,2));
Assert((a2,b2)=(1,2));