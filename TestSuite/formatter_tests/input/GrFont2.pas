// Демонстрация использования TextWidth и TextHeight
uses GraphABC;

const s = 'Width & Height';
  
begin
  Window.Title := 'Текст по центру';
  Window.IsFixedSize := True;
  SetWindowSize(700,300);
  Window.Center;
  Font.Name := 'Times';
  Font.Size := 50;
  var tw := TextWidth(s);
  var th := TextHeight(s);
  TextOut((Window.Width-tw) div 2,(Window.Height-th) div 2,s);
end.
