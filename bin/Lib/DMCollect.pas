// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
unit DMCollect;

{$reference 'System.Drawing.dll'}

interface

uses 
  System.Drawing,
  System.Collections;


type
  Vector = Point;
  
  Cell = record
    p: Point;
    v: Vector;
    procedure Init(x1,y1,x2,y2: integer);
    procedure InitAB(a,b: Point);
    function ToString: string; override;
    begin
      Result := IntToStr(p.x)+','+IntToStr(p.y)+','+IntToStr(v.x)+','+IntToStr(v.y);
    end;
  end;

  SortedCollection = class
  private
    a: ArrayList;
    function GetCount: integer;
    begin
      Result := a.Count;
    end;
    function GetItem(i: integer): Cell;
    begin
      Result := Cell(a[i]); //!! Не будет работать для Item[i].a.x := 1; !!!!!!!
    end;
    procedure SetItem(i: integer; c: Cell);
    begin
      a[i] := c;
    end;
  public
    property Count: integer read GetCount;
    property Items[i: integer]: Cell read GetItem write SetItem; default;
    constructor Create;
    destructor Done;
    procedure Insert (x1,y1,x2,y2: integer);
    procedure InsertAB (A,B: Point);
    procedure Print;
    procedure Normalize;
    procedure Clear;
    function IsEmpty: boolean;
  end;

  function InitVector(a,b: Point): Vector;
  function Len2(v: Vector): longint;

  function IsBetweenPP (P,A,B: Point): boolean;
  function IsEqualPP (P1,P2: Point): boolean;
  function IsEqualPxy (P: Point; x,y: integer): boolean;
  function LessThenPP (P1,P2: Point): boolean;
  function LessThenPxy (P: Point; x,y: integer): boolean;

  function LessThenVV (A,B: Vector): boolean;
  function IsParallel (A,B: Vector): boolean;

  function IsEqualCC (C1,C2: Cell): boolean;
  function LessThenCC (C1,C2: Cell): boolean;
  function LessEqualCC (C1,C2: Cell): boolean;

  function IsEqualSC(SC1,SC2: SortedCollection): boolean;

{--------------------------------------------}
                implementation
{--------------------------------------------}

function IsEqualPP (P1,P2: Point): boolean;
begin
  IsEqualPP := (P1.x=P2.x) and (P1.y=P2.y)
end;

function IsEqualPxy (P: Point; x,y: integer): boolean;
begin
  IsEqualPxy := (x=P.x) and (y=P.y)
end;

function LessThenPP (P1,P2: Point): boolean;
begin
  LessThenPP := (P1.x<P2.x) or ((P1.x=P2.x) and (P1.y<P2.y))
end;

function LessEqualPP (P1,P2: Point): boolean;
begin
  LessEqualPP := (P1.x<P2.x) or ((P1.x=P2.x) and (P1.y<=P2.y))
end;

function LessThenPxy (P: Point; x,y: integer): boolean;
begin
  LessThenPxy := (P.x<x) or ((P.x=x) and (P.y<y))
end;

function IsBetweenPP (P,A,B: Point): boolean;
var
  C: Point;
  AP,AB: Vector;
begin
  if LessThenPP(B,A) then begin
    C := A;
    A := B;
    B := C
  end;
  AP := InitVector(A,P);
  AB := InitVector(A,B);
  IsBetweenPP := IsParallel(AB,AP) and
                 LessEqualPP(A,P) and
                 LessEqualPP(P,B)
end;

function LessThenVV (A,B: Vector): boolean;
begin
  LessThenVV := longint(A.x)*longint(B.y) -
                longint(A.y)*longint(B.x) > 0
end;

function LessEqualVV (A,B: Vector): boolean;
begin
  LessEqualVV := longint(A.x)*longint(B.y) -
                 longint(A.y)*longint(B.x) >= 0
end;

function IsParallel (A,B: Vector): boolean;
begin
  IsParallel := longint(A.x)*longint(B.y) =
                longint(A.y)*longint(B.x)
end;

function IsEqualCC (C1,C2: Cell): boolean;
begin
  IsEqualCC := isequalPP(C1.p,C2.p) and
               isequalPP(C1.v,C2.v)
end;

function LessThenCC (C1,C2: Cell): boolean;
var
  vv: vector;
  ip: boolean;
{Самое сложное условие}
begin
  vv := initVector(C1.p,C2.p);
  ip := IsParallel(C1.v,C2.v);
  {C1<C2 если вектор1<вектора2                             или
              вектор1 параллелен вектору2 и вектор1<P1P2   или
              вектор1 параллелен вектору2
                и вектор1 параллелен P1P2 и НЕПОНЯТНОЕ УСЛОВИЕ!!!!!!???????!!

  }
  LessThenCC := LessThenVV(C1.v,C2.v) or
                (ip and LessThenVV(C1.v,vv)) or
                (ip and IsParallel(C1.v,vv) and
{                  LessThenPP(C1.v,C2.v)); НЕПОНЯТНОЕ УСЛОВИЕ! - vector lengths are comparing?}
                   LessThenPP(C1.p,C2.p)); {change on this 27.5.99 - i dont know am i right.}
                   { The idea - to compare origins of vectors}
end;

function LessEqualCC (C1,C2: Cell): boolean;
begin
  LessEqualCC := LessThenCC(C1,C2) or IsEqualCC(C1,C2)
end;

function IsEqualSC (SC1,SC2: SortedCollection): boolean;
var i: integer;
begin
  Result := SC1.Count = SC2.Count;
  if not Result then 
    exit;
  for i:=0 to SC1.Count-1 do
    if not IsEqualCC(SC1[i],SC2[i]) then
    begin
      Result := False;
      break;
    end;  
end;

function InitVector(a,b: Point): Vector;
begin
  Result := new Point(b.x-a.x,b.y-a.y);
end;

function Len2(v: Vector): longint;
begin
  Result := v.x*v.x + v.y*v.y;
end;

{--------------------------------------------}
{                    Cell                    }
{--------------------------------------------}
procedure Cell.Init(x1,y1,x2,y2: integer);
begin
  if (x1<x2) or ((x1=x2) and (y1<y2)) then
  begin
    v := new Vector(x2-x1, y2-y1);
    p := new Point(x1,y1);
  end
    else
  begin
    v := new Vector(x1-x2, y1-y2);
    p := new Point(x2,y2);
  end;
end;

procedure Cell.InitAB(A,B: Point);
begin
  Init(A.x,A.y,B.x,B.y)
end;

{--------------------------------------------}
{              SortedCollection              }
{--------------------------------------------}
constructor SortedCollection.Create;
begin
  a := new ArrayList;
end;

procedure SortedCollection.Insert(x1,y1,x2,y2: integer);
var 
  CC: Cell;
  i: integer;
begin
  if (x1=x2) and (y1=y2) then Exit; {Не вставлять точку}
  CC.Init(x1,y1,x2,y2);
  for i:=0 to Count-1 do
    if LessEqualCC(CC,Self[i]) then
    begin
      if not IsEqualCC(CC,Self[i]) then // Не дублировать
        a.Insert(i,CC);
      Exit;
    end;
  a.Add(CC); // Если больше всех, то добавить в конец
end;

procedure SortedCollection.InsertAB(A,B: Point);
begin
  Insert(A.x,A.y,B.x,B.y)
end;

procedure SortedCollection.Print;
var i: integer;
begin
  for i:=0 to Count-1 do
    writeln(Items[i]);
end;

procedure SortedCollection.Normalize;
// Cливать соседние пары 
var
  pend,p1end,pnew,pnewend: Point;
  CC: Cell;
  i: integer;
begin
  for i:=Count-2 downto 0 do
    if IsParallel(Items[i].v,Items[i+1].v) then
    begin
      pend := new Point(Items[i].p.x+Items[i].v.x,Items[i].p.y+Items[i].v.y);
      p1end := new Point(Items[i+1].p.x+Items[i+1].v.x,Items[i+1].p.y+Items[i+1].v.y);
      if IsBetweenPP(Items[i+1].p,Items[i].p,pend)  {если P1 между P и P+v}  or
         IsBetweenPP(Items[i].p,Items[i+1].p,p1end) {если P между P1 и P1+v} then
      begin
        if LessThenPP(Items[i].p,Items[i+1].p) then 
          pnew:=Items[i].p
        else pnew:=Items[i+1].p;
        if LessThenPP(pend,p1end) then 
          pnewend:=p1end
        else pnewend:=pend;
        // Слить
//        Items[i].p := pnew; Это не работает из-за упаковки!!!
//        Items[i].v := vv;
        CC.InitAB(pnew,pnewend);
        Items[i] := CC;
        a.RemoveAt(i+1);
      end;
    end;
(*1:
  b:=false;
  H := Head;
  if H=nil then Exit;
  while H^.next<>nil do
  begin
    H1 := H^.next;
    k := 0;
    if IsParallel(H^.v,H1^.v) then
    begin
      pend.init(H^.p.x+H^.v.x,H^.p.y+H^.v.y);
      p1end.init(H1^.p.x+H1^.v.x,H1^.p.y+H1^.v.y);
      if IsBetweenPP(H1^.p,H^.p,Pend)  {если P1 между P и P+v}  or
         IsBetweenPP(H^.p,H1^.p,P1end) {если P между P1 и P1+v}
      then
        begin
          {найти начало и конец нового вектора}
          if LessThenPP(H^.p,H1^.p) then pnew:=H^.p
            else pnew:=H1^.p;
          if LessThenPP(pend,p1end) then pnewend:=p1end
            else pnewend:=pend;
          { Слить }
          H^.p.init(pnew.x,pnew.y);
          H^.v.InitVector(pnew,pnewend);
          H^.next := H1^.next;
          dispose(H1);
          b:=True;
          k := 1;
          {А если v1 целиком внутри v ???}
          {По моему, тогда на следующем проходе и для этого k}
        end;
    end;
    if k=0 then H := H^.next;
  end;
  if b then goto 1; {поскольку не все могли слиться}
  {не лучший алгоритм, но...}*)
end;

procedure SortedCollection.Clear;
begin
  a.Clear;
end;

destructor SortedCollection.Done;
begin
  Clear;
end;

function SortedCollection.IsEmpty: boolean;
begin
  Result := a.Count = 0;
end;

end.