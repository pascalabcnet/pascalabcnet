uses NumLibABC;

// Нахождение действительного нуля функции на интервале изоляции
begin
   var f:real->real := t->sin(t)/(1+Sqr(Exp(t)))-0.1;
   var oL:=new Zeroin(f,1e-12);
   Println(oL.Value(-10,-9.5), oL.Value(-6.5,-6), oL.Value(-3.5,-3),
        oL.Value(0,0.5),oL.Value(1,1.5))
end.
