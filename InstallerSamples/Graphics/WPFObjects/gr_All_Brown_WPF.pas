// Изменение свойств графических объектов
// Броуновское движение графических объектов
uses WPFObjects;

procedure MoveAll(a,b: integer);
begin
  for var j:=0 to Objects.Count-1 do
    Objects[j].moveOn(a,b);
end;

begin
  Window.Title := 'Броуновское движение объектов';
  HideObjects;
  var sq := new SquareWPF(30,5,90,Colors.SkyBlue,1);
  var r := new RectangleWPF(10,10,100,180,RGB(255,100,100),1);
  var rr := new RoundRectWPF(200,180,180,50,20,RandomColor,1);
  var rsq:= new RoundSquareWPF(20,180,80,10,RandomColor,1);
  var c := new CircleWPF(160,55,70,Colors.Green);
  var z := new StarWPF(200,150,70,135,5,EmptyColor,1);  
  var el := new EllipseWPF(5,55,65,50,RandomColor,2,RandomColor);
  var t := new TextWPF(100,170,40,'Hello, WPFObjects!'); //
  t.BackgroundColor := Colors.Yellow;
  t.FontName := 'Times New Roman';
  //var br := new BoardWPF(200,20,7,5,20,20);
  //br.Filled := False;
  z.Height := 220;
  z.Radius := 160;
  sq.Width := 120;
  //t.FontSize := 40;
  c.Height := 50;
  c.Scale(2);
  MoveAll(160,110);
  ShowObjects;
  
  while True do
  begin
    for var j:=0 to Objects.Count-1 do
      Objects[j].MoveOn(Random(-1,1),Random(-1,1));
    Sleep(10);
  end;
end.
