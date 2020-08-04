// Модуль Controls - флажки
uses WPFObjects,Controls;

begin
  Window.Title := 'Модуль Controls - флажки';
  
  LeftPanel(170);
 
  var tb := TextBlock('Параметры круга:'); 
  tb.Margin := 12;
  
  var cb1 := new CheckBoxWPF('Заливка');
  var cb2 := new CheckBoxWPF('Жирная граница');
  var cb3 := new CheckBoxWPF('С текстом');
  
  var c := new CircleWPF(GraphWindow.Center,60,Colors.White,1);
  
  cb1.Click := procedure -> begin
    if cb1.Checked then 
      c.Color := Colors.Green
    else c.Color := Colors.White
  end;
  cb2.Click := procedure -> begin
    if cb2.Checked then 
      c.BorderWidth := 4
    else c.BorderWidth := 1
  end;
  cb3.Click := procedure -> begin
    if cb3.Checked then 
      c.Text := 'Текст'
    else c.Text := ''
  end;
end.