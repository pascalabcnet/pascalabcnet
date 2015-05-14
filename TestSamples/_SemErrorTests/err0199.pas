uses System;

[System.Runtime.InteropServices.DllImport('MyLib.dll')]
procedure Test; external 'MyLib.dll';

begin
  
end.