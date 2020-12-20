// Модуль Controls - меню
uses Controls,GraphWPF;

begin
  LeftPanel(150, Colors.Orange);
  var m := new MenuWPF;
  var mi1 := m.Add('File');
  mi1.Add('New');
  mi1.Add('Open');
  mi1.Add('Save');
  mi1.AddSeparator;
  mi1.Add('Exit').Click := procedure->Window.Close;
  //mi1.Add('Exit',procedure->Window.Close);
  m.Add('Edit');
  m.Add('Options');
  var mi := m.Add('Help');
end.