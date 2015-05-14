uses ABCObjects,GraphABC;

function CreateRandomABC: ObjectABC;
begin
  case Random(3) of
0: Result:=CircleABC.Create(Random(WindowWidth-30)+10,Random(WindowHeight-30)+10,Random(10)+5,clRandom);
1: Result:=RectangleABC.Create(Random(WindowWidth-30)+10,Random(WindowHeight-30)+10,Random(20)+10,Random(20)+10,clRandom);
2: Result:=StarABC.Create(Random(WindowWidth-30)+10,Random(WindowHeight-30)+10,Random(20)+10,Random(10)+5,Random(4)+4,clRandom);
  end;
end;

procedure Move(o: ObjectABC);
begin
  o.MoveOn(o.dx,o.dy);
  if (o.Left<0) or (o.Left+o.Width>WindowWidth) then
    o.dx:=-o.dx;
  if (o.Top<0) or (o.Top+o.Height>WindowHeight) then
    o.dy:=-o.dy;
end;

const n = 200;

var
  m: ObjectABC;
  i: integer;

begin
  SetWindowCaption('Движущиеся объекты');
  //SetSmoothingOff;
  LockDrawingObjects;
  for i:=1 to n do
  begin
    m:=CreateRandomABC;
    repeat
      m.dx:=Random(7)-3;
      m.dy:=Random(7)-3;
    until (m.dx<>0) and (m.dy<>0);
  end;
  while True do
  begin
    for i:=0 to Objects.Count-1 do
      Move(Objects[i]);
    RedrawObjects;
  end;
end.
