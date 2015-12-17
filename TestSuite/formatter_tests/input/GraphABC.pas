// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

///������ ������������� ���������, ����, ���������, ������� � ������ ��� ��������� � ����������� ����
unit GraphABC;

//ne udaljat, IB 7.10.08 
//� ������������ 2015.01 (mabr) 
{$apptype windows} 
{$reference 'System.Windows.Forms.dll'}
{$reference 'System.Drawing.dll'}
{$gendoc true}

interface

uses 
  System,
  System.Drawing,
  System.Windows.Forms, 
  System.Drawing.Drawing2D;

type 
/// ��� �����
  Color = System.Drawing.Color;
/// ��� ����� ��������� �����
  HatchStyle = System.Drawing.Drawing2D.HatchStyle;
/// ��� ����� ��������� ����
  DashStyle = System.Drawing.Drawing2D.DashStyle;
/// ��� ���������� GraphABC  
  GraphABCException = class(Exception) end;
/// ��� �����
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

/// ����������� ������ � ������������ (x,y) ������ c
procedure SetPixel(x,y: integer; c: Color);
/// ����������� ������ � ������������ (x,y) ������ c
procedure PutPixel(x,y: integer; c: Color);
/// ���������� ���� ������� � ������������ (x,y)
function GetPixel(x,y: integer): Color;

/// ������������� ������� ������� ��������� � ����� (x,y)
procedure MoveTo(x,y: integer);
/// ������ ������� �� ������� ������� �� ����� (x,y). ������� ������� ����������� � ����� (x,y)
procedure LineTo(x,y: integer);
/// ������ ������� �� ������� ������� �� ����� (x,y) ������ c. ������� ������� ����������� � ����� (x,y)
procedure LineTo(x,y: integer; c: Color);

/// ������ ������� �� ����� (x1,y1) �� ����� (x2,y2)
procedure Line(x1,y1,x2,y2: integer);
/// ������ ������� �� ����� (x1,y1) �� ����� (x2,y2) ������ c
procedure Line(x1,y1,x2,y2: integer; c: Color);

/// ��������� ������������ ���������� � ������� (x,y) � �������� r
procedure FillCircle(x,y,r: integer);
/// ������ ���������� � ������� (x,y) � �������� r
procedure DrawCircle(x,y,r: integer);
/// ��������� ������������ �������, ������������� ���������������, �������� ������������ ��������������� ������ (x1,y1) � (x2,y2)
procedure FillEllipse(x1,y1,x2,y2: integer);
/// ������ ������� �������, ������������� ���������������, �������� ������������ ��������������� ������ (x1,y1) � (x2,y2)
procedure DrawEllipse(x1,y1,x2,y2: integer);
/// ��������� ������������ ��������������, ��������� ������������ ��������������� ������ (x1,y1) � (x2,y2)
procedure FillRectangle(x1,y1,x2,y2: integer);
/// ��������� ������������ ��������������, ��������� ������������ ��������������� ������ (x1,y1) � (x2,y2)
procedure FillRect(x1,y1,x2,y2: integer);
/// ������ ������� ��������������, ��������� ������������ ��������������� ������ (x1,y1) � (x2,y2)
procedure DrawRectangle(x1,y1,x2,y2: integer);
/// ��������� ������������ �������������� �� ������������ ������; (x1,y1) � (x2,y2) ������ ���� ��������������� ������, � w � h � ������ � ������ �������, ������������� ��� ���������� �����
procedure FillRoundRect(x1,y1,x2,y2,w,h: integer);
/// ������ ������� �������������� �� ������������ ������; (x1,y1) � (x2,y2) ������ ���� ��������������� ������, � w � h � ������ � ������ �������, ������������� ��� ���������� �����
procedure DrawRoundRect(x1,y1,x2,y2,w,h: integer);

/// ������ ����������� ���������� � ������� (x,y) � �������� r
procedure Circle(x,y,r: integer);
/// ������ ����������� ������, ������������ ���������������, �������� ������������ ��������������� ������ (x1,y1) � (x2,y2)
procedure Ellipse(x1,y1,x2,y2: integer);
/// ������ ����������� �������������, �������� ������������ ��������������� ������ (x1,y1) � (x2,y2)
procedure Rectangle(x1,y1,x2,y2: integer);
/// ������ ����������� ������������� �� ������������ ������; (x1,y1) � (x2,y2) ������ ���� ��������������� ������, � w � h � ������ � ������ �������, ������������� ��� ���������� �����
procedure RoundRect(x1,y1,x2,y2,w,h: integer);

/// ������ ���� ���������� � ������� � ����� (x,y) � �������� r, ����������� ����� ����� ������, ����������� ���� a1 � a2 � ���� OX (a1 � a2 � ������������, �������� � �������� � ������������� ������ ������� �������)
procedure Arc(x,y,r,a1,a2: integer);
/// ��������� ������������ ������� ����������, ������������� ����� � ������� � ����� (x,y) � �������� r, ����������� ����� ����� ������, ����������� ���� a1 � a2 � ���� OX (a1 � a2 � ������������, �������� � �������� � ������������� ������ ������� �������)
procedure FillPie(x,y,r,a1,a2: integer);
/// ������ ������ ����������, ������������ ����� � ������� � ����� (x,y) � �������� r, ����������� ����� ����� ������, ����������� ���� a1 � a2 � ���� OX (a1 � a2 � ������������, �������� � �������� � ������������� ������ ������� �������)
procedure DrawPie(x,y,r,a1,a2: integer);
/// ������ ����������� ������ ����������, ������������ ����� � ������� � ����� (x,y) � �������� r, ����������� ����� ����� ������, ����������� ���� a1 � a2 � ���� OX (a1 � a2 � ������������, �������� � �������� � ������������� ������ ������� �������)
procedure Pie(x,y,r,a1,a2: integer);

/// ���������� ����� �� ��������� � ������������ (x,y)
function Pnt(x,y: integer): Point;

/// ������ ��������� ������� �� ������, ���������� ������� ������ � ������� points
procedure DrawPolygon(points: array of Point);
/// ��������� �������������, ���������� ������ �������� ������ � ������� points
procedure FillPolygon(points: array of Point);
/// ������ ����������� �������������, ���������� ������ �������� ������ � ������� points
procedure Polygon(points: array of Point);
/// ������ ������� �� ������, ���������� ������� ������ � ������� points
procedure Polyline(points: array of Point);
/// ������ ������ �� ������, ���������� ������� ������ � ������� points
procedure Curve(points: array of Point);
/// ������ ��������� ������ �� ������, ���������� ������� ������ � ������� points
procedure DrawClosedCurve(points: array of Point);
/// ��������� ��������� ������ �� ������, ���������� ������� ������ � ������� points
procedure FillClosedCurve(points: array of Point);
/// ������ ����������� ��������� ������ �� ������, ���������� ������� ������ � ������� points
procedure ClosedCurve(points: array of Point);

/// ������� ������ s � ������������� � ������������ ������ �������� ���� (x,y)
procedure TextOut(x,y: integer; s: string); 
/// ������� ����� n � ������������� � ������������ ������ �������� ���� (x,y)
procedure TextOut(x,y: integer; n: integer); 
/// ������� ������������ r � ������������� � ������������ ������ �������� ���� (x,y)
procedure TextOut(x,y: integer; r: real); 
/// ������� ������ s, ���������������� � �������������� � ������������ (x,y,x1,y1)
procedure DrawTextCentered(x,y,x1,y1: integer; s: string); 
/// ������� ����� �������� n, ���������������� � �������������� � ������������ (x,y,x1,y1)
procedure DrawTextCentered(x,y,x1,y1: integer; n: integer); 
/// ������� ������������ �������� r, ���������������� � �������������� � ������������ (x,y,x1,y1)
procedure DrawTextCentered(x,y,x1,y1: integer; r: real); 
/// �������� ������� ������ ����� ������ c, ������� � ����� (x,y).
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
////                �����
//--------------------------------------------
/// ���������� ����, ������� �������� ������� (r), ������� (g) � ����� (b) ������������ (r,g � b - � ��������� �� 0 �� 255)
function RGB(r,g,b: byte): Color;
/// ���������� ����, ������� �������� ������� (r), ������� (g) � ����� (b) ������������ � ������������ (a) (a,r,g,b - � ��������� �� 0 �� 255)
function ARGB(a,r,g,b: byte): Color;

/// ���������� ������� ���� � �������������� r (r - � ��������� �� 0 �� 255)
function RedColor(r: byte): Color;
/// ���������� ������� ���� � �������������� g (g - � ��������� �� 0 �� 255)
function GreenColor(g: byte): Color;
/// ���������� ����� ���� � �������������� b (b - � ��������� �� 0 �� 255)
function BlueColor(b: byte): Color;
/// ���������� ��������� ����
function clRandom: Color;

/// ���������� ������� ������������ �����
function GetRed(c: Color): integer;
/// ���������� ������� ������������ �����
function GetGreen(c: Color): integer;
/// ���������� ����� ������������ �����
function GetBlue(c: Color): integer;
/// ���������� ������������ ������������ �����
function GetAlpha(c: Color): integer;

//--------------------------------------------
////                �����
//--------------------------------------------
/// ������������� ���� �������� ����
procedure SetPenColor(c: Color); 
/// ���������� ���� �������� ����
function PenColor: Color;
/// ������������� ������ �������� ����
procedure SetPenWidth(Width: integer);
/// ���������� ������ �������� ����
function PenWidth: integer;
/// ������������� ����� �������� ����
procedure SetPenStyle(style: DashStyle);
/// ���������� ����� �������� ����
function PenStyle: DashStyle;
/// ������������� ����� �������� ����
procedure SetPenMode(m: integer);
/// ���������� ����� �������� ����
function PenMode: integer;
//2015.01>
/// ������������� ��� �������� ����� ������������ ������ �����
procedure SetPenRoundCap(isRoundCap: boolean);
/// ���������� tru�, ���� ���������� ����� ������������ ������ �����
function PenRoundCap: boolean;
//2015.01<    
/// ���������� x-���������� ������� ������� ���������
function PenX: integer;
/// ���������� y-���������� ������� ������� ���������
function PenY: integer;

//--------------------------------------------
////                �����
//--------------------------------------------
/// ������������� ���� ������� �����
procedure SetBrushColor(c: Color);
/// ���������� ���� ������� �����
function BrushColor: Color;
/// ������������� ���� ������� �����
procedure SetBrushStyle(bs: BrushStyleType);
/// ���������� ���� ������� �����
function BrushStyle: BrushStyleType;
/// ������������� ��������� ������� �����
procedure SetBrushHatch(bh: HatchStyle);
/// ���������� ��������� ������� �����
function BrushHatch: HatchStyle;
/// ������������� ���� ������� ����� ������� ��������� �����
procedure SetHatchBrushBackgroundColor(c: Color);
/// ���������� ���� ������� ����� ������� ��������� �����
function HatchBrushBackgroundColor: Color;
/// ������������� ������ ���� ������� ����������� �����
procedure SetGradientBrushSecondColor(c: Color);
/// ���������� ������ ���� ������� ����������� �����
function GradientBrushSecondColor: Color;

//--------------------------------------------
////                ������
//--------------------------------------------
/// ������������� ������ �������� ������ � �������
procedure SetFontSize(size: integer);
/// ���������� ������ �������� ������ � �������
function FontSize: integer;
/// ������������� ��� �������� ������
procedure SetFontName(name: string);
/// ���������� ��� �������� ������
function FontName: string;
/// ������������� ���� �������� ������
procedure SetFontColor(c: Color);
/// ���������� ���� �������� ������
function FontColor: Color;
/// ������������� ����� �������� ������
procedure SetFontStyle(fs: FontStyleType);
/// ���������� ����� �������� ������
function FontStyle: FontStyleType;
/// ���������� ������ ������ s � �������� ��� ������� ���������� ������
function TextWidth(s: string): integer;
/// ���������� ������ ������ s � �������� ��� ������� ���������� ������
function TextHeight(s: string): integer;

//--------------------------------------------
////                ����������� ����
//--------------------------------------------
/// ������� ����������� ���� ����� ������
procedure ClearWindow;
/// ������� ����������� ���� ������ c
procedure ClearWindow(c: Color);

/// ���������� ������ ���������� ����� ������������ ���� � ��������
function WindowWidth: integer;
/// ���������� ������ ���������� ����� ������������ ���� � ��������
function WindowHeight: integer;
/// ���������� ������ ������������ ���� �� ������ ���� ������ � ��������
function WindowLeft: integer;
/// ���������� ������ ������������ ���� �� �������� ���� ������ � ��������
function WindowTop: integer;
/// ���������� ����� ������������ ���� 
function WindowCenter: Point;
/// ���������� True, ���� ����������� ���� ����� ������������� ������, � False � ��������� ������ 
function WindowIsFixedSize: boolean;

/// ������������� ������ ���������� ����� ������������ ���� � ��������
procedure SetWindowWidth(w: integer);
/// ������������� ������ ���������� ����� ������������ ���� � ��������
procedure SetWindowHeight(h: integer);
/// ������������� ������ ������������ ���� �� ������ ���� ������ � ��������
procedure SetWindowLeft(l: integer);
/// ������������� ������ ������������ ���� �� �������� ���� ������ � ��������
procedure SetWindowTop(t: integer);
/// �������������, ����� �� ����������� ���� ������������� ������
procedure SetWindowIsFixedSize(b: boolean);

/// ������������� ������� ���������� ����� ������������ ���� � ��������
procedure SetWindowSize(w,h: integer);
/// ������������� ������ ������������ ���� �� ������ �������� ���� ������ � ��������
procedure SetWindowPos(l,t: integer);

/// ���������� ������ ������������ ���������� � �������� (�� ��������� ��������� � WindowWidth)
function GraphBoxWidth: integer;
/// ���������� ������ ������������ ���������� � �������� (�� ��������� ��������� � WindowHeight)
function GraphBoxHeight: integer;
/// ���������� ������ ������������ ���������� �� ������ ���� ���� � ��������
function GraphBoxLeft: integer;
/// ���������� ������ ������������ ���������� �� �������� ���� ���� � ��������
function GraphBoxTop: integer;

/// ���������� ��������� ������������ ����
function WindowCaption: string;
/// ���������� ��������� ������������ ����
function WindowTitle: string;
/// ������������� ��������� ������������ ����
procedure SetWindowCaption(s: string);
/// ������������� ��������� ������������ ����
procedure SetWindowTitle(s: string);

/// ������������� ������ � ������ ���������� ����� ������������ ���� � ��������
procedure InitWindow(Left,Top,Width,Height: integer; BackColor: Color := clWhite);

/// ��������� ���������� ������������ ���� � ���� � ������ fname
procedure SaveWindow(fname: string);
/// ��������������� ���������� ������������ ���� �� ����� � ������ fname
procedure LoadWindow(fname: string);
/// ��������� ���������� ������������ ���� ������ �� ����� � ������ fname
procedure FillWindow(fname: string);
/// ��������� ����������� ���� � ��������� ����������
procedure CloseWindow;

/// ���������� ������ ������ � ��������
function ScreenWidth: integer;
/// ���������� ������ ������ � ��������
function ScreenHeight: integer;

/// ���������� ����������� ���� �� ������ ������
procedure CenterWindow;
/// ������������� ����������� ����
procedure MaximizeWindow;
/// ����������� ����������� ����
procedure MinimizeWindow;
/// ���������� ����������� ���� � ����������� �������
procedure NormalizeWindow;

//--------------------------------------------
////           ����������� ���������
//--------------------------------------------
/// �������������� ���������� ������������ ����. ���������� � ���� � LockDrawing
procedure Redraw;
///--
procedure FullRedraw;
/// ��������� ��������� �� ����������� ����. ����������� ������������ ���� ����������� � ������� Redraw
procedure LockDrawing;
/// ������� ���������� ��������� �� ����������� ���� � ������������ ��� �����������
procedure UnlockDrawing;

//--------------------------------------------
////              �����������
//--------------------------------------------
/// ������������� ����� �����������
procedure SetSmoothing(sm: boolean);
/// �������� ����� �����������
procedure SetSmoothingOn;
/// ��������� ����� �����������
procedure SetSmoothingOff;
/// ���������� True, ���� ����� ����������� ����������
function SmoothingIsOn: boolean;

//--------------------------------------------
////        ��������������� ������������
//--------------------------------------------
/// ��������� ���������� �������. ���������� ��� �������������
procedure LockGraphics;
/// ������������ ���������� �������. ���������� ��� �������������
procedure UnLockGraphics;

//------------------------------------------------------------
////        ������������ ��� ������ � �������� ���������
//------------------------------------------------------------
/// ������������� ������ ��������� � ����� (x0,y0)
procedure SetCoordinateOrigin(x0,y0: integer);
/// ������������� ������� ������� ��������� 
procedure SetCoordinateScale(sx,sy: real);
/// ������������� ������� ������� ��������� 
procedure SetCoordinateAngle(a: real);

//----------------------------------------
////        ��������� ������������ 
//----------------------------------------
/// ���������� �������������, �������� ������������ ��������������� ������
function Rect(x1,y1,x2,y2: integer): System.Drawing.Rectangle;

/// ���������� ������������� ���������� ����� �������� ����
function ClientRectangle: System.Drawing.Rectangle;

//------------------------------------------
////        ��������� �������� ������� 
//------------------------------------------
/// ������ ������ ������� f �� ������� [a,b] � ��������������, ���������� ������������ x1,y1,x2,y2, 
procedure Draw(f: Func<real,real>; a,b: real; x1,y1,x2,y2: integer);
/// ������ ������ ������� f �� ������� [a,b] � �������������� r 
procedure Draw(f: Func<real,real>; a,b: real; r: System.Drawing.Rectangle);
/// ������ ������ ������� f �� ������� [-5,5] � �������������� r 
procedure Draw(f: Func<real,real>; r: System.Drawing.Rectangle);
/// ������ ������ ������� f �� ������� [a,b] �� ������ ����������� ���� 
procedure Draw(f: Func<real,real>; a,b: real);
/// ������ ������ ������� f �� ������� [-5,5] �� ������ ����������� ����  
procedure Draw(f: Func<real,real>);

//------------------------------------------
////        ��������� �����������
//------------------------------------------
/// ������� ��������� ����������� �� ����� � ������ fname � ������� x,y 
procedure Draw(fname: string; x: integer := 0; y: integer := 0);
/// ������� ��������� ����������� �� ����� � ������ fname � ������� x,y, ����������� ��� � ������� w �� h
procedure Draw(fname: string; x,y,w,h: integer);
/// ������� ��������� ����������� �� ����� � ������ fname � ������� x,y, ����������� ��� � ������������� Scale
procedure Draw(fname: string; x,y: integer; Scale: real);


/// �������������� ����������� ����. ������������ ��� ���������� �����
procedure InitGraphABC;

type 
/// ��� ���� GraphABC
  GraphABCPen = class
  private
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
//2015.01>
    procedure SetRoundCap(isRoundCap: boolean);
    function GetRoundCap: boolean;
//2015.01<    
  public  
    /// ������� ���� .NET
    property NETPen: System.Drawing.Pen read _NETPen write SetNETPen;
    /// ���� ����
    property Color: GraphABC.Color read GetColor write SetColor;
    /// ������ ����
    property Width: integer read GetWidth write SetWidth;
    /// ����� ����
    property Style: DashStyle read GetStyle write SetStyle;
    /// ����� ����
    property Mode: integer read GetMode write SetMode;
    /// X-���������� ������� ������� ����
    property X: integer read GetX;
    /// Y-���������� ������� ������� ����
    property Y: integer read GetY;
//2015.01>
    /// ����� ������������ ������ �����
    property RoundCap: boolean read GetRoundCap write SetRoundCap;
//2015.01<    
  end;

/// ��� ����� GraphABC
  GraphABCBrush = class
  private
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
/// ������� ����� .NET
    property NETBrush: System.Drawing.Brush read _NETBrush write SetNETBrush;
/// ���� �����
    property Color: GraphABC.Color read GetColor write SetColor;
/// ����� �����
    property Style: BrushStyleType read GetStyle write SetStyle;
/// ��������� �����
    property Hatch: HatchStyle read GetHatch write SetHatch;
/// ���� ������� ����� ��������� �����
    property HatchBackgroundColor: GraphABC.Color read GetHatchBackgroundColor write SetHatchBackgroundColor;
/// ������ ���� ����������� �����
    property GradientSecondColor: GraphABC.Color read GetGradientSecondColor write SetGradientSecondColor;
  end;
  
/// ��� ������ GraphABC
  GraphABCFont = class
  private
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
/// ������� ����� .NET
    property NETFont: System.Drawing.Font read _NETFont write SetNETFont;
    /// ���� ������
    property Color: GraphABC.Color read GetColor write SetColor;
    /// ����� ������
    property Style: FontStyleType read GetStyle write SetStyle;
    /// ������ ������ � �������
    property Size: integer read GetSize write SetSize;
    /// ������������ ������
    property Name: string read GetName write SetName;
  end;
  
  GraphABCCoordinate = class
  private
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
    /// ������������� ��������� ������� ���������
    procedure SetTransform(x0,y0,angle,sx,sy: real);
    /// ������������� ������ ������� ���������
    procedure SetOrigin(x0,y0: integer);
    /// ������������� ������� ������� ���������
    procedure SetScale(sx,sy: real);
    /// ������������� ������� ������� ���������
    procedure SetScale(scale: real);
    /// ������������� ������ ������� ��������� (��� OY ���������� �����, ��� OX - ������)
    procedure SetMathematic;
    /// ������������� ����� ������� ��������� (��� OY ���������� ����, ��� OX - ������)
    procedure SetStandard;
    /// X-���������� ������ ��������� ������������ ������ �������� ���� ����
    property OriginX: integer read GetOriginX write SetOriginX;
    /// Y-���������� ������ ��������� ������������ ������ �������� ���� ����
    property OriginY: integer read GetOriginY write SetOriginY;
    /// ���������� ������ ��������� ������������ ������ �������� ���� ����
    property Origin: Point read GetOrigin write SetOrigin;
    /// ���� �������� ������� ���������
    property Angle: real read GetAngle write SetAngle;
    /// ������� ������� ��������� �� ��� X
    property ScaleX: real read GetScaleX write SetScaleX;
    /// ������� ������� ��������� �� ��� Y
    property ScaleY: real read GetScaleY write SetScaleY;
    /// ������� ������� ��������� �� ����� ����
    property Scale: real write SetScale;
    /// ������� 3x3 �������������� ���������
    property Matrix: System.Drawing.Drawing2D.Matrix read GetMatrix;
  end;
  
  GraphABCWindow = class
  private
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
/// ������ ������������ ���� �� ������ ���� ������ � ��������
    property Left: integer read GetLeft write SetLeft; 
/// ������ ������������ ���� �� �������� ���� ������ � ��������
    property Top: integer read GetTop write SetTop; 
/// ������ ���������� ����� ������������ ���� � ��������
    property Width: integer read GetWidth write SetWidth; 
/// ������ ���������� ����� ������������ ���� � ��������
    property Height: integer read GetHeight write SetHeight;
/// ��������� ������������ ����
    property Caption: string read GetCaption write SetCaption;
/// ��������� ������������ ����
    property Title: string read GetCaption write SetCaption;
/// ����� �� ����������� ���� ������������� ������
    property IsFixedSize: boolean read GetIsFixedSize write SetIsFixedSize;
/// ������� ����������� ���� ����� ������
    procedure Clear;
/// ������� ����������� ���� ������ c
    procedure Clear(c: Color);
/// ������������� ������� ���������� ����� ������������ ���� � ��������
    procedure SetSize(w,h: integer);
/// ������������� ������ ������������ ���� �� ������ �������� ���� ������ � ��������
    procedure SetPos(l,t: integer);
/// ������������� ���������, ������� � ���� ������������ ����
    procedure Init(Left,Top,Width,Height: integer; BackColor: Color := clWhite);
/// ��������� ���������� ������������ ���� � ���� � ������ fname
    procedure Save(fname: string); 
/// ��������������� ���������� ������������ ���� �� ����� � ������ fname
    procedure Load(fname: string); 
/// ��������� ���������� ������������ ���� ������ �� ����� � ������ fname
    procedure Fill(fname: string); 
/// ��������� ����������� ���� � ��������� ����������
    procedure Close;
/// ����������� ����������� ����
    procedure Minimize;
/// ������������� ����������� ����
    procedure Maximize;
/// ���������� ����������� ���� � ����������� �������
    procedure Normalize;
/// ���������� ����������� ���� �� ������ ������
    procedure CenterOnScreen;
/// ���������� ����� ������������ ����
    function Center: Point;
  end;

/// ��� ������� GraphABC
  Picture = class
  private
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
/// ������� ������� ������� w �� h ��������
    constructor Create(w,h: integer);
/// ������� ������� �� ����� � ������ fname
    constructor Create(fname: string);
/// ������� ������� �� �������������� r ������������ ����
    constructor Create(r: System.Drawing.Rectangle); // Create from screen
/// ��������� ������� �� ����� � ������ fname
    procedure Load(fname: string);
/// ��������� ������� � ���� � ������ fname
    procedure Save(fname: string);
/// ������������� ������ ������� w �� h ��������
    procedure SetSize(w,h: integer);
/// ������ ������� � ��������
    property Width: integer read GetWidth write SetWidth;
/// ������ ������� � ��������
    property Height: integer read GetHeight write SetHeight;
/// ������������ �������; ���������� ���� �������� ��������� TransparentColor
    property Transparent: boolean read istransp write SetTransparent;
/// ���������� ���� �������. ������ ���� ����������� ������������ Transparent := True
    property TransparentColor: GraphABC.Color read GetTransparentColor write SetTransparentColor;
/// ���������� True, ���� ����������� ������� ������� ������������ � ������������ ������� p, � False � ��������� ������. ����� ���� ��������� ����������
    function Intersect(p: Picture): boolean;
/// ������� ������� � ������� (x,y)
    procedure Draw(x: integer := 0; y: integer := 0);
/// ������� ������� � ������� (x,y) �� ����������� ��������� g
    procedure Draw(x,y: integer; g: Graphics);
/// ������� ������� � ������� (x,y), ����������� ��� � ������� (w,h)
    procedure Draw(x,y,w,h: integer);
/// ������� ������� � ������� (x,y), ����������� ��� � ������� (w,h), �� ����������� ��������� g
    procedure Draw(x,y,w,h: integer; g: Graphics);
/// ������� ����� �������, ����������� � �������������� r, � ������� (x,y)
    procedure Draw(x,y: integer; r: System.Drawing.Rectangle); // r - part of Picture
/// ������� ����� �������, ����������� � �������������� r, � ������� (x,y) �� ����������� ��������� g
    procedure Draw(x,y: integer; r: System.Drawing.Rectangle; g: Graphics);
/// ������� ����� �������, ����������� � �������������� r, � ������� (x,y), ����������� ��� � ������� (w,h)
    procedure Draw(x,y,w,h: integer; r: System.Drawing.Rectangle); // r - part of Picture
/// ������� ����� �������, ����������� � �������������� r, � ������� (x,y), ����������� ��� � ������� (w,h), �� ����������� ��������� g
    procedure Draw(x,y,w,h: integer; r: System.Drawing.Rectangle; g: Graphics);
/// ������� �������, ���������� �� ���� rotateAngle, ����������� ��� � ������� (w,h)
    procedure Picture.Draw(x,y: integer; rotateAngle: real; w,h: integer);
/// �������� ������������� src ������� p � ������������� dst �������� �������
    procedure CopyRect(dst: System.Drawing.Rectangle; p: Picture; src: System.Drawing.Rectangle);
/// �������� ������������� src �������� ������ bmp � ������������� dst �������� �������
    procedure CopyRect(dst: System.Drawing.Rectangle; bmp: Bitmap; src: System.Drawing.Rectangle);
/// ��������� ���������� ������� ������������ �������������� ��� ���������
    procedure FlipHorizontal;
/// ��������� ���������� ������� ������������ ������������ ��� ��������� 
    procedure FlipVertical;

/// ����������� ������ (x,y) ������� ������ c 
    procedure SetPixel(x,y: integer; c: Color);
/// ����������� ������ (x,y) ������� ������ c 
    procedure PutPixel(x,y: integer; c: Color);
/// ���������� ���� ������� (x,y) �������
    function GetPixel(x,y: integer): Color;
 
/// ������� �� ������� ������� �� ����� (x1,y1) �� ����� (x2,y2)
    procedure Line(x1,y1,x2,y2: integer);
/// ������� �� ������� ������� �� ����� (x1,y1) �� ����� (x2,y2) ������ c
    procedure Line(x1,y1,x2,y2: integer; c: Color);

/// ��������� �� ������� ������������ ���������� � ������� (x,y) � �������� r
    procedure FillCircle(x,y,r: integer);
/// ������� �� ������� ���������� � ������� (x,y) � �������� r
    procedure DrawCircle(x,y,r: integer);
/// ��������� �� ������� ������������ �������, ������������� ���������������, �������� ������������ ��������������� ������ (x1,y1) � (x2,y2)
    procedure FillEllipse(x1,y1,x2,y2: integer);
/// ������� �� ������� ������� �������, ������������� ���������������, �������� ������������ ��������������� ������ (x1,y1) � (x2,y2)
    procedure DrawEllipse(x1,y1,x2,y2: integer);
/// ��������� �� ������� ������������ ��������������, ��������� ������������ ��������������� ������ (x1,y1) � (x2,y2)
    procedure FillRectangle(x1,y1,x2,y2: integer);
/// ��������� �� ������� ������������ ��������������, ��������� ������������ ��������������� ������ (x1,y1) � (x2,y2)
    procedure FillRect(x1,y1,x2,y2: integer);
/// ������� �� ������� ������� � ��������������, ��������� ������������ ��������������� ������ (x1,y1) � (x2,y2)
    procedure DrawRectangle(x1,y1,x2,y2: integer);

/// ������� �� ������� ����������� ���������� � ������� (x,y) � �������� r
    procedure Circle(x,y,r: integer);
/// ������� �� ������� ����������� ������, ������������ ���������������, �������� ������������ ��������������� ������ (x1,y1) � (x2,y2)
    procedure Ellipse(x1,y1,x2,y2: integer);
/// ������� �� ������� ����������� �������������, �������� ������������ ��������������� ������ (x1,y1) � (x2,y2)
    procedure Rectangle(x1,y1,x2,y2: integer);
/// ������� �� ������� ����������� ������������� �� ������������ ������; (x1,y1) � (x2,y2) ������ ���� ��������������� ������, � w � h � ������ � ������ �������, ������������� ��� ���������� �����
    procedure RoundRect(x1,y1,x2,y2,w,h: integer);
 
/// ������� �� ������� ���� ���������� � ������� � ����� (x,y) � �������� r, ����������� ����� ����� ������, ����������� ���� a1 � a2 � ���� OX (a1 � a2 � ������������, �������� � �������� � ������������� ������ ������� �������)
    procedure Arc(x,y,r,a1,a2: integer);
/// ��������� �� ������� ������������ ������� ����������, ������������� ����� � ������� � ����� (x,y) � �������� r, ����������� ����� ����� ������, ����������� ���� a1 � a2 � ���� OX (a1 � a2 � ������������, �������� � �������� � ������������� ������ ������� �������)
    procedure FillPie(x,y,r,a1,a2: integer);
/// ������� �� ������� ������ ����������, ������������ ����� � ������� � ����� (x,y) � �������� r, ����������� ����� ����� ������, ����������� ���� a1 � a2 � ���� OX (a1 � a2 � ������������, �������� � �������� � ������������� ������ ������� �������)
    procedure DrawPie(x,y,r,a1,a2: integer);
/// ������� �� ������� ����������� ������ ����������, ������������ ����� � ������� � ����� (x,y) � �������� r, ����������� ����� ����� ������, ����������� ���� a1 � a2 � ���� OX (a1 � a2 � ������������, �������� � �������� � ������������� ������ ������� �������)
    procedure Pie(x,y,r,a1,a2: integer);

/// ������� �� ������� ��������� ������� �� ������, ���������� ������� ������ � ������� points
    procedure DrawPolygon(points: array of Point);
/// ��������� �� ������� �������������, ���������� ������ �������� ������ � ������� points
    procedure FillPolygon(points: array of Point);
/// ������� �� ������� ����������� �������������, ���������� ������ �������� ������ � ������� points
    procedure Polygon(points: array of Point);
/// ������� �� ������� ������� �� ������, ���������� ������� ������ � ������� points
    procedure Polyline(points: array of Point);
/// ������� �� ������� ������ �� ������, ���������� ������� ������ � ������� points
    procedure Curve(points: array of Point);
/// ������� �� ������� ��������� ������ �� ������, ���������� ������� ������ � ������� points
    procedure DrawClosedCurve(points: array of Point);
/// ��������� �� ������� ��������� ������ �� ������, ���������� ������� ������ � ������� points
    procedure FillClosedCurve(points: array of Point);
/// ������� �� ������� ����������� ��������� ������ �� ������, ���������� ������� ������ � ������� points
    procedure ClosedCurve(points: array of Point);

/// ������� �� ������� ������ s � ������������� � ������������ ������ �������� ���� (x,y)
    procedure TextOut(x,y: integer; s: string); 
/// �������� �� ������� ������� ������ ����� ������ c, ������� � ����� (x,y).
    procedure FloodFill(x,y: integer; c: Color);
    
/// ������� ������� ����� ������
    procedure Clear;
/// ������� ������� ������ c
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
    procedure OnPaintBackground(e: PaintEventArgs); override; begin end; // ���� ��� ��������������!
  public  
    constructor (w,h: integer); 
  end;
  
/// ������� ������� ������� w �� h �������� � ���������� ��� � ���������� p
procedure CreatePicture(var p: Picture; w,h: integer);
/// ���������� ���� ������������ ����������
function Window: GraphABCWindow;
/// ���������� ������� ����� ������������ ����������
function MainForm: Form;
/// ���������� ����������� ���������
function GraphABCControl: ABCControl;
/// ���������� ������� ����
function Pen: GraphABCPen;
/// ���������� ������� �����
function Brush: GraphABCBrush;
/// ���������� ������� �����
function Font: GraphABCFont;
/// ���������� ������� ��������� GraphABC
function Coordinate: GraphABCCoordinate;

function GraphWindowGraphics: Graphics;
function GraphBufferGraphics: Graphics;
function GraphBufferBitmap: Bitmap;

/// ������������� ���������� ����-����� 
procedure SetConsoleIO;
/// ������������� ����-����� ����� ����������� ���� (�� ���������)
procedure SetGraphABCIO;

var 
/// ������� ������� �� ������ ����. (x,y) - ���������� ������� ���� � ������ ����������� �������, mousebutton = 1, ���� ������ ����� ������ ����, � 2, ���� ������ ������ ������ ����
  OnMouseDown: procedure (x,y,mousebutton: integer);
/// ������� ������� ������ ����. (x,y) - ���������� ������� ���� � ������ ����������� �������, mousebutton = 1, ���� ������ ����� ������ ����, � 2, ���� ������ ������ ������ ����
  OnMouseUp: procedure (x,y,mousebutton: integer);
/// ������� ����������� ����. (x,y) - ���������� ������� ���� � ������ ����������� �������, mousebutton = 0, ���� ������ ���� �� ������, 1, ���� ������ ����� ������ ����, � 2, ���� ������ ������ ������ ����
  OnMouseMove: procedure (x,y,mousebutton: integer);
/// ������� ������� �������
  OnKeyDown: procedure (key: integer);
/// ������� ������� �������
  OnKeyUp: procedure (key: integer);
/// ������� ������� ���������� �������
  OnKeyPress: procedure (ch: char);
/// ������� ��������� ������� ������������ ����
  OnResize: procedure;
/// ������� �������� ������������ ����
  OnClose: procedure;

/// ����������� ���������� ����������� ������������ ����. ���� ����� nil, �� ������������ ����������� �����������
  RedrawProc: procedure;
/// ������� �� �������� �� ����������� ������
  DrawInBuffer: boolean;

///--  
procedure __InitModule__;

implementation

uses 
  System.Threading,
  GraphABCHelper;
     
const 
  FILE_NOT_FOUND_MESSAGE = '���� {0} �� ������';
  BUTTON_ENTER_TEXT = '������';
  
type 
  Proc1Integer = procedure(x: integer);
  Proc1String = procedure(s: string);
  Proc2Integer = procedure(x,y: integer);
  Proc1Boolean = procedure(b: boolean);
  Proc1BorderStyle = procedure(st: FormBorderStyle);
  
  IOGraphABCSystem = class(IOStandardSystem)
  public
    constructor Create;
//    procedure write(p: pointer);     override;
    procedure write(obj: object);    override;
    procedure writeln;               override;
    function read_symbol: char;      override;
    function peek: integer;          override;
  end;
  
function SetProcessDPIAware(): boolean; external 'user32.dll'; 
  
var 
  // ������-����� �����
  MainThread: Thread;
  // ������-����� �����
  readbuffer: string;
  _Window: GraphABCWindow := new GraphABCWindow;
  _MainForm: Form; 
  _GraphABCControl: ABCControl;
  _Pen := new GraphABCPen;
  _Brush := new GraphABCBrush;
  _Font := new GraphABCFont;
  _Coordinate := new GraphABCCoordinate;

  __buffer: Bitmap;
  bmp: Bitmap; 
  gr: Graphics;
  gbmp: Graphics;

  f: ABCControl;
  ed: TextBox;
  IOPanel: Panel;
  EnterButton: Button;

  CurrentSolidBrush: SolidBrush;
  CurrentHatchBrush: HatchBrush;
  CurrentGradientBrush: LinearGradientBrush;
  PixelBrush: SolidBrush;
  
  // For MoveTo, LineTo
  x_coord,y_coord: integer;
  NotLockDrawing: boolean;
  
  StartIsComplete: boolean;
  MainFormThread: System.Threading.Thread;
  
  // coords for write 
  writecoords: Point := new Point(1,1);
  // font for write 
//  CurrentWriteFont: System.Drawing.Font;
  // format for TextWidth
  sf: StringFormat;

// ------------ IOGraphABCSystem -----------------
constructor IOGraphABCSystem.Create;
begin
  lock f do
  begin
{    var fnt := GraphABC.Font.NETFont;
    GraphABC.Font.NETFont := CurrentWriteFont;
    GraphABC.Font.NETFont := fnt;}
    readbuffer := '';
  end;  
end;

procedure NextLine;
begin
  writecoords.X := 1;
  writecoords.Y += TextHeight('a')-1;
  if writecoords.Y > Window.Height then
  begin
    writecoords.Y := 1;
  end;
end;

procedure IOGraphABCSystem.write(obj: object);

 function CountSymBeforeDivision(s: string): integer;
 begin
   var tw := TextWidth(s);
   var rem := Window.Width - writecoords.X;
   if rem = 0 then
     Result := 0
   else if tw <= rem then // ������ ������ ���������
     Result := Length(s)
   else   
   begin
     // ns - ��������� ���������� �������� �� ��������
     var ns := Round(Length(s)*rem/TextWidth(s));
     if ns>Length(s) then
       ns := Length(s);
     tw := TextWidth(s.Substring(0,ns));
     if tw > rem then
       while tw > rem do
       begin // �������� ��������� ns, ���� �� ������
         ns -= 1;
         tw := TextWidth(s.Substring(0,ns));
       end
     else if tw = rem then
     begin
       // ������ �� ������
     end
     else 
     begin // tw < rem
       // ����������� ns 
       while (ns<Length(s)) and (tw<rem) do
       begin
         ns += 1;
         tw := TextWidth(s.Substring(0,ns));
       end;
       if tw>rem then 
         ns -= 1;
     end;
     Result := ns;
   end;
 end;

 procedure InternalWrite(s: string);
 begin
    var cs := CountSymBeforeDivision(s);
    if cs = Length(s) then 
    begin
      TextOut(writecoords.X-1,writecoords.Y-1,s);
      var w := TextWidth(s)-1;
      writecoords.X += w;
    end
    else
    begin
      var srem := s.Substring(cs,s.Length-cs);
      s := s.Substring(0,cs);
      TextOut(writecoords.X-1,writecoords.Y-1,s);
      NextLine;
      InternalWrite(srem);
    end;
 end;

begin
  var s := _ObjectToString(obj);
  lock f do
  begin
    //var fnt := GraphABC.Font.NETFont;
//    GraphABC.Font.NETFont := CurrentWriteFont;
    InternalWrite(s);
//    GraphABC.Font.NETFont := fnt;
  end;  
end;

procedure IOGraphABCSystem.writeln;
begin
  NextLine
end;

procedure SetIOPanelVisible;
begin
  ed.Text := '';
  IOPanel.Visible := True;
  MainForm.Height := MainForm.Height + ed.Height;
  MainForm.ActiveControl := ed;
end;

procedure SetIOPanelInVisible;
begin
  IOPanel.Visible := False;
  MainForm.ActiveControl := f;
  MainForm.Height := MainForm.Height - ed.Height;
end;

function IOGraphABCSystem.read_symbol: char;
begin
  if readbuffer.Length=0 then
  begin
    IOPanel.Invoke(SetIOPanelVisible);
    // ������������� �������� �����
    MainThread.Suspend;
  end;
  Result := readBuffer[1];
  readBuffer := readBuffer.Remove(0,1);
end;

function IOGraphABCSystem.peek: integer;
begin
  if readbuffer.Length=0 then
  begin
    IOPanel.Invoke(SetIOPanelVisible);
    // ������������� �������� �����
    MainThread.Suspend;
  end;
  Result := integer(readBuffer[1]);
end;

// ------------ GraphABCCoordinate -----------------
constructor GraphABCCoordinate.Create;
begin
  // 1 - ������������ ������� ��������� (��� OY - ����)
  // -1 - ������������ ������� ��������� (��� OY - ����)
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

//2015.01>
procedure GraphABCPen.SetRoundCap(isRoundCap: boolean);
begin
  SetPenRoundCap(isRoundCap);
end;

function GraphABCPen.GetRoundCap: boolean;
begin
  result := PenRoundCap;
end;  
//2015.01<    




//!!!!!!!!!!!!!!!!!!!!

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
var
  tmp: Image;
  fs: System.IO.FileStream;
begin
  try
    fs := new System.IO.FileStream(fname, System.IO.FileMode.Open);
    tmp := Image.FromStream(fs);
    bmp := new Bitmap(tmp);
    fs.Flush;
    fs.Close;
    tmp.Dispose;
  except on ex: System.ArgumentException do
    raise new System.IO.FileNotFoundException(string.Format(FILE_NOT_FOUND_MESSAGE,fname));
  end;
   
  gb := Graphics.FromImage(bmp);  //!!!
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

{
// ������ ��������� ������������
// 1. Transparent := True  -  savedbmp:=bmp; bmp:=bmp.Clone; bmp.MakeTransparent(transpcolor);
// 2. Transparent := False  - bmp.Dispose; bmp := savedbmp; savedbmp := nil;
// 3. Transparentcolor := c
//   a) ���� Transparent = False, �� ������ ���������
//   �) ���� Transparent = True, �� bmp.Dispose; bmp:=savedbmp.Clone; bmp.MakeTransparent(transpcolor);
}
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
//2015.01>
var
  tmp: Image;
  fs: System.IO.FileStream;
//2015.01<
begin
  bmp.Dispose;
//2015.01>
//  bmp := new Bitmap(fname);
  try
    fs := new System.IO.FileStream(fname, System.IO.FileMode.Open);
    tmp := Image.FromStream(fs);
    bmp := new Bitmap(tmp);
    fs.Flush;
    fs.Close;
    tmp.Dispose;
  except on ex: System.ArgumentException do
    raise new System.IO.FileNotFoundException(string.Format(FILE_NOT_FOUND_MESSAGE,fname));
  end;
//2015.01<
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
// TODO �� ����, �����, ���� ���-�� ������ ��� ����������
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

procedure Picture.Draw(x,y: integer; rotateAngle: real; w,h: integer);
begin
  Monitor.Enter(f);
  var phi := (rotateAngle/360)*2*Pi;//���� � ��������
  
  //������ ����� ��������
  var new_w:integer := round(w*abs(cos(phi)) + h*abs(sin(phi)));
  var new_h:integer := round(w*abs(sin(phi)) + h*abs(cos(phi)));
  
  var newImg:Bitmap := new Bitmap(new_w, new_h);
  var g:Graphics := Graphics.FromImage(newImg);
  
  g.Clear(Color.FromArgb(0,0,0,0));
  g.TranslateTransform(new_w/2, new_h/2);
  g.RotateTransform(rotateAngle);
  g.TranslateTransform(-new_w/2, -new_h/2);
  
  g.DrawImage(bmp, (new_w-w)/2, (new_h-h)/2, w, h);
  //(x+w/2, y+h/2) - ����� ����������������� �����������
  //(x+new_w/2, y+new_h/2) - ����� ����������������� � ����������� �����������
  if NotLockDrawing then
  gr.DrawImage(newImg, x-(new_w-w)/2, y-(new_h-h)/2, new_w, new_h);
  if DrawInBuffer then
  gbmp.DrawImage(newImg, x-(new_w-w)/2, y-(new_h-h)/2, new_w, new_h);
  Monitor.Exit(f);
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
  
  gbmp.ReleaseHdc();
  
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

procedure InitBMP;
begin
  var ww := ScreenWidth;
  var hh := ScreenHeight;
  bmp := new Bitmap(ww,hh,gr);
  gbmp := Graphics.FromImage(bmp);
  __buffer := bmp;
  gbmp.FillRectangle(Brushes.White,0,0,ww,hh);
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
  
  Pen.NETPen := new System.Drawing.Pen(System.Drawing.Color.Black);
  GraphABC.Font.NETFont := new System.Drawing.Font('Arial',10);

  PixelBrush := new SolidBrush(System.Drawing.Color.Black);
  CurrentSolidBrush := new SolidBrush(System.Drawing.Color.White);
  CurrentHatchBrush := new HatchBrush(HatchStyle.Cross,System.Drawing.Color.Black,System.Drawing.Color.White);
  CurrentGradientBrush := new LinearGradientBrush(new Point(0,0), new Point(600,600),System.Drawing.Color.Black,System.Drawing.Color.White);
  Brush.NETBrush := CurrentSolidBrush;

//  CurrentWriteFont := new System.Drawing.Font('Courier New',10);

  NotLockDrawing := True;
  DrawInBuffer := True;
  RedrawProc := nil;
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

// Extension methods
procedure System.Drawing.Rectangle.MoveTo(x,y: integer);
begin
  Self.X := x;
  Self.Y := y;
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
// SSM 14.5.10
  FillEllipse(x-r,y-r,x+r+1,y+r+1);
end;

procedure DrawCircle(x,y,r: integer);
begin
// SSM 14.5.10
  DrawEllipse(x-r,y-r,x+r+1,y+r+1);
end;

procedure Circle(x,y,r: integer);
begin
// SSM 14.5.10
  Ellipse(x-r,y-r,x+r+1,y+r+1);
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

procedure TextOut(x,y: integer; n: integer); 
begin
  TextOut(x,y,n.ToString);
end;

procedure TextOut(x,y: integer; r: real); 
begin
  var nfi := new System.Globalization.NumberFormatInfo();
  nfi.NumberGroupSeparator := '.';

  TextOut(x,y,r.ToString(nfi));
end;

procedure DrawTextCentered(x,y,x1,y1: integer; s: string); 
begin
  Monitor.Enter(f);
  if NotLockDrawing then
    DrawTextCentered(x,y,x1,y1,s,gr);
  if DrawInBuffer then   
    DrawTextCentered(x,y,x1,y1,s,gbmp);
  Monitor.Exit(f);
end;

procedure DrawTextCentered(x,y,x1,y1: integer; n: integer); 
begin
  DrawTextCentered(x,y,x1,y1,n.ToString);  
end;

procedure DrawTextCentered(x,y,x1,y1: integer; r: real); 
begin
  var nfi := new System.Globalization.NumberFormatInfo();
  nfi.NumberGroupSeparator := '.';

  DrawTextCentered(x,y,x1,y1,r.ToString(nfi));  
end;

function Pnt(x,y: integer): Point;
begin
  Result := new Point(x,y);
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

  var hbmp := bmp.GetHbitmap(); // ��������� GDI Bitmap
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


//2015.01>
procedure SetPenRoundCap(isRoundCap: boolean);
var lCap: LineCap;
begin
  if isRoundCap then
    lCap := LineCap.Round
  else
    lCap := LineCap.Flat;
  lock f do
  begin   
    Pen.NETPen.StartCap := lCap;
    Pen.NETPen.EndCap := lCap;
  end;
end;
function PenRoundCap: boolean;
begin
  Result := (Pen.NETPen.StartCap = LineCap.Round)
    and (Pen.NETPen.EndCap = LineCap.Round);
end;
//2015.01<    


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
  Result := bsNone; // ���� ����� ��������������� ����� ������������� NETBrush
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
    _CurrentTextBrush.Color := c;
end;

function FontColor: Color;
begin
  Result := _CurrentTextBrush.Color;
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
// ������� 1. ��� ��� ������, ������� 1 ������ ������
  Result := round(gr.MeasureString(s,Font.NETFont,0,sf).Width) + 1;
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
//  Result := MainForm.ClientSize.Width;
  Result := f.Width;
end;

function WindowHeight: integer;
begin
//  Result := MainForm.ClientSize.Height;
  Result := f.Height;
end;

procedure SetWindowWidth(w: integer);
begin
//  SetWindowSize(w,MainForm.ClientSize.Height);
  SetWindowSize(w,WindowHeight);
end;

procedure SetWindowHeight(h: integer);
begin
//  SetWindowSize(MainForm.ClientSize.Width,h);
  SetWindowSize(WindowWidth,h);
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

procedure ChangeFormPos(l,t: integer); // ���������������
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

procedure ChangeFormClientSize(w,h: integer); // ���������������
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
//2015.01>
var
  b: Bitmap;
  tmp: Image;
  fs: System.IO.FileStream;
//2015.01<
begin
//2015.01>
//  var b: Bitmap := new Bitmap(fname);
  try
    fs := new System.IO.FileStream(fname, System.IO.FileMode.Open);
    tmp := Image.FromStream(fs);
    b := new Bitmap(tmp);
    fs.Flush;
    fs.Close;
    tmp.Dispose;
  except on ex: System.ArgumentException do
    raise new System.IO.FileNotFoundException(string.Format(FILE_NOT_FOUND_MESSAGE,fname));
  end;
//2015.01<
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

procedure _MaximizeWindow;
begin
  _MainForm.WindowState := FormWindowState.Maximized; 
end;

procedure _MinimizeWindow;
begin
  _MainForm.WindowState := FormWindowState.Minimized;
end;

procedure _NormalizeWindow;
begin
  _MainForm.WindowState := FormWindowState.Normal
end;

procedure MaximizeWindow;
begin
  _MainForm.Invoke(_MaximizeWindow);
end;

procedure MinimizeWindow;
begin
  _MainForm.Invoke(_MinimizeWindow);
end;

procedure NormalizeWindow;
begin
  _MainForm.Invoke(_NormalizeWindow);
end;

// BufferedDraw
procedure Redraw;
var tempbmp: Bitmap;
begin
  //TODO ��� ����� ������ ���� ��������
  if MainForm.WindowState=FormWindowState.Minimized then 
    exit;
  tempbmp := GetView(bmp,new System.Drawing.Rectangle(0,0,WindowWidth,WindowHeight));
  tempbmp.SetResolution(gr.DpiX,gr.DpiY);
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

function Rect(x1,y1,x2,y2: integer): System.Drawing.Rectangle;
begin
  Result := new System.Drawing.Rectangle(x1,y1,x2-x1,y2-y1);
end;

function ClientRectangle: System.Drawing.Rectangle;
begin
  Result := MainForm.ClientRectangle
end;

type FS = auto class
  mx,my,a,max: real;
  x0,y0: integer;
  f: Func<real,real>;

  function Apply(x: real): Point;
  begin
    Result := Pnt(x0+Round(mx*(x-a)),y0+Round(my*(max-f(x))));
  end;
end;

procedure Draw(f: Func<real,real>; a,b: real; x1,y1,x2,y2: integer);
begin
  Rectangle(x1,y1,x2+1,y2+1);
  var n := (x2-x1) div 3;
  var min := Range(a,b,n).Min(f);
  var max := Range(a,b,n).Max(f);
  var fso := new FS((x2-x1)/(b-a),(y2-y1)/(max-min),a,max,x1,y1,f);
  Polyline(Range(a,b,n).Select(fso.Apply).ToArray)
end;

procedure Draw(f: Func<real,real>; a,b: real; r: System.Drawing.Rectangle);
var x1 := r.X; y1 := r.Y;
    x2 := r.X+r.Width-1;
    y2 := r.Y+r.Height-1;
begin
  Draw(f,a,b,x1,y1,x2,y2);
end;

procedure Draw(f: Func<real,real>; r: System.Drawing.Rectangle);
begin
  Draw(f,-5,5,r);
end;

procedure Draw(f: Func<real,real>; a,b: real);
var x1 := 0; y1 := 0;
    x2 := Window.Width-1;
    y2 := Window.Height-1;
begin
  Draw(f,a,b,x1,y1,x2,y2);
end;

procedure Draw(f: Func<real,real>);
begin
  Draw(f,-5,5);
end;

var dpic := new Dictionary<string,Picture>;

procedure Draw(fname: string; x,y: integer);
begin
  if not dpic.ContainsKey(fname) then 
    dpic[fname] := new Picture(fname);
  dpic[fname].Draw(x,y);
end;

procedure Draw(fname: string; x,y,w,h: integer);
begin
  if not dpic.ContainsKey(fname) then 
    dpic[fname] := new Picture(fname);
  dpic[fname].Draw(x,y,w,h);
end;

procedure Draw(fname: string; x,y: integer; Scale: real);
begin
  if not dpic.ContainsKey(fname) then 
    dpic[fname] := new Picture(fname);
  var d := dpic[fname];
  var w := Round(d.Width * Scale);
  var h := Round(d.Height * Scale);
  dpic[fname].Draw(x,y,w,h);
end;

var firstcall := True;

procedure ReadlnTextBoxKeyDown(Sender: object; e: KeyEventArgs);
begin
  if IOPanel.Visible = False then 
    Exit;
  if e.KeyCode = Keys.Return then
  begin
    readbuffer := ed.Text + Environment.NewLine;
    IOPanel.Invoke(SetIOPanelInVisible);
    MainThread.Resume;
  end;
end;

procedure EnterButtonClick(Sender: object; e: EventArgs);
begin
  if IOPanel.Visible = False then 
    Exit;
  readbuffer := ed.Text + Environment.NewLine;
  IOPanel.Invoke(SetIOPanelInVisible);
  MainThread.Resume;
end;

procedure InitForm;
begin
  _CurrentTextBrush := new SolidBrush(System.Drawing.Color.Black);
  _ColorLinePen := new System.Drawing.Pen(System.Drawing.Color.Black);

  f := new ABCControl(defaultWindowWidth,defaultWindowHeight);
  _MainForm := new Form;
  _MainForm.Text := 'GraphABC.NET';
  _MainForm.ClientSize := new Size(defaultWindowWidth,defaultWindowHeight);
  _MainForm.BackColor := Color.White;
  _MainForm.Controls.Add(f);
  _MainForm.TopMost := True;
  _MainForm.StartPosition := FormStartPosition.CenterScreen;
  _MainForm.FormClosing += f.OnClosing;
  // ���� �����
  IOPanel := new Panel();
  IOPanel.Dock := DockStyle.Bottom;
  ed := new TextBox();
  ed.Dock := DockStyle.Fill;
  ed.Top := defaultWindowHeight;
  EnterButton := new Button();
  EnterButton.Text := BUTTON_ENTER_TEXT;
  EnterButton.Dock := DockStyle.Right;
  EnterButton.Click += EnterButtonClick;
  
  IOPanel.Visible := False;
  IOPanel.Size := new Size(defaultWindowWidth,ed.Height);
  IOPanel.Controls.Add(ed);
  IOPanel.Controls.Add(EnterButton);
  _MainForm.KeyPreview := True;
  _MainForm.KeyDown += ReadlnTextBoxKeyDown;
  _MainForm.Controls.Add(IOPanel);

  gr := Graphics.FromHwnd(f.Handle);
  
  if (System.Environment.OSVersion.Version.Major >= 6) then SetProcessDPIAware();
  InitBMP;

end;  

procedure InitForm0;
begin
  InitForm;
  StartIsComplete := True;
  Application.Run(MainForm);
end;

procedure InitGraphABC;
begin
  if not firstcall then exit;
  sf := new StringFormat(StringFormat.GenericTypographic);
  sf.FormatFlags := StringFormatFlags.MeasureTrailingSpaces;
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
  CurrentIOSystem := new IOGraphABCSystem;
end;

procedure SetConsoleIO;
begin
  CurrentIOSystem := new IOStandardSystem;
end;

procedure SetGraphABCIO;
begin
  CurrentIOSystem := new IOGraphABCSystem;
end;

procedure __InitModule;
begin
  MainThread := Thread.CurrentThread;
  InitGraphABC;
end;

var __initialized := false;

procedure __InitModule__;
begin
  if not __initialized then
  begin
    __initialized := true;
    GraphABCHelper.__InitModule__;
    __InitModule;
  end;
end;

initialization
  __InitModule;
finalization
//  Application.Run(MainWindow);
end.