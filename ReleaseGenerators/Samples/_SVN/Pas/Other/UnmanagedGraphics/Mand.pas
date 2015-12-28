{
Для создания dll:
1. Откомпилируйте проект \GraphTest\GraphTest.dpr с помощью Delphi
2. Запустите файл \GraphTest\copy_up.bat
}

procedure SetWindowSize(width, height : integer); 	external 'GraphTest.dll' name 'SetWindowSize';
procedure WaitForExit; 								external 'GraphTest.dll' name 'WaitForExit';
procedure PutPixel(x,y,color : integer);			external 'GraphTest.dll' name 'PutPixel';
procedure SetColor(a : integer); 					external 'GraphTest.dll' name 'SetColor';
function RGB(red, green, blue : integer) : integer; external 'GraphTest.dll' name 'RGB';
{function RGB(r, g, b: integer): integer;
begin
  Result := (r or (g shl 8) or (b shl 16));
end;{}

const
  n=255;
  max=10;

var
  x,y,x1,y1,cx,cy: real;
  i,ix,iy: integer;
  clRed:integer;
// z=z^2+c
begin
  clRed:=RGB(255,0,0);
  SetWindowSize(400,300);
  for ix:=0 to 399 do
  for iy:=0 to 299 do
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
    if i>=n then PutPixel(ix,iy,clRed)
      else PutPixel(ix,iy,RGB(255,255-i,255-i));
    //writeln(RGB(255,255-i,255-i));
  end;
  WaitForExit;
end.

