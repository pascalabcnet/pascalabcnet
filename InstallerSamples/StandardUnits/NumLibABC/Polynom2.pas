uses NumLibABC;

// полиномиальная арифметика
begin
  var a:=new Polynom(6.5,-4,2.12,1);
  var b:=new Polynom(3,0,-3.8);
  var c:=new Polynom(ArrGen(5,i->i*i+1.0));
      (-c +(a-2*b)*a+11.5*(1-b)).Println; // коэффициенты полинома-результата
  
  a:=new Polynom(3,0,-72,12,0,-1,2);
  b:=new Polynom(-1,0,2,1);
  var (p,q):=a/b;
  Print('Частное'); p.PrintlnBeauty;
  Print('остаток'); b.PrintlnBeauty

end.
