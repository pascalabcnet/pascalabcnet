{$reference System.Windows.Forms.dll}
{$reference System.Drawing.dll}

uses System, System.Drawing, System.Windows.Forms;

type
  TestForm = class(Form)
    procedure Form1_Load(sender: Object; e: EventArgs);
  {$region FormDesigner}
  private
    {$resource Form1.TestForm.resources}
    {$include Form1.TestForm.inc}
  {$endregion FormDesigner}
  public
    constructor;
    begin
      InitializeComponent;
    end;
  end;

procedure TestForm.Form1_Load(sender: Object; e: EventArgs);
begin
  Close;
end;

begin
  System.Windows.Forms.Application.EnableVisualStyles();
  System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
  System.Windows.Forms.Application.Run(new TestForm)
end.