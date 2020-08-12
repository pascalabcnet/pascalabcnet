// Модуль Controls - режим Wrapping для TextBlock
uses GraphWPF,Controls;

function R := Random(-7,7);

begin
  Window.Title := 'Модуль Controls - режим Wrapping для TextBlock';
  LeftPanel(150,Colors.Orange);
  var b := Button('No Wrap');
  b.Tooltip := 'Нажмите для изменения режима переноса слов';
  
  var tb1 := TextBlock('Этот текст не помещается на одной строке и в режиме Wrapping автоматически переносится');
  b.Click := procedure → begin
    tb1.Wrapping := not tb1.Wrapping;
    b.Text := if tb1.Wrapping then 'Wrap' else 'No Wrap';
  end;
  var tb := TextBlock('Этот текст усекается,'#10'но можно явно'#10'разделять на строки');
end.