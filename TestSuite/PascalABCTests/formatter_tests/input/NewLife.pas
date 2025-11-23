// Игра Жизнь на торе
// Оптимизация хешированием по равномерной сетке


uses Utils,GraphABC;

const 
  w =3;
  w1=1;
  k=20;
  m=300;
  n=220;
  graphW=0;
  mk=m div k;//15;
  nk=n div k;//10;
  mm=m+1;
  nn=n+1;
  x0=1;
  y0=21;
  ClearColor=clBlack;
  FillColor=clLimeGreen;
  FiledColor=RGB(0,40,0);
  FiledColor2=RGB(0,70,0);{}
  {ClearColor=clWhite;
  FillColor=clBlack;
  FiledColor=clLightGray;
  FiledColor2=clGray;{}
  

var
  //a,b,sosedia,sosedib: array [0..nn,0..mm] of byte;
  a,b,sosedia,sosedib:array of array of byte;
  //obnovA,obnovB: array [1..nk,1..mk] of boolean;
  obnovA,obnovB: array of array of boolean;
  count: integer;
  obn: boolean;
  mil,mil1: integer;
  hn,hm: integer;

procedure AssignArray(var arr:array of array of boolean; n,m:integer);
begin
  SetLength(arr,n);
  for var i:=0 to n-1 do
    SetLength(arr[i],m);
end;
procedure AssignArray(var arr:array of array of byte; n,m:integer);
begin
  SetLength(arr,n);
  for var i:=0 to n-1 do
    SetLength(arr[i],m);
end;
procedure CopyArray(arr1,arr2:array of array of byte);
begin
  for var i:=0 to arr1.Length-1 do 
    arr1[i].CopyTo(arr2[i],0);
end;
procedure CopyArray(arr1,arr2:array of array of boolean);
begin
  for var i:=0 to arr1.Length-1 do 
    arr1[i].CopyTo(arr2[i],0);
end;

procedure DrawCell(i,j: integer);
begin
  if BrushColor<>FillColor then begin
    SetBrushColor(FillColor);
    SetPenColor(FillColor);
  end;
  FillRect(x0+(j-1)*w,y0+(i-1)*w,x0+j*w-w1,y0+i*w-w1);
end;


procedure ClearCell(i,j: integer);
begin
  if BrushColor<>clearColor then begin
    SetBrushColor(clearColor);
    SetPenColor(clearColor);
  end;
  FillRect(x0+(j-1)*w,y0+(i-1)*w,x0+j*w-w1,y0+i*w-w1);
end;

procedure DrawConfiguration;
begin
  for var i:=1 to n do
    for var j:=1 to m do
      if a[i,j]=1 then
        DrawCell(i,j)
end;

procedure DrawField;
begin
  SetBrushColor(ClearColor);
  FillRectangle(x0,y0,x0+m*w,y0+n*w);
  SetPenColor(FiledColor);
  for var i:=0 to m do
    Line(x0+i*w-1,y0,x0+i*w-1,y0+n*w);
  for var i:=0 to n do
    Line(x0,y0+i*w-1,x0+m*w,y0+i*w-1);
  SetPenColor(FiledColor2);
  for var i:=0 to m div hm do
    Line(x0+i*w*hm-1,y0,x0+i*w*hm-1,y0+n*w);
  for var i:=0 to n div hn do
    Line(x0,y0+i*w*hn-1,x0+m*w,y0+i*w*hn-1);   
end;

procedure IncSosedi(i,j: integer);
begin
  var i1 := i=1 ? n : i-1;
  var i2 := i=n ? 1 : i+1;
  var j1 := j=1 ? m : j-1;
  var j2 := j=m ? 1 : j+1;
  SosediB[i1,j1] += 1;
  SosediB[i1,j]  += 1;
  SosediB[i1,j2] += 1;
  SosediB[i,j1]  += 1;
  SosediB[i,j2]  += 1;
  SosediB[i2,j1] += 1;
  SosediB[i2,j]  += 1;
  SosediB[i2,j2] += 1;
end;

procedure DecSosedi(i,j: integer);
begin
  var i1 := i=1 ? n : i-1;
  var i2 := i=n ? 1 : i+1;
  var j1 := j=1 ? m : j-1;
  var j2 := j=m ? 1 : j+1;
  SosediB[i1,j1] -= 1;
  SosediB[i1,j]  -= 1;
  SosediB[i1,j2] -= 1;
  SosediB[i,j1]  -= 1;
  SosediB[i,j2]  -= 1;
  SosediB[i2,j1] -= 1;
  SosediB[i2,j]  -= 1;
  SosediB[i2,j2] -= 1;
end;

procedure SetCell(i,j: integer);
begin
  if b[i,j]=0 then
  begin
    b[i,j]:=1;
    obn:=true;
    IncSosedi(i,j);
  end;
  count += 1;
end;

procedure UnSetCell(i,j: integer);
begin
  if b[i,j]=1 then
  begin
    b[i,j]:=0;
    obn:=true;
    DecSosedi(i,j);
  end;
  count -= 1;
end;

type
  ColonyType = (Big, LD, RD, LU, RU);
procedure AddColonyType(xc,yc:integer; ctype:ColonyType);
begin
 case ctype of
    ColonyType.Big:begin
      SetCell(xc,yc);
      SetCell(xc,yc+1);
      SetCell(xc,yc+2);
      SetCell(xc-1,yc+2);
      SetCell(xc+1,yc+1);
    end;
    ColonyType.LD:begin
      SetCell(xc,yc-1);
      SetCell(xc,yc);
      SetCell(xc,yc+1);
      SetCell(xc-1,yc-1);
      SetCell(xc-2,yc);      
    end;
  end;
  //SosediA:=SosediB;
  CopyArray(sosedib,sosedia);
  for var ik:=1 to nk do
    for var jk:=1 to mk do
      obnovB[ik,jk]:=true;
  //obnovA:=obnovB;
  CopyArray(obnovB,obnovA);
end;

procedure Init;
begin
  Count:=0;
  AddColonyType(n div 2,m div 2, ColonyType.Big);
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
      count -= 1;
    end;
3: if b[i,j]=0 then
    begin
      b[i,j]:=1;
      obn:=true;
      IncSosedi(i,j);
      DrawCell(i,j);
      count += 1;
    end;
  end;
end;

procedure NextGen;
var
  i,j,ik1,jk1,ik2,jk2,ifirst,jfirst,ilast,jlast: integer;
  l,r,u,d,lu,ld,ru,rd: boolean;
begin
    for var ik:=1 to nk do
    begin
      for var jk:=1 to mk do
      begin
        obn := false;
        ifirst := (ik-1)*hn+1;
        ilast  := ik*hn;
        jfirst := (jk-1)*hm+1;
        jlast  := jk*hm;
        if obnovA[ik,jk] then
        begin
          for i:=ifirst to ilast do
            for j:=jfirst to jlast do
              OnlyCase(i,j);
        end
        else
        begin
          ik1 := ik=1 ? nk : ik-1;
          ik2 := ik=nk ? 1 : ik+1;
          jk1 := jk=1 ? mk : jk-1;
          jk2 := jk=mk ? 1 : jk+1;
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

procedure MouseDown(x,y,b:integer);
begin
  case b of
    1:AddColonyType((y-y0)div w,(x-x0)div w, ColonyType.LD);
    
  end;
end;

begin
  SetConsoleIO;
  AssignArray(obnovA,nk+1,mk+1);
  AssignArray(obnovB,nk+1,mk+1);
  AssignArray(a,nn+1,mm+1);
  AssignArray(b,nn+1,mm+1);
  AssignArray(sosedia,nn+1,mm+1);
  AssignArray(sosedib,nn+1,mm+1);
  SetWindowCaption('Игра "Жизнь"');
  if (m mod mk<>0) or (n mod nk<>0) then
  begin
    writeln('Размер кластера не согласован с размером поля. Программа завершена');
    exit
  end;
  hm:=m div mk;
  hn:=n div nk;
  SetBrushColor(ClearColor);
  SetWindowSize(x0+m*w,y0+n*w+graphW);
  CenterWindow;
  ClearWindow(ClearColor);
  SetFontName('Courier New');
  SetFontSize(10);
  Init;
  DrawField;
  DrawConfiguration;
  OnMouseDown:=MouseDown;
  mil:=Milliseconds;
  var gen:=0;
  DrawInBuffer := false;
  while true do begin
    gen+=1;
    //SosediA:=SosediB;
    //obnovA:=obnovB;
    CopyArray(sosedib,sosedia);
    CopyArray(obnovB,obnovA);
    NextGen;    
    if gen mod 10 = 0 then begin
      DrawInBuffer := True;
      SetBrushColor(ClearColor);
      SetFontColor(FillColor);
      TextOut(25, 0,'Поколение: '+IntToStr(gen));
      TextOut(765,0,'Жителей: '+IntToStr(count)+'    ');
      if gen mod 1000 = 0 then begin
        mil1:=Milliseconds;
        writeln(gen,'  ',(mil1-mil)/1000);
        mil:=mil1;
      end;
      DrawInBuffer := false;
    end;
  end;
end.
