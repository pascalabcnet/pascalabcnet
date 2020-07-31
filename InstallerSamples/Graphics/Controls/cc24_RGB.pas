// Смешение цветов RGB
uses GraphWPF,Controls;

begin
  Window.Title := 'Цвета';
  Font.Size := 40;
  LeftPanel(150);

  var rb := IntegerBlock('Красный:');
  rb.Margin := 3;
  var r := Slider(0,255,255,16);
  var gb := IntegerBlock('Зеленый:');
  gb.Margin := 3;
  var g := Slider(0,255,255,16);
  var bb := IntegerBlock('Синий:');
  bb.Margin := 3;
  var b := Slider(0,255,255,16);
  
  Button('Выход').Click := procedure -> Window.Close;

  var p: procedure := ()-> begin
    Window.Clear(RGB(r.Value.Round,g.Value.Round,b.Value.Round));
    rb.Value := r.Value.Round;
    gb.Value := g.Value.Round;
    bb.Value := b.Value.Round;
    DrawText(GraphWindow.ClientRect,$'R={rb.Value}, G={gb.Value}, B={bb.Value}');
  end;
  r.ValueChanged := p;
  g.ValueChanged := p;
  b.ValueChanged := p;
  p;
end.