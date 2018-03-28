// Copyright (©) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
///Модуль элементов управления для GraphWPF
unit Controls;

interface

uses GraphWPFBase;
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
  GlobalMargin := 0;

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
  TextBlockT = class(CommonControl)
  protected
    function b: GTextBlock := element as GTextBlock;
    function GetText: string := InvokeString(()->b.Text);
    procedure SetTextP(t: string) := b.Text := t;
    procedure SetText(t: string) := Invoke(SetTextP,t);
    procedure CreateP(Txt: string);
    begin
      element := new GTextBlock;
      //element.Margin := new Thickness(0,0,0,8);
      //element.Margin := new Thickness(5,5,5,0);
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
      inherited Create('',w);
      var tb := element as GTextBox;
      tb.KeyDown += procedure (o,e) -> begin
        var s := e.Key.ToString;
        if (s[1] = 'D') and (s.Length = 2) then
        else
          e.Handled := True;
        {if (e. = #8) or (e.Key = '-') then 
          exit;}
        //if not Char.IsDigit(e.Key) then
        //  e.Handled := True;
      end;
      tb.TextInput += procedure (o,e) -> begin
        Print(e.Text);
        {if (e. = #8) or (e.Key = '-') then 
          exit;}
        //if not Char.IsDigit(e.Key) then
        //  e.Handled := True;
      end;
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
  
  function Button(Txt: string): ButtonT;
  function TextBlock(Txt: string): TextBlockT;
  function TextBox(Txt: string := ''; w: real := 0): TextBoxT;
  function IntegerBox(w: real := 0): IntegerBoxT;
  
  procedure EmptyBlock(sz: integer := 16);

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
  var bb := new Border();  
  bb.Background := new SolidColorBrush(c);
  var p := new StackPanel;
  bb.Child := p;
  //bb.Children.Add(p);
  p.Margin := new System.Windows.Thickness(20);
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
  DockPanel.SetDock(p,d);
  // Всегда добавлять предпоследним
  MainDockPanel.children.Insert(MainDockPanel.children.Count-1,bb);
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

function Button(Txt: string): ButtonT := ButtonT.Create(Txt);
function TextBlock(Txt: string): TextBlockT := TextBlockT.Create(Txt);
function TextBox(Txt: string; w: real): TextBoxT := TextBoxT.Create(Txt,w);
function IntegerBox(w: real): IntegerBoxT := IntegerBoxT.Create(w);

procedure EmptyBlock(sz: integer);
begin
  var e := TextBlock('');
  e.Height:= sz;
  e.Width:= sz;
end;


begin
end.