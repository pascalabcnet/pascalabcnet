uses NumLibABC;

// Решение СЛАУ с действительными коэффициентами
begin
  var A:=new real[3,3] ((2,3,-1),(1,-2,1),(1,0,2));
  var B:=new real[3] (9,3,2);
  var oL:=new Decomp(A);
  oL.Solve(B);
  B.Println;
  Writeln('cond=',oL.cond)
end.
