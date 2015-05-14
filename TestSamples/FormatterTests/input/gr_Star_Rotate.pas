uses ABCObjects,GraphABC;

var
  z: StarABC;
  i: integer;

begin
  z:=new StarABC(WindowWidth div 2,WindowHeight div 2,WindowHeight div 2 - 5,WindowHeight div 4 + 16,6,clRed);
  for i:=1 to 20 do
  begin
    Sleep(100);
    z.Count:=z.Count+1;
  end;
  for i:=1 to 180 do
  begin
    Sleep(10);
    z.Angle:=z.Angle+1;
  end;
end.
