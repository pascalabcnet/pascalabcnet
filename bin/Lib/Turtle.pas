/// Исполнитель Черепаха
unit Turtle;

uses GraphWPF;

var 
  tp := Window.Center;
  a: real := 0;
  dr := False;

/// Поворачивает Черепаху на угол da против часовой стрелки
procedure Turn(da: real);
begin
  a += da;
end;

/// Продвигает Черепаху вперёд на расстояние r
procedure Forw(r: real);
begin
  tp += r * Vect(Cos(DegToRad(a)),Sin(DegToRad(a)));
  if dr then 
    LineTo(tp.X,tp.Y)
  else MoveTo(tp.X,tp.Y)
end;

/// Опускает хвост Черепахи
procedure Down := dr := True;

/// Поднимает хвост Черепахи
procedure Up := dr := False;

/// Устанавливает ширину линии 
procedure SetWidth(w: real) := Pen.Width := w;

/// Устанавливает цвет линии 
procedure SetColor(c: GColor) := Pen.Color := c;

/// Перемещает Черепаху в точку (x,y)
procedure ToPoint(x,y: real);
begin
  tp := Pnt(x,y);
  MoveTo(tp.X,tp.Y);
end;
  

begin
  pen.RoundCap := True;
  MoveTo(tp.X,tp.Y) 
end.