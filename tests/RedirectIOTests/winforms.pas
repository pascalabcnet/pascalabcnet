// TEST-REGRESSION: keep forwarding WinForms UI exceptions to the IDE protocol.
{$reference System.Windows.Forms.dll}
uses __RedirectIOMode, System.Windows.Forms;
begin
  Application.OnThreadException(new System.Exception('redirect-winforms-test'));
  Println('redirect-winforms-ok');
end.
