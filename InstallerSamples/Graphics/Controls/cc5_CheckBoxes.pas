// Модуль Controls - флажки
uses GraphWPF,Controls;

begin
  Window.Title := 'Модуль Controls - флажки';
  
  var p := LeftPanel(170,Colors.Orange);
 
  var b1 := Button('Переключить флажок 1');
  var b2 := Button('Переключить флажок 2');
  var b3 := Button('Переключить флажок 3');

  var cb1 := new CheckBoxWPF('Флажок 1');
  var cb2 := new CheckBoxWPF('Флажок 2');
  var cb3 := new CheckBoxWPF('Флажок 3');
  
  b1.Click := procedure -> cb1.Checked := not cb1.Checked;
  b2.Click := procedure -> cb2.Checked := not cb2.Checked;
  b3.Click := procedure -> cb3.Checked := not cb3.Checked;
end.