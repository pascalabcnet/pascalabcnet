// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
unit FormsABC;

{$apptype windows} 
{$reference 'System.Windows.Forms.dll'}
{$reference 'System.Drawing.dll'}

interface

uses
  System,
  System.Windows.Forms,
  System.Drawing,
  System.Collections.Generic;

type
  DockStyle = System.Windows.Forms.DockStyle;
  DialogResult = System.Windows.Forms.DialogResult;
  BorderStyle = System.Windows.Forms.BorderStyle;
  FlowDirection = System.Windows.Forms.FlowDirection;
  Color = System.Drawing.Color;
  
  
  /// Поле ввода
  Field = class
  private 
    l: &Label;
    f: System.Windows.Forms.TextBox;
    p: Panel;
    function GetRealValue: real;
    procedure SetRealValue(x: real);
    function GetFieldWidth: integer;
    procedure SetFieldWidth(x: integer);
    function GetText: string;
    procedure SetText(s: string); virtual;
    procedure TBTextChanged(sender: Object; e: EventArgs);
  public 
    event TextChanged: procedure;
    constructor Create(name: string; w: integer := 0);
    property Text: string read GetText write SetText;
    property FieldWidth: integer read GetFieldWidth write SetFieldWidth;
  end;
  
  /// Поле ввода целых значений
  IntegerField = class(Field)
  private 
    nonNumberEntered: boolean;
    function GetValue: integer;
    procedure SetValue(x: integer);
    procedure fKeyPressed(sender: Object; e: KeyPressEventArgs);
  public 
    constructor Create(name: string; w: integer := 0);
    property Value: integer read GetValue write SetValue;
    property Text: string read GetText;
  end;
  
  /// Поле ввода вещественных значений
  RealField = class(Field)
  private 
    nonNumberEntered: boolean;
    function GetValue: real;
    procedure SetValue(x: real);
    procedure fKeyPressed(sender: Object; e: KeyPressEventArgs);
  public 
    constructor Create(name: string; w: integer := 0);
    property Value: real read GetValue write SetValue;
    property Text: string read GetText;
  end;
  
  /// Кнопка
  Button = class
  protected 
    b := new System.Windows.Forms.Button;
    procedure BClick(sender: Object; e: EventArgs);
    function GetW := b.Width;
    procedure SetW(w: integer) := b.Width := w;
    function GetText := b.Text;
    procedure SetText(t: string) := b.Text := t;
  public 
    event Click: procedure;
    constructor Create(text: string);
    property Width: integer read GetW write SetW;
    property Text: string read GetText write SetText;
  end;
  
  /// Текстовая метка
  TextLabel = class
  protected 
    l := new System.Windows.Forms.Label;
    function GetT(): string;
    begin
      Result := l.Text
    end;
    procedure SetT(w: string);
    begin
      l.Text := w;
    end;
  public 
    constructor(text: string);
    property Text: string read GetT write SetT;
  end;
  
  /// Флажок
  CheckBox = class
  private 
    cb := new System.Windows.Forms.CheckBox;
    function GetValue: boolean;
    procedure SetValue(x: boolean);
  public 
    constructor(text: string);
    property Checked: boolean read GetValue write SetValue;
  end;
  
  /// Радиокнопка
  RadioButton = class
  private 
    rb := new System.Windows.Forms.RadioButton;
    function GetValue: boolean;
    procedure SetValue(x: boolean);
  public 
    constructor(text: string);
    property Checked: boolean read GetValue write SetValue;
  end;
  
  /// Список
  ListBox = class
  protected 
    lb := new System.Windows.Forms.ListBox;
    function GetItems: System.Windows.Forms.ListBox.ObjectCollection;
    function GetSelectedIndex: integer;
    procedure SetSelectedIndex(i: integer);
    function GetSelectedItem: object;
    procedure SetSelectedItem(o: object);
    function GetCount: integer;
    procedure LBClick(sender: Object; e: EventArgs);
    procedure LBSelectedIndexChanged(sender: Object; e: EventArgs);
  public 
    event Click: procedure;
    event SelectedIndexChanged: procedure;
    constructor Create;
    property Items: System.Windows.Forms.ListBox.ObjectCollection read GetItems;
    property SelectedIndex: integer read GetSelectedIndex write SetSelectedIndex;
    property SelectedItem: object read GetSelectedItem write SetSelectedItem;
    property Count: integer read GetCount;
  end;
  
  /// Выпадающий список
  ComboBox = class
  private 
    cb := new System.Windows.Forms.ComboBox;
    function GetItems: System.Windows.Forms.ComboBox.ObjectCollection;
    function GetSelectedIndex: integer;
    procedure SetSelectedIndex(i: integer);
    function GetSelectedItem: object;
    procedure SetSelectedItem(o: object);
    function GetCount: integer;
    procedure CBClick(sender: Object; e: EventArgs);
    procedure CBSelectedIndexChanged(sender: Object; e: EventArgs);
    function GetWidth: integer;
    procedure SetWidth(x: integer);
  public 
    event Click: procedure;
    event SelectedIndexChanged: procedure;
    constructor;
    property Items: System.Windows.Forms.ComboBox.ObjectCollection read GetItems;
    property SelectedIndex: integer read GetSelectedIndex write SetSelectedIndex;
    property SelectedValue: object read GetSelectedItem write SetSelectedItem;
    property Count: integer read GetCount;
    property Width: integer read GetWidth write SetWidth;
  end;
  
  /// Ползунок
  TrackBar = class
  private 
    tb: System.Windows.Forms.TrackBar;
    function GetMinimum: integer;
    procedure SetMinimum(v: integer);
    function GetMaximum: integer;
    procedure SetMaximum(v: integer);
    function GetValue: integer;
    procedure SetValue(v: integer);
    function GetFrequency: integer;
    procedure SetFrequency(v: integer);
    procedure TBValueChanged(sender: Object; e: EventArgs);
  public 
    event ValueChanged: procedure;
    constructor Create;
    property Minimum: integer read GetMinimum write SetMinimum; 
    property Maximum: integer read GetMaximum write SetMaximum; 
    property Value: integer read GetValue write SetValue; 
    property Frequency: integer read GetFrequency write SetFrequency; 
  end;
  
  /// Ползунок
  IntegerUpDown = class
  private 
    n: System.Windows.Forms.NumericUpDown;
    function GetMinimum: integer;
    procedure SetMinimum(v: integer);
    function GetMaximum: integer;
    procedure SetMaximum(v: integer);
    function GetValue: integer;
    procedure SetValue(v: integer);
    function GetWidth: integer;
    procedure SetWidth(v: integer);
    procedure NValueChanged(sender: Object; e: EventArgs);
  public 
    event ValueChanged: procedure;
    constructor Create;
    property Minimum: integer read GetMinimum write SetMinimum; 
    property Maximum: integer read GetMaximum write SetMaximum; 
    property Value: integer read GetValue write SetValue; 
    property Width: integer read GetWidth write SetWidth; 
  end;
  
  BaseDockControl = class
  private 
    f: System.Windows.Forms.Control;
    function GetWidth: integer;
    procedure SetWidth(x: integer);
    function GetHeight: integer;
    procedure SetHeight(x: integer);
    function GetDock: DockStyle;
    procedure SetDock(d: DockStyle);
  public 
    property Width: integer read GetWidth write SetWidth;
    property Height: integer read GetHeight write SetHeight;
    property Dock: DockStyle read GetDock write SetDock;
  end;
  
  /// Текстовый редактор
  TextBox = class(BaseDockControl)
  private 
    function GetText: string;
    procedure SetText(s: string);
    function GetLines: array of string;
    procedure SetLines(s: array of string);
  public 
    constructor Create();
    property Text: string read GetText write SetText;
    property Lines: array of string read GetLines write SetLines;
    procedure SaveFile(fname: string);
    procedure LoadFile(fname: string);
    procedure AddLine(txt: string);
    procedure Undo;
    procedure Redo;
    procedure Cut;
    procedure Copy;
    procedure Paste;
  end;
  
  /// Веб-браузер
  WebBrowser = class(BaseDockControl)
  private 
    function GetText: string;
    procedure SetText(s: string);
    function GetDocumentStream: System.IO.Stream := (f as System.Windows.Forms.WebBrowser).DocumentStream;
    procedure SetDocumentStream(s: System.IO.Stream);
    begin
      (f as System.Windows.Forms.WebBrowser).DocumentStream := s;
    end;
    function GetAddress: string;
    procedure WBDocumentCompleted(sender: Object;	e: WebBrowserDocumentCompletedEventArgs);
  public 
    event DocumentCompleted: procedure;
    constructor Create();
    property DocumentText: string read GetText write SetText;
    procedure Navigate(address: string);
    procedure GoBack;
    procedure GoForward;
    procedure GoHome;
    property Address: string read GetAddress;
    property DocumentStream: System.IO.Stream read GetDocumentStream write SetDocumentStream;
  end;
  
  /// Окно для рисования
  PaintBox = class(BaseDockControl)
  private 
    procedure PResize(sender: Object;	e: EventArgs);
    procedure PPaint(sender: Object;	e: PaintEventArgs);
    procedure PMouseDown(sender: Object;	e: MouseEventArgs);
    procedure PMouseUp(sender: Object;	e: MouseEventArgs);
    procedure PMouseMove(sender: Object;	e: MouseEventArgs);
  public 
    event Resize: procedure;
    event Paint: procedure (e: PaintEventArgs);
    event MouseDown: procedure (e: MouseEventArgs);
    event MouseUp: procedure (e: MouseEventArgs);
    event MouseMove: procedure (e: MouseEventArgs);
    constructor Create();
    function Graphics: System.Drawing.Graphics;
    procedure Invalidate;
    //function pb: PictureBox;
  end;

  /// Пункт меню
  MenuItem = class;
  ItemProc = procedure(item: MenuItem);
  MenuItem = class
  private 
    mi: ToolStripMenuItem;
    items: List<MenuItem>;
    procedure MIClick(sender: Object; e: EventArgs);
    procedure SetText(text: string);
    function GetText: string;
    function GetItem(i: integer): MenuItem; 
  public 
    event Click: ItemProc;
    constructor Create(mi: ToolStripMenuItem);
    procedure Add(name: string; handler: ItemProc);
    procedure Add(name1,name2: string; handler: ItemProc);
    procedure Add(name1,name2,name3: string; handler: ItemProc);
    property Text: string read GetText write SetText;
    property Item[i: integer]: MenuItem read GetItem; default;
  end;
  
  /// Главное меню
  MainMenu = class
  private 
    m: MenuStrip;  
    items: List<MenuItem>; 
    function GetItem(i: integer): MenuItem; 
  public 
    constructor Create;
    procedure Add(params names: array of string);
    property Item[i: integer]: MenuItem read GetItem; default;
  end;
  
  ContainerControl = class
  private 
    NetControl: Control;  
    procedure Add(c: Control);
    begin
      NetControl.Controls.Add(c);
    end;
  public
  end;
  
  /// Тип главной формы
  MainFormType = class(ContainerControl)
  private 
    m: Form;
    procedure SetLeft(l: integer);
    function GetLeft: integer;
    procedure SetTop(t: integer);
    function GetTop: integer;
    procedure SetWidth(w: integer);
    function GetWidth: integer;
    procedure SetHeight(h: integer);
    function GetHeight: integer;
    procedure SetTitle(t: string);
    function GetTitle: string;
    procedure SetIsFixedSize(b: boolean);
    function GetIsFixedSize: boolean;
    procedure MResize(sender: object; e: EventArgs);
    constructor Create;
  public 
    event Resize: procedure;
    /// Отступ главного окна от левого края экрана в пикселах
    property Left: integer read GetLeft write SetLeft;
    /// Отступ главного окна от верхнего края экрана в пикселах
    property Top: integer read GetTop write SetTop;
    /// Ширина главного окна в пикселах
    property Width: integer read GetWidth write SetWidth;
    /// Высота главного окна в пикселах
    property Height: integer read GetHeight write SetHeight;
    /// Заголовок главного окна
    property Title: string read GetTitle write SetTitle;
    /// Имеет ли графическое окно фиксированный размер
    property IsFixedSize: boolean read GetIsFixedSize write SetIsFixedSize;
    /// Устанавливает размеры главного окна в пикселах
    procedure SetSize(w, h: integer);
    /// Устанавливает отступ главного окна от левого верхнего края экрана в пикселах
    procedure SetPos(l, t: integer);
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
    /// Возвращает главную .NET-форму
    function NetForm: Form;
  end;

  Panel = class(ContainerControl)
  private 
    p: System.Windows.Forms.Panel;
    function GetDock: DockStyle;
    procedure SetDock(d: DockStyle);
    procedure SetWidth(w: integer);
    function GetWidth: integer;
    procedure SetHeight(h: integer);
    function GetHeight: integer;
    function GetBorderStyle: BorderStyle;
    procedure SetBorderStyle(bs: BorderStyle);
    function GetColor: FormsABC.Color;
    procedure SetColor(c: FormsABC.Color);
    constructor Create(f: System.Windows.Forms.Panel);
  public 
    constructor Create;
    /// Ширина панели в пикселах
    property Width: integer read GetWidth write SetWidth;
    /// Высота панели в пикселах
    property Height: integer read GetHeight write SetHeight;
    property Dock: DockStyle read GetDock write SetDock;
    property Border: BorderStyle read GetBorderStyle write SetBorderStyle;
    property Color: FormsABC.Color read GetColor write SetColor;
  end;

  FlowPanel = class(Panel)
  private 
    function GetFlowDirection: FlowDirection;
    procedure SetFlowDirection(fd: FlowDirection);
    constructor Create(f: FlowLayoutPanel);
  public 
    constructor Create;
    property Direction: FlowDirection read GetFlowDirection write SetFlowDirection;
  end;

  CommonDialog = class
  private 
    d: System.Windows.Forms.CommonDialog;
  public 
    function ShowDialog: DialogResult;
  end;
  
  /// Диалог работы с файлами
  FileDialog = class(CommonDialog)
  private 
    procedure SetFilter(f: string);
    function GetFilter: string;
    procedure SetInitialDirectory(f: string);
    function GetInitialDirectory: string;
    procedure SetFileName(f: string);
    function GetFileName: string;
  public 
    property Filter: string read GetFilter write SetFilter;
    property FileName: string read GetFileName write SetFileName;
    property InitialDirectory: string read GetInitialDirectory write SetInitialDirectory;
  end;
  
  /// Диалог открытия файла
  OpenFileDialog = class(FileDialog)
  public 
    constructor Create;
  end;
  
  /// Диалог сохранения файла
  SaveFileDialog = class(FileDialog)
  public 
    constructor Create;
  end;
//------------- Процедуры ---------------

/// Перейти на новую строку
procedure LineBreak;
/// Добавить пустую строку
procedure EmptyLine(h: integer := 20);
/// Добавить пустое пространство
procedure EmptySpace(w: integer := 20);

type 
  FlowBreak = class
    constructor(h: integer := 0);
    begin
      if h=0 then
        LineBreak
      else EmptyLine(h)
    end;
  end;
  Space = class
    constructor(w: integer := 20);
    begin
      EmptySpace(w)
    end;
  end;

//------------- Переменные ---------------

var
  MainForm: MainFormType;
  MainPanel: FlowPanel;
  // Эксперимент: 20.11.10
  ParentControl: ContainerControl;

///--
procedure __InitModule__;
///--
procedure __FinalizeModule__;

implementation

var
  _MainForm : Form;
  _MainPanel : FlowLayoutPanel;
  _MainMenu: MenuStrip := nil;
  CurrentControl: Control := nil;
  nfi: System.Globalization.NumberFormatInfo;
  
  
//------------- Компоненты ---------------

//------------- Field ---------------

function Field.GetRealValue: real;
begin
  Result := real.Parse(f.Text,nfi);
end;

procedure Field.SetRealValue(x: real);
begin
  f.Text := x.ToString
end;

function Field.GetFieldWidth: integer;
begin
  Result := f.Width;
end;

procedure Field.SetFieldWidth(x: integer);
begin
  f.Width := x
end;

function Field.GetText: string;
begin
  Result := f.Text;    
end;

procedure Field.SetText(s: string);
begin
  f.Text := s;
end;

procedure Field.TBTextChanged(sender: Object; e: EventArgs);
begin
  if TextChanged <> nil then
    TextChanged;
end;

constructor Field.Create(name: string; w: integer);
begin
  f := new System.Windows.Forms.TextBox;
  p := new System.Windows.Forms.Panel;
  l := new &Label;
  l.AutoSize := True;
  l.Text := name;
  l.Size := new Size(l.PreferredWidth, l.PreferredHeight);
  f.Top := l.Top + l.Height;
  p.Controls.Add(l);
  p.Controls.Add(f);
  p.AutoSize := True;
  p.AutoSizeMode := AutoSizeMode.GrowAndShrink;
  ParentControl.Add(p);
  CurrentControl := p;
  if w <> 0 then 
    FieldWidth := w;
  f.TextChanged += TBTextChanged;
end;

//------------- IntegerField ---------------

function IntegerField.GetValue: integer;
begin
  if Trim(f.Text) = '' then
    Result := 0
  else Result := integer.Parse(f.Text);
end;

procedure IntegerField.SetValue(x: integer);
begin
  f.Text := x.ToString
end;

procedure IntegerField.fKeyPressed(sender: Object; e: KeyPressEventArgs);
begin
  if (e.KeyChar = #8) or (e.KeyChar = '-')  then 
    exit;
  if not Char.IsDigit(e.KeyChar) then
    e.Handled := True;
end;

constructor IntegerField.Create(name: string; w: integer);
begin
  inherited Create(name, w);  
  Value := 0;
  f.KeyPress += fKeyPressed;
end;

//------------- RealField ---------------

function RealField.GetValue: real;
begin
  if Trim(f.Text) = '' then
    Result := 0
  else Result := real.Parse(f.Text,nfi);
end;

procedure RealField.SetValue(x: real);
begin
  f.Text := x.ToString(nfi);
end;

procedure RealField.fKeyPressed(sender: Object; e: KeyPressEventArgs);
begin
  if (e.KeyChar = #8) or (e.KeyChar = '-') then 
    exit;
  if not Char.IsDigit(e.KeyChar) then
    if (e.KeyChar <> '.') or (f.Text.IndexOf('.') <> -1) then
      e.Handled := True;
end;

constructor RealField.Create(name: string; w: integer);
begin
  inherited Create(name, w);  
  Value := 0;
  f.KeyPress += fKeyPressed;
end;

//------------- Button ---------------

procedure Button.BClick(sender: Object; e: EventArgs);
begin
  if Click <> nil then
    Click;
end;

constructor Button.Create(text: string);
begin
  b.Text := text;
//  b.Dock := DockStyle.Right;
  b.AutoSize := True;
  ParentControl.Add(b);
  CurrentControl := b;
  b.Click += BClick;
end;

//------------- TextLabel ---------------

constructor TextLabel.Create(text: string);
begin
  l.Text := text;
  l.AutoSize := True;
  l.Size := new Size(l.PreferredWidth, l.PreferredHeight);
  _MainPanel.Controls.Add(l);
  CurrentControl := l;
end;

//------------- CheckBox ---------------

function CheckBox.GetValue: boolean;
begin
  Result := cb.Checked;
end;

procedure CheckBox.SetValue(x: boolean);
begin
  cb.Checked := x
end;

constructor CheckBox.Create(text: string);
begin
  cb.Text := text;
  cb.AutoSize := True;
  ParentControl.Add(cb);
  CurrentControl := cb;
end;

//------------- RadioButton ---------------

function RadioButton.GetValue: boolean;
begin
  Result := rb.Checked;
end;

procedure RadioButton.SetValue(x: boolean);
begin
  rb.Checked := x
end;

constructor RadioButton.Create(text: string);
begin
  rb.Text := text;
  rb.AutoSize := True;
  ParentControl.Add(rb);
  CurrentControl := rb;
end;

//------------- ListBox ---------------

function ListBox.GetItems: System.Windows.Forms.ListBox.ObjectCollection;
begin
  Result := lb.Items;
end;

function ListBox.GetSelectedIndex: integer;
begin
  Result := lb.SelectedIndex;
end;

procedure ListBox.SetSelectedIndex(i: integer);
begin
  lb.SelectedIndex := i;
end;

function ListBox.GetSelectedItem: object;
begin
  Result := lb.SelectedItem;
end;

procedure ListBox.SetSelectedItem(o: object);
begin
  lb.SelectedItem := o;
end;

function ListBox.GetCount: integer;
begin
  Result := lb.Items.Count;
end;

procedure ListBox.LBClick(sender: Object; e: EventArgs);
begin
  if Click <> nil then
    Click;
end;

procedure ListBox.LBSelectedIndexChanged(sender: Object; e: EventArgs);
begin
  if SelectedIndexChanged <> nil then
    SelectedIndexChanged;
end;

constructor ListBox.Create;
begin
  //lb.AutoSize := True;
  ParentControl.Add(lb);
  CurrentControl := lb;
  lb.Click += LBClick;
  lb.SelectedIndexChanged += LBSelectedIndexChanged;
end;

//------------- ComboBox ---------------

function ComboBox.GetItems: System.Windows.Forms.ComboBox.ObjectCollection;
begin
  Result := cb.Items;
end;

function ComboBox.GetSelectedIndex: integer;
begin
  Result := cb.SelectedIndex;
end;

procedure ComboBox.SetSelectedIndex(i: integer);
begin
  cb.SelectedIndex := i;
end;

function ComboBox.GetSelectedItem: object;
begin
  Result := cb.SelectedItem;
end;

procedure ComboBox.SetSelectedItem(o: object);
begin
  cb.SelectedItem := o;
end;

function ComboBox.GetCount: integer;
begin
  Result := cb.Items.Count;
end;

procedure ComboBox.CBClick(sender: Object; e: EventArgs);
begin
  if Click <> nil then
    Click;
end;

procedure ComboBox.CBSelectedIndexChanged(sender: Object; e: EventArgs);
begin
  if SelectedIndexChanged <> nil then
    SelectedIndexChanged;
end;

function ComboBox.GetWidth: integer;
begin
  Result := cb.Width;
end;

procedure ComboBox.SetWidth(x: integer);
begin
  cb.Width := x
end;

constructor ComboBox.Create;
begin
  cb.AutoSize := True;
  ParentControl.Add(cb);
  CurrentControl := cb;
  cb.Click += CBClick;
  cb.SelectedIndexChanged += CBSelectedIndexChanged;
end;

//------------- BaseDockControl ---------------

function BaseDockControl.GetWidth: integer;
begin
  Result := f.Width;
end;

procedure BaseDockControl.SetWidth(x: integer);
begin
  f.Width := x;
end;

function BaseDockControl.GetHeight: integer;
begin
  Result := f.Height;
end;

procedure BaseDockControl.SetHeight(x: integer);
begin
  f.Height := x;
end;

function BaseDockControl.GetDock: DockStyle;
begin
  Result := f.Dock
end;

procedure BaseDockControl.SetDock(d: DockStyle);
begin
  if f.Dock = d then
    exit;
  f.Dock := d;
  
{  if d<>DockStyle.Fill then
    if not _MainPanel.Visible then
      _MainPanel.Visible := True;}
      
{  case d of
  DockStyle.Right:  f.Parent := _MainForm;
  DockStyle.Left:   f.Parent := _MainForm;
  DockStyle.Top:    f.Parent := _MainForm;
  DockStyle.Bottom: f.Parent := _MainForm; 
  DockStyle.None:   f.Parent := _MainPanel;
  DockStyle.Fill: 
    begin
      f.Parent := _MainForm;
      //_MainPanel.Visible := False;
    end;
  end;  }
end;

//------------- TextBox ---------------

function TextBox.GetText: string;
begin
  Result := f.Text;
end;

procedure TextBox.SetText(s: string);
begin
  f.Text := s;
end;

function TextBox.GetLines: array of string;
begin
  Result := (f as System.Windows.Forms.TextBox).Lines;
end;

procedure TextBox.SetLines(s: array of string);
begin
  (f as System.Windows.Forms.TextBox).Lines := s;
end;

procedure TextBox.SaveFile(fname: string);
begin
  var ff := new System.IO.StreamWriter(fname, false, System.Text.Encoding.Default);
  ff.Write(f.Text);
  ff.Close;
end;

procedure TextBox.LoadFile(fname: string);
begin
  var ff := new System.IO.StreamReader(fname, System.Text.Encoding.Default);
  Text := ff.ReadToEnd;
  ff.Close;
end;

procedure TextBox.AddLine(txt: string);
begin
  (f as System.Windows.Forms.RichTextBox).AppendText(txt + Environment.NewLine);  
end;

procedure TextBox.Undo;
begin
  (f as System.Windows.Forms.RichTextBox).Undo  
end;

procedure TextBox.Redo;
begin
  (f as System.Windows.Forms.RichTextBox).Redo  
end;

procedure TextBox.Cut;
begin
  (f as System.Windows.Forms.RichTextBox).Cut  
end;

procedure TextBox.Copy;
begin
  (f as System.Windows.Forms.RichTextBox).Copy  
end;

procedure TextBox.Paste;
begin
  (f as System.Windows.Forms.RichTextBox).Paste  
end;

constructor TextBox.Create();
begin
  f := new System.Windows.Forms.RichTextBox;
  f.Width := 150;
  f.Height := 100;
  (f as System.Windows.Forms.RichTextBox).Multiline := True;
  (f as System.Windows.Forms.RichTextBox).WordWrap := false;
  ParentControl.Add(f);
  CurrentControl := f;
end;

//------------- PaintBox ---------------

procedure PaintBox.PResize(sender: Object;	e: EventArgs);
begin
  if Resize<>nil then
    Resize
end;

procedure PaintBox.PPaint(sender: Object;	e: PaintEventArgs);
begin
  if Paint<>nil then
    Paint(e);
end;

procedure PaintBox.PMouseDown(sender: Object;	e: MouseEventArgs);
begin
  if MouseDown<>nil then
    MouseDown(e);  
end;

procedure PaintBox.PMouseUp(sender: Object;	e: MouseEventArgs);
begin
  if MouseUp<>nil then
    MouseUp(e);  
end;

procedure PaintBox.PMouseMove(sender: Object;	e: MouseEventArgs);
begin
  if MouseMove<>nil then
    MouseMove(e);  
end;

function PaintBox.Graphics: System.Drawing.Graphics;
begin
  Result := System.Drawing.Graphics.FromImage((f as PictureBox).Image);
  //Result := System.Drawing.Graphics.FromHWND(f.Handle)
end;

procedure PaintBox.Invalidate;
begin
  (f as PictureBox).Invalidate;
end;

constructor PaintBox.Create();
begin
  f := new PictureBox;
  (f as PictureBox).Image := new Bitmap(1280,1024);
  Graphics.Clear(Color.White);
  ParentControl.Add(f);
  CurrentControl := f;
  var p := f as PictureBox;
  p.Resize += PResize;
  p.Paint += PPaint;
  p.MouseDown += PMouseDown;
  p.MouseMove += PMouseMove;
  p.MouseUp += PMouseUp;
end;

//------------- WebBrowser ---------------

function WebBrowser.GetText: string;
begin
  Result := (f as System.Windows.Forms.WebBrowser).DocumentText
end;

procedure WebBrowser.SetText(s: string);
begin
  (f as System.Windows.Forms.WebBrowser).DocumentText := s;
end;

function WebBrowser.GetAddress: string;
begin
  Result := (f as System.Windows.Forms.WebBrowser).Url.AbsoluteUri
end;

procedure WebBrowser.WBDocumentCompleted(sender: Object;	e: WebBrowserDocumentCompletedEventArgs);
begin
  if DocumentCompleted<>nil then
    DocumentCompleted;
end;

constructor WebBrowser.Create();
begin
  f := new System.Windows.Forms.WebBrowser;
  (f as System.Windows.Forms.WebBrowser).ScriptErrorsSuppressed := True;
  ParentControl.Add(f);
  CurrentControl := f;
  (f as System.Windows.Forms.WebBrowser).DocumentCompleted += WBDocumentCompleted;
end;

procedure WebBrowser.Navigate(address: string);
begin
  (f as System.Windows.Forms.WebBrowser).Navigate(address);  
end;

procedure WebBrowser.GoBack;
begin
  (f as System.Windows.Forms.WebBrowser).GoBack;  
end;

procedure WebBrowser.GoForward;
begin
  (f as System.Windows.Forms.WebBrowser).GoForward
end;

procedure WebBrowser.GoHome;
begin
  (f as System.Windows.Forms.WebBrowser).GoHome
end;

//------------- MenuItem ---------------

constructor MenuItem.Create(mi: ToolStripMenuItem);
begin
  Self.mi := mi;
  items := new List<MenuItem>;
end;

procedure MenuItem.Add(name: string; handler: ItemProc);
begin
  var nmi := new ToolStripMenuItem(name);
  var mmi := new MenuItem(nmi);
  mmi.Click += handler;
  items.Add(mmi);
  nmi.Click += mmi.MIClick;
  mi.DropDownItems.Add(nmi);
end;

procedure MenuItem.Add(name1,name2: string; handler: ItemProc);
begin
  Add(name1, handler);
  Add(name2, handler);
end;

procedure MenuItem.Add(name1,name2,name3: string; handler: ItemProc);
begin
  Add(name1, handler);
  Add(name2, handler);
  Add(name3, handler);
end;

procedure MenuItem.MIClick(sender: Object; e: EventArgs);
begin
  if Click<>nil then
    Click(Self);
end;

procedure MenuItem.SetText(text: string);
begin
  mi.Text := text  
end;

function MenuItem.GetText: string;
begin
  Result := mi.Text;
end;

function MenuItem.GetItem(i: integer): MenuItem; 
begin
  Result := items[i];
end;

//------------- MainMenu ---------------

constructor MainMenu.Create;
begin
  m := new MenuStrip;
  _MainMenu := m;
  _MainForm.MainMenuStrip := m;
  //_MainForm.Controls.Add(m);
  items := new List<MenuItem>;
end;

procedure MainMenu.Add(params names: array of string);
begin
  var mi: ToolStripMenuItem;
  foreach x: string in names do
  begin
    mi := new ToolStripMenuItem(x);
    m.Items.Add(mi);
    items.Add(new MenuItem(mi))
  end;
end;

function MainMenu.GetItem(i: integer): MenuItem; 
begin
  Result := items[i];
end;

//------------- TrackBar ---------------

function TrackBar.GetMinimum: integer;
begin
  Result := tb.Minimum
end;

procedure TrackBar.SetMinimum(v: integer);
begin
  tb.Minimum := v
end;

function TrackBar.GetMaximum: integer;
begin
  Result := tb.Maximum
end;

procedure TrackBar.SetMaximum(v: integer);
begin
  tb.Maximum := v
end;

function TrackBar.GetValue: integer;
begin
  Result := tb.Value
end;

procedure TrackBar.SetValue(v: integer);
begin
  tb.Value := v
end;

function TrackBar.GetFrequency: integer;
begin
  Result := tb.TickFrequency
end;

procedure TrackBar.SetFrequency(v: integer);
begin
  tb.TickFrequency := v
end;

procedure TrackBar.TBValueChanged(sender: Object; e: EventArgs);
begin
  if ValueChanged<>nil then
    ValueChanged;
end;

constructor TrackBar.Create;
begin
  tb := new System.Windows.Forms.TrackBar;
  ParentControl.Add(tb);
  CurrentControl := tb;
  tb.ValueChanged += TBValueChanged;
end;

//------------- NumericUpDown ---------------

function IntegerUpDown.GetMinimum: integer;
begin
  Result := integer(n.Minimum)
end;

procedure IntegerUpDown.SetMinimum(v: integer);
begin
  n.Minimum := v
end;

function IntegerUpDown.GetMaximum: integer;
begin
  Result := integer(n.Maximum)
end;

procedure IntegerUpDown.SetMaximum(v: integer);
begin
  n.Maximum := v
end;

function IntegerUpDown.GetValue: integer;
begin
  Result := integer(n.Value)
end;

procedure IntegerUpDown.SetValue(v: integer);
begin
  n.Value := v
end;

function IntegerUpDown.GetWidth: integer;
begin
  Result := n.Width
end;

procedure IntegerUpDown.SetWidth(v: integer);
begin
  n.Width := v
end;

procedure IntegerUpDown.NValueChanged(sender: Object; e: EventArgs);
begin
  if ValueChanged<>nil then
    ValueChanged;
end;

constructor IntegerUpDown.Create;
begin
  n := new System.Windows.Forms.NumericUpDown;
  ParentControl.Add(n);
  CurrentControl := n;
  n.ValueChanged += NValueChanged;
end;

//------------- MainWindowType ---------------

procedure MainFormType.MResize(sender: object; e: EventArgs);
begin
  if Resize<>nil then
    Resize;
end;

constructor MainFormType.Create;
begin
  m := _MainForm;
  NetControl := m;
  m.Resize += MResize;
end;

procedure MainFormType.SetLeft(l: integer);
begin
  _MainForm.Left := l;
end;

function MainFormType.GetLeft: integer;
begin
  Result := _MainForm.Left
end;

procedure MainFormType.SetTop(t: integer);
begin
  _MainForm.Top := t;
end;

function MainFormType.GetTop: integer;
begin
  Result := _MainForm.Top
end;

procedure MainFormType.SetWidth(w: integer);
begin
  _MainForm.Width := w;
end;

function MainFormType.GetWidth: integer;
begin
  Result := _MainForm.Width
end;

procedure MainFormType.SetHeight(h: integer);
begin
  _MainForm.Height := h;
end;

function MainFormType.GetHeight: integer;
begin
  Result := _MainForm.Height
end;

procedure MainFormType.SetTitle(t: string);
begin
  _MainForm.Text := t;
end;

function MainFormType.GetTitle: string;
begin
  Result := _MainForm.Text
end;

procedure MainFormType.SetIsFixedSize(b: boolean);
begin
  if b then 
    _MainForm.FormBorderStyle := FormBorderStyle.FixedSingle
  else _MainForm.FormBorderStyle := FormBorderStyle.Sizable
end;

function MainFormType.GetIsFixedSize: boolean;
begin
  if _MainForm.FormBorderStyle = FormBorderStyle.FixedSingle then
    Result := True 
  else Result := False;
end;

procedure MainFormType.SetSize(w, h: integer);
begin
  _MainForm.Size := new Size(w, h);
end;

procedure MainFormType.SetPos(l, t: integer);
begin
  _MainForm.Left := l;
  _MainForm.Top := t;
end;

procedure MainFormType.Close;
begin
  _MainForm.Close;
end;

procedure MainFormType.Minimize;
begin
  _MainForm.WindowState := FormWindowState.Minimized;
end;

procedure MainFormType.Maximize;
begin
  _MainForm.WindowState := FormWindowState.Maximized;
end;

procedure MainFormType.Normalize;
begin
  _MainForm.WindowState := FormWindowState.Normal;
end;

procedure MainFormType.CenterOnScreen;
begin
  var ScreenWidth := Screen.PrimaryScreen.Bounds.Width;
  var ScreenHeight := Screen.PrimaryScreen.Bounds.Height;
  SetPos((ScreenWidth - MainForm.Width) div 2, (ScreenHeight - MainForm.Height) div 2);
end;

function MainFormType.NetForm: Form;
begin
  Result := _MainForm
end;

//------------- Dialogs ---------------

function CommonDialog.ShowDialog: DialogResult;
begin
  Result := d.ShowDialog;
end;

procedure FileDialog.SetFilter(f: string);
begin
  (d as System.Windows.Forms.FileDialog).Filter := f;
end;

function FileDialog.GetFilter: string;
begin
  Result := (d as System.Windows.Forms.FileDialog).Filter
end;

constructor OpenFileDialog.Create;
begin
  d := new System.Windows.Forms.OpenFileDialog;
end;

constructor SaveFileDialog.Create;
begin
  d := new System.Windows.Forms.SaveFileDialog;
end;

procedure FileDialog.SetInitialDirectory(f: string);
begin
  (d as System.Windows.Forms.FileDialog).InitialDirectory := f;
end;

function FileDialog.GetInitialDirectory: string;
begin
  Result := (d as System.Windows.Forms.FileDialog).InitialDirectory
end;

procedure FileDialog.SetFileName(f: string);
begin
  (d as System.Windows.Forms.FileDialog).InitialDirectory := f;
end;

function FileDialog.GetFileName: string;
begin
  Result := (d as System.Windows.Forms.FileDialog).FileName  
end;

//------------- PanelType ---------------

constructor Panel.Create(f: System.Windows.Forms.Panel);
begin
  p := f;
  NetControl := p;
end;

constructor Panel.Create;
begin
  p := new System.Windows.Forms.Panel();  
  //p.BorderStyle := BorderStyle.Fixed3D;
  NetControl := p;
  ParentControl.Add(p);
end;

function Panel.GetColor: FormsABC.Color;
begin
  Result := p.BackColor;  
end;

procedure Panel.SetColor(c: FormsABC.Color);
begin
  p.BackColor := c;
end;

procedure Panel.SetBorderStyle(bs: BorderStyle);
begin
  p.BorderStyle := bs;
end;

function Panel.GetBorderStyle: BorderStyle;
begin
  Result := p.BorderStyle;  
end;

function Panel.GetDock: DockStyle;
begin
  Result := p.Dock
end;

procedure Panel.SetDock(d: DockStyle);
begin
  p.Dock := d;
  p.BringToFront;
end;

procedure Panel.SetWidth(w: integer);
begin
  p.Width := w
end;

function Panel.GetWidth: integer;
begin
  Result := p.Width
end;

procedure Panel.SetHeight(h: integer);
begin
  p.Height := h;
end;

function Panel.GetHeight: integer;
begin
  Result := p.Height
end;

constructor FlowPanel.Create;
begin
  inherited Create(new System.Windows.Forms.FlowLayoutPanel());
  ParentControl.Add(p);
end;

constructor FlowPanel.Create(f: FlowLayoutPanel);
begin
  inherited Create(f);
end;

procedure FlowPanel.SetFlowDirection(fd: FlowDirection);
begin
  FlowLayoutPanel(p).FlowDirection := fd;
end;

function FlowPanel.GetFlowDirection: FlowDirection;
begin
  Result := FlowLayoutPanel(p).FlowDirection;
end;

//------------- Процедуры ---------------

procedure LineBreak;
begin
  _MainPanel.SetFlowBreak(CurrentControl, true);
end;

procedure EmptyLine(h: integer);
begin
  var p := new System.Windows.Forms.Panel;
  p.Height := h;
  ParentControl.Add(p);
  CurrentControl := p;
  LineBreak;
end;

procedure EmptySpace(w: integer);
begin
  var p := new System.Windows.Forms.Panel;
  p.Width := w;
  p.Height := 1;
  ParentControl.Add(p);
  CurrentControl := p;
end;
 
var __initialized := false;
var __finalized := false;

procedure __InitModule;
begin
  _MainForm := new Form;
  _MainPanel := new FlowLayoutPanel;
  _MainForm.Text := 'FormsABC.NET';
  _MainForm.StartPosition := FormStartPosition.CenterScreen;
  _MainPanel.Size := _MainForm.ClientSize;
  _MainPanel.Dock := DockStyle.Fill;
  MainForm := new MainFormType;
  MainPanel := new FlowPanel(_MainPanel);
  ParentControl := MainPanel;
  nfi := new System.Globalization.NumberFormatInfo();
  nfi.NumberGroupSeparator := '.';
end;

procedure __FinalizeModule;
begin
  _MainForm.Controls.Add(_MainPanel);
  if _MainMenu<>nil then
    _MainForm.Controls.Add(_MainMenu);
  Application.Run(_MainForm);
end;

procedure __InitModule__;
begin
  if not __initialized then
  begin
    __initialized := true;
    __InitModule;
  end;
end;

procedure __FinalizeModule__;
begin
  if not __finalized then
  begin
    __finalized := true;
    __FinalizeModule;
  end;
end;

initialization
  __InitModule;
finalization
  __FinalizeModule;
end.