 uses GraphABC;

const
  MAX_DimX = 200;
  MAX_DimY = 200;
  MaxConds = 10000; // сколько максимально условий может проверить –обот - направлено против зацикливани€!
  MaxMoveNum = 10000;

var
  RPaintColor: Color;
  RBorderColor: Color;
  RobotColor: Color;
  RobotColor1: Color;
  BackColor: Color;

type 
  TRobotField = class
  private
    Tags: array [1..MAX_DimX,1..MAX_DimY] of boolean;
    Painted,Painted1: array [1..MAX_DimX,1..MAX_DimY] of integer; // 0-not  1-user painted  2-zadan painted
    HorizWalls: array [1..MAX_DimX,0..MAX_DimY] of boolean;
    VertWalls: array [0..MAX_DimX,1..MAX_DimY] of boolean;
    FirstRobotX, FirstRobotY: integer;
    LastRobotX, LastRobotY: integer;
    RobotX, RobotY: integer;
    RobotErr: boolean;
  public
    CellSize,DimX,DimY: integer;
    constructor Create(sizex,sizey,cellsize: integer);
    procedure DrawFieldOnly;
    procedure DrawCentered;
    procedure Draw0;
    procedure Draw(px,py: integer);
    procedure DrawCell(x,y: integer); { нарисовать одну €чейку }
    procedure Clear;
    function IsSolution: boolean;
    // примитивы дл€ постановщика
    procedure DrawLastRobotPos;
    procedure DrawTag(x,y: integer);
    procedure SetDim(DX,DY,CellSz: integer);
    procedure SetPaintSystem(x,y: integer); { пометить €чейку как нарисованную - постановщик}
    procedure SetTag(x,y: integer); { пометить €чейку как помеченную }
    procedure SetTagRect(x1,y1,x2,y2: integer);
    procedure SetFirstRobotPos(x,y: integer);
    procedure MoveRobotToFirstPos;
    procedure SetLastRobotPos(x,y: integer);
    procedure SetFirstLastRobotPos(x,y,x1,y1: integer);
    procedure HorizWall(x,y,len: integer);
    procedure VertWall(x,y,len: integer);
    // примитивы дл€ выполнител€
    procedure DrawRobot;
    procedure MoveRobot(x,y: integer);
    procedure SetPaintUser(x,y: integer); { пометить €чейку как нарисованную - выполнитель}
    procedure SetRobotPos(x,y: integer); // синоним MoveRobot
    function WallFromLeft(x,y: integer): boolean;
    function WallFromRight(x,y: integer): boolean;
    function WallFromUp(x,y: integer): boolean;
    function WallFromDown(x,y: integer): boolean;
    function CellIsFree(x,y: integer): boolean;
    function CellIsPainted(x,y: integer): boolean;
  end;


constructor TRobotField.Create(sizex,sizey,cellsize: integer);
begin
  Self.CellSize := cellsize;
  DimX := sizex;
  DimY := sizey;
  RobotX := 1;
  RobotY := 1;
  FirstRobotX := 1;
  FirstRobotY := 1;
  LastRobotX := 0;
  LastRobotY := 0;
  RobotErr := False;
  Clear;
end;

procedure TRobotField.Draw(px,py: integer);
begin
  SetCoordinateOrigin(px,py);
  Draw0;
  SetCoordinateOrigin(-px,-py);
end;

procedure TRobotField.DrawCentered;
var w,h: integer;
begin
  w := CellSize*DimX; 
  h := CellSize*DimY;
  Draw((WindowWidth - w) div 2,(WindowHeight - h) div 2);
end;

procedure TRobotField.DrawFieldOnly;
var ix,iy,w,h: integer;
begin
  w := CellSize*DimX; 
  h := CellSize*DimY;
  Pen.Width := 1;
  Pen.Color := RGB(191,191,191);
  for ix:=0 to DimX do
    Line(ix*CellSize,0,ix*CellSize,h);
  for iy:=0 to DimY do
    Line(0,iy*CellSize,w,iy*CellSize);
end;

procedure TRobotField.Draw0;
var x,y: integer;
begin
//  LockDrawing;
  ClearWindow(BackColor);
  DrawFieldOnly;
  Pen.Style:=psSolid;
  Pen.Color:=clBlack;
  Pen.Width:=3;
  for x:=1 to DimX do
  for y:=0 to DimY do
    if HorizWalls[x,y] then
    begin
      MoveTo((x-1)*CellSize,y*CellSize);
      LineTo(x*CellSize,y*CellSize);
    end;
  for x:=0 to DimX do
  for y:=1 to DimY do
    if VertWalls[x,y] then
    begin
      MoveTo(x*CellSize,(y-1)*CellSize);
      LineTo(x*CellSize,y*CellSize);
    end;

  Pen.Style:=psSolid;
  Pen.Color:=clBlack;
  Pen.Width:=1;
  for x:=1 to DimX do
  for y:=1 to DimY do
    DrawCell(x,y);
  Pen.Style:=psSolid;
//  UnLockDrawing;
end;

procedure TRobotField.DrawLastRobotPos;
var Sz,Z,Dim: integer;
begin
  Sz:=CellSize;
  Z:=max(Sz div 12,2);
  if Sz>64 then Dim:=10
  else if Sz>56 then Dim:=9
  else if Sz>48 then Dim:=8
  else if Sz>40 then Dim:=8
  else if Sz>34 then Dim:=7
  else if Sz>28 then Dim:=6
  else if Sz>22 then Dim:=5
  else if Sz>12 then Dim:=4
  else Dim:=3;

  Brush.Color:=RobotColor;
  Pen.Color:=RBorderColor;
  Pen.Style:=psSolid;
  Rectangle(Sz*(LastRobotX-1)+Z,Sz*(LastRobotY-1)+Z,Sz*(LastRobotX-1)+Z+Dim,Sz*(LastRobotY-1)+Z+Dim);
end;

procedure TRobotField.DrawTag(x,y: integer);
var Sz,sz2: integer;
begin
  Sz:=CellSize;
  if Sz>80 then sz2:=Sz div 2 - 4
    else if Sz>52 then sz2:=Sz div 2 - 3
      else if Sz>12 then sz2:=Sz div 2 - 2
        else sz2:=Sz div 2 - 1;

  if (RobotX=x) and (RobotY=y)
    then Brush.Color:=clLightGray
    else Brush.Color:=clBlack;
  FillRectangle(Sz*(x-1)+1+sz2,Sz*(y-1)+1+sz2,Sz*x+1-sz2,Sz*y+1-sz2);
end;

procedure TRobotField.DrawRobot;
var Sz,Z1: integer;
begin
  Sz:=CellSize;
  Z1:=max(Sz div 6,3);
  if CellSize<10 then Z1:=2;
  if Painted[RobotX,RobotY]=0
    then Brush.Color:=RobotColor
    else Brush.Color:=RobotColor1;
  Pen.Color:=RBorderColor;
  Pen.Style:=psSolid;
  Rectangle(Sz*(RobotX-1)+Z1,Sz*(RobotY-1)+Z1,Sz*RobotX+1-Z1,Sz*RobotY+1-Z1);
end;

procedure TRobotField.DrawCell(x,y: integer);
var Sz,Z: integer;
begin
  Sz:=CellSize;
  Z:=max(Sz div 12,2);

  Pen.Style:=psClear;
  if (Painted[x,y]=1) or (Painted[x,y]=2) then
    Brush.Color:=RPaintColor
  else
    Brush.Color:=clWhite;
  if (Painted[x,y]=1) or (Painted[x,y]=2) then
    Rectangle(Sz*(x-1)+Z,Sz*(y-1)+Z,Sz*x+2-Z,Sz*y+2-Z)
  else
    Rectangle(Sz*(x-1)+2,Sz*(y-1)+2,Sz*x+2-2,Sz*y+2-2);

  if (LastRobotX=x) and (LastRobotY=y) then
    DrawLastRobotPos;

  if (RobotX=x) and (RobotY=y) then
    DrawRobot;

  if Tags[x,y] then
    DrawTag(x,y);
end;

procedure TRobotField.SetPaintUser(x,y: integer);
begin
  if Painted[x,y]=0 then Painted[x,y]:=1;
end;

procedure TRobotField.SetPaintSystem(x,y: integer);
begin
  Painted[x,y]:=2;
end;

procedure TRobotField.SetTag(x,y: integer);
begin
  Tags[x,y]:=True;
end;

procedure TRobotField.SetTagRect(x1,y1,x2,y2: integer);
var x,y: integer;
begin
  for x:=x1 to x2 do
  for y:=y1 to y2 do
    SetTag(x,y);
end;

procedure TRobotField.SetFirstRobotPos(x,y: integer);
begin
  FirstRobotX:=x;
  FirstRobotY:=y;
  SetRobotPos(x,y);
end;

procedure TRobotField.MoveRobotToFirstPos;
begin
  SetRobotPos(FirstRobotX,FirstRobotY);
end;

procedure TRobotField.SetLastRobotPos(x,y: integer);
begin
  LastRobotX:=x;
  LastRobotY:=y;
end;

procedure TRobotField.SetFirstLastRobotPos(x,y,x1,y1: integer);
begin
  SetFirstRobotPos(x,y);
  SetLastRobotPos(x1,y1);
end;

procedure TRobotField.MoveRobot(x,y: integer);
var vx,vy: integer;
begin
  vx:=RobotX;                                  
  vy:=RobotY;
  RobotX:=x;
  RobotY:=y;
  DrawCell(vx,vy);
  DrawCell(x,y);
end;

procedure TRobotField.SetRobotPos(x,y: integer);
begin
  MoveRobot(x,y);
end;

function TRobotField.WallFromLeft(x,y: integer): boolean;
begin
  WallFromLeft:=VertWalls[x-1,y];
end;

function TRobotField.WallFromRight(x,y: integer): boolean;
begin
  WallFromRight:=VertWalls[x,y];
end;

function TRobotField.WallFromUp(x,y: integer): boolean;
begin
  WallFromUp:=HorizWalls[x,y-1];
end;

function TRobotField.WallFromDown(x,y: integer): boolean;
begin
  WallFromDown:=HorizWalls[x,y];
end;

function TRobotField.CellIsFree(x,y: integer): boolean;
begin
  CellIsFree:=Painted[x,y]=0;
end;

function TRobotField.CellIsPainted(x,y: integer): boolean;
begin
  CellIsPainted:=Painted[x,y]<>0;
end;

procedure TRobotField.SetDim(DX, DY, CellSz: integer);
begin
  Clear;
  DimX := DX;
  DimY := DY;
  CellSize := CellSz;
  HorizWall(1,0,DimX);
  HorizWall(1,DimY,DimX);
  VertWall(0,1,DimY);
  VertWall(DimX,1,DimY);
  DrawCentered;
end;

procedure TRobotField.HorizWall(x,y,len: integer);
var xx: integer;
begin
  for xx:=x to x+len-1 do
    HorizWalls[xx,y] := True;
end;

procedure TRobotField.VertWall(x,y,len: integer);
var yy: integer;
begin
  for yy:=y to y+len-1 do
    VertWalls[x,yy] := True;
end;

procedure TRobotField.Clear;
var x,y: integer;
begin
  for x:=1 to Max_DimX do
  for y:=0 to Max_DimY do
    HorizWalls[x,y] := False;
  for x:=0 to Max_DimX do
  for y:=1 to Max_DimY do
    VertWalls[x,y] := False;
  for x:=1 to Max_DimX do
  for y:=1 to Max_DimY do
  begin
    Painted[x,y] := 0;
    Tags[x,y] := False;
  end;
  LastRobotX:=1;
  LastRobotY:=1;
  HorizWall(1,0,DimX);
  HorizWall(1,DimY,DimX);
  VertWall(0,1,DimY);
  VertWall(DimX,1,DimY);
end;

function TRobotField.IsSolution: boolean;
label 1;
var x,y: integer;
    ID: boolean;
begin
  ID:=True;
  for x:=1 to DimX do
  for y:=1 to DimY do
  begin
    ID := ID and (((Tags[x,y]=True) and ((Painted[x,y]=1) or (Painted[x,y]=2))) or ((Tags[x,y]=False) and ((Painted[x,y]=0) or (Painted[x,y]=1) or (Painted[x,y]=2))));
    if not ID then goto 1;
  end;
  ID := ID and (RobotX=LastRobotX) and (RobotY=LastRobotY);
1:
  IsSolution := ID and not RobotErr;
end;
 

var 
  f: TRobotField;
  i: integer;

begin
  RPaintColor := RGB(0,200,0);
  RobotColor := RGB(255,255,220);
  RobotColor1 := RGB(200,255,200);
  RBorderColor := RGB(1,1,1);
  BackColor := clWhite;

  CenterWindow;
  f := new TRobotField(5,4,50);
  f.HorizWall(2,1,3);
  f.VertWall(1,2,2);
  f.SetTagRect(2,2,4,3);
  f.SetPaintUser(2,2);
  f.DrawCentered;
{  for i:=50 to 70 do
  begin
    f.CellSize := i;
    ClearWindow;
    f.DrawCentered;
    Sleep(100);
  end;}
end.