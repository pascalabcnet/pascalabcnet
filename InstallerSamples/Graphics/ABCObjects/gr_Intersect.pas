// Иллюстрация метода Intersect для графических объектов
uses ABCObjects,GraphABC;

var Destroyer: CircleABC;

procedure CheckPulyaIntersects;
begin
  for var i:=Objects.Count-1 downto 0 do
  begin
    if (Destroyer.Intersect(Objects[i])) and (Objects[i]<>Destroyer) then
      Objects[i].Destroy;
  end;    
end;

begin
  Window.Title := 'Разрушитель: метод Intersect пересечения объектов';
  for var i:=1 to 500 do
    new RectangleABC(Random(WindowWidth-200)+100,Random(WindowHeight-100),Random(200),Random(200),clRandom);
  Destroyer := new CircleABC(10,WindowHeight div 2,100,clBlack);
  Destroyer.FontColor := clYellow;
  Destroyer.Text := 'Destroyer';

  for var i:=1 to 900 do
  begin
    Destroyer.MoveOn(1,0);
    CheckPulyaIntersects;
  end;
end.
