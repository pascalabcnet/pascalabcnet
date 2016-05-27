// Процедура drawGraph рисования графика функции в полном окне
// с масштабированием по оси OY
// Перерисовывает график при изменении размеров окна
uses GraphABC;

function f(x: real) := x*sin(x)*exp(-0.1*x);

// l (logical) - логические координаты
// s (screen) - физические координаты
procedure drawGraph(x1,x2: real; f: real -> real);
 var
  xl0,wl,yl0,hl: real;
  xs0,ws,ys0,hs: integer;

 function LtoSx(xl: real) := round(ws/wl*(xl-xl0)+xs0);
 function LtoSy(yl: real) := round(hs/hl*(yl-yl0)+ys0);
 function StoLx(xs: integer) := wl/ws*(xs-xs0)+xl0;

begin // drawGraph
  xs0 := 0;
  ys0 := WindowHeight-1;
  ws := WindowWidth;
  hs := WindowHeight-1;
  
  xl0 := x1;
  wl := x2-x1;

  var yi: array of real;
  SetLength(yi,ws+1);

  var min := real.MaxValue;
  var max := real.MinValue;
  for var xi:=0 to ws do
  begin
    yi[xi] := f(StoLx(xi+xs0));
    if yi[xi]<min then 
      min := yi[xi];
    if yi[xi]>max then 
      max := yi[xi];
  end;
  
  yl0 := min;
  hl := -(max-min);

  // Нарисовать оси системы координат
  Line(0,LtoSy(0),ws,LtoSy(0));
  Line(LtoSx(0),0,LtoSx(0),hs);

  Pen.Color := clBlue;
  MoveTo(xs0,LtoSy(yi[0]));
  for var xi:=xs0+1 to xs0+ws do
    LineTo(xi,LtoSy(yi[xi-xs0]));
end;

procedure Resize;
begin
  ClearWindow;
  drawGraph(0,60,f);
  Redraw;
end;

begin
  LockDrawing;
  SetWindowCaption('График функции: масштабирование');
  drawGraph(0,60,f);
  Redraw;
  OnResize := Resize;
end.
