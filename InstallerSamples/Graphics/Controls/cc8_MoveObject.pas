// Модуль Controls - кнопки управления перемещением + клавиатура
uses WPFObjects,Controls;

begin
  Window.Title := 'Модуль Controls - кнопки управления перемещением + клавиатура';

  LeftPanel;

  var c := new CircleWPF(GraphWindow.Center,30,Colors.Blue);
  
  OnKeyDown := k -> begin
    case k of
      Key.Left: c.MoveOn(-2,0);
      Key.Right: c.MoveOn(2,0);
      Key.Up: c.MoveOn(0,-2);
      Key.Down: c.MoveOn(0,2);
    end;
  end;
  
  var l := Button('Left');
  var r := Button('Right');
  var u := Button('Up');
  var d := Button('Down');
  l.Click := ()->c.MoveOn(-2,0);
  r.Click := ()->c.MoveOn(2,0);
  u.Click := ()->c.MoveOn(0,-2);
  d.Click := ()->c.MoveOn(0,2);
end.