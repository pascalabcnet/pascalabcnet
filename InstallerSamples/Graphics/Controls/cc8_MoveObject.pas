// Модуль Controls - кнопки управления перемещением + клавиатура
uses WPFObjects,Controls;

begin
  Window.Title := 'Модуль Controls - кнопки управления перемещением + клавиатура';

  LeftPanel;

  var c := new CircleWPF(GraphWindow.Center,30,Colors.Blue);
  
  OnKeyDown := k -> begin
    case k of
      Key.Left: c.MoveBy(-2,0);
      Key.Right: c.MoveBy(2,0);
      Key.Up: c.MoveBy(0,-2);
      Key.Down: c.MoveBy(0,2);
    end;
  end;
  
  var l := Button('Left');
  var r := Button('Right');
  var u := Button('Up');
  var d := Button('Down');
  l.Click := ()->c.MoveBy(-2,0);
  r.Click := ()->c.MoveBy(2,0);
  u.Click := ()->c.MoveBy(0,-2);
  d.Click := ()->c.MoveBy(0,2);
end.