uses WPFObjects;

var Destroyer: CircleWPF;

procedure CheckPulyaIntersects;
begin
  Destroyer.IntersectionList.ForEach(procedure(o)->o.Destroy);
end;

begin
  HideObjects;
  Window.SetSize(640,480);
  Window.Title := 'Разрушитель: метод Intersect пересечения объектов';
  loop 500 do
    new RectangleWPF(Random(Window.Width.Round-200)+100,Random(Window.Height.Round-100),Random(200),Random(200),clRandom);
  Destroyer := new CircleWPF(10,Window.Height / 2,100,Colors.Black);
  Destroyer.SetText('Destroyer',30,'Arial',Colors.Yellow);
  ShowObjects;

  loop 900 do
  begin
    Destroyer.MoveOn(1,0);
    CheckPulyaIntersects;
    Sleep(1);
  end;
end.
