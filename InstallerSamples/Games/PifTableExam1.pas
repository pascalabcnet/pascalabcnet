uses GraphWPF,Controls,Sounds;

function Оценка(ВерныхОтветов: integer): integer;
begin
  case ВерныхОтветов of
    10, 9: Оценка := 5;
    8, 7:  Оценка := 4;
    4..6:  Оценка := 3;
    else   Оценка := 2;
  end;  
end;

begin
  Window.Title := 'Проверка таблицы умножения';
  Font.Size := 60;
  var l := LeftPanel(200);
  l.FontSize := 16;
  var sb := StatusBar;

  var Результат := IntegerBox('Введите ответ:',0,100);
  var b := Button('Ответить');
  var КоличествоОтветов := IntegerBlock('Ответов:');
  var ВерныхОтветов := IntegerBlock('Верных ответов:');
  
  var ТестОкончен := procedure → begin
    MessageBox.Show('Тест окончен.'+#10#10+'Верных ответов: ' + 
      ВерныхОтветов.Value + #10#10 + 'Оценка: ' + Оценка(ВерныхОтветов));
    // Переход к следующему тесту
    ВерныхОтветов := 0;
    КоличествоОтветов := 0;
  end;
  
  var x, y: integer; // сомножители 

  var НарисоватьТест := procedure → begin
    Window.Clear;
    DrawText(GraphWindow.ClientRect,$'{x} × {y} = ?');
  end;

  var СледующийВопрос := procedure → begin
    (x, y) := Random2(2, 9);
    НарисоватьТест;
  end;
  СледующийВопрос;
  
  b.Click := procedure → begin
    if x * y = Результат then
    begin
      sb.Text := 'Верно!';
      ВерныхОтветов += 1;
    end  
    else 
    begin
      sb.Text := 'Неверно :(';
    end;  
    КоличествоОтветов += 1;
    Результат := 0;
    if КоличествоОтветов = 10 then // Конец опроса
      ТестОкончен;
    СледующийВопрос;
  end;
  OnResize := НарисоватьТест;
end.