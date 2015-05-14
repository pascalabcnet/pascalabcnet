unit u_delegates3;
type 
  Func1Real = function(x:real):real;
  Func1Integer = function(x : integer):integer;
  Func2 = function(a : array of integer; x : integer):integer;

var f1 : Func1Real;
    f2 : Func1Integer;
    f3 : Func2;
    
begin
f1 := System.Math.Sin;
assert(abs(f1(3.2)-System.Math.Sin(3.2))<0.00000001);
f1 := sin;
assert(f1(3.2)-sin(3.2)<0.000000001);

end.