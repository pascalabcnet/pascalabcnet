var glob1: real -> real := x->x*x;
var a := ArrGen(10,x->x);

var y := 222;
var glob2: real -> real := x->x+y;


begin
  Print(glob1(2));
  Print(glob2(2));
end.