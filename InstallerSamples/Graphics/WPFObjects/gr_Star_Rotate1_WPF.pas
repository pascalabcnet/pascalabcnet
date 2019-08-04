// Изменение свойств объекта StarWPF
uses WPFObjects;

begin
  var z := new StarWPF(Window.Width / 2, Window.Height / 2, Window.Height / 2 - 5, Window.Height / 4 + 16, 6, Colors.Red);
  loop 20 do
  begin
    Sleep(100);
    z.Count += 1;
  end;
  z.AnimRotate(200,2);
end.
