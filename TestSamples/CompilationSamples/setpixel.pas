// Демонстрация возможностей функции SetPixel.
uses GraphABC;

var i,j: integer;

begin
  SetWindowSize(300,200);
  for j:=0 to WindowHeight-1 do
  for i:=0 to WindowWidth-1 do
    SetPixel(i,j,RGB(i+j,i-j,i+2*j));

  for i:=0 to WindowWidth-1 do
  for j:=0 to WindowHeight-1 do
    SetPixel(i,j,RGB(2*i+j,i-2*j,i+2*j));

  for j:=0 to WindowHeight-1 do
  for i:=0 to WindowWidth-1 do
    SetPixel(i,j,RGB(0,0,i*i+j*j));
    
  for i:=0 to WindowWidth-1 do
  for j:=0 to WindowHeight-1 do
    if (i-150)*(i-150)+(j-100)*(j-100)<100*100
      then SetPixel(i,j,RGB(255-round(sqrt((i-100)*(i-100)+(j-50)*(j-50))),0,0))
      else SetPixel(i,j,clWhite);
    
  for j:=0 to WindowHeight-1 do
  for i:=0 to WindowWidth-1 do
    SetPixel(i,j,clRandom);
    
  ClearWindow;
  for j:=0 to 20000 do
  begin
    SetBrushColor(clRandom);
    FillRect(Random(WindowWidth),Random(WindowHeight),Random(WindowWidth),Random(WindowHeight));
  end;  

  ClearWindow;
  for j:=0 to 70000 do
    SetPixel(Random(WindowWidth),Random(WindowHeight),clRandom);

  SetBrushColor(clWhite);
  Rectangle(40,80,WindowWidth-40,WindowHeight-80);
  SetFontSize(14);
  SetFontName('Arial');
  SetFontColor(clRed);
  TextOut(50,WindowHeight div 2-14,'Модуль GraphABC!');
end.










