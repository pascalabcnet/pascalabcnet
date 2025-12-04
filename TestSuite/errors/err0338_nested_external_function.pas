procedure p1;
  function GetDC(hwnd: System.IntPtr): System.IntPtr;
  external 'user32.dll';
begin
  GetDC(System.IntPtr.Zero);
end;

begin
  p1;
end.