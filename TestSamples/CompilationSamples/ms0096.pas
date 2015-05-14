uses GraphABC{,Events};

const
  n=10;
  sz=40;

type
  Field=array[0..n+1,0..n+1] of integer;
  orientation=(horiz,vert);
  ship=record
    x,y,len,life: integer;
    orient: orientation;
  end;
  CellState=(NeStrelial,Pusto,Mimo,Popal,Potopil);

var
  a: Field;
  ships: array[1..10] of ship;
  b: array [0..n+1, 0..n+1] of CellState; // выстрелы

procedure Clear(var a: Field);
var x,y: integer;
begin
  for x:=0 to n+1 do
  for y:=0 to n+1 do
  begin
    A[x,y]:=0;
    if (x=0) or (x=n+1) or (y=0) or (y=n+1) then
      b[x,y]:=Pusto
    else b[x,y]:=NeStrelial;
  end;
end;

function CanPlace(x0,y0: integer; var a: Field): boolean;
var x,y: integer;
begin
  result:=true;
  for x:=x0-1 to x0+1 do
  for y:=y0-1 to y0+1 do
  if a[x,y]<>0 then
  begin
    result:=false;
    exit;
  end;
end;

procedure DrawCell(x,y: integer);
begin
  if (x<1) or (x>n) or (y<1) or (y>n) then
    exit;
  case b[x,y] of
Mimo, NeStrelial, Pusto: SetBrushColor(clWhite);
Popal: SetBrushColor(clGray);
Potopil: SetBrushColor(clRed);
  end;
  if (b[x,y]=NeStrelial) and (a[x,y]<>0) then
    SetBrushColor(clYellow);
  Rectangle((x-1)*sz,(y-1)*sz, x*sz-1, y*sz-1);
  if b[x,y] in [Mimo,Popal,Potopil] then
   begin
     line((x-1)*sz, (y-1)*sz-1, x*sz-1, y*sz-2);
     line((x-1)*sz, y*sz-2, x*sz-1, (y-1)*sz-1);
   end;
  if b[x,y]=Pusto then
  begin
    SetBrushColor(clGreen);
    Circle(x*sz-sz div 2,y*sz-sz div 2,5)
  end;
end;

procedure SetB(x,y: integer; st: CellState);
begin
  b[x,y]:=st;
  DrawCell(x,y);
end;

procedure DrawField(var a: Field);
var x,y: integer;
begin
  for x:=1 to n do
  for y:=1 to n do
    DrawCell(x,y);
end;

procedure PlaceShip(i,x0,y0,len: integer; orient: orientation);
var x,y: integer;
begin
  if orient=horiz then
    for x:=x0 to x0+len-1 do
      a[x,y0]:=i
  else
    for y:=y0 to y0+len-1 do
      a[x0,y]:=i;
end;

function CanPlaceShip(x0,y0,len: integer; orient: orientation): boolean;
var x,y: integer;
begin
  Result:=true;
  if orient=horiz then
    for x:=x0 to x0+len-1 do
      if not CanPlace(x,y0,a) then
      begin
        Result:=False;
        Exit;
      end;
  if orient=vert then
    for y:=y0 to y0+len-1 do
      if not CanPlace(x0,y,a) then
      begin
        Result:=false;
        Exit;
      end;
end;

function TryPlaceShip(i: integer; var s: ship): boolean ;
var orient:orientation;
    len,x,y:integer;
begin
  len:=s.len;
  if Random(2)=1 then orient:=horiz
  else orient:=vert;
  if orient=horiz then
  begin
    y:=Random(10)+1;
    x:=Random(10-len+1)+1;
  end
  else
  begin
    x:=Random(10)+1;
    y:=Random(10-len+1)+1;
  end;
  result:=CanPlaceShip(x,y,len,orient);
  if result then
  begin
    PlaceShip(i,x,y,len,orient);
    s.x:=x;
    s.y:=y;
    s.orient:=orient;
  end;
end;

function TryPlaceShipNTimes(i: integer; var s:ship; n: integer): boolean;
var j: integer;
begin
  Result:=False;
  for j:=1 to n do
  if TryPlaceShip(i,s) then
  begin
    Result:=true;
    break;
  end;
end;

function TryPlaceShips(n: integer): boolean;
var i: integer;
begin
  Result:=True;
  for i:=1 to 10 do
  if not TryPlaceShipNTimes(i,ships[i],n) then
  begin
    Result:=False;
    exit;
  end;
end;

procedure KillShipInA(i:integer);
begin
  PlaceShip(-i,ships[i].x, Ships[i].y, ships[i].len, ships[i].orient);
end;

procedure KillShipInBFromCell(x,y: integer);
 procedure KillShipInDirection(x,y,dx,dy: integer);
 begin
   repeat
     x:=x+dx;
     y:=y+dy;
     if b[x,y]<>Popal then break;
     SetB(x,y,Potopil);
   until False;
   if b[x,y]=NeStrelial then
     SetB(x,y,Pusto);
 end;
begin
  if b[x,y]<>Potopil then Exit;
  KillShipInDirection(x,y,1,0);
  KillShipInDirection(x,y,-1,0);
  KillShipInDirection(x,y,0,1);
  KillShipInDirection(x,y,0,-1);
end;

procedure InitShips;
var i: integer;
begin
  ships[1].len:=4;
  ships[2].len:=3;
  ships[3].len:=3;
  ships[4].len:=2;
  ships[5].len:=2;
  ships[6].len:=2;
  ships[7].len:=1;
  ships[8].len:=1;
  ships[9].len:=1;
  ships[10].len:=1;
  for i:=1 to n do
    Ships[i].life:=ships[i].len;
  if not TryPlaceShips(100) then
  begin
    write('Ќе удалось расположить корабли');
    exit;
  end;
end;

var prevgoodx,prevgoody: integer;

procedure Pli(x,y: integer);
var i: integer;
begin
  if b[x,y]<>NeStrelial then exit;
  i:=a[x,y];
  if i>0 then // попал
  begin
    Dec(Ships[i].life);
    if Ships[i].life = 0 then
    begin
      SetB(x,y,Potopil);
      KillShipInA(i);
      KillShipInBFromCell(x,y);
      prevgoodx:=0;
      prevgoody:=0;
    end
    else
      SetB(x,y,Popal);
      prevgoodx:=x;
      prevgoody:=y;
      if b[x-1,y-1]=NeStrelial then
        SetB(x-1,y-1,Pusto);
      if b[x-1,y+1]=NeStrelial then
        SetB(x-1,y+1,Pusto);
      if b[x+1,y-1]=NeStrelial then
        SetB(x+1,y-1,Pusto);
      if b[x+1,y+1]=NeStrelial then
        SetB(x+1,y+1,Pusto);
end
  else SetB(x,y,Mimo);
end;

procedure TryPli;
var x,y: integer;
begin
  repeat
    x:=Random(10)+1;
    y:=Random(10)+1;
  until b[x,y]=NeStrelial;
  Pli(x,y);
end;

procedure IntellectualTryPli;
 procedure SearchNextPli(var x,y: integer);
 var d: (vert,horiz,unknown);
 begin
   d:=unknown;
   x:=prevgoodx; y:=prevgoody;
   // определить направление корабл€
   if (b[x,y+1]=Popal) or (b[x,y-1]=Popal) then
     d:=vert
   else if (b[x+1,y]=Popal) or (b[x-1,y]=Popal) then
     d:=horiz;
     
   if (d=horiz) or (d=unknown) then
   begin
     repeat
       x:=x+1
     until b[x,y]<>Popal;
     if b[x,y]=NeStrelial then Exit;
     x:=prevgoodx;
     repeat
       x:=x-1
     until b[x,y]<>Popal;
     if b[x,y]=NeStrelial then Exit;
   end;
   if (d=vert) or (d=unknown) then
   begin
     x:=prevgoodx;
     repeat
       y:=y+1
     until b[x,y]<>Popal;
     if b[x,y]=NeStrelial then Exit;
     y:=prevgoody;
     repeat
       y:=y-1
     until b[x,y]<>Popal;
     if b[x,y]=NeStrelial then Exit;
     writeln('ќшибка: корабль не потоплен, но выстрелов больше нет');
     writeln(prevgoodx,' ',prevgoody);
   end;
 end;
 
var x,y: integer;

begin
  if b[prevgoodx,prevgoody]=Popal then // что-то интеллектуальное - добивать
    SearchNextPli(x,y)
  else
  repeat
    x:=Random(10)+1;
    y:=Random(10)+1;
  until b[x,y]=NeStrelial;
  Pli(x,y);
end;

var i: integer;

begin
  cls;
  Clear(a);
  InitShips;

  DrawField(a);
  for i:=1 to 40 do
  begin
    IntellectualTryPli;
    Sleep(20);
  end;
end.
