// Модуль Controls - меню 2
uses Controls,GraphWPF;

begin
  LeftPanel(150, Colors.Orange);
  var m := new MenuWPF;
  m.AddRange('File','Edit','Options','Help');
  m[0].AddRange('New','Open','Save','-','Exit');
  m[0][3].Click := procedure->Window.Close;
end.