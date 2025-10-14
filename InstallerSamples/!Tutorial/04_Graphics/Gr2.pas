// Графика. Точка. Случайный цвет. Скорость работы
uses GraphABC;

begin
  for var i:=1 to 100000 do
    SetPixel(Random(Window.Width),Random(Window.Height),clRandom);
  Println('Время работы:',Milliseconds/1000,'секунд');  
end.