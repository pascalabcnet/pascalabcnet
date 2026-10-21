{$platform WinForms}
{$apptype windows}
uses System.Windows.Forms, System.Drawing;
begin
  var f := new Form;
  f.Text := 'WinForms smoke';
  f.ClientSize := new Size(120, 80);
  assert(f.Text = 'WinForms smoke');
  assert(f.ClientSize.Width = 120);
  f.Dispose;
  Println('DESKTOP_OK');
end.
