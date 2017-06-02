// Демонстрация работы со шрифтами
uses GraphABC;

begin
  Window.Title := 'Шрифты';
  SetWindowSize(760,460);
  Font.Name := 'Arial';
  Font.Style := fsBoldItalic;
  for var i:=4 to 14 do
  begin
    Font.Size := 2*i;
    Font.Color := clRandom;
    TextOut(30,2*i*i-15,'PascalABC.NET');
  end;
  Font.Name := 'Times New Roman';
  Font.Style := fsBoldUnderline;
  for var i:=4 to 14 do
  begin
    Font.Size := 2*i;
    Font.Color := clRandom;
    TextOut(400,2*i*i-15,'PascalABC.NET');
  end;
end.
