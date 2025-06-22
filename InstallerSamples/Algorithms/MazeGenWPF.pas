uses GraphWPF;

const 
  w = 60;        // ширина в клетках
  h = 40;        // высота в клетках
  cellSize = 10; // размер клетки в пикселях
  wallColor = Colors.Black;
  pathColor = Colors.White;
  startColor = Colors.Green;
  endColor = Colors.Red;

var maze := MatrGen(w*2+1, h*2+1, (x,y) -> True);

var dirs := Arr(
    (0, -2), // Up
    (2,  0), // Right
    (0,  2), // Down
    (-2, 0)  // Left
  );

procedure DrawCell(x, y: integer; c: Color)
  := FillRectangle(x*cellSize/2, y*cellSize/2, 
       cellSize/2, cellSize/2, c);

procedure GenerateMaze(x, y: integer);
begin
  maze[x, y] := false;
  
  dirs.Shuffle;
  
  foreach var (dx, dy) in dirs do
  begin
    var (nx, ny) := (x + dx, y + dy);
    
    if (nx in 1..w*2-1) and (ny in 1..h*2-1) and maze[nx, ny] then
    begin
      var (wx, wy) := (x + dx div 2, y + dy div 2);
      maze[wx, wy] := false;
      GenerateMaze(nx, ny);
    end;
  end;
end;

procedure DrawMaze;
begin
  Window.SetSize(w*cellSize + 5, h*cellSize + 5);
  Window.Clear(Colors.White);
  
  for var x := 0 to w*2 do
    for var y := 0 to h*2 do
      if maze[x, y] then
        DrawCell(x, y, wallColor)
      else DrawCell(x, y, pathColor);
  
  // Вход и выход
  DrawCell(0, 1, startColor);
  DrawCell(w*2, h*2-1, endColor);
end;

begin
  Window.Title := 'Генератор случайного лабиринта';
  GenerateMaze(1, 1);
  DrawMaze;
end.