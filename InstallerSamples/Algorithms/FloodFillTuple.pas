uses GraphABC;

procedure DrawCell(a: array [,] of integer; x,y: integer);
var sz := 30;
begin
  case a[y,x] of
0: Brush.Color := Color.White;
1: Brush.Color := Color.Black;
2: Brush.Color := Color.Chocolate;
  end;
  Rectangle(x*sz,y*sz,x*sz+sz-1,y*sz+sz-1)
end;

procedure AddToQueue(a: array [,] of integer; x,y: integer; q: Queue<(integer,integer)>);
begin
  if a[y,x] = 0 then
  begin
    q.Enqueue(new Point(x,y));
    a[y,x] := 2;
    Sleep(200);
    DrawCell(a,x,y);
  end;
end;

procedure FloodFill(a: array [,] of integer; x,y: integer);
begin
  var q := new Queue<(integer,integer)>;
  AddToQueue(a,x,y,q);
  while not (q.Count=0) do
  begin
    (x,y) := q.Dequeue();
    AddToQueue(a,x+1,y,q);
    AddToQueue(a,x-1,y,q);
    AddToQueue(a,x,y+1,q);
    AddToQueue(a,x,y-1,q);
  end;
end;

procedure ReadFromFile(fname: string; var a: array [,] of integer);
begin
  var f := OpenRead(fname);
  var dimx,dimy: integer;
  readln(f,dimy,dimx);
  SetLength(a,dimy,dimx);
  for var y := 0 to dimy-1 do
  begin
    for var x := 0 to dimx-1 do
    begin
      var c := f.ReadChar;
      a[y,x]:= c='*' ? 1 : 0;
    end;
    readln(f)  
  end;
  f.Close;
end;

procedure Draw(a: array [,] of integer);
begin
  for var y := 0 to a.GetLength(1)-1 do
  for var x := 0 to a.GetLength(0)-1 do
    DrawCell(a,x,y);
end;

begin
  Window.Title := 'Иллюстрация алгоритма FloodFill';
  var a: array [,] of integer;
  ReadFromFile('field.txt',a);
  Draw(a);
  FloodFill(a,4,4);
end.