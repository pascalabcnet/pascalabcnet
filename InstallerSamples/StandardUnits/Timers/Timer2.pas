// "Собачка". Иллюстрация использования таймера.
uses GraphWPF, Timers;

var
  xx,yy,px,py: real;
  
procedure Draw;
begin
  FillCircle(xx,yy,11);
end;

procedure TimerProc;
begin
  if (xx<>px) or (yy<>py) then
  begin
    var t := 1/10;
    var newx := round((1-t)*xx+t*px);
    var newy := round((1-t)*yy+t*py);
    (xx,yy) := (newx,newy);
  end;
end;

begin
  Window.Title := '"Собачка"';
  Brush.Color := Colors.Black;

  xx := 100; yy := 100;
  
  OnMouseMove := procedure(x,y,mb) -> (px,py) := (x,y);

  OnDrawFrame := dt -> begin
    FillCircle(xx,yy,11);
  end;
  
  var t := new Timer(20,TimerProc);
  t.Start;
end.
