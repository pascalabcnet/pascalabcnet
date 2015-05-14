uses ABCObjects,GraphABC;

var r: RectangleABC;
  i: integer;
begin
  r:=RectangleABC.Create(50,100,200,100,clMoneyGreen);
  r.TextVisible:=True;
  r.TextScale:=0.5;
  r.Scale(0.5);
  for i:=1 to 100 do
  begin
    r.RealNumber:=i/10;
    Sleep(100);
  end;
end.

