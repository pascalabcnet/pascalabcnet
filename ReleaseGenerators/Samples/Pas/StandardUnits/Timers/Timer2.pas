// "Собачка". Иллюстрация использования таймера.
uses GraphABC, Timers;

var
  t: Timer;
  xx,yy,px,py: integer;
  
procedure Draw;
begin
  FillCircle(xx,yy,11);
end;

procedure Show;
begin
  Brush.Color := clBlack;
  Draw;
end;

procedure Hide;
begin
  Brush.Color := clWhite;
  Draw;
end;

procedure Move(x,y: integer);
begin
  Hide;
  xx := x; 
  yy := y;
  show;
end;

procedure Timer1;
begin
  if (xx<>px) or (yy<>py) then
  begin
    var t := 1/10;
    var newx := round((1-t)*xx+t*px);
    var newy := round((1-t)*yy+t*py);
    Move(newx,newy);
  end;
end;

procedure MouseMove(x,y,mb: integer);
begin
  px := x; py := y;
end;

begin
  SetWindowCaption('"Собачка"');
  SetSmoothingOff;
  OnMouseMove:=MouseMove;
  xx := 100; yy := 100;
  px := xx; py := yy;
  Show;
  Timer1;
  t := new Timer(20,Timer1);
  t.Start;
end.
