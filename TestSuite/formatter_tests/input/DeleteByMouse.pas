uses ABCObjects,GraphABC;

const CountSquares = 20;

var
  /// Текущая цифра
  CurrentDigit: integer;
  /// Количество ошибок
  Mistakes: integer;
  /// Строка информации
  StatusRect: RectangleABC;

/// Вывод информационной строки
procedure DrawStatusText;
begin
  if CurrentDigit<=CountSquares then
    StatusRect.Text := 'Удалено квадратов: ' + IntToStr(CurrentDigit-1) + '    Ошибок: ' + IntToStr(Mistakes)
  else StatusRect.Text := 'Игра окончена. Время: ' + IntToStr(Milliseconds div 1000) + ' с.    Ошибок: ' + IntToStr(Mistakes);
end;

/// Обработчик события мыши
procedure MyMouseDown(x,y,mb: integer);
begin
  var ob := ObjectUnderPoint(x,y);
  if (ob<>nil) and (ob is RectangleABC) then
    if ob.Number=CurrentDigit then
    begin
      ob.Destroy;
      Inc(CurrentDigit);
      DrawStatusText;
    end
    else
    begin
      ob.Color := clRed;
      Inc(Mistakes);
      DrawStatusText;
    end;
end;

begin
  Window.Title := 'Игра: удали все квадраты по порядку';
  Window.IsFixedSize := True;
  for var i:=1 to CountSquares do
  begin
    var x := Random(WindowWidth-50);
    var y := Random(WindowHeight-100);
    var ob := RectangleABC.Create(x,y,50,50,clMoneyGreen);
    ob.Number := i;
  end;
  StatusRect := RectangleABC.Create(0,Window.Height-40,Window.Width,40,Color.LightSteelBlue);
  CurrentDigit := 1;
  Mistakes := 0;
  DrawStatusText;
  // Установка обработчиков 
  OnMouseDown := MyMouseDown;
end.