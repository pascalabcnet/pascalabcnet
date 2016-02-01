function Sqr3(x: integer) := x*x*x;

function CircleLen(r: real): real := 2 * Pi * r;

function Hypot(a,b: real) := sqrt(a*a + b*b);

function Len(x1,y1,x2,y2: real) := Hypot(x2-x1,y2-y1);

function DigitCount(x: integer) := abs(x).ToString.Length;

begin
  writeln(Hypot(3,4));
  writeln(DigitCount(-1234));
end.