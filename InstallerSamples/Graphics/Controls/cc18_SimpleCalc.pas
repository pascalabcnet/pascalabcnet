// Модуль Controls - Калькулятор
uses Controls,GraphWPF;

begin
  Window.Title := 'Калькулятор с выводом в TextBox';
  LeftPanel(150, Colors.Orange);
  var tb := SetMainControl.AsTextBox;
  tb.FontSize := 30;
  var x := IntegerBox('X:');
  var y := IntegerBox('Y:');
  
  var cb := new ComboBoxWPF('Операция');
  cb.AddRange('+','-','*','/');
  
  var b := Button('+');
  b.Click := procedure -> begin
    case cb.SelectedString of
      string('+'): tb.Println($'{x.Value} + {y.Value} = {x.Value + y.Value}');
      string('-'): tb.Println($'{x.Value} - {y.Value} = {x.Value - y.Value}');
      string('*'): tb.Println($'{x.Value} * {y.Value} = {x.Value * y.Value}');
      string('/'): tb.Println($'{x.Value} / {y.Value} = {x.Value / y.Value}');
    end;
    x.Value := Random(0,10);
    y.Value := Random(0,10);
  end;
  
  cb.SelectionChanged := procedure -> begin
    b.Text := cb.SelectedString
  end;
end.