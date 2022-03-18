uses WPFObjects;

const
  /// отступ по оси x
  zx = 100;
  /// отступ по оси y
  zy = 50;

begin
  Window.IsFixedSize := True;
  Window.Title := 'Секундомер';
  var r := new RoundRectWPF(zx, zy, Window.Width - 2 * zx, Window.Height - 2 * zy, 100, Colors.LightGreen, 5, Colors.Green);
  r.FontSize := 200;
  for var i := 1 to 1000 do
  begin
    r.RealNumber := i/10;
    Sleep(100);
  end;
end.

