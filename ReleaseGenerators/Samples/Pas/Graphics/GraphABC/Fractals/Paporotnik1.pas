// Демонстрация фрактальной графики. Папоротник
// Для каждой точки комплексной плоскости z=(x,y) выполняем итерационный процесс z=z^2+c, c=(cx,cy)
// Считаем количество итераций i до тех пор пока не выполнится условие |x|>max и |y|>max
// После этого рисуем точку x,y с насыщенностью красного цвета, пропорциональной i 
uses GraphABC,Utils;

const
  max = 10;
  cx = 0.251;
  cy = 0.95;
  coef1 = 0.5;
  coef2 = 0.88;
 
  scalex = 0.001;
  scaley = 0.001;
  dx = 200;
  dy = 130;

begin
  Window.Title := 'Фракталы: папоротник';
  SetWindowSize(800,600);
  CenterWindow;
  for var ix:=0 to Window.Width-1 do
  for var iy:=0 to Window.Height-1 do
  begin
    var x := scalex*(ix-dx);
    var y := scaley*(iy-dy);
    var i := 1;
    while i<255 do
    begin
      var x1 := coef1*x*x-coef2*y*y+cx;
      var y1 := x*y+cy;
      x := x1;
      y := y1;
      if (abs(x)>max) and (abs(y)>max) then break;
      i += 1;
    end;
    if i>=255 then SetPixel(ix,iy,clRed)
      else SetPixel(ix,iy,RGB(255,255-i,255-i));
  end;
  writeln('Время расчета = ',Milliseconds/1000,' с');
end.
