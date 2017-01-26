// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
unit DrawManField;

//#savepcu false

interface

uses GraphABC,DMCollect,System.Drawing;

const
  IniFileName = 'ABC.ini';
  colorTask = clRed;
  colorSolve = clBlue;

var
  BackColor: Color;
  LabelReadyColor: Color;
  LabelDoColor: Color;
  LabelStopColor: Color;
  LabelErrorColor: Color;
  LabelBadEndColor: Color;
  LabelGoodEndColor: Color;
  ZazX1 := 18;
  ZazY1 := 5;
  ZazX2 := 10;
  ZazY2 := 16;

type 
  TDMField = class
  private
    orx,ory: integer;
    DMColl, DMMakerColl: SortedCollection;
    DMX, DMY: integer;
    PenIsUp: boolean;
    MakerX, MakerY: integer;
    MakerPenIsUp: boolean;
    Interval: integer;
    StepFlag: boolean;
    movenum: integer;
    CellSize,DimX,DimY,X0,Y0: integer;
    DMPicture: Picture;
    procedure Draw0;
    procedure DrawFieldOnly;
    procedure DrawXY;
  public
    dmwidth := 2;
    // Добавил - МА
    TaskName: string;  
    //
    constructor Create(sizex,sizey,cellsize: integer);
    procedure DrawCentered;
    procedure Draw;
    procedure DrawDMDrawing;
    procedure DrawDMMakerDrawing;
    procedure DMLine(x1,y1,x2,y2: integer; c: GraphABC.Color);
    procedure Clear;
    procedure SetSpeed(speed: integer);
    property StepState: boolean read StepFlag;
    property SizeX: integer read DimX;
    property SizeY: integer read DimY;
    function IsSolution: boolean;
    procedure CheckTaskCall;
    // примитивы для постановщика (Maker)
    procedure SetOrigin(x0,y0: integer);
    procedure SetDim(DX,DY,CellSz: integer);
    procedure TaskText(s: string);
    
    procedure AddMakerLine(x1,y1,x2,y2: integer);
    procedure MakerPenUp;
    procedure MakerPenDown;
    procedure MakerToPoint(x,y: integer);
    procedure MakerOnVector(x,y: integer);
    
    procedure SetDrawManWidth(w: integer);

    // примитивы для выполнителя
    procedure DrawDM;
    procedure ClearDM;
    
    procedure PenUp;
    procedure PenDown;
    procedure ToPoint(x,y: integer);
    procedure OnVector(x,y: integer);
    
    procedure Pause;
    
    procedure Start;
    procedure Stop;
  end;

procedure CorrectWHLT;
procedure SetTaskCall;

procedure DrawmanError(s: string);
procedure DrawmanError(s,s1: string);

var 
  DMField: TDMField;

///--
procedure __InitModule__;
///--
procedure __FinalizeModule__;

implementation

uses 
  System,
  System.Windows.Forms,
  System.Drawing,
  GraphABCHelper,GraphABC,
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

procedure SetLabelExStateText(s: string);
begin
  labelExState.Text := s;
end;

procedure SetTrackBarSpeedValue(speed: integer);
begin
  trackBarSpeed.Value := speed;
end;

procedure SetSafeTrackBarSpeedValue(speed: integer);
begin
  trackBarSpeed.Invoke(SetTrackBarSpeedValue,speed);
end;

//------------ TDMField -------------
constructor TDMField.Create(sizex,sizey,cellsize: integer);
begin
  orx := 0;
  ory := 0;
  Self.CellSize := cellsize;
  DimX := sizex;
  DimY := sizey;
  X0 := 0;
  Y0 := 0;
  
  DMX := 0;
  DMY := 0;
  PenIsUp := True;

  MakerX := 0;
  MakerY := 0;
  MakerPenIsUp := True;
  
  DMColl := new SortedCollection;
  DMMakerColl := new SortedCollection;
  
  DMPicture := new Picture(9,9);
//  DMRedrawPicture := new Picture(9,9);

  StepFlag := False;
  movenum := 0;
end;

procedure TDMField.Draw;
begin
  if (MainForm.windowstate=formwindowstate.Minimized) then
    Exit;
  LockDrawing;
  ClearWindow(BackColor);
  Draw0;
  UnLockDrawing;
end;

procedure TDMField.DrawDMDrawing;
begin
  for var i:=0 to DMColl.Count-1 do
    DMLine(DMColl[i].p.x,DMColl[i].p.y,DMColl[i].p.x+DMColl[i].v.x,DMColl[i].p.y+DMColl[i].v.y,colorSolve);
end;

procedure TDMField.DrawDMMakerDrawing;
begin
  for var i:=0 to DMMakerColl.Count-1 do
    DMLine(DMMakerColl[i].p.x,DMMakerColl[i].p.y,DMMakerColl[i].p.x+DMMakerColl[i].v.x,DMMakerColl[i].p.y+DMMakerColl[i].v.y,colorTask);
end;

procedure TDMField.DrawCentered;
var w,h: integer;
begin
  if (DimX=0) or (DimY=0) then
    exit;
  w := CellSize*DimX+ZazX1+ZazX2; 
  h := CellSize*DimY+ZazY1+ZazY2;
  X0 := (GraphABCControl.Width - w) div 2;
  Y0 := (GraphABCControl.Height - h) div 2;
  Draw;
end;

procedure TDMField.DrawFieldOnly;
begin
  var w := CellSize*DimX; 
  var h := CellSize*DimY;
  Brush.Color := clWhite;
  FillRectangle(X0,Y0,X0+ZazX1+w+1+ZazX2,Y0+ZazY1+h+1+ZazY2);
  Pen.Width := 1;
  Pen.Color := RGB(191,191,191);
  for var ix:=0 to DimX do
    Line(X0+ZazX1+ix*CellSize,Y0+ZazY1,X0+ZazX1+ix*CellSize,Y0+ZazY1+h);
  for var iy:=0 to DimY do
    Line(X0+ZazX1,Y0+ZazY1+iy*CellSize,X0+ZazX1+w,Y0+ZazY1+iy*CellSize);
  Pen.Color := clGray;
  if (orx>-DimX) and (orx<0) then
    Line(X0+ZazX1-orx*CellSize,Y0+ZazY1,X0+ZazX1-orx*CellSize,Y0+ZazY1+h);
  if (ory>-DimY) and (ory<0) then
    Line(X0+ZazX1,Y0+ZazY1+(DimY+ory)*CellSize,X0+ZazX1+w,Y0+ZazY1+(DimY+ory)*CellSize);

  Pen.Color := clBlack;
  DrawRectangle(X0+ZazX1,Y0+ZazY1,X0+ZazX1+w+1,Y0+ZazY1+h+1);
end;

procedure TDMField.DrawXY;
var ww: integer;
    s: string;
    interval: integer;
    bs: BrushStyleType;
begin
  interval:=2;
  case CellSize of
  0..1: interval:=20;
  2..3: interval:=10;
  4..5: interval:=5;
  6..7: interval:=3;
  8..100: interval:=2;
  end;
  bs := Brush.Style;
  Brush.Style := bsClear;
  Font.Name := 'MS Sans Serif';
  Font.Size := 8;
  for var x:=0 to DimX do
    if (x+orx) mod interval = 0 then
    begin
      s:=IntToStr(x+orx);
      ww:=TextWidth(s);
//        hh:=Canvas.TextHeight(s);
      TextOut(X0+ZazX1+x*CellSize-ww div 2+1,Y0+ZazY1+DimY*CellSize+1+2,s);
    end;
  for var y:=0 to DimY do
    if (y+ory) mod interval = 0 then
    begin
      s:=IntToStr(y+ory);
      ww:=TextWidth(s);
      if (y+ory>=10) or (y+ory<0) then 
        Dec(ww);
//        hh:=Canvas.TextHeight(s);
      TextOut(X0+ZazX1-ww-3,Y0+ZazY1+(DimY-y)*CellSize+1-8,s);
    end;
  Brush.Style := bsSolid;
end;

procedure TDMField.DMLine(x1,y1,x2,y2: integer; c: GraphABC.Color);
begin
  Pen.Width := dmwidth;
  Line(X0+ZazX1+(x1-orx)*CellSize,Y0+ZazY1+(DimY-y1+ory)*CellSize,X0+ZazX1+(x2-orx)*CellSize,Y0+ZazY1+(DimY-y2+ory)*CellSize,c);
end;

procedure TDMField.Draw0;
begin
  DrawFieldOnly;
  DrawXY;
  DrawDMMakerDrawing;
  DrawDMDrawing;
  DrawDM;
end;

procedure TDMField.SetDim(DX, DY, CellSz: integer);
begin
  Clear;
  DimX := DX;
  DimY := DY;
  CellSize := CellSz;
//  DrawCentered;
end;

procedure TDMField.SetOrigin(x0,y0: integer);
begin
  orx := x0;
  ory := y0;
  if orx<=-10 then 
    ZazX1 := 23
  else ZazX1 := 18;  
end;

procedure TDMField.AddMakerLine(x1,y1,x2,y2: integer);
begin
  DMMakerColl.Insert(x1,y1,x2,y2);
end;

procedure TDMField.MakerPenUp;
begin
  MakerPenIsUp := True;
end;

procedure TDMField.MakerPenDown;
begin
  MakerPenIsUp := False;
end;

procedure TDMField.MakerToPoint(x,y: integer);
begin
  if not MakerPenIsUp then
    AddMakerLine(MakerX,MakerY,x,y);
  MakerX := x;
  MakerY := y;
end;

procedure TDMField.MakerOnVector(x,y: integer);
begin
  MakerToPoint(MakerX+x,MakerY+y);
end;

procedure TDMField.SetDrawManWidth(w: integer);
begin
  dmwidth := 2;
end;

procedure TDMField.DrawDM;
var 
  ZZ: integer;
  r,r1: System.Drawing.Rectangle;
begin
  if PenIsUp then
    ZZ := 4
  else ZZ := 2;  
  r := new System.Drawing.Rectangle(0,0,8,8);
  r1 := r;
  r1.Offset(X0+ZazX1+(DMX-orx)*CellSize-4+1,Y0+ZazY1+(DimY-DMY+ory)*CellSize-4+1);
  DMPicture.CopyRect(r,GraphBufferBitmap,r1);
  Pen.Width := 1;
  DrawRectangle(X0+ZazX1+(DMX-orx)*CellSize-ZZ+1,Y0+ZazY1+(DimY+ory-DMY)*CellSize-ZZ+1,X0+ZazX1+(DMX-orx)*CellSize+ZZ,Y0+ZazY1+(DimY+ory-DMY)*CellSize+ZZ);
end;

procedure TDMField.ClearDM;
begin
  DMPicture.Draw(X0+ZazX1+(DMX-orx)*CellSize-4+1,Y0+ZazY1+(DimY-DMY+ory)*CellSize-4+1)
end;

procedure TDMField.PenUp;
begin
  CheckTaskCall;
  ClearDM;
  PenIsUp := True;
  DrawDM;
  Inc(movenum);
  SetSafeLabelStepText('Шаг: ' + IntToStr(movenum));
end;

procedure TDMField.PenDown;
begin
  CheckTaskCall;
  ClearDM;
  PenIsUp := False;
  DrawDM;
  Inc(movenum);
  SetSafeLabelStepText('Шаг: ' + IntToStr(movenum));
end;

procedure TDMField.ToPoint(x,y: integer);
begin
  CheckTaskCall;
  ClearDM;
  if (x<orx) or (x>DimX+orx) or (y<ory) or (y>DimY+ory) then
    DrawmanError('Чертежник: вышел за границы поля рисования','Задание прервано');
  if not PenIsUp then
  begin
    DMLine(DMX,DMY,x,y,colorSolve);
    DMColl.Insert(DMX,DMY,x,y);
  end;  
  DMX := x;
  DMY := y;
  DrawDM;
  Inc(movenum);
  SetSafeLabelStepText('Шаг: ' + IntToStr(movenum));
end;

procedure TDMField.OnVector(x,y: integer);
begin
  ToPoint(DMX+x,DMY+y);
end;

procedure TDMField.TaskText(s: string);
begin
  SetSafeLabelZadText(s);
end;

procedure TDMField.SetSpeed(speed: integer);
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

procedure TDMField.Stop;
begin
  buttonStart.Text := 'Пуск (Enter)';
  buttonStep.Enabled := True;
  t.Suspend;
end;
 
procedure TDMField.Start;
begin
  buttonStart.Text := 'Стоп (Enter)';
  buttonStep.Enabled := False;
  t.Resume;
end;

procedure TDMField.Pause;
begin
  Sleep(Interval);
end;

procedure TDMField.Clear;
begin
  DMColl.Clear;
  DMMakerColl.Clear;
  DMX := 0;
  DMY := 0;
  PenIsUp := True;
  MakerX := 0;
  MakerY := 0;
  MakerPenIsUp := True;
end;

function TDMField.IsSolution: boolean;
begin
  DMColl.Normalize;
  DMMakerColl.Normalize;
  
{  for i:=0 to DMColl.Count-1 do
    writeln(DMColl[i]);
  writeln;
  for i:=0 to DMMakerColl.Count-1 do
    writeln(DMMakerColl[i]);
}  
  Result := IsEqualSC(DMColl,DMMakerColl) and (DMX=0) and (DMY=0) and (PenIsUp);
end;

procedure TDMField.CheckTaskCall;
begin
  if not TaskIsCalled then
    DrawManError('Процедура Task вызова задания должна быть первой','Выполнение программы прервано');
end;

//--------- Интерфейс и обработчики 
procedure LoadIni(var settings: IniSettings);
var Ini: TIniFile;
begin
  Ini := new TIniFile(IniFileName);
  settings.Width := Ini.ReadInteger('DrawmanWindow','Width',679);
  settings.Height := Ini.ReadInteger('DrawmanWindow','Height',490);
  settings.Left := Ini.ReadInteger('DrawmanWindow','Left',(Screen.PrimaryScreen.Bounds.Width-settings.Width) div 2);
  settings.Top := Ini.ReadInteger('DrawmanWindow','Top',(Screen.PrimaryScreen.Bounds.Height-settings.Height) div 2);

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
  
  settings.Speed := Ini.ReadInteger('DrawmanWindow','Speed',3);
  Ini := nil;
end;

procedure SaveIni;
var Ini: TIniFile;
begin
  if (MainForm.Left<0) or (MainForm.Top<0) then
    exit;
  Ini := new TIniFile(IniFileName);
  Ini.WriteInteger('DrawmanWindow','Width',MainForm.Width);
  Ini.WriteInteger('DrawmanWindow','Height',MainForm.Height);
  Ini.WriteInteger('DrawmanWindow','Left',MainForm.Left);
  Ini.WriteInteger('DrawmanWindow','Top',MainForm.Top);
  Ini.WriteInteger('DrawmanWindow','Speed',trackBarSpeed.Value);
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
    DMField.StepFlag := False;
    labelExState.Text := 'Чертежник: Выполняю задание';
    DMField.Start;
  end  
  else 
  begin
    DMField.StepFlag := True;
    labelExState.Text := 'Чертежник: Остановился';
    DMField.Stop;
  end;  
  (GraphABCControl as Control).Focus;
end;

procedure buttonStepClick(o: Object; e: EventArgs);
begin
  DMField.StepFlag := True;
  t.Resume;
  (GraphABCControl as Control).Focus;
  labelExState.Text := 'Чертежник: Команда выполнена';
end;

procedure buttonHelpClick(o: Object; e: EventArgs);
begin
  MessageBox.Show('Разработчик  исполнителя  Чертежник:  Михалкович С.С., 2002-07  '#10#13#10#13+
    'Команды  исполнителя  Чертежник:'#10#13+
    '    PenDown - опустить перо'#10#13+
    '    PenUp - поднять перо'#10#13
    '    ToPoint(x,y) - переместиться в точку с координатами (x,y)'#10#13+
    '    OnVector(a,b) - переместиться из текущей точки на вектор (a,b)'#10#13
    '       a>0 - вправо, a<0 - влево, b>0 - вверх, b<0 - вниз'#10#13
    '    Speed(n) - установить скорость n (n=0..10)'#10#13
    '    Stop - остановить Чертежника'#10#13
    '    Start - запустить Чертежника'#10#13,
    'Исполнитель Чертежник - Справка');

  (GraphABCControl as Control).Focus;
end;

procedure trackBarSpeedScroll(o: Object; e: EventArgs);
begin
  DMField.SetSpeed(trackBarSpeed.Value);
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
    DMField.SetSpeed(trackBarSpeed.Value+1)
  else if (e.KeyCode = Keys.Left) then
    DMField.SetSpeed(trackBarSpeed.Value-1)
  else if (e.KeyCode = Keys.F1) then
    buttonHelpClick(o,e)
end;

procedure CorrectWHLT;
var mw,mh: integer;
begin
  if (DMField.DimX=0) or (DMField.DimY=0) then
    exit;
  mw := (GraphABCControl.Width - Zazx1 - ZazX2 - 30) div DMField.DimX;
  mh := (GraphABCControl.Height - ZazY1 - ZazY2 - 20) div DMField.DimY;
  DMField.CellSize := min(mw,mh);
{  if DMField.CellSize mod 2 = 1 then 
    DMField.CellSize := DMField.CellSize - 1;}
  DMField.DrawCentered;  
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
  labelExState.Text := 'Чертежник: Готов';
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
//  MainForm.ClientSize := new Size(679, 490);
  MainForm.Controls.Add(tableLayoutPanelTop);
  MainForm.Controls.Add(BottomPanel);
  MainForm.MinimumSize := new Size(687, 240);
  MainForm.Text := 'Исполнитель Чертежник';
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

procedure NonSafeDrawmanError(s: string);
begin
  buttonStart.Enabled := False;  
  buttonStep.Enabled := False;  
  tableLayoutPanel1.BackColor := LabelErrorColor;
  labelExState.BackColor := LabelErrorColor;
  labelExState.Text := s;
  tableLayoutPanelBottom.BackColor := LabelErrorColor;
  t.Suspend;
end;

procedure DrawmanError(s: string);
begin
  buttonStart.Invoke(NonSafeDrawmanError,s);
end;

procedure DrawmanError(s,s1: string);
begin
  //labelZad.Text := s1;
  SetSafeLabelZadText(s1);
  DrawmanError(s);
end;

var 
  settings: IniSettings;
  
procedure _InternalAssignBackColor;
begin
  BackColor := MainForm.BackColor;
end;

procedure _HideInternal;
begin
  MainForm.Hide;
end;

procedure _SetBoundsInternal;
begin
  MainForm.Bounds := new System.Drawing.Rectangle(settings.Left,settings.Top,settings.Width,settings.Height);
end;

procedure _ShowInternal;
begin
  MainForm.Show;
end;

procedure _SetButtonStartEnabled(value : boolean);
begin
 buttonStart.Enabled := value; 
end;

procedure _SetButtonStepEnabled(value : boolean);
begin
 buttonStep.Enabled := value; 
end;
 
procedure SetSafeTextLabelExState(s : string);
begin
 labelExState.Text := s;
end;

// Добавил - МА
procedure ResDM(TaskName: string); 
  external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'resdm';
//  

var __initialized := false;

procedure __InitModule;
begin
  MainForm.Invoke(_HideInternal);
  //MainForm.Hide;
  MainForm.Invoke(_InternalAssignBackColor);
  //BackColor := MainForm.BackColor;
  
  LabelReadyColor  := RGB(95,109,154);
  LabelDoColor     := RGB(95,109,154);
  LabelStopColor   := RGB(95,109,154);
  LabelErrorColor  := clRed;
  LabelBadEndColor := clBlack;
  LabelGoodEndColor:= RGB(0,156,0);
  
  TaskIsCalled := False;

  Brush.Color := MainForm.BackColor;
  FillRectangle(0,0,1280,1024);
  MainForm.Invoke(InitControls);
  
  OnClose := MainWindowClose; 

//  CenterWindow;

  DMField := new TDMField(0,0,50);
//  DMField.DrawCentered;
  t := System.Threading.Thread.CurrentThread;  
  LoadIni(settings);
  DMField.SetSpeed(settings.Speed);
  MainForm.Invoke(_SetBoundsInternal);
  //MainForm.Bounds := new System.Drawing.Rectangle(settings.Left,settings.Top,settings.Width,settings.Height);
  SetSmoothingOff;
  //GraphABCControl.BackColor := MainForm.BackColor;
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
  buttonStart.Invoke(_SetButtonStartEnabled, false);
  buttonStep.Invoke(_SetButtonStepEnabled, false);
  //buttonStart.Enabled := False;
  //buttonStep.Enabled := False;
  if DMField.DimX*DMField.DimY=0 then
    DrawmanError('Чертежник: Не вызвана процедура Task','Задание отсутствует')
  else if DMField.IsSolution then
  begin
    tableLayoutPanel1.BackColor := LabelGoodEndColor;
    labelExState.BackColor := LabelGoodEndColor;
    tableLayoutPanelBottom.BackColor := LabelGoodEndColor;
    labelExState.Invoke(SetSafeTextLabelExState,'Чертежник: Задание выполнено');
    //labelExState.Text := 'Чертежник: Задание выполнено';
    // Добавил - МА
    try
      ResDM(DMField.TaskName);
    except
    end;    
    //
  end
  else
  begin
    labelExState.Invoke(SetSafeTextLabelExState,'Чертежник: Работа окончена, задание не выполнено');
    //labelExState.Text := 'Чертежник: Работа окончена, задание не выполнено';
  end;
end;

var __finalized := false;

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
finalization
  __FinalizeModule;
end.