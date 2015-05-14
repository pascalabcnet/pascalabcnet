uses GraphABC;

var i: integer;

begin
  SetWindowSize(700,500);
  LockDrawing;
  for i:=1 to 500 do
  begin
    SetBrushColor(clGreen);
    FillEllipse(i,100,i+100,200);
    Redraw;
    Sleep(1);
    SetBrushColor(clWhite);
    FillRect(i,100,i+100,200);
  end;
end.
