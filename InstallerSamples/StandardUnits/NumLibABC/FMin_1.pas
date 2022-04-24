uses NumLibABC;

// Одномерная оптимизация

begin
  var fun:real->real:=x->x*Sqr(x)-2*x-5;
  var oL:=new Fmin(fun,-1,1);
  Println(oL.x, oL.Value)
end.
