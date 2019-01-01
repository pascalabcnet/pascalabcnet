// Рекурсивное рисование двоичного дерева
uses GraphWPF;

const 
  LevelHeight = 50;
  Levels = 8;
  delay = 10;

procedure DrawTree(x,y,dx: real; level: integer);
// обход: левое поддерево, корень, правое
begin
  if level>0 then 
  begin
    DrawTree(x-dx,y+LevelHeight,dx / 2,level-1);
    Line(x,y,x-dx,y+LevelHeight);
    Line(x,y,x+dx,y+LevelHeight);
    Sleep(delay);
    DrawTree(x+dx,y+LevelHeight,dx / 2,level-1);
  end;
end;

begin
  Window.Title := 'Рекурсивное рисование бинарного дерева';
  Window.SetSize(800,30+Levels*LevelHeight);
  DrawTree(Window.Width / 2,10,Window.Width / 5,Levels);
end.
