// Градиентная заливка. Выполнена интерполяцией по каждой составляющей цвета.
uses GraphABC;

var i,j: integer;
    r,g,b:array[0..2] of integer;
    field:array[0..256*3] of Color;
    
function LinearInterp(a,b,fa,fb,x: integer): integer;
var t: real;
begin
  t:=(x-a)/(b-a);
  LinearInterp:=round(fa*(1-t)+fb*t);
end;

begin
  r[0]:=255;
  g[0]:=0;
  b[0]:=255;
  
  r[1]:=255;
  g[1]:=255;
  b[1]:=0;
  
  r[2]:=0;
  g[2]:=255;
  b[2]:=255;{}
  
  for i:=0 to 255 do 
    for j:=0 to 2 do 
      Field[i+255*j]:=RGB(LinearInterp(0,255,r[j],r[(j+1) mod 3],i),
   			  	      	LinearInterp(0,255,g[j],g[(j+1) mod 3],i),
      			      	LinearInterp(0,255,b[j],b[(j+1) mod 3],i));

  SetWindowCaption('Градиент');
  SetWindowSize(256*3,400);
  LockDrawing;
  for i:=0 to 256*3 do
  	Line(i,0,i,WindowHeight-1,Field[i]);
  UnlockDrawing;
end.
