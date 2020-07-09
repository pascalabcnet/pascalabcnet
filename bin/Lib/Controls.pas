// Copyright (©) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
///Модуль элементов управления для GraphWPF
unit Controls;

interface

uses GraphWPFBase;
uses System.Windows; 
uses System.Windows.Media; 
uses System.Windows.Controls; 
uses System.ComponentModel;

procedure AddRightPanel(Width: real := 200; c: Color := Colors.LightGray; Margin: real := 10);
procedure AddLeftPanel(Width: real := 200; c: Color := Colors.LightGray; Margin: real := 10);
procedure AddTopPanel(Height: real := 70; c: Color := Colors.LightGray; Margin: real := 10);
procedure AddBottomPanel(Height: real := 70; c: Color := Colors.LightGray; Margin: real := 10);
procedure AddStatusBar(Height: real := 24);

var
  ActivePanel: Panel;
  GlobalHMargin := 12;

type
  GButton = System.Windows.Controls.Button;
  GTextBlock = System.Windows.Controls.TextBlock;
  GTextBox = System.Windows.Controls.TextBox;
  GListBox = System.Windows.Controls.ListBox;
  Key = System.Windows.Input.Key;
  ///!#
  CommonControl = class
  protected 
    element: FrameworkElement;
    function GetM: real := InvokeReal(()->element.Margin.Top);
    procedure SetMP(m: real) := element.Margin := new Thickness(m);
    procedure SetM(m: real) := Invoke(SetMP, m);
    function GetW: real := InvokeReal(()->element.Width);
    procedure SetWP(w: real) := element.Width := w;
    procedure SetW(w: real) := Invoke(SetWP, w);
    function GetH: real := InvokeReal(()->element.Height);
    procedure SetHP(h: real) := element.Height := h;
    procedure SetH(h: real) := Invoke(SetHP, h);
  public 
    property Width: real read GetW write SetW;
    property Height: real read GetH write SetH;
    property Margin: real read GetM write SetM;
  end;
  ///!#
  ButtonT = class(CommonControl)
  protected
    function b: GButton := element as GButton;
    procedure BClick(sender: Object; e: RoutedEventArgs);
    begin
      if Click <> nil then
        Click;
    end;
    
    function GetText: string := InvokeString(()->b.Content as string);
    procedure SetTextP(t: string) := b.Content := t;
    procedure SetText(t: string) := Invoke(SetTextP, t);
    procedure CreateP(Txt: string; fontSize: real);
    begin
      element := new GButton;
      element.Margin := new Thickness(0, 0, 0, GlobalHMargin);
      //Margin := GlobalMargin;
      Text := Txt;
      Self.FontSize := fontSize;
      b.Click += BClick;
      b.Focusable := False;
      ActivePanel.Children.Add(b);
    end;
    
    procedure CreatePXY(x, y: real; Txt: string; fontSize: real);
    begin
      element := new GButton;
      element.SetLeft(x);
      element.SetTop(y);
      element.Margin := new Thickness(0, 0, 0, GlobalHMargin);
      //Margin := GlobalMargin;
      Text := Txt;
      b.Click += BClick;
      b.Focusable := False;
      ActivePanel.Children.Add(b);
    end;
  
  public 
    Click: procedure;
    constructor Create(Txt: string; fontSize: real := 12);
    begin
      Invoke(CreateP, Txt, fontSize);
    end;
    
    constructor Create(x, y: real; Txt: string; fontsize: real := 12);
    begin
      Invoke(CreatePXY, x, y, Txt, fontsize);
    end;
    
    property Text: string read GetText write SetText;
    property FontSize: real read InvokeReal(()->b.FontSize) write Invoke(procedure(t: real) -> b.FontSize := t, value);
    property Enabled: boolean read InvokeBoolean(()->b.IsEnabled) write Invoke(procedure(t: boolean) -> b.IsEnabled := t, value);
  end;
  
  ///!#
  TextBlockT = class(CommonControl)
  protected
    function b: GTextBlock := element as GTextBlock;
    procedure CreateP(Txt: string; fontsize: real);
    begin
      var tb := new GTextBlock;
      element := tb;
      //element.Margin := new Thickness(0, 0, 0, GlobalHMargin);
      //element.Margin := new Thickness(5,5,5,0);
      tb.FontSize := fontsize;
      Text := Txt;
      ActivePanel.Children.Add(b);
    end;
    
    procedure CreatePXY(x, y: real; Txt: string; fontsize: real);
    begin
      var tb := new GTextBlock;
      element := tb;
      element.Margin := new Thickness(0, 0, 0, GlobalHMargin);
      //tb.Background := new SolidColorBrush(Colors.White);
      //tb.Opacity := 0.7;
      Canvas.SetLeft(element, x);
      Canvas.SetTop(element, y);
      tb.FontSize := fontsize;
      Text := Txt;
      ActivePanel.Children.Add(b);
    end;
  
  public 
    constructor Create(Txt: string; fontsize: real := 12):= Invoke(CreateP, Txt, fontsize);
  constructor Create(x, y: real; Txt: string; fontsize: real := 12):= Invoke(CreatePXY, x, y, Txt, fontsize);
property Text: string read InvokeString(()->b.Text) write Invoke(procedure(t: string) -> b.Text := t, value);
property FontSize: real read InvokeReal(()->b.FontSize) write Invoke(procedure(t: real) -> b.FontSize := t, value);
end;

IntegerBlockT = class(TextBlockT)
private 
  val := 0;
  message: string;
  procedure CreateP(message: string; fontSize: real := 12; initValue: integer := 0);
  begin
    inherited CreateP(message + ' ' + initValue, fontSize);
    Self.message := message;
    val := initValue;
  end;

public 
  property Value: integer read val write begin val := value; Text := message + ' ' + val; end;
  constructor Create(message: string; fontSize: real := 12; initValue: integer := 0):= Invoke(CreateP, message, fontSize, initValue);
end;

RealBlockT = class(TextBlockT)
private 
  val := 0.0;
  message: string;
  procedure CreateP(message: string; fontSize: real := 12; initValue: real := 0);
  begin
    inherited CreateP(message + ' ' + initValue.ToString(FracDigits), fontSize);
    Self.message := message;
    val := initValue;
  end;

public 
  property Value: real read val write begin val := value; Text := message + ' ' + val.ToString(FracDigits); end;
  constructor Create(message: string; fontSize: real := 12; initValue: real := 0):= Invoke(CreateP, message, fontSize, initValue);
auto property FracDigits: integer := 1;
end;

  ///!#
TextBoxT = class(CommonControl)
protected
  function tb: GTextBox := element as GTextBox;
  procedure BTextChanged(sender: Object; e: TextChangedEventArgs);
  begin
    if Click <> nil then
      Click;
  end;
  
  function GetText: string := InvokeString(()->tb.Text);
  procedure SetTextP(t: string) := tb.Text := t;
  procedure SetText(t: string) := Invoke(SetTextP, t);
  procedure CreateP(Txt: string; w: real);
  begin
    element := new GTextBox;
    element.HorizontalAlignment := HorizontalAlignment.Stretch;
    element.Margin := new Thickness(0, 0, 0, GlobalHMargin);
    Text := Txt;
    if w > 0 then
      Width := w;
    tb.TextChanged += BTextChanged;
    ActivePanel.Children.Add(tb);
  end;

public 
  event Click: procedure;
  constructor Create(Txt: string := ''; w: real := 0);
  begin
    Invoke(CreateP, Txt, w);
  end;
  
  property Text: string read GetText write SetText;
  property FontSize: real read InvokeReal(()->tb.FontSize) write Invoke(procedure(t: real) -> tb.FontSize := t, value);
end;

IntegerBoxT = class(TextBoxT)
  function GetValue: integer;
  begin
    if Trim(Text) = '' then
      Result := 0
    else Result := integer.Parse(Text);
  end;
  
  procedure SetValue(x: integer) := Text := x.ToString;
public 
  constructor Create(w: real := 0);
  begin
    inherited Create('0', w);
    var tb := element as GTextBox;
    tb.MouseWheel += procedure (o, e) -> begin
      if e.Delta > 0 then
        SetValue(GetValue + 1)
      else if e.Delta < 0 then
        SetValue(GetValue - 1)
    end;
    tb.KeyDown += procedure (o, e) -> begin
      if not ((e.Key >= Key.D0) and (e.Key <= Key.D9)) then
        e.Handled := True;
    end;
    {tb.TextInput += procedure (o,e) -> begin
    Print(e.Text);
    if not ((e.Key>=Key.D0) and (e.Key<=Key.D9)) then
    e.Handled := True;
    end;}
  end;
  
  property Value: integer read GetValue write SetValue;
  property FontSize: real read InvokeReal(()->tb.FontSize) write Invoke(procedure(t: real) -> tb.FontSize := t, value);
end;

TextBoxWithBlockT = class(CommonControl)
private 
  l: TextBlock;
protected
  function tb: GTextBox := (element as StackPanel).Children[1] as GTextBox;
  procedure BTextChanged(sender: Object; e: TextChangedEventArgs);
  begin
    if Click <> nil then
      Click;
  end;
  
  function GetText: string := InvokeString(()->tb.Text);
  procedure SetTextP(t: string) := tb.Text := t;
  procedure SetText(t: string) := Invoke(SetTextP, t);
  procedure CreateP(BlockTxt, Txt: string; w: real);
  begin
    var sp := new StackPanel;
    element := sp;
    sp.Orientation := Orientation.Horizontal;
    sp.HorizontalAlignment := HorizontalAlignment.Stretch;
    l := new TextBlock();
    l.Text := BlockTxt;
    sp.Children.Add(l);
    var tb := new GTextBox;
    tb.Width := 100;
    tb.VerticalAlignment := VerticalAlignment.Stretch;
    tb.Margin := new Thickness(0, 0, 0, GlobalHMargin);
    sp.Children.Add(tb);
    Text := Txt;
    if w > 0 then
      Width := w;
    tb.TextChanged += BTextChanged;
    ActivePanel.Children.Add(sp);
  end;

public 
  event Click: procedure;
  constructor Create(BlockTxt: string; Txt: string := ''; w: real := 0);
  begin
    Invoke(CreateP, BlockTxt, Txt, w);
  end;
  
  property Text: string read GetText write SetText;
  property FontSize: real read InvokeReal(()->tb.FontSize) write Invoke(procedure(t: real) -> begin tb.FontSize := t; l.FontSize := t; end, value);
end;

  ///!#
ListBoxT = class(CommonControl)
protected
  function tb: GListBox := element as GListBox;
  procedure CreateP(w, h: real);
  begin
    element := new GListBox;
    element.HorizontalAlignment := HorizontalAlignment.Stretch;
    element.Margin := new Thickness(0, 0, 0, GlobalHMargin);
    if w > 0 then
      Width := w;
    Height := h;
    ActivePanel.Children.Add(tb);
  end;
  
  procedure AddP(s: string);
  begin
    var lbi := new ListBoxItem();
    lbi.Content := s;
    tb.Items.Add(lbi);
  end;  
  
  procedure SortP := tb.Items.SortDescriptions.Add(new SortDescription('Content', ListSortDirection.Ascending));
  procedure SortPDescending := tb.Items.SortDescriptions.Add(new SortDescription('Content', ListSortDirection.Descending));
public 
  event Click: procedure;
  constructor Create(w: real := 0; h: real := 200):= Invoke(CreateP, w, h);
procedure Sort := Invoke(SortP);
  procedure SortDescending := Invoke(SortPDescending);
  procedure Add(s: string) := Invoke(AddP, s);
  property FontSize: real read InvokeReal(()->tb.FontSize) write Invoke(procedure(t: real) -> tb.FontSize := t, value);
  property Count: integer read InvokeInteger(()->tb.Items.Count);
  property SelectedIndex: integer read InvokeInteger(()->tb.SelectedIndex) write Invoke(procedure(t: integer) -> tb.SelectedIndex := t, value);
  property SelectedText: string read InvokeString(()->tb.SelectedItem as string) write Invoke(procedure(t: string) -> tb.SelectedItem := t, value);
end;

SliderT = class(CommonControl)
private
  function sl: Slider := element as Slider;
  function GetMinimum: real := InvokeReal(()->sl.Minimum);
  procedure SetMinimumP(r: real) := sl.Minimum := r;
  procedure SetMinimum(r: real) := Invoke(SetMinimumP, r);
  function GetMaximum: real := InvokeReal(()->sl.Maximum);
  procedure SetMaximumP(r: real) := sl.Maximum := r;
  procedure SetMaximum(r: real) := Invoke(SetMaximumP, r);
  function GetValue: real := InvokeReal(()->sl.Value);
  procedure SetValueP(r: real) := sl.Value := r;
  procedure SetValue(r: real) := Invoke(SetValueP, r);
  function GetFrequency: real := InvokeReal(()->sl.TickFrequency);
  procedure SetFrequencyP(r: real) := sl.TickFrequency := r;
  procedure SetFrequency(r: real) := Invoke(SetFrequencyP, r);
protected 
  procedure CreateP(min, max, val: real);
  begin
    element := new Slider;
    sl.ValueChanged += procedure(o, e) -> ValueChangedP;
    sl.TickPlacement := System.Windows.Controls.Primitives.TickPlacement.BottomRight;
    sl.Minimum := min;
    sl.Maximum := max;
    sl.Value := val;
    sl.Foreground := Brushes.Black;
    ActivePanel.Children.Add(sl);
  end;
  
  procedure ValueChangedP := if ValueChanged <> nil then ValueChanged;
public 
  ValueChanged: procedure;
  constructor Create(min, max, val: real);
  begin
    Invoke(CreateP, min, max, val);
  end;
  
  property Minimum: real read GetMinimum write SetMinimum;
  property Maximum: real read GetMaximum write SetMaximum;
  property Value: real read GetValue write SetValue;
  property Frequency: real read GetFrequency write SetFrequency;
end;


function Button(Txt: string; fontSize: real := 12): ButtonT;
function Button(x, y: integer; Txt: string; fontSize: real := 12): ButtonT;
function TextBlock(Txt: string; fontSize: real := 12): TextBlockT;
function TextBlock(x, y: real; Txt: string; fontSize: real := 12): TextBlockT;
function IntegerBlock(message: string; fontSize: real := 12; initValue: integer := 0): IntegerBlockT;
function RealBlock(message: string; fontSize: real := 12; initValue: real := 0): RealBlockT;
function TextBox(Txt: string := ''; w: real := 0): TextBoxT;
function ListBox(w: real := 0; h: real := 200): ListBoxT;
function IntegerBox(w: real := 0): IntegerBoxT;
function Slider(min: real := 0; max: real := 10; val: real := 0): SliderT;

procedure EmptyBlock(sz: integer := 16);

implementation

uses GraphWPF;
uses System.Windows; 
uses System.Windows.Controls;
uses System.Windows.Controls.Primitives;
uses System.Windows.Media.Imaging;

var
  StatusBarPanel: StatusBar;
  LeftPanel, RightPanel, TopPanel, BottomPanel: Panel;

procedure AddPanel(var pp: StackPanel; wh: real; d: Dock; c: Color; Margin: real := 10);
begin
  if pp <> nil then
    exit;
  var bb := new Border();  
  bb.Background := new SolidColorBrush(c);
  var p := new StackPanel;
  bb.Child := p;
  //bb.Children.Add(p);
  p.Margin := new System.Windows.Thickness(Margin);
  if (d = Dock.Left) or (d = Dock.Right) then
  begin
    p.Orientation := Orientation.Vertical;
    bb.Width := wh;
  end
  else
  begin
    p.Orientation := Orientation.Horizontal;
    bb.Height := wh;
  end;
  p.Background := new SolidColorBrush(c);
  DockPanel.SetDock(bb, d);
  //MainDockPanel.children.Insert(MainDockPanel.children.Count-1,bb);
  MainDockPanel.children.Insert(0, bb);
  //MainDockPanel.children.Add(bb);
  pp := p;
  ActivePanel := p;
end;

procedure AddRightPanel(Width: real; c: Color; Margin: real) := Invoke(AddPanel, RightPanel, Width, Dock.Right, c, Margin);

procedure AddLeftPanel(Width: real; c: Color; Margin: real) := Invoke(AddPanel, LeftPanel, Width, Dock.Left, c, Margin);

procedure AddTopPanel(Height: real; c: Color; Margin: real) := Invoke(AddPanel, TopPanel, Height, Dock.Top, c, Margin);

procedure AddBottomPanel(Height: real; c: Color; Margin: real) := Invoke(AddPanel, BottomPanel, Height, Dock.Bottom, c, Margin);

procedure AddStatusBarP(Height: real);
begin
  if StatusBarPanel <> nil then
    exit;
  var sb := new StatusBar;
  sb.Height := 24;
  DockPanel.SetDock(sb, Dock.Bottom);
  // Всегда первая
  MainDockPanel.children.Insert(0, sb);
  StatusBarPanel := sb;
  {var sbi := new StatusBarItem();
  sbi.Content := 'sdghj';
  sb.Items.Add(sbi);
  sbi := new StatusBarItem();
  sbi.Content := '222';
  sb.Items.Add(sbi);}
end;

procedure AddStatusBar(Height: real) := Invoke(AddStatusBarP, Height);

function Button(Txt: string; fontSize: real): ButtonT := ButtonT.Create(Txt,fontSize);

function Button(x, y: integer; Txt: string; fontSize: real): ButtonT := ButtonT.Create(x, y, Txt,fontSize);

function TextBlock(Txt: string; fontsize: real): TextBlockT := TextBlockT.Create(Txt, fontsize);

function TextBlock(x, y: real; Txt: string; fontsize: real): TextBlockT := TextBlockT.Create(x, y, Txt, fontsize);

function IntegerBlock(message: string; fontsize: real; initValue: integer): IntegerBlockT := IntegerBlockT.Create(message, fontsize, initValue);

function RealBlock(message: string; fontsize: real; initValue: real): RealBlockT := RealBlockT.Create(message, fontsize, initValue);

function TextBox(Txt: string; w: real): TextBoxT := TextBoxT.Create(Txt, w);

function ListBox(w, h: real): ListBoxT := ListBoxT.Create(w, h);

function IntegerBox(w: real): IntegerBoxT := IntegerBoxT.Create(w);

function Slider(min, max, val: real): SliderT := SliderT.Create(min, max, val);

procedure EmptyBlock(sz: integer);
begin
  var e := TextBlock('');
  e.Height := sz;
  e.Width := sz;
end;

procedure SetActivePanelInit;
begin
  ActivePanel := MainWindow.MainPanel.Children[0] as Panel
end;

begin
  Invoke(SetActivePanelInit);
end.