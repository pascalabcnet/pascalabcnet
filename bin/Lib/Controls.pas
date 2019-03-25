// Copyright (©) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
///Модуль элементов управления для GraphWPF
unit Controls;

interface

uses GraphWPFBase;
uses System.Windows; 
uses System.Windows.Media; 
uses System.Windows.Controls; 

procedure AddRightPanel(Width: real := 200; c: Color := Colors.LightGray; Margin: real := 10);
procedure AddLeftPanel(Width: real := 200; c: Color := Colors.LightGray; Margin: real := 10);
procedure AddTopPanel(Height: real := 70; c: Color := Colors.LightGray; Margin: real := 10);
procedure AddBottomPanel(Height: real := 70; c: Color := Colors.LightGray; Margin: real := 10);
procedure AddStatusBar(Height: real := 24);

var 
  ActivePanel: Panel;
  GlobalMargin := 0;

type
  GButton = System.Windows.Controls.Button;
  GTextBlock = System.Windows.Controls.TextBlock;
  GTextBox = System.Windows.Controls.TextBox;
  Key = System.Windows.Input.Key;  
  ///!#
  CommonControl = class
  protected 
    element: FrameworkElement;
    function GetM: real := InvokeReal(()->element.Margin.Top);
    procedure SetMP(m: real) := element.Margin := new Thickness(m);
    procedure SetM(m: real) := Invoke(SetMP,m);
    function GetW: real := InvokeReal(()->element.Width);
    procedure SetWP(w: real) := element.Width := w;
    procedure SetW(w: real) := Invoke(SetWP,w);
    function GetH: real := InvokeReal(()->element.Height);
    procedure SetHP(h: real) := element.Height := h;
    procedure SetH(h: real) := Invoke(SetHP,h);
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
    procedure SetText(t: string) := Invoke(SetTextP,t);
    procedure CreateP(Txt: string);
    begin
      element := new GButton;
      element.Margin := new Thickness(0,0,0,8);
      //Margin := GlobalMargin;
      Text := Txt;
      b.Click += BClick;
      ActivePanel.Children.Add(b);
    end;
  public 
    event Click: procedure;
    constructor Create(Txt: string);
    begin
      Invoke(CreateP,Txt);
    end;
    property Text: string read GetText write SetText;
  end;
  
  ///!#
  TextLabelT = class(CommonControl)
  protected
    function b: GTextBlock := element as GTextBlock;
    procedure CreateP(Txt: string);
    begin
      var tb := new GTextBlock;
      element := tb;
      //element.Margin := new Thickness(0,0,0,8);
      //element.Margin := new Thickness(5,5,5,0);
      Text := Txt;
      ActivePanel.Children.Add(b);
    end;
    procedure CreatePXY(x,y: real; Txt: string);
    begin
      var tb := new GTextBlock;
      element := tb;
      tb.Background := new SolidColorBrush(Colors.White);
      tb.FontSize := 20;
      tb.Opacity := 0.7;
      Canvas.SetLeft(element,x);
      Canvas.SetTop(element,y);
      Text := Txt;
      ActivePanel.Children.Add(b);
    end;
  public 
    constructor Create(Txt: string) := Invoke(CreateP,Txt);
    constructor Create(x,y: real; Txt: string) := Invoke(CreatePXY,x,y,Txt);
    property Text: string read InvokeString(()->b.Text) write Invoke(procedure(t: string) -> b.Text := t,value);
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
    procedure SetText(t: string) := Invoke(SetTextP,t);
    procedure CreateP(Txt: string; w: real);
    begin
      element := new GTextBox;
      element.HorizontalAlignment := HorizontalAlignment.Stretch;
      element.Margin := new Thickness(0,0,0,8);
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
      Invoke(CreateP,Txt,w);
    end;
    property Text: string read GetText write SetText;
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
      inherited Create('0',w);
      var tb := element as GTextBox;
      tb.MouseWheel += procedure (o,e) -> begin
        if e.Delta>0 then
          SetValue(GetValue+1)
        else if e.Delta<0 then
          SetValue(GetValue-1)
      end;
      tb.KeyDown += procedure (o,e) -> begin
        if not ((e.Key>=Key.D0) and (e.Key<=Key.D9)) then
          e.Handled := True;
      end;
      {tb.TextInput += procedure (o,e) -> begin
        Print(e.Text);
        if not ((e.Key>=Key.D0) and (e.Key<=Key.D9)) then
          e.Handled := True;
      end;}
    end;
    property Value: integer read GetValue write SetValue;
  end;
  
  TextBoxWithLabelT = class(CommonControl)
  protected
    function tb: GTextBox := (element as StackPanel).Children[1] as GTextBox;
    procedure BTextChanged(sender: Object; e: TextChangedEventArgs);
    begin
      if Click <> nil then
        Click;
    end;
    function GetText: string := InvokeString(()->tb.Text);
    procedure SetTextP(t: string) := tb.Text := t;
    procedure SetText(t: string) := Invoke(SetTextP,t);
    procedure CreateP(LabelTxt,Txt: string; w: real);
    begin
      var sp := new StackPanel;
      element := sp;
      sp.Orientation := Orientation.Horizontal;
      sp.HorizontalAlignment := HorizontalAlignment.Stretch;
      var l := new TextBlock();
      l.Text := LabelTxt;
      sp.Children.Add(l);
      var tb := new GTextBox;
      tb.Width := 100;
      tb.VerticalAlignment := VerticalAlignment.Stretch;
      tb.Margin := new Thickness(0,0,0,8);
      sp.Children.Add(tb);
      Text := Txt;
      if w > 0 then
        Width := w;
      tb.TextChanged += BTextChanged;
      ActivePanel.Children.Add(sp);
    end;
  public 
    event Click: procedure;
    constructor Create(LabelTxt: string; Txt: string := ''; w: real := 0);
    begin
      Invoke(CreateP,LabelTxt,Txt,w);
    end;
    property Text: string read GetText write SetText;
  end;

  SliderT = class(CommonControl)
  private
    function sl: Slider := element as Slider;
    function GetMinimum: real := InvokeReal(()->sl.Minimum);
    procedure SetMinimumP(r: real) := sl.Minimum := r;
    procedure SetMinimum(r: real) := Invoke(SetMinimumP,r);
    function GetMaximum: real := InvokeReal(()->sl.Maximum);
    procedure SetMaximumP(r: real) := sl.Maximum := r;
    procedure SetMaximum(r: real) := Invoke(SetMaximumP,r);
    function GetValue: real := InvokeReal(()->sl.Value);
    procedure SetValueP(r: real) := sl.Value := r;
    procedure SetValue(r: real) := Invoke(SetValueP,r);
    function GetFrequency: real := InvokeReal(()->sl.TickFrequency);
    procedure SetFrequencyP(r: real) := sl.TickFrequency := r;
    procedure SetFrequency(r: real) := Invoke(SetFrequencyP,r);
  protected
    procedure CreateP(min,max,val: real);
    begin
      element := new Slider;
      sl.ValueChanged += procedure(o,e) -> ValueChangedP;
      sl.TickPlacement := System.Windows.Controls.Primitives.TickPlacement.BottomRight;
      sl.Minimum := min;
      sl.Maximum := max;
      sl.Value := val;
      ActivePanel.Children.Add(sl);
    end;
    procedure ValueChangedP := if ValueChanged<>nil then ValueChanged;
  public 
    event ValueChanged: procedure;
    constructor Create(min,max,val: real);
    begin
      Invoke(CreateP,min,max,val);
    end;
    property Minimum: real read GetMinimum write SetMinimum;
    property Maximum: real read GetMaximum write SetMaximum;
    property Value: real read GetValue write SetValue;
    property Frequency: real read GetFrequency write SetFrequency;
  end;

  
function Button(Txt: string): ButtonT;
function TextLabel(Txt: string): TextLabelT;
function TextLabel(x,y: real; Txt: string): TextLabelT;
function TextBox(Txt: string := ''; w: real := 0): TextBoxT;
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
  LeftPanel,RightPanel,TopPanel,BottomPanel: Panel;

procedure AddPanel(var pp: StackPanel; wh: real; d: Dock; c: Color; Margin: real := 10);
begin
  if pp<>nil then
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
  DockPanel.SetDock(bb,d);
  // Всегда добавлять предпоследним
  MainDockPanel.children.Insert(MainDockPanel.children.Count-1,bb);
  pp := p;
  ActivePanel := p;
end;

procedure AddRightPanel(Width: real; c: Color; Margin: real) := Invoke(AddPanel,RightPanel,Width,Dock.Right,c,Margin);
procedure AddLeftPanel(Width: real; c: Color; Margin: real) := Invoke(AddPanel,LeftPanel,Width,Dock.Left,c,Margin);
procedure AddTopPanel(Height: real; c: Color; Margin: real) := Invoke(AddPanel,TopPanel,Height,Dock.Top,c,Margin);
procedure AddBottomPanel(Height: real; c: Color; Margin: real) := Invoke(AddPanel,BottomPanel,Height,Dock.Bottom,c,Margin);

procedure AddStatusBarP(Height: real);
begin
  if StatusBarPanel<>nil then
    exit;
  var sb := new StatusBar;
  sb.Height := 24;
  DockPanel.SetDock(sb,Dock.Bottom);
  // Всегда первая
  MainDockPanel.children.Insert(0,sb);
  StatusBarPanel := sb;
  {var sbi := new StatusBarItem();
  sbi.Content := 'sdghj';
  sb.Items.Add(sbi);
  sbi := new StatusBarItem();
  sbi.Content := '222';
  sb.Items.Add(sbi);}
end;
procedure AddStatusBar(Height: real) := Invoke(AddStatusBarP,Height);

function Button(Txt: string): ButtonT := ButtonT.Create(Txt);
function TextLabel(Txt: string): TextLabelT := TextLabelT.Create(Txt);
function TextLabel(x,y: real; Txt: string): TextLabelT := TextLabelT.Create(x,y,Txt);
function TextBox(Txt: string; w: real): TextBoxT := TextBoxT.Create(Txt,w);
function IntegerBox(w: real): IntegerBoxT := IntegerBoxT.Create(w);
function Slider(min,max,val: real): SliderT := SliderT.Create(min,max,val);

procedure EmptyBlock(sz: integer);
begin
  var e := TextLabel('');
  e.Height:= sz;
  e.Width:= sz;
end;

procedure SetActivePanelInit;
begin
  ActivePanel := MainWindow.MainPanel.Children[0] as Panel
end;

begin
  Invoke(SetActivePanelInit);
end.