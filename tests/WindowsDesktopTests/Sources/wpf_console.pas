{$platform WPF}
uses System.Windows.Controls;
begin
  assert(System.Threading.Thread.CurrentThread.GetApartmentState = System.Threading.ApartmentState.STA);
  var b := new Button;
  b.Content := 'Console WPF';
  Println('DESKTOP_OK');
end.
