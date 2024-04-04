uses NumLibABC;

// Нахождение всех корней полинома с действительными коэффициентами
// методом Ньютона —Рафсона 
begin
  var p:=new Polynom(-609, -283 ,294, -38, -5,1);
  var oL:=new PolRt(p);
  if oL.ier=0 then oL.Value.Println
  else Writeln('Ошибка: ier=',oL.ier);
end.
