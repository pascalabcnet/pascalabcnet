uses WPFObjects;

var Destroyer: CircleWPF;

procedure CheckPulyaIntersects;
begin
  Destroyer.IntersectionList.ForEach(procedure(o)->o.Destroy);
end;

begin
  Window.SetSize(640,480);
  Window.Title := 'Разрушитель: метод Intersect пересечения объектов';
  for var i:=1 to 500 do
    new RectangleWPF(Random(Window.Width.Round-200)+100,Random(Window.Height.Round-100),Random(200),Random(200),clRandom);
  Destroyer := new CircleWPF(10,Window.Height.Round div 2,100,Colors.Black);
  Destroyer.FontColor := Colors.Yellow;
  Destroyer.Text := 'Destroyer';

  for var i:=1 to 900 do
  begin
    Destroyer.MoveOn(1,0);
    CheckPulyaIntersects;
    Sleep(1);
  end;
end.
