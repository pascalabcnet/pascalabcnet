// Модуль Controls - StatusBar
uses Controls,GraphWPF;

begin
  Window.Title := 'Модуль Controls - StatusBar';
  var l := LeftPanel(150,Colors.Orange);
  l.Tooltip := 'Измените размеры окна';
  var sb := StatusBar(24,55);
  sb.AddText('',55);
  Button('Очистить поле W').Click := procedure -> sb.ItemText[0] := '';
  Button('Очистить поле H').Click := procedure -> sb.ItemText[1] := '';
  OnResize := () -> begin
    sb.ItemText[0] := 'W=' + Window.Width.Round;
    sb.ItemText[1] := 'H=' + Window.Height.Round;
  end;
  OnResize();
end.