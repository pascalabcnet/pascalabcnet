// Модуль Controls - StatusBar
uses Controls,WPFObjects;

// Управление свойствами объекта, связанными с перемещением и поворотом.
// В OnDrawFrame объект сам себя перерисовывает
begin
  Window.Title := 'Модуль Controls - StatusBar';
  var l := LeftPanel(180,Colors.Orange);
  
  var c := new RectangleWPF(50,200,80,50,Colors.Green);
  c.Direction := (1,0);
  c.Velocity := 0;
  c.Tag := 0; // скорость поворота
  
  OnDrawFrame := dt -> begin
    c.MoveTime(dt);  
    c.RotateAngle += integer(c.Tag) * dt;
  end;

  Button('Начать перемещение').Click := procedure -> begin
    c.Velocity := 30;
  end;
  Button('Закончить перемещение').Click := procedure -> begin
    c.Velocity := 0;
  end;
  Button('Начать поворот').Click := procedure -> begin
    c.Tag := 30;
  end;
  Button('Закончить поворот').Click := procedure -> begin
    c.Tag := 0;
  end;
end.