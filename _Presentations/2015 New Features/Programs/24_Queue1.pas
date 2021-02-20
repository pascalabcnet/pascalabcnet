uses GraphABC;

procedure FloodFill(x,y: integer);
begin
  var c := Color.Green;
  var q := new Queue<Point>;
  q.Enqueue(Pnt(x,y));
  var v := 1;
  while q.Count<>0 do
  begin
    var p := q.Dequeue;
    SetPixel(p.x,p.y,c);
    writeln(v);
    
    v += 1;
    writeln(GetPixel(x-1,y).Equals(Color.White));
    exit;
    if GetPixel(x-1,y)<>Color.White then q.Enqueue(Pnt(x-1,y));
    if GetPixel(x+1,y)=Color.White then q.Enqueue(Pnt(x+1,y));
    if GetPixel(x,y-1)=Color.White then q.Enqueue(Pnt(x,y-1));
    if GetPixel(x,y+1)=Color.White then q.Enqueue(Pnt(x,y+1));
  end;
end;

begin
  SetPixel(100,100,Color.White);
  writeln(GetPixel(100,100).Equals(Color.White));
  {SetSmoothingOff;
  Circle(100,100,50);  
  FloodFill(72,72);}
end.