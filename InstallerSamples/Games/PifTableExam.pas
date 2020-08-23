uses GraphWPF,Controls,Sounds;

var x, y: integer;

procedure NextQuestion;
begin
  Window.Clear;
  (x, y) := Random2(2, 9);  
  DrawText(GraphWindow.ClientRect,$'{x} × {y} = ?');
end;

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
  Font.Size := 40;
  var l := LeftPanel(200);
  l.FontSize := 16;
  var sb := StatusBar;
  NextQuestion;

  var Результат := IntegerBox('Введите ответ:',0,100);
  var b := Button('Ответить');
  var КоличествоОтветов := IntegerBlock('Ответов:');
  var ВерныхОтветов := IntegerBlock('Верных ответов:');
  
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
    begin
      MessageBox.Show('Тест окончен.'#10#10'Верных ответов: ' + 
        ВерныхОтветов.Value + #10#10 + 'Оценка: ' + Оценка(ВерныхОтветов));
      // Переход к следующему тесту
      ВерныхОтветов := 0;
      КоличествоОтветов := 0;
    end;
    NextQuestion;
  end;
  OnResize := procedure → begin
    Window.Clear;
    DrawText(GraphWindow.ClientRect,$'{x} × {y} = ?');
  end;
end.