// Графика. Линии. Размеры окна. Заголовок окна
uses GraphABC;

begin
  Window.Title := 'Первая графическая программа';
  Line(0,0,Window.Width-1,Window.Height-1);
  Line(0,Window.Height-1,Window.Width-1,0);
end.