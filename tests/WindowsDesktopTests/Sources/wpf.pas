{$platform WPF}
{$apptype windows}
uses System.Windows, System.Windows.Controls, System.Windows.Media;
begin
  assert(System.Threading.Thread.CurrentThread.GetApartmentState = System.Threading.ApartmentState.STA);
  var b := new Button;
  b.Content := 'WPF smoke';
  b.Width := 120;
  b.Background := Brushes.Cyan;
  var p := new Point(2, 3);
  assert(p.X = 2);
  assert(b.Width = 120);
  Println('DESKTOP_OK');
end.
