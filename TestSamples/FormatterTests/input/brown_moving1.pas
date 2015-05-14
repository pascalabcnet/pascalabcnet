// Броуновское движение
uses GraphABC;

var x,y: integer;

procedure LineOn(dx,dy: integer);
begin
  if (x+dx>10) and (y+dy>10) and (x+dx<WindowWidth-10) and (y+dy<WindowHeight-10) then
  begin
    Line(x,y,x+dx,y+dy);
    x:=x+dx;
    y:=y+dy;
  end;  
end;

var i: integer;

const len=3;

begin
  SetWindowCaption('Броуновское движение');
  SetWindowSize(640,480);
  SetBrushColor(clBlack);
  FillRect(0,0,WindowWidth div 2,WindowHeight-1);
  SetFontColor(RGB(255,255,255));
  x:=Windowwidth div 2;
  y:=WindowHeight div 2;
{  Sleep(1000);}
  TextOut(5,5,'Начало!');
  MoveTo(x,y);
  for i:=1 to 100000 do
  begin
//    if i mod 5 = 0 then Sleep(1);
    if i mod 1000 = 0 then TextOut(5,25,IntToStr(i)+' итераций');
    SetPenColor(RGB(Random(256),Random(256),Random(256)));
    LineOn(Random(2*len+1)-len,Random(2*len+1)-len)
  end;  
  TextOut(5,45,'Конец!');
//  writeln(Milliseconds);
end.
