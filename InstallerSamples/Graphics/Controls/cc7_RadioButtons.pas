// Модуль Controls - переключатели
uses WPFObjects,Controls;

begin
  Window.Title := 'Модуль Controls - переключатели';
  LeftPanel(150,Colors.Beige);
 
  var tb := TextBlock('Цвет круга:'); 
  //tb.Margin := 12;
  
  var rb1 := new RadioButtonWPF('Красный');
  var rb2 := new RadioButtonWPF('Зелёный');
  var rb3 := new RadioButtonWPF('Синий');
  
  var c := new CircleWPF(GraphWindow.Center,60,Colors.White,1);
  
  rb1.Click := procedure -> begin
    c.Color := Colors.Red;
  end;
  rb2.Click := procedure -> begin
    c.Color := Colors.Green;
  end;
  rb3.Click := procedure -> begin
    c.Color := Colors.Blue;
  end;
end.