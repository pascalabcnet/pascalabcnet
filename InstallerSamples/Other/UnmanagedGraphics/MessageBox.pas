uses System.Runtime.InteropServices;

[DllImport('User32.dll', CharSet = CharSet.Unicode)]
function MessageBox(h: integer; m,c: string; t: integer): integer; external;

begin
  MessageBox(0,'Привет!','Сообщение',0);
end.