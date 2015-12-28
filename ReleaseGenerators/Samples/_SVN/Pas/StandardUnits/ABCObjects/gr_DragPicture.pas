// Передвижение графических объектов мышью
uses ABCObjects,GraphABC;

var 
  ob: ObjectABC;
  sx,sy: integer;

procedure MyMouseDown(x,y,mb: integer);
begin
  ob := ObjectUnderPoint(x,y);
  if ob<>nil then
  begin
    sx := ob.Left - x;
    sy := ob.Top - y;
  end; 
end;

procedure MyMouseMove(x,y,mb: integer);
begin
  if ob<>nil then
    ob.Position := new Point(x+sx,y+sy);
end;

procedure MyMouseUp(x,y,mb: integer);
begin
  ob := nil;
end;


begin
  Window.Title := 'Передвигайте мышью объекты';
  for var i:=1 to 10 do
  begin
    var p := new PictureABC(Random(Window.Width-100),Random(Window.Height-100),'demo.bmp');
    p.Transparent := True;
  end;  
  OnMouseDown := MyMouseDown;
  OnMouseMove := MyMouseMove;
  OnMouseUp := MyMouseUp;
end.