// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
unit RobotField;

//#savepcu false

interface

uses GraphABC;

const
  IniFileName = 'ABC.ini';
  MAX_DimX = 100;
  MAX_DimY = 100;

var
  RPaintColor: Color;
  RBorderColor: Color;
  RobotColor: Color;
  RobotColor1: Color;
  BackColor: Color;

  LabelReadyColor: Color;
  LabelDoColor: Color;
  LabelStopColor: Color;
  LabelErrorColor: Color;
  LabelBadEndColor: Color;
  LabelGoodEndColor: Color;
  
type 
  TRobotField = class
  private
    Tags: array [1..MAX_DimX,1..MAX_DimY] of boolean;
    Painted: array [1..MAX_DimX,1..MAX_DimY] of integer; // 0-not  1-user painted  2-zadan painted
    HorizWalls: array [1..MAX_DimX,0..MAX_DimY] of boolean;
    VertWalls: array [0..MAX_DimX,1..MAX_DimY] of boolean;
    FirstRobotX, FirstRobotY: integer;
    LastRobotX, LastRobotY: integer;
    RobotX, RobotY: integer;
    Interval: integer;
    StepFlag: boolean;
    movenum: integer;
    CellSize,DimX,DimY,X0,Y0: integer;
    procedure Draw0;
    procedure DrawFieldOnly;
    procedure DrawCell(x,y: integer); { нарисовать одну ячейку }
  public
    // Добавил - МА
    TaskName: string;  
    //
    constructor Create(sizex,sizey,cellsize: integer);
    procedure DrawCentered;
    procedure Draw;
    procedure Clear;
    procedure SetSpeed(speed: integer);
    property StepState: boolean read StepFlag;
    property SizeX: integer read DimX;
    property SizeY: integer read DimY;
    function IsSolution: boolean;
    procedure CheckTaskCall;
    // примитивы для постановщика
    procedure DrawLastRobotPos;
    procedure DrawTag(x,y: integer);
    procedure SetDim(DX,DY,CellSz: integer);
    procedure SetPaintMaker(x,y: integer); { пометить ячейку как нарисованную - постановщик}
    procedure SetTag(x,y: integer); { пометить ячейку как помеченную }
    procedure SetTagRect(x1,y1,x2,y2: integer);
    procedure SetFirstRobotPos(x,y: integer);
    procedure MoveRobotToFirstPos;
    procedure SetLastRobotPos(x,y: integer);
    procedure SetFirstLastRobotPos(x,y,x1,y1: integer);
    procedure HorizWall(x,y,len: integer);
    procedure VertWall(x,y,len: integer);
    procedure TaskText(s: string);
    // примитивы для выполнителя
    procedure DrawRobot;
    procedure MoveRobot(x,y: integer);
    procedure SetPaintUser(x,y: integer); { пометить ячейку как нарисованную - выполнитель}
    procedure SetRobotPos(x,y: integer); // синоним MoveRobot
    function WallFromLeft(x,y: integer): boolean;
    function WallFromRight(x,y: integer): boolean;
    function WallFromUp(x,y: integer): boolean;
    function WallFromDown(x,y: integer): boolean;
    function CellIsFree(x,y: integer): boolean;
    function CellIsPainted(x,y: integer): boolean;
    
    procedure Right;
    procedure Left;
    procedure Up;
    procedure Down;
    procedure Paint;
    procedure Pause;

    function WallFromLeft: boolean;
    function WallFromRight: boolean;
    function WallFromUp: boolean;
    function WallFromDown: boolean;
    function CellIsFree: boolean;
    function CellIsPainted: boolean;
    
    procedure Start;
    procedure Stop;
  end;

procedure CorrectWHLT;
procedure SetTaskCall;

procedure RobotError(s: string);
procedure RobotError(s,s1: string);

var 
  RobField: TRobotField;

///--
procedure __InitModule__;
///--
procedure __FinalizeModule__;

implementation

uses 
  System,
  System.Windows.Forms,
  System.Drawing,
  GraphABC,
  IniFile;

type
  TLabel = System.Windows.Forms.Label;
  IniSettings = record
    Width: integer;
    Height: integer;
    Left: integer;
    Top: integer;
    Speed: integer;
  end;

var 
  BottomPanel: Panel;
  panelBottomLeft: Panel;
  buttonHelp: Button;
  buttonExit: Button;
  buttonStep: Button;
  buttonStart: Button;
  tableLayoutPanelBottom: TableLayoutPanel;
  labelExState: TLabel;
  groupBoxExState: GroupBox;
  tableLayoutPanel1: TableLayoutPanel;
  labelState: TLabel;
  labelStep: TLabel;
  trackBarSpeed: TrackBar;
  labelSpeed: TLabel;
  tableLayoutPanelTop: TableLayoutPanel;
  labelZad: TLabel;
  
  TaskIsCalled: boolean;

  t: System.Threading.Thread;
  
//------------ Вспомогательные -------------
procedure SetLabelStepText(s: string);
begin
  labelStep.Text := s;
end;

procedure SetSafeLabelStepText(s: string);
begin
  labelStep.Invoke(SetLabelStepText,s);
end;

procedure SetLabelZadText(s : string);
begin
  labelZad.Text := s;
end;

procedure SetSafeLabelZadText(s: string);
begin
  labelZad.Invoke(SetLabelZadText,s);
end;

procedure SetTrackBarSpeedValue(speed: integer);
begin
  trackBarSpeed.Value := speed;
end;

procedure SetSafeTrackBarSpeedValue(speed: integer);
begin
  trackBarSpeed.Invoke(SetTrackBarSpeedValue,speed);
end;

//------------ TRobotField -------------
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
  X0 := 0;
  y0 := 0;
  StepFlag := False;
  movenum := 0;
  SetSpeed(3);
  Clear;
end;

procedure TRobotField.Draw;
begin
  if (MainForm.windowstate=formwindowstate.Minimized) then
    Exit;
  LockDrawing;
  ClearWindow(BackColor);
  Draw0;
  UnLockDrawing;
end;

procedure TRobotField.DrawCentered;
var w,h: integer;
begin
  if (DimX=0) or (DimY=0) then
    exit;
  w := CellSize*DimX; 
  h := CellSize*DimY;
  X0 := (GraphABCControl.Width - w) div 2;
  Y0 := (GraphABCControl.Height - h) div 2;
  Draw;
end;

procedure TRobotField.DrawFieldOnly;
var ix,iy,w,h: integer;
begin
  w := CellSize*DimX; 
  h := CellSize*DimY;
  Brush.Color := clWhite;
  FillRectangle(X0,Y0,X0+w+1,Y0+h+1);
  Pen.Width := 1;
  Pen.Color := RGB(191,191,191);
  for ix:=0 to DimX do
    Line(X0+ix*CellSize,Y0,X0+ix*CellSize,Y0+h);
  for iy:=0 to DimY do
    Line(X0,Y0+iy*CellSize,X0+w,Y0+iy*CellSize);
end;

procedure TRobotField.Draw0;
var x,y: integer;
begin
  DrawFieldOnly;
  Pen.Color:=clBlack;
  Pen.Width:=3;
  for x:=1 to DimX do
  for y:=0 to DimY do
    if HorizWalls[x,y] then
    begin
      MoveTo(X0+(x-1)*CellSize,Y0+y*CellSize);
      LineTo(X0+x*CellSize,Y0+y*CellSize);
    end;
  for x:=0 to DimX do
  for y:=1 to DimY do
    if VertWalls[x,y] then
    begin
      MoveTo(X0+x*CellSize,Y0+(y-1)*CellSize);
      LineTo(X0+x*CellSize,Y0+y*CellSize);
    end;

  Pen.Color:=clBlack;
  Pen.Width:=1;
  for x:=1 to DimX do
  for y:=1 to DimY do
    DrawCell(x,y);
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
  Rectangle(X0+Sz*(LastRobotX-1)+Z,Y0+Sz*(LastRobotY-1)+Z,X0+Sz*(LastRobotX-1)+Z+Dim,Y0+Sz*(LastRobotY-1)+Z+Dim);
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
  FillRectangle(X0+Sz*(x-1)+1+sz2,Y0+Sz*(y-1)+1+sz2,X0+Sz*x+1-sz2,Y0+Sz*y+1-sz2);
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
  Rectangle(X0+Sz*(RobotX-1)+Z1,Y0+Sz*(RobotY-1)+Z1,X0+Sz*RobotX+1-Z1,Y0+Sz*RobotY+1-Z1);
end;

procedure TRobotField.DrawCell(x,y: integer);
var Sz,Z: integer;
begin
  Sz:=CellSize;
  Z:=max(Sz div 12,2);

  if (Painted[x,y]=1) or (Painted[x,y]=2) then
    Brush.Color:=RPaintColor
  else
    Brush.Color:=clWhite;
  if (Painted[x,y]=1) or (Painted[x,y]=2) then
    FillRectangle(X0+Sz*(x-1)+Z,Y0+Sz*(y-1)+Z,X0+Sz*x+2-Z-1,Y0+Sz*y+2-Z-1)
  else
    FillRectangle(X0+Sz*(x-1)+2,Y0+Sz*(y-1)+2,X0+Sz*x+2-3,Y0+Sz*y+2-3);

  if (LastRobotX=x) and (LastRobotY=y) then
    DrawLastRobotPos;

  if (RobotX=x) and (RobotY=y) then
    DrawRobot;

  if Tags[x,y] then
    DrawTag(x,y);
end;

procedure TRobotField.SetPaintUser(x,y: integer);
begin
  if Painted[x,y]=0 then 
    Painted[x,y]:=1;
end;

procedure TRobotField.SetPaintMaker(x,y: integer);
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
  Inc(movenum);
  SetSafeLabelStepText('Шаг: ' + IntToStr(movenum));
//  labelStep.Invoke(SetLabelStepText,'Шаг: ' + IntToStr(movenum));
end;

procedure TRobotField.SetRobotPos(x,y: integer);
begin
  MoveRobot(x,y);
end;

function TRobotField.WallFromLeft(x,y: integer): boolean;
begin
  CheckTaskCall;
  WallFromLeft:=VertWalls[x-1,y];
end;

function TRobotField.WallFromRight(x,y: integer): boolean;
begin
  CheckTaskCall;
  WallFromRight:=VertWalls[x,y];
end;

function TRobotField.WallFromUp(x,y: integer): boolean;
begin
  CheckTaskCall;
  WallFromUp:=HorizWalls[x,y-1];
end;

function TRobotField.WallFromDown(x,y: integer): boolean;
begin
  CheckTaskCall;
  WallFromDown:=HorizWalls[x,y];
end;

function TRobotField.CellIsFree(x,y: integer): boolean;
begin
  CheckTaskCall;
  CellIsFree:=Painted[x,y]=0;
end;

function TRobotField.CellIsPainted(x,y: integer): boolean;
begin
  CheckTaskCall;
  CellIsPainted:=Painted[x,y]<>0;
end;

function TRobotField.WallFromLeft: boolean;
begin
  Result := WallFromLeft(RobotX,RobotY);
end;

function TRobotField.WallFromRight: boolean;
begin
  Result := WallFromRight(RobotX,RobotY);
end;

function TRobotField.WallFromUp: boolean;
begin
  Result := WallFromUp(RobotX,RobotY);
end;

function TRobotField.WallFromDown: boolean;
begin
  Result := WallFromDown(RobotX,RobotY);
end;

function TRobotField.CellIsFree: boolean;
begin
  Result := CellIsFree(RobotX,RobotY);
end;

function TRobotField.CellIsPainted: boolean;
begin
  Result := CellIsPainted(RobotX,RobotY);
end;

procedure TRobotField.CheckTaskCall;
begin
  if not TaskIsCalled then
    RobotError('Процедура Task вызова задания должна быть первой','Выполнение программы прервано');
end;

procedure TRobotField.Right;
begin
  CheckTaskCall;
  if not WallFromRight then 
    MoveRobot(RobotX+1,RobotY)
  else RobotError('Робот: врезался в правую стену');
end;

procedure TRobotField.Left;
begin
  CheckTaskCall;
  if not WallFromLeft then 
    MoveRobot(RobotX-1,RobotY)
  else RobotError('Робот: врезался в левую стену');
end;

procedure TRobotField.Up;
begin
  CheckTaskCall;
  if not WallFromUp then 
    MoveRobot(RobotX,RobotY-1)
  else RobotError('Робот: врезался в верхнюю стену');
end;

procedure TRobotField.Down;
begin
  CheckTaskCall;
  if not WallFromDown then 
    MoveRobot(RobotX,RobotY+1)
  else RobotError('Робот: врезался в нижнюю стену');
end;

procedure TRobotField.Paint;
begin
  CheckTaskCall;
  Inc(movenum);
  SetSafeLabelStepText('Шаг: ' + IntToStr(movenum));
//  labelStep.Text := 'Шаг: ' + IntToStr(movenum);
  SetPaintUser(RobotX,RobotY);
  DrawCell(RobotX,RobotY);
end;

procedure TRobotField.Pause;
begin
  Sleep(Interval);
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
//  DrawCentered;
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

procedure TRobotField.TaskText(s: string);
begin
  //labelZad.Text := s;
  //labelZad.Invoke(SetSafeText,s);
  SetSafeLabelZadText(s);
end;

procedure TRobotField.SetSpeed(speed: integer);
begin
  if speed<0 then speed := 0;
  case speed of
 0: Interval := 2*1024;
 1: Interval := 2*512;
 2: Interval := 2*256;
 3: Interval := 2*128;
 4: Interval := 2*64;
 5: Interval := 2*32;
 6: Interval := 2*16;
 7: Interval := 2*8;
 8: Interval := 2*4;
 9: Interval := 2*2;
 10: Interval := 2*1;
 1000: Interval := 0;
  end;
  if speed>10 then speed := 10;
  SetSafeTrackBarSpeedValue(speed);
end;

procedure TRobotField.Stop;
begin
  buttonStart.Text := 'Пуск (Enter)';
  buttonStep.Enabled := True;
  t.Suspend;
end;
 
procedure TRobotField.Start;
begin
  buttonStart.Text := 'Стоп (Enter)';
  buttonStep.Enabled := False;
  t.Resume;
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
{  HorizWall(1,0,DimX);
  HorizWall(1,DimY,DimX);
  VertWall(0,1,DimY);
  VertWall(DimX,1,DimY);}
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
    //ID := ID and (((Tags[x,y]=True) and ((Painted[x,y]=1) or (Painted[x,y]=2))) or ((Tags[x,y]=False) and ((Painted[x,y]=0) or (Painted[x,y]=2))));
    ID := ID and (((Tags[x,y]=True) and (Painted[x,y] in [1,2])) or ((Tags[x,y]=False) and (Painted[x,y] in [0,2])));
    if not ID then goto 1;
  end;
  ID := ID and (RobotX=LastRobotX) and (RobotY=LastRobotY);
1:
  Result := ID;
end;

//--------- Интерфейс и обработчики 
procedure LoadIni(var settings: IniSettings);
var Ini: TIniFile;
begin
  Ini := new TIniFile(IniFileName);
  settings.Width := Ini.ReadInteger('RobotWindow','Width',679);
  settings.Height := Ini.ReadInteger('RobotWindow','Height',490);
  settings.Left := Ini.ReadInteger('RobotWindow','Left',(Screen.PrimaryScreen.Bounds.Width-settings.Width) div 2);
  settings.Top := Ini.ReadInteger('RobotWindow','Top',(Screen.PrimaryScreen.Bounds.Height-settings.Height) div 2);
  
  if settings.Width>screen.PrimaryScreen.Bounds.Width then 
    settings.Width := Screen.PrimaryScreen.Bounds.Width;
    
  if settings.Height>Screen.PrimaryScreen.Bounds.Height then 
    settings.Height := Screen.PrimaryScreen.Bounds.Height;
    
  if settings.Left < 0 then 
    settings.Left := 0;
    
  if settings.Top < 0 then 
    settings.Top := 0;
    
  if (settings.Left > Screen.PrimaryScreen.Bounds.Width - 10) or 
     (settings.Top > Screen.PrimaryScreen.Bounds.Height - 10) then
  begin
    settings.Left := 0;
    settings.Top := 0;
  end;
  
  settings.Speed := Ini.ReadInteger('RobotWindow','Speed',3);
  Ini := nil;
end;

procedure SaveIni;
var Ini: TIniFile;
begin
  if (MainForm.Left<0) or (MainForm.Top<0) then
    exit;
  Ini := new TIniFile(IniFileName);
  Ini.WriteInteger('RobotWindow','Width',MainForm.Width);
  Ini.WriteInteger('RobotWindow','Height',MainForm.Height);
  Ini.WriteInteger('RobotWindow','Left',MainForm.Left);
  Ini.WriteInteger('RobotWindow','Top',MainForm.Top);
  Ini.WriteInteger('RobotWindow','Speed',trackBarSpeed.Value);
  Ini.Save;
end;

procedure buttonExitClick(o: Object; e: EventArgs);
begin
  SaveIni;
  halt;
end;   

procedure ABCWindowResize(o: Object; e: EventArgs);
begin
  CorrectWHLT;
end;

procedure MainWindowClose;
begin
  SaveIni;  
end;

procedure buttonStartClick(o: Object; e: EventArgs);
begin
  if t.ThreadState = System.Threading.ThreadState.Suspended then
  begin
    robField.StepFlag := False;
    labelExState.Text := 'Робот: Выполняю задание';
    robField.Start;
  end  
  else 
  begin
    robField.StepFlag := True;
    labelExState.Text := 'Робот: Остановился';
    robField.Stop;
  end;  
  (GraphABCControl as Control).Focus;
end;

procedure buttonStepClick(o: Object; e: EventArgs);
begin
  robField.StepFlag := True;
  t.Resume;
  (GraphABCControl as Control).Focus;
  labelExState.Text := 'Робот: Команда выполнена';
end;

procedure buttonHelpClick(o: Object; e: EventArgs);
begin
  MessageBox.Show('Разработчик  исполнителя  Робот:  Михалкович С.С., 2002-07  '#10#13#10#13+
    'Команды  исполнителя  Робот:'#10#13+
    '    Right - вправо'#10#13+
    '    Left - влево'#10#13
    '    Up - вверх'#10#13+
    '    Down - вниз'#10#13
    '    Paint - закрасить текущую клетку'#10#13
    '    Speed(n) - установить скорость n (n=0..10)'#10#13
    '    Stop - остановить Робота'#10#13
    '    Start - запустить Робота'#10#13#10#13
    'Условия, проверяемые  исполнителем  Робот:'#10#13
    '    WallFromRight - справа стена'#10#13
    '    WallFromLeft -  слева стена;'#10#13
    '    WallFromUp - сверху стена;'#10#13
    '    WallFromDown - снизу стена;'#10#13
    '    FreeFromRight - справа свободно;'#10#13
    '    FreeFromLeft - слева свободно;'#10#13
    '    FreeFromUp - сверху свободно;'#10#13
    '    FreeFromDown - снизу свободно;'#10#13
    '    CellIsPainted - клетка закрашена;'#10#13
    '    CellIsFree - клетка не закрашена.'#10#13,
    'Исполнитель Робот - Справка');

  (GraphABCControl as Control).Focus;
end;

procedure trackBarSpeedScroll(o: Object; e: EventArgs);
begin
  robField.SetSpeed(trackBarSpeed.Value);
end;

procedure MainWindowKeyDown(o: Object; e: KeyEventArgs);
begin
  if (e.KeyCode = Keys.Return) and buttonStart.Enabled then
    buttonStartClick(o,e)
  else if e.KeyCode = Keys.Escape then
    buttonExitClick(o,e)
  else if (e.KeyCode = Keys.Space) and buttonStep.Enabled then
    buttonStepClick(o,e)
  else if (e.KeyCode = Keys.Right) then
    robField.SetSpeed(trackBarSpeed.Value+1)
  else if (e.KeyCode = Keys.Left) then
    robField.SetSpeed(trackBarSpeed.Value-1)
  else if (e.KeyCode = Keys.F1) then
    buttonHelpClick(o,e)
end;

procedure CorrectWHLT;
var mw,mh: integer;
begin
  if (RobField.DimX=0) or (RobField.DimY=0) then
    exit;
  mw := (GraphABCControl.Width - 30) div RobField.DimX;
  mh := (GraphABCControl.Height - 20) div RobField.DimY;
  RobField.CellSize := min(mw,mh);
  if RobField.CellSize mod 2 = 1 then 
    RobField.CellSize := RobField.CellSize - 1;
  RobField.DrawCentered;  
end;

procedure SetTaskCall;
begin
  TaskIsCalled := True;
end;

procedure InitControls;
begin
  BottomPanel := new Panel();
  panelBottomLeft := new Panel();
  buttonHelp := new Button();
  buttonExit := new Button();
  buttonStep := new Button();
  buttonStart := new Button();
  tableLayoutPanelBottom := new TableLayoutPanel();
  labelExState := new TLabel();
  groupBoxExState := new GroupBox();
  tableLayoutPanel1 := new TableLayoutPanel();
  labelState := new TLabel();
  labelStep := new TLabel();
  trackBarSpeed := new TrackBar();
  labelSpeed := new TLabel();
  tableLayoutPanelTop := new TableLayoutPanel();
  labelZad := new TLabel();
//  CenterPanel := new Panel();
  
  BottomPanel.SuspendLayout();
  panelBottomLeft.SuspendLayout();
  tableLayoutPanelBottom.SuspendLayout();
  groupBoxExState.SuspendLayout();
  tableLayoutPanelTop.SuspendLayout();
  MainForm.SuspendLayout();
  // 
  // BottomPanel
  // 
  BottomPanel.Controls.Add(panelBottomLeft);
  BottomPanel.Controls.Add(groupBoxExState);
  BottomPanel.Dock := DockStyle.Bottom;
  BottomPanel.Location := new Point(0, 407);
  BottomPanel.Size := new Size(679, 83);
  // 
  // panelBottomLeft
  // 
  panelBottomLeft.Anchor := ((System.Windows.Forms.AnchorStyles)((integer(System.Windows.Forms.AnchorStyles.Bottom) or integer(System.Windows.Forms.AnchorStyles.Left))));
  panelBottomLeft.Controls.Add(buttonHelp);
  panelBottomLeft.Controls.Add(buttonExit);
  panelBottomLeft.Controls.Add(buttonStep);
  panelBottomLeft.Controls.Add(buttonStart);
  panelBottomLeft.Controls.Add(tableLayoutPanelBottom);
  panelBottomLeft.Location := new Point(0, 0);
  panelBottomLeft.Size := new Size(474, 83);
  // 
  // buttonHelp
  // 
  buttonHelp.FlatStyle := System.Windows.Forms.FlatStyle.System;
  buttonHelp.Location := new Point(359, 18);
  buttonHelp.Size := new Size(107, 24);
  buttonHelp.TabStop := false;
  buttonHelp.Text := 'Справка (F1)';
//  buttonHelp.UseVisualStyleBackColor := true;
  // 
  // buttonExit
  // 
  buttonExit.FlatStyle := System.Windows.Forms.FlatStyle.System;
  buttonExit.Location := new System.Drawing.Point(241, 18);
  buttonExit.Size := new System.Drawing.Size(107, 24);
  buttonExit.TabStop := false;
  buttonExit.Text := 'Выход (Esc)';
//  buttonExit.UseVisualStyleBackColor := true;
  // 
  // buttonStep
  // 
  buttonStep.FlatStyle := FlatStyle.System;
  buttonStep.Location := new Point(123, 18);
  buttonStep.Size := new Size(107, 24);
  buttonStep.TabStop := false;
  buttonStep.Text := 'Шаг (Space)';
//  buttonStep.UseVisualStyleBackColor := true;
  // 
  // buttonStart
  // 
  buttonStart.FlatStyle := FlatStyle.System;
  buttonStart.Location := new Point(5, 18);
  buttonStart.Size := new Size(107, 24);
  buttonStart.TabStop := false;
  buttonStart.Text := 'Пуск (Enter)';
//  buttonStart.UseVisualStyleBackColor := true;
  // 
  // tableLayoutPanelBottom
  // 
  tableLayoutPanelBottom.BackColor := LabelReadyColor;
  tableLayoutPanelBottom.CellBorderStyle := System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
  tableLayoutPanelBottom.ColumnCount := 1;
  tableLayoutPanelBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
  tableLayoutPanelBottom.Controls.Add(labelExState);
  tableLayoutPanelBottom.ForeColor := System.Drawing.Color.White;
  tableLayoutPanelBottom.Location := new Point(4, 52);
  tableLayoutPanelBottom.RowCount := 1;
  tableLayoutPanelBottom.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
  tableLayoutPanelBottom.Size := new Size(463, 23);
  // 
  // labelExState
  // 
  labelExState.AutoEllipsis := true;
  labelExState.AutoSize := true;
  labelExState.Dock := DockStyle.Fill;
  labelExState.FlatStyle := FlatStyle.System;
  labelExState.Font := new System.Drawing.Font('Microsoft Sans Serif', 8, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
  labelExState.Location := new Point(5, 2);
  labelExState.Margin := new Padding(3, 0, 3, 3);
  labelExState.Size := new Size(453, 15);
  labelExState.Text := 'Робот: Готов';
  labelExState.TextAlign := ContentAlignment.MiddleCenter;
  // 
  // groupBoxExState
  // 
  groupBoxExState.Controls.Add(tableLayoutPanel1);
  groupBoxExState.Controls.Add(labelState);
  groupBoxExState.Controls.Add(labelStep);
  groupBoxExState.Controls.Add(trackBarSpeed);
  groupBoxExState.Controls.Add(labelSpeed);
  groupBoxExState.Dock := DockStyle.Right;
  groupBoxExState.Location := new Point(475, 0);
  groupBoxExState.Size := new Size(204, 83);
  groupBoxExState.TabStop := false;
  // 
  // tableLayoutPanel1
  // 
  tableLayoutPanel1.BackColor := LabelDoColor;
  tableLayoutPanel1.CellBorderStyle := TableLayoutPanelCellBorderStyle.Outset;
  tableLayoutPanel1.ColumnCount := 1;
  tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(System.Windows.Forms.SizeType.Percent, 50));
  tableLayoutPanel1.Location := new Point(91, 52);
  tableLayoutPanel1.RowCount := 1;
  tableLayoutPanel1.RowStyles.Add(new RowStyle(System.Windows.Forms.SizeType.Percent, 50));
  tableLayoutPanel1.Size := new Size(22, 22);
  // 
  // labelState
  // 
  labelState.AutoSize := true;
  labelState.Location := new Point(6, 54);
  labelState.Margin := new Padding(0);
  labelState.Size := new Size(83, 17);
  labelState.Text := 'Состояние:';
  // 
  // labelStep
  // 
  labelStep.BackColor := SystemColors.Control;
  labelStep.Font := new System.Drawing.Font('Microsoft Sans Serif', 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
  labelStep.ForeColor := System.Drawing.Color.Black;
  labelStep.Location := new Point(130, 54);
  labelStep.Size := new Size(66, 18);
  labelStep.Text := 'Шаг: 0';
  labelStep.TextAlign := ContentAlignment.MiddleLeft;
  // 
  // trackBarSpeed
  // 
  trackBarSpeed.CausesValidation := false;
  trackBarSpeed.LargeChange := 1;
  trackBarSpeed.Location := new Point(80, 11);
  trackBarSpeed.Size := new Size(121, 53);
  trackBarSpeed.TabStop := false;
  // 
  // labelSpeed
  // 
  labelSpeed.AutoSize := true;
  labelSpeed.Location := new Point(6, 16);
  labelSpeed.Margin := new Padding(0);
  labelSpeed.Size := new Size(73, 17);
  labelSpeed.Text := 'Скорость:';
  // 
  // tableLayoutPanelTop
  // 
  tableLayoutPanelTop.BackColor := System.Drawing.Color.FromArgb(238,238,238);
  tableLayoutPanelTop.CellBorderStyle := TableLayoutPanelCellBorderStyle.Outset;
  tableLayoutPanelTop.ColumnCount := 1;
  tableLayoutPanelTop.ColumnStyles.Add(new ColumnStyle(System.Windows.Forms.SizeType.Percent, 50));
  tableLayoutPanelTop.Controls.Add(labelZad);
  tableLayoutPanelTop.Dock := DockStyle.Top;
  tableLayoutPanelTop.Location := new Point(0, 0);
  tableLayoutPanelTop.RowCount := 1;
  tableLayoutPanelTop.RowStyles.Add(new RowStyle(System.Windows.Forms.SizeType.Percent, 50));
  tableLayoutPanelTop.Size := new Size(679, 23);
  // 
  // labelZad
  // 
  labelZad.AutoEllipsis := true;
  labelZad.AutoSize := true;
  labelZad.Dock := DockStyle.Fill;
  labelZad.FlatStyle := FlatStyle.System;
  labelZad.Font := new System.Drawing.Font('Microsoft Sans Serif', 8, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
  labelZad.ForeColor := SystemColors.HotTrack;
  labelZad.Location := new Point(6, 3);
  labelZad.Margin := new Padding(4, 1, 3, 0);
  labelZad.Size := new Size(668, 14);
  labelZad.Text := 'Задание';
  labelZad.TextAlign := ContentAlignment.MiddleLeft;
  // 
  // ExecutorForm
  // 
  MainForm.BackColor := SystemColors.Control;
//  MainWindow.ClientSize := new Size(679, 490);
  MainForm.Controls.Add(tableLayoutPanelTop);
  MainForm.Controls.Add(BottomPanel);
  MainForm.MinimumSize := new Size(687, 240);
  MainForm.Text := 'Исполнитель Робот';
  BottomPanel.ResumeLayout(false);
  panelBottomLeft.ResumeLayout(false);
  tableLayoutPanelBottom.ResumeLayout(false);
  tableLayoutPanelBottom.PerformLayout();
  groupBoxExState.ResumeLayout(false);
  groupBoxExState.PerformLayout();
  tableLayoutPanelTop.ResumeLayout(false);
  tableLayoutPanelTop.PerformLayout();
  MainForm.ResumeLayout(false);
  MainForm.PerformLayout();
  MainForm.KeyPreview := True;
  
  MainForm.KeyDown += MainWindowKeyDown;
  
  (GraphABCControl as Control).Resize += ABCWindowResize;
  buttonStart.Click += buttonStartClick;
  buttonStep.Click += buttonStepClick;
  buttonHelp.Click += buttonHelpClick;
  buttonExit.Click += buttonExitClick;
  trackBarSpeed.Scroll += trackBarSpeedScroll;
end;

procedure NonSafeRobotError(s: string);
begin
  buttonStart.Enabled := False;  
  buttonStep.Enabled := False;  
  tableLayoutPanel1.BackColor := LabelErrorColor;
  labelExState.BackColor := LabelErrorColor;
  labelExState.Text := s;
  tableLayoutPanelBottom.BackColor := LabelErrorColor;
  t.Suspend;
end;

procedure RobotError(s: string);
begin
  buttonStart.Invoke(NonSafeRobotError,s);
end;

procedure RobotError(s,s1: string);
begin
  //labelZad.Text := s1;
  SetSafeLabelZadText(s1);
  RobotError(s);
end;

procedure SetWindowBounds(rec: System.Drawing.Rectangle);
begin
  MainForm.Bounds := rec;
end;

// Добавил - МА
procedure ResRB(TaskName: string); 
  external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'resrb';
//  

procedure FinalizeSafe;
begin
  buttonStart.Enabled := False;  
  buttonStep.Enabled := False;
  if (robField.DimX*robField.DimY=0) then
    RobotError('Робот: Не вызвана процедура Task','Задание отсутствует')
  else if robField.IsSolution then
  begin
    tableLayoutPanel1.BackColor := LabelGoodEndColor;
    labelExState.BackColor := LabelGoodEndColor;
    tableLayoutPanelBottom.BackColor := LabelGoodEndColor;
    labelExState.Text := 'Робот: Задание выполнено';
    // Добавил - МА
    try
      ResRB(robField.TaskName);
    except
    end;    
    //
  end
  else labelExState.Text := 'Робот: Работа окончена, задание не выполнено';
end;

var 
  settings: IniSettings;

var __initialized := false;
var __finalized := false;

procedure __InitModule;
begin
  RPaintColor := RGB(0,200,0);
  RobotColor := RGB(255,255,220);
  RobotColor1 := RGB(200,255,200);
  RBorderColor := RGB(1,1,1);
  BackColor := MainForm.BackColor;
  
  LabelReadyColor  := RGB(95,109,154);
  LabelDoColor     := RGB(95,109,154);
  LabelStopColor   := RGB(95,109,154);
  LabelErrorColor  := clRed;
  LabelBadEndColor := clBlack;
  LabelGoodEndColor:= RGB(0,156,0);
  
  TaskIsCalled := False;

  Brush.Color := MainForm.BackColor;
  FillRectangle(0,0,1280,1024);
  //MainWindow.Hide;
  //InitControls;
  MainForm.Invoke(InitControls);
  
  OnClose := MainWindowClose; 

//  CenterWindow;

  RobField := new TRobotField(0,0,50);
//  RobField.DrawCentered;
  t := System.Threading.Thread.CurrentThread;  
  LoadIni(settings);
  robField.SetSpeed(settings.Speed);
  //MainWindow.Bounds := new System.Drawing.Rectangle(settings.Left,settings.Top,settings.Width,settings.Height);
  MainForm.Invoke(SetWindowBounds, new System.Drawing.Rectangle(settings.Left,settings.Top,settings.Width,settings.Height));
  var del : procedure := MainForm.Show;
  MainForm.Invoke(del);
end;

procedure __InitModule__;
begin
  if not __initialized then
  begin
    __initialized := true;
    GraphABC.__InitModule__;
    __InitModule;
  end;
end;

procedure __FinalizeModule;
begin
  MainForm.Invoke(FinalizeSafe);
end;

procedure __FinalizeModule__;
begin
  if not __finalized then
  begin
    __finalized := true;
    __FinalizeModule;
  end;
end;

initialization
  __InitModule;
  //MainForm.Show;
finalization
  {buttonStart.Enabled := False;  
  buttonStep.Enabled := False;
  if (robField.DimX*robField.DimY=0) then
    RobotError('Робот: Не вызвана процедура Task','Задание отсутствует')
  else if robField.IsSolution then
  begin
    tableLayoutPanel1.BackColor := LabelGoodEndColor;
    labelExState.BackColor := LabelGoodEndColor;
    tableLayoutPanelBottom.BackColor := LabelGoodEndColor;
    labelExState.Text := 'Робот: Задание выполнено';
  end
  else labelExState.Text := 'Робот: Работа окончена, задание не выполнено';}
  __FinalizeModule;
end.