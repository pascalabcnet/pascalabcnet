// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
///--
unit GraphWPFBase;

{$reference 'PresentationFramework.dll'}
{$reference 'WindowsBase.dll'}
{$reference 'PresentationCore.dll'}

{$apptype windows}

uses System.Windows; 
uses System.Windows.Controls;
uses System.Windows.Media;

type 
  /// Тип цвета
  GColor = System.Windows.Media.Color;
  /// Тип прямоугольника
  GRect = System.Windows.Rect;
  GBrush = System.Windows.Media.Brush;

  GWindow = System.Windows.Window;
  GMainWindow = class(GWindow)
  public
    function CreateContent: DockPanel; virtual;
    begin
      var g := new DockPanel;
      g.LastChildFill := True;
      Result := g;
    end;
    procedure InitMainGraphControl; virtual;
    begin
    end;
    procedure InitWindowProperties; virtual;
    begin
    end;
    procedure InitGlobals; virtual;
    begin
    end;
    procedure InitHandlers; virtual;
    begin
    end;
    
    constructor Create;
    begin
      Content := CreateContent; 
      InitMainGraphControl;
      InitGlobals;
      InitWindowProperties;
      InitHandlers;
    end;
    
    function MainPanel: DockPanel := Content as DockPanel;
  end;

var 
  app: Application;
  MainWindow: GMainWindow;

//var BrushesDict := new Dictionary<GColor,GBrush>;

function GetBrush(c: GColor): GBrush;
begin
  Result := new SolidColorBrush(c);
  Result.Freeze
end;  
{begin
  if not (c in BrushesDict) then
  begin
    var b := new SolidColorBrush(c);
    //BrushesDict[c] := b;
    Result := b
  end
  //else Result := BrushesDict[c];
end;}

procedure Invoke(d: System.Delegate; params args: array of object) := app.Dispatcher.Invoke(d, args);
procedure InvokeP(p: procedure(r: real); r: real) := Invoke(p,r); 

procedure Invoke(d: ()->()) := app.Dispatcher.Invoke(d);

function Invoke<T>(d: Func0<T>): T := T(app.Dispatcher.Invoke(d));
function InvokeReal(f: ()->real): real := Invoke&<Real>(f);
function InvokeString(f: ()->string): string := Invoke&<string>(f);
function InvokeBoolean(d: Func0<boolean>): boolean := Invoke&<boolean>(d);
function InvokeInteger(d: Func0<integer>): integer := Invoke&<integer>(d);
function Inv<T>(p: ()->T): T := Invoke&<T>(p); // Теперь это работает!

function MainDockPanel: DockPanel := MainWindow.MainPanel;

//{{{doc: Начало секции 1 }}} 
type
// -----------------------------------------------------
//>>     Класс WindowType # WindowType class
// -----------------------------------------------------
  ///!#
  /// Тип главного окна приложения
  WindowType = class
  private 
    procedure SetLeft(l: real);
    function GetLeft: real;
    procedure SetTop(t: real);
    function GetTop: real;
    procedure SetWidth(w: real);
    function GetWidth: real;
    procedure SetHeight(h: real);
    function GetHeight: real;
    procedure SetCaption(c: string);
    function GetCaption: string;
    procedure SetFixedSize(b: boolean);
    function GetFixedSize: boolean;
  public 
    /// Отступ главного окна от левого края экрана 
    property Left: real read GetLeft write SetLeft;
    /// Отступ главного окна от верхнего края экрана 
    property Top: real read GetTop write SetTop;
    /// Ширина клиентской части главного окна
    property Width: real read GetWidth write SetWidth;
    /// Высота клиентской части главного окна
    property Height: real read GetHeight write SetHeight;
    /// Заголовок окна
    property Caption: string read GetCaption write SetCaption;
    /// Заголовок окна
    property Title: string read GetCaption write SetCaption;
    /// Имеет ли графическое окно фиксированный размер
    property IsFixedSize: boolean read GetFixedSize write SetFixedSize;
    /// Очищает графическое окно белым цветом
    procedure Clear; virtual;
    /// Очищает графическое окно указанным цветом
    procedure Clear(c: GColor); virtual;
    /// Устанавливает размеры клиентской части главного окна 
    procedure SetSize(w, h: real);
    /// Устанавливает отступ главного окна от левого верхнего края экрана 
    procedure SetPos(l, t: real);
    /// Закрывает главное окно и завершает приложение
    procedure Close;
    /// Сворачивает главное окно
    procedure Minimize;
    /// Максимизирует главное окно
    procedure Maximize;
    /// Возвращает главное окно к нормальному размеру
    procedure Normalize;
    /// Центрирует главное окно по центру экрана
    procedure CenterOnScreen;
    /// Возвращает центр главного окна
    function Center: Point;
    /// Возвращает прямоугольник клиентской области окна
    function ClientRect: GRect;
    /// Возвращает случайную точку в границах экрана. Необязательный параметр margin задаёт минимальный отступ от границы 
    function RandomPoint(margin: real := 0): Point;
    private procedure CenterOnScreenP;
  end;
//{{{--doc: Конец секции 1 }}} 
  

var wp: real := -1;
var hp: real := -1;

function wplus: real;
begin
  if wp = -1 then
    wp := (SystemParameters.BorderWidth + SystemParameters.FixedFrameVerticalBorderWidth) * 2;
  Result := wp;
end; 

function hplus: real;
begin
  if hp = -1 then
    hp := SystemParameters.WindowCaptionHeight + (SystemParameters.BorderWidth + SystemParameters.FixedFrameHorizontalBorderHeight) * 2;
  Result := hp;
end;

///---- Window -----

procedure WindowTypeSetLeftP(l: real) := MainWindow.Left := l;
procedure WindowType.SetLeft(l: real) := Invoke(WindowTypeSetLeftP,l);

function WindowTypeGetLeftP := MainWindow.Left;
function WindowType.GetLeft := InvokeReal(WindowTypeGetLeftP);

procedure WindowTypeSetTopP(t: real) := MainWindow.Top := t;
procedure WindowType.SetTop(t: real) := Invoke(WindowTypeSetTopP,t);

function WindowTypeGetTopP := MainWindow.Top;
function WindowType.GetTop := InvokeReal(WindowTypeGetTopP);

procedure WindowTypeSetWidthP(w: real);
begin
  if MainWindow.ResizeMode = ResizeMode.NoResize then
    MainWindow.Width := w + 2.5
  else MainWindow.Width := w + wplus; 
end;  
  
procedure WindowType.SetWidth(w: real) := Invoke(WindowTypeSetWidthP,w);

function WindowTypeGetWidthP: real;
begin
  if MainWindow.ResizeMode = ResizeMode.NoResize then
    Result := MainWindow.ActualWidth - 2.5
  else Result := MainWindow.ActualWidth - wplus; 
end; 
function WindowType.GetWidth := InvokeReal(WindowTypeGetWidthP);

procedure WindowTypeSetHeightP(h: real);
begin
  if MainWindow.ResizeMode = ResizeMode.NoResize then
    MainWindow.Height := h + SystemParameters.WindowCaptionHeight + 2.5
  else MainWindow.Height := h + hplus;
end;
procedure WindowType.SetHeight(h: real) := Invoke(WindowTypeSetHeightP,h);

function WindowTypeGetHeightP: real;
begin
  if MainWindow.ResizeMode = ResizeMode.NoResize then
    Result := MainWindow.ActualHeight - SystemParameters.WindowCaptionHeight - 2.5
  else Result := MainWindow.ActualHeight - hplus;
  //Print(SystemParameters.WindowCaptionHeight, SystemParameters.WindowResizeBorderThickness.Top, SystemParameters.WindowResizeBorderThickness.Bottom);
end; 
function WindowType.GetHeight := InvokeReal(WindowTypeGetHeightP);

procedure WindowTypeSetCaptionP(c: string) := MainWindow.Title := c;
procedure WindowType.SetCaption(c: string) := Invoke(WindowTypeSetCaptionP,c);

function WindowTypeGetCaptionP := MainWindow.Title;
function WindowType.GetCaption := Invoke&<string>(WindowTypeGetCaptionP);

procedure WindowTypeSetFixedSizeP(b: boolean);
begin
  if b then 
    MainWindow.ResizeMode := ResizeMode.NoResize 
  else MainWindow.ResizeMode := ResizeMode.CanResize;
end;  
procedure WindowType.SetFixedSize(b: boolean) := Invoke(WindowTypeSetFixedSizeP,b);

function WindowTypeGetFixedSizeP := MainWindow.ResizeMode = ResizeMode.NoResize;
function WindowType.GetFixedSize := Invoke&<boolean>(WindowTypeGetFixedSizeP);

procedure WindowTypeClearP := begin {Host.children.Clear; CountVisuals := 0;} end;
procedure WindowType.Clear := Invoke(WindowTypeClearP);

procedure WindowType.Clear(c: GColor);
begin
  raise new System.NotImplementedException('WindowType.Clear(color) не реализовано')
end;

procedure WindowTypeSetSizeP(w, h: real);
begin
  WindowTypeSetWidthP(w);
  WindowTypeSetHeightP(h);
end;
procedure WindowType.SetSize(w, h: real) := Invoke(WindowTypeSetSizeP,w,h);

procedure WindowTypeSetPosP(l, t: real);
begin
  WindowTypeSetLeftP(l);
  WindowTypeSetTopP(t);
end;
procedure WindowType.SetPos(l, t: real) := Invoke(WindowTypeSetPosP,l,t);

procedure WindowType.Close := Invoke(MainWindow.Close);

procedure WindowTypeMinimizeP := MainWindow.WindowState := WindowState.Minimized;
procedure WindowType.Minimize := Invoke(WindowTypeMinimizeP);

procedure WindowTypeMaximizeP := MainWindow.WindowState := WindowState.Maximized;
procedure WindowType.Maximize := Invoke(WindowTypeMaximizeP);

procedure WindowTypeNormalizeP := MainWindow.WindowState := WindowState.Normal;
procedure WindowType.Normalize := Invoke(WindowTypeNormalizeP);

procedure WindowType.CenterOnScreenP;
begin
  var w := SystemParameters.PrimaryScreenWidth - MainWindow.Width;
  var h := SystemParameters.PrimaryScreenHeight - Mainwindow.Height;
  SetPos(w/2,h/2);
end;
 
procedure WindowType.CenterOnScreen := Invoke(CenterOnScreenP);

function Pnt(x,y: real) := new Point(x,y);
function Rect(x,y,w,h: real) := new System.Windows.Rect(x,y,w,h);

function WindowType.Center := Pnt(Width/2,Height/2);

function WindowType.ClientRect := Rect(0,0,Width,Height);

function WindowType.RandomPoint(margin: real): Point := Pnt(Random(margin,Width-margin),Random(margin,Height-margin));

function operator implicit(Self: (integer, integer)): Point; extensionmethod := new Point(Self[0], Self[1]);
function operator implicit(Self: (integer, real)): Point; extensionmethod := new Point(Self[0], Self[1]);
function operator implicit(Self: (real, integer)): Point; extensionmethod := new Point(Self[0], Self[1]);
function operator implicit(Self: (real, real)): Point; extensionmethod := new Point(Self[0], Self[1]);


function operator implicit(Self: (integer, integer)): Size; extensionmethod := new Size(Self[0], Self[1]);
function operator implicit(Self: (integer, real)): Size; extensionmethod := new Size(Self[0], Self[1]);
function operator implicit(Self: (real, integer)): Size; extensionmethod := new Size(Self[0], Self[1]);
function operator implicit(Self: (real, real)): Size; extensionmethod := new Size(Self[0], Self[1]);

function operator implicit(Self: array of (real, real)): array of Point; extensionmethod := 
  Self.Select(t->new Point(t[0],t[1])).ToArray;
function operator implicit(Self: array of (integer, integer)): array of Point; extensionmethod := 
  Self.Select(t->new Point(t[0],t[1])).ToArray;
 
procedure SetLeft(Self: UIElement; l: real); extensionmethod := Canvas.SetLeft(Self,l);

procedure SetTop(Self: UIElement; t: real); extensionmethod := Canvas.SetTop(Self,t);

var __initialized: boolean;

procedure __InitModule;
begin
end;

procedure __InitModule__;
begin
  if not __initialized then
  begin
    __initialized := true;
    __InitModule;
  end;
end;

initialization
  __InitModule;

finalization  
end.