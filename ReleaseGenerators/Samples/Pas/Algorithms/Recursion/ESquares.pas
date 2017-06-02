// E-квадраты. Демонстрация рекурсии
uses GraphABC;

const mw = 2.9;

procedure ESquares(n,x,y,w: integer);
begin
  var w1 := round(w/mw);
  var h := (w-2*w1) div 3;
  Brush.Color := clRandom;
  Rectangle(x,y,x+w,y+w);
  if n>0 then
  begin
    Sleep(1);
    ESquares(n-1,x+h,y+h,w1);
    ESquares(n-1,x+w-h-w1,y+h,w1);
    ESquares(n-1,x+h,y+w-h-w1,w1);
    ESquares(n-1,x+w-h-w1,y+w-h-w1,w1);
  end;
end;

begin
  Window.Title := 'Рекурсия: E-квадраты';
  SetWindowSize(750,530);
  Pen.Color := clWhite;
  ESquares(4,125,18,490);
end.
