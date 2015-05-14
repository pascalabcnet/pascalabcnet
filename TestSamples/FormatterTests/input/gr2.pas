uses GraphABC,ABCObjects;

var 
  x,y: integer;
  p: Picture;
  i: integer;

begin
  p := Picture.Create('computer.png');
  p.Draw(10,10);
  p.Rectangle(20,20,30,30);
  p.Transparent := False;
  p.Draw(30,10);
  Brush.Color := clGray;
//  Rectangle(10,10,500,400);
{  for i:=1 to 5000 do
  begin
    Brush.Color := clRandom;
    Rectangle(10,10,500,400);
  end;
  TextOut(10,10,'Конец');}

end.
