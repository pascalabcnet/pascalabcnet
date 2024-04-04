uses NumLibABC;

// Разложение полинома с целочисленными коэффициентами
// на рациональные линейные множители 
begin
  var oL:=new Factors(-20, 7, 73, -42);
  var r:=oL.Factorize;
  Writeln('k:=',r[0,1]);
  for var i:=1 to r[0,0] do r.Row(i).Println;
end.
