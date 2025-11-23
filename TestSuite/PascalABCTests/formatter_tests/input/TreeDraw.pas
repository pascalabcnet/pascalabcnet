// Рекурсивное рисование двоичного дерева
uses GraphABC;

const 
  LevelHeight = 50;
  Levels = 8;
  delay = 10;

procedure DrawTree(x,y,dx,level: integer);
// обход: левое поддерево, корень, правое
begin
  if level>0 then 
  begin
    DrawTree(x-dx,y+LevelHeight,dx div 2,level-1);
    Line(x,y,x-dx,y+LevelHeight);
    Line(x,y,x+dx,y+LevelHeight);
    Sleep(delay);
    DrawTree(x+dx,y+LevelHeight,dx div 2,level-1);
  end;
end;

begin
  Window.Title := 'Рекурсивное рисование бинарного дерева';
  SetSmoothingOff;
  SetWindowSize(800,30+Levels*LevelHeight);
  DrawTree(WindowWidth div 2,10,WindowWidth div 5,Levels);
end.
