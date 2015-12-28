// Ханойские башни
uses GraphABC;

type
  DiskType = record
    sz:integer;
    color: GraphABC.Color;
  end;
  IArr = array [1..10] of DiskType;

const
  m=8;
  h=12;
  dw=12;
  y0=300;
  x1=200;
  x2=400;
  x3=600;
  delay=50;

var
  a: array [1..3] of IArr;
  n: array [1..3] of integer;
  num: integer;

procedure DrawAll; forward;

procedure MoveDisk(From, Tol: integer);
begin
  Inc(num);
  Inc(n[tol]);
  a[tol][n[tol]]:=a[from][n[from]];
  Dec(n[from]);
  Sleep(delay);
  ClearWindow;
  DrawAll;
end;

procedure MoveTower(n: integer; From, Tol, Work: integer);
begin
  if n=0 then exit;
  MoveTower(n-1, From, Work, Tol);
  MoveDisk (From, Tol);
  MoveTower(n-1, Work, Tol, From);
end;

procedure Init;
var i: integer;
begin
  n[1]:=m;
  n[2]:=0;
  n[3]:=0;
  for i:=1 to n[1] do
  begin
    a[1][i].sz:=n[1]-i+1;
    a[1][i].color:=clRandom;
  end;
end;

procedure DrawTown(a: IArr; n: integer; x0,y0: integer);
var i: integer;
begin
  SetBrushColor(clBlack);
  Rectangle(x0-5,y0,x0+5,y0-h*m-10);
  for i:=1 to n do
  begin
    SetBrushColor(a[i].color);
    Rectangle(x0-a[i].sz*dw,y0-h*(i-1),x0+a[i].sz*dw,y0-h*i)
  end;
end;

procedure DrawAll;
begin
  DrawTown(a[1],n[1],x1,y0);
  DrawTown(a[2],n[2],x2,y0);
  DrawTown(a[3],n[3],x3,y0);
  SetFontSize(14);
  SetFontName('Arial');
  SetBrushColor(clWhite);
  TextOut(20,20,'Число перемещений дисков='+IntToStr(num));
  Redraw;
end;

begin
  SetWindowCaption('Ханойские башни');
  SetWindowSize(760,400);
  Init;
  LockDrawing;
  DrawAll;
  MoveTower(m, 1, 3, 2);
end.


