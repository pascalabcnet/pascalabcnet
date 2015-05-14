uses GraphABC;

//  Фрактальный папоротник

const 
  Iterations = 1000000;         
  Height =     900;            
  Brightness = 200;
  Width =      Height div 2;            
  Size =       Height/10 - 2;  
  Dx =         Width div 2 - 15;
  //неправильное позиционирование курсора
  Dc =         Iterations div Brightness;
  data: array [0..19] of real = (
        0.00,  0.85,  0.20, -0.15,
        0.00,  0.04, -0.26,  0.28,
        0.00, -0.04,  0.23,  0.26,
        0.16,  0.85,  0.22,  0.24,
        0.00,  1.60,  1.60,  0.44);

begin
  SetWindowSize(Width, Height);
  SetWindowPos(200, 50);
  SetBrushColor(clBlack);
  FillRectangle(0, 0, Width, Height);
  var plotx, ploty, x, y : real;
  for var i:=1 to Iterations do begin
    var pick := Random(101) - 84;    
    var rndnum := pick<=0 ? 1 : pick<=7 ? 2 : pick<=15 ? 3 : 0;
    plotx := data[    rndnum]*x + data[4  + rndnum]*y;
    ploty := data[8 + rndnum]*x + data[12 + rndnum]*y + data[16 + rndnum];
    x := plotx;
    y := ploty;
    SetPixel(Round(plotx*Size) + dx, Round(ploty*Size), RGB(0, byte(i div Dc), 0));
  end;
end.