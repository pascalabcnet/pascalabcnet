#reference 'PresentationFramework.dll'
#reference 'PresentationCore.dll'
#reference 'WindowsBase.dll'
#reference 'System.Windows.Forms.dll'
#reference 'System.Drawing.dll'
#reference 'System.Xml.dll'

uses System.Windows, System.Windows.Controls, System.Windows.Markup, System.Windows.Media, System.Threading;

type Window1 = class(Window)
  private button1 : Button;
  
  procedure button1_Click(sender : object; e : RoutedEventArgs);
  begin
    button1.Content := 'Thank you';
  end;
  
  procedure InitComponent;
  begin
    self.Width := 285;
    self.Height := 285;
    self.Left := 100;
    self.Top := 100;
    self.Title := 'Test Window';
    var panel := new DockPanel();
    button1 := new Button();
    button1.Content := 'Click me';
    button1.Click += button1_Click;
    button1.Margin := new System.Windows.Thickness(30);
    var container : IAddChild := panel;
    //container.AddChild(button1);
    container := self;
    container.AddChild(panel);
    
    
  end;
  
  public constructor Create;
  begin
    InitComponent;
  end;
 
end;

type TProgram = class(Application)

end;

var prog : TProgram;

procedure RunThread;
begin
  prog := new TProgram();
  prog.MainWindow := new Window1();
  prog.MainWindow.ShowDialog();
end;

begin
  var th := new Thread(RunThread);
  th.SetApartmentState(System.Threading.ApartmentState.STA);
  th.Start();
end.