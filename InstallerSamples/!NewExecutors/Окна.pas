unit ќкна;

{$reference 'PresentationFramework.dll'}
{$reference 'WindowsBase.dll'}
{$reference 'PresentationCore.dll'}

{$apptype windows}

uses System.Windows.Controls;
uses System.Windows; 
uses System.Windows.Data; 
uses System.Reflection;
uses System.Collections.ObjectModel;


var app := new Application();
var MainWindow := new Window;

procedure InitWPF;
begin
  MainWindow.Title := 'WPF';
  MainWindow.WindowStartupLocation := WindowStartupLocation.CenterScreen;
  MainWindow.Width := 800;
  MainWindow.Height := 600;
  MainWindow.Show;
end;

function GetFields<T>(t1: T): sequence of string;
begin
  Result := t1.GetType.GetProperties.Select(f->f.Name);
end;

procedure ќтобразить<T>(Self: ListView; data: sequence of T); extensionmethod;
begin
  var gv := Self.View as GridView;
  var fields := GetFields(data.First);
  gv.Columns.Clear;
  foreach var fld in fields do
  begin
    var col := new GridViewColumn; 
    col.Header := fld; 
    col.Width := 150; 
    col.DisplayMemberBinding := new Binding(fld); 
    gv.Columns.Add(col); 
  end;
  Self.ItemsSource := data.Skip(1);
end;

var 
  LeftPanel: StackPanel;
  RightPanel: DockPanel;
  
procedure Init();
begin
  var dp := new DockPanel;
  MainWindow.Content := dp; 

  LeftPanel := new StackPanel();
  LeftPanel.Width := 150;
  dp.Children.Add(LeftPanel);
  
  RightPanel := new DockPanel();
  dp.Children.Add(RightPanel);
end;

function CreateListView(): ListView;
begin
  var list := new ListView;
  var gv := new GridView;
  list.View := gv;
  RightPanel.Children.Add(list);
  Result := list;
end;

function —оздатьќкно—писка: ListView;
begin
  Result := CreateListView;
end;


function CreateButton(Content: string; p: procedure): Button;
begin
  var btn := new Button;
  LeftPanel.Children.Add(btn);
  btn.HorizontalAlignment := HorizontalAlignment.Stretch;
  btn.Margin := new Thickness(5); 
  btn.Content := Content; 
  btn.Click += procedure (sender: object; args: RoutedEventArgs) ->
  begin
    p;
  end;
  Result := btn;
end;

function —оздать нопку(Content: string; p: procedure): Button;
begin
  Result := CreateButton(Content,p);
end;



initialization
  InitWPF;
  Init;
finalization  
  app.Run();
end.