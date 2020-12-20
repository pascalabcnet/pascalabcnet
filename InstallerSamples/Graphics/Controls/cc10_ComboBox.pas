// Модуль Controls - элемент управления "Список" и использование словаря
uses Controls,GraphWPF;

begin
  Window.Title := 'Столицы стран';
  LeftPanel(150,Colors.Orange);
  var l := ComboBox('Список стран');
  l.Tooltip := 'Выбери страну - отобразится столица';
  l.Add('Россия');
  l.Add('США');
  l.Add('Китай');
  l.Add('Германия');
  l.Add('Франция');
  
  var d := Dict(('Россия','Москва'),
    ('США','Вашингтон'),
    ('Китай','Пекин'),
    ('Германия','Берлин'),
    ('Франция','Париж')
  );
  
  Font.Size := 70;
  l.SelectionChanged := procedure -> begin
    Window.Clear;
    DrawText(GraphWindow.ClientRect,d[l.SelectedText]);
  end;
end.