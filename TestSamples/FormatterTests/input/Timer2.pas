// "Собачка". Иллюстрация использования таймера.
uses GraphABC, Timers;

var
  t: Timer;
  xx,yy,px,py: integer;
 
procedure Draw;
begin
  Circle(xx,yy,5);
end;

procedure Show;
begin
  SetPenColor(clBlack);
  SetBrushColor(clBlack);
  Draw;
end;

procedure Hide;
begin
  SetPenColor(clWhite);
  SetBrushColor(clWhite);
  Draw;
end;

procedure Move(x,y: integer);
begin
  Hide;
  xx:=x; yy:=y;
  show;
end;

procedure Timer1;
var
  t: real;
  newx,newy: integer;
begin
  if (xx<>px) or (yy<>py) then
  begin
    t:=1/10;
    newx:=round((1-t)*xx+t*px);
    newy:=round((1-t)*yy+t*py);
    Move(newx,newy);
  end;
end;

procedure MouseMove(x,y,mb: integer);
begin
  px:=x; py:=y;
end;

begin
  SetWindowCaption('"Собачка"');
  OnMouseMove:=MouseMove;
  xx:=100; yy:=100;
  px:=xx; py:=yy;
  Show;
  Timer1;
  t := new Timer(20,Timer1);
  t.Start;
end.
