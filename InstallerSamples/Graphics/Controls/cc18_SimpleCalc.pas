// Модуль Controls - Калькулятор
uses Controls,GraphWPF;

begin
  Window.Title := 'Калькулятор с выводом в TextBox';
  LeftPanel(150, Colors.Orange);
  var tb := SetMainControl.AsTextBox;
  tb.FontSize := 30;
  var x := IntegerBox('X:');
  var y := IntegerBox('Y:');
  x.Value := Random(0,10);
  y.Value := Random(0,10);
  
  var cb := new ComboBoxWPF('Операция');
  cb.AddRange('+','-','*','/');
  
  var count := 0;
  var sb := StatusBar;
  sb.Text := 'Количество вычислений: ' + count;
  
  var b := Button('+');
  b.Click := procedure -> begin
    case cb.SelectedText of
      '+': tb.Println($'{x.Value} + {y.Value} = {x.Value + y.Value}');
      '-': tb.Println($'{x.Value} - {y.Value} = {x.Value - y.Value}');
      '*': tb.Println($'{x.Value} * {y.Value} = {x.Value * y.Value}');
      '/': tb.Println($'{x.Value} / {y.Value} = {x.Value / y.Value}');
    end;
    x.Value := Random(0,10);
    y.Value := Random(0,10);
    count += 1;
    sb.Text := 'Количество вычислений: ' + count;
  end;
  
  Button('Очистить').Click := () -> begin
    count := 0;
    sb.Text := 'Количество вычислений: ' + count;
    tb.Clear;
  end;
  
  cb.SelectionChanged := procedure -> begin
    b.Text := cb.SelectedText
  end;
end.