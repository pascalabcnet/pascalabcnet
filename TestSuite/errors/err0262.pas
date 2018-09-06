type
  t1=class
  
    private class function MessageBox<T>(wnd: System.IntPtr; message, caption: string; flags: cardinal):integer; 
    external 'User32.dll';
    
  end;

begin
  
end.