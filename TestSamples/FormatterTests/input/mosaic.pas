// Мозаика. Квадратики случайным образом меняются местами
uses GraphABC;

const 
  w=25;
  w1=1;
  m=50;
  n=70;
  x0=0;
  y0=0;

var i,j,i1,j1,di,dj,k: integer;
    a: array [0..n,0..m] of Color;
    v : Color;
begin
 SetWindowSize(800,800);
  for i:=0 to n-1 do
  for j:=0 to m-1 do
  begin
    for k:=1 to 3 do
    a[i][j]:=RGB(Random(256),Random(256),Random(256));
    SetBrushColor(a[i][j]);
    FillRectangle(x0+i*w,y0+j*w,x0+(i+1)*w-w1,y0+(j+1)*w-w1);
  end;
  while true do
  begin
    k:=k+1;
    if k mod 1000 = 0 then 
    begin
      k:=0;
      Sleep(1);
    end;  
    i:=Random(n-2)+1;
    j:=Random(m-2)+1;
    di:=Random(3)-1;
    dj:=Random(3)-1;
    i1:=i+di; j1:=j+dj;
    v:=a[i][j];
    a[i][j]:=a[i1][j1];
    a[i1][j1]:=v;
    SetBrushColor(a[i][j]);
    FillRectangle(x0+i*w,y0+j*w,x0+(i+1)*w-w1,y0+(j+1)*w-w1);
    SetBrushColor(a[i1][j1]);
    FillRectangle(x0+i1*w,y0+j1*w,x0+(i1+1)*w-w1,y0+(j1+1)*w-w1);
  end;
end.
