uses WPFObjects;

const CountSquares = 20;

var
  /// Текущая цифра
  CurrentDigit: integer;
  /// Количество ошибок
  Mistakes: integer;
  /// Строка информации
  StatusRect: RectangleWPF;

/// Вывод информационной строки
procedure DrawStatusText;
begin
  if CurrentDigit<=CountSquares then
    StatusRect.Text := $'Удалено квадратов: {CurrentDigit-1}     Ошибок: {Mistakes}'
  else StatusRect.Text := $'Игра окончена. Время: {Milliseconds div 1000} с.    Ошибок: {Mistakes}';
end;

/// Обработчик события мыши
procedure MyMouseDown(x,y: real; mb: integer);
begin
  var ob := ObjectUnderPoint(x,y);
  if (ob<>nil) and (ob is RectangleWPF) and (ob<>StatusRect) then
    if ob.Number=CurrentDigit then
    begin
      ob.Destroy;
      Inc(CurrentDigit);
      DrawStatusText;
    end
    else
    begin
      ob.Color := Colors.Red;
      Inc(Mistakes);
      DrawStatusText;
    end;
end;

begin
  Window.Title := 'Игра: удали все квадраты по порядку';
  for var i:=1 to CountSquares do
  begin
    var x := Random(Window.Width-50);
    var y := Random(Window.Height-100);
    var ob := RectangleWPF.Create(x,y,50,50,Colors.LightGreen,1);
    ob.FontSize := 25;
    ob.Number := i;
  end;
  StatusRect := RectangleWPF.Create(0,Window.Height-40,Window.Width,40,Colors.LightBlue);
  CurrentDigit := 1;
  Mistakes := 0;
  DrawStatusText;
  // Установка обработчиков 
  OnMouseDown := MyMouseDown;
end.