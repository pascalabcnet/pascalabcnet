uses Graph3D;

type 
  FigureColor = (Black,White);
  FigureKind = (BishopK, HorseK, KingK, PownK, QueenK, RockK);

var 
  BlackC := GrayColor(60);
  WhiteC := Colors.White;
  delay := 1000;

type ChessFigure = class
  f: FileModelT;
public
  color: FigureColor;
  k: FigureKind;
  x,y: integer;
  constructor Create(kk: FigureKind; cc: FigureColor);
  function MoveTo(xx,yy: integer): ChessFigure;
  function AnimMoveTo(xx,yy: integer): ChessFigure;
  procedure Destroy;
  begin
    f.Destroy;
  end;
end;

var a := new ChessFigure[8,8];

constructor ChessFigure.Create(kk: FigureKind; cc: FigureColor);
begin
  color := cc;
  var c := cc=Black ? BlackC : WhiteC;
  case kk of
bishopK: f := FileModel3D(4,14,0,'bishop.obj',c);
horseK : f := FileModel3D(2,14,0,'horse.obj',c);
kingK  : f := FileModel3D(6,14,0,'king.obj',c);
pownK  : f := FileModel3D(0,12,0,'pawn.obj',c);
queenK : f := FileModel3D(8,14,0,'queen.obj',c);
rockK  : f := FileModel3D(0,14,0,'rook.obj',c);
  end;
  k := kk;
end;

function ChessFigure.MoveTo(xx,yy: integer): ChessFigure;
begin
  a[y,x] := nil;
  if a[yy,xx]<>nil then
    a[yy,xx].Destroy;
  a[yy,xx] := Self;
  var dx := xx - x;
  var dy := yy - y;
  f.MoveOn(-dx*2,-dy*2,0);
  (x,y) := (xx,yy);
  Result := Self;
end;

function ChessFigure.AnimMoveTo(xx,yy: integer): ChessFigure;
begin
  var dx := xx - x;
  var dy := yy - y;
  f.AnimMoveOn(-dx*2,-dy*2,0,delay/1000).WhenCompleted(procedure -> begin
    a[y,x] := nil;
    if a[yy,xx]<>nil then
      a[yy,xx].Destroy;
    a[yy,xx] := Self;
  end
  ).Begin;
  (x,y) := (xx,yy);
  Result := Self;
end;

function Bishop(x,y: integer; c: FigureColor) := ChessFigure.Create(FigureKind.BishopK,c).MoveTo(x,y);
function Horse(x,y: integer; c: FigureColor) := ChessFigure.Create(FigureKind.HorseK,c).MoveTo(x,y);
function King(x,y: integer; c: FigureColor) := ChessFigure.Create(FigureKind.KingK,c).MoveTo(x,y);
function Queen(x,y: integer; c: FigureColor) := ChessFigure.Create(FigureKind.QueenK,c).MoveTo(x,y);
function Rock(x,y: integer; c: FigureColor) := ChessFigure.Create(FigureKind.RockK,c).MoveTo(x,y);
function Pown(x,y: integer; c: FigureColor) := ChessFigure.Create(FigureKind.PownK,c).MoveTo(x,y);

procedure InitScene;
begin
  var d := 7;
  for var c := 'A' to 'H' do
  begin
    var t := Text3D(d,8.5,0,c,0.5);
    t.UpDirection := v3d(0,-1,0);
    d -= 2;
  end;
  d := 7;
  for var c := '1' to '8' do
  begin
    var t := Text3D(8.3,d,0,c,0.5);
    t.UpDirection := v3d(0,-1,0);
    d -= 2;
  end;
  View3D.ShowGridLines := False;
  FileModel3D(0,0,0,'board.obj',GrayColor(100));
end;

procedure StartupPosition;
begin
  Rock(0,0,White);
  Horse(1,0,White);
  Bishop(2,0,White);
  King(3,0,White);
  Queen(4,0,White);
  Bishop(5,0,White);
  Horse(6,0,White);
  Rock(7,0,White);
  for var i:=0 to 7 do
    Pown(i,1,White);

  Rock(0,7,Black);
  Horse(1,7,Black);
  Bishop(2,7,Black);
  King(3,7,Black);
  Queen(4,7,Black);
  Bishop(5,7,Black);
  Horse(6,7,Black);
  Rock(7,7,Black);
  for var i:=0 to 7 do
    Pown(i,6,Black);
end;

procedure Turn(x,y,x1,y1: integer);
begin
  if a[y,x]=nil then
  begin
    Println('>',y,x);
    exit;
  end;
  
  //Print(a[y,x],a[y1,x1]);
  a[y,x].AnimMoveTo(x1,y1);
  
  //Println('->',a[y,x],a[y1,x1]);
  Sleep(delay);
end;

procedure TurnB(s1,s2: string);
begin
  //Println(s1,s2);
  Turn(Ord(s1[1])-Ord('a'),s1[2].ToDigit-1,Ord(s2[1])-Ord('a'),s2[2].ToDigit-1);
end;

procedure Turns(s: string);
begin
  var ss := s.ToWords.Batch(3).SelectMany(d->d.ToArray[1:]);
  var i := 0;
  foreach var d in ss do
  begin
    var p := Pos('-',d);
    var p1 := d[:p];
    if Length(p1)=3 then 
      p1 := p1[2:];
    var p2 := d[p+1:];
    if Length(p2)=3 then 
      p2 := p2[2:];
    if (p1 = '0') and (p2 = '0') then
    begin
      if i mod 2 = 0 then
      begin
        TurnB('e1','g1');
        TurnB('h1','f1');
      end
      else
      begin
        TurnB('e8','g8');
        TurnB('h8','f8');
      end
    end
    else TurnB(p1,p2);
    i += 1;
  end;
end;

begin
  InitScene;
  StartupPosition;
  //Turns('1. c2-c4  g7-g6  2. e2-e4  Cf8-g7  3. d2-d4  d7-d6  4. Kb1-c3  Kg8-f6 5. Kg1-f3  0-0  6. Cf1-e2  e7-e5  7. Cc1-e3  Kf6-g4  8. Ce3-g5  f7-f6 9. Cg5-h4  g6-g5  10. Ch4-g3  Kg4-h6');
  Turns('1. e2-e4 e7-e5 2. f1-c4 g8-f6 3. d2-d4 e5-d4 4. g1-f3 d7-d5 5. e4-d5 f8-b4 6. c2-c3 d8-e7');
end.