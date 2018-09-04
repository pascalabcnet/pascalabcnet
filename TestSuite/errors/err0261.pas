type
  t1<T>=class
  
    private class function MessageBox(wnd: System.IntPtr; message, caption: string; flags: cardinal):integer; 
    external 'User32.dll';
    
  end;

begin
  var a := new t1<byte>;
end.