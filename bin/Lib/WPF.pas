unit WPF;

{$reference 'PresentationFramework.dll'}
{$reference 'WindowsBase.dll'}
{$reference 'PresentationCore.dll'}

{$apptype windows}

uses System.Windows; 
uses System.Windows.Controls;
uses System.Windows.Controls.Primitives;
uses System.Windows.Data;
uses System.Windows.Media;
uses System.Windows.Shapes;
uses System.Windows.Markup;

type
  TControl = Control;

  Panel = Panel;
  Grid = Grid;
  Canvas = Canvas;
  StackPanel = StackPanel;
  DockPanel = DockPanel;
  WrapPanel = WrapPanel;

  TPanel = Panel;
  TGrid = Grid;
  TStackPanel = StackPanel;
  TDockPanel = DockPanel;
  TCanvas = Canvas;
  
  // Типы элементов управления
  Button = Button;
  TButton = Button;
  &Label = &Label;
  TLabel = &Label;
  CheckBox = CheckBox;
  TextBox = TextBox;
  Slider = Slider;
  ComboBox = ComboBox;
  TextBlock = TextBlock;
  Border = System.Windows.Controls.Border;
  Rectangle = Rectangle;

  DrawingVisual = DrawingVisual;
  
  Thickness = Thickness;
  TThickness = Thickness;
  FontStyle = FontStyle;
  FontStyles = FontStyles;
  GridLength = GridLength;
  GridUnitType = GridUnitType;
  Brushes = Brushes;
  Brush = Brush;
  Pen = Pen;
  Point = Point;
  Colors = Colors;
  SolidColorBrush = SolidColorBrush;
  Orientation = Orientation;
  TOrientation = Orientation;
  VerticalAlignment = VerticalAlignment;
  HorizontalAlignment = HorizontalAlignment;
  HA = HorizontalAlignment;
  VA = VerticalAlignment;
  Dock = Dock;
  Color = Color;
  TColor = Color;
  TickPlacement = TickPlacement;
  Binding = Binding;
  PropertyPath = PropertyPath;
  
  EventArgs = System.EventArgs;
  RoutedEventArgs = RoutedEventArgs;
  RoutedEventHandler = RoutedEventHandler;
  EventHandler = RoutedEventHandler;

  SizeToContent = SizeToContent;
  
  FontWeight = FontWeight;
  FontWeights = FontWeights;
  WindowStartupLocation = WindowStartupLocation;

var Application := new Application;
var MainWindow: Window;
var Content: UIElement;
var CurrentParent: FrameworkElement := nil;

type 
  Drawing = class(FrameworkElement)
  public  
    children: VisualCollection;
  protected 
    function GetVisualChild(index: integer): Visual; override;
    begin
      if (index < 0) or (index >= children.Count) then
        raise new System.ArgumentOutOfRangeException();
      Result := children[index];
    end;
  public  
    constructor := children := new VisualCollection(Self);
    property VisualChildrenCount: integer read children.Count; override;
    procedure Add(v: DrawingVisual) := children.Add(v);
  end;
  
function RGB(r,g,b: byte) := Color.Fromrgb(r, g, b);
function ARGB(a,r,g,b: byte) := Color.FromArgb(a, r, g, b);
function GrayColor(b: byte): Color := RGB(b, b, b);
function RandomColor := RGB(PABCSystem.Random(256), PABCSystem.Random(256), PABCSystem.Random(256));
function EmptyColor: Color := ARGB(0,0,0,0);
function Pnt(x,y: real) := new Point(x,y);
function Rect(x,y,w,h: real) := new System.Windows.Rect(x,y,w,h);

function CreateDrawing := Drawing.Create;

function operator implicit(n: integer): Thickness; extensionmethod;
begin
  Result := new Thickness(n);
end;

function operator implicit(n: real): Thickness; extensionmethod;
begin
  Result := new Thickness(n);
end;

function operator implicit(a: array of real): Thickness; extensionmethod;
begin
  case a.Length of
    1: Result := new Thickness(a[0]);
    2: Result := new Thickness(a[0],a[1],0,0);
    3: Result := new Thickness(a[0],a[1],a[2],0);
    4: Result := new Thickness(a[0],a[1],a[2],a[3]);
  end;  
end;

function operator implicit(a: array of integer): Thickness; extensionmethod;
begin
  case a.Length of
    1: Result := new Thickness(a[0]);
    2: Result := new Thickness(a[0],a[1],0,0);
    3: Result := new Thickness(a[0],a[1],a[2],0);
    4: Result := new Thickness(a[0],a[1],a[2],a[3]);
  end;  
end;

function operator implicit(name: string): FontFamily; extensionmethod;
begin
  Result := new FontFamily(name);
end;


procedure SetPosition(Self: Control; Left,Top: real); extensionmethod;
begin
  Canvas.SetLeft(Self,Left);
  Canvas.SetTop(Self,Top);
end;

procedure Add(Self: Panel; c: FrameworkElement); extensionmethod := Self.Children.Add(c);

procedure Add(Self: Grid; c: FrameworkElement; row,col: integer); extensionmethod;
begin
  Self.Children.Add(c);
  Grid.SetRow(c,row);
  Grid.SetColumn(c,col);
end;

function AddToCell<T>(Self: T; gr: TGrid; row,col: integer): T; extensionmethod; where T: FrameworkElement;
begin
  Result := Self;
  if gr<>nil then
    gr.Add(Result,row,col);
end;
  
function AddTo<T>(Self: T; gr: TPanel): T; extensionmethod; where T: FrameworkElement;
begin
  Result := Self;
  if gr<>nil then
    gr.Add(Result);
end;

function AddTo(Self: DrawingVisual; dr: Drawing): DrawingVisual; extensionmethod;
begin
  Result := Self;
  if dr<>nil then
    dr.children.Add(Result);
end;

function GBrush(c: Color): Brush := new SolidColorBrush(c);
function GBrush(r,g,b: integer): Brush := new SolidColorBrush(Color.FromRgb(r,g,b));
function GBrush(a,r,g,b: integer): Brush := new SolidColorBrush(Color.FromARgb(a,r,g,b));

function GPen(c: Color; width: real := 1) := new Pen(GBrush(c),width);

var CurrentPen := GPen(Colors.Black,1);
var CurrentBrush := GBrush(Colors.White);

// Декораторы 
function &With(Self: TGrid; ShowGridLines: boolean := True): TGrid; extensionmethod;
begin
  Self.ShowGridLines := ShowGridLines;
  Result := Self;
end;  

function &With<T>(Self: T; Margin: Thickness := 0; 
  HA: HorizontalAlignment := WPF.HA.Stretch;
  VA: VerticalAlignment := WPF.VA.Stretch
  ): T; extensionmethod; where T: FrameworkElement;
begin
  Self.Margin := Margin;
  Self.HorizontalAlignment := HA;
  Self.VerticalAlignment := VA;
  Result := Self;
end;  

function &With(Self: StackPanel; Background: Brush := nil; 
  Color: TColor := Colors.White): StackPanel; extensionmethod; 
begin
  if Background <> nil then
    Self.Background := Background
  else Self.Background := GBrush(color);
  Result := Self;
end;  

function &With(Self: Canvas; Background: Brush := nil; 
  Color: TColor := Colors.White): Canvas; extensionmethod; 
begin
  if Background <> nil then
    Self.Background := Background
  else Self.Background := GBrush(color);
  Result := Self;
end;  

function AsMainContent<T>(Self: T): T; extensionmethod; where T: FrameworkElement;
begin
  MainWindow.Content := Self;
  Result := Self;
  CurrentParent := Self;
end;

procedure Render(Self: DrawingVisual; draw: DrawingContext -> ()); extensionmethod;
begin
  var dc := Self.RenderOpen();
  draw(dc);
  dc.Close;
end;

function CreateBinding(Source: FrameworkElement; PropertyName: string; Mode: BindingMode := BindingMode.Default): Binding;
begin
  var bind := new Binding();
  bind.Source := Source; 
  bind.Path := new PropertyPath(PropertyName);
  bind.Mode := Mode;
  Result := bind;
end;

function SetBinding(Self: FrameworkElement; dp: DependencyProperty; dest: FrameworkElement; 
  PropertyName: string; Mode: BindingMode := BindingMode.Default): BindingExpressionBase; extensionmethod
  := Self.SetBinding(dp,CreateBinding(dest,PropertyName,Mode));

// Методы расширения панелей
procedure DockPanel.Add(c: FrameworkElement; dock: WPF.Dock);
begin
  Self.Add(c);
  DockPanel.SetDock(c,dock);
end;

procedure StackPanel.Add(c: FrameworkElement);
begin
  Self.Children.Add(c);
end;

procedure StackPanel.AddElements(params aa: array of FrameworkElement);
begin
  foreach var c in aa do
    Self.Children.Add(c);
end;

procedure StackPanel.AddButtons(buttons: array of Button; 
  Width: real := real.NaN; Height: real := real.NaN; Padding: Thickness := 0; Margin: Thickness := 0);
begin
  foreach var b in buttons do
  begin  
    b.Margin := Margin;
    b.Padding := Padding;
    b.Width := Width;
    b.Height := Height;
    Self.Add(b);
  end;  
end;

function Text(Self: TButton): string; extensionmethod := Self.Content as string;
function Text(Self: &Label): string; extensionmethod := Self.Content as string;
function Text(Self: CheckBox): string; extensionmethod := Self.Content as string;

function AddTo<T>(Self: T; gr: DockPanel; dock: WPF.Dock): T; extensionmethod; where T: FrameworkElement;
begin
  Result := Self;
  if gr<>nil then
    gr.Add(Result,dock);
end;

function Init(Self: FrameworkElement; Width: real := real.NaN; Height: real := real.NaN; 
  Margin: Thickness := 0): FrameworkElement; extensionmethod;
begin
  Self.Width := Width;
  Self.Height := Height;
  Self.Margin := Margin;
  Result := Self;
end;

function ParseXaml(s: string): object;
begin
  var context := new ParserContext();
  context.XmlnsDictionary.Add('','http://schemas.microsoft.com/winfx/2006/xaml/presentation');
  context.XmlnsDictionary.Add('x', 'http://schemas.microsoft.com/winfx/2006/xaml');
  Result := System.Windows.Markup.XamlReader.Parse(s,context);
end;

function LoadFromXaml(fname: string): object;
begin
  var s := ReadAllText(fname);
  Result := ParseXaml(s);
end;

// Функции создания элементов управления

function CreateButton(Text: string; Width: real := real.NaN; Height: real := real.NaN;
  Margin: Thickness := 0; Padding: Thickness := 0): Button;
begin
  Result := Button.Create;
  Result.Init(Width,Height,Margin);
  Result.Content := Text;
  Result.Padding := Padding;
end;

function CreateButtons(Texts: array of string; Width: real := real.NaN; Height: real := real.NaN;
  Margin: Thickness := 0; Padding: Thickness := 0): array of Button;
begin
  Result := new Button[Texts.Length];
  for var i:=0 to Result.Length - 1 do
    Result[i] := CreateButton(Texts[i], Width, Height, Margin, Padding);
end;

function CreateTextBox(Text: string; Width: real := real.NaN; Height: real := real.NaN;
  Margin: Thickness := 0): TextBox;
begin
  Result := TextBox.Create;
  Result.Init(Width,Height,Margin);
  Result.Text := Text;
end;

function CreateLabel(Text: string; Width: real := real.NaN; Height: real := real.NaN;
  Margin: Thickness := 0): &Label;
begin
  Result := &Label.Create;
  Result.Init(Width,Height,Margin);
  Result.Content := Text;
end;

function CreateTextBlock(Text: string := ''; Width: real := real.NaN; Height: real := real.NaN;
  Margin: Thickness := 0): TextBlock;
begin
  Result := TextBlock.Create;
  Result.Init(Width,Height,Margin);
  Result.Text := Text;
end;

function CreateCheckBox(Text: string; Width: real := real.NaN; Height: real := real.NaN;
  Margin: Thickness := 0; IsChecked: boolean := False): CheckBox;
begin
  Result := CheckBox.Create;
  Result.Init(Width,Height,Margin);
  Result.Content := Text;
  Result.IsChecked := IsChecked;
end;

function CreateSlider(Min: real := 0; Max: real := 10; Step: real := 1; TickFrequency: real := 1; 
  Width: real := real.NaN; Height: real := real.NaN; Margin: Thickness := 0;
  Value: real := real.NaN): Slider;
begin
  Result := Slider.Create;
  Result.Init(Width,Height,Margin);
  Result.Minimum := Min;
  Result.Maximum := Max;
  Result.SmallChange := Step;
  Result.LargeChange := Step;
  Result.TickFrequency := TickFrequency;
  Result.TickPlacement := TickPlacement.TopLeft;
  if not real.IsNaN(Value) then
    Result.Value := Value
end;

//-- Создание панелей

function CreateComboBox(Items: array of string := nil; Width: real := real.NaN; Height: real := real.NaN;
  Margin: Thickness := 0): ComboBox;
begin
  Result := ComboBox.Create;
  Result.Init(Width,Height,Margin);
  if items <> nil then
    foreach var item in items do
      Result.Items.Add(item);
  Result.SelectedIndex := 0;
end;

function CreateGrid(rows: integer := 1; cols: integer := 1; ColumnWidths: array of real := nil; 
  RowHeights: array of real := nil): Grid;
begin
  var gr := new Grid;
  //gr.ShowGridLines := True;
  loop rows do
    gr.RowDefinitions.Add(new RowDefinition);
  loop cols do
    gr.ColumnDefinitions.Add(new ColumnDefinition);
  if ColumnWidths <> nil then
    for var i:=0 to ColumnWidths.Length - 1 do
      gr.ColumnDefinitions[i].Width := new GridLength(ColumnWidths[i],GridUnitType.Star);
  if RowHeights <> nil then
    for var i:=0 to RowHeights.Length - 1 do
      gr.RowDefinitions[i].Height := new GridLength(RowHeights[i],GridUnitType.Star);
  Result := gr;
end;

function CreateDockPanel(Margin: Thickness := 0): DockPanel;
begin
  Result := new DockPanel;
  Result.Margin := Margin;
end;

function CreateCanvas(Margin: Thickness := 0): Canvas;
begin
  Result := new Canvas;
  Result.Margin := Margin;
end;

function CreateStackPanel(Width: real := real.NaN; Height: real := real.NaN;
  Margin: Thickness := 0; Horizontal: boolean := False): StackPanel;
begin
  Result := new StackPanel;
  Result.Init(Width,Height,Margin);
  if Horizontal then
    Result.Orientation := Orientation.Horizontal;
end;

function CreateRectangle(Width: real := real.NaN; Height: real := real.NaN;
  Margin: Thickness := 0): Rectangle;
begin
  Result := new Rectangle;
  Result.Init(Width,Height,Margin);
end;

type
  ///!#
  Controls = static class
    static function Button(Text: string; Left: real := real.NaN; Top: real := real.NaN; 
      Width: real := real.NaN; Height: real := real.NaN; Margin: Thickness := 0;
      Padding: Thickness := 0): TButton
        := WPF.CreateButton(Text,Width,Height,Margin,Padding);
    static function Drawing: WPF.Drawing
      := CreateDrawing;
    static function DrawingVisual: WPF.DrawingVisual
      := new WPF.DrawingVisual;
  end;
  Panels = static class
    static function Grid(rows: integer := 1; cols: integer := 1; ColumnWidths: array of real := nil; RowHeights: array of real := nil): TGrid 
      := CreateGrid(rows,cols,ColumnWidths,RowHeights);
    static function StackPanel(Width: real := real.NaN; Height: real := real.NaN; 
      Margin: Thickness := 0; Horizontal: boolean := False): TStackPanel
        := CreateStackPanel(Width,Height,Margin,Horizontal);
    static function DockPanel(Margin: Thickness := 0): TDockPanel := CreateDockPanel(Margin);
  end;

initialization
  MainWindow := new Window;
  
  MainWindow.Width := 800;
  MainWindow.Height := 600;
  MainWindow.Title := 'WPF';
  
  var w := SystemParameters.PrimaryScreenWidth - MainWindow.Width;
  var h := SystemParameters.PrimaryScreenHeight - Mainwindow.Height;
  MainWindow.Left := w/2;
  MainWindow.Top := h/2;
  
  var gr := new Grid;
  MainWindow.Content := gr;
  CurrentParent := MainWindow;
finalization
  Application.Run(MainWindow);
end.