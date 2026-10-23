unit u_forms_lookup;
{$platform WinForms}
interface
procedure CheckForms;
implementation
uses System.Windows.Forms;
procedure CheckForms;
begin
  if System.Environment.TickCount = -1 then MessageBox.Show('test');
  assert(typeof(MessageBox).FullName = 'System.Windows.Forms.MessageBox');
end;
end.
