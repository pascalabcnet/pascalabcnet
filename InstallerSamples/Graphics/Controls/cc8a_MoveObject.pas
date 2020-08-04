// Модуль Controls - IntegerBlock, IntegerBox и перемещение объекта
uses WPFObjects,Controls;

begin
  Window.Title := 'Модуль Controls - IntegerBlock, IntegerBox и перемещение объекта';

  LeftPanel;

  var c := new CircleWPF(300,300,30,Colors.Blue);
  
  var X := IntegerBox('X:',0,600);
  X.Value := 300;
  var Y := IntegerBox('Y:',0,600);
  Y.Value := 300;

  var XX := IntegerBlock('X:',300);
  var YY := IntegerBlock('Y:',300);
  
  X.ValueChanged := procedure → begin
    c.Center := Pnt(X.Value,Y.Value);
    XX.Value := X.Value;
  end;
  Y.ValueChanged := procedure → begin
    c.Center := Pnt(X.Value,Y.Value);
    YY.Value := Y.Value;
  end;
end.