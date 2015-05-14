// Демонстрация фрактальной графики. Множество Мандельброта
uses GraphABC;

const
  n=255;
  max=10;

var
  x,y,x1,y1,cx,cy: real;
  i,ix,iy: integer;
// z=z^2+c
begin
  SetWindowSize(400,300);
  SetWindowCaption('Фракталы: множество Мандельброта');
  for ix:=0 to WindowWidth-1 do
  for iy:=0 to WindowHeight-1 do
  begin
    x:=0;
    y:=0;
    cx:=0.002*(ix-720);
    cy:=0.002*(iy-150);
    for i:=1 to n do
    begin
      x1:=x*x-y*y+cx;
      y1:=2*x*y+cy;
      if (x1>max) or (y1>max) then break;
      x:=x1;
      y:=y1;
    end;
    if i>=n then SetPixel(ix,iy,clRed)
      else SetPixel(ix,iy,RGB(255,255-i,255-i));
  end;
end.
