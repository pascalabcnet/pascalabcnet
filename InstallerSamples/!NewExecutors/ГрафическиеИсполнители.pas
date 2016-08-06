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
  end;
  
procedure Button2.ppp(sender: Object; e: System.EventArgs);
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

function СоздатьБраузер: WebBrowser2;
begin
  MainPanel.Dock := Dockstyle.Left;
  MainPanel.Width := 130;
  ParentControl := MainForm;
  Result := new WebBrowser2;
  Result.Dock := DockStyle.Fill;
end;


begin
  Init();
end.