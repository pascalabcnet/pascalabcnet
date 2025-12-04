// Мозаика. Квадратики случайным образом меняются местами
uses GraphABC;

const 
  w  = 25;
  w1 = 1;
  m  = 50;
  n  = 70;
  x0 = 0;
  y0 = 0;
  delay = 10;

var a: array [0..n,0..m] of Color;

begin
  Window.Title := 'Мозаика';
  Window.SetSize(800,600);
  
  // Заполнение массива случайными цветами
  for var i:=0 to n-1 do
  for var j:=0 to m-1 do
  begin
    a[i,j] := clRandom;
    Brush.Color := a[i,j];
    FillRect(x0+i*w,y0+j*w,x0+(i+1)*w-w1,y0+(j+1)*w-w1);
  end;
  
  var k := 0;
  while true do
  begin
    k += 1;
    if k mod 1000 = 0 then 
    begin
      k := 0;
      Sleep(delay);
    end;  
    
    var i := Random(1,n-2);
    var j := Random(1,m-2);
    var di := Random(-1,1);
    var dj := Random(-1,1);
    var i1 := i+di; 
    var j1 := j+dj;
    Swap(a[i,j],a[i1,j1]);
    
    Brush.Color := a[i,j];
    FillRect(x0+i*w,y0+j*w,x0+(i+1)*w-w1,y0+(j+1)*w-w1);
    Brush.Color := a[i1,j1];
    FillRect(x0+i1*w,y0+j1*w,x0+(i1+1)*w-w1,y0+(j1+1)*w-w1);
  end;
end.
