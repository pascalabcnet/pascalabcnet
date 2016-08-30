// Программа генерации случайных лабиринтов
uses GraphABC;

const
  szw = 70;      // размер лабиринта
  szh = 50;
  cellsz = 10;   // размер ячейки

type
  point = record
    x,y: integer;
  end;

var
  maze: array [0..szw-1] of array [0..szh-1] of integer;
  todo: array [0..szw*szh-1] of point;
  todonum: integer;

const
  dx: array [0..3] of integer = (0, 0, -1, 1);
  dy: array [0..3] of integer = (-1, 1, 0, 0);

procedure Init;
begin
  for var x:=0 to szw-1 do
  for var y:=0 to szh-1 do
    if (x=0) or (x=szw-1) or (y=0) or (y=szh-1) then
      maze[x][y]:=32
    else maze[x][y]:=63;

  var x := Random(szw-2)+1;
  var y := Random(szh-2)+1;

// Пометить клетку как принадлежащую лабиринту
  maze[x][y]:= maze[x][y] and not 48;

// Занести в список todo все ближайшие необработанные клетки
  for var d:=0 to 3 do
    if (maze[x + dx[d]][y + dy[d]] and 16) <> 0 then
    begin
      todo[todonum].x := x + dx[d];
      todo[todonum].y := y + dy[d];
      Inc(todonum);
      maze[x + dx[d]][y + dy[d]] := maze[x + dx[d]][y + dy[d]] and not 16;
    end;

   // Пока не обработаны все клетки
   while todonum > 0 do
   begin
     // Выбрать из списка todo произвольную клетку
     var n := Random(todonum);
     x := todo[n].x;
     y := todo[n].y;

     // Удалить из списка обработанную клетку
     Dec(todonum);
     todo[n]:= todo[todonum];

     // Выбрать направление, которое ведет к лабиринту
     var dd: integer;
     repeat
       dd:=Random (4);
     until not ((maze[x + dx[dd]][y + dy[dd]] and 32) <> 0);

     // Присоединить выбранную клетку к лабиринту
     maze[x][y] := maze[x][y] and not ((1 shl dd) or 32);
     maze[x + dx[dd]][y + dy[dd]] := maze[x + dx[dd]][y + dy[dd]] and not (1 shl (dd xor 1));

     // Занести в список todo все ближайшие необработанные клетки
     for var d:=0 to 3 do
       if (maze[x + dx[d]][y + dy[d]] and 16) <> 0 then
       begin
         todo[todonum].x := x + dx[d];
         todo[todonum].y := y + dy[d];
         Inc(todonum);
         maze[x + dx[d]][y + dy[d]] := maze[x + dx[d]][y + dy[d]] and not 16;
       end;
   end;

   maze[1][1] := maze[1][1] and not 1;                 // начало лабиринта - в левом верхнем углу
   maze[szw-2][szh-2] := maze[szw-2][szh-2] and not 2; // конец лабиринта - в правом нижнем углу
end;

procedure Draw;
begin
  for var x:=1 to szw-2 do
  for var y:=1 to szh-2 do
  begin
   if ((maze[x][y] and 1) <> 0) then // верхняя стена
     Line(x * cellsz, y * cellsz, x * cellsz + cellsz , y * cellsz);
   if ((maze[x][y] and 2) <> 0) then // нижняя стена
     Line(x * cellsz, y * cellsz + cellsz, x * cellsz + cellsz , y * cellsz + cellsz);
   if ((maze[x][y] and 4) <> 0) then // левая стена
     Line(x * cellsz, y * cellsz, x * cellsz, y * cellsz + cellsz );
   if ((maze[x][y] and 8) <> 0) then // правая стена
     Line(x * cellsz + cellsz, y * cellsz, x * cellsz + cellsz, y * cellsz + cellsz);
  end;
end;

begin
  Window.Title := 'Генерация лабиринта';
  SetWindowSize(szw*cellsz,szh*cellsz);
  Init;
  Draw;
end.

