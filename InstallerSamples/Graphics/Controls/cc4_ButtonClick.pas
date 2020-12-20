// Модуль Controls - кнопки и их обработчики
uses GraphWPF,Controls;

begin
  Window.Title := 'Модуль Controls - кнопки и их обработчики';
  var p := LeftPanel;
  p.Color := Colors.Orange;
 
  Button('Случайный цвет').Click := () -> begin
    Window.Clear(RandomColor);
  end;
  
  var b2 := Button('Плюс');
  b2.Click := procedure -> 
    if b2.Text = 'Плюс' then b2.Text := 'Минус' else b2.Text := 'Плюс';

  var b1 := Button('Закрыть окно');
  b1.Click := procedure -> Window.Close;
end.