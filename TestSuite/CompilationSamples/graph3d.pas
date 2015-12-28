// Пример из пакета KuMir/PMir
// Публикуется практически без изменений
// Дорог как память :)
Uses GraphABC;

var Xmin,Xmax,Xstep: real;
    Ymin,Ymax,Ystep,asp: real;
    dx: integer;

function f(x,y:real): integer;
var r: real;
begin
  r := x*x+y*y+1;
  f := round(5*asp*(cos(r)/r+0.1))
end;

procedure gr(N : integer);
var X,Y: real; 
    i,j,k,Z0,dy: integer; 
    pred: array [1..100] of integer;
    jj,maxX,maxY: integer;
begin
  Xmin := -4;
  Xmax := 4;
  Ymin := -3;
  Ymax := 3;
  maxX := 600;
  maxY := 400;
  Xstep := dx*(Xmax-Xmin)/maxX; 
  X := Xmin;
  Ystep := (Ymax-Ymin)/N;     
  Y := Ymin;
  dy := maxY div N div 2;        
  asp := maxY/8;
  for i := 1 to N do
  begin
    pred[i] := maxY-i*dy-f(X,Y);
    Y := Y + Ystep
  end;
  for jj := 1 to maxX div dx do 
  begin
    j := jj*dx;
    X := X + Xstep;
    Y := Ymin; Z0 := maxY;
    for i := 1 to N do 
    begin
      k := maxY-i*dy-f(X,Y);
      if k<Z0 then
      begin
        Line(j-dx,pred[i],j,k);
        Z0 := k
      end;
      pred[i] := Z0;
      Y := Y+Ystep
    end;
  end;
end;

begin
  SetWindowCaption('График функции двух переменных');
  SetWindowSize(600,400);
  dx := 2; { разрешение по оси X }
  gr(100); { количество линий по Y <= MaxN }
end.
