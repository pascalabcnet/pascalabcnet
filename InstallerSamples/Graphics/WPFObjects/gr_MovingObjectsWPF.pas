uses WPFObjects;

const 
/// Количество графических объектов
  Count = 300;
  
/// Возвращает случайный графический объект
function NewRandomABC: ObjectWPF;
begin
  case Random(3) of
0: Result := new CircleWPF(Random(Window.Width.Round-30)+10,Random(Window.Height.Round-30)+10,Random(10)+5,RandomColor);
1: Result := new RectangleWPF(Random(Window.Width.Round-30)+10,Random(Window.Height.Round-30)+10,Random(20)+10,Random(20)+10,RandomColor);
2: Result := new StarWPF(Random(Window.Width.Round-30)+10,Random(Window.Height.Round-30)+10,Random(20)+10,Random(10)+5,Random(4)+4,RandomColor);
  end;
end;

/// Передвигает графический объект с отражением его от стенок
procedure Move(o: ObjectWPF);
begin
  o.Move;
  if (o.Left<0) or (o.Left+o.Width>Window.Width) then
    o.dx := -o.dx;
  if (o.Top<0) or (o.Top+o.Height>Window.Height) then
    o.dy := -o.dy;
end;

begin
  Window.Title := 'Движущиеся объекты';
  for var i:=1 to Count do
  begin
    var m: ObjectWPF := NewRandomABC;
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
    k += 1;
    Window.Title := Format('{0,5:f2}',k/Milliseconds*1000)+' кадров в секунду';
  end;
end.
