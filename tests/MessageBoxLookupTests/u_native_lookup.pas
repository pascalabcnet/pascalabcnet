unit u_native_lookup;
interface
procedure CheckNative;
implementation
uses System.Runtime.InteropServices, System.Text;
function MessageBox(wnd: integer; text, caption: string; flags: integer): integer;
  external 'user32.dll' name 'MessageBoxA';
procedure CheckNative;
begin
  MessageBox(0, 'test' + System.Environment.MachineName, 'test', 16);
end;
end.
