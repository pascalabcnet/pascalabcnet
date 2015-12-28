// Рекурсивное рисование двоичного дерева
uses GraphABC;

const dy=60;

procedure DrawTree(x,y,dx,level: integer);
// обход: левое, корень, правое
begin
  if level=0 then Exit;
  DrawTree(x-dx,y+dy,dx div 2,level-1);
  Line(x,y,x-dx,y+dy);
  Line(x,y,x+dx,y+dy);
  Sleep(10);
  DrawTree(x+dx,y+dy,dx div 2,level-1);
end;

begin
  DrawTree(WindowWidth div 2,10,190,8);
end.
