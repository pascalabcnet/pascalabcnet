// Игра Жизнь на торе
// Оптимизация хешированием по равномерной сетке

//#apptype console

uses Utils,GraphABC;

const 
  w=4;
  w1=1;
  m=300;
  n=220;
  x0=2;
  y0=20;
  mm=301;
  nn=221;
  mk=15;
  nk=10;

var
  a,b,sosedia,sosedib: array [0..nn,0..mm] of integer;
  obnovA,obnovB: array [1..nk,1..mk] of boolean;
  count: integer;
  obn: boolean;
  gen,mil,mil1: integer;
  hn,hm: integer;

procedure DrawCell(i,j: integer);
begin
  SetBrushColor(clBlack);
  SetPenColor(clBlack);
  FillRect(x0+(j-1)*w,y0+(i-1)*w,x0+j*w-w1,y0+i*w-w1);
end;

procedure ClearCell(i,j: integer);
begin
  SetBrushColor(clWhite);
  SetPenColor(clWhite);
  FillRect(x0+(j-1)*w,y0+(i-1)*w,x0+j*w-w1,y0+i*w-w1);
end;

procedure DrawConfiguration;
var i,j,bb: integer;
begin
  for i:=1 to n do
  for j:=1 to m do
  begin
    bb:=b[i,j];
    if a[i,j]<>bb then
      if bb=1 then DrawCell(i,j)
        else ClearCell(i,j);
  end;
end;

procedure DrawConfigurationFull;
var i,j,bb: integer;
begin
  for i:=1 to n do
  for j:=1 to m do
  begin
    bb:=b[i,j];
    if bb=1 then DrawCell(i,j)
      else ClearCell(i,j);
  end;
end;

procedure DrawField;
var i: integer;
begin
  SetPenColor(clLightGray);
  for i:=0 to m do
  begin
    if i mod hm = 0 then
      SetPenColor(clGray)
    else SetPenColor(clLightGray);
    Line(x0+i*w-1,y0,x0+i*w-1,y0+n*w);
  end;
  for i:=0 to n do
  begin
    if i mod hn = 0 then
      SetPenColor(clGray)
    else SetPenColor(clLightGray);
    Line(x0,y0+i*w-1,x0+m*w,y0+i*w-1);
  end;
end;

procedure IncSosedi(i,j: integer);
var i1,i2,j1,j2: integer;
begin
  if i=1 then i1:=n else i1:=i-1;
  if i=n then i2:=1 else i2:=i+1;
  if j=1 then j1:=m else j1:=j-1;
  if j=m then j2:=1 else j2:=j+1;
  Inc(SosediB[i1,j1]);
  Inc(SosediB[i1,j]);
  Inc(SosediB[i1,j2]);
  Inc(SosediB[i,j1]);
  Inc(SosediB[i,j2]);
  Inc(SosediB[i2,j1]);
  Inc(SosediB[i2,j]);
  Inc(SosediB[i2,j2]);
end;

procedure DecSosedi(i,j: integer);
var i1,i2,j1,j2: integer;
begin
  if i=1 then i1:=n else i1:=i-1;
  if i=n then i2:=1 else i2:=i+1;
  if j=1 then j1:=m else j1:=j-1;
  if j=m then j2:=1 else j2:=j+1;
  Dec(SosediB[i1,j1]);
  Dec(SosediB[i1,j]);
  Dec(SosediB[i1,j2]);
  Dec(SosediB[i,j1]);
  Dec(SosediB[i,j2]);
  Dec(SosediB[i2,j1]);
  Dec(SosediB[i2,j]);
  Dec(SosediB[i2,j2]);
end;

procedure SetCell(i,j: integer);
begin
  if b[i,j]=0 then
  begin
    b[i,j]:=1;
    obn:=true;
    IncSosedi(i,j);
  end;
  Inc(count);
end;

procedure UnSetCell(i,j: integer);
begin
  if b[i,j]=1 then
  begin
    b[i,j]:=0;
    obn:=true;
    DecSosedi(i,j);
  end;
  Dec(count);
end;

procedure Init;
var i,j,ik,jk,xc,yc: integer;
begin
  xc:=n div 2;
  yc:=m div 2;
//  xc:=1;
//  yc:=1;
  for i:=0 to n+1 do
  for j:=0 to m+1 do
    b[i,j]:=0;
  a:=b;
  sosedib:=b;
  Count:=0;
  SetCell(xc,yc);
  SetCell(xc,yc+1);
  SetCell(xc,yc+2);
  SetCell(xc-1,yc+2);
  SetCell(xc+1,yc+1);
  SosediA:=SosediB;
  for ik:=1 to nk do
  for jk:=1 to mk do
    obnovB[ik,jk]:=true;
  obnovA:=obnovB;
end;

procedure OnlyCase(i,j: integer);
begin
  case SosediA[i,j] of
0..1,4..9:
    if b[i,j]=1 then
    begin
      b[i,j]:=0;
      obn:=true;
      DecSosedi(i,j);
      ClearCell(i,j);
      Dec(count);
    end;
3: if b[i,j]=0 then
    begin
      b[i,j]:=1;
      obn:=true;
      IncSosedi(i,j);
      DrawCell(i,j);
      Inc(count);
    end;
  end; {case}
end;

procedure NextGen;
var
  i,j,ik,jk,ik1,jk1,ik2,jk2,ifirst,jfirst,ilast,jlast: integer;
  l,r,u,d,lu,ld,ru,rd: boolean;
begin
    for ik:=1 to nk do
    begin
      for jk:=1 to mk do
      begin
        obn:=false;
        ifirst:=(ik-1)*hn+1;
        ilast:=(ik-1)*hn+hn;
        jfirst:=(jk-1)*hm+1;
        jlast:=(jk-1)*hm+hm;
        if obnovA[ik,jk] then
        begin
          for i:=ifirst to ilast do
          begin
            for j:=jfirst to jlast do
            begin
              case SosediA[i,j] of
            0..1,4..9: if b[i,j]=1 then
              begin
                b[i,j]:=0;
                obn:=true;
                DecSosedi(i,j);
                ClearCell(i,j);
                Dec(count);
              end;
            3: if b[i,j]=0 then
              begin
                b[i,j]:=1;
                obn:=true;
                IncSosedi(i,j);
                DrawCell(i,j);
                Inc(count);
              end;
              end; {case}
            end;
          end
        end
        else
        begin
          if ik=1 then ik1:=nk else ik1:=ik-1;
          if ik=nk then ik2:=1 else ik2:=ik+1;
          if jk=1 then jk1:=mk else jk1:=jk-1;
          if jk=mk then jk2:=1 else jk2:=jk+1;
          l:=obnovA[ik,jk1];
          r:=obnovA[ik,jk2];
          u:=obnovA[ik1,jk];
          d:=obnovA[ik2,jk];
          lu:=obnovA[ik1,jk1];
          ld:=obnovA[ik2,jk1];
          ru:=obnovA[ik1,jk2];
          rd:=obnovA[ik2,jk2];
          if u then
          begin
            i:=ifirst;
            for j:=jfirst+1 to jlast-1 do
              OnlyCase(i,j);
          end;
          if d then
          begin
            i:=ilast;
            for j:=jfirst+1 to jlast-1 do
              OnlyCase(i,j);
          end;
          if l then
          begin
            j:=jfirst;
            for i:=ifirst+1 to ilast-1 do
              OnlyCase(i,j);
          end;
          if r then
          begin
            j:=jlast;
            for i:=ifirst+1 to ilast-1 do
              OnlyCase(i,j);
          end;
          if u or l or lu then
            OnlyCase(ifirst,jfirst);
          if u or r or ru then
            OnlyCase(ifirst,jlast);
          if d or l or ld then
            OnlyCase(ilast,jfirst);
          if d or r or rd then
            OnlyCase(ilast,jlast);
        end;
        obnovB[ik,jk]:=obn;
      end;
    end;
end;

begin
  SetWindowCaption('Игра "Жизнь"');
  if (m mod mk<>0) or (n mod nk<>0) then
  begin
    writeln('Размер кластера не согласован с размером поля. Программа завершена');
    exit
  end;
  hm:=m div mk;
  hn:=n div nk;
  Cls;
  SetWindowSize(x0+m*w,y0+n*w);
  CenterWindow;
  SetFontName('Arial');
  SetFontSize(10);
  Init;
  TextOut(25,0,'Поколение '+IntToStr(gen));
  TextOut(765,0,'Жителей: '+IntToStr(count)+'    ');
  DrawField;
  DrawConfiguration;
  mil:=Milliseconds;
  gen:=0;
  while true do
  begin
    gen:=gen+1;
    SetBrushColor(clWhite);
    TextOut(25,0,'Поколение: '+IntToStr(gen));
    TextOut(765,0,'Жителей: '+IntToStr(count)+'    ');

    a:=b;
    SosediA:=SosediB;
    obnovA:=obnovB;
    NextGen;
    
    if gen mod 1000 = 0 then
    begin
      mil1:=Milliseconds;
      writeln(gen,'  ',(mil1-mil)/1000);
      mil:=mil1;
    end;
  end;
end.
