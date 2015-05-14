unit uRobot;

uses GraphABC;

const
  StandardDelay=100;
  maxDimX=40;
  maxDimY=30;

var
  x,y: integer;       // текущие координаты Робота
  Delay: integer;     // пауза между ходами
  DimX,DimY: integer; // размеры поля
  Cellsz: integer;    // размер клетки поля
  x0,y0: integer;     // левый верхний углл поля в пикселах
  HorizWalls: array [1..maxDimX,0..maxDimY] of boolean;
  VertWalls: array [0..maxDimX,1..maxDimY] of boolean;
  Painted: array [1..maxDimX,1..maxDimY] of integer;
  Tagged: array [1..maxDimX,1..maxDimY] of boolean;

procedure ClearArrays;
var ix,iy: integer;
begin
  for ix:=1 to maxDimX do
  for iy:=0 to maxDimY do
    HorizWalls[ix,iy]:=False;
  for ix:=0 to maxDimX do
  for iy:=1 to maxDimY do
    VertWalls[ix,iy]:=False;
  for ix:=1 to maxDimX do
  for iy:=1 to maxDimY do
  begin
    Painted[ix,iy]:=0;
    Tagged[ix,iy]:=False;
  end;
end;

procedure vw(x,y,len: integer);
var i: integer;
begin
  for i:=y to y+len-1 do
    VertWalls[x,i+1]:=True;
end;

procedure hw(x,y,len: integer);
var i: integer;
begin
  for i:=x to x+len-1 do
    HorizWalls[i+1,y]:=True;
end;

procedure DrawTag(x,y: integer);
var zzz: integer;
begin
  SetPenWidth(1);
  SetPenColor(clBlack);
  SetBrushColor(clBlack);
  zzz:=Cellsz div 2 - 1;
  Rectangle(x0+(x-1)*Cellsz+zzz,y0+(y-1)*Cellsz+zzz,x0+x*Cellsz-zzz+2,y0+y*Cellsz-zzz+2);
end;

procedure DrawPainted(x,y: integer);
const zzz=4;
begin
  SetPenWidth(1);
  SetPenColor(clBlack);
  SetPenStyle(psclear);
  SetBrushColor(clGreen);
  Rectangle(x0+(x-1)*Cellsz+zzz,y0+(y-1)*Cellsz+zzz,x0+x*Cellsz-zzz+2,y0+y*Cellsz-zzz+2);
  SetPenStyle(pssolid);
end;

procedure DrawRobotRect(x,y: integer);
const zz=6;
begin
  SetPenWidth(1);
  SetPenColor(clBlack);
  SetBrushColor(clYellow);
  Rectangle(x0+(x-1)*Cellsz+zz,y0+(y-1)*Cellsz+zz,x0+x*Cellsz-zz+1,y0+y*Cellsz-zz+1);
end;

procedure ClearCell(x,y: integer);
const zz=2;
begin
  SetPenWidth(1);
  SetPenColor(clWhite);
  SetBrushColor(clWhite);
  Rectangle(x0+(x-1)*Cellsz+zz,y0+(y-1)*Cellsz+zz,x0+x*Cellsz-zz+1,y0+y*Cellsz-zz+1);
end;

procedure DrawWalls;
var ix,iy: integer;
begin
  SetPenWidth(3);
  for ix:=1 to DimX do
  for iy:=0 to DimY do
    if HorizWalls[ix,iy] then
      Line(x0+(ix-1)*Cellsz,y0+iy*Cellsz,x0+ix*Cellsz,y0+iy*Cellsz);
  for ix:=0 to DimX do
  for iy:=1 to DimY do
    if VertWalls[ix,iy] then
      Line(x0+ix*Cellsz,y0+(iy-1)*Cellsz,x0+ix*Cellsz,y0+iy*Cellsz);
end;

procedure DrawTags;
var ix,iy: integer;
begin
  SetPenWidth(1);
  SetPenColor(clBlack);
  SetBrushColor(clBlack);
  for ix:=1 to DimX do
  for iy:=1 to DimY do
    if Tagged[ix,iy] then
      DrawTag(ix,iy);
end;

procedure DrawPaints;
var ix,iy: integer;
begin
  SetPenWidth(1);
  SetPenColor(clBlack);
  SetBrushColor(clBlack);
  for ix:=1 to DimX do
  for iy:=1 to DimY do
    if Painted[ix,iy]<>0 then
      DrawPainted(ix,iy);
end;

procedure DrawField;
var cx,cy,w,h: integer;
begin
  w:=Cellsz*DimX;
  h:=Cellsz*DimY;
  SetPenColor(clBlack);
  SetPenWidth(1);
  for cy:=0 to DimY do
    line(x0,y0+cy*Cellsz,x0+w,y0+cy*Cellsz);
  for cx:=0 to DimX do
    line(x0+cx*Cellsz,y0,x0+cx*Cellsz,y0+h);
  DrawWalls;
  DrawTags;
  DrawPaints
end;

procedure InitField(mm,nn,ssz,x,y: integer);
begin
  DimX:=mm; DimY:=nn; Cellsz:=ssz; x0:=x; y0:=y;
  Delay:=StandardDelay;
  ClearArrays;
  vw(0,0,DimY);
  hw(0,0,DimX);
  vw(DimX,0,DimY);
  hw(0,DimY,DimX);
end;

procedure DrawRobot;
const zz=6;
begin
  if Painted[x,y]<>0 then
    DrawPainted(x,y);
  DrawRobotRect(x,y);
  if Tagged[x,y] then
    DrawTag(x,y);
end;

procedure ClearRobot;
const zz=2; zzz=4;
begin
  ClearCell(x,y);
  if Painted[x,y]<>0 then
    DrawPainted(x,y);
  if Tagged[x,y] then
    DrawTag(x,y);
end;

procedure SetRobotCoords(xx,yy: integer);
begin
  x:=xx; y:=yy;
end;

procedure Tag(x,y: integer);
begin
  Tagged[x,y]:=True;
end;

procedure TagRect(x,y,x1,y1: integer);
var ix,iy: integer;
begin
  for ix:=x to x1 do
  for iy:=y to y1 do
    Tagged[ix,iy]:=True;
end;

procedure TagPainted(x,y: integer);
begin
  Painted[x,y]:=2;
end;

procedure Up;
begin
  Sleep(Delay);
  clearRobot;
  y:=y-1;
  drawRobot;
end;

procedure Right;
begin
  Sleep(Delay);
  clearRobot;
  x:=x+1;
  drawRobot;
end;

procedure Down;
begin
  Sleep(Delay);
  clearRobot;
  y:=y+1;
  drawRobot;
end;

procedure Left;
begin
  Sleep(Delay);
  clearRobot;
  x:=x-1;
  drawRobot;
end;

procedure Paint;
begin
  Sleep(Delay);
  clearRobot;
  Painted[x,y]:=1;
  drawRobot;
end;

procedure Speed(i: integer);
begin
  case i of
    1: Delay:=1000;
    2: Delay:=300;
    3: Delay:=100;
    4: Delay:=40;
    5: Delay:=1;
  end;
end;

function WallFromUp: boolean;
begin
  Result:=HorizWalls[x,y-1];
end;

function WallFromDown: boolean;
begin
  Result:=HorizWalls[x,y];
end;

function WallFromLeft: boolean;
begin
  Result:=HorizWalls[x-1,y];
end;

function WallFromRight: boolean;
begin
  Result:=HorizWalls[x,y];
end;

function FreeFromUp: boolean;
begin
  Result:=not WallFromUp;
end;

function FreeFromDown: boolean;
begin
  Result:=not WallFromDown;
end;

function FreeFromLeft: boolean;
begin
  Result:=not WallFromLeft;
end;

function FreeFromRight: boolean;
begin
  Result:=not WallFromRight;
end;

initialization
  SetWindowCaption('Робот');
end.
