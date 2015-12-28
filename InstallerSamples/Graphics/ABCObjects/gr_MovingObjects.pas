uses ABCObjects,GraphABC,Utils;

const 
/// Количество графических объектов
  Count = 300;
/// Флаг ускорения анимации
  AnimationIsFast = True;
  
/// Возвращает случайный графический объект
function NewRandomABC: ObjectABC;
begin
  case Random(3) of
0: Result := new CircleABC(Random(WindowWidth-30)+10,Random(WindowHeight-30)+10,Random(10)+5,clRandom);
1: Result := new RectangleABC(Random(WindowWidth-30)+10,Random(WindowHeight-30)+10,Random(20)+10,Random(20)+10,clRandom);
2: Result := new StarABC(Random(WindowWidth-30)+10,Random(WindowHeight-30)+10,Random(20)+10,Random(10)+5,Random(4)+4,clRandom);
  end;
end;

/// Передвигает графический объект с отражением его от стенок
procedure Move(o: ObjectABC);
begin
  o.Move;
  if (o.Left<0) or (o.Left+o.Width>WindowWidth) then
    o.dx := -o.dx;
  if (o.Top<0) or (o.Top+o.Height>WindowHeight) then
    o.dy := -o.dy;
end;

begin
  Window.Title := 'Движущиеся объекты';
  if AnimationIsFast then
    LockDrawingObjects;
  for var i:=1 to Count do
  begin
    var m: ObjectABC := NewRandomABC;
    repeat
      m.dx := Random(-3,3);
      m.dy := Random(-3,3);
    until (m.dx<>0) and (m.dy<>0);
  end;
  var k := 1;
  while True do
  begin
    for var i:=0 to Objects.Count-1 do
      Move(Objects[i]);
    if AnimationIsFast then
      RedrawObjects;
    k += 1;
    Window.Title := Format('{0,5:f2}',k/Milliseconds*1000)+' кадров в секунду';
  end;
end.
