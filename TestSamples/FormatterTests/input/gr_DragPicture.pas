uses ABCObjects,GraphABC,PointRect,Events;

var
  s: PictureABC;
  ob: ObjectABC;

procedure MyMouseDown(x,y,mb: integer);
begin
  ob:=ObjectUnderPoint(x,y);
end;

procedure MyMouseMove(x,y,mb: integer);
begin
  if ob<>nil then
    ob.Center:=new Point(x,y);
end;

procedure MyMouseUp(x,y,mb: integer);
begin
  ob:=nil;
end;


begin
  cls;
  s:=PictureABC.Create(100,100,'demo.bmp');
  OnMouseDown:=MyMouseDown;
  OnMouseMove:=MyMouseMove;
  OnMouseUp:=MyMouseUp;
end.