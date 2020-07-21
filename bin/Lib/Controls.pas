// Copyright (©) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
///Модуль элементов управления для GraphWPF и WPFObjects
unit Controls;

interface

uses GraphWPFBase,GraphWPF;
uses System.Windows; 
uses System.Windows.Media; 
uses System.Windows.Media.Imaging;
uses System.Windows.Controls; 
uses System.Windows.Controls.Primitives; 
uses System.ComponentModel;

const PanelsColor = Colors.WhiteSmoke;

type
  Dock = System.Windows.Controls.Dock;
  GButton = System.Windows.Controls.Button;
  GTextBlock = System.Windows.Controls.TextBlock;
  GTextBox = System.Windows.Controls.TextBox;
  GListBox = System.Windows.Controls.ListBox;
  GPanel = System.Windows.Controls.Panel;
  GControl = System.Windows.Controls.Control;
  GStatusBar = System.Windows.Controls.Primitives.StatusBar;
  Colors = System.Windows.Media.Colors;
  Key = System.Windows.Input.Key;

var
  ActivePanel: GPanel;
  GlobalHMargin := 12;

type
  ///!#
  PanelWPF = class
  protected
    p: StackPanel;
    bb: Border;
    procedure CreateP(wh: real; d: Dock; c: Color; InternalMargin: real);
    begin
      bb := new Border();  
      bb.Background := new SolidColorBrush(c);
      p := new StackPanel;
      bb.Child := p;
      p.Background := GetBrush(c);
      p.Margin := new System.Windows.Thickness(InternalMargin);

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
      DockPanel.SetDock(bb, d);
      MainDockPanel.children.Insert(0, bb);
      ActivePanel := p;
    end;
    procedure SetColorP(c: GColor);
    begin
      p.Background := GetBrush(c); 
      bb.Background := p.Background
    end;
    function GetColorP := (p.Background as SolidColorBrush).Color;
  public 
    constructor Create(wh: real := 150; d: Dock := Dock.Left; c: Color := PanelsColor; internalMargin: real := 10);
    begin
      Invoke(CreateP,wh,d,c,internalMargin);
    end;
    property Color: GColor read Invoke&<GColor>(GetColorP) write Invoke(SetColorP, value);
    property InternalMargin: real 
      read InvokeReal(()->p.Margin.Left)
      write Invoke(procedure -> p.Margin := new System.Windows.Thickness(internalMargin), value);
  end;
  
  LeftPanelWPF = class(PanelWPF)
  public
    constructor Create(width: real := 150; c: GColor := PanelsColor; internalMargin: real := 10);
    begin
      inherited Create(width, Dock.Left,c,internalMargin)
    end;
  end;

  RightPanelWPF = class(PanelWPF)
  public
    constructor Create(width: real := 150; c: GColor := PanelsColor; internalMargin: real := 10);
    begin
      inherited Create(width, Dock.Right,c,internalMargin)
    end;
  end;

  ///!#
  CommonElementWPF = class
  protected 
    element: FrameworkElement;
    procedure Init0(el: FrameworkElement; width: real := 0; x: real := -1; y: real := -1);
    begin
      element := el;
      element.Focusable := False;
      if ActivePanel is StackPanel(var sp) then
        if sp.Orientation = Orientation.Vertical then
          element.Margin := new Thickness(0,0,0,GlobalHMargin);
        //else element.Margin := new Thickness(0,0,GlobalHMargin,0);
      if x<>-1 then
      begin
        element.SetLeft(x);
        element.SetTop(y);
      end;
      if width>0 then
        element.Width := width;
      ActivePanel.Children.Add(element);
    end;

    procedure SetWP(w: real) := element.Width := w;
    procedure SetHP(h: real) := element.Height := h;
    procedure SetMP(m: real) := element.Margin := new Thickness(0,0,0,m);
    procedure SetEP(enabled: boolean) := element.IsEnabled := enabled;
  public 
    property Width: real read InvokeReal(()->element.Width) write Invoke(SetWP, value);
    property Height: real read InvokeReal(()->element.Height) write Invoke(SetHP, value);
    property Margin: real read InvokeReal(()->element.Margin.Bottom) write Invoke(SetMP, value);
    property Enabled: boolean read InvokeBoolean(()->element.IsEnabled) write Invoke(SetEP, value);
  end;
  
  CommonControlWPF = class(CommonElementWPF)
  private
    procedure SetFSzP(size: real) := control.FontSize := size;
  public  
    property Control: GControl read element as GControl;
    property FontSize: real read InvokeReal(()->control.FontSize) write Invoke(SetFSzP, value);
  end;

  ///!#
  ClickableControlWPF = class(CommonControlWPF)
  protected
    procedure ClickP(sender: Object; e: RoutedEventArgs);
    begin
      if Click <> nil then
        Click;
    end;
  public  
    Click: procedure;
  end;

  ///!#
  ImageWPF = class(CommonControlWPF)
  protected
    property im: Image read element as Image;  
    procedure CreateP(name: string);
    begin
      Init0(new Image());
      im.Source := new BitmapImage();
      im.Source := new BitmapImage(new System.Uri(name));
    end;
    procedure CreatePP;
    begin
      element := new Image();
      element.Focusable := False;
      MainDockPanel.children.RemoveAt(MainDockPanel.children.Count-1);
      MainDockPanel.children.Add(im);
    end;
  private
    function GetNameP: string;
    begin
      Result := (im.Source as BitmapImage).UriSource.AbsolutePath
    end;
    procedure SetNameP(value: string);
    begin
      im.Source := new BitmapImage(new System.Uri(value));
    end;
    constructor (i: integer) := Invoke(CreatePP); // фиктивный первый параметр
  public 
    constructor (name: string) := Invoke(CreateP,name);
    property Name: string read InvokeString(GetNameP) write Invoke(SetNameP,value);
  end;

  ///!#
  ButtonWPF = class(CommonControlWPF)
  protected
    property b: GButton read element as GButton;
    
    procedure CreatePXY(x,y: real; Txt: string; width: real; fontSize: real);
    begin
      Init0(new GButton,width,x,y);
      Text := Txt;
      Self.FontSize := fontSize;
      b.Click += (o,e) -> if Click <> nil then Click;
    end;
    procedure CreateP(text: string; fontSize: real) := CreatePXY(-1,-1,text,0,fontSize);

    procedure SetTextP(t: string) := b.Content := t;
  public 
    Click: procedure;
    constructor Create(Txt: string; fontSize: real := 12) := Invoke(CreateP, Txt, fontSize);
    constructor Create(x, y: real; Txt: string; width: real := 0; fontSize: real := 12) := Invoke(CreatePXY, x, y, Txt, width, fontSize);
    
    property Text: string read InvokeString(()->b.Content as string) write Invoke(SetTextP, value);
  end;
  
  ///!#
  TextBlockWPF = class(CommonElementWPF)
  protected
    property tb: GTextBlock read element as GTextBlock;

    procedure CreatePXY(x, y: real; Txt: string; width: real; fontsize: real);
    begin
      Init0(new GTextBlock, width, x, y);
      Margin := 3;
      Text := Txt;
      Self.FontSize := fontSize;
    end;
    procedure CreateP(text: string; fontSize: real) := CreatePXY(-1,-1,text,0,fontSize);
  
    procedure SetTextP(t: string) := tb.Text := t;
    procedure SetFontSizeP(sz: real) := tb.FontSize := sz;
  public 
    constructor Create(Txt: string; fontsize: real := 12):= Invoke(CreateP, Txt, fontsize);
    constructor Create(x, y: real; Txt: string; width: real := 0; fontSize: real := 12):= Invoke(CreatePXY, x, y, Txt, width, fontSize);

    property Text: string read InvokeString(()->tb.Text) write Invoke(SetTextP, value);
    property FontSize: real read InvokeReal(()->tb.FontSize) write Invoke(SetFontSizeP, value);
  end;

  IntegerBlockWPF = class(TextBlockWPF)
  private 
    val := 0;
    message: string;
    procedure CreatePXY(x, y: real; message: string; width: real; initValue: integer := 0; fontSize: real := 12);
    begin
      inherited CreatePXY(x,y,message + ' ' + initValue, width, fontSize);
      Self.message := message;
      val := initValue;
    end;
    procedure CreateP(message: string; initValue: integer := 0; fontSize: real := 12) := CreatePXY(-1,-1,message,0,initValue,fontSize);
  public 
    constructor Create(message: string; initValue: integer := 0; fontSize: real := 12) := Invoke(CreateP, message, initValue, fontSize);
    constructor Create(x, y: real; message: string; width: real := 0; initValue: integer := 0; fontSize: real := 12) := Invoke(CreatePXY, x, y, message, width, initValue, fontSize);

    property Value: integer read val write begin val := value; Text := message + ' ' + val; end;
  end;

  RealBlockWPF = class(TextBlockWPF)
  private 
    val := 0.0;
    message: string;
    procedure CreatePXY(x, y: real; message: string; width: real; initValue: real := 0; fontSize: real := 12);
    begin
      inherited CreatePXY(x,y,message + ' ' + initValue, width, fontSize);
      Self.message := message;
      val := initValue;
    end;
    procedure CreateP(message: string; initValue: real := 0; fontSize: real := 12) := CreatePXY(-1,-1,message,0,initValue,fontSize);
  public 
    constructor Create(message: string; initValue: real := 0; fontSize: real := 12) := Invoke(CreateP, message, initValue, fontSize);
    constructor Create(x, y: real; message: string; width: real := 0; initValue: real := 0; fontSize: real := 12) := Invoke(CreatePXY, x, y, message, width, initValue, fontSize);

    property Value: real read val write begin val := value; Text := message + ' ' + val.ToString(FracDigits); end;
    auto property FracDigits: integer := 1;
  end;

  ///!#
  TextBoxWPF = class(CommonControlWPF)
  protected
    property tb: GTextBox read element as GTextBox;
    
    procedure SetTextP(t: string) := tb.Text := t;
    
    procedure CreatePXY(x,y: real; txt: string; width: real);
    begin
      Init0(new GTextBox,width,x,y);
      Text := txt;
      tb.TextChanged += (o,e) -> if Click <> nil then Click;
      element.Focusable := True;
      //element.HorizontalAlignment := HorizontalAlignment.Stretch;
    end;
    procedure CreateP(text: string) := CreatePXY(-1,-1,text,0);
  public 
    Click: procedure;
    constructor Create(text: string := '') := Invoke(CreateP, text);
    constructor Create(x,y: real; text: string := ''; width: real := 0) := Invoke(CreatePXY, x, y, text, width);
    
    property Text: string read InvokeString(()->tb.Text) write Invoke(SetTextP, value);
  end;

  IntegerBoxWPF = class(TextBoxWPF)
    function GetValue: integer;
    begin
      if Trim(Text) = '' then
        Result := 0
      else Result := integer.Parse(Text);
      Result := Result.Clamp(Min,Max);
    end;
    
    procedure SetValue(x: integer) := Text := x.Clamp(Min,Max).ToString;
    procedure Rest(min,max:integer);
    begin
      Self.Min := min;
      Self.Max := max;
      Value := Min;
      tb.MouseWheel += (o, e) -> begin
        if e.Delta > 0 then
          Value := Value + 1
        else if e.Delta < 0 then
          Value := Value - 1
      end;
      tb.KeyDown += (o, e) -> begin
        if not ((e.Key >= Key.D0) and (e.Key <= Key.D9)) then
          e.Handled := True;
      end;
    end;
  public 
    constructor Create(min: integer := 0; max: integer := 10);
    begin
      inherited Create('');
      Rest(min,max);
    end;
    constructor Create(x,y: real; min,max: integer; width: real := 0);
    begin
      inherited Create(x,y,'',width);
      Rest(min,max);
    end;
    
    auto property Min: integer := 0;
    auto property Max: integer := 100;
    
    property Value: integer read GetValue write SetValue;
  end;

  {TextBoxWithBlockWPF = class(CommonControlWPF)
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
    Click: procedure;
    constructor Create(blockTxt: string; Txt: string := ''; w: real := 0);
    begin
      Invoke(CreateP, blockTxt, Txt, w);
    end;
    
    property Text: string read GetText write SetText;
    property FontSize: real read InvokeReal(()->tb.FontSize) write Invoke(procedure(t: real) -> begin tb.FontSize := t; l.FontSize := t; end, value);
  end;}

  ///!#
  ListBoxWPF = class(CommonControlWPF)
  protected
    function lb: GListBox := element as GListBox;
    procedure CreatePXY(x,y,w,h: real);
    begin
      Init0(new GListBox,w,x,y);
      //element.HorizontalAlignment := HorizontalAlignment.Stretch;
      Height := h;
      lb.SelectionChanged += (o,e) -> if SelectionChanged<>nil then SelectionChanged;
    end;
    procedure CreateP(h: real) := CreatePXY(-1,-1,0,h);
    
    procedure AddP(s: string);
    begin
      var lbi := new ListBoxItem();
      lbi.Content := s;
      lb.Items.Add(lbi);
    end;  

    procedure AddRangeP(ss: sequence of string);
    begin
      foreach var s in ss do
        AddP(s);  
    end;  
    
    procedure SortP := lb.Items.SortDescriptions.Add(new SortDescription('Content', ListSortDirection.Ascending));
    procedure SortPDescending := lb.Items.SortDescriptions.Add(new SortDescription('Content', ListSortDirection.Descending));
  public 
    SelectionChanged: procedure;
    constructor Create(height: real := 150):= Invoke(CreateP, height);
    constructor Create(x,y: real; width: real; height: real := 150):= Invoke(CreatePXY, x, y, width, height);
    procedure Sort := Invoke(SortP);
    procedure SortDescending := Invoke(SortPDescending);
    procedure Add(s: string) := Invoke(AddP, s);
    procedure AddRange(ss: sequence of string) := Invoke(AddRangeP, ss);
    property Count: integer read InvokeInteger(()->lb.Items.Count);
    property SelectedIndex: integer read InvokeInteger(()->lb.SelectedIndex) write Invoke(procedure(t: integer) -> lb.SelectedIndex := t, value);
    property SelectedText: string read InvokeString(()->(lb.SelectedItem as ListBoxItem).Content.ToString) 
      write Invoke(procedure(t: string) -> (lb.SelectedItem as ListBoxItem).Content := t, value);
  end;

  SliderWPF = class(CommonControlWPF)
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
    procedure CreatePXY(x,y: real; min, max, val, freq: real; width: real);
    begin
      Init0(new Slider,width,x,y);
      sl.ValueChanged += procedure(o, e) -> ValueChangedP;
      sl.TickPlacement := System.Windows.Controls.Primitives.TickPlacement.BottomRight;
      sl.Minimum := min;
      sl.Maximum := max;
      sl.Value := val;
      if freq<=0 then
        sl.TickFrequency := (max-min)/10
      else sl.TickFrequency := freq;
      sl.Foreground := Brushes.Black;
    end;
    procedure CreateP(min, max, val, freq: real) := CreatePXY(-1,-1,min, max, val, freq, 0);
    
    procedure ValueChangedP := if ValueChanged <> nil then ValueChanged;
  public 
    ValueChanged: procedure;
    constructor Create(min, max, val: real; freq: real := 0) := Invoke(CreateP, min, max, val, freq);
    constructor Create(x,y: real; min, max, val: real; freq: real := 0; width: real := 0) := Invoke(CreatePXY, x,y,min, max, val, freq, width);
    property Minimum: real read GetMinimum write SetMinimum;
    property Maximum: real read GetMaximum write SetMaximum;
    property Value: real read GetValue write SetValue;
    property Frequency: real read GetFrequency write SetFrequency;
  end;
  
  CheckBoxWPF = class(ClickableControlWPF)
  private
    property cb: CheckBox read element as CheckBox;
  protected 
    procedure CreatePXY(x,y: real; text: string; width: real);
    begin
      Init0(new CheckBox,width,x,y);
      cb.Content := text;
      cb.Click += ClickP;
    end;
    procedure CreateP(text: string) := CreatePXY(-1,-1,text,0);
  public 
    constructor Create(text: string) := Invoke(CreateP, text);
    constructor Create(x,y: real; text: string; width: real := 0) := Invoke(CreatePXY, x,y,text,width);
    
    property Text: string read InvokeString(()->cb.Content as string) write Invoke(procedure(t: string) -> cb.Content := t, value);
    property IsChecked: boolean read InvokeBoolean(()->cb.IsChecked.Value) write Invoke(procedure(t: boolean) -> cb.IsChecked := t, value);
  end;

  RadioButtonWPF = class(ClickableControlWPF)
  private
    property rb: RadioButton read element as RadioButton;
  protected 
    procedure CreatePXY(x,y: real; text: string; width: real);
    begin
      Init0(new RadioButton,width,x,y);
      rb.Content := text;
      rb.Click += ClickP;
    end;
    procedure CreateP(text: string) := CreatePXY(-1,-1,text,0);
  public 
    constructor Create(text: string) := Invoke(CreateP, text);
    constructor Create(x,y: real; text: string; width: real := 0) := Invoke(CreatePXY, x,y,text,width);

    property Text: string read InvokeString(()->rb.Content as string) write Invoke(procedure(t: string) -> rb.Content := t, value);
    property IsChecked: boolean read InvokeBoolean(()->rb.IsChecked.Value) write Invoke(procedure(t: boolean) -> rb.IsChecked := t, value);
  end;

  StatusBarWPF = class(CommonControlWPF)
  private
    property sb: GStatusBar read element as GStatusBar;
    procedure SetTextP(text: string) := (sb.Items[0] as StatusBarItem).Content := text;
    procedure SetTextPI(i: integer; text: string) := (sb.Items[i] as StatusBarItem).Content := text;
  protected 
    procedure CreateP(height: real; itemWidth: real);
    begin
      element := new GStatusBar;
      element.Focusable := False;
      element.Height := height;
      DockPanel.SetDock(sb, Dock.Bottom);
      MainDockPanel.children.Insert(0, sb);
      
      var sbi := new StatusBarItem();
      sbi.Content := '';
      if itemWidth > 0 then 
        sbi.Width := itemWidth;
      sb.Items.Add(sbi);
      
      // последняя фиктивная панель - для заполнения
      sbi := new StatusBarItem();
      sbi.Content := '';
      sb.Items.Add(sbi);
    end;
    procedure AddItemP(text: string; itemWidth: real);
    begin
      var sbi := new StatusBarItem();
      sbi.Content := text;
      if itemWidth > 0 then 
        sbi.Width := itemWidth;
      sb.Items.Insert(sb.Items.Count-1,sbi);
    end;
  public
    constructor Create(height: real := 24; itemWidth: real := 0) := Invoke(CreateP,height,itemWidth);
    procedure AddText(text: string; itemWidth: real := 0) := Invoke(AddItemP,text,itemWidth);

    property Text: string read InvokeString(()->(sb.Items[0] as StatusBarItem).Content as string) 
      write Invoke(SetTextP,value);
    property ItemText[i: integer]: string read InvokeString(()->(sb.Items[0] as StatusBarItem).Content as string)
      write Invoke(SetTextPI,i,value);
  end;
  
  SetMainControl = static class
  public  
    static function AsImage: ImageWPF := new ImageWPF(0);
  end;
  
  
  {WebBrowserWPF = class(CommonControlWPF)
  private
    property wb: WebBrowser read element as WebBrowser;
    procedure CreatePXY(x,y,w,h: real);
    begin
      element := new WebBrowser;
      //element.Focusable := False;
      //DockPanel.SetDock(wb, Dock.Bottom);
      MainDockPanel.children.Insert(0, wb);

      //wb.Height := h;
      wb.Navigate('http://yandex.ru');
      //wb.Source := new System.Uri('http://yandex.ru',System.UriKind.Absolute);
    end;
  public
    constructor Create(x,y,w,h: real) := Invoke(CreatePXY,x,y,w,h);
  end;}

{-----------------------------------------------------}
// Для каждого контрола - пара функций. 
// Функция без координат и ширины рассчитана на создание внутри вертикальной StackPanel (LeftPanel, RightPanel)
// Panel - вспомогательная, нужна только для создания LeftPanel, RightPanel

function Button(Txt: string; fontSize: real := 12): ButtonWPF;
function Button(x, y: real; Txt: string; width: real := 0; fontSize: real := 12): ButtonWPF;

function TextBlock(Txt: string; fontSize: real := 12): TextBlockWPF;
function TextBlock(x, y: real; Txt: string; width: real := 0; fontSize: real := 12): TextBlockWPF;

function IntegerBlock(message: string; initValue: integer := 0; fontSize: real := 12): IntegerBlockWPF;
function IntegerBlock(x, y: real; message: string; width: real := 0; initValue: integer := 0; fontSize: real := 12): IntegerBlockWPF;

function RealBlock(message: string; initValue: real := 0; fontSize: real := 12): RealBlockWPF;
function RealBlock(x, y: real; message: string; width: real := 0; initValue: real := 0; fontSize: real := 12): RealBlockWPF;

function TextBox(text: string := ''): TextBoxWPF;
function TextBox(x, y: real; text: string := ''; width: real := 0): TextBoxWPF;

function IntegerBox(max: integer := 10): IntegerBoxWPF;
function IntegerBox(min,max: integer): IntegerBoxWPF;
function IntegerBox(x, y: real; min,max: integer; width: real := 0): IntegerBoxWPF;

function ListBox(height: real := 150): ListBoxWPF;
function ListBox(x,y: real; width: real; height: real := 150): ListBoxWPF;

function Slider(): SliderWPF;
function Slider(min,max: real; val: real := real.MinValue; freq: real := 0): SliderWPF;
function Slider(x,y: real; min,max: real; width: real := 0; val: real := real.MinValue; freq: real := 0): SliderWPF;

function LeftPanel(width: real := 150; c: Color := PanelsColor; InternalMargin: real := 10): PanelWPF;
function RightPanel(width: real := 150; c: Color := PanelsColor; InternalMargin: real := 10): PanelWPF;

function StatusBar(height: real := 24; itemWidth: real := 0): StatusBarWPF;


procedure EmptyBlock(sz: integer := 16);

function Window: WindowTypeWPF;

implementation

uses GraphWPF;
uses System.Windows; 
uses System.Windows.Controls;
uses System.Windows.Controls.Primitives;
uses System.Windows.Media.Imaging;

function Window: WindowTypeWPF := GraphWPF.Window;

procedure AddStatusBarP(Height: real);
begin
  {var sb := new StatusBar;
  sb.Height := Height;
  DockPanel.SetDock(sb, Dock.Bottom);
  // Всегда первая
  MainDockPanel.children.Insert(0, sb);}
  //StatusBarPanel := sb;
  {var sbi := new StatusBarItem();
  sbi.Content := 'sdghj';
  sb.Items.Add(sbi);
  sbi := new StatusBarItem();
  sbi.Content := '222';
  sb.Items.Add(sbi);}
end;

function Button(Txt: string; fontSize: real): ButtonWPF := ButtonWPF.Create(Txt,fontSize);
function Button(x, y: real; Txt: string; width: real; fontSize: real): ButtonWPF := ButtonWPF.Create(x, y, Txt, width, fontSize);

function TextBlock(Txt: string; fontSize: real): TextBlockWPF := TextBlockWPF.Create(Txt, fontSize);
function TextBlock(x, y: real; Txt: string; width: real; fontSize: real): TextBlockWPF := TextBlockWPF.Create(x, y, Txt, width, fontSize);

function IntegerBlock(message: string; initValue: integer; fontSize: real): IntegerBlockWPF := IntegerBlockWPF.Create(message, initValue, fontSize);
function IntegerBlock(x, y: real; message: string; width: real; initValue: integer; fontSize: real): IntegerBlockWPF := IntegerBlockWPF.Create(x,y,message,width,initValue,fontSize);

function RealBlock(message: string; initValue: real; fontSize: real): RealBlockWPF := RealBlockWPF.Create(message, initValue, fontSize);
function RealBlock(x, y: real; message: string; width: real; initValue: real; fontSize: real): RealBlockWPF := RealBlockWPF.Create(x,y,message,width,initValue,fontSize);

function TextBox(text: string): TextBoxWPF := TextBoxWPF.Create(text);
function TextBox(x, y: real; text: string; width: real): TextBoxWPF := TextBoxWPF.Create(x,y,text,width);

function IntegerBox(max: integer): IntegerBoxWPF := IntegerBoxWPF.Create(0,max);
function IntegerBox(min,max: integer): IntegerBoxWPF := IntegerBoxWPF.Create(min,max);
function IntegerBox(x, y: real; min,max: integer; width: real) := IntegerBoxWPF.Create(x,y,min,max,width);

function ListBox(height: real): ListBoxWPF := ListBoxWPF.Create(height);
function ListBox(x,y: real; width: real; height: real): ListBoxWPF := ListBoxWPF.Create(x,y,width,height);

function Slider: SliderWPF := SliderWPF.Create(0, 10, 0, 0);
function Slider(min, max, val, freq: real): SliderWPF;
begin
  if val = real.MinValue then
    val := min;
  Result := SliderWPF.Create(min, max, val, freq);
end;  
function Slider(x,y,min,max,width,val,freq: real): SliderWPF;
begin
  if val = real.MinValue then
    val := min;
  Result := SliderWPF.Create(x, y, min, max, val, freq, width);
end;  

function LeftPanel(width: real; c: Color; internalMargin: real) := new LeftPanelWPF(width,c,internalMargin);
function RightPanel(width: real; c: Color; internalMargin: real) := new RightPanelWPF(width,c,internalMargin);

function StatusBar(height: real; itemWidth: real): StatusBarWPF := new StatusBarWPF(height);


procedure EmptyBlock(sz: integer);
begin
  var e := TextBlock('');
  e.Height := sz;
  e.Width := sz;
end;

procedure SetActivePanelInit;
begin
  ActivePanel := MainWindow.MainPanel.Children[0] as GPanel
end;

begin
  Invoke(SetActivePanelInit);
end.