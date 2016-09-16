unit ГрафическиеИсполнители;

uses FormsABC;

type
  WebBrowser2 = class(WebBrowser)
  public
    procedure Отобразить(адрес: string) := Navigate(адрес);
    function ОтобразитьСайт(адрес: string): () -> () := 
      () -> Self.Отобразить(адрес);
  end;
  
  Button2 = class(Button)
  private
    procedure ppp(sender: Object; e: System.EventArgs);
  public
    event ПриНажатии: procedure;
    constructor Create(text: string);
    begin
      inherited Create(text);
      b.Click += ppp;
    end;
    property Текст: string read GetText write SetText;
  end;
  
  ListBox2 = class(ListBox)
  private
    procedure ppp(sender: Object; e: System.EventArgs);
    function GetHeight := lb.Height;
    procedure SetHeight(h: integer) := lb.Height := h;
  public
    event ПриНажатии: procedure;
    constructor Create;
    begin
      inherited Create;
      lb.Click += ppp;
      {lb.AutoSize := False;
      lb.Width := 160;}
      lb.Width := 190;
    end;
    procedure Добавить(name: string) := Items.Add(name);
    procedure Очистить := Items.Clear;
    procedure ДобавитьМного(m: sequence of string);
    begin
      foreach var x in m do
        Items.Add(x);
    end;
    function ТекущаяСтрока := Items[Selectedindex] as string;
    property Высота: integer read GetHeight write SetHeight;
  end;
  TextLabel2 = class(TextLabel)
  public 
    property Текст: string read GetT write SetT;
  
  end;
  
  
procedure Button2.ppp(sender: Object; e: System.EventArgs);
begin
  if ПриНажатии<>nil then
    ПриНажатии
end;
  
procedure ListBox2.ppp(sender: Object; e: System.EventArgs);
begin
  if ПриНажатии<>nil then
    ПриНажатии
end;

procedure Init();
begin
  MainForm.SetSize(1024,768);
end;

function СоздатьКнопку(Заголовок: string): Button2;
begin
  ParentControl := MainPanel;
  Result := new Button2(Заголовок);
end;

function СоздатьСписок: ListBox2;
begin
  ParentControl := MainPanel;
  Result := new ListBox2;
end;

function СоздатьТекст(txt: string := ''): TextLabel2;
begin
  ParentControl := MainPanel;
  Result := new TextLabel2(txt);
end;

function СоздатьБраузер: WebBrowser2;
begin
  MainPanel.Dock := Dockstyle.Left;
  MainPanel.Width := 200;
  ParentControl := MainForm;
  Result := new WebBrowser2;
  Result.Dock := DockStyle.Fill;
end;

procedure НоваяСтрока := LineBreak;

begin
  Init();
end.