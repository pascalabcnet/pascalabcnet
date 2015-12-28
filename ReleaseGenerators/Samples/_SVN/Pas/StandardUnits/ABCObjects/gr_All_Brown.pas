// Изменение свойств графических объектов
// Броуновское движение графических объектов
uses ABCObjects,GraphABC;

procedure MoveAll(a,b: integer);
begin
  for var j:=0 to Objects.Count-1 do
    Objects[j].moveOn(a,b);
end;

begin
//  LockDrawingObjects;
  Window.Title := 'Броуновское движение объектов';
  var sq := new SquareABC(30,5,90,clSkyBlue);
  var r := new RectangleABC(10,10,100,180,RGB(255,100,100));
  var rr := new RoundRectABC(200,180,180,50,20,clRandom);
  var rsq:= new RoundSquareABC(20,180,80,10,clRandom);
  var c := new CircleABC(160,55,70,clGreen);
  var z := new StarABC(200,150,70,135,5,clRandom);
  z.Filled := False;
  var el := new EllipseABC(5,55,65,50,clRandom);
  el.Bordered := False;
  var t := new TextABC(100,170,15,'Hello, ABCObjects!');
  var br := new BoardABC(200,20,7,5,20,20);
  br.Filled := False;
  z.Height := 200;
  z.Radius := 70;
  sq.Width := 120;
  t.TransparentBackground := False;
  t.BackgroundColor := clYellow;
  t.FontName := 'Times New Roman';
  t.FontSize := 20;
  c.Height := 50;
  c.Scale(2);
  MoveAll(160,110);
  
  while True do
  begin
    for var j:=0 to Objects.Count-1 do
      Objects[j].moveOn(Random(-1,1),Random(-1,1));
//    RedrawObjects;
  end;
end.
