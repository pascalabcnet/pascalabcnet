unit TwoPanelsWindow;

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

function GetProperties<T>(t1: T): sequence of string;
begin
  var props := t1.GetType.GetProperties;
  Result := props.OrderBy(f->f.CustomAttributes.FirstOrDefault?.ToString ?? 'NoAttr').Select(f->f.Name);
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

type 
  ///!#
  Button = class
  private
    btn := new System.Windows.Controls.Button;
    procedure p;
    begin
      if Click<>nil then
        Click;
    end;
  public
    Click: procedure;
    constructor (Content: string);
    begin
      LeftPanel.Children.Add(btn);
      btn.HorizontalAlignment := HorizontalAlignment.Stretch;
      btn.Margin := new Thickness(5); 
      btn.Content := Content; 
      btn.Click += procedure (sender: object; args: RoutedEventArgs) ->
      begin
        p;
      end;
    end;
  end;
  
  ///!#
  ListView = class
  private
    lv := new System.Windows.Controls.ListView;
  public
    constructor ();
    begin
      var gv := new GridView;
      lv.View := gv;
      RightPanel.Children.Add(lv);
    end;
    procedure Отобразить<T>(data: sequence of T); 
    begin
      var gv := lv.View as GridView;
      var properties := GetProperties(data.First);
      gv.Columns.Clear;
      foreach var fld in properties do
      begin
        var col := new GridViewColumn; 
        col.Header := fld; 
        col.Width := 150; 
        col.DisplayMemberBinding := new Binding(fld); 
        gv.Columns.Add(col); 
      end;
      lv.ItemsSource := data.Skip(1);
    end;
  end;

initialization
  InitWPF;
  Init;
finalization  
  app.Run();
end.