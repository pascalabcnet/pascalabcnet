// Демонстрация работы со шрифтами
uses GraphABC;

begin
  Window.Title := 'Шрифты';
  SetWindowSize(750,520);
  Font.Name := 'Arial';
  Font.Style := fsBoldItalic;
  for var i:=4 to 15 do
  begin
    Font.Size := 2*i;
    Font.Color := clRandom;
    TextOut(50,2*i*i-15,'Pascal ABC');
  end;
  Font.Name := 'Times New Roman';
  Font.Style := fsBoldUnderline;
  for var i:=4 to 15 do
  begin
    Font.Size := 2*i;
    Font.Color := clRandom;
    TextOut(450,2*i*i-15,'Pascal ABC');
  end;
end.
