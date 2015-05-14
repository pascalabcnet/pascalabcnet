// Изменение свойств объекта StarABC
#reference 'ABCObjectsLib.dll'
#reference 'System.Drawing.dll'

uses ABCObjectsLib;

var z: ABCObjectsLib.StarABC;

begin
  z := new ABCObjectsLib.StarABC(200,200,190,210,6,System.Drawing.Color.Red);
  for var i:=1 to 20 do
  begin
    Sleep(100);
    z.Count := z.Count + 1;
  end;
  for var i:=1 to 180 do
  begin
    Sleep(10);
    z.Angle := z.Angle + 1;
  end;
end.
