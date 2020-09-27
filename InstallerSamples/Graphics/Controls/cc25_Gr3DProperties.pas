// Модуль Controls 
uses Graph3D, Controls;

begin
  var l := LeftPanel(200, Colors.LightGoldenrodYellow);
  
  var height := IntegerBox('Высота:', 1, 15);
  height.Value := 5;
  height.Tooltip := 'Покрутите колёсико мыши';
  var radius := IntegerBox('Радиус:', 1, 6);
  radius.Value := 2;
  radius.Tooltip := 'Покрутите колёсико мыши';
  var sides := Slider('Количество сторон: ', 3, 20);
  sides.Frequency := 1;
  sides.Value := 5;
  
  var p := Pyramid(Origin, sides.Value, height.Value, radius.Value, Colors.Green);
  
  height.ValueChanged := procedure -> begin
    p.Height := height.Value;
  end;
  radius.ValueChanged := procedure -> begin
    p.Radius := radius.Value;
  end;
  sides.ValueChanged := procedure -> begin
    p.Sides := sides.Value;
  end;
end.