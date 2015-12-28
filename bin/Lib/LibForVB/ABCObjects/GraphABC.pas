///Модуль предоставляет константы, типы, процедуры, функции и классы для рисования в графическом окне
unit GraphABC;

//ne udaljat, IB 7.10.08 
#apptype windows 
#reference 'System.Windows.Forms.dll'
#reference 'System.Drawing.dll'
//#gendoc true
#savepcu false

interface

uses 
  System,
  System.Drawing,
  System.Windows.Forms, 
  System.Drawing.Drawing2D;

type 
/// Тип цвета
  Color = System.Drawing.Color;
/// Тип стиля штриховки кисти
  HatchStyle = System.Drawing.Drawing2D.HatchStyle;
/// Тип стиля штриховки пера
  DashStyle = System.Drawing.Drawing2D.DashStyle;
/// Тип исключения GraphABC  
  GraphABCException = class(Exception) end;
/// Тип точки
  Point = System.Drawing.Point;

var 
  clMoneyGreen: Color;

const 
  // Default graph window size
  defaultWindowWidth = 640;
  defaultWindowHeight = 480;
  
  // Color constants
  clAquamarine = Color.Aquamarine;             clAzure = Color.Azure;                     
  clBeige = Color.Beige;                       clBisque = Color.Bisque;                   
  clBlack = Color.Black;                       clBlanchedAlmond = Color.BlanchedAlmond;   
  clBlue = Color.Blue;                         clBlueViolet = Color.BlueViolet;           
  clBrown = Color.Brown;                       clBurlyWood = Color.BurlyWood;             
  clCadetBlue = Color.CadetBlue;               clChartreuse = Color.Chartreuse;           
  clChocolate = Color.Chocolate;               clCoral = Color.Coral;                     
  clCornflowerBlue = Color.CornflowerBlue;     clCornsilk = Color.Cornsilk;               
  clCrimson = Color.Crimson;                   clCyan = Color.Cyan;                       
  clDarkBlue = Color.DarkBlue;                 clDarkCyan = Color.DarkCyan;               
  clDarkGoldenrod = Color.DarkGoldenrod;       clDarkGray = Color.DarkGray;               
  clDarkGreen = Color.DarkGreen;               clDarkKhaki = Color.DarkKhaki;             
  clDarkMagenta = Color.DarkMagenta;           clDarkOliveGreen = Color.DarkOliveGreen;   
  clDarkOrange = Color.DarkOrange;             clDarkOrchid = Color.DarkOrchid;           
  clDarkRed = Color.DarkRed;                   clDarkTurquoise = Color.DarkTurquoise;     
  clDarkSeaGreen = Color.DarkSeaGreen;         clDarkSlateBlue = Color.DarkSlateBlue;     
  clDarkSlateGray = Color.DarkSlateGray;       clDarkViolet = Color.DarkViolet;           
  clDeepPink = Color.DeepPink;                 clDarkSalmon = Color.DarkSalmon;           
  clDeepSkyBlue = Color.DeepSkyBlue;           clDimGray = Color.DimGray;                 
  clDodgerBlue = Color.DodgerBlue;             clFirebrick = Color.Firebrick;             
  clFloralWhite = Color.FloralWhite;           clForestGreen = Color.ForestGreen;         
  clFuchsia = Color.Fuchsia;                   clGainsboro = Color.Gainsboro;             
  clGhostWhite = Color.GhostWhite;             clGold = Color.Gold;                       
  clGoldenrod = Color.Goldenrod;               clGray = Color.Gray;                       
  clGreen = Color.Green;                       clGreenYellow = Color.GreenYellow;         
  clHoneydew = Color.Honeydew;                 clHotPink = Color.HotPink;                 
  clIndianRed = Color.IndianRed;               clIndigo = Color.Indigo;                   
  clIvory = Color.Ivory;                       clKhaki = Color.Khaki;                     
  clLavender = Color.Lavender;                 clLavenderBlush = Color.LavenderBlush;     
  clLawnGreen = Color.LawnGreen;               clLemonChiffon = Color.LemonChiffon;       
  clLightBlue = Color.LightBlue;               clLightCoral = Color.LightCoral;           
  clLightCyan = Color.LightCyan;               clLightGray = Color.LightGray;             
  clLightGreen = Color.LightGreen;             clLightGoldenrodYellow = Color.LightGoldenrodYellow;
  clLightPink = Color.LightPink;               clLightSalmon = Color.LightSalmon;         
  clLightSeaGreen = Color.LightSeaGreen;       clLightSkyBlue = Color.LightSkyBlue;       
  clLightSlateGray = Color.LightSlateGray;     clLightSteelBlue = Color.LightSteelBlue;   
  clLightYellow = Color.LightYellow;           clLime = Color.Lime;                       
  clLimeGreen = Color.LimeGreen;               clLinen = Color.Linen;                     
  clMagenta = Color.Magenta;                   clMaroon = Color.Maroon;                   
  clMediumBlue = Color.MediumBlue;             clMediumOrchid = Color.MediumOrchid;       
  clMediumAquamarine = Color.MediumAquamarine; clMediumPurple = Color.MediumPurple;       
  clMediumSeaGreen = Color.MediumSeaGreen;     clMediumSlateBlue = Color.MediumSlateBlue; 
  clPlum = Color.Plum;                         clMistyRose = Color.MistyRose;             
  clNavy = Color.Navy;                         clMidnightBlue = Color.MidnightBlue;       
  clMintCream = Color.MintCream;               clMediumSpringGreen = Color.MediumSpringGreen;
  clMoccasin = Color.Moccasin;                 clNavajoWhite = Color.NavajoWhite;         
  clMediumTurquoise = Color.MediumTurquoise;   clOldLace = Color.OldLace;                 
  clOlive = Color.Olive;                       clOliveDrab = Color.OliveDrab;             
  clOrange = Color.Orange;                     clOrangeRed = Color.OrangeRed;             
  clOrchid = Color.Orchid;                     clPaleGoldenrod = Color.PaleGoldenrod;     
  clPaleGreen = Color.PaleGreen;               clPaleTurquoise = Color.PaleTurquoise;     
  clPaleVioletRed = Color.PaleVioletRed;       clPapayaWhip = Color.PapayaWhip;           
  clPeachPuff = Color.PeachPuff;               clPeru = Color.Peru;                       
  clPink = Color.Pink;                         clMediumVioletRed = Color.MediumVioletRed; 
  clPowderBlue = Color.PowderBlue;             clPurple = Color.Purple;                   
  clRed = Color.Red;                           clRosyBrown = Color.RosyBrown;             
  clRoyalBlue = Color.RoyalBlue;               clSaddleBrown = Color.SaddleBrown;         
  clSalmon = Color.Salmon;                     clSandyBrown = Color.SandyBrown;           
  clSeaGreen = Color.SeaGreen;                 clSeaShell = Color.SeaShell;               
  clSienna = Color.Sienna;                     clSilver = Color.Silver;                   
  clSkyBlue = Color.SkyBlue;                   clSlateBlue = Color.SlateBlue;             
  clSlateGray = Color.SlateGray;               clSnow = Color.Snow;                       
  clSpringGreen = Color.SpringGreen;           clSteelBlue = Color.SteelBlue;             
  clTan = Color.Tan;                           clTeal = Color.Teal;                       
  clThistle = Color.Thistle;                   clTomato = Color.Tomato;                   
  clTransparent = Color.Transparent;           clTurquoise = Color.Turquoise;             
  clViolet = Color.Violet;                     clWheat = Color.Wheat;                     
  clWhite = Color.White;                       clWhiteSmoke = Color.WhiteSmoke;           
  clYellow = Color.Yellow;                     clYellowGreen = Color.YellowGreen;
  
// Virtual Key Codes
  VK_Back = 8;              VK_Tab = 9;
  VK_LineFeed = 10;         VK_Enter = 13;
  VK_Return = 13;           VK_ShiftKey = 16;           VK_ControlKey = 17;
  VK_Menu = 18;             VK_Pause = 19;              VK_CapsLock = 20;
  VK_Capital = 20;
  VK_Escape = 27;
  VK_Space = 32;
  VK_Prior = 33;            VK_PageUp = 33;             VK_PageDown = 34;
  VK_Next = 34;             VK_End = 35;                VK_Home = 36;
  VK_Left = 37;             VK_Up = 38;                 VK_Right = 39;
  VK_Down = 40;             VK_Select = 41;             VK_Print = 42;
  VK_Snapshot = 44;         VK_PrintScreen = 44;
  VK_Insert = 45;           VK_Delete = 46;             VK_Help = 47;
  VK_A = 65;                VK_B = 66;
  VK_C = 67;                VK_D = 68;                  VK_E = 69;
  VK_F = 70;                VK_G = 71;                  VK_H = 72;
  VK_I = 73;                VK_J = 74;                  VK_K = 75;
  VK_L = 76;                VK_M = 77;                  VK_N = 78;
  VK_O = 79;                VK_P = 80;                  VK_Q = 81;
  VK_R = 82;                VK_S = 83;                  VK_T = 84;
  VK_U = 85;                VK_V = 86;                  VK_W = 87;
  VK_X = 88;                VK_Y = 89;                  VK_Z = 90;
  VK_LWin = 91;             VK_RWin = 92;               VK_Apps = 93;
  VK_Sleep = 95;            VK_NumPad0 = 96;            VK_NumPad1 = 97;
  VK_NumPad2 = 98;          VK_NumPad3 = 99;            VK_NumPad4 = 100;
  VK_NumPad5 = 101;         VK_NumPad6 = 102;           VK_NumPad7 = 103;
  VK_NumPad8 = 104;         VK_NumPad9 = 105;           VK_Multiply = 106;
  VK_Add = 107;             VK_Separator = 108;         VK_Subtract = 109;
  VK_Decimal = 110;         VK_Divide = 111;            VK_F1 = 112;
  VK_F2 = 113;              VK_F3 = 114;                VK_F4 = 115;
  VK_F5 = 116;              VK_F6 = 117;                VK_F7 = 118;
  VK_F8 = 119;              VK_F9 = 120;                VK_F10 = 121;
  VK_F11 = 122;             VK_F12 = 123;               VK_NumLock = 144;
  VK_Scroll = 145;          VK_LShiftKey = 160;         VK_RShiftKey = 161;
  VK_LControlKey = 162;     VK_RControlKey = 163;       VK_LMenu = 164;
  VK_RMenu = 165;           
  VK_KeyCode = 65535;       VK_Shift = 65536;           VK_Control = 131072;
  VK_Alt = 262144;          VK_Modifiers = -65536;
  
// Pen style constants
  psSolid = DashStyle.Solid;
  psClear = DashStyle.Custom;    
  psDash = DashStyle.Dash; 
  psDot = DashStyle.Dot;   
  psDashDot = DashStyle.DashDot;   
  psDashDotDot = DashStyle.DashDotDot; 

// Pen mode constants
  pmCopy = 0;
  pmNot = 1;
  
// Brush hatch type constants
  bhHorizontal = HatchStyle.Horizontal;
  bhMin = HatchStyle.Min;
  bhVertical = HatchStyle.Vertical;
  bhForwardDiagonal = HatchStyle.ForwardDiagonal;
  bhBackwardDiagonal = HatchStyle.BackwardDiagonal;
  bhCross = HatchStyle.Cross;
  bhLargeGrid = HatchStyle.LargeGrid;
  bhMax = HatchStyle.Max;
  bhDiagonalCross = HatchStyle.DiagonalCross;
  bhPercent05 = HatchStyle.Percent05;
  bhPercent10 = HatchStyle.Percent10;
  bhPercent20 = HatchStyle.Percent20;
  bhPercent25 = HatchStyle.Percent25;
  bhPercent30 = HatchStyle.Percent30;
  bhPercent40 = HatchStyle.Percent40;
  bhPercent50 = HatchStyle.Percent50;
  bhPercent60 = HatchStyle.Percent60;
  bhPercent70 = HatchStyle.Percent70;
  bhPercent75 = HatchStyle.Percent75;
  bhPercent80 = HatchStyle.Percent80;
  bhPercent90 = HatchStyle.Percent90;
  bhLightDownwardDiagonal = HatchStyle.LightDownwardDiagonal;
  bhLightUpwardDiagonal = HatchStyle.LightUpwardDiagonal;
  bhDarkDownwardDiagonal = HatchStyle.DarkDownwardDiagonal;
  bhDarkUpwardDiagonal = HatchStyle.DarkUpwardDiagonal;
  bhWideDownwardDiagonal = HatchStyle.WideDownwardDiagonal;
  bhWideUpwardDiagonal = HatchStyle.WideUpwardDiagonal;
  bhLightVertical = HatchStyle.LightVertical;
  bhLightHorizontal = HatchStyle.LightHorizontal;
  bhNarrowVertical = HatchStyle.NarrowVertical;
  bhNarrowHorizontal = HatchStyle.NarrowHorizontal;
  bhDarkVertical = HatchStyle.DarkVertical;
  bhDarkHorizontal = HatchStyle.DarkHorizontal;
  bhDashedDownwardDiagonal = HatchStyle.DashedDownwardDiagonal;
  bhDashedUpwardDiagonal = HatchStyle.DashedUpwardDiagonal;
  bhDashedHorizontal = HatchStyle.DashedHorizontal;
  bhDashedVertical = HatchStyle.DashedVertical;
  bhSmallConfetti = HatchStyle.SmallConfetti;
  bhLargeConfetti = HatchStyle.LargeConfetti;
  bhZigZag = HatchStyle.ZigZag;
  bhWave = HatchStyle.Wave;
  bhDiagonalBrick = HatchStyle.DiagonalBrick;
  bhHorizontalBrick = HatchStyle.HorizontalBrick;
  bhWeave = HatchStyle.Weave;
  bhPlaid = HatchStyle.Plaid;
  bhDivot = HatchStyle.Divot;
  bhDottedGrid = HatchStyle.DottedGrid;
  bhDottedDiamond = HatchStyle.DottedDiamond;
  bhShingle = HatchStyle.Shingle;
  bhTrellis = HatchStyle.Trellis;
  bhSphere = HatchStyle.Sphere;
  bhSmallGrid = HatchStyle.SmallGrid;
  bhSmallCheckerBoard = HatchStyle.SmallCheckerBoard;
  bhLargeCheckerBoard = HatchStyle.LargeCheckerBoard;
  bhOutlinedDiamond = HatchStyle.OutlinedDiamond;
  bhSolidDiamond = HatchStyle.SolidDiamond;

// Font & Brush style constants 
type 
  FontStyleType = (fsNormal, fsBold, fsItalic, fsBoldItalic, fsUnderline, fsBoldUnderline, fsItalicUnderline, fsBoldItalicUnderline);
  BrushStyleType = (bsSolid, bsClear, bsHatch, bsGradient, bsNone);

/// Закрашивает пиксел с координатами (x,y) цветом c
procedure SetPixel(x,y: integer; c: Color);
/// Закрашивает пиксел с координатами (x,y) цветом c
procedure PutPixel(x,y: integer; c: Color);
/// Возвращает цвет пиксела с координатами (x,y)
function GetPixel(x,y: integer): Color;

/// Устанавливает текущую позицию рисования в точку (x,y)
procedure MoveTo(x,y: integer);
/// Рисует отрезок от текущей позиции до точки (x,y). Текущая позиция переносится в точку (x,y)
procedure LineTo(x,y: integer);
/// Рисует отрезок от текущей позиции до точки (x,y) цветом c. Текущая позиция переносится в точку (x,y)
procedure LineTo(x,y: integer; c: Color);

/// Рисует отрезок от точки (x1,y1) до точки (x2,y2)
procedure Line(x1,y1,x2,y2: integer);
/// Рисует отрезок от точки (x1,y1) до точки (x2,y2) цветом c
procedure Line(x1,y1,x2,y2: integer; c: Color);

/// Заполняет внутренность окружности с центром (x,y) и радиусом r
procedure FillCircle(x,y,r: integer);
/// Рисует окружность с центром (x,y) и радиусом r
procedure DrawCircle(x,y,r: integer);
/// Заполняет внутренность эллипса, ограниченного прямоугольником, заданным координатами противоположных вершин (x1,y1) и (x2,y2)
procedure FillEllipse(x1,y1,x2,y2: integer);
/// Рисует границу эллипса, ограниченного прямоугольником, заданным координатами противоположных вершин (x1,y1) и (x2,y2)
procedure DrawEllipse(x1,y1,x2,y2: integer);
/// Заполняет внутренность прямоугольника, заданного координатами противоположных вершин (x1,y1) и (x2,y2)
procedure FillRectangle(x1,y1,x2,y2: integer);
/// Заполняет внутренность прямоугольника, заданного координатами противоположных вершин (x1,y1) и (x2,y2)
procedure FillRect(x1,y1,x2,y2: integer);
/// Рисует границу прямоугольника, заданного координатами противоположных вершин (x1,y1) и (x2,y2)
procedure DrawRectangle(x1,y1,x2,y2: integer);
/// Заполняет внутренность прямоугольника со скругленными краями; (x1,y1) и (x2,y2) задают пару противоположных вершин, а w и h – ширину и высоту эллипса, используемого для скругления краев
procedure FillRoundRect(x1,y1,x2,y2,w,h: integer);
/// Рисует границу прямоугольника со скругленными краями; (x1,y1) и (x2,y2) задают пару противоположных вершин, а w и h – ширину и высоту эллипса, используемого для скругления краев
procedure DrawRoundRect(x1,y1,x2,y2,w,h: integer);

/// Рисует заполненную окружность с центром (x,y) и радиусом r
procedure Circle(x,y,r: integer);
/// Рисует заполненный эллипс, ограниченный прямоугольником, заданным координатами противоположных вершин (x1,y1) и (x2,y2)
procedure Ellipse(x1,y1,x2,y2: integer);
/// Рисует заполненный прямоугольник, заданный координатами противоположных вершин (x1,y1) и (x2,y2)
procedure Rectangle(x1,y1,x2,y2: integer);
/// Рисует заполненный прямоугольник со скругленными краями; (x1,y1) и (x2,y2) задают пару противоположных вершин, а w и h – ширину и высоту эллипса, используемого для скругления краев
procedure RoundRect(x1,y1,x2,y2,w,h: integer);

/// Рисует дугу окружности с центром в точке (x,y) и радиусом r, заключенной между двумя лучами, образующими углы a1 и a2 с осью OX (a1 и a2 – вещественные, задаются в градусах и отсчитываются против часовой стрелки)
procedure Arc(x,y,r,a1,a2: integer);
/// Заполняет внутренность сектора окружности, ограниченного дугой с центром в точке (x,y) и радиусом r, заключенной между двумя лучами, образующими углы a1 и a2 с осью OX (a1 и a2 – вещественные, задаются в градусах и отсчитываются против часовой стрелки)
procedure FillPie(x,y,r,a1,a2: integer);
/// Рисует сектор окружности, ограниченный дугой с центром в точке (x,y) и радиусом r, заключенной между двумя лучами, образующими углы a1 и a2 с осью OX (a1 и a2 – вещественные, задаются в градусах и отсчитываются против часовой стрелки)
procedure DrawPie(x,y,r,a1,a2: integer);
/// Рисует заполненный сектор окружности, ограниченный дугой с центром в точке (x,y) и радиусом r, заключенной между двумя лучами, образующими углы a1 и a2 с осью OX (a1 и a2 – вещественные, задаются в градусах и отсчитываются против часовой стрелки)
procedure Pie(x,y,r,a1,a2: integer);

/// Рисует замкнутую ломаную по точкам, координаты которых заданы в массиве points
procedure DrawPolygon(points: array of Point);
/// Заполняет многоугольник, координаты вершин которого заданы в массиве points
procedure FillPolygon(points: array of Point);
/// Рисует заполненный многоугольник, координаты вершин которого заданы в массиве points
procedure Polygon(points: array of Point);
/// Рисует ломаную по точкам, координаты которых заданы в массиве points
procedure Polyline(points: array of Point);
/// Рисует кривую по точкам, координаты которых заданы в массиве points
procedure Curve(points: array of Point);
/// Рисует замкнутую кривую по точкам, координаты которых заданы в массиве points
procedure DrawClosedCurve(points: array of Point);
/// Заполняет замкнутую кривую по точкам, координаты которых заданы в массиве points
procedure FillClosedCurve(points: array of Point);
/// Рисует заполненную замкнутую кривую по точкам, координаты которых заданы в массиве points
procedure ClosedCurve(points: array of Point);

/// Выводит строку s в прямоугольник к координатами левого верхнего угла (x,y)
procedure TextOut(x,y: integer; s: string); 
/// Заливает область одного цвета цветом c, начиная с точки (x,y).
procedure FloodFill(x,y: integer; c: Color);

{procedure FillCircle(x,y,r: integer; c: Color);
procedure DrawCircle(x,y,r: integer; c: Color);
procedure FillEllipse(x1,y1,x2,y2: integer; c: Color);
procedure DrawEllipse(x1,y1,x2,y2: integer; c: Color);
procedure FillRectangle(x1,y1,x2,y2: integer; c: Color);
procedure FillRect(x1,y1,x2,y2: integer; c: Color);
procedure DrawRectangle(x1,y1,x2,y2: integer; c: Color);
procedure DrawRoundRect(x1,y1,x2,y2,w,h: integer; c: Color);
procedure FillRoundRect(x1,y1,x2,y2,w,h: integer; c: Color);

procedure Circle(x,y,r: integer; c: Color);
procedure Ellipse(x1,y1,x2,y2: integer; c: Color);
procedure Rectangle(x1,y1,x2,y2: integer; c: Color);
procedure RoundRect(x1,y1,x2,y2,w,h: integer; c: Color);

procedure Arc(x,y,r,a1,a2: integer; c: Color);
procedure FillPie(x,y,r,a1,a2: integer; c: Color);
procedure DrawPie(x,y,r,a1,a2: integer; c: Color);
procedure Pie(x,y,r,a1,a2: integer; c: Color);

procedure DrawPolygon(a: array of Point; c: Color);
procedure FillPolygon(a: array of Point; c: Color);
procedure Polygon(a: array of Point; c: Color);
procedure Polyline(a: array of Point; c: Color);}

//--------------------------------------------
////                Цвета
//--------------------------------------------
/// Возвращает цвет, который содержит красную (r), зеленую (g) и синюю (b) составляющие (r,g и b - в диапазоне от 0 до 255)
function RGB(r,g,b: byte): Color;
/// Возвращает цвет, который содержит красную (r), зеленую (g) и синюю (b) составляющие и прозрачность (a) (a,r,g,b - в диапазоне от 0 до 255)
function ARGB(a,r,g,b: byte): Color;

/// Возвращает красный цвет с интенсивностью r (r - в диапазоне от 0 до 255)
function RedColor(r: byte): Color;
/// Возвращает зеленый цвет с интенсивностью g (g - в диапазоне от 0 до 255)
function GreenColor(g: byte): Color;
/// Возвращает синий цвет с интенсивностью b (b - в диапазоне от 0 до 255)
function BlueColor(b: byte): Color;
/// Возвращает случайный цвет
function clRandom: Color;

/// Возвращает красную составляющую цвета
function GetRed(c: Color): integer;
/// Возвращает зеленую составляющую цвета
function GetGreen(c: Color): integer;
/// Возвращает синюю составляющую цвета
function GetBlue(c: Color): integer;
/// Возвращает составляющую прозрачности цвета
function GetAlpha(c: Color): integer;

//--------------------------------------------
////                Перья
//--------------------------------------------
/// Устанавливает цвет текущего пера
procedure SetPenColor(c: Color); 
/// Возвращает цвет текущего пера
function PenColor: Color;
/// Устанавливает ширину текущего пера
procedure SetPenWidth(Width: integer);
/// Возвращает ширину текущего пера
function PenWidth: integer;
/// Устанавливает стиль текущего пера
procedure SetPenStyle(style: DashStyle);
/// Возвращает стиль текущего пера
function PenStyle: DashStyle;
/// Устанавливает режим текущего пера
procedure SetPenMode(m: integer);
/// Возвращает режим текущего пера
function PenMode: integer;

/// Возвращают x-координату текущей позиции рисования
function PenX: integer;
/// Возвращают y-координату текущей позиции рисования
function PenY: integer;

//--------------------------------------------
////                Кисти
//--------------------------------------------
/// Устанавливает цвет текущей кисти
procedure SetBrushColor(c: Color);
/// Возвращает цвет текущей кисти
function BrushColor: Color;
/// Устанавливает цвет текущей кисти
procedure SetBrushStyle(bs: BrushStyleType);
/// Возвращает цвет текущей кисти
function BrushStyle: BrushStyleType;
/// Устанавливает штриховку текущей кисти
procedure SetBrushHatch(bh: HatchStyle);
/// Возвращает штриховку текущей кисти
function BrushHatch: HatchStyle;
/// Устанавливает цвет заднего плана текущей штриховой кисти
procedure SetHatchBrushBackgroundColor(c: Color);
/// Возвращает цвет заднего плана текущей штриховой кисти
function HatchBrushBackgroundColor: Color;
/// Устанавливает второй цвет текущей градиентной кисти
procedure SetGradientBrushSecondColor(c: Color);
/// Возвращает второй цвет текущей градиентной кисти
function GradientBrushSecondColor: Color;

//--------------------------------------------
////                Шрифты
//--------------------------------------------
/// Устанавливает размер текущего шрифта в пунктах
procedure SetFontSize(size: integer);
/// Возвращает размер текущего шрифта в пунктах
function FontSize: integer;
/// Устанавливает имя текущего шрифта
procedure SetFontName(name: string);
/// Возвращает имя текущего шрифта
function FontName: string;
/// Устанавливает цвет текущего шрифта
procedure SetFontColor(c: Color);
/// Возвращает цвет текущего шрифта
function FontColor: Color;
/// Устанавливает стиль текущего шрифта
procedure SetFontStyle(fs: FontStyleType);
/// Возвращает стиль текущего шрифта
function FontStyle: FontStyleType;
/// Возвращает ширину строки s в пикселях при текущих настройках шрифта
function TextWidth(s: string): integer;
/// Возвращает высоту строки s в пикселях при текущих настройках шрифта
function TextHeight(s: string): integer;

//--------------------------------------------
////                Графическое окно
//--------------------------------------------
/// Очищает графическое окно белым цветом
procedure ClearWindow;
/// Очищает графическое окно цветом c
procedure ClearWindow(c: Color);

/// Возвращает ширину клиентской части графического окна в пикселах
function WindowWidth: integer;
/// Возвращает высоту клиентской части графического окна в пикселах
function WindowHeight: integer;
/// Возвращает отступ графического окна от левого края экрана в пикселах
function WindowLeft: integer;
/// Возвращает отступ графического окна от верхнего края экрана в пикселах
function WindowTop: integer;
/// Возвращает центр графического окна 
function WindowCenter: Point;
/// Возвращает True, если графическое окно имеет фиксированный размер, и False в противном случае 
function WindowIsFixedSize: boolean;

/// Устанавливает ширину клиентской части графического окна в пикселах
procedure SetWindowWidth(w: integer);
/// Устанавливает высоту клиентской части графического окна в пикселах
procedure SetWindowHeight(h: integer);
/// Устанавливает отступ графического окна от левого края экрана в пикселах
procedure SetWindowLeft(l: integer);
/// Устанавливает отступ графического окна от верхнего края экрана в пикселах
procedure SetWindowTop(t: integer);
/// Устанавливает, имеет ли графическое окно фиксированный размер
procedure SetWindowIsFixedSize(b: boolean);

/// Устанавливает размеры клиентской части графического окна в пикселах
procedure SetWindowSize(w,h: integer);
/// Устанавливает отступ графического окна от левого верхнего края экрана в пикселах
procedure SetWindowPos(l,t: integer);

/// Возвращает ширину графического компонента в пикселах (по умолчанию совпадает с WindowWidth)
function GraphBoxWidth: integer;
/// Возвращает высоту графического компонента в пикселах (по умолчанию совпадает с WindowHeight)
function GraphBoxHeight: integer;
/// Возвращает отступ графического компонента от левого края окна в пикселах
function GraphBoxLeft: integer;
/// Возвращает отступ графического компонента от верхнего края окна в пикселах
function GraphBoxTop: integer;

/// Возвращает заголовок графического окна
function WindowCaption: string;
/// Возвращает заголовок графического окна
function WindowTitle: string;
/// Устанавливает заголовок графического окна
procedure SetWindowCaption(s: string);
/// Устанавливает заголовок графического окна
procedure SetWindowTitle(s: string);

/// Устанавливает ширину и высоту клиентской части графического окна в пикселах
procedure InitWindow(Left,Top,Width,Height: integer; BackColor: Color := clWhite);

/// Сохраняет содержимое графического окна в файл с именем fname
procedure SaveWindow(fname: string);
/// Восстанавливает содержимое графического окна из файла с именем fname
procedure LoadWindow(fname: string);
/// Заполняет содержимое графического окна обоями из файла с именем fname
procedure FillWindow(fname: string);
/// Закрывает графическое окно и завершает приложение
procedure CloseWindow;

/// Возвращает ширину экрана в пикселях
function ScreenWidth: integer;
/// Возвращает высоту экрана в пикселях
function ScreenHeight: integer;

/// Центрирует графическое окно по центру экрана
procedure CenterWindow;
/// Максимизирует графическое окно
procedure MaximizeWindow;
/// Сворачивает графическое окно
procedure MinimizeWindow;
/// Возвращает графическое окно к нормальному размеру
procedure NormalizeWindow;

//--------------------------------------------
////           Буферизация рисования
//--------------------------------------------
/// Перерисовывает содержимое графического окна. Вызывается в паре с LockDrawing
procedure Redraw;
///--
procedure FullRedraw;
/// Блокирует рисование на графическом окне. Перерисовка графического окна выполняется с помощью Redraw
procedure LockDrawing;
/// Снимает блокировку рисования на графическом окне и осуществляет его перерисовку
procedure UnlockDrawing;

//--------------------------------------------
////              Сглаживание
//--------------------------------------------
/// Устанавливает режим сглаживания
procedure SetSmoothing(sm: boolean);
/// Включает режим сглаживания
procedure SetSmoothingOn;
/// Выключает режим сглаживания
procedure SetSmoothingOff;
/// Возвращает True, если режим сглаживания установлен
function SmoothingIsOn: boolean;

//--------------------------------------------
////        Вспомогательные подпрограммы
//--------------------------------------------
/// Блокирует прорисовку графики. Вызывается для синхронизации
procedure LockGraphics;
/// Разблокирует прорисовку графики. Вызывается для синхронизации
procedure UnLockGraphics;

//------------------------------------------------------------
////        Подпрограммы для работы с системой координат
//------------------------------------------------------------
/// Устанавливает начало координат в точку (x0,y0)
procedure SetCoordinateOrigin(x0,y0: integer);
/// Устанавливает масштаб системы координат 
procedure SetCoordinateScale(sx,sy: real);
/// Устанавливает поворот системы координат 
procedure SetCoordinateAngle(a: real);

/// Инициализирует графическое окно. Используется для внутренних целей
procedure InitGraphABC;

type 
/// Тип пера GraphABC
  GraphABCPen = class
  public
    _NETPen: System.Drawing.Pen;
    procedure SetColor(c: GraphABC.Color);
    function GetColor: GraphABC.Color;
    procedure SetWidth(w: integer);
    function GetWidth: integer;
    procedure SetStyle(st: DashStyle);
    function GetStyle: DashStyle;
    procedure SetMode(m: integer);
    function GetMode: integer;
    procedure SetNETPen(p: System.Drawing.Pen);
    function GetX: integer;
    function GetY: integer;
  public  
    /// Текущее перо .NET
    property NETPen: System.Drawing.Pen read _NETPen write SetNETPen;
    /// Цвет пера
    property Color: GraphABC.Color read GetColor write SetColor;
    /// Ширина пера
    property Width: integer read GetWidth write SetWidth;
    /// Стиль пера
    property Style: DashStyle read GetStyle write SetStyle;
    /// Режим пера
    property Mode: integer read GetMode write SetMode;
    /// X-координата текущей позиции пера
    property X: integer read GetX;
    /// Y-координата текущей позиции пера
    property Y: integer read GetY;
  end;

/// Тип кисти GraphABC
  GraphABCBrush = class
  public
    _NETBrush: System.Drawing.Brush;
    procedure SetNETBrush(b: System.Drawing.Brush);
    procedure SetColor(c: GraphABC.Color);
    function GetColor: GraphABC.Color;
    procedure SetStyle(st: BrushStyleType);
    function GetStyle: BrushStyleType;
    procedure SetHatch(h: HatchStyle);
    function GetHatch: HatchStyle;
    procedure SetHatchBackgroundColor(c: GraphABC.Color);
    function GetHatchBackgroundColor: GraphABC.Color;
    procedure SetGradientSecondColor(c: GraphABC.Color);
    function GetGradientSecondColor: GraphABC.Color;
  public  
/// Текущая кисть .NET
    property NETBrush: System.Drawing.Brush read _NETBrush write SetNETBrush;
/// Цвет кисти
    property Color: GraphABC.Color read GetColor write SetColor;
/// Стиль кисти
    property Style: BrushStyleType read GetStyle write SetStyle;
/// Штриховка кисти
    property Hatch: HatchStyle read GetHatch write SetHatch;
/// Цвет заднего плана штриховой кисти
    property HatchBackgroundColor: GraphABC.Color read GetHatchBackgroundColor write SetHatchBackgroundColor;
/// Второй цвет градиентной кисти
    property GradientSecondColor: GraphABC.Color read GetGradientSecondColor write SetGradientSecondColor;
  end;
  
/// Тип шрифта GraphABC
  GraphABCFont = class
  public
    _NETFont: System.Drawing.Font;
    procedure SetNETFont(f: System.Drawing.Font);
    procedure SetColor(c: GraphABC.Color);
    function GetColor: GraphABC.Color;
    procedure SetStyle(st: FontStyleType);
    function GetStyle: FontStyleType;
    procedure SetSize(sz: integer);
    function GetSize: integer;
    procedure SetName(nm: string);
    function GetName: string;
  public  
/// Текущий шрифт .NET
    property NETFont: System.Drawing.Font read _NETFont write SetNETFont;
    /// Цвет шрифта
    property Color: GraphABC.Color read GetColor write SetColor;
    /// Стиль шрифта
    property Style: FontStyleType read GetStyle write SetStyle;
    /// Размер шрифта в пунктах
    property Size: integer read GetSize write SetSize;
    /// Наименование шрифта
    property Name: string read GetName write SetName;
  end;
  
  GraphABCCoordinate = class
  public
    coef: integer;
    procedure SetOriginX(x: integer);
    procedure SetOriginY(y: integer);
    procedure SetOrigin(p: Point);
    procedure SetAngle(a: real);
    procedure SetScaleX(sx: real);
    procedure SetScaleY(sy: real);
    function GetOriginX: integer;
    function GetOriginY: integer;
    function GetOrigin: Point;
    function GetAngle: real;
    function GetScaleX: real;
    function GetScaleY: real;
    function GetMatrix: System.Drawing.Drawing2D.Matrix;
  public
    constructor;
    /// Устанавливает параметры системы координат
    procedure SetTransform(x0,y0,angle,sx,sy: real);
    /// Устанавливает начало системы координат
    procedure SetOrigin(x0,y0: integer);
    /// Устанавливает масштаб системы координат
    procedure SetScale(sx,sy: real);
    /// Устанавливает масштаб системы координат
    procedure SetScale(scale: real);
    /// Устанавливает правую систему координат (ось OY направлена вверх, ось OX - вправо)
    procedure SetMathematic;
    /// Устанавливает левую систему координат (ось OY направлена вниз, ось OX - вправо)
    procedure SetStandard;
    /// X-координата начала координат относительно левого верхнего угла окна
    property OriginX: integer read GetOriginX write SetOriginX;
    /// Y-координата начала координат относительно левого верхнего угла окна
    property OriginY: integer read GetOriginY write SetOriginY;
    /// Координаты начала координат относительно левого верхнего угла окна
    property Origin: Point read GetOrigin write SetOrigin;
    /// Угол поворота системы координат
    property Angle: real read GetAngle write SetAngle;
    /// Масштаб системы координат по оси X
    property ScaleX: real read GetScaleX write SetScaleX;
    /// Масштаб системы координат по оси Y
    property ScaleY: real read GetScaleY write SetScaleY;
    /// Масштаб системы координат по обоим осям
    property Scale: real write SetScale;
    /// Матрица 3x3 преобразований координат
    property Matrix: System.Drawing.Drawing2D.Matrix read GetMatrix;
  end;
  
  GraphABCWindow = class
  public
    procedure SetLeft(l: integer);
    function GetLeft: integer;
    procedure SetTop(t: integer);
    function GetTop: integer;
    procedure SetWidth(w: integer);
    function GetWidth: integer;
    procedure SetHeight(h: integer);
    function GetHeight: integer;
    procedure SetCaption(c: string);
    function GetCaption: string;
    procedure SetIsFixedSize(b: boolean);
    function GetIsFixedSize: boolean;
  public
/// Отступ графического окна от левого края экрана в пикселах
    property Left: integer read GetLeft write SetLeft; 
/// Отступ графического окна от верхнего края экрана в пикселах
    property Top: integer read GetTop write SetTop; 
/// Ширина клиентской части графического окна в пикселах
    property Width: integer read GetWidth write SetWidth; 
/// Высота клиентской части графического окна в пикселах
    property Height: integer read GetHeight write SetHeight;
/// Заголовок графического окна
    property Caption: string read GetCaption write SetCaption;
/// Заголовок графического окна
    property Title: string read GetCaption write SetCaption;
/// Имеет ли графическое окно фиксированный размер
    property IsFixedSize: boolean read GetIsFixedSize write SetIsFixedSize;
/// Очищает графическое окно белым цветом
    procedure Clear;
/// Очищает графическое окно цветом c
    procedure Clear(c: Color);
/// Устанавливает размеры клиентской части графического окна в пикселах
    procedure SetSize(w,h: integer);
/// Устанавливает отступ графического окна от левого верхнего края экрана в пикселах
    procedure SetPos(l,t: integer);
/// Устанавливает положение, размеры и цвет графического окна
    procedure Init(Left,Top,Width,Height: integer; BackColor: Color := clWhite);
/// Сохраняет содержимое графического окна в файл с именем fname
    procedure Save(fname: string); 
/// Восстанавливает содержимое графического окна из файла с именем fname
    procedure Load(fname: string); 
/// Заполняет содержимое графического окна обоями из файла с именем fname
    procedure Fill(fname: string); 
/// Закрывает графическое окно и завершает приложение
    procedure Close;
/// Сворачивает графическое окно
    procedure Minimize;
/// Максимизирует графическое окно
    procedure Maximize;
/// Возвращает графическое окно к нормальному размеру
    procedure Normalize;
/// Центрирует графическое окно по центру экрана
    procedure CenterOnScreen;
/// Возвращает центр графического окна
    function Center: Point;
  end;

/// Тип рисунка GraphABC
  Picture = class
  public
    bmp,savedbmp: Bitmap;
    gb: Graphics;
    istransp: boolean;
    transpcolor: System.Drawing.Color;
    procedure SetWidth(w: integer);
    function GetWidth: integer;
    procedure SetHeight(h: integer);
    function GetHeight: integer;
    procedure SetTransparent(b: boolean);
    procedure SetTransparentColor(c: GraphABC.Color);
    function GetTransparentColor: GraphABC.Color;
  public
/// Создает рисунок размера w на h пикселей
    constructor Create(w,h: integer);
/// Создает рисунок из файла с именем fname
    constructor Create(fname: string);
/// Создает рисунок из прямоугольника r графического окна
    constructor Create(r: System.Drawing.Rectangle); // Create from screen
/// Загружает рисунок из файла с именем fname
    procedure Load(fname: string);
/// Сохраняет рисунок в файл с именем fname
    procedure Save(fname: string);
/// Устанавливает размер рисунка w на h пикселей
    procedure SetSize(w,h: integer);
/// Ширина рисунка в пикселах
    property Width: integer read GetWidth write SetWidth;
/// Высота рисунка в пикселах
    property Height: integer read GetHeight write SetHeight;
/// Прозрачность рисунка; прозрачный цвет задается свойством TransparentColor
    property Transparent: boolean read istransp write SetTransparent;
/// Прозрачный цвет рисунка. Должна быть установлена прозрачность Transparent := True
    property TransparentColor: GraphABC.Color read GetTransparentColor write SetTransparentColor;
/// Возвращает True, если изображение данного рисунка пересекается с изображением рисунка p, и False в противном случае. Белый цвет считается прозрачным
    function Intersect(p: Picture): boolean;
/// Выводит рисунок в позиции (x,y)
    procedure Draw(x,y: integer);
/// Выводит рисунок в позиции (x,y) на поверхность рисования g
    procedure Draw(x,y: integer; g: Graphics);
/// Выводит рисунок в позиции (x,y), масштабируя его к размеру (w,h)
    procedure Draw(x,y,w,h: integer);
/// Выводит рисунок в позиции (x,y), масштабируя его к размеру (w,h), на поверхность рисования g
    procedure Draw(x,y,w,h: integer; g: Graphics);
/// Выводит часть рисунка, заключенную в прямоугольнике r, в позиции (x,y)
    procedure Draw(x,y: integer; r: System.Drawing.Rectangle); // r - part of Picture
/// Выводит часть рисунка, заключенную в прямоугольнике r, в позиции (x,y) на поверхность рисования g
    procedure Draw(x,y: integer; r: System.Drawing.Rectangle; g: Graphics);
/// Выводит часть рисунка, заключенную в прямоугольнике r, в позиции (x,y), масштабируя его к размеру (w,h)
    procedure Draw(x,y,w,h: integer; r: System.Drawing.Rectangle); // r - part of Picture
/// Выводит часть рисунка, заключенную в прямоугольнике r, в позиции (x,y), масштабируя его к размеру (w,h), на поверхность рисования g
    procedure Draw(x,y,w,h: integer; r: System.Drawing.Rectangle; g: Graphics);
/// Копирует прямоугольник src рисунка p в прямоугольник dst текущего рисунка
    procedure CopyRect(dst: System.Drawing.Rectangle; p: Picture; src: System.Drawing.Rectangle);
/// Копирует прямоугольник src битового образа bmp в прямоугольник dst текущего рисунка
    procedure CopyRect(dst: System.Drawing.Rectangle; bmp: Bitmap; src: System.Drawing.Rectangle);
/// Зеркально отображает рисунок относительно горизонтальной оси симметрии
    procedure FlipHorizontal;
/// Зеркально отображает рисунок относительно вертикальной оси симметрии 
    procedure FlipVertical;

/// Закрашивает пиксел (x,y) рисунка цветом c 
    procedure SetPixel(x,y: integer; c: Color);
/// Закрашивает пиксел (x,y) рисунка цветом c 
    procedure PutPixel(x,y: integer; c: Color);
/// Возвращает цвет пиксела (x,y) рисунка
    function GetPixel(x,y: integer): Color;
 
/// Выводит на рисунке отрезок от точки (x1,y1) до точки (x2,y2)
    procedure Line(x1,y1,x2,y2: integer);
/// Выводит на рисунке отрезок от точки (x1,y1) до точки (x2,y2) цветом c
    procedure Line(x1,y1,x2,y2: integer; c: Color);

/// Заполняет на рисунке внутренность окружности с центром (x,y) и радиусом r
    procedure FillCircle(x,y,r: integer);
/// Выводит на рисунке окружность с центром (x,y) и радиусом r
    procedure DrawCircle(x,y,r: integer);
/// Заполняет на рисунке внутренность эллипса, ограниченного прямоугольником, заданным координатами противоположных вершин (x1,y1) и (x2,y2)
    procedure FillEllipse(x1,y1,x2,y2: integer);
/// Выводит на рисунке границу эллипса, ограниченного прямоугольником, заданным координатами противоположных вершин (x1,y1) и (x2,y2)
    procedure DrawEllipse(x1,y1,x2,y2: integer);
/// Заполняет на рисунке внутренность прямоугольника, заданного координатами противоположных вершин (x1,y1) и (x2,y2)
    procedure FillRectangle(x1,y1,x2,y2: integer);
/// Заполняет на рисунке внутренность прямоугольника, заданного координатами противоположных вершин (x1,y1) и (x2,y2)
    procedure FillRect(x1,y1,x2,y2: integer);
/// Выводит на рисунке границу ы прямоугольника, заданного координатами противоположных вершин (x1,y1) и (x2,y2)
    procedure DrawRectangle(x1,y1,x2,y2: integer);

/// Выводит на рисунке заполненную окружность с центром (x,y) и радиусом r
    procedure Circle(x,y,r: integer);
/// Выводит на рисунке заполненный эллипс, ограниченный прямоугольником, заданным координатами противоположных вершин (x1,y1) и (x2,y2)
    procedure Ellipse(x1,y1,x2,y2: integer);
/// Выводит на рисунке заполненный прямоугольник, заданный координатами противоположных вершин (x1,y1) и (x2,y2)
    procedure Rectangle(x1,y1,x2,y2: integer);
/// Выводит на рисунке заполненный прямоугольник со скругленными краями; (x1,y1) и (x2,y2) задают пару противоположных вершин, а w и h – ширину и высоту эллипса, используемого для скругления краев
    procedure RoundRect(x1,y1,x2,y2,w,h: integer);
 
/// Выводит на рисунке дугу окружности с центром в точке (x,y) и радиусом r, заключенной между двумя лучами, образующими углы a1 и a2 с осью OX (a1 и a2 – вещественные, задаются в градусах и отсчитываются против часовой стрелки)
    procedure Arc(x,y,r,a1,a2: integer);
/// Заполняет на рисунке внутренность сектора окружности, ограниченного дугой с центром в точке (x,y) и радиусом r, заключенной между двумя лучами, образующими углы a1 и a2 с осью OX (a1 и a2 – вещественные, задаются в градусах и отсчитываются против часовой стрелки)
    procedure FillPie(x,y,r,a1,a2: integer);
/// Выводит на рисунке сектор окружности, ограниченный дугой с центром в точке (x,y) и радиусом r, заключенной между двумя лучами, образующими углы a1 и a2 с осью OX (a1 и a2 – вещественные, задаются в градусах и отсчитываются против часовой стрелки)
    procedure DrawPie(x,y,r,a1,a2: integer);
/// Выводит на рисунке заполненный сектор окружности, ограниченный дугой с центром в точке (x,y) и радиусом r, заключенной между двумя лучами, образующими углы a1 и a2 с осью OX (a1 и a2 – вещественные, задаются в градусах и отсчитываются против часовой стрелки)
    procedure Pie(x,y,r,a1,a2: integer);

/// Выводит на рисунке замкнутую ломаную по точкам, координаты которых заданы в массиве points
    procedure DrawPolygon(points: array of Point);
/// Заполняет на рисунке многоугольник, координаты вершин которого заданы в массиве points
    procedure FillPolygon(points: array of Point);
/// Выводит на рисунке заполненный многоугольник, координаты вершин которого заданы в массиве points
    procedure Polygon(points: array of Point);
/// Выводит на рисунке ломаную по точкам, координаты которых заданы в массиве points
    procedure Polyline(points: array of Point);
/// Выводит на рисунке кривую по точкам, координаты которых заданы в массиве points
    procedure Curve(points: array of Point);
/// Выводит на рисунке замкнутую кривую по точкам, координаты которых заданы в массиве points
    procedure DrawClosedCurve(points: array of Point);
/// Заполняет на рисунке замкнутую кривую по точкам, координаты которых заданы в массиве points
    procedure FillClosedCurve(points: array of Point);
/// Выводит на рисунке заполненную замкнутую кривую по точкам, координаты которых заданы в массиве points
    procedure ClosedCurve(points: array of Point);

/// Выводит на рисунке строку s в прямоугольник к координатами левого верхнего угла (x,y)
    procedure TextOut(x,y: integer; s: string); 
/// Заливает на рисунке область одного цвета цветом c, начиная с точки (x,y).
    procedure FloodFill(x,y: integer; c: Color);
    
/// Очищает рисунок белым цветом
    procedure Clear;
/// Очищает рисунок цветом c
    procedure Clear(c: Color);
  end;
  
type 
  ABCControl = class(Control)
  private
    procedure OnPaint(sender: Object; e: PaintEventArgs);
    procedure OnClosing(sender: Object; e: FormClosingEventArgs);
    procedure OnMouseDown(sender: Object; e: MouseEventArgs);
    procedure OnMouseUp(sender: Object; e: MouseEventArgs);
    procedure OnMouseMove(sender: Object; e: MouseEventArgs);
    procedure OnKeyDown(sender: Object; e: KeyEventArgs);
    procedure OnKeyUp(sender: Object; e: KeyEventArgs);
    procedure OnKeyPress(sender: Object; e: KeyPressEventArgs);
    procedure OnResize(sender: Object; e: EventArgs);
    procedure Init;
  protected
    function IsInputKey(keyData: Keys): boolean; override;
    procedure OnPaintBackground(e: PaintEventArgs); override; begin end; // сами все перерисовываем!
  public  
    constructor (w,h: integer); 
  end;
  
/// Создает рисунок размера w на h пикселов и записывает его в переменную p
procedure CreatePicture(var p: Picture; w,h: integer);
/// Возвращает окно графического приложения
function Window: GraphABCWindow;
/// Возвращает главную форму графического приложения
function MainForm: Form;
/// Возвращает графический компонент
function GraphABCControl: ABCControl;
/// Возвращает текущее перо
function Pen: GraphABCPen;
/// Возвращает текущую кисть
function Brush: GraphABCBrush;
/// Возвращает текущий шрифт
function Font: GraphABCFont;
/// Возвращает систему координат GraphABC
function Coordinate: GraphABCCoordinate;

function GraphWindowGraphics: Graphics;
function GraphBufferGraphics: Graphics;
function GraphBufferBitmap: Bitmap;

var 
/// Событие нажатия на кнопку мыши. (x,y) - координаты курсора мыши в момент наступления события, mousebutton = 1, если нажата левая кнопка мыши, и 2, если нажата правая кнопка мыши
  OnMouseDown: procedure (x,y,mousebutton: integer);
/// Событие отжатия кнопки мыши. (x,y) - координаты курсора мыши в момент наступления события, mousebutton = 1, если отжата левая кнопка мыши, и 2, если отжата правая кнопка мыши
  OnMouseUp: procedure (x,y,mousebutton: integer);
/// Событие перемещения мыши. (x,y) - координаты курсора мыши в момент наступления события, mousebutton = 0, если кнопка мыши не нажата, 1, если нажата левая кнопка мыши, и 2, если нажата правая кнопка мыши
  OnMouseMove: procedure (x,y,mousebutton: integer);
/// Событие нажатия клавиши
  OnKeyDown: procedure (key: integer);
/// Событие отжатия клавиши
  OnKeyUp: procedure (key: integer);
/// Событие нажатия символьной клавиши
  OnKeyPress: procedure (ch: char);
/// Событие изменения размера графического окна
  OnResize: procedure;
/// Событие закрытия графического окна
  OnClose: procedure;

/// Процедурная переменная перерисовки графического окна. Если равна nil, то используется стандартная перерисовка
  RedrawProc: procedure;
/// Следует ли рисовать во внеэкранном буфере
  DrawInBuffer: boolean := true;
  
implementation

uses 
  System.Threading,
  GraphABCHelper;
     
const 
  FILE_NOT_FOUND_MESSAGE = 'Файл {0} не найден';
  
type 
  Proc1Integer = procedure(x: integer);
  Proc1String = procedure(s: string);
  Proc2Integer = procedure(x,y: integer);
  Proc1Boolean = procedure(b: boolean);
  Proc1BorderStyle = procedure(st: FormBorderStyle);
  
var 
  _Window: GraphABCWindow := new GraphABCWindow;
  _MainForm: Form; 
  _GraphABCControl: ABCControl;
  _Pen: GraphABCPen := new GraphABCPen;
  _Brush: GraphABCBrush := new GraphABCBrush;
  _Font: GraphABCFont := new GraphABCFont;
  _Coordinate := new GraphABCCoordinate;


  __buffer: Bitmap;
  bmp: Bitmap; 
  gr: Graphics;
  gbmp: Graphics;

  f: ABCControl;

  CurrentSolidBrush: SolidBrush;
  CurrentHatchBrush: HatchBrush;
  CurrentGradientBrush: LinearGradientBrush;
  PixelBrush: SolidBrush;
  
  x_coord,y_coord: integer;
  NotLockDrawing: boolean;
  
  StartIsComplete: boolean;
  MainFormThread: System.Threading.Thread;
  
  writecoords: Point;
  CurrentWriteFont: System.Drawing.Font;

// ------------ GraphABCCoordinate -----------------
constructor GraphABCCoordinate.Create;
begin
  // 1 - компьютерная система координат (ось OY - вниз)
  // -1 - компьютерная система координат (ось OY - вниз)
  coef := 1;
end;

procedure GraphABCCoordinate.SetTransform(x0,y0,angle,sx,sy: real);
begin
  sx := abs(sx);
  sy := abs(sy);
  angle := DegToRad(angle);
  var m11 := sx * cos(angle);
  var m12 := coef * sx * sin(angle);
  var m21 := - sy * sin(angle);
  var m22 := coef * sy * cos(angle);
  var m := new System.Drawing.Drawing2D.Matrix(m11,m12,m21,m22,x0,y0);
  lock f do
  begin
    gr.Transform := m;
    gbmp.Transform := m;
  end;
end;

procedure GraphABCCoordinate.SetOrigin(x0,y0: integer);
begin
  SetTransform(x0,y0,Angle,ScaleX,ScaleY);
end;

procedure GraphABCCoordinate.SetScale(sx,sy: real);
begin
  SetTransform(OriginX,OriginY,Angle,sx,sy);
end;

procedure GraphABCCoordinate.SetScale(scale: real);
begin
  SetScale(scale,scale);
end;

procedure GraphABCCoordinate.SetMathematic;
begin
  coef := -1;
  SetTransform(OriginX,OriginY,Angle,ScaleX,ScaleY);
end;

procedure GraphABCCoordinate.SetStandard;
begin
  coef := 1;
  SetTransform(OriginX,OriginY,Angle,ScaleX,ScaleY);
end;

procedure GraphABCCoordinate.SetOriginX(x: integer);
begin
  SetOrigin(x,OriginY);
end;

procedure GraphABCCoordinate.SetOriginY(y: integer);
begin
  SetOrigin(OriginX,y);
end;

procedure GraphABCCoordinate.SetOrigin(p: Point);
begin
  SetOrigin(p.x,p.y);
end;

procedure GraphABCCoordinate.SetAngle(a: real);
begin
  SetTransform(OriginX,OriginY,a,ScaleX,ScaleY);
end;

procedure GraphABCCoordinate.SetScaleX(sx: real);
begin
  SetScale(sx,ScaleY);
end;

procedure GraphABCCoordinate.SetScaleY(sy: real);
begin
  SetScale(ScaleX,sy);
end;

function GraphABCCoordinate.GetOriginX: integer;
begin
  lock (f) do
    Result := Round(gr.Transform.OffsetX);
end;

function GraphABCCoordinate.GetOrigin: Point;
begin
  Result := new Point(GetOriginX,GetOriginY);
end;

function GraphABCCoordinate.GetOriginY: integer;
begin
  lock(f) do
    Result := Round(gr.Transform.OffsetY);
end;

function GraphABCCoordinate.GetAngle: real;
begin
  var a := Coordinate.Matrix.Elements;
  Result := ArcSin(a[1]/ScaleY);
  if a[0]<0 then
    if a[1]>0 then
      Result := Pi - Result
    else Result := -Pi - Result;
  Result *= coef * 180/Pi;
end;

function GraphABCCoordinate.GetScaleX: real;
begin
  var a := Coordinate.Matrix.Elements;
  Result := sqrt(sqr(a[0])+sqr(a[1]));
end;

function GraphABCCoordinate.GetScaleY: real;
begin
  var a := Coordinate.Matrix.Elements;
  Result := sqrt(sqr(a[2])+sqr(a[3]));
end;

function GraphABCCoordinate.GetMatrix: System.Drawing.Drawing2D.Matrix;
begin
  lock (f) do
    Result := gr.Transform;
end;

procedure SetCoordinateOrigin(x0,y0: integer);
begin
  Coordinate.SetOrigin(x0,y0);
end;

procedure SetCoordinateScale(sx,sy: real);
begin
  Coordinate.SetScale(sx,sy);
end;

procedure SetCoordinateAngle(a: real);
begin
  Coordinate.Angle := a;
end;

{function CurrentABCWindow: ABCWindow;
begin
  Result := f;
end;  }
  
procedure LockGraphics;
begin
  Monitor.Enter(f);
end;

procedure UnLockGraphics;
begin
  Monitor.Exit(f);
end;

procedure SetSmoothingOn;
begin
  gr.SmoothingMode := SmoothingMode.AntiAlias;
  gbmp.SmoothingMode := SmoothingMode.AntiAlias;
end;

procedure SetSmoothingOff;
begin
  gr.SmoothingMode := SmoothingMode.None;
  gbmp.SmoothingMode := SmoothingMode.None;
end;

procedure SetSmoothing(sm: boolean);
begin
  if sm then 
    SetSmoothingOn
  else SetSmoothingOff;  
end;

function SmoothingIsOn: boolean;
begin
  Result := gr.SmoothingMode = SmoothingMode.AntiAlias;
end;

function GraphWindowGraphics: Graphics;
begin
  Result := gr;
end;
  
function GraphBufferGraphics: Graphics;
begin
  Result := gbmp;
end;
  
function GraphBufferBitmap: Bitmap;
begin
  Result := bmp;
end;
  
procedure Swap(var x1,x2: integer);
begin
  var t := x1;
  x1 := x2;
  x2 := t;
end; 

// Graphics Primitives
// ------------ __MyPen -----------------
procedure GraphABCPen.SetColor(c: GraphABC.Color);
begin
  SetPenColor(c);
end;

function GraphABCPen.GetColor: GraphABC.Color;
begin
  Result := PenColor;
end;

procedure GraphABCPen.SetWidth(w: integer);
begin
  SetPenWidth(w);
end;

function GraphABCPen.GetWidth: integer;
begin
  Result := PenWidth;
end;

procedure GraphABCPen.SetStyle(st: DashStyle);
begin
  SetPenStyle(st);
end;

function GraphABCPen.GetStyle: DashStyle;
begin
  Result := PenStyle;
end;

procedure GraphABCPen.SetMode(m: integer);
begin
  SetPenMode(m);
end;

function GraphABCPen.GetMode: integer;
begin
  Result := PenMode;
end; 

procedure GraphABCPen.SetNETPen(p: System.Drawing.Pen);
begin
  if p=nil then exit;
  _NETPen := p;
end;

function GraphABCPen.GetX: integer;
begin
  Result := PenX;
end; 

function GraphABCPen.GetY: integer;
begin
  Result := PenY;
end; 

// ------------ GraphABCBrush -----------------
procedure GraphABCBrush.SetNETBrush(b: System.Drawing.Brush);
begin
  //if b=nil then Exit;
  _NETBrush := b;
end;

procedure GraphABCBrush.SetColor(c: GraphABC.Color);
begin
  SetBrushColor(c);
end;

function GraphABCBrush.GetColor: GraphABC.Color;
begin
  Result := BrushColor;
end;

procedure GraphABCBrush.SetStyle(st: BrushStyleType);
begin
  SetBrushStyle(st);
end;

function GraphABCBrush.GetStyle: BrushStyleType;
begin
  Result := BrushStyle;
end;

procedure GraphABCBrush.SetHatch(h: HatchStyle);
begin
  SetBrushHatch(h);
end;

function GraphABCBrush.GetHatch: HatchStyle;
begin
  Result := BrushHatch;
end;

procedure GraphABCBrush.SetHatchBackgroundColor(c: GraphABC.Color);
begin
  SetHatchBrushBackgroundColor(c);    
end;

function GraphABCBrush.GetHatchBackgroundColor: GraphABC.Color;
begin
  Result := HatchBrushBackgroundColor;
end;

procedure GraphABCBrush.SetGradientSecondColor(c: GraphABC.Color);
begin
  SetGradientBrushSecondColor(c);    
end;

function GraphABCBrush.GetGradientSecondColor: GraphABC.Color;
begin
  Result := GradientBrushSecondColor;
end;

// ------------ GraphABCFont -----------------
procedure GraphABCFont.SetNETFont(f: System.Drawing.Font);
begin
  if f=nil then Exit;
  _NetFont := f;
end;

procedure GraphABCFont.SetColor(c: GraphABC.Color);
begin
  SetFontColor(c); 
end;

function GraphABCFont.GetColor: GraphABC.Color;
begin
  Result := FontColor;
end;

procedure GraphABCFont.SetStyle(st: FontStyleType);
begin
  SetFontStyle(st); 
end;

function GraphABCFont.GetStyle: FontStyleType;
begin
  Result := FontStyle;
end;

procedure GraphABCFont.SetSize(sz: integer);
begin
  SetFontSize(sz); 
end;

function GraphABCFont.GetSize: integer;
begin
  Result := FontSize;
end;

procedure GraphABCFont.SetName(nm: string);
begin
  SetFontName(nm); 
end;

function GraphABCFont.GetName: string;
begin
  Result := FontName;
end;

// Picture
constructor Picture.Create(w,h: integer);
begin
  if (w<=0) or (h<=0) then
    raise new GraphABCException('w or h <= 0');
  bmp := new Bitmap(w,h);
  gb := Graphics.FromImage(bmp);
  transpcolor := bmp.GetPixel(0,bmp.Height-1);
  istransp := false;
  savedbmp := nil;
end;

constructor Picture.Create(fname: string);
begin
  try
    bmp := new Bitmap(fname);
  except on ex: System.ArgumentException do
    raise new System.IO.FileNotFoundException(string.Format(FILE_NOT_FOUND_MESSAGE,fname));
  end;
   
  gb := Graphics.FromImage(bmp);
  transpcolor := bmp.GetPixel(0,bmp.Height-1);
  istransp := false;
  savedbmp := nil;
end;

constructor Picture.Create(r: System.Drawing.Rectangle);
// Create from Screen
begin
  bmp := new Bitmap(r.Width,r.Height);
  gb := Graphics.FromImage(bmp);
  gb.CopyFromScreen(r.Left,r.Top,0,0,r.Size);
  transpcolor := bmp.GetPixel(0,bmp.Height-1);
  istransp := false;
  savedbmp := nil;
end;

procedure Picture.SetWidth(w: integer);
begin
  SetSize(w,Height);  
end;

function Picture.GetWidth: integer;
begin
  Result := bmp.Width;
end;

procedure Picture.SetHeight(h: integer);
begin
  SetSize(Width,h);  
end;

function Picture.GetHeight: integer;
begin
  Result := bmp.Height;
end;

// Логика установки прозрачности
// 1. Transparent := True  -  savedbmp:=bmp; bmp:=bmp.Clone; bmp.MakeTransparent(transpcolor);
// 2. Transparent := False  - bmp.Dispose; bmp := savedbmp; savedbmp := nil;
// 3. Transparentcolor := c
//   a) Если Transparent = False, то просто присвоить
//   б) Если Transparent = True, то bmp.Dispose; bmp:=savedbmp.Clone; bmp.MakeTransparent(transpcolor);

function Picture.GetTransparentColor: Color;
begin
  Result := transpcolor;
end;

procedure Picture.SetTransparentColor(c: Color);
var ob: Object;
begin
  if c=TransparentColor then 
    Exit;
  transpcolor := c;
  if istransp then 
  begin
    bmp.Dispose; 
    ob := savedbmp.Clone;
    bmp := Bitmap(ob);
    bmp.MakeTransparent(transpcolor);
  end;
end;

procedure Picture.SetTransparent(b: boolean);
var ob: Object;
begin
  if b = istransp then
    Exit;
  istransp := b;  
  if istransp then
  begin
    savedbmp := bmp; 
    ob := bmp.Clone;
    bmp := Bitmap(ob);
    bmp.MakeTransparent(transpcolor);  
  end
  else
  begin
    bmp.Dispose; 
    bmp := savedbmp; 
    savedbmp := nil;
  end;
end;

procedure Picture.Load(fname: string);
begin
  bmp.Dispose;
  bmp := new Bitmap(fname);
  gb := Graphics.FromImage(bmp);
  istransp := False;
  if savedbmp<>nil then
  begin
    savedbmp.Dispose;
    savedbmp := nil;
  end;
end;

procedure Picture.Save(fname: string);
begin
  bmp.Save(fname);
end;

procedure Picture.SetSize(w,h: integer);
var oldbmp: Bitmap;
begin
  oldbmp := bmp;
  bmp := new Bitmap(oldbmp,w,h);
  gb := Graphics.FromImage(bmp);
  gb.DrawImage(oldbmp,0,0);
  oldbmp.Dispose;
// TODO не знаю, может, надо что-то делать для прозрачной
{  if istransp then
  begin
  end}
end;

function Picture.Intersect(p: Picture): boolean;
begin
//  bmp.L
  result := false;
// TODO
end;

procedure Picture.Draw(x,y: integer);
begin
  Monitor.Enter(f);
  if NotLockDrawing then
    gr.DrawImage(bmp,x,y,bmp.Width,bmp.Height);
  if DrawInBuffer then   
    gbmp.DrawImage(bmp,x,y,bmp.Width,bmp.Height);
  Monitor.Exit(f);
end;

procedure Picture.Draw(x,y: integer; g: Graphics);
begin
  g.DrawImage(bmp,x,y,bmp.Width,bmp.Height);
end;

procedure Picture.Draw(x,y,w,h: integer);
// Draw bmp scaled to size w,h
begin
  Monitor.Enter(f);
  if NotLockDrawing then
    gr.DrawImage(bmp,x,y,w,h);
  if DrawInBuffer then   
    gbmp.DrawImage(bmp,x,y,w,h);
  Monitor.Exit(f);
end;

procedure Picture.Draw(x,y,w,h: integer; g: Graphics);
// Draw bmp scaled to size w,h
begin
  g.DrawImage(bmp,x,y,w,h);
end;

procedure Picture.Draw(x,y: integer; r: System.Drawing.Rectangle);
// Draw bmp in rectangle r
begin
  Monitor.Enter(f);
  if NotLockDrawing then
    gr.DrawImage(bmp,x,y,r,GraphicsUnit.Pixel);
  if DrawInBuffer then   
    gbmp.DrawImage(bmp,x,y,r,GraphicsUnit.Pixel);
  Monitor.Exit(f);
end;

procedure Picture.Draw(x,y: integer; r: System.Drawing.Rectangle; g: Graphics);
begin
  g.DrawImage(bmp,x,y,r,GraphicsUnit.Pixel);
end;

procedure Picture.Draw(x,y,w,h: integer; r: System.Drawing.Rectangle);
// Draw rectangle r portion of bmp scaled to size w,h
var 
  r1: System.Drawing.Rectangle;
  tempbmp: Bitmap;
begin
  r1 := new System.Drawing.Rectangle(x,y,w,h);
  tempbmp := GetView(bmp,r);
  Monitor.Enter(f);
  if NotLockDrawing then
    gr.DrawImage(tempbmp,r1);
//    gr.DrawImage(bmp,r1,r,GraphicsUnit.Pixel);
  if DrawInBuffer then   
    gbmp.DrawImage(tempbmp,r1);
//    gbmp.DrawImage(bmp,r1,r,GraphicsUnit.Pixel);
  Monitor.Exit(f);
  tempbmp.Dispose;
end;

procedure Picture.Draw(x,y,w,h: integer; r: System.Drawing.Rectangle; g: Graphics);
var tempbmp: Bitmap;
begin
  tempbmp := GetView(bmp,r);
  g.DrawImage(tempbmp,x,y,new System.Drawing.Rectangle(x,y,w,h),GraphicsUnit.Pixel);
  tempbmp.Dispose;
end;

procedure Picture.CopyRect(dst: System.Drawing.Rectangle; p: Picture; src: System.Drawing.Rectangle);
// Copy src portion of p on dst rectangle of this picture  
begin
  CopyRect(dst,p.bmp,src);
end;

procedure Picture.CopyRect(dst: System.Drawing.Rectangle; bmp: Bitmap; src: System.Drawing.Rectangle);
var tempbmp: Bitmap;
begin
// Copy src portion of bmp on dst rectangle of this picture  
  tempbmp := GetView(bmp,src);
//  gb.DrawImage(bmp,dst,src,GraphicsUnit.Pixel);
  gb.DrawImage(tempbmp,dst);
  tempbmp.Dispose;
end;

procedure Picture.FlipHorizontal;
begin
  bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
end;

procedure Picture.FlipVertical;
begin
  bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
end;

procedure Picture.SetPixel(x,y: integer; c: Color);
begin
  bmp.SetPixel(x,y,c);
end;

procedure Picture.PutPixel(x,y: integer; c: Color);
begin
  bmp.SetPixel(x,y,c);
end;

function Picture.GetPixel(x,y: integer): Color;
begin
  Result := bmp.GetPixel(x,y);
end;

procedure Picture.Line(x1,y1,x2,y2: integer);
begin
  GraphABCHelper.Line(x1,y1,x2,y2,gb);
end;

procedure Picture.Line(x1,y1,x2,y2: integer; c: Color);
begin
  GraphABCHelper.Line(x1,y1,x2,y2,c,gb);
end;

procedure Picture.FillCircle(x,y,r: integer);
begin
  GraphABCHelper.FillEllipse(x-r,y-r,x+r,y+r,gb);
end;

procedure Picture.DrawCircle(x,y,r: integer);
begin
  GraphABCHelper.DrawEllipse(x-r,y-r,x+r,y+r,gb);
end;

procedure Picture.FillEllipse(x1,y1,x2,y2: integer);
begin
  GraphABCHelper.FillEllipse(x1,y1,x2,y2,gb);
end;

procedure Picture.DrawEllipse(x1,y1,x2,y2: integer);
begin
  GraphABCHelper.DrawEllipse(x1,y1,x2,y2,gb);
end;

procedure Picture.FillRectangle(x1,y1,x2,y2: integer);
begin
  GraphABCHelper.FillRectangle(x1,y1,x2,y2,gb);
end;

procedure Picture.FillRect(x1,y1,x2,y2: integer);
begin
  GraphABCHelper.FillRectangle(x1,y1,x2,y2,gb);
end;

procedure Picture.DrawRectangle(x1,y1,x2,y2: integer);
begin
  GraphABCHelper.DrawRectangle(x1,y1,x2,y2,gb);
end;

procedure Picture.Circle(x,y,r: integer);
begin
  GraphABCHelper.Ellipse(x-r,y-r,x+r,y+r,gb);
end;

procedure Picture.Ellipse(x1,y1,x2,y2: integer);
begin
  GraphABCHelper.Ellipse(x1,y1,x2,y2,gb);
end;

procedure Picture.Rectangle(x1,y1,x2,y2: integer);
begin
  GraphABCHelper.Rectangle(x1,y1,x2,y2,gb);  
end;

procedure Picture.RoundRect(x1,y1,x2,y2,w,h: integer);
begin
  GraphABCHelper.RoundRect(x1,y1,x2,y2,w,h,gb);  
end;

procedure Picture.Arc(x,y,r,a1,a2: integer);
begin
  GraphABCHelper.Arc(x,y,r,a1,a2,gb);
end;

procedure Picture.FillPie(x,y,r,a1,a2: integer);
begin
  GraphABCHelper.FillPie(x,y,r,a1,a2,gb);
end;

procedure Picture.DrawPie(x,y,r,a1,a2: integer);
begin
  GraphABCHelper.DrawPie(x,y,r,a1,a2,gb);
end;

procedure Picture.Pie(x,y,r,a1,a2: integer);
begin
  GraphABCHelper.Pie(x,y,r,a1,a2,gb);
end;

procedure Picture.DrawPolygon(points: array of Point);
begin
  GraphABCHelper.DrawPolygon(points,gb);
end;

procedure Picture.FillPolygon(points: array of Point);
begin
  GraphABCHelper.FillPolygon(points,gb);
end;

procedure Picture.Polygon(points: array of Point);
begin
  GraphABCHelper.Polygon(points,gb);
end;

procedure Picture.Polyline(points: array of Point);
begin
  GraphABCHelper.Polyline(points,gb);
end;

procedure Picture.Curve(points: array of Point);
begin
  GraphABCHelper.Curve(points,gb);
end;

procedure Picture.DrawClosedCurve(points: array of Point);
begin
  GraphABCHelper.DrawClosedCurve(points,gb);
end;

procedure Picture.FillClosedCurve(points: array of Point);
begin
  GraphABCHelper.FillClosedCurve(points,gb);
end;

procedure Picture.ClosedCurve(points: array of Point);
begin
  GraphABCHelper.ClosedCurve(points,gb);
end;

procedure Picture.TextOut(x,y: integer; s: string); 
begin
  GraphABCHelper.TextOut(x,y,s,gb);
end;

function ExtFloodFill(hdc: IntPtr; x,y: integer; color: integer; filltype: integer): boolean; external 'Gdi32.dll' name 'ExtFloodFill';
function SelectObject(hdc, hgdiobj: IntPtr): IntPtr; external 'Gdi32.dll' name 'SelectObject';
function CreateSolidBrush(c: integer): IntPtr; external 'Gdi32.dll' name 'CreateSolidBrush';
function DeleteObject(obj: IntPtr): integer; external 'Gdi32.dll' name 'DeleteObject';
function CreateCompatibleDC(obj: IntPtr): IntPtr; external 'Gdi32.dll' name 'CreateCompatibleDC';

procedure Picture.FloodFill(x,y: integer; c: Color);  
var hdc,hBrush,hOldBrush: IntPtr;
begin
  var borderColor: Color := GetPixel(x,y);
  
  var bc := ColorTranslator.ToWin32(borderColor);
  var cc := ColorTranslator.ToWin32(c);

  Monitor.Enter(f);

  hdc := gbmp.GetHDC();
  hBrush := CreateSolidBrush(cc);

  hOldBrush := SelectObject(hdc, hBrush);
  ExtFloodFill(hdc, x, y, bc, 1);
  SelectObject(hdc, holdBrush);

  DeleteObject(hBrush);
  
  gr.ReleaseHdc();
  
  DeleteObject(hdc);
 
  Monitor.Exit(f);
end;

procedure Picture.Clear;
begin
  Monitor.Enter(f);
  gb.FillRectangle(Brushes.White,0,0,WindowWidth,WindowHeight);
  Monitor.Exit(f);
end;

procedure Picture.Clear(c: Color);
begin
  Monitor.Enter(f);
  gb.FillRectangle(new SolidBrush(c),0,0,WindowWidth,WindowHeight);
  Monitor.Exit(f);
end;

// ABCControl
function ABCControl.IsInputKey(keyData: Keys): boolean;
begin
  Result := True;
end;

procedure ABCControl.Init;
begin
  BackColor := System.Drawing.Color.White;
  Dock := DockStyle.Fill;
  
  Paint += OnPaint;
  MouseDown += OnMouseDown;
  MouseUp += OnMouseUp;
  MouseMove += OnMouseMove;
  Resize += OnResize;
  KeyDown += OnKeyDown;
  KeyUp += OnKeyUp;
  KeyPress += OnKeyPress;

// These Events must be initialised in main form
//  FormClosing += OnClosing;

// Initialization of global vars 

  bmp := new Bitmap(Screen.PrimaryScreen.Bounds.Width,Screen.PrimaryScreen.Bounds.Height);
  gr := Graphics.FromHwnd(Handle);
  gbmp := Graphics.FromImage(bmp);
  __buffer:=bmp;
  gbmp.FillRectangle(Brushes.White,0,0,Screen.PrimaryScreen.Bounds.Width,Screen.PrimaryScreen.Bounds.Height);
  _Pen.NETPen := new System.Drawing.Pen(System.Drawing.Color.Black);
  _Font.NETFont := new System.Drawing.Font('Arial',10);

  PixelBrush := new SolidBrush(System.Drawing.Color.Black);
  CurrentSolidBrush := new SolidBrush(System.Drawing.Color.White);
  CurrentHatchBrush := new HatchBrush(HatchStyle.Cross,System.Drawing.Color.Black,System.Drawing.Color.White);
  CurrentGradientBrush := new LinearGradientBrush(new Point(0,0), new Point(600,600),System.Drawing.Color.Black,System.Drawing.Color.White);
  _Brush.NETBrush := CurrentSolidBrush;

  CurrentWriteFont := new System.Drawing.Font('Courier New',10);

  NotLockDrawing := True;
  //DrawInBuffer := True;
  //RedrawProc := nil;
end;

constructor ABCControl.Create(w,h: integer);
begin
  ClientSize := new System.Drawing.Size(w,h);
  Init;
end;

procedure ABCControl.OnPaint(sender: object; e: PaintEventArgs);
begin
  Monitor.Enter(f);
  if (e <> nil) and NotLockDrawing then
  begin
    if RedrawProc<>nil then
      RedrawProc
    else e.Graphics.DrawImage(bmp,0,0);
  end;  
  Monitor.Exit(f);
end;

procedure ABCControl.OnClosing(sender: object; e: FormClosingEventArgs);
begin
  if GraphABC.OnClose<>nil then  
    GraphABC.OnClose;
  NotLockDrawing := False;
  //Sleep(0);
  Halt;
end;

procedure ABCControl.OnMouseDown(sender: Object; e: MouseEventArgs);
type MouseButtons = System.Windows.Forms.MouseButtons;
var mb: integer;
begin
  if e.Button = MouseButtons.Left then
    mb := 1
  else if e.Button = MouseButtons.Right then
    mb := 2;
  if GraphABC.OnMouseDown<>nil then  
    GraphABC.OnMouseDown(e.x,e.y,mb); 
end;

procedure ABCControl.OnMouseUp(sender: Object; e: MouseEventArgs);
type MouseButtons = System.Windows.Forms.MouseButtons;
var mb: integer;
begin
  if e.Button = MouseButtons.Left then
    mb := 1
  else if e.Button = MouseButtons.Right then
    mb := 2;
  if GraphABC.OnMouseUp<>nil then  
    GraphABC.OnMouseUp(e.x,e.y,mb);  
end;

procedure ABCControl.OnMouseMove(sender: Object; e: MouseEventArgs);
type MouseButtons = System.Windows.Forms.MouseButtons;
var mb: integer;
begin
  if e.Button = MouseButtons.Left then
    mb := 1
  else if e.Button = MouseButtons.Right then
    mb := 2;
  if GraphABC.OnMouseMove<>nil then  
    GraphABC.OnMouseMove(e.x,e.y,mb);  
end;

procedure ABCControl.OnKeyDown(sender: Object; e: KeyEventArgs);
begin
  if GraphABC.OnKeyDown<>nil then  
    GraphABC.OnKeyDown(integer(e.KeyCode));
end;

procedure ABCControl.OnKeyUp(sender: Object; e: KeyEventArgs);
begin
  if GraphABC.OnKeyUp<>nil then  
    GraphABC.OnKeyUp(integer(e.KeyCode));
end;

procedure ABCControl.OnKeyPress(sender: Object; e: KeyPressEventArgs);
begin
  if GraphABC.OnKeyPress<>nil then  
    GraphABC.OnKeyPress(e.KeyChar);
end;

procedure ResizeHelper;
var t: SmoothingMode;
begin
  Monitor.Enter(f);
  t := gr.SmoothingMode;
  var m := gr.Transform;
  gr := Graphics.FromHwnd(f.Handle);
  gr.Transform := m;
  gr.SmoothingMode := t;
  Monitor.Exit(f);
end;

procedure ABCControl.OnResize(sender: Object; e: EventArgs);
begin
  ResizeHelper;
  if GraphABC.OnResize<>nil then  
    GraphABC.OnResize;
end;

// Primitives
procedure SetPixel(x,y: integer; c: Color);
var b: boolean;
begin
  lock f do begin
    if NotLockDrawing then begin
      b := SmoothingIsOn;
      SetSmoothingOff;
      PixelBrush.Color := c;  
      gr.FillRectangle(PixelBrush,x,y,1,1);
      SetSmoothing(b);  
    end;
    if DrawInBuffer then   
      bmp.SetPixel(x,y,c);
  end;
end;

procedure PutPixel(x,y: integer; c: Color);
var b: boolean;
begin
  Monitor.Enter(f);
  b := SmoothingIsOn;
  SetSmoothingOff;
  PixelBrush.Color := c;
  if NotLockDrawing then
    gr.FillRectangle(PixelBrush,x,y,1,1);
  if DrawInBuffer then   
    gbmp.FillRectangle(PixelBrush,x,y,1,1);
  SetSmoothing(b);  
  Monitor.Exit(f);
end;

function GetPixel(x,y: integer): Color;
begin
  Monitor.Enter(f);
  Result := bmp.GetPixel(x,y);
  Monitor.Exit(f);
end;

procedure MoveTo(x,y: integer);
begin
  x_coord := x;
  y_coord := y;
end;

procedure LineTo(x,y: integer);
begin
  Line(x_coord,y_coord,x,y);
  x_coord := x;
  y_coord := y;
end;

procedure LineTo(x,y: integer; c: Color);
begin
  Line(x_coord,y_coord,x,y,c);
  x_coord := x;
  y_coord := y;
end;

procedure Line(x1,y1,x2,y2: integer; c: Color);
begin
  Monitor.Enter(f);
  if NotLockDrawing then
    Line(x1,y1,x2,y2,c,gr);
  if DrawInBuffer then   
    Line(x1,y1,x2,y2,c,gbmp);
  Monitor.Exit(f);
end;

procedure Line(x1,y1,x2,y2: integer);
begin
  Monitor.Enter(f);
  if NotLockDrawing then
    Line(x1,y1,x2,y2,gr);
  if DrawInBuffer then   
    Line(x1,y1,x2,y2,gbmp);
  Monitor.Exit(f);
end;

procedure FillCircle(x,y,r: integer);
begin
  FillEllipse(x-r,y-r,x+r,y+r);
end;

procedure DrawCircle(x,y,r: integer);
begin
  DrawEllipse(x-r,y-r,x+r,y+r);
end;

procedure Circle(x,y,r: integer);
begin
  Ellipse(x-r,y-r,x+r,y+r);
end;

procedure FillEllipse(x1,y1,x2,y2: integer);
begin
  Monitor.Enter(f);
  if NotLockDrawing then
    FillEllipse(x1,y1,x2,y2,gr);
  if DrawInBuffer then   
    FillEllipse(x1,y1,x2,y2,gbmp);
  Monitor.Exit(f);
end;

procedure DrawEllipse(x1,y1,x2,y2: integer);
begin
  Monitor.Enter(f);
  if NotLockDrawing then
    DrawEllipse(x1,y1,x2,y2,gr);
  if DrawInBuffer then   
    DrawEllipse(x1,y1,x2,y2,gbmp);
  Monitor.Exit(f);
end;

procedure Ellipse(x1,y1,x2,y2: integer);
begin
  if Brush.NETBrush <> nil then 
    FillEllipse(x1,y1,x2,y2);
  if Pen.NETPen.DashStyle <> DashStyle.Custom then
    DrawEllipse(x1,y1,x2,y2);
end;

procedure FillRectangle(x1,y1,x2,y2: integer);
begin
  Monitor.Enter(f);
  if NotLockDrawing then
    FillRectangle(x1,y1,x2,y2,gr);
  if DrawInBuffer then   
    FillRectangle(x1,y1,x2,y2,gbmp);
  Monitor.Exit(f);
end;

procedure DrawRectangle(x1,y1,x2,y2: integer);
begin
  Monitor.Enter(f);
  if NotLockDrawing then
    DrawRectangle(x1,y1,x2,y2,gr);
  if DrawInBuffer then   
    DrawRectangle(x1,y1,x2,y2,gbmp);
  Monitor.Exit(f);
end;

procedure Rectangle(x1,y1,x2,y2: integer);
begin
  if x1>x2 then 
    Swap(x1,x2);
  if y1>y2 then 
    Swap(y1,y2);
  if Brush.NETBrush <> nil then 
    FillRectangle(x1,y1,x2-1,y2-1);
  if Pen.NETPen.DashStyle <> DashStyle.Custom then
    DrawRectangle(x1,y1,x2,y2);
end;

procedure DrawRoundRect(x1,y1,x2,y2,w,h: integer);
begin
  Monitor.Enter(f);
  if NotLockDrawing then
    DrawRoundRect(x1,y1,x2,y2,w,h,gr);
  if DrawInBuffer then   
    DrawRoundRect(x1,y1,x2,y2,w,h,gbmp);
  Monitor.Exit(f);
end;

procedure FillRoundRect(x1,y1,x2,y2,w,h: integer);
begin
  Monitor.Enter(f);
  if NotLockDrawing then
    FillRoundRect(x1,y1,x2,y2,w,h,gr);
  if DrawInBuffer then   
    FillRoundRect(x1,y1,x2,y2,w,h,gbmp);
  Monitor.Exit(f);
end;

procedure RoundRect(x1,y1,x2,y2,w,h: integer);
begin
  if Brush.NETBrush <> nil then 
    FillRoundRect(x1,y1,x2,y2,w,h);
  if Pen.NETPen.DashStyle <> DashStyle.Custom then
    DrawRoundRect(x1,y1,x2,y2,w,h);
end;

procedure Arc(x,y,r,a1,a2: integer);
begin
  Monitor.Enter(f);
  if NotLockDrawing then
    Arc(x,y,r,a1,a2,gr);
  if DrawInBuffer then   
    Arc(x,y,r,a1,a2,gbmp);
  Monitor.Exit(f);
end;

procedure FillPie(x,y,r,a1,a2: integer);
begin
  Monitor.Enter(f);
  if NotLockDrawing then
    FillPie(x,y,r,a1,a2,gr);
  if DrawInBuffer then   
    FillPie(x,y,r,a1,a2,gbmp);
  Monitor.Exit(f);
end;

procedure DrawPie(x,y,r,a1,a2: integer);
begin
  Monitor.Enter(f);
  if NotLockDrawing then
    DrawPie(x,y,r,a1,a2,gr);
  if DrawInBuffer then   
    DrawPie(x,y,r,a1,a2,gbmp);
  Monitor.Exit(f);
end;

procedure Pie(x,y,r,a1,a2: integer);
begin
  if Brush.NETBrush <> nil then 
    FillPie(x,y,r,a1,a2);
  if Pen.NETPen.DashStyle <> DashStyle.Custom then
    DrawPie(x,y,r,a1,a2);
end;

procedure TextOut(x,y: integer; s: string); 
begin
  Monitor.Enter(f);
  if NotLockDrawing then
    TextOut(x,y,s,gr);
  if DrawInBuffer then   
    TextOut(x,y,s,gbmp);
  Monitor.Exit(f);
end;

procedure DrawPolygon(points: array of Point);
begin
  Monitor.Enter(f);
  if NotLockDrawing then
    DrawPolygon(points,gr);
  if DrawInBuffer then   
    DrawPolygon(points,gbmp);
  Monitor.Exit(f);
end;

procedure FillPolygon(points: array of Point);
begin
  Monitor.Enter(f);
  if NotLockDrawing then
    FillPolygon(points,gr);
  if DrawInBuffer then   
    FillPolygon(points,gbmp);
  Monitor.Exit(f);
end;

procedure Polygon(points: array of Point);
begin
  if Brush.NETBrush <> nil then 
    FillPolygon(points);
  if Pen.NETPen.DashStyle <> DashStyle.Custom then
    DrawPolygon(points);
end;

procedure Polyline(points: array of Point);
begin
  Monitor.Enter(f);
  if NotLockDrawing then
    Polyline(points,gr);
  if DrawInBuffer then   
    Polyline(points,gbmp);
  Monitor.Exit(f);
end;

procedure Curve(points: array of Point);
begin
  Monitor.Enter(f);
  if NotLockDrawing then
    Curve(points,gr);
  if DrawInBuffer then   
    Curve(points,gbmp);
  Monitor.Exit(f);
end;

procedure DrawClosedCurve(points: array of Point);
begin
  Monitor.Enter(f);
  if NotLockDrawing then
    DrawClosedCurve(points,gr);
  if DrawInBuffer then   
    DrawClosedCurve(points,gbmp);
  Monitor.Exit(f);
end;

procedure FillClosedCurve(points: array of Point);
begin
  Monitor.Enter(f);
  if NotLockDrawing then
    FillClosedCurve(points,gr);
  if DrawInBuffer then   
    FillClosedCurve(points,gbmp);
  Monitor.Exit(f);
end;

procedure ClosedCurve(points: array of Point);
begin
  Monitor.Enter(f);
  if NotLockDrawing then
    ClosedCurve(points,gr);
  if DrawInBuffer then   
    ClosedCurve(points,gbmp);
  Monitor.Exit(f);
end;

// Fills
procedure FloodFill(x,y: integer; c: Color);
var hdc,hBrush,hOldBrush: IntPtr;
begin
  var borderColor: Color := GetPixel(x,y);
//  var bc: integer := integer(borderColor.R) + (integer(borderColor.G) shl 8) + (integer(borderColor.B) shl 16);
//  var cc: integer := integer(c.R) + (integer(c.G) shl 8) + (integer(c.B) shl 16);
  
  var bc := ColorTranslator.ToWin32(borderColor);
  var cc := ColorTranslator.ToWin32(c);

  Monitor.Enter(f);

  hdc := gr.GetHDC();
  hBrush := CreateSolidBrush(cc);

  hOldBrush := SelectObject(hdc, hBrush);
  ExtFloodFill(hdc, x, y, bc, 1);
  SelectObject(hdc, holdBrush);

  var hbmp := bmp.GetHbitmap(); // Создается GDI Bitmap
  var memdc: IntPtr := CreateCompatibleDC(hdc);
  SelectObject(memdc,hbmp);
  
  hOldBrush := SelectObject(memdc, hBrush);
  ExtFloodFill(memdc, x, y, bc, 1);
  SelectObject(hdc, holdBrush);
  
  var bmp1 := Bitmap.FromHbitmap(hbmp);
  gbmp.DrawImage(bmp1,0,0);

  bmp1.Dispose();
//  bmp := Bitmap.FromHbitmap(hbmp);

  DeleteObject(memdc);
  DeleteObject(hbmp);
  DeleteObject(hBrush);
  
  gr.ReleaseHdc();
  
  DeleteObject(hdc);
 
  Monitor.Exit(f);
end;

procedure FillRect(x1,y1,x2,y2: integer);
begin
  FillRectangle(x1,y1,x2,y2);
end;

// Colors
function RGB(r,g,b: byte): Color;
begin
  Result := System.Drawing.Color.FromArgb(r,g,b);
end;

//------------------------------------------------------------------------------
// Color from component
//------------------------------------------------------------------------------
const AlphaBase = $FF000000;

function RedColor(r: byte): Color;
begin
  Result := System.Drawing.Color.FromArgb(AlphaBase or (r shl 16));
end;

function GreenColor(g: byte): Color;
begin
  Result := System.Drawing.Color.FromArgb(AlphaBase or (g shl 8));
end;

function BlueColor(b: byte): Color;
begin
  Result := System.Drawing.Color.FromArgb(AlphaBase or b);
end;
//------------------------------------------------------------------------------

function ARGB(a,r,g,b: byte) : Color;
begin
  Result := System.Drawing.Color.FromArgb(a,r,g,b);
//  Result := ((((r shl 16) or (g shl 8)) or b) or (a shl 24)) and -1;
end;

function clRandom: Color;
begin
  Result:=System.Drawing.Color.FromArgb(255,PABCSystem.Random(255),PABCSystem.Random(255),PABCSystem.Random(255));
end;

function GetRed(c: Color): integer;
begin
  Result := c.R;
//  Result := (c shr 16) and 255;
end;

function GetGreen(c: Color): integer;
begin
  Result := c.G;
//  Result := (c shr 8) and 255;
end;

function GetBlue(c: Color): integer;
begin
  Result := c.B;
//  Result := c and 255;
end;

function GetAlpha(c: Color): integer;
begin
  Result := c.A;
end;

// Pens
procedure SetPenColor(c: Color);
begin
  //if Pen.NETPen.Color <> c then
    lock f do
      Pen.NETPen.Color := c;
end;

function PenColor: Color;
begin
  Result := Pen.NETPen.Color;
end;

procedure SetPenWidth(Width: integer);
begin
  lock f do
    Pen.NETPen.Width := Width;
end;

function PenWidth: integer;
begin
  Result := round(Pen.NETPen.Width);
end;

procedure SetPenStyle(style: DashStyle);
begin
  LockGraphics; 
//  try
  case style of
    psSolid:		  Pen.NETPen.DashStyle := DashStyle.Solid;
    psClear:    	Pen.NETPen.DashStyle := DashStyle.Custom;
    psDash:  		  Pen.NETPen.DashStyle := DashStyle.Dash;
    psDot: 			  Pen.NETPen.DashStyle := DashStyle.Dot;
    psDashDot:		Pen.NETPen.DashStyle := DashStyle.DashDot;
    psDashDotDot:	Pen.NETPen.DashStyle := DashStyle.DashDotDot;
  end;  
{    except
      on e: System.InvalidOperationException do
        writeln(e);
    end;}
  UnLockGraphics; 
end;

function PenStyle: DashStyle;
begin
  case Pen.NETPen.DashStyle of
    DashStyle.Solid:      Result := psSolid;
    DashStyle.Dash:       Result := psDash;
    DashStyle.Dot:        Result := psDot;
    DashStyle.DashDot:    Result := psDashDot;
    DashStyle.DashDotDot: Result := psDashDotDot;
    DashStyle.Custom:     Result := psClear;
  end;  
end;

procedure SetPenMode(m: integer);
begin
// TODO  
end;

function PenMode: integer;
begin
  result := -1;
// TODO
end;

function PenX: integer;
begin
  Result := x_coord;
end;

function PenY: integer;
begin
  Result := y_coord;
end;

// Brushes
procedure SetBrushColor(c: Color);
begin
  LockGraphics;
  if Brush.NETBrush = CurrentHatchBrush then
  begin
    CurrentHatchBrush := new HatchBrush(CurrentHatchBrush.HatchStyle,c,CurrentHatchBrush.BackgroundColor);
    Brush.NETBrush := CurrentHatchBrush;
  end  
  else 
  begin
//    try
    CurrentSolidBrush.Color := c;
{    except
      on e: System.InvalidOperationException do
        writeln(e.StackTrace);
    end;}
    CurrentGradientBrush.LinearColors[0] := CurrentSolidBrush.Color;
  end;
  UnLockGraphics;  
end;

function BrushColor: Color;
begin
  if Brush.NETBrush = CurrentHatchBrush then
    Result := CurrentHatchBrush.ForegroundColor
  else Result := CurrentSolidBrush.Color;
end;

procedure SetBrushStyle(bs: BrushStyleType);
begin
  lock f do
    case bs of
      bsSolid:	Brush.NETBrush := CurrentSolidBrush;
      bsClear:	Brush.NETBrush := nil;
      bsHatch:	Brush.NETBrush := CurrentHatchBrush;
      bsGradient:	Brush.NETBrush := CurrentGradientBrush;
    end;
end;

function BrushStyle: BrushStyleType;
begin
  Result := bsNone; // Если кисть устанавливалась явным присваиванием NETBrush
  if Brush.NETBrush = CurrentSolidBrush then
    Result := bsSolid
  else if Brush.NETBrush = nil then
    Result := bsClear
  else if Brush.NETBrush = CurrentHatchBrush then
    Result := bsHatch
  else if Brush.NETBrush = CurrentGradientBrush then
    Result := bsGradient;
end;

procedure SetBrushHatch(bh: HatchStyle);
begin
  lock f do
  begin
    var flag := CurrentHatchBrush = Brush.NETBrush;
    CurrentHatchBrush := new HatchBrush(HatchStyle(bh),CurrentHatchBrush.ForegroundColor,CurrentHatchBrush.BackgroundColor);
    if flag then
      Brush.NETBrush := CurrentHatchBrush;
  end;    
end;

function BrushHatch: HatchStyle;
begin
  Result := CurrentHatchBrush.HatchStyle;
end;

procedure SetHatchBrushBackgroundColor(c: Color);
var flag: boolean;
begin
  lock f do
  begin
    flag := CurrentHatchBrush = Brush.NETBrush;
    CurrentHatchBrush := new HatchBrush(CurrentHatchBrush.HatchStyle,CurrentHatchBrush.ForegroundColor,c);
    if flag then
      Brush.NETBrush := CurrentHatchBrush;
  end;    
end;

function HatchBrushBackgroundColor: Color;
begin
  Result := CurrentHatchBrush.BackgroundColor;
end;

procedure SetGradientBrushSecondColor(c: Color);
begin
  lock f do
    CurrentGradientBrush.LinearColors[1] := c;
end;

function GradientBrushSecondColor: Color;
begin
  Result := CurrentGradientBrush.LinearColors[1];
end;

// Fonts
procedure SetFontSize(size: integer);
begin
  Font.NETFont := new System.Drawing.Font(Font.NETFont.Name,Convert.ToSingle(size),Font.NETFont.Style);
end;

function FontSize: integer;
begin
  Result := round(Font.NETFont.SizeInPoints);
end;

procedure SetFontName(name: string);
begin
  lock f do
    Font.NETFont := new System.Drawing.Font(name,Font.NETFont.SizeInPoints,Font.NETFont.Style);
end;

function FontName: string;
begin
  Result := Font.NETFont.Name;
end;

procedure SetFontColor(c: Color);
begin
  lock f do
    CurrentTextBrush.Color := c;
end;

function FontColor: Color;
begin
  Result := CurrentTextBrush.Color;
end;

procedure SetFontStyle(fs: FontStyleType);
begin
  lock f do
    Font.NETFont := new System.Drawing.Font(Font.NETFont.name,Font.NETFont.SizeInPoints,System.Drawing.FontStyle(integer(fs)));
end;

function FontStyle: FontStyleType;
begin
  Result := FontStyleType(integer(Font.NETFont.Style));
end;

function TextWidth(s: string): integer;
begin
// добавил 1. Без нее рисует, занимая 1 лишний пиксел
  Result := round(gr.MeasureString(s,Font.NETFont,0,new StringFormat(StringFormat.GenericTypographic)).Width) + 1;
end;

function TextHeight(s: string): integer;
begin
  Result := round(gr.MeasureString(s,Font.NETFont).Height);
end;

// Window
procedure ClearWindow;
begin
  ClearWindow(clWhite);
end;

procedure ClearWindow(c: Color);
var i: Color;
begin
  Monitor.Enter(f);
  i := BrushColor;
  SetBrushColor(c);
  var m := gr.Transform;
  gr.ResetTransform;
  gbmp.ResetTransform;
  FillRect(0,0,GraphABCControl.Width,GraphABCControl.Height);
  gr.Transform := m;
  gbmp.Transform := m;
  SetBrushColor(i);
  Monitor.Exit(f);
end;

function WindowLeft: integer;
begin
  Result := MainForm.Left;
end;

function WindowTop: integer;
begin
  Result := MainForm.Top;
end;

function WindowCenter: Point;
begin
  Result := new Point(WindowWidth div 2,WindowHeight div 2);
end;

function WindowIsFixedSize: boolean;
begin
  Result := (MainForm.FormBorderStyle = FormBorderStyle.FixedSingle) and (MainForm.MaximizeBox = False);
end;

function WindowWidth: integer;
begin
  Result := MainForm.ClientSize.Width;
end;

function WindowHeight: integer;
begin
  Result := MainForm.ClientSize.Height;
end;

procedure SetWindowWidth(w: integer);
begin
  SetWindowSize(w,MainForm.ClientSize.Height);
end;

procedure SetWindowHeight(h: integer);
begin
  SetWindowSize(MainForm.ClientSize.Width,h);
end;  

procedure SetWindowLeft(l: integer);
begin
  SetWindowPos(l,MainForm.Top);
end;

procedure SetWindowTop(t: integer);
begin
  SetWindowPos(MainForm.Left,t);
end;

procedure SetMaximizeBoxInternal(b: boolean);
begin
  MainForm.MaximizeBox := b;
end;

procedure SetBorderStyleInternal(st : FormBorderStyle);
begin
  MainForm.FormBorderStyle := st;
end;

procedure SetWindowIsFixedSize(b: boolean);
var p : Proc1Boolean;
    q : Proc1BorderStyle;
begin
  p := SetMaximizeBoxInternal;
  q := SetBorderStyleInternal;
  if b then
    MainForm.Invoke(q,FormBorderStyle.FixedSingle)
  else MainForm.Invoke(q,FormBorderStyle.Sizable);
  MainForm.Invoke(p,not b);
end;

procedure ChangeFormPos(l,t: integer); // вспомогательная
begin
  MainForm.Left := l;
  MainForm.Top := t;
end;

procedure SetWindowPos(l,t: integer);
var p: Proc2Integer;
begin
  p := ChangeFormPos;
  f.Invoke(p,l,t);
end;

procedure ChangeFormClientSize(w,h: integer); // вспомогательная
begin
  MainForm.ClientSize := new System.Drawing.Size(w,h);
  ResizeHelper;
end;

procedure SetWindowSize(w,h: integer);
var p: Proc2Integer;
begin
  p := ChangeFormClientSize;
  f.Invoke(p,w,h);
  //ResizeHelper;
end;

function GraphBoxWidth: integer;
begin
  Result := f.Width;
end;

function GraphBoxHeight: integer;
begin
  Result := f.Height;
end;

function GraphBoxLeft: integer;
begin
  Result := f.Left;
end;

function GraphBoxTop: integer;
begin
  Result := f.Top;
end;

function WindowCaption: string;
begin
  Result := MainForm.Text;
end;

function WindowTitle: string;
begin
  Result := MainForm.Text;
end;

procedure InitWindow(Left,Top,Width,Height: integer; BackColor: Color);
begin
  SetWindowSize(Width, Height);
  SetWindowPos(Left, Top);
  SetBrushColor(BackColor);
  FillRectangle(0, 0, Width, Height);
end;

procedure ChangeFormTitle(s: string);
begin
  MainForm.Text := s;
end;

procedure SetWindowTitle(s: string);
var p: Proc1String;
begin
  p := ChangeFormTitle;
  f.Invoke(p,s);
end;

procedure SetWindowCaption(s: string);
begin
  SetWindowTitle(s);
end;

procedure SaveWindow(fname: string);
begin
  var tempbmp := GetView(bmp,new System.Drawing.Rectangle(0,0,Window.Width,Window.Height));
  tempbmp.Save(fname);
  tempbmp.Dispose;
end;

procedure LoadWindow(fname: string);
begin
  var b: Bitmap := new Bitmap(fname);
  SetWindowSize(b.Width,b.Height);
  Monitor.Enter(f);
  gr.DrawImage(b,0,0);  
  gbmp.DrawImage(b,0,0);  
  Monitor.Exit(f);
end;

procedure FillWindow(fname: string);
begin
  Monitor.Enter(f);
  var b: System.Drawing.Brush := Brush.NETBrush;
  Brush.NETBrush := new TextureBrush(Bitmap.FromFile(fname));
  FillRect(0,0,GraphABCControl.Width,GraphABCControl.Height);
  Brush.NETBrush := b;
  Monitor.Exit(f);
end;

procedure CloseWindow;
begin
//  MainForm.Close;
  Halt;
end;

function ScreenWidth: integer;
begin
  Result := Screen.PrimaryScreen.Bounds.Width;
end;

function ScreenHeight: integer;
begin
  Result := Screen.PrimaryScreen.Bounds.Height;
end;

procedure CenterWindow;
begin
  SetWindowPos((ScreenWidth - MainForm.Width) div 2, (ScreenHeight - MainForm.Height) div 2);
end;

procedure MaximizeWindow;
begin
  MainForm.WindowState := FormWindowState.Maximized; 
end;

procedure MinimizeWindow;
begin
  MainForm.WindowState := FormWindowState.Minimized;
end;

procedure NormalizeWindow;
begin
  MainForm.WindowState := FormWindowState.Normal
end;

// BufferedDraw
procedure Redraw;
var tempbmp: Bitmap;
begin
  //TODO Без этого падает если свернуто
  if MainForm.WindowState=FormWindowState.Minimized then 
    exit;
  tempbmp := GetView(bmp,new System.Drawing.Rectangle(0,0,WindowWidth,WindowHeight));
  Monitor.Enter(f);
  if gr<>nil then 
  begin
    var m := gr.Transform;
    gr.ResetTransform;
    gr.DrawImage(tempbmp,0,0);
    gr.Transform := m;
  end;  
  Monitor.Exit(f);
end;
procedure FullRedraw;
begin
  Monitor.Enter(f);
  if gr<>nil then 
    gr.DrawImage(bmp,0,0);
  Monitor.Exit(f);
end;

procedure LockDrawing;
begin
  NotLockDrawing := False;
end;

procedure UnlockDrawing;
begin
  NotLockDrawing := True;
  Redraw;
end;

procedure InitForm;
begin
  f := new ABCControl(defaultWindowWidth,defaultWindowHeight);
  _MainForm := new Form;
  _MainForm.Text := 'GraphABC.NET';
  _MainForm.ClientSize := new Size(defaultWindowWidth,defaultWindowHeight);
//  _MainForm.BackColor := Color.White;
  _MainForm.Controls.Add(f);
  _MainForm.TopMost := True;
  _MainForm.StartPosition := FormStartPosition.CenterScreen;
  _MainForm.FormClosing += f.OnClosing;
end;  

procedure InitForm0;
begin
  InitForm;
  StartIsComplete := True;
  Application.Run(MainForm);
end;

function RobotUnitUsed: boolean;
var t: &Type;
begin
  t := System.Reflection.Assembly.GetExecutingAssembly.GetType('Robot.Robot');
  if t=nil then
    result := false
  else
    result := t.GetField('__IS_ROBOT_UNIT') <> nil;
end;

procedure HideForm;
begin
  _MainForm.Hide;
end;

procedure CreatePicture(var p: Picture; w,h: integer);
begin
  p := new Picture(w,h);
end;

function Window: GraphABCWindow;
begin
  Result := _Window;
end;

function MainForm: Form;
begin
  Result := _MainForm;
end;

function GraphABCControl: ABCControl;
begin
  Result := _GraphABCControl;
end;

function Pen: GraphABCPen;
begin
  Result := _Pen;
end;

function Brush: GraphABCBrush;
begin
  Result := _Brush;
end;

function Font: GraphABCFont;
begin
  Result := _Font;
end;

function Coordinate: GraphABCCoordinate;
begin
  Result := _Coordinate;
end;

procedure GraphABCWindow.SetLeft(l: integer);
begin
  SetWindowLeft(l);
end;

function GraphABCWindow.GetLeft: integer;
begin
  Result := WindowLeft;
end;

procedure GraphABCWindow.SetTop(t: integer);
begin
  SetWindowTop(t);
end;

function GraphABCWindow.GetTop: integer;
begin
  Result := WindowTop;
end;

procedure GraphABCWindow.SetWidth(w: integer);
begin
  SetWindowWidth(w);
end;

function GraphABCWindow.GetWidth: integer;
begin
  Result := WindowWidth;
end;

procedure GraphABCWindow.SetHeight(h: integer);
begin
  SetWindowHeight(h);
end;

function GraphABCWindow.GetHeight: integer;
begin
  Result := WindowHeight;
end;

procedure GraphABCWindow.SetCaption(c: string);
begin
  SetWindowCaption(c);
end;

function GraphABCWindow.GetCaption: string;
begin
  Result := WindowCaption;
end;

procedure GraphABCWindow.SetIsFixedSize(b: boolean);
begin
  SetWindowIsFixedSize(b);
end;

function GraphABCWindow.GetIsFixedSize: boolean;
begin
  Result := WindowIsFixedSize;
end;

procedure GraphABCWindow.Clear;
begin
  ClearWindow;
end;

procedure GraphABCWindow.Clear(c: Color);
begin
  ClearWindow(c);
end;

procedure GraphABCWindow.SetSize(w,h: integer);
begin
  SetWindowSize(w,h)
end;

procedure GraphABCWindow.SetPos(l,t: integer);
begin
  SetWindowPos(l,t)
end;

procedure GraphABCWindow.Init(Left,Top,Width,Height: integer; BackColor: Color);
begin
  InitWindow(Left,Top,Width,Height,BackColor);
end;

procedure GraphABCWindow.Save(fname: string);
begin
  SaveWindow(fname);
end;

procedure GraphABCWindow.Load(fname: string);
begin
  LoadWindow(fname);
end;

procedure GraphABCWindow.Fill(fname: string);
begin
  FillWindow(fname);
end;

procedure GraphABCWindow.Close;
begin
  CloseWindow
end;

procedure GraphABCWindow.Minimize;
begin
  MinimizeWindow
end;

procedure GraphABCWindow.Maximize;
begin
  MaximizeWindow;
end;

procedure GraphABCWindow.Normalize;
begin
  NormalizeWindow;
end;

procedure GraphABCWindow.CenterOnScreen;
begin
  CenterWindow;
end;

function GraphABCWindow.Center: Point;
begin
  Result := WindowCenter;
end;

var firstcall := True;

procedure InitGraphABC;
begin
  if not firstcall then exit;
  firstcall := False;
  clMoneyGreen := RGB(192,220,192);
  StartIsComplete := False;
  MainFormThread := new System.Threading.Thread(InitForm0);
  MainFormThread.Start;
  while not StartIsComplete do
    Sleep(30);
  Sleep(30);
  SetSmoothingOn;
  _GraphABCControl := f;
end;

initialization
  InitGraphABC;
finalization
//  Application.Run(MainWindow);
end.