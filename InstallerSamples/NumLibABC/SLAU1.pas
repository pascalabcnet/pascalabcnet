uses NumLibABC;

// Линейная алгебра (СЛАУ)
begin
  var A:=new Matrix(3,3,2,3,-1,1,-2,1,1,0,2);
  var B:=new Vector(9,3,2);
  var cond:real;
  var x:=A.SLAU(B,cond);
  x.Println;
  Writeln('Число обусловленности = ',cond)
end.
