// Модуль Controls - замена графической панели на элемент управления "Канва"
uses Controls,GraphWPF;

begin
  Window.Title := 'Модуль Controls - замена графической панели на элемент управления "Канва"';
  var left := LeftPanel(150,Colors.Orange);
  var can := SetMainControl.AsCanvas;
  can.Color := Colors.AntiqueWhite;

  Button(100,100,'На Канве можно располагать элементы');
  Button(50,300,'в любом месте,');
  TextBlock(150,500,'указывая их координаты');
  
  // Смена активной панели
  SetActivePanel(left);
  Button('Закрыть').Click := procedure → Window.Close;
end.