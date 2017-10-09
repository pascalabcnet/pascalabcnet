uses NumLibABC;

// Случайный поиск BestP
begin
  var f:function(x:array of real):real:= x->Power(x[0],4)+
      Power(x[1],4)-2*Sqr(x[0])+4*x[0]*x[1]-2*Sqr(x[1])+3;
  var a:=Arr(-20.0,-20.0); // нижние границы
  var b:=Arr(20.0,20.0); // верхние границы
  var x:=new real[a.Length]; // для конструктора MinHJ
  var oL:=new FMinN(x,f);
  var r:=oL.BestP(a,b,0.01);
  var y:real;
  var fet:=f(Arr(Sqrt(2),-Sqrt(2)));
  foreach var t in r do begin
    (y,x):=(t[0],t[1]);
    Write('Вектор аргументов: '); x.Println;
    Write('Абс.погрешности: ');
    x.Foreach(z->WriteFormat('{0:0.0e0} ',Abs(z)-Sqrt(2)));
    Writeln;
    Writeln('Значение функции: ', y, ', абс.погрешность: ',Abs(y-fet));
    Writeln
    end
end.
