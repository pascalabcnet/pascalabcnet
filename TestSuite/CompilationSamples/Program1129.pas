type
  t1 = class
    [System.Runtime.InteropServices.DllImport('kernel32.dll')]
    class function Beep(a,b: integer): boolean; external;//Подпрограмма должна иметь атрибут DllImport
  
  end;

begin 
  writeln(t1.Beep(200,400));
end.