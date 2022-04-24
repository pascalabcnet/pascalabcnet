uses NumLibABC;

// Случайный поиск ARS
begin
  var f:function(x:array of real):real:=
      x->4*Sqr(x[0]-5)+Sqr(x[1]-6);
  var x:=Arr(-8.0,9.0);
  var (t,R):=(1.0,1e-6);
  var oL:=new FMinN(x,f);
  oL.ARS(R,t);
  Write('Аргументы: '); oL.x.Println;
  Writeln('Значение функции: ',f(oL.x))
end.
