uses NumLibABC;

// Случайный поиск (функция с ограничениями)

function f(x:array of real):real;
begin
  var x1:=x[0];
  var x2:=x[1];
  var s:=0.0; // штрафная функция
  if x1+x2>8 then s:=real.MaxValue
  else if -2*x1+3*x2>9 then s:=real.MaxValue
  else if 2*x1-x2>10 then s:=real.MaxValue
  else if x1<0 then s:=real.MaxValue
  else if x2<0 then s:=real.MaxValue;
  Result:=-4*x1-3*x2+1+s
end;

begin
  var a:=Arr(0.0,0.0);
  var b:=Arr(8.0,8.0);
  var y:real;
  var oL:=new FMinN(a,f);
  oL.MKSearch(a,b,y);
  oL.x.Transform(t->real(Round(t)));
  Write('Полученные значения аргументов: '); oL.x.Println;
  Writeln('Полученное значение функции: ',f(oL.x))
end.
