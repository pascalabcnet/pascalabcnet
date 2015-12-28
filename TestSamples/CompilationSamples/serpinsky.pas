// Ковер Серпинского. Иллюстрация forward-объявлений
uses GraphABC;

var
  a,b,x,y: integer;
  N: integer;

procedure lrel (dx,dy: integer);
begin
  x:=x+dx; y:=y+dy; LineTo(x,y);
end;

procedure BB(k: integer); forward;
procedure CC(k: integer); forward;
procedure DD(k: integer); forward;

procedure AA(k: integer);
begin
 if k>0 then
 begin
   AA(k-1); lrel(a,b);
   BB(k-1); lrel(a,0);
   DD(k-1); lrel(a,-b);
   AA(k-1);
 end;
end;

procedure BB(k: integer);
begin
 if k>0 then
 begin
   BB(k-1); lrel(-a,b);
   CC(k-1); lrel(0,b);
   AA(k-1); lrel(a,b);
   BB(k-1);
 end;
end;

procedure CC(k: integer);
begin
 if k>0 then
 begin
   CC(k-1); lrel(-a,-b);
   DD(k-1); lrel(-a,0);
   BB(k-1); lrel(-a,b);
   CC(k-1);
 end;
end;

procedure DD(k: integer);
begin
 if k>0 then
 begin
   DD(k-1); lrel(a,-b);
   AA(k-1); lrel(0,-b);
   CC(k-1); lrel(-a,-b);
   DD(k-1);
 end;
end;

begin
  N:=6;
  a:=3;
  b:=a;
  x:=10;
  y:=10;
  SetWindowCaption('Ковер Серпинского');
  SetWindowSize(590,590);
  MoveTo(x,y);
  AA(N); lrel(a,b);
  BB(N); lrel(-a,b);
  CC(N); lrel(-a,-b);
  DD(N); lrel(a,-b);
end.

