// Графика. Точка. Случайный цвет. Скорость работы
uses GraphABC,Utils;

begin
  for var i:=1 to 100000 do
    SetPixel(Random(Window.Width),Random(Window.Height),clRandom);
  writeln('Время работы: ',Milliseconds/1000,' секунд');  
end.