{$reference 'System.Windows.Forms.dll'}
{$reference 'System.Drawing.dll'}

uses System, System.Windows.Forms;

type TForm = class(Form)
constructor Create;
begin
  writeln;
  inherited;
end;
end;

begin
  Application.Run(new Form);
end.