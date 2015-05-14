//(c) DarkStar 2008
uses GraphABC, Paporotnik, PaporotnikData;

const 
  Iterations = 300000;         
  Height     = 600;            
  Fast       = false;
  Width      = Height div 2;        
  WindowWidth= Width * 3;        
  Brightness = 170;

var 
  Paprotnik := new PaporotnikFractal(PaprotnikData);
  SimplePaprotnik := new PaporotnikFractal(SimplePaprotnikData);
  Elka := new PaporotnikFractal(ElkaData);

begin
  InitWindow(200, 50, WindowWidth , Height, clBlack);
  Paprotnik.Draw(0, 0, Iterations, Height, Brightness, fast);
  SimplePaprotnik.Draw(Width, 0, Iterations, Height, Brightness, fast);
  Elka.Draw(Width*2, 0, Iterations, Height, Brightness, fast);
end.