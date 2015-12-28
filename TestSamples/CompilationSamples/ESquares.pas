// E-квадраты. Демонстрация рекурсии
uses GraphABC;

const mw = 2.9;

procedure EKv(n,x,y,w: integer);
var w1,h: integer;
begin
  w1:=round(w/mw);
  h := (w-2*w1) div 3;
  Rectangle(x,y,x+w,y+w);
  if n>0 then
  begin
//    Sleep(1);
    SetBrushColor(clRandom);
    EKv(n-1,x+h,y+h,w1);
    EKv(n-1,x+w-h-w1,y+h,w1);
    EKv(n-1,x+h,y+w-h-w1,w1);
    EKv(n-1,x+w-h-w1,y+w-h-w1,w1);
  end;
end;

var s: string;
    r: integer;
begin
  SetWindowCaption('Рекурсия: квадраты');
  SetWindowSize(750,530);
  SetPenColor(clWhite);
  SetBrushColor(clRandom);
  EKv(4,125,18,490);
end.
