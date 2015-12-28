//Рисование фракталов семейства "Лист папоротника"
//(c) DarkStar 2007

uses GraphABC, 
     PaporotnikData;   //Лист папоротника
     //Paporotnik2Data;  //Прямой лист папоротника
     //ElkaData;         //Елка

const 
  Iterations = 1000000;         
  Height =     900;            
  Brightness = 170;
  Width =      Height div 2;            
  Size =       Height/11;
  dx =         Width div 2;
	dc =         Iterations div Brightness;
	
begin
  InitWindow(200, 50, Width, Height, clBlack);
  var plotx, ploty, x, y : real;
  for var i:=1 to Iterations do begin
    var P := Random(100);    
    var rnd := P<P0 ? 0 : P<P1+P0 ? 1 : P<P2+P1+P0 ? 2 : 3;
    plotx := data[rnd,0]*x + data[rnd,1]*y;
    ploty := data[rnd,2]*x + data[rnd,3]*y + data[rnd,5];
    x := plotx;
    y := ploty;
    SetPixel(Round(x*Size) + dx, Height - Round(y*Size), GreenColor(byte(30 + (i div dc))));
  end;
end.