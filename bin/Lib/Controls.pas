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
  MessageBox = System.Windows.MessageBox;
  MessageBoxButton = System.Windows.MessageBoxButton;
  MessageBoxResult = System.Windows.MessageBoxResult;
  MessageBoxImage = System.Windows.MessageBoxImage;
  /// Набор предопределённых цветов
  Colors = System.Windows.Media.Colors;
  Key = System.Windows.Input.Key;

var
  ActivePanel: GPanel;
  GlobalHMargin := 12;

procedure Invoke(d: System.Delegate; params args: array of object);
procedure InvokeP(p: procedure(r: real); r: real); 

type
  ///!#
  /// Базовый класс для панелей
  PanelWPF = class
  private
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
  
  /// Класс левой панели
  LeftPanelWPF = class(PanelWPF)
  public
    constructor Create(width: real := 150; c: GColor := PanelsColor; internalMargin: real := 10);
    begin
      inherited Create(width, Dock.Left,c,internalMargin)
    end;
  end;

  /// Класс правой панели
  RightPanelWPF = class(PanelWPF)
  public
    constructor Create(width: real := 150; c: GColor := PanelsColor; internalMargin: real := 10);
    begin
      inherited Create(width, Dock.Right,c,internalMargin)
    end;
  end;

  ///!#
  /// Базовый класс для всех элементов управления
  CommonElementWPF = class
  private 
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
    /// Ширина элемента управления
    property Width: real read InvokeReal(()->element.Width) write Invoke(SetWP, value);
    /// Высота элемента управления
    property Height: real read InvokeReal(()->element.Height) write Invoke(SetHP, value); virtual;
    /// Нижний отступ элемента управления
    property Margin: real read InvokeReal(()->element.Margin.Bottom) write Invoke(SetMP, value);
    /// Активирован ли элемент управления
    property Enabled: boolean read InvokeBoolean(()->element.IsEnabled) write Invoke(SetEP, value);
  end;
  
  CommonControlWPF = class(CommonElementWPF)
  private
    property Control: GControl read element as GControl;
    procedure SetFSzP(size: real) := control.FontSize := size;
    procedure SetFontNameP(name: string) := control.FontFamily := new FontFamily(name);
  public  
    /// Размер шрифта
    property FontSize: real read InvokeReal(()->control.FontSize) write Invoke(SetFSzP, value); virtual;
    /// Имя шрифта
    property FontName: string write Invoke(SetFontNameP, value); virtual;
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
    /// Событие нажатия на элемент управления
    Click: procedure;
  end;

  /// Элемент управления "Изображение"
  ImageWPF = class(CommonControlWPF)
  private
    property im: Image read element as Image;  
    procedure CreatePXY(x,y: real; name: string; width: real := 0);
    begin
      Init0(new Image(),width,x,y);
      im.Source := new BitmapImage();
      im.Source := new BitmapImage(new System.Uri(name,System.UriKind.RelativeOrAbsolute));
    end;
    procedure CreateP(name: string) := CreatePXY(-1,-1,name,0);
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
      im.Source := new BitmapImage(new System.Uri(value,System.UriKind.RelativeOrAbsolute));
    end;
    constructor (i: integer) := Invoke(CreatePP); // фиктивный первый параметр
  public 
    constructor (name: string) := Invoke(CreateP,name);
    constructor (x,y: real; name: string; width: real := 0) := Invoke(CreatePXY,x,y,name,width);
    /// Имя файла изображения
    property Name: string read InvokeString(GetNameP) write Invoke(SetNameP,value);
  end;
  

  /// Элемент управления "Кнопка"
  ButtonWPF = class(ClickableControlWPF)
  private
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
    constructor Create(Txt: string; fontSize: real := 12) := Invoke(CreateP, Txt, fontSize);
    constructor Create(x, y: real; Txt: string; width: real := 0; fontSize: real := 12) := Invoke(CreatePXY, x, y, Txt, width, fontSize);
    
    /// Текст на кнопке
    property Text: string read InvokeString(()->b.Content as string) write Invoke(SetTextP, value);
  end;
  
  /// Элемент управления "Текст"
  TextBlockWPF = class(CommonElementWPF)
  private
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
    procedure SetFontNameP(name: string) := tb.FontFamily := new FontFamily(name);
  public 
    constructor Create(Txt: string; fontsize: real := 12):= Invoke(CreateP, Txt, fontsize);
    constructor Create(x, y: real; Txt: string; width: real := 0; fontSize: real := 12):= Invoke(CreatePXY, x, y, Txt, width, fontSize);

    /// Текст блока текста
    property Text: string read InvokeString(()->tb.Text) write Invoke(SetTextP, value);
    /// Размер шрифта
    property FontSize: real read InvokeReal(()->tb.FontSize) write Invoke(SetFontSizeP, value); virtual;
    /// Имя шрифта
    property FontName: string write Invoke(SetFontNameP, value); virtual;
  end;

  /// Элемент управления "Целое число"
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

    /// Значение целого
    property Value: integer read val write begin val := value; Text := message + ' ' + val; end;
  end;

  /// Элемент управления "Вещественное число"
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

    /// Значение вещественного
    property Value: real read val write begin val := value; Text := message + ' ' + val.ToString(FracDigits); end;
    /// Количество цифр после десятичной точки
    auto property FracDigits: integer := 1;
  end;

  /// Элемент управления "Текстовое поле"
  TextBoxNoTitleWPF = class(CommonControlWPF)
  private
    property tb: GTextBox read element as GTextBox;
    
    procedure SetTextP(t: string) := tb.Text := t;
    
    procedure CreatePXY(x,y: real; txt: string; width: real);
    begin
      Init0(new GTextBox,width,x,y);
      Text := txt;
      tb.TextChanged += (o,e) -> if TextChanged <> nil then TextChanged;
      element.Focusable := True;
      //element.HorizontalAlignment := HorizontalAlignment.Stretch;
    end;
    procedure CreateP(text: string) := CreatePXY(-1,-1,text,0);
    
    procedure CreatePP;
    begin
      element := new TextBox();
      element.Focusable := False;
      tb.AcceptsReturn := True;
      //tb.TextWrapping := TextWrapping.Wrap;
      tb.VerticalScrollBarVisibility := ScrollBarVisibility.Auto;
      tb.HorizontalScrollBarVisibility := ScrollBarVisibility.Auto;
      MainDockPanel.children.RemoveAt(MainDockPanel.children.Count-1);
      MainDockPanel.children.Add(element);
    end;
    procedure SetReadOnlyP(value: boolean);
    begin
      element.Focusable := not value;
    end;
    constructor Create(i: integer) := Invoke(CreatePP);
  public 
    /// Событие изменения текста
    TextChanged: procedure;
    constructor Create(text: string := '') := Invoke(CreateP, text);
    constructor Create(x,y: real; text: string := ''; width: real := 0) := Invoke(CreatePXY, x, y, text, width);
    
    /// Текст в текстовом поле
    property Text: string read InvokeString(()->tb.Text) write Invoke(SetTextP, value);
    /// Добавление строки в конец 
    procedure Print(s: string);
    begin
      Text += s + ' ';
    end;
    /// Добавление строки в конец и переход на новую строку
    procedure Println(s: string);
    begin
      Text += s + NewLine;
    end;
    /// Очистка текста в текстовом поле
    procedure Clear := Text := '';
    /// Разрешено ли ручное редактирование текста
    property Focusable: boolean read InvokeBoolean(()->not element.Focusable) write Invoke(SetReadOnlyP,value);
  end;

  ///!#
  ControlWithTitleWPF = class(CommonControlWPF)
  private
    property tb: GTextBlock read (element as StackPanel).Children[0] as TextBlock;
    property MainElement: GControl read (element as StackPanel).Children[1] as GControl;
    procedure SetTitleP(t: string);
    begin
      if t = '' then
        tb.Visibility := System.Windows.Visibility.Collapsed
      else tb.Visibility := System.Windows.Visibility.Visible;
      tb.Text := t;
    end;
    procedure SetHP(h: real) := MainElement.Height := h;
    
    function CreateStackPanel(el: FrameworkElement; title: string): StackPanel;
    begin
      var sp := new StackPanel;
      var tb := new TextBlock;
      tb.Text := title;
      tb.Margin := new Thickness(0,0,0,3);
      sp.Children.Add(tb);
      if title = '' then
        tb.Visibility := System.Windows.Visibility.Collapsed;
      sp.Children.Add(el);
      Result := sp;      
    end;
    
    procedure Init1(el: FrameworkElement; title: string; width: real := 0; x: real := -1; y: real := -1);
    begin
      var sp := CreateStackPanel(el,title);
      Init0(sp,width,x,y);
    end;
    procedure SetFSzP(size: real);
    begin
      tb.FontSize := size;
      MainElement.FontSize := size;
    end;
    procedure SetFontNameP(name: string);
    begin
      tb.FontFamily := new FontFamily(name);
      MainElement.FontFamily := new FontFamily(name);
    end;
  public 
    /// Заголовок
    property Title: string read InvokeString(()->tb.Text) write Invoke(SetTitleP, value);
    /// Высота элемента управления
    property Height: real read InvokeReal(()->MainElement.ActualHeight) write Invoke(SetHP, value); override;
    /// Размер шрифта
    property FontSize: real read InvokeReal(()->MainElement.FontSize) write Invoke(SetFSzP, value); override;
    /// Имя шрифта
    property FontName: string write Invoke(SetFontNameP, value); override;
  end;

  /// Элемент управления "Текстовое поле с заголовком"
  TextBoxWPF = class(ControlWithTitleWPF)
  private
    property tbx: GTextBox read MainElement as TextBox;
    
    procedure SetTextP(t: string) := tbx.Text := t;
    procedure SetMultiLineP(b: boolean) := if b then tbx.TextWrapping := TextWrapping.Wrap else tbx.TextWrapping := TextWrapping.NoWrap;

    procedure CreatePXY(x,y: real; title,txt: string; width: real);
    begin
      Init1(new TextBox, title, width, x, y);
      //tbx.TextWrapping := TextWrapping.Wrap;
      tbx.Text := txt;
      tbx.TextChanged += (o,e) -> if TextChanged <> nil then TextChanged;
    end;
    procedure CreateAsMainControlP;
    begin
      element := CreateStackPanel(new TextBox,'');
      Multiline := True;
      tbx.TextChanged += (o,e) -> if TextChanged <> nil then TextChanged;
      MainDockPanel.children.RemoveAt(MainDockPanel.children.Count-1);
      MainDockPanel.children.Add(element);
    end;
    procedure CreateP(title,text: string) := CreatePXY(-1,-1,title,text,0);
    constructor Create(i: integer) := Invoke(CreateAsMainControlP);
  public 
    /// Событие "текст изменился"
    TextChanged: procedure;
    constructor Create(title: string := '') := Invoke(CreateP, title, '');
    constructor Create(x,y: real; title: string := ''; width: real := 120) := Invoke(CreatePXY, x, y, title,'', width);
    
    /// Очистка текста в текстовом поле
    procedure Clear := Text := '';
    /// Текст в текстовом поле
    property Text: string read InvokeString(()->tbx.Text) write Invoke(SetTextP, value);
    /// Разрешено ли несколько строк в текстовом поле
    property MultiLine: boolean read InvokeBoolean(()->tbx.TextWrapping = TextWrapping.Wrap) write Invoke(SetMultiLineP, value);
  end;

  /// Элемент управления "Целое поле"
  IntegerBoxWPF = class(TextBoxWPF)
  private
    function GetValue: integer;
    begin
      if Trim(Text) = '' then
        Result := 0
      else Result := integer.Parse(Text);
      Result := Result.Clamp(Minimum,Maximum);
    end;
    
    procedure SetValue(x: integer) := Text := x.Clamp(Minimum,Maximum).ToString;
    procedure Rest(min,max:integer);
    begin
      Invoke(()-> begin
        Self.Minimum := min;
        Self.Maximum := max;
        Value := Minimum;
        tbx.MouseWheel += (o, e) -> begin
          if e.Delta > 0 then
            Value := Value + 1
          else if e.Delta < 0 then
            Value := Value - 1
        end;
        tbx.KeyDown += (o, e) -> begin
          if not ((e.Key >= Key.D0) and (e.Key <= Key.D9)) then
            e.Handled := True;
        end;
      end);
    end;
  public 
    constructor Create(title: string := ''; min: integer := 0; max: integer := 10);
    begin
      inherited Create(title);
      Rest(min,max);
    end;
    constructor Create(x,y: real; title: string; min,max: integer; width: real := 0);
    begin
      inherited Create(x,y,title,width);
      Rest(min,max);
    end;
    
    /// Минимальное значение в тектовом поле
    auto property Minimum: integer := 0;
    /// Максимальное значение в тектовом поле
    auto property Maximum: integer := 100;
    /// Текущее значение текстового поля
    property Value: integer read GetValue write SetValue;
  end;

  /// Элемент управления "Список"
  ListBoxWPF = class(ControlWithTitleWPF)
  protected
    function lb: GListBox := MainElement as GListBox;
    procedure CreatePXY(x,y: real; title: string; width,height: real);
    begin
      Init1(new GListBox,title,width,x,y);
      //element.HorizontalAlignment := HorizontalAlignment.Stretch;
      Self.Height := height;
      lb.SelectionChanged += (o,e) -> if SelectionChanged<>nil then SelectionChanged;
    end;
    procedure CreateP(title: string; height: real) := CreatePXY(-1,-1,title,0,height);
    
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
    /// Событие, возникающее при изменении текущего выделения
    SelectionChanged: procedure;
    constructor Create(title: string := ''; height: real := 150):= Invoke(CreateP, title, height);
    constructor Create(x,y: real; title: string := ''; width: real := 150; height: real := 150):= Invoke(CreatePXY, x, y, title, width, height);
    /// Сортировка строк в списке по возрастанию
    procedure Sort := Invoke(SortP);
    /// Сортировка строк в списке по убыванию
    procedure SortDescending := Invoke(SortPDescending);
    /// Добавляет элемент в список
    procedure Add(s: string) := Invoke(AddP, s);
    /// Добавляет набор элементов в список
    procedure AddRange(params ss: array of string) := Invoke(AddRangeP, ss);
    /// Количество элементов в списке
    property Count: integer read InvokeInteger(()->lb.Items.Count);
    /// Индекс выбранного элемента
    property SelectedIndex: integer read InvokeInteger(()->lb.SelectedIndex) write Invoke(procedure(t: integer) -> lb.SelectedIndex := t, value);
    /// Текст выбранного элемента
    property SelectedText: string read InvokeString(()->(lb.SelectedItem as ListBoxItem).Content.ToString) 
      write Invoke(procedure(t: string) -> (lb.SelectedItem as ListBoxItem).Content := t, value);
  end;
  
  /// Элемент управления "Раскрывающийся список"
  ComboBoxWPF = class(ControlWithTitleWPF)
  private
    property cb: ComboBox read MainElement as ComboBox;
    
    procedure CreatePXY(x,y: real; title: string; width: real);
    begin
      Init1(new ComboBox, title, width, x, y);
      cb.SelectionChanged += (o,e) -> if SelectionChanged <> nil then SelectionChanged;
    end;
    procedure CreateP(title: string) := CreatePXY(-1,-1,title,0);
    procedure AddP(s: string);
    begin
      cb.Items.Add(s);
      if cb.Items.Count = 1 then
        cb.SelectedIndex := 0;
    end;
    procedure AddRangeP(params ss: array of string);
    begin
      foreach var s in ss do
        AddP(s);
    end;
  public 
    /// Событие, возникающее при изменении текущего выделения
    SelectionChanged: procedure;
    constructor Create(title: string := '') := Invoke(CreateP, title);
    constructor Create(x,y: real; title: string := ''; width: real := 120) := Invoke(CreatePXY, x, y, title,'', width);

    /// Добавляет элемент в список
    procedure Add(s: string) := Invoke(AddP, s);
    /// Добавляет набор элементов в список
    procedure AddRange(params ss: array of string) := Invoke(AddRangeP, ss);
    /// Выбранная строка
    function SelectedString := InvokeString(()->cb.SelectedItem as string);
  end;

  /// Элемент управления "Слайдер"
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
      sl.Value := val.Clamp(min,max);
      if freq<=0 then
        sl.TickFrequency := (max-min)/10
      else sl.TickFrequency := freq;
      sl.Foreground := Brushes.Black;
    end;
    procedure CreateP(min, max, val, freq: real) := CreatePXY(-1,-1,min, max, val, freq, 0);
    
    procedure ValueChangedP := if ValueChanged <> nil then ValueChanged;
  public 
    /// Событие, возникающее при изменении значения
    ValueChanged: procedure;
    constructor Create(min, max: real; val: real := real.MinValue; freq: real := 0) := Invoke(CreateP, min, max, val, freq);
    constructor Create(x,y: real; min, max: real; width: real; val: real := real.MinValue; freq: real := 0) := Invoke(CreatePXY, x,y,min, max, val, freq, width);
    /// Минимальное значение 
    property Minimum: real read GetMinimum write SetMinimum;
    /// Максимальное значение 
    property Maximum: real read GetMaximum write SetMaximum;
    /// Текущее значение 
    property Value: real read GetValue write SetValue;
    /// Интервал между делениями
    property Frequency: real read GetFrequency write SetFrequency;
  end;
  
  /// Элемент управления "Флажок"
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
    
    /// Текст элемента управления
    property Text: string read InvokeString(()->cb.Content as string) write Invoke(procedure(t: string) -> cb.Content := t, value);
    /// Включен ли флажок
    property Checked: boolean read InvokeBoolean(()->cb.IsChecked.Value) write Invoke(procedure(t: boolean) -> cb.IsChecked := t, value);
  end;

  /// Элемент управления "Радиокнопка"
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

    /// Текст элемента управления
    property Text: string read InvokeString(()->rb.Content as string) write Invoke(procedure(t: string) -> rb.Content := t, value);
    /// Включена ли радиокнопка
    property Checked: boolean read InvokeBoolean(()->rb.IsChecked.Value) write Invoke(procedure(t: boolean) -> rb.IsChecked := t, value);
  end;
  
  /// Элемент управления "Строка статуса"
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
    /// Добавить текстовое поле указанной ширины к строке статуса
    procedure AddText(text: string; itemWidth: real := 0) := Invoke(AddItemP,text,itemWidth);

    /// Текст основного текстового поля в строке статуса
    property Text: string read InvokeString(()->(sb.Items[0] as StatusBarItem).Content as string) 
      write Invoke(SetTextP,value);
    /// Текст i-того текстового поля в строке статуса
    property ItemText[i: integer]: string read InvokeString(()->(sb.Items[i] as StatusBarItem).Content as string)
      write Invoke(SetTextPI,i,value);
  end;
  
  /// Элемент управления "Канва"
  CanvasWPF = class(CommonControlWPF)
  protected
    property can: Canvas read element as Canvas;
    procedure CreatePP;
    begin
      element := new Canvas();
      element.Focusable := False;
      MainDockPanel.children.RemoveAt(MainDockPanel.children.Count-1);
      MainDockPanel.children.Add(can);
      ActivePanel := can;
    end;
    constructor (i: integer) := Invoke(CreatePP); // фиктивный первый параметр
    procedure SetColorP(c: GColor);
    begin
      can.Background := GetBrush(c); 
    end;
    function GetColorP := (can.Background as SolidColorBrush).Color;
  public 
    /// Цвет элемента управления
    property Color: GColor read Invoke&<GColor>(GetColorP) write Invoke(SetColorP, value);
  end;

function GetProperties<T>(t1: T): sequence of string;
function GetFields<T>(t1: T): sequence of string;

type  
  /// Элемент управления "Список данных"
  ListViewWPF = class(CommonControlWPF)
  private
    gv: GridView;
    property lv: ListView read element as ListView;
    procedure CreatePP;
    begin
      element := new ListView();
      element.Focusable := False;
      MainDockPanel.children.RemoveAt(MainDockPanel.children.Count-1);
      MainDockPanel.children.Add(lv);
      gv := new GridView;
      lv.View := gv;
    end;
    function ColumnCountP: integer := gv.Columns.Count;
    function RowCountP: integer := lv.Items.Count;
    constructor (i: integer);
    begin
      Invoke(CreatePP);
    end;
  public
    /// Заполнить элемент управления данными
    procedure Fill<T>(data: sequence of T); 
    begin
      Invoke(()-> begin
      var properties := GetProperties(data.First);
      gv.Columns.Clear;
      foreach var fld in properties do
      begin
        var col := new GridViewColumn; 
        col.Header := fld; 
        col.Width := 150; 
        col.DisplayMemberBinding := new System.Windows.Data.Binding(fld); 
        gv.Columns.Add(col); 
      end;
      lv.ItemsSource := data;
      end);
    end;
    /// Очистить данные
    procedure Clear;
    begin
      Invoke(()-> begin
        gv.Columns.Clear;
      end);
    end;
    /// Установить заголовки для данных
    procedure SetHeaders(params a: array of string);
    begin
      Invoke(()-> begin
        for var i:=0 to a.Length - 1 do
          gv.Columns[i].Header := a[i];
      end);
    end;
    /// Число столбцов данных
    function ColumnCount: integer := InvokeInteger(ColumnCountP);
    /// Число строк данных
    function RowCount: integer := InvokeInteger(RowCountP);
  end;

  ///!#
  /// Пункт меню
  MenuItemWPF = class
  private
    mi: MenuItem;
    litem := new List<MenuItemWPF>;
    procedure CreateP(mii: MenuItem; text: string);
    begin
      mi := mii;
      mi.Header := text;
      mi.Click += (o,e) -> begin
        if Click<>nil then Click;
      end;  
    end;
    constructor Create(mi: MenuItem; text: string) := Invoke(CreateP,mi,text);
    procedure SetTextP(text: string);
    begin
      mi.Header := text;
    end;
    function AddMenuItemP(text: string; p: procedure): MenuItemWPF;
    begin
      if text='-' then
      begin
        mi.Items.Add(new Separator);
        exit
      end;
      var mii := new MenuItem;
      mii.Header := text;
      mi.Items.Add(mii);
      mii.Click += (o,e)->if Click<>nil then Click;
      Result := new MenuItemWPF(mii,text);
      Result.Click := p;
      litem.Add(Result);
    end;
    procedure AddSeparatorP := mi.Items.Add(new Separator);
  public
    /// Событие выбора пункта меню
    Click: procedure;
    /// Текст пункта меню
    property Text: string read InvokeString(()-> mi.Header as string) write Invoke(SetTextP,text);
    /// Набор пунктов меню
    property Items[i: integer]: MenuItemWPF read Invoke&<MenuItemWPF>(()->litem[i]); default;
    /// Добавить пункт подменю
    function Add(text: string; p: procedure := nil): MenuItemWPF 
      := Invoke&<MenuItemWPF>(()->AddMenuItemP(text,p));

    /// Добавить разделитель
    procedure AddSeparator := Invoke(AddSeparatorP);
    /// Добавить набор пунктов меню
    procedure AddRange(params items: array of string);
    begin
      foreach var it in items do
        Add(it);
    end;
  end;
  
  /// Элемент управления "Главное меню"
  MenuWPF = class(CommonControlWPF)
  private
    litem := new List<MenuItemWPF>;
    property m: Menu read element as Menu;
    procedure CreatePP;
    begin
      element := new Menu();
      element.Height := 20;
      DockPanel.SetDock(m, Dock.Top);
      MainDockPanel.children.Insert(0,element);
    end;
    function AddMenuItemP(text: string): MenuItemWPF;
    begin
      var mi := new MenuItem;
      mi.Header := text;
      m.Items.Add(mi);
      Result := new MenuItemWPF(mi,text);
      litem.Add(Result);
    end;
  public
    constructor Create := Invoke(CreatePP);
    property Items[i: integer]: MenuItemWPF read Invoke&<MenuItemWPF>(()->litem[i]); default;

    /// Добавить пункт горизонтального меню
    function Add(text: string): MenuItemWPF := Invoke&<MenuItemWPF>(()->AddMenuItemP(text));
    /// Добавить набор пунктов меню
    procedure AddRange(params items: array of string);
    begin
      foreach var it in items do
        Add(it);
    end;
  end;
  
  ///!#
  /// Статический класс для создания главных элементов управления
  SetMainControl = static class
  public  
    static function AsImage: ImageWPF := new ImageWPF(0);
    static function AsCanvas: CanvasWPF := new CanvasWPF(0);
    static function AsListView: ListViewWPF := new ListViewWPF(0);
    static function AsTextBox: TextBoxNoTitleWPF := new TextBoxNoTitleWPF(0);
  end;
  
  ///!#
  /// Базовый  класс стандартных окон диалога
  FileDialogWPF = class
  private  
    d: Microsoft.Win32.FileDialog;
    procedure SetFilterP(value: string) := d.Filter := value;
    procedure SetInitialDirectoryP(value: string) := d.InitialDirectory := value;
    function ShowDialogP: boolean;
    begin
      var r := d.ShowDialog;
      Result := r.HasValue and r.Value;
    end;
  public
    function ShowDialog: boolean := InvokeBoolean(ShowDialogP);
    function FileName: string := InvokeString(()->d.FileName);
    property Filter: string read InvokeString(()->d.Filter) write Invoke(SetFilterP,value);
    property InitialDirectory: string read InvokeString(()->d.InitialDirectory) write Invoke(SetInitialDirectoryP,value);
  end;
  
  /// Стандартное окно диалога открытия файла
  OpenFileDialogWPF = class(FileDialogWPF)
  private  
    procedure CreateP(initDirectory,filter: string);
    begin
      d := new Microsoft.Win32.OpenFileDialog;
      d.InitialDirectory := initDirectory;
      d.Filter := filter;
    end;  
  public
    constructor Create(initDirectory: string := ''; filter: string := '') := Invoke(CreateP,initDirectory,filter);
  end;

  /// Стандартное окно диалога сохранения файла
  SaveFileDialogWPF = class(FileDialogWPF)
  private  
    procedure CreateP(initDirectory,filter: string);
    begin
      d := new Microsoft.Win32.SaveFileDialog;
      d.InitialDirectory := initDirectory;
      d.Filter := filter;
    end;  
  public
    constructor Create(initDirectory: string := ''; filter: string := '') := Invoke(CreateP,initDirectory,filter);
  end;
  
{-----------------------------------------------------}
// Для каждого контрола - пара функций. 
// Функция без координат и ширины рассчитана на создание внутри вертикальной StackPanel (LeftPanel, RightPanel)
// Panel - вспомогательная, нужна только для создания LeftPanel, RightPanel

/// Элемент управления "Кнопка" 
function Button(Txt: string; fontSize: real := 12): ButtonWPF;
/// Элемент управления "Кнопка" с заданными координатами
function Button(x, y: real; Txt: string; width: real := 0; fontSize: real := 12): ButtonWPF;

/// Элемент управления "Текст" 
function TextBlock(Txt: string; fontSize: real := 12): TextBlockWPF;
/// Элемент управления "Текст" с заданными координатами
function TextBlock(x, y: real; Txt: string; width: real := 0; fontSize: real := 12): TextBlockWPF;

/// Элемент управления "Целое число"
function IntegerBlock(message: string; initValue: integer := 0; fontSize: real := 12): IntegerBlockWPF;
/// Элемент управления "Целое число" с заданными координатами
function IntegerBlock(x, y: real; message: string; width: real := 0; initValue: integer := 0; fontSize: real := 12): IntegerBlockWPF;

/// Элемент управления "Вещественное число"
function RealBlock(message: string; initValue: real := 0; fontSize: real := 12): RealBlockWPF;
/// Элемент управления "Вещественное число" с заданными координатами
function RealBlock(x, y: real; message: string; width: real := 0; initValue: real := 0; fontSize: real := 12): RealBlockWPF;

/// Элемент управления "Текстовое поле с заголовком"
function TextBox(text: string := ''): TextBoxWPF;
/// Элемент управления "Текстовое поле с заголовком" с заданными координатами
function TextBox(x, y: real; text: string := ''; width: real := 0): TextBoxWPF;

/// Элемент управления "Целое поле"
function IntegerBox(title: string; max: integer := 10): IntegerBoxWPF;
/// Элемент управления "Целое поле" с заданным диапазоном значений
function IntegerBox(title: string; min,max: integer): IntegerBoxWPF;
/// Элемент управления "Целое поле" с заданными координатами
function IntegerBox(x, y: real; title: string; min,max: integer; width: real := 0): IntegerBoxWPF;

/// Элемент управления "Список"
function ListBox(title: string := ''; height: real := 150): ListBoxWPF;
/// Элемент управления "Список" с заданными координатами
function ListBox(x,y: real; title: string := ''; width: real := 150; height: real := 150): ListBoxWPF;

/// Элемент управления "Слайдер"
function Slider(): SliderWPF;
/// Элемент управления "Слайдер" с заданным диапазоном значений
function Slider(min,max: real; val: real := real.MinValue; freq: real := 0): SliderWPF;
/// Элемент управления "Слайдер" с заданными координатами
function Slider(x,y: real; min,max: real; width: real; val: real := real.MinValue; freq: real := 0): SliderWPF;

/// Элемент управления "Изображение"
function Image(name: string): ImageWPF;
/// Элемент управления "Изображение" с заданными координатами
function Image(x,y: real; name: string; width: real := 0): ImageWPF;

/// Элемент управления "Раскрывающийся список"
function ComboBox(title: string := ''): ComboBoxWPF;
/// Элемент управления "Раскрывающийся список" с заданными координатами
function ComboBox(x,y: real; title: string := ''; width: real := 120): ComboBoxWPF;

/// Элемент управления "Главное меню"
function Menu: MenuWPF;

/// Стандартное окно диалога открытия файла
function OpenFileDialog(initDirectory: string := ''; filter: string := ''): OpenFileDialogWPF;
/// Стандартное окно диалога сохранения файла
function SaveFileDialog(initDirectory: string := ''; filter: string := ''): SaveFileDialogWPF;

/// Элемент управления "Левая панель"
function LeftPanel(width: real := 150; c: Color := PanelsColor; InternalMargin: real := 10): PanelWPF;
/// Элемент управления "Правая панель"
function RightPanel(width: real := 150; c: Color := PanelsColor; InternalMargin: real := 10): PanelWPF;

/// Элемент управления "Строка статуса"
function StatusBar(height: real := 24; itemWidth: real := 0): StatusBarWPF;

/// Пустой блок для задания промежутков между компонентами
procedure EmptyBlock(sz: integer := 16);

/// Графическое окно
function Window: WindowTypeWPF;

implementation

uses GraphWPF;
uses System.Windows; 
uses System.Windows.Controls;
uses System.Windows.Controls.Primitives;
uses System.Windows.Media.Imaging;

function Window: WindowTypeWPF := GraphWPF.Window;

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

function IntegerBox(title: string; max: integer): IntegerBoxWPF := IntegerBoxWPF.Create(title,0,max);
function IntegerBox(title: string; min,max: integer): IntegerBoxWPF := IntegerBoxWPF.Create(title,min,max);
function IntegerBox(x, y: real; title: string; min,max: integer; width: real) := IntegerBoxWPF.Create(x,y,title,min,max,width);

function ListBox(title: string; height: real): ListBoxWPF := ListBoxWPF.Create(title,height);
function ListBox(x,y: real; title: string; width: real; height: real): ListBoxWPF := ListBoxWPF.Create(x,y,title,width,height);

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
  Result := SliderWPF.Create(x, y, min, max, width, val, freq);
end;  

function Image(name: string): ImageWPF := new ImageWPF(name);
function Image(x,y: real; name: string; width: real): ImageWPF := new ImageWPF(x,y,name,width);

function ComboBox(title: string): ComboBoxWPF := new ComboBoxWPF(title);
function ComboBox(x,y: real; title: string; width: real): ComboBoxWPF := new ComboBoxWPF(x,y,title,width);

function Menu: MenuWPF := new MenuWPF;

function OpenFileDialog(initDirectory: string; filter: string) := new OpenFileDialogWPF(initDirectory,filter);
function SaveFileDialog(initDirectory: string; filter: string) := new SaveFileDialogWPF(initDirectory,filter);

function LeftPanel(width: real; c: Color; internalMargin: real) := new LeftPanelWPF(width,c,internalMargin);
function RightPanel(width: real; c: Color; internalMargin: real) := new RightPanelWPF(width,c,internalMargin);

function StatusBar(height: real; itemWidth: real): StatusBarWPF := new StatusBarWPF(height,itemWidth);


procedure EmptyBlock(sz: integer);
begin
  var e := TextBlock('');
  e.Height := sz;
  e.Width := sz;
end;

function GetProperties<T>(t1: T): sequence of string;
begin
  var props := t1.GetType.GetProperties();
  Result := props.OrderBy(f->f.CustomAttributes.FirstOrDefault?.ToString ?? 'NoAttr').Select(f->f.Name);
end;

function GetFields<T>(t1: T): sequence of string;
begin
  var flds := t1.GetType.GetFields();
  //var flds := t1.GetType.GetFields(System.Reflection.BindingFlags.Instance or System.Reflection.BindingFlags.Public or System.Reflection.BindingFlags.NonPublic);
  Result := flds.OrderBy(f->f.CustomAttributes.FirstOrDefault?.ToString ?? 'NoAttr').Select(f->f.Name);
end;

procedure Invoke(d: System.Delegate; params args: array of object) := GraphWPFBase.Invoke(d, args);
procedure InvokeP(p: procedure(r: real); r: real) := GraphWPFBase.Invoke(p,r); 
function Invoke<T>(d: Func0<T>): T := T(app.Dispatcher.Invoke(d));

procedure SetActivePanelInit;
begin
  ActivePanel := MainWindow.MainPanel.Children[0] as GPanel
end;

begin
  Invoke(SetActivePanelInit);
end.