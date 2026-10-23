unit u_wpf_lookup;
{$platform WPF}
interface
procedure CheckWpf;
implementation
uses System.Windows;
procedure CheckWpf;
begin
  if System.Environment.TickCount = -1 then MessageBox.Show('test');
  assert(typeof(MessageBox).FullName = 'System.Windows.MessageBox');
end;
end.
