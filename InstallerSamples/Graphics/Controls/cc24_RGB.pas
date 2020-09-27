// Смешение цветов RGB
uses GraphWPF,Controls;

begin
  Window.Title := 'Цвета';
  Font.Size := 40;
  LeftPanel(150);

  var r := Slider('Красный: ',0,255,255,16);
  var g := Slider('Зеленый: ',0,255,255,16);
  var b := Slider('Синий: ',0,255,255,16);
  
  Button('Выход').Click := procedure → Window.Close;

  var p: procedure := () → begin
    var c := RGB(r.Value,g.Value,b.Value);
    Window.Clear(c);
    DrawText(GraphWindow.ClientRect,$'R={c.R}, G={c.G}, B={c.B}');
  end;
  r.ValueChanged := p;
  g.ValueChanged := p;
  b.ValueChanged := p;
  p;
end.