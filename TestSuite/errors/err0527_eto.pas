//!Сборка 'Microsoft.WindowsAPICodePack.Shell.dll' не найдена
{$apptype windows}
{$reference Eto.dll}
{$reference Eto.WinForms.dll}
uses Eto.Forms;

type MyForm = class(Form)
  constructor;
  begin
    Title := 'My Cross-Platform App';
		ClientSize := new Eto.Drawing.Size(200, 200);
		var lbl := new &Label();
		lbl.Text := 'Test';
		Content := lbl;
  end;
end;
begin
  (new Application()).Run(new MyForm());
end.