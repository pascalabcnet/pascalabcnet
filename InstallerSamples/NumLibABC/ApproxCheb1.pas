uses NumLibABC;

// Аппроксимация табличной функции полиномами Чебышева
// по методу наименьших квадратов 
begin
  var e:=0.1;
  var x:=ArrGen(12,i->0.25*i-2); x.Println;
  var y:=x.Select(z->2*z-5*Sqr(z)+8*z*Sqr(z)).ToArray; y.Println;
  var oL:=new ApproxCheb(x,y,e);
  oL.f.Println;  // аппроксимированные значения
  Println(oL.r,oL.tol);  // предлагаемая степень полинома и вычисленная погрешность
  oL.MakeCoef; // раскодированные коэффициенты
  oL.c.Println;
end.
