unit graph;

interface
{*****  Dos Style - Const  *****}
const
  {for SetBkColor, SetColor, SetFillStyle, SetRGBPalette / 16Color}
    Black            =  0;
    Blue             =  1;
    Green            =  2;
    Cyan             =  3;
    Red              =  4;
    Magenta          =  5;
    Brown            =  6;
    LightGray        =  7;
    DarkGray         =  8;
    LightBlue        =  9;
    LightGreen       = 10;
    LightCyan        = 11;
    LightRed         = 12;
    LightMagenta     = 13;
    Yellow           = 14;
    White            = 15;

  {for SetLineStyle}
    SolidLn          =  0;
    DottedLn         =  1;
    CenterLn         =  2;
    DashedLn         =  3;
    UserBitLn        =  4;
    NormWidth        =  1;
    ThickWidth       =  3;

  {for SetTextJustify}
    LeftText         =  0;
    CenterText       =  1;
    RightText        =  2;
    BottomText       =  0;
    TopText          =  2;
  {for SetTextStyle?}
    DefaultFont      =  0; 
    TriplexFont      =  1; 
    SmallFont        =  2;
    SansSerifFont    =  3;
    GothicFont       =  4;
    HorizDir         =  0;
    VertDir          =  1;

  {for SetFillStyle}
    EmptyFill        =  0;  
    SolidFill        =  1;  
    LineFill         =  2;  {-----}
    LtSlashFill      =  3;  {/////}
    SlashFill        =  4;  {/////}
    BkSlashFill      =  5;  {\\\\\}
    LtBkSlashFill    =  6;  {\\\\\}
    HatchFill        =  7;  {+++++}
    XHatchFill       =  8;  {XXXXX}
    InterleaveFill   =  9;  
    WideDotFill      = 10;  
    CloseDotFill     = 11;  
    UserFill         = 12;  

  {for SetWriteMode}
    CopyPut          =  0; { MOV }
    XORPut           =  1; { XOR }

  {Graphics drivers constants }

    detect = 0;
    cga = 1;
    mcga = 2;
    ega = 3;
    ega64 = 4;
    egamono = 5;
    ibm8514 = 6;
    hercmono = 7;
    att400 = 8;
    vga = 9;
    pc3270 = 10;

  { graphics modes constants. }
    cgac0 = 						0;
    cgac1 = 						1;
    cgac2 = 						2;
    cgac3 = 						3;
    cgahi = 						4;
    mcgac1 = 						1;
    mcgac2 = 						2;
    mcgac3 = 						3;
    mcgamed = 					4;
    mcgahi = 						5;
    egalo = 						0;
    egahi = 						1;
    ega64lo = 					0;
    ega64hi = 					1;
    egamonohi = 				3;
    hercmonohi = 				0;
    att400c0 = 					0;
    att400c1 = 					1;
    att400c2 = 					2;
    att400c3 = 				  3;
    att400med = 				4;
    att400hi = 					5;
    vgalo = 						0;
    vgamed = 						1;
    vgahi = 						2; 
    pc3270hi = 	   			0;
    ibm8514lo = 				0;
    ibm8514hi = 				1;
    

  {Graphics error codes}

    grok =              0;
    grnoinitgraph =     -1;
    grnotdetected =     -2;
    grfilenotfound =    -3;
    grinvaliddriver	=   -4;
    grnoloadmem =       -5;
    grnoscanmem =       -6;
    grnofloodmem =      -7;
    grfontnotfound =    -8;
    grnofontmem =       -9;
    grinvalidmode =	    -10;
    grerror =           -11;
    grioerror =         -12;
    grinvalidfont =     -13;
    grinvalidfontnum =  -14;
    grinvaliddevicenum =-15;
    grinvalidversion =  -18;

    TopOn = true;
    TopOff = false;

    ClipOn = true;
    ClipOff = false;

    UserCharSize = 0;

{*****  Dos Style - Type  *****}
type
  TColor =  integer;
   
  ArcCoordsType = record
     x, y, 
     xstart, ystart,
     xend, yend : integer;
  end;

  FillPatternType = array [1..8] of byte;

  FillSettingsType = record
     pattern : word;
     color   : word;
   end;

  LineSettingsType = record
     linestyle : word;
     pattern   : word;
     thickness : word;
   end;

  const
     MaxColors = 15;
  type
     PaletteType = record
       size    : byte;
       colors  : array[0..maxcolors] of shortint;
     end;

  PointType = record
     x, y : integer;
   end;

  TextSettingsType = record
     font      : word;
     direction : word;
     charsize  : word;
     horiz     : word;
     vert      : word;
   end;

  ViewPortType = record
     x1, y1, x2, y2 : integer;
     clip           : boolean;
   end;

  PointsType = array [1..100] of integer;

procedure InitGraph(var GraphDrv, GraphMode : integer; path_to_driver : string);		    
procedure CloseGraph;

function  GetMaxX : integer;
function  GetMaxY : integer;

function  GetX : integer;
function  GetY : integer;

procedure SetViewPort(x1, y1, x2, y2: integer; clip: Boolean);

procedure SetFillStyle(Pattern, Color: word);
procedure SetFillPattern(upattern : FillPatternType; Color: word);
procedure SetLineStyle(LineStyle, Pattern, Thickness: word);
procedure SetWriteMode(WriteMode: integer);

procedure GetArcCoords(var ArcCoords: ArcCoordsType);

procedure GetFillSettings(var FillInfo: FillSettingsType);
procedure GetFillPAttern(var FillPattern:  FillPatternType);
procedure GetLineSettings(var LineInfo: LineSettingsType);

function  GetDriverName : String;
function  GetGraphMode : integer;
procedure SetGraphMode(mode : integer);
function  GetModeName(GraphMode	: integer): string;

procedure GetAspectRatio(var ax, ay : word); 
procedure SetAspectRatio(ax, ay : word); 

procedure SetUserCharSize(multx, divx, multy, divy : word);

procedure GetTextSettings(var TextInfo : TextSettingsType);
procedure GetViewSettings(var ViewPort: ViewPortType);

procedure PutPixel(X, Y, Pixel : integer);
function  GetPixel(X, Y : integer) : integer;

function  GraphResult : integer; 
function  GraphErrorMsg(code : integer) : String;

function  ImageSize(x1, y1, x2, y2 : integer) : integer;
procedure GetImage(x1, y1, x2, y2 : integer; p : pointer);
procedure PutImage(x, y	: integer; p : pointer; i : integer); 

procedure Arc(x, y : integer; StAngle, EndAngle, Radius: Word);
procedure Bar(x1, y1, x2, y2 : integer);
procedure Bar3D(x1, y1, x2, y2 : integer; Depth: word; Top: boolean);
procedure Circle(X, Y : integer; Radius : word);
procedure DrawPoly(NumPoints : word; var Points : integer);
procedure Ellipse(x, y: integer; StAngle, EndAngle, XRadius, YRadius: Word);
procedure FillEllipse(x, y: integer; XRadius, YRadius: Word);
procedure FillPoly(NumPoints : word; Points : PointsType); 
procedure FloodFill(X, Y : integer; Color: TColor);
procedure MoveTo(X, Y : integer);
procedure LineTo(X, Y : integer);
procedure Line(x1, y1, x2, y2 : integer);
procedure LineRel(Dx, Dy : integer);
procedure MoveRel(Dx, Dy : integer);
procedure PieSlice(x, y: integer; StAngle, EndAngle, Radius: Word);
procedure Rectangle(x1, y1, x2, y2 : integer);
procedure RoundRect(x1, y1, x2, y2, x3, y3 : integer);
procedure Sector(x, y: integer; StAngle, EndAngle, XRadius, YRadius: Word);

{ *** color and palette routines *** }
procedure SetBkColor(Color : TColor);
procedure SetColor(Color : TColor);
function  GetColor : TColor;
procedure SetRGBPalette(I, R, G, B: word);
procedure SetAllPalette(Palette	: PaletteType );
procedure SetPalette(ColorNum	: word; Color : shortint);
procedure SetRGBPalette(ColorNum : word; red, green, blue : byte);

function  GetBkColor: word;
procedure GetDefaultPalette(var Palette: PaletteType);
function  GetMaxColor: word;
procedure GetPalette(var Palette: PaletteType);

{ *** text routines *** }
procedure OutText(Text: String);
procedure OutTextXY(X, Y: Integer; Text: String);
procedure SetTextJustify(Horiz, Vert: word);
procedure SetTextStyle(Font, Direction, CharSize: Word);
function  TextHeight(Text : String) : integer;
function  TextWidth(Text : String) : integer;


function KeyPressed : Boolean;
function ReadKey    : Char;

procedure Delay(msec : integer);

procedure ClearViewPort;
procedure ClearDevice;

implementation 

const NotImplementedMessage = 'This function not implemented';

procedure InitGraph(var GraphDrv, GraphMode : integer; path_to_driver : string);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure CloseGraph;
begin
  raise new Exception(NotImplementedMessage);
end;		    

function  GetMaxX : integer;
begin
  raise new Exception(NotImplementedMessage);
end;		    

function  GetMaxY : integer;
begin
  raise new Exception(NotImplementedMessage);
end;		    

function  GetX : integer;
begin
  raise new Exception(NotImplementedMessage);
end;		    

function  GetY : integer;
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure SetViewPort(x1, y1, x2, y2: integer; clip: Boolean);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure SetFillStyle(Pattern, Color: word);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure SetFillPattern(upattern : FillPatternType; Color: word);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure SetLineStyle(LineStyle, Pattern, Thickness: word);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure SetWriteMode(WriteMode: integer);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure GetArcCoords(var ArcCoords: ArcCoordsType);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure GetFillSettings(var FillInfo: FillSettingsType);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure GetFillPAttern(var FillPattern:  FillPatternType);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure GetLineSettings(var LineInfo: LineSettingsType);
begin
  raise new Exception(NotImplementedMessage);
end;		    

function  GetDriverName : String;
begin
  raise new Exception(NotImplementedMessage);
end;		    

function  GetGraphMode : integer;
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure SetGraphMode(mode : integer);
begin
  raise new Exception(NotImplementedMessage);
end;		    

function  GetModeName(GraphMode	: integer): string;
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure GetAspectRatio(var ax, ay : word); 
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure SetAspectRatio(ax, ay : word); 
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure SetUserCharSize(multx, divx, multy, divy : word);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure GetTextSettings(var TextInfo : TextSettingsType);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure GetViewSettings(var ViewPort: ViewPortType);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure PutPixel(X, Y, Pixel : integer);
begin
  raise new Exception(NotImplementedMessage);
end;		    

function  GetPixel(X, Y : integer) : integer;
begin
  raise new Exception(NotImplementedMessage);
end;		    

function  GraphResult : integer; 
begin
  raise new Exception(NotImplementedMessage);
end;		    

function  GraphErrorMsg(code : integer) : String;
begin
  raise new Exception(NotImplementedMessage);
end;		    

function  ImageSize(x1, y1, x2, y2 : integer) : integer;
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure GetImage(x1, y1, x2, y2 : integer; p : pointer);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure PutImage(x, y	: integer; p : pointer; i : integer); 
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure Arc(x, y : integer; StAngle, EndAngle, Radius: Word);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure Bar(x1, y1, x2, y2 : integer);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure Bar3D(x1, y1, x2, y2 : integer; Depth: word; Top: boolean);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure Circle(X, Y : integer; Radius : word);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure DrawPoly(NumPoints : word; var Points : integer);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure Ellipse(x, y: integer; StAngle, EndAngle, XRadius, YRadius: Word);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure FillEllipse(x, y: integer; XRadius, YRadius: Word);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure FillPoly(NumPoints : word; Points : PointsType); 
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure FloodFill(X, Y : integer; Color: TColor);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure MoveTo(X, Y : integer);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure LineTo(X, Y : integer);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure Line(x1, y1, x2, y2 : integer);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure LineRel(Dx, Dy : integer);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure MoveRel(Dx, Dy : integer);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure PieSlice(x, y: integer; StAngle, EndAngle, Radius: Word);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure Rectangle(x1, y1, x2, y2 : integer);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure RoundRect(x1, y1, x2, y2, x3, y3 : integer);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure Sector(x, y: integer; StAngle, EndAngle, XRadius, YRadius: Word);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure SetBkColor(Color : TColor);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure SetColor(Color : TColor);
begin
  raise new Exception(NotImplementedMessage);
end;		    

function  GetColor : TColor;
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure SetRGBPalette(I, R, G, B: word);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure SetAllPalette(Palette	: PaletteType );
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure SetPalette(ColorNum	: word; Color : shortint);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure SetRGBPalette(ColorNum : word; red, green, blue : byte);
begin
  raise new Exception(NotImplementedMessage);
end;		    

function  GetBkColor: word;
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure GetDefaultPalette(var Palette: PaletteType);
begin
  raise new Exception(NotImplementedMessage);
end;		    

function  GetMaxColor: word;
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure GetPalette(var Palette: PaletteType);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure OutText(Text: String);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure OutTextXY(X, Y: Integer; Text: String);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure SetTextJustify(Horiz, Vert: word);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure SetTextStyle(Font, Direction, CharSize: Word);
begin
  raise new Exception(NotImplementedMessage);
end;		    

function  TextHeight(Text : String) : integer;
begin
  raise new Exception(NotImplementedMessage);
end;		    

function  TextWidth(Text : String) : integer;
begin
  raise new Exception(NotImplementedMessage);
end;		    

function KeyPressed : Boolean;
begin
  raise new Exception(NotImplementedMessage);
end;		    

function ReadKey    : Char;
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure Delay(msec : integer);
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure ClearViewPort;
begin
  raise new Exception(NotImplementedMessage);
end;		    

procedure ClearDevice;
begin
  raise new Exception(NotImplementedMessage);
end;		    

end.	