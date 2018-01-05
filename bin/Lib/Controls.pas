// Copyright (©) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
///Модуль элементов управления для GraphWPF
unit Controls;

interface

uses GraphWPF;
uses System.Windows; 
uses System.Windows.Media; 
uses System.Windows.Controls; 

procedure AddRightPanel(Width: real := 200; c: Color := Colors.LightGray);
procedure AddLeftPanel(Width: real := 200; c: Color := Colors.LightGray);
procedure AddTopPanel(Height: real := 70; c: Color := Colors.LightGray);
procedure AddBottomPanel(Height: real := 70; c: Color := Colors.LightGray);
procedure AddStatusBar(Height: real := 24);

var 
  ActivePanel: Panel;
  GlobalMargin := 5;

type
  GButton = System.Windows.Controls.Button;
  GTextBlock = System.Windows.Controls.TextBlock;
  GTextBox = System.Windows.Controls.TextBox;
  
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
  Button = class(CommonControl)
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
      Margin := GlobalMargin;
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
  TextBlock = class(CommonControl)
  protected
    function b: GTextBlock := element as GTextBlock;
    function GetText: string := InvokeString(()->b.Text);
    procedure SetTextP(t: string) := b.Text := t;
    procedure SetText(t: string) := Invoke(SetTextP,t);
    procedure CreateP(Txt: string);
    begin
      element := new GTextBlock;
      element.Margin := new Thickness(5,5,5,0);
      Text := Txt;
      ActivePanel.Children.Add(b);
    end;
  public 
    constructor Create(Txt: string);
    begin
      Invoke(CreateP,Txt);
    end;
    property Text: string read GetText write SetText;
  end;
  ///!#
  TextBox = class(CommonControl)
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
    procedure CreateP(Txt: string);
    begin
      element := new GTextBox;
      element.HorizontalAlignment := HorizontalAlignment.Left;
      element.Margin := new Thickness(5,0,5,5);
      //Margin := GlobalMargin;
      Text := Txt;
      tb.TextChanged += BTextChanged;
      ActivePanel.Children.Add(tb);
    end;
  public 
    event Click: procedure;
    constructor Create(Txt: string);
    begin
      Invoke(CreateP,Txt);
    end;
    property Text: string read GetText write SetText;
  end;


implementation

uses System.Windows; 
uses System.Windows.Controls;
uses System.Windows.Controls.Primitives;
uses System.Windows.Media.Imaging;

var 
  StatusBarPanel: StatusBar;
  LeftPanel,RightPanel,TopPanel,BottomPanel: Panel;

procedure AddPanel(var pp: StackPanel; wh: real; d: Dock; c: Color);
begin
  if pp<>nil then
    exit;
  var p := new StackPanel;
  if (d = Dock.Left) or (d = Dock.Right) then
  begin
    p.Orientation := Orientation.Vertical;
    p.Width := wh;
  end
  else
  begin
    p.Orientation := Orientation.Horizontal;
    p.Height := wh;
  end;
  p.Background := new SolidColorBrush(c);
  DockPanel.SetDock(p,d);
  // Всегда добавлять предпоследним
  MainDockPanel.children.Insert(MainDockPanel.children.Count-1,p);
  pp := p;
  ActivePanel := p;
end;

procedure AddRightPanel(Width: real; c: Color) := Invoke(AddPanel,RightPanel,Width,Dock.Right,c);
procedure AddLeftPanel(Width: real; c: Color) := Invoke(AddPanel,LeftPanel,Width,Dock.Left,c);
procedure AddTopPanel(Height: real; c: Color) := Invoke(AddPanel,TopPanel,Height,Dock.Top,c);
procedure AddBottomPanel(Height: real; c: Color) := Invoke(AddPanel,BottomPanel,Height,Dock.Bottom,c);

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

begin
end.