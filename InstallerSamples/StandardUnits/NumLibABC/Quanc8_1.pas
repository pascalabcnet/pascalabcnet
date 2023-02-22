uses NumLibABC;

// Адаптивная квадратурная программа

begin
  var f:real->real := x->x=0?1.0:sin(x)/x;
  var oL := new Quanc8(f,0,2,1e-7,0);
  Writeln(oL.Value);
end.
