uses crt;
 
const F_PODKLAD = 'podklad.txt';
      LEFT   = 29;
      WIDTH  = 10;      { sirka }
      HEIGHT = 24;      { vyska }
      TO_LEV = 24;      { kolko riadkov ma level }
      MAX_OS = 10;      { pocet osob v rebricku  }
 
      S_CUBE = 'öö';
      S_FOOT = 'üü';
 
      FOOT: array[0..1] of byte =
        ( DarkGray, Black);
 
      CUBE: array[1..7,1..4,1..4] of byte =
      (((0,0,0,0),    { kocka }
        (0,1,1,0),
        (0,1,1,0),
        (0,0,0,0)),
 
       ((0,1,0,0),    { dlhe I }
        (0,1,0,0),
        (0,1,0,0),
        (0,1,0,0)),
 
       ((0,1,0,0),    { opacne T }
        (0,1,1,0),
        (0,1,0,0),
        (0,0,0,0)),
 
       ((0,0,0,0),    { Z vpravo }
        (0,1,1,0),
        (1,1,0,0),
        (0,0,0,0)),
 
       ((0,0,0,0),    { Z vlavo }
        (1,1,0,0),
        (0,1,1,0),
        (0,0,0,0)),
 
       ((0,0,1,0),    { L vpravo }
        (0,0,1,0),
        (0,1,1,0),
        (0,0,0,0)),
 
       ((0,1,0,0),    { L vlavo }
        (0,1,0,0),
        (0,1,1,0),
        (0,0,0,0)));
 
type t_option = (Put, Clr, Sav);
     { moznosti Put  - nakreslit kocku      }
     {          Clr  - zmaze kocku          }
     {          Save - ulozit kocku do pola }
 
     { pre ukladania rebricka }
     t_top = record
       meno:string[10];
       body:integer;
     end;
 
var  pole:array[1..WIDTH,1..HEIGHT] of byte;
     body:integer;       { pocet bodov }
     tlev:integer;       { pocitanie do dalsieho lavelu }
    level:byte;          { v akom levely sa nachadza    }
      typ:byte;          { typ kocky, dalsi typ kocky   }
     otoc:byte;          { otocenie kocky }
      col:byte;          { farba kocky    }
      x,y:integer;
       ch:char;
 
 
{ zlucenie GotoXY a Write }
procedure WriteXY(x,y:integer;s:string);
begin
  GotoXY(LEFT+2*x,y);
  Write(s);
end;
 
 
{ nastavi prazdne pole }
procedure ClrPole;
var x,y:integer;
begin
  for x:=1 to WIDTH do
    for y:=1 to HEIGHT do
      Pole[x,y]:=0;
end;
 
 
{ urci predchadzajuci prvok }
function TPred(otoc:byte):byte;
begin
  TPred:=otoc-1;
  if(otoc=1)then TPred:=4;
end;
 
 
{ urci nasledujuci prvok }
function TSucc(otoc:byte):byte;
begin
  TSucc:=otoc+1;
  if(otoc=4)then TSucc:=1;
end;
 
 
{ urci mensi prvok }
function Min(a,b:integer):integer;
begin
  if(a<b)then
     Min:=a
  else
     Min:=b;
end;
 
 
{ zapne/vypne zobrazenie kurzora }
procedure KurzorZap(ZapVyp:boolean);
begin
  {with Regs do
   begin
    AH := $03;
    BH := $00;
    Intr($10,Regs);
    If not (Zapvyp) then
       CH := CH or $20
      else
       CH := CH and $DF;
 
    AH := $01;
    Intr($10,Regs);
   end;}
end;
 
 
{ precita zo suboru podklad }
procedure Podklad;
var f:text;
    s:string;
    x,y:integer;
begin
  {$I-}
  {assign(f, F_PODKLAD);
  reset(f);
  TextColor(LightGray);
 
  while( not(eof(f))) do
  begin
    ReadLn(f,s);
    Write(s);
    if(not(eof(f)))then WriteLn;
  end;}
 
  close(f);
  {$I+}
 
  { vynulujem pripadne chyby }
  //x:=IOResult;
 
  { vykresli vodiace ciary }
  for y:=1 to HEIGHT do
    for x:=1 to WIDTH do
    begin
      TextColor(FOOT[x mod 2]);
      WriteXY(x,y,S_FOOT);
    end;
end;
 
 
{ vykresli, zmaz, uloz kocku, alebo urci ci je mozne kocku polozit }
procedure Kocka(xp,yp,typ,otoc,col:integer;option:t_option);
var x,y: integer;
    bod: byte;
begin
  { v cykle vygenerujeme jednotlive prvky kocky }
  for y:=1 to 4 do
     for x:=1 to 4 do
     begin
 
       case otoc of
        1: bod := CUBE[typ,x,y];
        2: bod := CUBE[typ,5-y,x];
        3: bod := CUBE[typ,5-x,5-y];
        4: bod := CUBE[typ,y,5-x];
       end;
 
       case option of
         clr: { zmaze kocku }
              if( bod=1 )then
              begin
                TextColor( FOOT[(x+xp) mod 2]);
                WriteXY(xp+x,yp+y,S_FOOT);
              end;
 
         put: { nakresli, zmaze kocku }
              if( bod=1 )then
              begin
                TextColor(col);
                WriteXY(xp+x,yp+y,S_CUBE);
              end;
 
         sav: { ulozi kocku do pola }
              if( bod=1 )then
                  pole[x+xp,y+yp]:=col;
 
       end;   { case }
     end;   { for }
 
  { vypnem kurzor }
  //KurzorZap(false);
end;
 
 
{ zisti ci je mozne polozit kocku }
function KockaOK(xp,yp,typ,otoc:integer):boolean;
var x,y: integer;
    bod: byte;
    res: boolean;
begin
  { zatial si mysli ze kocku je mozne polozit }
  res:=true;
 
  { v cykle vygenerujeme jednotlive prvky kocky }
  for y:=1 to 4 do
     for x:=1 to 4 do
     begin
 
       case otoc of
        1: bod := CUBE[typ,x,y];
        2: bod := CUBE[typ,5-y,x];
        3: bod := CUBE[typ,5-x,5-y];
        4: bod := CUBE[typ,y,5-x];
       end;
 
       if( bod=1 )then
       begin
         { hrube podmienky }
         if((x+xp) <1      )then res:=false;
         if((x+xp) >WIDTH  )then res:=false;
         if((y+yp) >HEIGHT )then res:=false;
         if(  otoc <1      )then res:=false;
         if(  otoc >4      )then res:=false;
 
         { este ci je tam volne miesto }
         if( res )then
            if( pole[x+xp,y+yp]<>0 )then
                res:=false;
 
       end;   { if }
     end;   { for }
 
  { moja odpoved }
  kockaOK:=res;
end;
 
 
{ zmaze zaplneny riadok    }
{ a ostane posunie nadol   }
procedure ZmazRiadok(yr:integer);
var x,y:integer;
begin
  TextColor(Black);
 
  { efekt postupneho mazania }
  for x:=1 to WIDTH do
  begin
    WriteXY(x,yr,S_CUBE);
    Delay(20);
  end;
 
  { efekt padu riadkov }
  for y:=yr downto 2 do
    for x:=1 to WIDTH do
    begin
      pole[x,y]:=pole[x,y-1];
 
      if( pole[x,y]=0 )then
      begin
        TextColor(FOOT[x mod 2]);
        WriteXY(x,y,S_FOOT);
      end
      else begin
        TextColor(pole[x,y]);
        WriteXY(x,y,S_CUBE);
      end
    end;
end;
 
 
{ skontroluje ktore riadky ma zmazat }
procedure Skontroluj(yr:integer);
var x,y:integer;
    del:boolean;
begin
  for y:=yr to Min(yr+4, HEIGHT) do
  begin
    del:=true;
 
    for x:=1 to WIDTH do
      if( pole[x,y]=0 )then
          del:=false;
 
    if( del )then
    begin
      ZmazRiadok(y);
      body:=body+level;
      tlev:=tlev+1;
    end;
  end;
 
  { ideme do dalsieho levelu }
  if( tlev>=TO_LEV )then
  begin
    tlev:=0;
    level:=level+1;
  end;
end;
 
 
{ precita stlacenu klavesu }
function GetKey(level:byte):char;
var i:integer;
   ch:char;
begin
  ch:=#0;
 
  for i:=1 to 200-level*5 do
  begin
    { ak stlacil precitam klaves }
    if( keypressed )then
    begin
       ch:=readkey;
       if( ch=#0 )then
           ch:=readkey;
    end;
 
    delay(1);
  end;
 
  GetKey:=ch;
end;
 
 
BEGIN
   ClrScr;
   Randomize;
 
   ClrPole;
   Podklad;
 
   body :=0;
   tlev :=0;
   level:=1;
 
   y :=1;
   ch:=#0;
 
   repeat
     { generuj kocku a next typ }
     if( y=1 )then
     begin
       x    := (WIDTH div 2)-2;
       otoc := random(4)+1;
       typ  := random(7)+1;
       col  := random(15)+1;
     end;
 
     { nakresli }
     Kocka(x,y,typ,otoc,col,put);
 
     { bud rychlo pada alebo citam klaves }
     if( ch<>#32 )then
         ch:=GetKey(level);
 
     { zmaz staru }
     Kocka(x,y,typ,otoc,Black,clr);
 
     { podmienky otocit, vlavo, vpravo }
     if(ch=#37) and KockaOK(x-1,y,typ,otoc) then x:=x-1;
     if(ch=#39) and KockaOK(x+1,y,typ,otoc) then x:=x+1;
     if(ch=#40) and KockaOK(x,y,typ,TPred(otoc)) then otoc:=TPred(otoc);
     if(ch=#38) and KockaOK(x,y,typ,TSucc(otoc)) then otoc:=TSucc(otoc);
 
     { posuniem o riadok nizsie }
     if( KockaOK(x,y+1,typ,otoc))then
         y:=y+1
     else
       { kocka spadla }
       begin
         Kocka(x,y,typ,otoc,col,put);
         Kocka(x,y,typ,otoc,col,sav);
         Skontroluj(y);
 
         ch:=#0;
         if(y=1) then ch:=#27;  { niet kam polozit koncim }
         y :=1;
       end;
 
   until( ch=#27 );
 
   KurzorZap(true);
end.