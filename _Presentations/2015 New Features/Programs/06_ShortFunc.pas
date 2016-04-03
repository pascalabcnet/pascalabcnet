function Sqr3(x: integer) := x*x*x;

function CircleLen(r: real): real := 2 * Pi * r;

function Hypot(a,b: real) := sqrt(a*a + b*b);

function Len(x1,y1,x2,y2: real) := Hypot(x2-x1,y2-y1);

function DigitCount(x: integer) := abs(x).ToString.Length;

function Минимум(a,b,c: real): real := Min(Min(a,b),c);

procedure Вывод<ЛюбойТип>(x: ЛюбойТип) := Println(x);

begin
  Println(Sqr3(2),CircleLen(1));
  Println(Hypot(3,4),Len(1,1,3,4));
  Println(DigitCount(-1234));
  Вывод(Минимум(5,3,8));
end.