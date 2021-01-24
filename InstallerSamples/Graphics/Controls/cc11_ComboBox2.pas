// Модуль Controls - элемент управления "ComboBox" и графики функций
uses GraphWPF,Controls;

begin
  Window.Title := 'Модуль Controls - элемент управления "ComboBox" и графики функций';
  LeftPanel(150, Colors.Orange);
  var cb := ComboBox('Графики функций');
  cb.AddRange('x*sin(x)','exp(x)','x*x','sin(x)-cos(2.5*x)');
  
  var Redraw: procedure := () -> begin
    case cb.SelectedIndex of
      0: DrawGraph(x->x*sin(x));
      1: DrawGraph(x->exp(x));
      2: DrawGraph(x->x*x);
      3: DrawGraph(x->sin(x)-cos(2.5*x));
    end;
  end;
  
  cb.SelectionChanged := Redraw;
  OnResize := Redraw;
  Redraw;
end.