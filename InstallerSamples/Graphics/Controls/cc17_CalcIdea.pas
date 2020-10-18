// Модуль Controls - Калькулятор Light 
uses Controls,GraphWPF;

begin
  Window.Title := 'Калькулятор Light';
  LeftPanel(150, Colors.Orange);
  var tb := SetMainControl.AsTextBox;
  tb.FontSize := 30;
  var x := IntegerBox('X:',1,9);
  var y := IntegerBox('Y:',1,9);
  Button('Sum').Click := procedure -> begin
    tb.Println($'{x.Value} + {y.Value} = {x.Value + y.Value}');
  end;
end.