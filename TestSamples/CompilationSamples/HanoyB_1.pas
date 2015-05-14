uses GraphABC,Events;

type IntArr = array [0..10] of integer;

var
    x,y,c,d,h,n: integer;
    pw,x0,y0,f1,f2: integer;
    pc,bc,fc: Color;
    mc: array [0..9] of color;
    m: IntArr;
    s: string;

procedure   Segment;
begin
  SetPenWidth(pw);
  SetPenColor(pc);
  SetBrushColor(bc);
  SetFontColor(fc);
  RoundRect(x,y,x+320-30*c,y+38,10,10);
  TextOut(x+5,y+5,intToStr(c+1));
end;

procedure  Messag(xm,ym: integer; s: string);
begin
  SetBrushColor(clSilver);
  SetFontColor(fc);
  TextOut(xm,ym,s);
end;

procedure  HideMessag(xm,ym: integer);
begin
  SetBrushColor(clSilver);
  SetPenColor(clSilver);
  Rectangle(xm,ym,xm+400,ym+25);
end;
    
procedure   NewGame;
var
  k,i,xm,ym: integer;
  s: string;
begin
  SetFontSize(16);
  SetFontStyle(fsBold);

  SetBrushColor(clSilver);
  Rectangle(0,0,1025,710);

  SetPenWidth(3);
  SetPenColor(clBrown);
  SetBrushColor(clLime);
  Rectangle(0,120,1025,590);

  pw:=1;
  x:=20;y:=540;c:=0;s:='I';
  bc:=clSilver;fc:=clSilver;
  for i:= 1 to 3 do
  begin
    Segment;
    SetFontColor(clBlack);
    TextOut(x+150,y+7,s);
    x:=x+340;s:=s+'I';
  end;
   k:=9;
   pc:=clBlack;
  for i:=0 to k do
  begin
    c:=i; bc:=mc[i];
   x:=20+15*c;y:=500-40*c;
    fc:=clBlack;Segment;
  end;
  f1:=0;d:=0;h:=0;
  xm:=300;ym:=20;
  fc:=clGreen;
  s:='Показать ПРИМЕР  перемещения ?';
  Messag(xm,ym,s);

  SetBrushColor(clSilver);
  SetFontColor(clGreen);
  TextOut(120,600,'Вам  необходимо  переместить  БАШНЮ  с  позиции   I   на  позицию   III.');
  TextOut(80,620,'Перемещать можно только по одному сегменту,используя дополнительную позицию  II.');
  SetFontColor(clRed);
  TextOut(130,640,'Нельзя  устанавливать  сегмент  большего  размера  на  меньший.');
  SetFontColor(clBrown);
  TextOut(100,660,'Выберите  мышью  перемещаемый  сегмент, а  затем  место  для  перемещения.');
  SetFontColor(clGray);
  SetFontSize(8);
  TextOut(600,685,'Разработка -  Владимир ДЕГТЯРЬ               PascalABC               2007 г.');
  SetFontColor(clBlue);
  TextOut(50,685,'E-mail: degvv@mail.ru');
  SetFontSize(16);
end;

procedure  Tablo;
begin
  Inc(d);
  SetBrushColor(clSilver);
  SetFontColor(clBlue);
  TextOut(10,70,'КОЛИЧЕСТВО  ХОДОВ -  '+intToStr(d div 2));
  TextOut(750,70,'КОЛИЧЕСТВО ОШИБОК - '+intToStr(h));
end;

procedure  Show;
var
  i,j,r: integer;
  xs1,ys1,xs2,ys2: integer;
  fr: text;
begin
  SetPenColor(clLime);
  SetBrushColor(clLime);
  Rectangle(70,140,300,378);d:=0;
  for i:=1 to 15 do
  begin
    assign(fr,'Show.txt');
    reset(fr);
    for j:=0 to ((5*i)-1) do
    begin
      read(fr,r);
      case j of
       0,5,10,15,20,25,30,35,40,45,50,55,60,65,70: c:=r;
       1,6,11,16,21,26,31,36,41,46,51,56,61,66,71: xs1:=r;
       2,7,12,17,22,27,32,37,42,47,52,57,62,67,72: ys1:=r;
       3,8,13,18,23,28,33,38,43,48,53,58,63,68,73: xs2:=r;
       4,9,14,19,24,29,34,39,44,49,54,59,64,69,74: ys2:=r;
      end;
    end;
    close(fr);
    pw:=5;pc:=clRed;x:=xs1+15*c;y:=ys1;bc:=mc[c];Segment;Sleep(500);
    pc:=clLime;bc:=clLime;fc:=bc;Segment;
    pw:=1;pc:=clBlack;fc:=pc;bc:=mc[c];x:=xs2+15*c;y:=ys2;Segment;Sleep(500);
    d:=d+1;Tablo;
  end;
  NewGame;Sleep(1000);
end;

procedure  MyMouse;
var
 xm,ym: integer;
 s: string;
begin
  if f2 = 0 then
  begin
    if n = 0 then exit;
    c:=m[n];pw:=5;pc:=clRed;x:=x0+15*c;y:=y0;bc:=mc[c];Segment;
    f2:=1;y0:=y0+40;Dec(n);
  end
  else
  begin
    if c < m[n] then
    begin
      xm:=300;ym:=40;
      fc:=clRed;
      s:='Такое  перемещение  НЕДОПУСТИМО !';Inc(h);
      Messag(xm,ym,s);Sleep(2000);HideMessag(xm,ym);exit;
    end;
    pc:=clLime;bc:=clLime;fc:=bc;Segment;
    pw:=1;pc:=clBlack;fc:=pc;bc:=mc[c];x:=x0+15*c;y:=y0-40;Segment;
    Inc(n);f2:=0;m[n]:=c;y0:=y0-40;
  end;
end;

procedure    MouseDown(xt,yt,ms: integer);
var
  i,n1,n2,n3,x1,y1,x2,y2,x3,y3,xm,ym: integer;
  m1,m2,m3: IntArr;
  s: string;
begin
  xm:=300;ym:=20;
  HideMessag(xm,ym);
  xm:=400;ym:=70;
  fc:=clBrown;
  s:='НОВАЯ   ИГРА';
  Messag(xm,ym,s); fc:=clBlack;
  
   if f1= 0 then
  begin
    x1:=20;y1:=140;x2:=360;y2:=540;x3:=700;y3:=540;
    n1:=10;n2:=1;n3:=1;
    for i:=0 to 9 do
    begin
      m1[i+1]:=i;m2[i+1]:=0;m3[i+1]:=0;
    end;
    if (((xt>300) and (xt<700)) and ((yt>20) and (yt<50))) then Show;
  end;
  if f2 = 0 then
  begin
    if (((xt>350) and (xt<750)) and ((yt>70) and (yt<100))) then NewGame;
  end;
    if((yt<120) or (yt>550)) then d:=d-1;

  if (((xt>20) and (xt<340)) and ((yt>140) and (yt<540))) then
  begin
    if f1 = 1 then exit;
    f1:=1;m:=m1;n:=n1;x0:=x1;y0:=y1;
    MyMouse;
    m1:=m;n1:=n;x1:=x0;y1:=y0;
   end;

  if (((xt>360) and (xt<680)) and ((yt>140) and (yt<540))) then
  begin
    if f1 = 2 then exit;
    f1:=2;m:=m2;n:=n2;x0:=x2;y0:=y2;
    MyMouse;
    m2:=m;n2:=n;x2:=x0;y2:=y0;
  end;

  if (((xt>700) and (xt<1020)) and ((yt>140) and (yt<540))) then
  begin
    if f1 = 3 then exit;
    f1:=3;m:=m3;n:=n3;x0:=x3;y0:=y3;
    MyMouse;
    m3:=m;n3:=n;x3:=x0;y3:=y0;
  end;
  Tablo;
end;

begin
  SetWindowSize(1050,750);
  SetWindowCaption('        Логическаая  головоломка                 << ХАНОЙСКАЯ   БАШНЯ >>                      V 1.2');

     mc[0]:= clRed;
     mc[1]:= clGreen;
     mc[2]:= clYellow;
     mc[3]:= clTeal;
     mc[4]:= clBrown;
     mc[5]:= clGreen;
     mc[6]:= clPurple;
     mc[7]:= clSkyBlue;
     mc[8]:= clBlue;
     mc[9]:= clDarkGray;

   NewGame;
  
  OnMouseDown:=MouseDown;

end.
