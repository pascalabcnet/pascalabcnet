// Ханойские башни
uses GraphABC;

type
  /// Тип диска
  DiskType = record
    /// Диаметр диска
    Sz: integer;
    /// Цвет диска
    Color: GraphABC.Color;
  end;
  /// Тип массива дисков на стержне
  DiskArr = array of DiskType;

const
  /// Количество дисков
  CountDisks = 8;
  /// Высота диска
  DiskHeight = 12;
  /// Приращение ширины диска
  DiskWidthDelta = 12;
  h = CountDisks * DiskWidthDelta * 2 + 20; 
  /// y-координата основания пирамид дисков
  y0 = DiskHeight * CountDisks + 80;
  hh = 30;
  /// x-координата первого стержня
  x1 = h div 2 + hh;
  /// x-координата второго стержня
  x2 = x1 + h;
  /// x-координата третьего стержня
  x3 = x2 + h;
  /// Пауза, мс
  delay = 50;

var
  /// Массив пирамид дисков 
  Tower: array [1..3] of DiskArr;
  /// Массив количеств дисков в пирамидах
  DisksInTower: array [1..3] of integer;
  /// Номер хода
  MoveNumber: integer;

/// Рисование пирамиды
procedure DrawTower(a: DiskArr; n: integer; x0,y0: integer);
begin
  Brush.Color := clBlack;
  Rectangle(x0-5,y0,x0+5,y0-DiskHeight*CountDisks-10);
  for var i:=0 to n-1 do
  begin
    Brush.Color := a[i].Color;
    Rectangle(x0-a[i].sz*DiskWidthDelta,y0-DiskHeight*(i-1),x0+a[i].sz*DiskWidthDelta,y0-DiskHeight*i+1)
  end;
end;

/// Рисование всех пирамид и информационной строки
procedure DrawAll;
begin
  DrawTower(Tower[1],DisksInTower[1],x1,y0);
  DrawTower(Tower[2],DisksInTower[2],x2,y0);
  DrawTower(Tower[3],DisksInTower[3],x3,y0);
  Brush.Color := clWhite;
  TextOut(20,20,'Число перемещений дисков = '+IntToStr(MoveNumber));
  Redraw;
end;

/// Перемещение диска со стержня FromN на стержень ToN
procedure MoveDisk(FromN, ToN: integer);
begin
  Inc(MoveNumber);
  Inc(DisksInTower[ToN]);
  Tower[ToN][DisksInTower[ToN]-1] := Tower[FromN][DisksInTower[FromN]-1];
  Dec(DisksInTower[FromN]);
  Sleep(delay);
  ClearWindow;
  DrawAll;
end;

/// Основная екурсивная процедура алгоритма "Ханойские башни"
procedure MoveTower(n: integer; FromN, ToN, WorkN: integer);
begin
  if n=0 then exit;
  MoveTower(n-1, FromN, WorkN, ToN);
  MoveDisk(FromN, ToN);
  MoveTower(n-1, WorkN, ToN, FromN);
end;

/// Инициализация массивов
procedure InitTowers;
begin
  SetLength(Tower[1],CountDisks);
  SetLength(Tower[2],CountDisks);
  SetLength(Tower[3],CountDisks);
  DisksInTower[1] := CountDisks;
  DisksInTower[2] := 0;
  DisksInTower[3] := 0;
  for var i:=0 to DisksInTower[1]-1 do
  begin
    Tower[1][i].Sz := DisksInTower[1]-i+1;
    Tower[1][i].Color := clRandom;
  end;
end;

/// Инициализация окна
procedure InitWindow;
begin
  SetWindowSize(x3+x1,y0+50);
  CenterWindow;
  Window.Title := 'Ханойские башни';
  Font.Size := 14;
  Font.Name := 'Arial';
end;

begin
  InitWindow;
  InitTowers;
  LockDrawing;
  DrawAll;
  MoveTower(CountDisks,1,3,2);
end.


