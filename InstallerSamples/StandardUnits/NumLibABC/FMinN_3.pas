uses NumLibABC;

// Случайный поиск BPHS
begin
  var f:function(x:array of real):real:= x->Power(x[0],4)+
      Power(x[1],4)-2*Sqr(x[0])+4*x[0]*x[1]-2*Sqr(x[1])+3;
  var a:=Arr(-20.0,-20.0); // нижние границы
  var b:=Arr(20.0,20.0); // верхние границы
  var y:real; // значение функции
  var oL:=new FMinN(a,f);
  oL.BPHS(a,b,y);
  Write('Вектор аргументов: '); oL.x.Println;
  Writeln('Значение функции: ', y);
end.
