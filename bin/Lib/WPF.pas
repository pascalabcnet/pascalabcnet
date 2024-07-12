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

  Panel = System.Windows.Controls.Panel; // Это для других модулей
  Grid = System.Windows.Controls.Grid;
  Canvas = System.Windows.Controls.Canvas;
  StackPanel = System.Windows.Controls.StackPanel;
  DockPanel = System.Windows.Controls.DockPanel;
  WrapPanel = System.Windows.Controls.WrapPanel;
  //TPanel = Panel;                        // Это для себя
  //TGrid = Grid;
  //TCanvas = Canvas;
  //TStackPanel = StackPanel;
  //TDockPanel = DockPanel;
  //TWrapPanel = WrapPanel;
  
  // Типы элементов управления
  Button = System.Windows.Controls.Button;
  &Label = System.Windows.Controls.Label;
  TextBlock = System.Windows.Controls.TextBlock;
  TextBox = System.Windows.Controls.TextBox;
  CheckBox = System.Windows.Controls.CheckBox;
  Slider = System.Windows.Controls.Slider;
  ComboBox = System.Windows.Controls.ComboBox;
  Border = System.Windows.Controls.Border;
  Rectangle = System.Windows.Shapes.Rectangle;
  RadioButton = System.Windows.Controls.RadioButton;
  //TButton = Button;
  TLabel = &Label;
  //TTextBlock = TextBlock;
  //TTextBox = TextBox;
  //TCheckBox = CheckBox;
  //TSlider = Slider;
  //TComboBox = ComboBox;

  DrawingVisual = DrawingVisual;
  
  Thickness = System.Windows.Thickness;
  //TThickness = Thickness;
  FontStyle = FontStyle;
  FontStyles = FontStyles;
  GridLength = GridLength;
  GridUnitType = GridUnitType;
  Brushes = Brushes;
  Brush = System.Windows.Media.Brush;
  Pen = Pen;
  Point = Point;
  Colors = Colors;
  SolidColorBrush = SolidColorBrush;
  Orientation = Orientation;
  //TOrientation = Orientation;
  VerticalAlignment = System.Windows.VerticalAlignment;
  HorizontalAlignment = System.Windows.HorizontalAlignment;
  HA = HorizontalAlignment;
  VA = VerticalAlignment;
  Dock = Dock;
  Color = Color;
  //TColor = Color;
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

function MainPanel: Panel := MainWindow.Content as Panel;

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

function AddToCell<T>(Self: T; gr: Grid; row,col: integer): T; extensionmethod; where T: FrameworkElement;
begin
  Result := Self;
  if gr<>nil then
    gr.Add(Result,row,col);
end;
  
function AddTo<T>(Self: T; gr: Panel): T; extensionmethod; where T: FrameworkElement;
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
function &With(Self: Grid; ShowGridLines: boolean := True): Grid; extensionmethod;
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

function &With<T>(Self: T; Background: Brush := nil; 
  Color: Color := Colors.White): T; extensionmethod; where T: Panel;
begin
  if Background <> nil then
    Self.Background := Background
  else Self.Background := GBrush(color);
  Result := Self;
end;  

{function &With(Self: Canvas; Background: Brush := nil; 
  Color: TColor := Colors.White): Canvas; extensionmethod; 
begin
  if Background <> nil then
    Self.Background := Background
  else Self.Background := GBrush(color);
  Result := Self;
end;}

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

{procedure StackPanel.Add(c: FrameworkElement);
begin
  Self.Children.Add(c);
end;}

procedure Panel.AddElements(params aa: array of FrameworkElement);
begin
  foreach var c in aa do
    Self.Children.Add(c);
end;

procedure Panel.AddElements(elements: sequence of FrameworkElement; 
  Width: real := real.NaN; Height: real := real.NaN; 
  Padding: Thickness := -1; Margin: Thickness := -1);
begin
  foreach var b in elements do
  begin  
    if Margin <> -1 then
      b.Margin := Margin;
    if Padding <> -1 then
    begin
      var prop := b.GetType.GetProperty('Padding');
      if prop <> nil then
        prop.SetValue(b,Padding);
    end;    
    if not real.IsNaN(Width) then
      b.Width := Width;
    if not real.IsNaN(Height) then
      b.Height := Height;
    Self.Add(b);
  end;  
end;

function Elements(params cc: array of FrameworkElement) := Lst(cc);

function Elements(cc: sequence of FrameworkElement) := Lst(cc);

function Text(Self: Button): string; extensionmethod := Self.Content as string;
function Text(Self: &Label): string; extensionmethod := Self.Content as string;
function Text(Self: CheckBox): string; extensionmethod := Self.Content as string;

function AddTo<T>(Self: T; gr: DockPanel; dock: WPF.Dock): T; extensionmethod; where T: FrameworkElement;
begin
  Result := Self;
  if gr<>nil then
    gr.Add(Result,dock);
end;

function Init(Self: FrameworkElement; Width: real := real.NaN; Height: real := real.NaN; 
  Margin: Thickness := -1): FrameworkElement; extensionmethod;
begin
  if not real.IsNaN(Width) then
    Self.Width := Width;
  if not real.IsNaN(Height) then
    Self.Height := Height;
  if Margin <> -1 then
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

//-----------------------------------------------------------------------//
//                 Функции создания элементов управления
//-----------------------------------------------------------------------//

function CreateElement<T>(Width: real := real.NaN; Height: real := real.NaN; Margin: Thickness := -1): T;
  where T: FrameworkElement, constructor;
begin
  Result := new T;
  Result.Init(Width,Height,Margin);
end;

/// Элемент управления Кнопка. Основное событие - Click
function CreateButton(Text: string; Width: real := real.NaN; Height: real := real.NaN;
  Margin: Thickness := -1; Padding: Thickness := -1): Button;
begin
  Result := CreateElement&<Button>(Width,Height,Margin);
  Result.Content := Text;
  if Padding <> -1 then
    Result.Padding := Padding;
end;

// Создает массив элементов управления Кнопка
function CreateButtons(Texts: array of string; Width: real := real.NaN; Height: real := real.NaN;
  Margin: Thickness := -1; Padding: Thickness := -1): array of Button;
begin
  Result := new Button[Texts.Length];
  for var i:=0 to Result.Length - 1 do
    Result[i] := CreateButton(Texts[i], Width, Height, Margin, Padding);
end;

/// Элемент управления для редактирования текста 
function CreateTextBox(Text: string; Width: real := real.NaN; Height: real := real.NaN;
  Margin: Thickness := -1): TextBox;
begin
  Result := CreateElement&<TextBox>(Width,Height,Margin);
  Result.Text := Text;
end;

/// Элемент управления для отображения текста
function CreateTextBlock(Text: string := ''; Width: real := real.NaN; Height: real := real.NaN;
  Margin: Thickness := -1): TextBlock;
begin
  Result := CreateElement&<TextBlock>(Width,Height,Margin);
  Result.Text := Text;
end;

/// Текстовая подпись для элемента управления
function CreateLabel(Text: string; Width: real := real.NaN; Height: real := real.NaN;
  Margin: Thickness := -1): &Label;
begin
  Result := CreateElement&<&Label>(Width,Height,Margin);
  Result.Content := Text;
end;

/// Элемент управления Флажок
function CreateCheckBox(Text: string; Width: real := real.NaN; Height: real := real.NaN;
  Margin: Thickness := -1; IsChecked: boolean := False): CheckBox;
begin
  Result := CreateElement&<CheckBox>(Width,Height,Margin);
  Result.Content := Text;
  Result.IsChecked := IsChecked;
end;

/// Элемент управления Слайдер
function CreateSlider(Min: real := 0; Max: real := 10; Step: real := 1; TickFrequency: real := 1; 
  Width: real := real.NaN; Height: real := real.NaN; Margin: Thickness := -1;
  Value: real := real.NaN): Slider;
begin
  Result := CreateElement&<Slider>(Width,Height,Margin);
  Result.Minimum := Min;
  Result.Maximum := Max;
  Result.SmallChange := Step;
  Result.LargeChange := Step;
  Result.TickFrequency := TickFrequency;
  Result.TickPlacement := TickPlacement.TopLeft;
  if not real.IsNaN(Value) then
    Result.Value := Value
end;

/// Элемент управления для выбора с раскрывающимся списком
function CreateComboBox(Items: array of string := nil; Width: real := real.NaN; Height: real := real.NaN;
  Margin: Thickness := -1): ComboBox;
begin
  Result := CreateElement&<ComboBox>(Width,Height,Margin);
  if items <> nil then
    foreach var item in items do
      Result.Items.Add(item);
  Result.SelectedIndex := 0;
end;

/// Элемент Прямоугольник
function CreateRectangle(Width: real := real.NaN; Height: real := real.NaN; Margin: Thickness := -1): Rectangle;
begin
  Result := CreateElement&<Rectangle>(Width,Height,Margin);
end;

/// Отображает границу и фон вокруг дочернего элемента. Основное свойство - Child 
function CreateBorder(Width: real := real.NaN; Height: real := real.NaN; 
  Margin: Thickness := -1; Background: Brush := nil; BorderBrush: Brush := nil;
  BorderThickness: Thickness := -1; Child: UIElement := nil): Border;
begin
  Result := CreateElement&<Border>(Width,Height,Margin);
  if Background<>nil then
    Result.Background := Background;
  if BorderBrush<>nil then
    Result.BorderBrush := BorderBrush;
  if BorderThickness<>-1 then
  Result.BorderThickness := BorderThickness;
  if Child <> nil then
    Result.Child := Child;
end;

/// Элемент управления Радиокнопка. Радиокнопки могут быть сгруппированы по GroupName. Основное событие - Checked
function CreateRadioButton(Width: real := real.NaN; Height: real := real.NaN; Margin: Thickness := -1;
  Text: string := ''; GroupName: string := nil; IsChecked: boolean := False): RadioButton;
begin
  Result := CreateElement&<Radiobutton>(Width,Height,Margin);
  Result.Content := Text;
  if GroupName<>nil then
    Result.GroupName := GroupName;
  Result.IsChecked := IsChecked;
end;

// Создает массив элементов управления Радиокнопка
function CreateRadioButtons(Texts: array of string; Width: real := real.NaN; Height: real := real.NaN;
  Margin: Thickness := -1; GroupName: string := nil): array of WPF.RadioButton;
begin
  Result := new WPF.RadioButton[Texts.Length];
  for var i:=0 to Result.Length - 1 do
    Result[i] := CreateRadioButton(Width, Height, Margin, Texts[i], GroupName);
end;

//-----------------------------------------------------------------------//
//                        Функции создания панелей
//-----------------------------------------------------------------------//

/// Панель, состоящая из строк и столбцов переменного размера
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

/// Панель, в которой можно пристыковывать элементы управления к краям, используя DockPanel.SetDock 
function CreateDockPanel(Margin: Thickness := -1): DockPanel;
begin
  Result := new DockPanel;
  if Margin <> -1 then
    Result.Margin := Margin;
end;

/// Определяет область, в которой можно располагать элементы управления с помощью координат
function CreateCanvas(Margin: Thickness := -1): Canvas;
begin
  Result := new Canvas;
  if Margin <> -1 then
    Result.Margin := Margin;
end;

/// Панель, располагающая элементы управления вертикально или горизонтально  
function CreateStackPanel(Width: real := real.NaN; Height: real := real.NaN;
  Margin: Thickness := -1; Horizontal: boolean := False; 
  HAlign: HorizontalAlignment := HA.Left; VAlign: VerticalAlignment := VA.Top): StackPanel;
begin
  Result := new StackPanel;
  Result.Init(Width,Height,Margin);
  if Horizontal then
    Result.Orientation := Orientation.Horizontal;
  Result.HorizontalAlignment := HAlign;
  Result.VerticalAlignment := VAlign;
end;

type
  ///!#
  Controls = static class
  public  
    /// Элемент управления Кнопка. Основное событие - Click
    static function Button(Text: string; Width: real := real.NaN; Height: real := real.NaN; 
      Margin: Thickness := -1; Padding: Thickness := -1): WPF.Button := WPF.CreateButton(Text,Width,Height,Margin,Padding);
    /// Создает массив элементов управления Кнопка
    static function Buttons(Texts: array of string; Width: real := real.NaN; Height: real := real.NaN;
      Margin: Thickness := -1; Padding: Thickness := -1): array of WPF.Button
      := WPF.CreateButtons(Texts,Width,Height,Margin,Padding);
    /// Элемент управления для редактирования текста 
    static function TextBox(Text: string; Width: real := real.NaN; Height: real := real.NaN;
      Margin: Thickness := -1): WPF.TextBox := WPF.CreateTextBox(Text, Width, Height, Margin);
    /// Элемент управления для редактирования текста    
    static function TextBlock(Text: string := ''; Width: real := real.NaN; Height: real := real.NaN;
      Margin: Thickness := -1) := WPF.CreateTextBlock(Text, Width, Height, Margin);
    /// Текстовая подпись для элемента управления
    static function &Label(Text: string; Width: real := real.NaN; Height: real := real.NaN;
      Margin: Thickness := -1): WPF.Label := WPF.CreateLabel(Text, Width, Height, Margin);
    /// Элемент управления Флажок
    static function CheckBox(Text: string; Width: real := real.NaN; Height: real := real.NaN;
      Margin: Thickness := -1; IsChecked: boolean := False): WPF.CheckBox 
      := WPF.CreateCheckBox(Text, Width, Height, Margin, IsChecked);
    /// Элемент управления Слайдер
    static function Slider(Min: real := 0; Max: real := 10; Step: real := 1; TickFrequency: real := 1; 
      Width: real := real.NaN; Height: real := real.NaN; Margin: Thickness := -1; Value: real := real.NaN): WPF.Slider
      := WPF.CreateSlider(Min, Max, Step, TickFrequency, Width, Height, Margin, Value);
    /// Элемент управления для выбора с раскрывающимся списком
    static function ComboBox(Items: array of string := nil; Width: real := real.NaN; Height: real := real.NaN;
      Margin: Thickness := -1): WPF.ComboBox := CreateComboBox(Items, Width, Height, Margin);
    /// Элемент Прямоугольник
    static function Rectangle(Width: real := real.NaN; Height: real := real.NaN; 
      Margin: Thickness := -1): WPF.Rectangle
      := CreateRectangle(Width, Height, Margin);
    /// Отображает границу и фон вокруг дочернего элемента. Основное свойство - Child 
    static function Border(Width: real := real.NaN; Height: real := real.NaN; 
      Margin: Thickness := -1; Background: Brush := nil;
      BorderBrush: Brush := nil; BorderThickness: Thickness := -1; Child: UIElement := nil): WPF.Border
      := CreateBorder(Width,Height,Margin,Background,BorderBrush,BorderThickness,Child);
    /// Элемент управления Радиокнопка. Радиокнопки могут быть сгруппированы по GroupName. Основное событие - Checked
    static function RadioButton(Width: real := real.NaN; Height: real := real.NaN; Margin: Thickness := -1;
      Text: string := ''; GroupName: string := nil; IsChecked: boolean := False): WPF.RadioButton 
      := CreateRadioButton(Width,Height,Margin,Text,GroupName,IsChecked);
    // Создает массив элементов управления Радиокнопка
    static function RadioButtons(Texts: array of string; Width: real := real.NaN; Height: real := real.NaN;
      Margin: Thickness := -1; GroupName: string := nil): array of WPF.RadioButton 
      := CreateRadioButtons(Texts, Width, Height, Margin, GroupName);
      
    static function Drawing: WPF.Drawing := CreateDrawing;
    /// Визуальный объект для отрисовки
    static function DrawingVisual: WPF.DrawingVisual := new WPF.DrawingVisual;
  end;
  
  Panels = static class
    /// Панель, состоящая из строк и столбцов переменного размера
    static function Grid(rows: integer := 1; cols: integer := 1; ColumnWidths: array of real := nil; RowHeights: array of real := nil): WPF.Grid 
      := CreateGrid(rows,cols,ColumnWidths,RowHeights);
    /// Панель, располагающая элементы управления вертикально или горизонтально  
    static function StackPanel(Width: real := real.NaN; Height: real := real.NaN; 
      Margin: Thickness := -1; Horizontal: boolean := False; 
      HAlign: HorizontalAlignment := HA.Left; VAlign: VerticalAlignment := VA.Top): WPF.StackPanel
      := CreateStackPanel(Width,Height,Margin,Horizontal,HAlign,VAlign);
    /// Панель, в которой можно пристыковывать элементы управления к краям, используя DockPanel.SetDock 
    static function DockPanel(Margin: Thickness := -1): WPF.DockPanel := CreateDockPanel(Margin);
    /// Определяет область, в которой можно располагать элементы управления с помощью координат
    static function Canvas(Margin: Thickness := -1): WPF.Canvas := CreateCanvas(Margin);
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
  
  MainWindow.Content := new Grid; // как в стандартном WPF-приложении
  CurrentParent := MainWindow;
finalization
  Application.Run(MainWindow);
end.